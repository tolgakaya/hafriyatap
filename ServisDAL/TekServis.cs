using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;
using ServisDAL.Repo;

namespace ServisDAL
{
    public class TekServis
    {
        radiusEntities dc;
        int servisid;
        string kimlik;
        public TekServis(radiusEntities dc, int servisid, string kimlik)
        {
            this.servisid = servisid;
            this.kimlik = kimlik;
            this.dc = dc;
        }

        public ServisInfo servis()
        {
            ServisInfo s = new ServisInfo();
            List<ServisHesapRepo> kar = this.kararlar();
            s.kararlar = kar;
            ServisRepo g = this.genel();
            s.genel = g;
            s.genel = this.genel();
            s.detaylar = this.detaylar();
            s.atamalar = this.atamalar();
            DateTime son = g.kapanmaZamani == null ? DateTime.Now : (DateTime)g.kapanmaZamani;
            s.girisler = this.girisler(g.acilmaZamani, son);
            s.teorikler = this.teorikler();
            return s;
        }
        private ServisRepo genel()
        {
            if (String.IsNullOrEmpty(kimlik))
            {
                return (from s in dc.services
                        where s.ServiceID == servisid && s.iptal == false
                        select new ServisRepo
                        {
                            serviceID = s.ServiceID,
                            musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                            adres = s.CustID == null ? "-" : s.customer.Adres,
                            telefon = s.CustID == null ? "-" : s.customer.telefon,
                            aciklama = s.Aciklama,
                            acilmaZamani = s.AcilmaZamani,
                            kapanmaZamani = s.KapanmaZamani,
                            custID = s.CustID == null ? -99 : (int)s.CustID,

                            baslik = s.Baslik,

                            css = s.service_tips.css,
                            sonGorevliID = s.SonAtananID,
                            son_dis_servis = s.son_dis_servis,
                            usta = s.usta,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                        }).FirstOrDefault<ServisRepo>();
            }
            else
            {
                return (from s in dc.services
                        where s.Servis_Kimlik_No == kimlik && s.iptal == false
                        select new ServisRepo
                        {
                            serviceID = s.ServiceID,
                            musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                            adres = s.CustID == null ? "-" : s.customer.Adres,
                            telefon = s.CustID == null ? "-" : s.customer.telefon,
                            aciklama = s.Aciklama,
                            acilmaZamani = s.AcilmaZamani,
                            kapanmaZamani = s.KapanmaZamani,
                            custID = s.CustID == null ? -99 : (int)s.CustID,

                            baslik = s.Baslik,

                            css = s.service_tips.css,
                            sonGorevliID = s.SonAtananID,
                            son_dis_servis = s.son_dis_servis,
                            usta = s.usta,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                        }).FirstOrDefault<ServisRepo>();
            }


        }
        private List<ServisHesapRepo> kararlar()
        {
            if (String.IsNullOrEmpty(kimlik))
            {
                return (from s in dc.servicehesaps
                        from m in dc.service_maliyets
                        where s.ServiceID == servisid && s.iptal == false && s.HesapID == m.hesapid
                        orderby s.TarihZaman descending
                        select new ServisHesapRepo
                        {
                            hesapID = s.HesapID,
                            aciklama = s.Aciklama,
                            islemParca = s.IslemParca,
                            kdv = s.KDV,
                            makine_id = s.makine_id,
                            disServis = s.tamirci_id == null ? "-" : s.customer1.Ad,
                            onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                            dakika = s.makine_id == null ? (s.adet.ToString() + " " + s.birim) : (s.dakika.ToString() + " dakika"),
                            sure = s.calisma_saati,
                            tarife = s.tarife_kodu,
                            onaylimi = s.onay,
                            onayTarih = s.Onay_tarih,
                            tarihZaman = s.TarihZaman,
                            servisID = s.ServiceID,
                            tutar = s.Tutar,
                            yekun = (decimal)s.Yekun,
                            cihaz = s.cihaz_adi,
                            birim_maliyet = s.birim_maliyet,
                            toplam_maliyet = s.toplam_maliyet + m.tutar,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                        }).ToList();
            }
            else
            {
                return (from s in dc.servicehesaps
                        from m in dc.service_maliyets
                        where s.service.Servis_Kimlik_No == kimlik && s.iptal == false && s.HesapID == m.hesapid
                        orderby s.TarihZaman descending
                        select new ServisHesapRepo
                        {
                            hesapID = s.HesapID,
                            aciklama = s.Aciklama,
                            islemParca = s.IslemParca,
                            kdv = s.KDV,
                            disServis = s.tamirci_id == null ? "-" : s.customer1.Ad,
                            makine_id = s.makine_id,
                            onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                            dakika = s.makine_id == null ? (s.adet.ToString() + " " + s.birim) : (s.dakika.ToString() + " dakika"),
                            sure = s.calisma_saati,
                            tarife = s.tarife_kodu,
                            onaylimi = s.onay,
                            onayTarih = s.Onay_tarih,
                            tarihZaman = s.TarihZaman,
                            servisID = s.ServiceID,
                            tutar = s.Tutar,
                            yekun = (decimal)s.Yekun,
                            cihaz = s.cihaz_adi,
                            birim_maliyet = s.birim_maliyet,
                            toplam_maliyet = s.toplam_maliyet + m.tutar,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                        }).ToList();
            }


        }
        private List<ServisDetayRepo> detaylar()
        {
            if (string.IsNullOrEmpty(kimlik))
            {
                return (from s in dc.servicedetays

                        where s.ServiceID == servisid
                        orderby s.DetayID descending
                        select new ServisDetayRepo
                        {
                            detayID = s.DetayID,
                            servisID = s.ServiceID,
                            tarihZaman = s.TarihZaman,
                            belgeYol = s.BelgeYol,
                            aciklama = s.Aciklama,
                            gorevliID = s.KullaniciID,
                            baslik = s.Baslik,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                        }).ToList();
            }
            else
            {
                return (from s in dc.servicedetays

                        where s.service.Servis_Kimlik_No == kimlik
                        orderby s.DetayID descending
                        select new ServisDetayRepo
                        {
                            detayID = s.DetayID,
                            servisID = s.ServiceID,
                            tarihZaman = s.TarihZaman,
                            belgeYol = s.BelgeYol,
                            aciklama = s.Aciklama,

                            gorevliID = s.KullaniciID,
                            baslik = s.Baslik,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                        }).ToList();
            }

        }
        private List<MasrafHesap> teorikler()
        {
            if (String.IsNullOrEmpty(kimlik))
            {

                return (from m in dc.makine_masraf_hesaps2
                        where m.iptal == false && m.serviceid == servisid
                        select new MasrafHesap
                        {
                            id = m.id,
                            makine_plaka = m.makine_caris.adi + m.makine_caris.plaka,
                            masraf = m.makine_masrafs.adi,
                            miktar = m.miktar,
                            tutar = m.tutar,
                            tarih = m.tarih
                        }).ToList();

            }
            else
            {

                return (from m in dc.makine_masraf_hesaps2
                        where m.iptal == false && m.service.Servis_Kimlik_No == kimlik
                        select new MasrafHesap
                        {
                            id = m.id,
                            makine_plaka = m.makine_caris.adi + m.makine_caris.plaka,
                            masraf = m.makine_masrafs.adi,
                            miktar = m.miktar,
                            tutar = m.tutar,
                            tarih = m.tarih
                        }).ToList();


            }

        }
      
