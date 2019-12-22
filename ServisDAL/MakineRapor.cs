using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class MakineRapor
    {
        radiusEntities dc;

        int makineid;
        public MakineRapor(radiusEntities dc, int makineid)
        {
            this.dc = dc;
            this.makineid = makineid;

        }
        public MakineAnaliz get(string bas, string son)
        {
            MakineAnaliz a = new MakineAnaliz();
            TekMakine t = new TekMakine(dc, makineid, bas, son);
            MakineInfo m = t.servis();
            a.adi = m.genel.adi;
            a.tarih_araligi = bas + "-" + son;
            a.plaka = m.genel.plaka;
            a.aciklama = m.genel.aciklama;
            a.genel = m.genel;
            a.sayaclar = m.sayaclar;
            a.kararlar = m.kararlar;
            a.girisler = m.girisler;
            a.teorikler = m.teorikler;

            decimal toplam_calisma_saat = 0;
            decimal toplam_calisma_gun = 0;
            decimal toplam_calisma_hafta = 0;
            decimal toplam_calisma_ay = 0;
            decimal toplam_gelir = 0;
            int toplam_dakika = 0;
            string net_sure = "";
            if (m.kararlar.Count > 0)
            {
                toplam_calisma_saat = m.kararlar.Where(x => x.tarifekodu == "saat" || x.tarifetipi == "saat").Sum(x => x.calisma_saati);
                toplam_dakika = m.kararlar.Where(x => x.tarifekodu == "saat" || x.tarifetipi == "saat").Sum(x => x.dakika);
                toplam_calisma_gun = m.kararlar.Where(x => x.tarifekodu == "gun" || x.tarifetipi == "gun").Sum(x => x.calisma_saati);
                toplam_calisma_hafta = m.kararlar.Where(x => x.tarifekodu == "hafta" || x.tarifetipi == "hafta").Sum(x => x.calisma_saati);
                toplam_calisma_ay = m.kararlar.Where(x => x.tarifekodu == "ay" || x.tarifetipi == "ay").Sum(x => x.calisma_saati);
                toplam_gelir = m.kararlar.Sum(x => x.yekun);
            }


            TimeSpan ts = TimeSpan.FromMinutes(toplam_dakika);
            int gun = ts.Days;
            int toplam_saat = gun * 24 + ts.Hours;
            net_sure = toplam_saat + " saat " + ts.Minutes + " dakika";

            decimal toplam_masraf_teorik = 0;
            decimal toplam_masraf_gercek = 0;
            if (m.teorikler.Count > 0)
            {
                toplam_masraf_teorik = m.teorikler.Sum(x => x.tutar);
            }

            if (m.girisler.Count > 0)
            {
                toplam_masraf_gercek = m.girisler.Sum(x => x.tutar);
            }
            a.toplam_calisma_ay = toplam_calisma_ay;
            a.toplam_calisma_gun = toplam_calisma_gun;
            a.toplam_calisma_hafta = toplam_calisma_hafta;
            a.toplam_calisma_saat = toplam_calisma_saat;
            a.toplam_masraf_gercek = toplam_masraf_gercek;
            a.toplam_masraf_teorik = toplam_masraf_teorik;
            a.toplam_gelir = toplam_gelir;
            a.toplam_dakika = toplam_dakika;
            a.net_sure = net_sure;
            a.tarih_araligi = bas + " - " + son;
            return a;
        }
    }
    public class MakineAnaliz
    {
        public string adi { get; set; }
        public string plaka { get; set; }
        public string aciklama { get; set; }
        public decimal toplam_calisma_saat { get; set; }
        public decimal toplam_calisma_gun { get; set; }
        public decimal toplam_calisma_hafta { get; set; }
        public decimal toplam_calisma_ay { get; set; }
        public decimal toplam_masraf_teorik { get; set; }
        public decimal toplam_masraf_gercek { get; set; }
        public decimal toplam_gelir { get; set; }
        public string tarih_araligi { get; set; }
        public string net_sure { get; set; }
        public int toplam_dakika { get; set; }
        public List<MakineCalismalari> kararlar { get; set; }
        public List<MakineGiris> girisler { get; set; }
        public List<MasrafHesap> teorikler { get; set; }
        public Makine genel { get; set; }
        public List<ServisSayac> sayaclar { get; set; }
    }
}
