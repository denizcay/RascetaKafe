using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RascetaKafe.Data
{
    public class KafeVeri
    {
        public int MasaAdet { get; set; } = 20;
        public List<Urunler> Urunler { get; set; } = new List<Urunler>();
        public List<Siparis> AktifSiparisler { get; set; } = new List<Siparis>();
        public List<Siparis> GecmisSiparisler { get; set; } = new List<Siparis>();
    }
}
