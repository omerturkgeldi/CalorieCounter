using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Core.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime OlusturulanTarih { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }
        public bool SilindiMi { get; set; }
        public string EkleyenKullaniciFk { get; set; }
        public string SonGuncellemeYapanKullaniciFk { get; set; }

    }
}
