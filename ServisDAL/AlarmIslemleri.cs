using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class AlarmIslemleri
    {
        private radiusEntities dc;
        public AlarmIslemleri(radiusEntities dc)
        {
            this.dc = dc;
        }
        public List<alarm> AlarmListesi()
        {
            var serbestler = (from s in dc.servicehesap_ops
                              where s.iptal == false && s.onay == false
                              select new alarm
                              {
                                  id = 0,
                                  tarih = DateTime.Now,
                                  mesaj = "Operatör: " + s.kullanici + " operatörü yeni bir çalışma ekledi",
                                  baglanti = "/TeknikMakine/SerbestOperatorCalismalari",
                                  okundu = false
                              }).ToList();

            var sayaclar = (from a in dc.makine_servis_sayacs
                            where a.iptal != true && a.alarm_sayac >= a.sayac
                            select new alarm
                            {
                                id = 0,
                                tarih = DateTime.Now,
                                mesaj = "Servis zamanı: " + a.makine_caris.adi + " makinesinin " + a.makine_masrafs.adi + " değişim zamanı gelmek üzere ",
                                baglanti = "/TeknikMakine/MakineTek?makineid=" + a.makine_id.ToString(),
                                okundu = false
                            }).ToList();

            return serbestler.Union(sayaclar).ToList();

        }
        public int AlarmCount()
        {
            int serbestler = (from s in dc.servicehesap_ops
                              where s.iptal == false && s.onay == false
                              select s).Count();

            int sayaclar = (from a in dc.makine_servis_sayacs
                            where a.iptal != true && a.alarm_sayac >= a.sayac
                            select a).Count();

            return serbestler + sayaclar;
        }




    }

    public class alarm
    {
        public int id { get; set; }
        public string mesaj { get; set; }
        public System.DateTime tarih { get; set; }
        public string baglanti { get; set; }
        public Nullable<bool> okundu { get; set; }
    }

}
