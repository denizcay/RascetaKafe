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
    public partial class UrunlerForm : Form
    {
        private readonly KafeVeri _db;
        private readonly BindingList<Urunler> _blUrunler;
        public UrunlerForm(KafeVeri db)
        {
            _db = db;
            _blUrunler = new BindingList<Urunler>(_db.Urunler);
            InitializeComponent();
            UrunleriListele();
        }

        private void UrunleriListele()
        {
            dgvUrunler.DataSource = _blUrunler;
        }
        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            
        }

        private void EkleFormuSifirla()
        {
            txtUrun.Clear();
            nudFiyat.Value = 0;
            btnUrunEkle.Text = "EKLE";
            btnIptal.Hide();
            _duzenlenen = null;
        }
              

        Urunler _duzenlenen;      

        private void btnUrunEkle_Click_1(object sender, EventArgs e)
        {
            string urunAd = txtUrun.Text.Trim();
            if (urunAd == "")
            {
                MessageBox.Show("Lütfen bir ürün adı giriniz.");
                return;
            }
            if (_duzenlenen == null) //duzenlenen yoksa ekle
            {
                _blUrunler.Add(new Urunler()
                {
                    UrunAd = urunAd,
                    BirimFiyat = nudFiyat.Value
                });
            }
            else // duzenlenen varsa guncelle
            {
                _duzenlenen.UrunAd = urunAd;
                _duzenlenen.BirimFiyat = nudFiyat.Value;
                _blUrunler.ResetBindings();
            }
            EkleFormuSifirla();
        }

        private void btnIptal_Click_1(object sender, EventArgs e)
        {
            EkleFormuSifirla();
        }

        private void btnGuncelle_Click_1(object sender, EventArgs e)
        {
            if (dgvUrunler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Ürün güncellemek için önce ürün seçmelisiniz.");
                return;
            }
            _duzenlenen = (Urunler)dgvUrunler.SelectedRows[0].DataBoundItem;
            txtUrun.Text = _duzenlenen.UrunAd;
            nudFiyat.Value = _duzenlenen.BirimFiyat;
            btnUrunEkle.Text = "KAYDET";
            btnIptal.Show();
        }

        private void dgvUrunler_KeyDown_1(object sender, KeyEventArgs e)
        {
        if (e.KeyCode == Keys.Delete && dgvUrunler.SelectedRows.Count > 0)
        {
            DialogResult dr = MessageBox.Show(
                "Seçili ürün silinecektir. Onaylıyor musunuz?",
                "Ürün sılme onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                //secili satira bagli urunu al
                Urunler urun = (Urunler)dgvUrunler.SelectedRows[0].DataBoundItem;
                _blUrunler.Remove(urun);
            }
        }
        }
    }
}
