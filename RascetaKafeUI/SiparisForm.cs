using RascetaKafe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RascetaKafe.UI
{
    public partial class SiparisForm : Form
    {
        public event EventHandler<MasaTasindiEventArgs> MasaTasindi;

        private readonly KafeVeri _db;
        private readonly Siparis _siparis;
        private readonly BindingList<SiparisDetay> _blSiparisDetaylar;
        public SiparisForm(KafeVeri kafeVeri, Siparis siparis)
        {
            _db = kafeVeri;
            _siparis = siparis;
            _blSiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);
            InitializeComponent();
            dgvSiparis.AutoGenerateColumns = false; // otomatik sutun olusturmayi kapatmak icin 
            UrunleriGoster();
            EkleFormSifirla();
            MasaNoGuncelle();
            FiyatGuncelle();
            DetaylariListele();
            MasaNolariDoldur();
            _blSiparisDetaylar.ListChanged += _blSiparisDetaylar_ListChanged;
        }
        private void MasaNolariDoldur()
        {
            List<int> bosMasaNolar = new List<int>();
            for (int i = 1; i <= _db.MasaAdet; i++)
            {
                if (!_db.AktifSiparisler.Any(x => x.MasaNo == i)) //aktif siparislerde i masanosuna sah'p siparis var DEGILSE/yoksa
                    bosMasaNolar.Add(i);
            }
            cbMasaNo.DataSource = bosMasaNolar;

            // KISA YOLU:
            //cbMasaNo.DataSource = Enumerable.Range(1, 20).Where(i=>!_db.AktifSiparisler.Any(x=>x.MasaNo==i)).ToList();
        }

        private void _blSiparisDetaylar_ListChanged(object sender, ListChangedEventArgs e)
        {
            FiyatGuncelle();
        }

        private void UrunleriGoster()
        {
            cbUrun.DataSource = _db.Urunler;
        }

        private void FiyatGuncelle()
        {
            lblTutar.Text = _siparis.ToplamTutarTL;

        }

        private void MasaNoGuncelle()
        {
            Text = $"Masa {_siparis.MasaNo} Siparis Bilgileri";
            lblMasaNo.Text = _siparis.MasaNo.ToString("00");
        }



        private void EkleFormSifirla()
        {
            cbUrun.SelectedIndex = -1; // urunleri secmesi gerekir eger son urun kalsin istersen bu satiri yoruma al
            nudAdet.Value = 1;
        }

        private void DetaylariListele()
        {
            dgvSiparis.DataSource = _blSiparisDetaylar;
        }

        private void btnEkle_Click_1(object sender, EventArgs e)
        {
            if (cbUrun.SelectedIndex == -1 || nudAdet.Value < 1)
                return; //secili urun yok, metottan cik
            Urunler urun = (Urunler)cbUrun.SelectedItem;


            SiparisDetay siparisDetay = new SiparisDetay()
            {
                UrunAd = urun.UrunAd,
                BirimFiyat = urun.BirimFiyat,
                Adet = (int)nudAdet.Value

            };

            _blSiparisDetaylar.Add(siparisDetay);
            EkleFormSifirla();
        }

        private void btnTasi_Click_1(object sender, EventArgs e)
        {
            if (cbMasaNo.SelectedIndex == -1) return;
            int eskiMasaNo = _siparis.MasaNo;
            int yeniMasaNo = (int)cbMasaNo.SelectedItem;
            _siparis.MasaNo = yeniMasaNo;
            MasaNolariDoldur(); // dolu masalar degisti

            if (MasaTasindi != null)
            {
                MasaTasindi(this, new MasaTasindiEventArgs(eskiMasaNo, yeniMasaNo));
            }
            MasaNoGuncelle();
        }

        private void btnOdeme_Click(object sender, EventArgs e)
        {
            SiparisKapat(SiparisDurum.Odendi, _siparis.ToplamTutar());
        }
        private void SiparisKapat(SiparisDurum siparisDurum, decimal odenenTutar)
        {
            _siparis.OdenenTutar = odenenTutar;
            _siparis.Durum = siparisDurum;
            _siparis.KapanisZamani = DateTime.Now;
            _db.AktifSiparisler.Remove(_siparis);
            _db.GecmisSiparisler.Add(_siparis);
            Close();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            SiparisKapat(SiparisDurum.Iptal, 0);
        }

        private void btnAnaSayfa_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvSiparis_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Secili siparis detaylari silinecektir. Emin misiniz?", //text
                "Silme", //caption
                MessageBoxButtons.YesNo, //buttons
                icon: MessageBoxIcon.Exclamation, //degerleri isimleriyle girebiliriz
                defaultButton: MessageBoxDefaultButton.Button2
                );
            e.Cancel = dr == DialogResult.No;
        }
    }
}