        private List<MakineGiris> girisler(DateTime bas, DateTime son)
        {
            //girişleri bulabilmek için
            //burada çalışmış makinelere
            //ve onaylanmış makine hesaplarının tarih aralığına bakmak lazım

            var hesaplar = (from h in dc.servicehesaps
                            where h.service.Servis_Kimlik_No == kimlik && h.iptal == false && h.makine_id != null
                            select h.makine_id).Distinct();

            return (from m in dc.makine_masraf_girislers
                    from k in hesaplar
                    where  m.makine_id == k  && m.iptal == false && m.tarih >= bas && m.tarih <= son
                    select new MakineGiris
                    {
                        id = m.id,
                        makine_id = m.makine_id,
                        makine_adi = m.makine_caris.adi,
                        makine_plaka = m.makine_caris.plaka,
                        masraf_id = m.masraf_id,
                        masraf_adi = m.makine_masrafs.adi,
                        miktar = m.miktar,
                        tutar = m.tutar,
                        aciklama = m.aciklama,
                        tarih = m.tarih

                    }).ToList();


        }

        private List<ServisAtamalari> atamalar()
        {

            if (String.IsNullOrEmpty(kimlik))
            {
                return (from m in dc.servis_kullanicis
                        where m.iptal == false && m.servis_id == servisid
                        select new ServisAtamalari
                        {
                            id = m.id,
                            servis_id = m.servis_id,
                            kullanici = m.kullanici,
                            atama = m.atama,
                            cikarma = m.cikarma
                        }).ToList();
            }
            else
            {
                return (from m in dc.servis_kullanicis
                        where m.iptal == false && m.service.Servis_Kimlik_No == kimlik
                        select new ServisAtamalari
                        {
                            id = m.id,
                            servis_id = m.servis_id,
                            kullanici = m.kullanici,
                            atama = m.atama,
                            cikarma = m.cikarma
                        }).ToList();
            }


        }
    }

    public class ServisInfo
    {
        public ServisRepo genel { get; set; }
        public List<ServisHesapRepo> kararlar { get; set; }
        public List<ServisDetayRepo> detaylar { get; set; }
        public List<MakineGiris> girisler { get; set; }
        public List<MasrafHesap> teorikler { get; set; }
        public List<ServisAtamalari> atamalar { get; set; }

    }
    public class ServisAtamalari
    {
        public int id { get; set; }
        public int servis_id { get; set; }
        public string kullanici { get; set; }
        public System.DateTime atama { get; set; }
        public DateTime? cikarma { get; set; }

    }
}
