using ServisDAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class TekMakine
    {
        radiusEntities dc;

        int makineid;
        string basDates;
        string sonDates;
        public TekMakine(radiusEntities dc, int makineid, string basDates, string sonDates)
        {
            this.makineid = makineid;
            this.basDates = basDates;
            this.sonDates = sonDates;
            this.dc = dc;
        }

        public MakineInfo servis()
        {
            MakineInfo s = new MakineInfo();
            s.genel = this.genel();
            s.sayaclar = this.sayaclar();
            s.kararlar = this.calismalar();
            s.girisler = this.girisler();
            s.teorikler = this.teorikler();
            s.atamalar = this.atamalar();
            s.tanimlar = this.tanimlar();
            s.tarifeler = this.tarifeler();
            return s;
        }

        public List<ServisSayac> sayaclar()
        {
            return (from s in dc.makine_servis_sayacs
                    where s.iptal == false && s.makine_id == makineid
                    select new ServisSayac
                    {
                        sayac_id = s.sayac_id,
                        masraf_id = s.masraf_id,
                        masraf_adi = s.makine_masrafs.adi,
                        sayac = s.sayac
                    }).ToList();
            //sayaclar ayrı liste olarak gösterilecek
            //gider eklemede sayaci sıfırla diye bir buton konulduğunda 
            //sayaç sıfırlanacak
        }

        public List<MakineMasrafTanimi> tanimlar()
        {


            return (from m in dc.makine_masraf_tanims2
                    where m.iptal == false & m.makine_id == makineid
                    select new MakineMasrafTanimi
                    {
                        tanim_id = m.tanim_id,
                        makine_id = m.makine_id,
                        makine_plaka = m.makine_caris.adi + "-" + m.makine_caris.plaka,
                        masraf_id = m.masraf_id,
                        masraf = m.makine_masrafs.adi,
                        masraf_saat = m.masraf_saat,
                        birim = m.birim,
                        aciklama = m.aciklama,
                        calisma_tipli = true,
                        tarife_kodu = m.kiralama_tarifes.tarife_kodu + "-" + m.kiralama_tarifes.calisma_tipi,
                        tarifeid = m.tarifeid
                    }).ToList();


        }

        private Makine genel()
        {
            return (from m in dc.makine_caris
                    where m.iptal == false && m.makine_id == makineid
                    select new Makine
                    {
                        makine_id = m.makine_id,
                        adi = m.adi,
                        plaka = m.plaka,
                        aciklama = m.aciklama,
                        son_sayac = m.son_sayac,
                        tarife_ay = m.tarife_ay,
                        tarife_gun = m.tarife_gun,
                        tarife_hafta = m.tarife_hafta,
                        tarife_saat = m.tarife_saat,
                        toplam_calisma_saat = m.toplam_calisma_saat,
                        toplam_calisma_gun = m.toplam_calisma_gun,
                        toplam_calisma_hafta = m.toplam_calisma_hafta,
                        toplam_calisma_ay = m.toplam_calisma_ay,
                        toplam_masraf_teorik = m.toplam_masraf_teorik,
                        toplam_gelir = m.toplam_gelir,
                        servis_sayaci = m.servis_sayaci,
                        toplam_masraf_gercek = m.toplam_masraf_gercek
                    }).FirstOrDefault();

        }

        private List<MakineCalismalari> calismalar()
        {
            DateTime bas = DateTime.Now.AddYears(-10);
            DateTime son = DateTime.Now;
            if (!string.IsNullOrEmpty(basDates))
            {
                bas = DateTime.Parse(basDates);
            }

            if (!string.IsNullOrEmpty(sonDates))
            {
                son = DateTime.Parse(sonDates);
            }
            return (from s in dc.servicehesaps
                    where s.makine_id == makineid && s.iptal == false && s.TarihZaman >= bas && s.TarihZaman <= son
                    orderby s.TarihZaman descending
                    select new MakineCalismalari
                    {
                        hesapID = s.HesapID,
                        musteriAdi = s.customer.Ad,
                        islemParca = s.IslemParca,
                        baslangic_tarih = s.baslangic_tarih,
                        bitis_tarih = s.bitis_tarih,
                        baslangic = s.baslangic,
                        tarifekodu = s.tarife_kodu,
                        tarifetipi = s.tarife_tipi,
                        bitis = s.bitis,
                        dakika = s.dakika,
                        calisma_saati = s.calisma_saati,
                        toplam_maliyet = s.toplam_maliyet,
                        yekun = (decimal)s.Yekun,
                        aciklama = s.Aciklama,
                        tarihZaman = s.TarihZaman
                    }).ToList();

        }

        private List<MasrafHesap> teorikler()
        {
            DateTime bas = DateTime.Now.AddYears(-10);
            DateTime son = DateTime.Now;
            if (!string.IsNullOrEmpty(basDates))
            {
                bas = DateTime.Parse(basDates);
            }

            if (!string.IsNullOrEmpty(sonDates))
            {
                son = DateTime.Parse(sonDates);
            }


            return (from m in dc.makine_masraf_hesaps2
                    where m.iptal == false && m.makine_id == makineid && m.tarih >= bas && m.tarih <= son
                    select new MasrafHesap
                    {
                        id = m.id,
                        makine_plaka = m.makine_caris.adi + m.makine_caris.plaka,
                        masraf = m.makine_masrafs.adi,
                        miktar = m.miktar,
                        birim = m.birim,
                        tutar = m.tutar,
                        tarih = m.tarih
                    }).ToList();


        }
        private List<MakineGiris> girisler()
        {

            DateTime bas = DateTime.Now.AddYears(-10);
            DateTime son = DateTime.Now;
            if (!string.IsNullOrEmpty(basDates))
            {
                bas = DateTime.Parse(basDates);
            }

            if (!string.IsNullOrEmpty(sonDates))
            {
                son = DateTime.Parse(sonDates);
            }

            return (from m in dc.makine_masraf_girislers
                    where m.iptal == false && m.makine_id == makineid && m.tarih >= bas && m.tarih <= son
                    select new MakineGiris
                    {
                        id = m.id,
                        makine_id = m.makine_id,
                        makine_adi = m.makine_caris.adi,
                        makine_plaka = m.makine_caris.plaka,
                        masraf_id = m.masraf_id,
                        masraf_adi = m.makine_masrafs.adi,
                        miktar = m.miktar,
                        birim = m.birim,
                        tutar = m.tutar,
                        aciklama = m.aciklama,
                        tarih = m.tarih

                    }).ToList();
        }
        private List<Tarife> tarifeler()
        {
            return (from t in dc.kiralama_tarifes
                    where t.makine_id == makineid && t.iptal == false
                    select new Tarife
                    {
                        id = t.id,
                        ad = t.tarife_kodu + "-" + t.calisma_tipi,
                        tarife_kodu = t.tarife_kodu,
                        calisma_tipi = t.calisma_tipi,
                        tutar = t.tutar,
                        saatlik = t.saatlik
                    }).ToList();
        }
        private List<MakineOperatorleri> atamalar()
        {
            return (from m in dc.makine_kullanicis
                    where m.iptal == false && m.makine_id == makineid
                    select new MakineOperatorleri
                    {
                        id = m.id,
                        makine_id = m.makine_id,
                        atama = m.atama,
                        cikarma = m.cikarma,
                        kullanici = m.kullanici
                    }).ToList();
        }

    }
    public class ServisSayac
    {
        public int sayac_id { get; set; }
        public int masraf_id { get; set; }
        public string masraf_adi { get; set; }
        public decimal sayac { get; set; }
        public decimal alarm { get; set; }
    }

    public class MakineCalismalari
    {
        public int hesapID { get; set; }
        public string musteriAdi { get; set; }
        public string islemParca { get; set; }
        public decimal yekun { get; set; }
        public decimal? toplam_maliyet { get; set; }
        public string aciklama { get; set; }
        public string tarifekodu { get; set; }
        public string tarifetipi { get; set; }
        public decimal baslangic { get; set; }
        public decimal bitis { get; set; }
        public int dakika { get; set; }
        public DateTime baslangic_tarih { get; set; }
        public DateTime bitis_tarih { get; set; }
        public decimal calisma_saati { get; set; }
        public DateTime tarihZaman { get; set; }

    }

    public class MakineOperatorleri
    {
        public int id { get; set; }
        public int makine_id { get; set; }
        public string kullanici { get; set; }
        public System.DateTime atama { get; set; }
        public Nullable<System.DateTime> cikarma { get; set; }
    }
    public class MakineInfo
    {
        public Makine genel { get; set; }
        public List<ServisSayac> sayaclar { get; set; }
        public List<MakineCalismalari> kararlar { get; set; }
        public List<MakineGiris> girisler { get; set; }
        public List<MasrafHesap> teorikler { get; set; }
        public List<MakineMasrafTanimi> tanimlar { get; set; }
        public List<Tarife> tarifeler { get; set; }

        public List<MakineOperatorleri> atamalar { get; set; }

    }
}
