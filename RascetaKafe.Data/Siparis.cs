using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RascetaKafe.Data
{
    public class Siparis
    {
        public int MasaNo { get; set; }
        public SiparisDurum Durum { get; set; }
        public decimal OdenenTutar { get; set; }
        public DateTime? AcilisZamani { get; set; }
        public DateTime? KapanisZamani { get; set; }
        public List<SiparisDetay> SiparisDetaylar { get; set; } = new List<SiparisDetay>();
        public string ToplamTutarTL => ToplamTutar().ToString("₺0.00");

        public decimal ToplamTutar()
        {
            //decimal toplam = 0;
            //foreach (SiparisDetay tutar in SiparisDetaylar)
            //    toplam += tutar.Tutar();
            //return toplam;
            return SiparisDetaylar.Sum(x => x.Tutar());
        }
    }
}
