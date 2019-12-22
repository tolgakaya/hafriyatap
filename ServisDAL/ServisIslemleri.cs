using System;
using System.Collections.Generic;
using System.Linq;
using ServisDAL.Repo;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class sayfali
    {
        public List<ServisRepo> servis_listesi { get; set; }
        public int kayit_sayisi { get; set; }
    }
    public class maliyet
    {
        public List<ServisRepo> servis_listesi { get; set; }
        public int adet { get; set; }
        public decimal toplam_maliyet { get; set; }
        public decimal toplam_tutar { get; set; }
        public decimal toplam_fark { get; set; }
        public DateTime basTarih { get; set; }

        public string kisit { get; set; }
        public string firma { get; set; }
    }
    public class ServisIslemleri
    {

        radiusEntities db;

        public ServisIslemleri(radiusEntities db)
        {

            this.db = db;

        }

        #region servis arama ve listeleme işlemleri

        public TeknikServis.Radius.service servisTekR(int id)
        {
            return (from s in db.services
                    where s.ServiceID == id
                    select s).FirstOrDefault();

        }



        //tamirci carisine servis kapatıldığı zaman yansıma yapılıyor
        //tamirci kaydında prim oranı belirleniyor
        //servis kapatıldığı zaman service faturas kapandı olarak işaretleniyor
        //servis_faturas'dan triggerla usta_id nin carisine ustanın bilgilerine göre hesaplama yapılıp yazılıyor.

        public List<ServisRepo> servisTamirciRapor(int tamirci_id, bool? kapanma, DateTime? zaman = null)
        {


            DateTime sinir = zaman == null ? DateTime.Now.AddYears(-1) : (DateTime)zaman;
            if (kapanma != null)
            {
                return (from s in db.services
                        where s.usta_id == tamirci_id && (kapanma == false ? s.KapanmaZamani == null : s.KapanmaZamani != null) && s.iptal == false && s.AcilmaZamani > sinir
                        orderby s.AcilmaZamani descending
                        select new ServisRepo
                        {
                            serviceID = s.ServiceID,
                            custID = s.CustID == null ? -99 : (int)s.CustID,
                            musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                            adres = s.CustID == null ? "-" : s.customer.Adres,
                            telefon = s.CustID == null ? "-" : s.customer.telefon,
                            kullaniciID = s.olusturan_Kullanici,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                            sonGorevli = s.SonAtananID,
                            usta = s.usta,
                            usta_id = s.usta_id,
                            aciklama = s.Aciklama,
                            acilmaZamani = s.AcilmaZamani,
                            kapanmaZamani = s.KapanmaZamani,

                            kimlikNo = s.Servis_Kimlik_No,
                            tipID = s.tip_id,
                            css = s.service_tips.css,
                            servisTipi = s.service_tips.tip_ad,

                            baslik = s.Baslik,
                            tutar = s.service_faturas.Tutar,
                            yekun = s.service_faturas.Yekun,
                            maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                            fark = 0,
                            hesaplar = (from h in db.servicehesaps
                                        where h.ServiceID == s.ServiceID && h.iptal == false
                                        orderby h.TarihZaman descending
                                        select new ServisHesapRepo
                                        {
                                            hesapID = h.HesapID,
                                            aciklama = h.Aciklama,
                                            islemParca = h.IslemParca,
                                            kdv = h.KDV,
                                            onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                            onaylimi = h.onay,
                                            onayTarih = h.Onay_tarih,
                                            disServis = h.customer1.Ad,

                                            tarihZaman = h.TarihZaman,
                                            servisID = s.ServiceID,
                                            tutar = h.Tutar,
                                            yekun = (decimal)h.Yekun,
                                            cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                            birim_maliyet = h.birim_maliyet,
                                            toplam_maliyet = h.toplam_maliyet,
                                            kullanici = h.updated == null ? h.inserted : (h.inserted + "-" + h.updated)
                                        }).ToList()

                        }).ToList<ServisRepo>();
            }
            else
            {
                //hepsi
                return (from s in db.services
                        where s.usta_id == tamirci_id && s.iptal == false && s.AcilmaZamani > sinir
                        orderby s.AcilmaZamani descending
                        select new ServisRepo
                        {
                            serviceID = s.ServiceID,
                            custID = s.CustID == null ? -99 : (int)s.CustID,
                            musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                            adres = s.CustID == null ? "-" : s.customer.Adres,
                            telefon = s.CustID == null ? "-" : s.customer.telefon,
                            kullaniciID = s.olusturan_Kullanici,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                            sonGorevli = s.SonAtananID,
                            usta_id = s.usta_id,
                            usta = s.usta,
                            aciklama = s.Aciklama,
                            acilmaZamani = s.AcilmaZamani,
                            kapanmaZamani = s.KapanmaZamani,

                            kimlikNo = s.Servis_Kimlik_No,
                            tipID = s.tip_id,
                            css = s.service_tips.css,
                            servisTipi = s.service_tips.tip_ad,

                            baslik = s.Baslik,
                            tutar = s.service_faturas.Tutar,
                            yekun = s.service_faturas.Yekun,
                            maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                            fark = 0,
                            hesaplar = (from h in db.servicehesaps
                                        where h.ServiceID == s.ServiceID && h.iptal == false
                                        orderby h.TarihZaman descending
                                        select new ServisHesapRepo
                                        {
                                            hesapID = h.HesapID,
                                            aciklama = h.Aciklama,
                                            islemParca = h.IslemParca,
                                            kdv = h.KDV,
                                            onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                            onaylimi = h.onay,
                                            disServis = h.customer1.Ad,
                                            onayTarih = h.Onay_tarih,
                                            tarihZaman = h.TarihZaman,
                                            servisID = s.ServiceID,
                                            tutar = h.Tutar,
                                            yekun = (decimal)h.Yekun,
                                            cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                            birim_maliyet = h.birim_maliyet,
                                            toplam_maliyet = h.toplam_maliyet,
                                            kullanici = h.updated == null ? h.inserted : (h.inserted + "-" + h.updated)
                                        }).ToList()

                        }).ToList<ServisRepo>();
            }


        }
        public List<ServisRepo> servisRapor(bool? kapanma, DateTime? zaman = null)
        {


            DateTime sinir = zaman == null ? DateTime.Now.AddYears(-1) : (DateTime)zaman;
            if (kapanma != null)
            {
                return (from s in db.services
                        where (kapanma == false ? s.KapanmaZamani == null : s.KapanmaZamani != null) && s.iptal == false && s.AcilmaZamani > sinir
                        orderby s.AcilmaZamani descending
                        select new ServisRepo
                        {
                            serviceID = s.ServiceID,
                            custID = s.CustID == null ? -99 : (int)s.CustID,
                            musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                            adres = s.CustID == null ? "-" : s.customer.Adres,
                            telefon = s.CustID == null ? "-" : s.customer.telefon,
                            kullaniciID = s.olusturan_Kullanici,
                            sonGorevli = s.SonAtananID,
                            aciklama = s.Aciklama,
                            acilmaZamani = s.AcilmaZamani,
                            kapanmaZamani = s.KapanmaZamani,

                            kimlikNo = s.Servis_Kimlik_No,
                            tipID = s.tip_id,
                            css = s.service_tips.css,
                            servisTipi = s.service_tips.tip_ad,

                            baslik = s.Baslik,
                            tutar = s.service_faturas.Tutar,
                            yekun = s.service_faturas.Yekun,
                            maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                            fark = 0,
                            hesaplar = (from h in db.servicehesaps
                                        where h.ServiceID == s.ServiceID && h.iptal == false
                                        orderby h.TarihZaman descending
                                        select new ServisHesapRepo
                                        {
                                            hesapID = h.HesapID,
                                            aciklama = h.Aciklama,
                                            islemParca = h.IslemParca,
                                            kdv = h.KDV,
                                            onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                            onaylimi = h.onay,
                                            onayTarih = h.Onay_tarih,
                                            tarihZaman = h.TarihZaman,
                                            servisID = s.ServiceID,
                                            tutar = h.Tutar,
                                            yekun = (decimal)h.Yekun,
                                            cihaz = (h.cihaz_id == null && h.makine_id == null) ? "Servis" : (h.makine_id == null ? (h.adet.ToString() + "-" + h.cihaz.cihaz_adi) : (h.cihaz_adi + "-" + h.calisma_saati.ToString() + "-" + h.tarife_kodu)),
                                            birim_maliyet = h.birim_maliyet,
                                            toplam_maliyet = h.toplam_maliyet,
                                            dakika = h.makine_id == null ? (h.adet.ToString() + " " + h.birim) : (h.dakika.ToString() + " dakika"),
                                            tarife = h.tarife_kodu,
                                            sure = h.calisma_saati

                                        }).ToList()

                        }).ToList<ServisRepo>();
            }
            else
            {
                //hepsi
                return (from s in db.services
                        where s.iptal == false && s.AcilmaZamani > sinir
                        orderby s.AcilmaZamani descending
                        select new ServisRepo
                        {
                            serviceID = s.ServiceID,
                            custID = s.CustID == null ? -99 : (int)s.CustID,
                            musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                            adres = s.CustID == null ? "-" : s.customer.Adres,
                            telefon = s.CustID == null ? "-" : s.customer.telefon,
                            kullaniciID = s.olusturan_Kullanici,
                            sonGorevli = s.SonAtananID,
                            aciklama = s.Aciklama,
                            acilmaZamani = s.AcilmaZamani,
                            kapanmaZamani = s.KapanmaZamani,

                            kimlikNo = s.Servis_Kimlik_No,
                            tipID = s.tip_id,
                            css = s.service_tips.css,
                            servisTipi = s.service_tips.tip_ad,

                            baslik = s.Baslik,
                            tutar = s.service_faturas.Tutar,
                            yekun = s.service_faturas.Yekun,
                            maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                            fark = 0,
                            hesaplar = (from h in db.servicehesaps
                                        where h.ServiceID == s.ServiceID && h.iptal == false
                                        orderby h.TarihZaman descending
                                        select new ServisHesapRepo
                                        {
                                            hesapID = h.HesapID,
                                            aciklama = h.Aciklama,
                                            islemParca = h.IslemParca,
                                            kdv = h.KDV,
                                            onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                            onaylimi = h.onay,
                                            onayTarih = h.Onay_tarih,
                                            tarihZaman = h.TarihZaman,
                                            servisID = s.ServiceID,
                                            tutar = h.Tutar,
                                            yekun = (decimal)h.Yekun,
                                            cihaz = (h.cihaz_id == null && h.makine_id == null) ? "Servis" : (h.makine_id == null ? (h.adet.ToString() + "-" + h.cihaz.cihaz_adi) : (h.cihaz_adi + "-" + h.calisma_saati.ToString() + "-" + h.tarife_kodu)),
                                            birim_maliyet = h.birim_maliyet,
                                            toplam_maliyet = h.toplam_maliyet,
                                            dakika = h.makine_id == null ? (h.adet.ToString() + " " + h.birim) : (h.dakika.ToString() + " dakika"),
                                            tarife = h.tarife_kodu,
                                            sure = h.calisma_saati

                                        }).ToList()

                        }).ToList<ServisRepo>();
            }


        }
        public void usta_ata(int tamirci_id, int servis_id, string kullanici)
        {
            service s = db.services.FirstOrDefault(x => x.ServiceID == servis_id);
            if (s != null)
            {
                customer usta = db.customers.FirstOrDefault(x => x.CustID == tamirci_id);
                if (usta != null)
                {
                    s.usta_id = tamirci_id;
                    s.usta = usta.Ad;
                    s.updated = kullanici;
                    KaydetmeIslemleri.kaydetR(db);
                }

            }
        }
        public void usta_fire(int servis_id, string kullanici)
        {
            service s = db.services.FirstOrDefault(x => x.ServiceID == servis_id);
            if (s != null)
            {
                s.usta_id = null;
                s.usta = "";
                s.updated = kullanici;
                KaydetmeIslemleri.kaydetR(db);
            }
        }


        public sayfali servisTamirciSayfali(int tamirci_id, bool kapanma, int sayfaNo, int perpage, DateTime? zaman = null)
        {

            sayfali say = new sayfali();
            DateTime sinir = zaman == null ? DateTime.Now.AddYears(-1) : (DateTime)zaman;
            if (kapanma == false)
            {
                List<service> liste = (from s in db.services
                                       where s.usta_id == tamirci_id && s.KapanmaZamani == null && s.iptal == false && s.AcilmaZamani > sinir
                                       orderby s.AcilmaZamani descending
                                       select s).ToList();
                say.kayit_sayisi = liste.Count;

                List<ServisRepo> servisler = (from s in liste
                                              select new ServisRepo
                                              {
                                                  serviceID = s.ServiceID,
                                                  custID = s.CustID == null ? -99 : (int)s.CustID,
                                                  musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                                                  adres = s.CustID == null ? "-" : s.customer.Adres,
                                                  telefon = s.CustID == null ? "-" : s.customer.telefon,
                                                  kullaniciID = s.olusturan_Kullanici,
                                                  kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                                                  usta = s.usta,
                                                  usta_id = s.usta_id,
                                                  sonGorevli = s.SonAtananID,
                                                  aciklama = s.Aciklama,
                                                  acilmaZamani = s.AcilmaZamani,
                                                  kapanmaZamani = s.KapanmaZamani,

                                                  kimlikNo = s.Servis_Kimlik_No,
                                                  tipID = s.tip_id,
                                                  css = s.service_tips.css,
                                                  servisTipi = s.service_tips.tip_ad,

                                                  baslik = s.Baslik,
                                                  tutar = s.service_faturas.Tutar,
                                                  yekun = s.service_faturas.Yekun,
                                                  maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                                                  fark = 0,
                                                  hesaplar = (from h in db.servicehesaps
                                                              from m in db.service_maliyets
                                                              where h.ServiceID == s.ServiceID && h.iptal == false && m.hesapid == h.HesapID
                                                              orderby h.TarihZaman descending
                                                              select new ServisHesapRepo
                                                              {
                                                                  hesapID = h.HesapID,
                                                                  aciklama = h.Aciklama,
                                                                  islemParca = h.IslemParca,
                                                                  kdv = h.KDV,
                                                                  onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                                                  disServis = h.tamirci_id == null ? "" : h.customer1.Ad,
                                                                  onaylimi = h.onay,
                                                                  onayTarih = h.Onay_tarih,
                                                                  tarihZaman = h.TarihZaman,
                                                                  servisID = s.ServiceID,
                                                                  tutar = h.Tutar,
                                                                  yekun = (decimal)h.Yekun,
                                                                  cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                                                  birim_maliyet = h.birim_maliyet,
                                                                  toplam_maliyet = h.toplam_maliyet + m.tutar,
                                                                  kullanici = h.updated == null ? h.inserted : (h.inserted + "-" + h.updated)
                                                              }).ToList()

                                              }).Skip((sayfaNo - 1) * perpage).Take(perpage).ToList<ServisRepo>();
                say.servis_listesi = servisler;
                return say;
            }
            else
            {
                //hepsi
                List<service> liste = (from s in db.services
                                       where s.usta_id == tamirci_id && s.iptal == false && s.KapanmaZamani != null && s.AcilmaZamani > sinir
                                       orderby s.AcilmaZamani descending
                                       select s).ToList();
                say.kayit_sayisi = liste.Count;

                List<ServisRepo> servisler = (from s in liste
                                              select new ServisRepo
                                              {
                                                  serviceID = s.ServiceID,
                                                  custID = s.CustID == null ? -99 : (int)s.CustID,
                                                  musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                                                  adres = s.CustID == null ? "-" : s.customer.Adres,
                                                  telefon = s.CustID == null ? "-" : s.customer.telefon,
                                                  kullaniciID = s.olusturan_Kullanici,
                                                  kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                                                  sonGorevli = s.SonAtananID,
                                                  aciklama = s.Aciklama,
                                                  usta_id = s.usta_id,
                                                  usta = s.usta,
                                                  acilmaZamani = s.AcilmaZamani,
                                                  kapanmaZamani = s.KapanmaZamani,

                                                  kimlikNo = s.Servis_Kimlik_No,
                                                  tipID = s.tip_id,
                                                  css = s.service_tips.css,
                                                  servisTipi = s.service_tips.tip_ad,

                                                  baslik = s.Baslik,
                                                  tutar = s.service_faturas.Tutar,
                                                  yekun = s.service_faturas.Yekun,
                                                  maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                                                  fark = 0,
                                                  hesaplar = (from h in db.servicehesaps
                                                              from m in db.service_maliyets
                                                              where h.ServiceID == s.ServiceID && h.iptal == false && m.hesapid == h.HesapID
                                                              orderby h.TarihZaman descending
                                                              select new ServisHesapRepo
                                                              {
                                                                  hesapID = h.HesapID,
                                                                  aciklama = h.Aciklama,
                                                                  islemParca = h.IslemParca,
                                                                  kdv = h.KDV,
                                                                  onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                                                  onaylimi = h.onay,
                                                                  disServis = h.tamirci_id == null ? "" : h.customer1.Ad,
                                                                  onayTarih = h.Onay_tarih,
                                                                  tarihZaman = h.TarihZaman,
                                                                  servisID = s.ServiceID,
                                                                  tutar = h.Tutar,
                                                                  yekun = (decimal)h.Yekun,
                                                                  cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                                                  birim_maliyet = h.birim_maliyet,
                                                                  toplam_maliyet = h.toplam_maliyet + m.tutar,
                                                                  kullanici = h.updated == null ? h.inserted : (h.inserted + "-" + h.updated)
                                                              }).ToList()

                                              }).Skip((sayfaNo - 1) * perpage).Take(perpage).ToList<ServisRepo>();
                say.servis_listesi = servisler;
                return say;
            }


        }

        public sayfali servisSayfali(bool kapanma, int sayfaNo, int perpage, DateTime? zaman = null)
        {

            sayfali say = new sayfali();
            DateTime sinir = zaman == null ? DateTime.Now.AddDays(-7) : (DateTime)zaman;
            if (kapanma == false)
            {
                List<service> liste = (from s in db.services
                                       where s.KapanmaZamani == null && s.iptal == false && s.AcilmaZamani > sinir
                                       orderby s.AcilmaZamani descending
                                       select s).ToList();
                say.kayit_sayisi = liste.Count;
                List<ServisRepo> servisler = new List<ServisRepo>();
                if (liste.Count > 0)
                {
                    servisler = (from s in liste
                                 select new ServisRepo
                                 {
                                     serviceID = s.ServiceID,
                                     custID = s.CustID == null ? -99 : (int)s.CustID,
                                     musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                                     adres = s.CustID == null ? "-" : s.customer.Adres,
                                     telefon = s.CustID == null ? "-" : s.customer.telefon,
                                     kullaniciID = s.olusturan_Kullanici,
                                     kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                                     sonGorevli = s.SonAtananID,
                                     usta = s.usta,
                                     usta_id = s.usta_id,
                                     aciklama = s.Aciklama,
                                     acilmaZamani = s.AcilmaZamani,
                                     kapanmaZamani = s.KapanmaZamani,

                                     kimlikNo = s.Servis_Kimlik_No,
                                     tipID = s.tip_id,
                                     css = s.service_tips.css,
                                     servisTipi = s.service_tips.tip_ad,

                                     baslik = s.Baslik,
                                     tutar = s.service_faturas.Tutar,
                                     yekun = s.service_faturas.Yekun,
                                     maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                                     fark = 0,
                                     hesaplar = (from h in db.servicehesaps
                                                 from m in db.service_maliyets
                                                 where h.ServiceID == s.ServiceID && h.iptal == false && m.hesapid == h.HesapID
                                                 orderby h.TarihZaman descending
                                                 select new ServisHesapRepo
                                                 {
                                                     hesapID = h.HesapID,
                                                     aciklama = h.Aciklama,
                                                     islemParca = h.IslemParca,
                                                     kdv = h.KDV,
                                                     onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                                     disServis = h.tamirci_id == null ? "" : h.customer1.Ad,
                                                     onaylimi = h.onay,
                                                     onayTarih = h.Onay_tarih,
                                                     tarihZaman = h.TarihZaman,
                                                     servisID = s.ServiceID,
                                                     tutar = h.Tutar,
                                                     yekun = (decimal)h.Yekun,
                                                     cihaz = h.cihaz_id == null ? h.cihaz_adi : h.cihaz.cihaz_adi,
                                                     birim_maliyet = h.birim_maliyet,
                                                     toplam_maliyet = h.toplam_maliyet + m.tutar,
                                                     kullanici = h.updated == null ? h.inserted : (h.inserted + "-" + h.updated),
                                                     dakika = h.makine_id == null ? (h.adet.ToString() + " " + h.birim) : (h.dakika.ToString() + " dakika"),
                                                     tarife = h.tarife_kodu,
                                                     sure = h.calisma_saati

                                                 }).ToList()

                                 }).Skip((sayfaNo - 1) * perpage).Take(perpage).ToList<ServisRepo>();
                }

                say.servis_listesi = servisler;
                return say;
            }
            else
            {
                //hepsi
                List<service> liste = (from s in db.services
                                       where s.KapanmaZamani != null && s.iptal == false && s.AcilmaZamani > sinir
                                       orderby s.AcilmaZamani descending
                                       select s).ToList();
                say.kayit_sayisi = liste.Count;
                List<ServisRepo> servisler = new List<ServisRepo>();
                if (liste.Count >= 0)
                {
                    servisler = (from s in liste
                                 select new ServisRepo
                                 {
                                     serviceID = s.ServiceID,
                                     custID = s.CustID == null ? -99 : (int)s.CustID,
                                     musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                                     adres = s.CustID == null ? "-" : s.customer.Adres,
                                     telefon = s.CustID == null ? "-" : s.customer.telefon,
                                     kullaniciID = s.olusturan_Kullanici,
                                     sonGorevli = s.SonAtananID,
                                     usta = s.usta,
                                     usta_id = s.usta_id,
                                     aciklama = s.Aciklama,
                                     acilmaZamani = s.AcilmaZamani,
                                     kapanmaZamani = s.KapanmaZamani,

                                     kimlikNo = s.Servis_Kimlik_No,
                                     tipID = s.tip_id,
                                     css = s.service_tips.css,
                                     servisTipi = s.service_tips.tip_ad,

                                     baslik = s.Baslik,
                                     tutar = s.service_faturas.Tutar,
                                     yekun = s.service_faturas.Yekun,
                                     maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                                     fark = 0,
                                     hesaplar = (from h in db.servicehesaps
                                                 from m in db.service_maliyets
                                                 where h.ServiceID == s.ServiceID && h.iptal == false && m.hesapid == h.HesapID
                                                 orderby h.TarihZaman descending
                                                 select new ServisHesapRepo
                                                 {
                                                     hesapID = h.HesapID,
                                                     aciklama = h.Aciklama,
                                                     islemParca = h.IslemParca,
                                                     kdv = h.KDV,
                                                     onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                                     onaylimi = h.onay,
                                                     onayTarih = h.Onay_tarih,
                                                     disServis = h.tamirci_id == null ? "" : h.customer1.Ad,
                                                     tarihZaman = h.TarihZaman,
                                                     servisID = s.ServiceID,
                                                     tutar = h.Tutar,
                                                     yekun = (decimal)h.Yekun,
                                                     cihaz = h.cihaz_id == null ? h.cihaz_adi : h.cihaz.cihaz_adi,
                                                     birim_maliyet = h.birim_maliyet,
                                                     toplam_maliyet = h.toplam_maliyet + m.tutar,
                                                     dakika = h.makine_id == null ? (h.adet.ToString() + " " + h.birim) : (h.dakika.ToString() + " dakika"),
                                                     tarife = h.tarife_kodu,
                                                     sure = h.calisma_saati
                                                 }).ToList()

                                 }).Skip((sayfaNo - 1) * perpage).Take(perpage).ToList<ServisRepo>();
                }

                say.servis_listesi = servisler;
                return say;
            }


        }

        public List<ServisRepo> ServisListesi(string tipID, string custids, string gorevliID, string kelime)
        {
            int tipid = 0;
            if (!string.IsNullOrEmpty(tipID))
            {
                tipid = Int32.Parse(tipID);
            }
            int? custid = null;

            if (!String.IsNullOrEmpty(custids))
            {
                custid = Int32.Parse(custids);
            }

            //sadece müşteri seçilmişse kapanan servisler de görünüyor
            return (from s in db.services
                    where (string.IsNullOrEmpty(custids) ? s.KapanmaZamani == null : s.CustID == custid) && s.iptal == false
                    && (!string.IsNullOrEmpty(tipID) ? s.tip_id == tipid : s.iptal == false)
                    && ((!string.IsNullOrEmpty(gorevliID) ? (s.SonAtananID == gorevliID || s.SonAtananID.Equals("0")) : s.iptal == false))
                    && (!string.IsNullOrEmpty(kelime) ? (s.customer.Ad.Contains(kelime) || s.Baslik.Contains(kelime) ||
                    s.Servis_Kimlik_No.Contains(kelime) || s.Aciklama.Contains(kelime) || s.customer.TC.Contains(kelime) || s.customer.tanimlayici.Contains(kelime) || s.customer.Adres.Contains(kelime)) : s.iptal == false)

                    orderby s.AcilmaZamani descending
                    select new ServisRepo
                    {
                        serviceID = s.ServiceID,
                        custID = s.CustID == null ? -99 : (int)s.CustID,
                        musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                        adres = s.CustID == null ? "-" : s.customer.Adres,
                        telefon = s.CustID == null ? "-" : s.customer.telefon,
                        son_dis_servis = s.son_dis_servis,
                        usta = s.usta,
                        kullaniciID = s.olusturan_Kullanici,
                        sonGorevli = s.SonAtananID,
                        aciklama = s.Aciklama,
                        acilmaZamani = s.AcilmaZamani,
                        kapanmaZamani = s.KapanmaZamani,
                        kimlikNo = s.Servis_Kimlik_No,
                        tipID = s.tip_id,
                        css = s.service_tips.css,
                        servisTipi = s.service_tips.tip_ad,
                        baslik = s.Baslik,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                    }).ToList<ServisRepo>();

        }

        public List<ServisRepo> ServisListesiOperator(string kelime, string kullanici)
        {

            return (from s in db.servis_kullanicis
                    where s.kullanici == kullanici && s.iptal == false && s.service.KapanmaZamani == null && s.service.iptal == false
                    && (!string.IsNullOrEmpty(kelime) ? (s.service.customer.Ad.Contains(kelime) || s.service.Baslik.Contains(kelime) ||
                    s.service.Servis_Kimlik_No.Contains(kelime) || s.service.Aciklama.Contains(kelime) || s.service.customer.TC.Contains(kelime) || s.service.customer.tanimlayici.Contains(kelime) || s.service.customer.Adres.Contains(kelime)) : s.iptal == false)
                    orderby s.service.AcilmaZamani descending
                    select new ServisRepo
                    {
                        serviceID = s.service.ServiceID,
                        custID = s.service.CustID == null ? -99 : (int)s.service.CustID,
                        musteriAdi = s.service.CustID == null ? "-" : s.service.customer.Ad,
                        adres = s.service.CustID == null ? "-" : s.service.customer.Adres,
                        telefon = s.service.CustID == null ? "-" : s.service.customer.telefon,
                        son_dis_servis = s.service.son_dis_servis,
                        usta = s.service.usta,
                        kullaniciID = s.service.olusturan_Kullanici,
                        sonGorevli = s.service.SonAtananID,
                        aciklama = s.service.Aciklama,
                        acilmaZamani = s.service.AcilmaZamani,
                        kapanmaZamani = s.service.KapanmaZamani,
                        kimlikNo = s.service.Servis_Kimlik_No,
                        tipID = s.service.tip_id,
                        css = s.service.service_tips.css,
                        servisTipi = s.service.service_tips.tip_ad,
                        baslik = s.service.Baslik,
                        kullanici = s.service.updated == null ? s.service.inserted : (s.service.inserted + "-" + s.service.updated)

                    }).ToList<ServisRepo>();

        }
        public ServisRepo servisAraKimlikDetayTekR(string servisKimlikNo)
        {
            return (from s in db.services
                    where s.Servis_Kimlik_No == servisKimlikNo && s.iptal == false
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
        public ServisRepo ServisTek(int servisid)
        {
            return (from s in db.services
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
        public ServisRepo servisAraBarkod(string servisKimlikNo)
        {
            return (from s in db.services
                    where s.Servis_Kimlik_No == servisKimlikNo && s.iptal == false
                    select new ServisRepo
                    {
                        serviceID = s.ServiceID,


                        custID = s.CustID == null ? -99 : (int)s.CustID,

                    }).FirstOrDefault<ServisRepo>();

        }

        //manager için

        #endregion

        #region servis ekleme ve güncelleme işlemleri


        public void servisEkleGorevliR(int musID, string kullaniciID, string aciklama, int tipID, string atananID, string kimlik, string baslik, DateTime acilma_zamani, string kullanici)
        {

            TeknikServis.Radius.service servis = new TeknikServis.Radius.service();

            servis.CustID = musID;
            servis.Baslik = baslik;
            servis.Firma = "firma";
            servis.olusturan_Kullanici = kullaniciID;
            servis.SonAtananID = atananID;
            servis.AcilmaZamani = acilma_zamani;

            servis.iptal = false;
            servis.Aciklama = aciklama;
            servis.inserted = kullanici;
            servis.Servis_Kimlik_No = kimlik;
            servis.tip_id = tipID;
            //servis.SonDurum = durum_ayar.Durum;
            //servis.durum_id = durum_ayar.Durum_ID;

            db.services.Add(servis);
            KaydetmeIslemleri.kaydetR(db);

        }


        public decimal? servisEkleKararli(int musID, string kullaniciID, string aciklama, string kimlik, string baslik, DateTime acilma_zamani, karar_wrap karar, string kullanici)
        {

            //burada baslangic durumu neymiş bakalım.
            AyarIslemleri ayar = new AyarIslemleri(db);
            decimal cari = db.carihesaps.Find(musID).ToplamBakiye;

            TeknikServis.Radius.service servis = new TeknikServis.Radius.service();
            servis.CustID = (int)musID;
            servis.Baslik = baslik;
            servis.Firma = "firma";
            servis.olusturan_Kullanici = kullaniciID;
            servis.SonAtananID = "0";
            servis.AcilmaZamani = acilma_zamani;
            //servis.KapanmaZamani = DateTime.Now.AddYears(-20);
            servis.iptal = false;
            servis.Aciklama = aciklama;

            servis.KapanmaZamani = acilma_zamani;
            servis.Servis_Kimlik_No = kimlik;
            servis.tip_id = -1;

            servis.inserted = kullanici;


            //yukarıda otomatik servisi tanımladık, seçime göre paket satışı yapılırsa şu paket uygulamasını ekliyoruz

            if (karar != null)
            {
                int stok = 1;
                //paket değilmiş
                if (karar.cihaz_id != null)
                {
                    //cihaz stoğuna bakalım
                    decimal bakiye = db.cihaz_stoks.Find(karar.cihaz_id).bakiye;
                    if (bakiye <= 0)
                    {
                        stok *= 0;

                    }
                }
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();

                hesap.ServiceID = servis.ServiceID;
                hesap.MusteriID = (int)musID;

                hesap.IslemParca = karar.islemParca;
                hesap.iptal = false;
                hesap.servis_kimlik = kimlik;
                hesap.Tutar = karar.tutar;
                hesap.kullanici = kullaniciID;
                decimal tutar = (100 * karar.yekun) / (100 + karar.kdv);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * karar.kdv) / 100;
                hesap.birim = karar.birim;
                hesap.Yekun = karar.yekun;
                hesap.Aciklama = karar.aciklama;
                hesap.Firma = "firma";
                hesap.birim_fiyat = (decimal)(karar.yekun / karar.adet);
                hesap.TarihZaman = acilma_zamani;
                hesap.cihaz_id = karar.cihaz_id;
                hesap.adet = karar.adet;
                hesap.cihaz_adi = karar.cihaz_adi;
                hesap.cihaz_gsure = (int)karar.cihaz_gsure;
                hesap.sinirsiz = karar.sinirsiz;
                hesap.inserted = kullanici;
                hesap.tarife_kodu = "-";
                hesap.baslangic = 0;
                hesap.bitis = 0;
                hesap.baslangic_tarih = DateTime.Now;
                hesap.bitis_tarih = DateTime.Now;
                hesap.dakika = 0;
                hesap.yeni_sayac = 0;

                if (stok > 0)
                {
                    servis.servicehesaps.Add(hesap);

                    db.services.Add(servis);


                    KaydetmeIslemleri.kaydetR(db);
                }
                else
                {
                    return null;
                }


                //şimdi bu servis hesaplarının tamamını onaylayacağız

                List<servicehesap> kararlar = db.servicehesaps.Where(x => x.servis_kimlik == kimlik).ToList();
                if (kararlar.Count > 0)
                {
                    customer mu = db.customers.FirstOrDefault(x => x.CustID == musID);
                    foreach (servicehesap h in kararlar)
                    {
                        fatura fatik = new fatura();
                        fatik.bakiye = (decimal)h.Yekun;


                        fatik.no = h.HesapID.ToString();
                        fatik.taksit_no = 0;
                        fatik.odenen = 0;
                        fatik.Firma = h.Firma;
                        fatik.tc = mu.TC;
                        fatik.telefon = mu.telefon;
                        fatik.sattis_tarih = DateTime.Now;
                        fatik.servicehesap_id = h.HesapID;
                        fatik.service_id = h.ServiceID;
                        fatik.MusteriID = (int)musID;
                        fatik.islem_tarihi = DateTime.Now;
                        fatik.son_odeme_tarihi = acilma_zamani;

                        fatik.sattis_tarih = acilma_zamani;
                        fatik.tutar = (decimal)h.Yekun;

                        fatik.tur = "Taksit";
                        fatik.iptal = false;
                        fatik.service_id = h.ServiceID;
                        fatik.servicehesap_id = h.HesapID;
                        fatik.inserted = kullanici;
                        db.faturas.Add(fatik);

                        h.onay = true;
                        h.Onay_tarih = acilma_zamani;
                        if (h.cihaz_id != null)
                        {
                            var fifo = (from f in db.cihaz_fifos
                                        where f.cihaz_id == h.cihaz_id && f.bakiye > 0
                                        select f).FirstOrDefault();
                            if (fifo != null)
                            {
                                h.stok_id = fifo.id;
                                h.birim_maliyet = fifo.fiyat;
                                h.toplam_maliyet = 0;
                                if (h.sinirsiz == true)
                                {
                                    h.toplam_maliyet = h.adet * fifo.fiyat;
                                }
                            }

                        }

                    }
                }

                KaydetmeIslemleri.kaydetR(db);
            }


            return cari;

        }


        public void servisGuncelleR(int servisID, int musID, string aciklama, int tipID, string konu, string kullanici)
        {
            TeknikServis.Radius.service servis = tekServisR(servisID);
            if (servis != null)
            {
                servis.CustID = musID;

                servis.Aciklama = aciklama;
                servis.Baslik = konu;

                servis.tip_id = tipID;
                servis.updated = kullanici;

                KaydetmeIslemleri.kaydetR(db);
            }


        }



        private TeknikServis.Radius.service tekServisR(int serviceID)
        {
            return (from s in db.services
                    where s.ServiceID == serviceID
                    select s).FirstOrDefault();
        }

        private void servisIptalR(TeknikServis.Radius.service servis, string kullanici)
        {

            //servis toplamlarına göre
            if (servis.usta_id != null)
            {
                //usta prim oranları
                customer usta = db.customers.FirstOrDefault(x => x.CustID == servis.usta_id);
                carihesap cari = db.carihesaps.FirstOrDefault(x => x.MusteriID == servis.usta_id);
                service_faturas fat = db.service_faturas.FirstOrDefault(x => x.ServiceID == servis.ServiceID);
                decimal prim_yekun = usta.prim_yekun == null ? 0 : (decimal)usta.prim_yekun;
                decimal prim_kar = usta.prim_kar == null ? 0 : (decimal)usta.prim_kar;

                cari.ToplamAlacak = cari.ToplamAlacak - (fat.Yekun * prim_yekun / 100) - (fat.Yekun - (decimal)fat.toplam_maliyet) * prim_kar / 100;

            }
            servis.iptal = true;
            servis.deleted = kullanici;
            KaydetmeIslemleri.kaydetR(db);
        }

        public void servisIptalR(int serviceID, string kullanici)
        {
            TeknikServis.Radius.service s = tekServisR(serviceID);
            if (s != null)
            {
                servisIptalR(s, kullanici);
            }

        }
        #endregion
        #region servis detay ve hesap işlemleri
        public void servisDetayEkleR(int servisID, string belgeYol, string aciklama, string kullanici, string baslik)
        {
            TeknikServis.Radius.servicedetay detay = new TeknikServis.Radius.servicedetay();
            detay.ServiceID = servisID;
            detay.Firma = "firma";
            detay.TarihZaman = DateTime.Now;
            detay.BelgeYol = belgeYol;
            detay.Aciklama = aciklama;

            detay.KullaniciID = kullanici;
            detay.inserted = kullanici;
            detay.Baslik = baslik;
            db.servicedetays.Add(detay);
            KaydetmeIslemleri.kaydetR(db);

        }


        public List<ServisDetayRepo> detayListesiDetayKimlikR(string kimlik)
        {
            return (from s in db.servicedetays

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

        public void serviceKararEkleRYeni(int serviceID, int? musteriID, string islem, decimal kdvOran, decimal yekun, string aciklama, decimal adet, DateTime karar_tarihi, string kullanici)
        {
            //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
            service ser = db.services.FirstOrDefault(x => x.ServiceID == serviceID);
            if (ser != null)
            {
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = serviceID;
                if (musteriID != null)
                {
                    hesap.MusteriID = (int)musteriID;

                }
                else
                {
                    hesap.MusteriID = null;
                }
                // hesap.MusteriID = ser.CustID; query stringden alınıyor gereksiz mi bakıver
                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.birim_maliyet = 0;
                hesap.toplam_maliyet = 0;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.inserted = kullanici;
                hesap.adet = adet;
                hesap.TarihZaman = karar_tarihi;

                db.servicehesaps.Add(hesap);

                KaydetmeIslemleri.kaydetR(db);
            }

        }

        public void serviceKararEkleTamirci(int serviceID, int? musteriID, int tamirci_id, string islem, decimal kdvOran, decimal yekun, decimal maliyet, string aciklama, DateTime karar_tarihi, string kullanici)
        {
            //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
            service ser = db.services.FirstOrDefault(x => x.ServiceID == serviceID);
            if (ser != null)
            {
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = serviceID;
                if (musteriID != null)
                {
                    hesap.MusteriID = (int)musteriID;

                }
                else
                {
                    hesap.MusteriID = null;
                }

                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = ser.Baslik + " " + aciklama;
                hesap.Firma = "firma";
                hesap.birim_maliyet = maliyet;
                hesap.toplam_maliyet = maliyet;
                hesap.tamirci_id = tamirci_id;
                hesap.adet = 1;
                hesap.TarihZaman = karar_tarihi;
                hesap.inserted = kullanici;
                hesap.tarife_kodu = "dışarıdan";
                hesap.baslangic = 0;
                hesap.bitis = 0;
                hesap.dakika = 0;
                hesap.bitis_tarih = DateTime.Now;
                hesap.baslangic_tarih = DateTime.Now;
                db.servicehesaps.Add(hesap);

                KaydetmeIslemleri.kaydetR(db);
            }

        }
        public void serviceKararGuncelleTamirci(int hesapID, int tamirci_id, string islem, decimal kdvOran, decimal yekun, decimal maliyet, string aciklama, DateTime karar_tarihi, string kullanici)
        {
            //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
            servicehesap hesap = db.servicehesaps.FirstOrDefault(x => x.HesapID == hesapID);
            if (hesap != null)
            {
                //customer tamirci = db.customers.FirstOrDefault(x => x.CustID == tamirci_id);
                //hesap.tamirci_id = tamirci_id;
                //hesap.son_dis_servis = tamirci.Ad;
                //hesap.MusteriID = ser.CustID; query stringden alınıyor gereksiz mi bakıver
                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.birim_maliyet = maliyet;
                hesap.toplam_maliyet = maliyet;
                hesap.tamirci_id = tamirci_id;
                hesap.adet = 1;
                hesap.TarihZaman = karar_tarihi;
                hesap.updated = kullanici;
                KaydetmeIslemleri.kaydetR(db);
            }
        }

        public void serviceKararEkleRYeniCihazli(int serviceID, int? musteriID, string islem, decimal kdvOran, decimal yekun, string aciklama, int cihazID, decimal adet, string cihaz_adi, string garanti_sure, DateTime karar_tarihi, string kullanici, string birim, decimal fiyat, bool sinir)
        {
            int sure = 12;
            if (!String.IsNullOrEmpty(garanti_sure))
            {
                sure = Int32.Parse(garanti_sure);
            }

            service ser = db.services.FirstOrDefault(x => x.ServiceID == serviceID);
            if (ser != null)
            {
                //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = serviceID;
                if (musteriID != null)
                {
                    hesap.MusteriID = (int)musteriID;
                }
                else
                {
                    hesap.MusteriID = null;
                }
                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.inserted = kullanici;
                hesap.sinirsiz = sinir;
                hesap.TarihZaman = karar_tarihi;
                hesap.birim = birim;
                hesap.cihaz_id = cihazID;
                hesap.adet = adet;
                hesap.cihaz_adi = cihaz_adi;
                hesap.cihaz_gsure = sure;
                hesap.birim_fiyat = fiyat;
                hesap.calisma_saati = 0;
                hesap.baslangic = 0;
                hesap.bitis = 0;
                hesap.baslangic_tarih = DateTime.Now;
                hesap.bitis_tarih = DateTime.Now;
                hesap.tarife_kodu = "-";

                db.servicehesaps.Add(hesap);

                KaydetmeIslemleri.kaydetR(db);
            }

        }

        public void serviceKararEkleRYeniMakine(int serviceID, int? musteriID, string islem, decimal kdvOran, decimal yekun, string aciklama, int makineID, string makine, DateTime karar_tarihi, string kullanici, string tarife_kodu, decimal baslangic, decimal bitis, decimal calisma_saati, DateTime baslangic_tarih, DateTime bitis_tarih, decimal son_sayac, int dakika)
        {

            service ser = db.services.FirstOrDefault(x => x.ServiceID == serviceID);
            if (ser != null)
            {
                //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = serviceID;
                if (musteriID != null)
                {
                    hesap.MusteriID = (int)musteriID;
                }
                else
                {
                    hesap.MusteriID = null;
                }
                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.cihaz_adi = makine;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.inserted = kullanici;
                hesap.TarihZaman = karar_tarihi;
                hesap.dakika = dakika;
                hesap.makine_id = makineID;
                hesap.calisma_saati = calisma_saati;
                hesap.baslangic = baslangic;
                hesap.bitis = bitis;
                hesap.baslangic_tarih = baslangic_tarih;
                hesap.bitis_tarih = bitis_tarih;
                hesap.tarife_kodu = tarife_kodu;

                servicemakine m = db.servicemakines.FirstOrDefault(x => x.serviceid == serviceID && x.makine_id == makineID && x.tarife_kodu == tarife_kodu);
                if (m == null)
                {
                    servicemakine yeni = new servicemakine();
                    yeni.calisma_saati = 0;
                    yeni.iptal = false;
                    yeni.makine_id = makineID;
                    yeni.maliyet = 0;
                    yeni.serviceid = serviceID;
                    yeni.tarife_kodu = tarife_kodu;
                    yeni.yekun = 0;
                    db.servicemakines.Add(yeni);
                }

                db.servicehesaps.Add(hesap);

                KaydetmeIslemleri.kaydetR(db);
            }

        }

        public void serviceKararEkleCalismaTipli(int serviceID, int? musteriID, string islem, decimal kdvOran, decimal yekun, string aciklama, int makineID, string makine, DateTime karar_tarihi, string kullanici, string tarife_kodu, string tarife_tipi, int tarifeid, decimal baslangic, decimal bitis, decimal calisma_saati, DateTime baslangic_tarih, DateTime bitis_tarih, decimal son_sayac, int dakika, decimal sayac_farki, decimal fiyat, string sure_aciklama)
        {

            service ser = db.services.FirstOrDefault(x => x.ServiceID == serviceID);
            if (ser != null)
            {
                //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = serviceID;
                if (musteriID != null)
                {
                    hesap.MusteriID = (int)musteriID;
                }
                else
                {
                    hesap.MusteriID = null;
                }
                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.cihaz_adi = makine;
                hesap.toplam_sayac = sayac_farki;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.tarifeid = tarifeid;
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.inserted = kullanici;
                hesap.sure_aciklama = sure_aciklama;
                hesap.TarihZaman = karar_tarihi;
                hesap.tarife_tipi = tarife_tipi;
                hesap.dakika = dakika;
                hesap.birim_fiyat = fiyat;
                hesap.makine_id = makineID;
                hesap.calisma_saati = calisma_saati;
                hesap.baslangic = baslangic;
                hesap.bitis = bitis;
                hesap.baslangic_tarih = baslangic_tarih;
                hesap.bitis_tarih = bitis_tarih;
                hesap.tarife_kodu = tarife_kodu;

                servicemakine m = db.servicemakines.FirstOrDefault(x => x.serviceid == serviceID && x.makine_id == makineID && x.tarife_kodu == tarife_kodu);
                if (m == null)
                {
                    servicemakine yeni = new servicemakine();
                    yeni.calisma_saati = 0;
                    yeni.toplam_dakika = 0;
                    yeni.toplam_sayac = 0;
                    yeni.iptal = false;
                    yeni.makine_id = makineID;
                    yeni.maliyet = 0;
                    yeni.serviceid = serviceID;
                    yeni.tarife_kodu = tarife_kodu;
                    yeni.yekun = 0;
                    db.servicemakines.Add(yeni);
                }

                db.servicehesaps.Add(hesap);

                KaydetmeIslemleri.kaydetR(db);
            }

        }

        public void serbest_sil(int hesapid)
        {
            var hes = db.servicehesap_ops.FirstOrDefault(x => x.HesapID == hesapid);
            if (hes != null)
            {
                hes.iptal = true;
                KaydetmeIslemleri.kaydetR(db);
            }
        }
        public void kararekle_operator_seyyar(string islem, decimal kdvOran, decimal yekun, string aciklama, int makineID, string makine, DateTime karar_tarihi, string kullanici, string tarife_kodu, decimal baslangic, decimal bitis, decimal calisma_saati, DateTime baslangic_tarih, DateTime bitis_tarih, decimal son_sayac, int dakika, string tarife_tipi, int tarifeid, decimal sayac_farki, string sure_aciklama)
        {

            TeknikServis.Radius.servicehesap_ops hesap = new TeknikServis.Radius.servicehesap_ops();

            hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
            hesap.IslemParca = islem;
            hesap.cihaz_adi = makine;
            hesap.iptal = false;
            decimal tutar = (100 * yekun) / (100 + kdvOran);
            hesap.Tutar = tutar;
            hesap.KDV = (tutar * kdvOran) / 100;
            hesap.Yekun = yekun;//burası kdv dahil fiyat
            hesap.Aciklama = aciklama;
            hesap.Firma = "firma";
            hesap.inserted = kullanici;
            hesap.tarife_tipi = tarife_tipi;
            hesap.tarifeid = tarifeid;
            hesap.TarihZaman = karar_tarihi;
            hesap.dakika = dakika;
            hesap.makine_id = makineID;
            hesap.calisma_saati = calisma_saati;
            hesap.baslangic = baslangic;
            hesap.bitis = bitis;
            hesap.baslangic_tarih = baslangic_tarih;
            hesap.bitis_tarih = bitis_tarih;
            hesap.tarife_kodu = tarife_kodu;
            hesap.toplam_sayac = sayac_farki;
            hesap.sure_aciklama = sure_aciklama;

            db.servicehesap_ops.Add(hesap);

            KaydetmeIslemleri.kaydetR(db);

        }

        public servicehesap_ops tekserbest(int hesapid)
        {
            return db.servicehesap_ops.FirstOrDefault(x => x.HesapID == hesapid);
        }
        public void kararonay_operator(int hesapid, int musteriID, string kimlik)
        {
            var hes = db.servicehesap_ops.FirstOrDefault(x => x.HesapID == hesapid);
            if (hes != null)
            {
                TeknikServis.Radius.service servis = new TeknikServis.Radius.service();
                servis.CustID = musteriID;
                servis.Baslik = "serbest operatör faaliyeti";
                servis.Firma = "firma";
                servis.olusturan_Kullanici = hes.kullanici;
                servis.SonAtananID = "0";
                servis.AcilmaZamani = hes.TarihZaman;
                servis.iptal = false;
                servis.Aciklama = "serbest operatör faaliyeti";
                servis.KapanmaZamani = hes.TarihZaman;
                servis.Servis_Kimlik_No = kimlik;
                servis.tip_id = -1;
                servis.inserted = hes.kullanici;

                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = servis.ServiceID;
                hesap.MusteriID = musteriID;
                hesap.kullanici = hes.kullanici;
                hesap.IslemParca = hes.IslemParca;
                hesap.cihaz_adi = hes.cihaz_adi;
                hesap.iptal = false;
                hesap.Tutar = hes.Tutar;
                hesap.KDV = hes.KDV;
                hesap.Yekun = hes.Yekun;//burası kdv dahil fiyat
                hesap.Aciklama = hes.Aciklama;
                hesap.Firma = "firma";
                hesap.inserted = hes.kullanici;
                hesap.tarife_tipi = hes.tarife_tipi;
                hesap.tarifeid = hes.tarifeid;
                hesap.TarihZaman = hes.TarihZaman;
                hesap.dakika = hes.dakika;
                hesap.makine_id = hes.makine_id;
                hesap.calisma_saati = hes.calisma_saati;
                hesap.baslangic = hes.baslangic;
                hesap.bitis = hes.bitis;
                hesap.baslangic_tarih = hes.baslangic_tarih;
                hesap.bitis_tarih = hes.bitis_tarih;
                hesap.tarife_kodu = hes.tarife_kodu;
                hesap.toplam_sayac = hes.toplam_sayac;

                servis.servicehesaps.Add(hesap);


                servicemakine yeni = new servicemakine();
                yeni.calisma_saati = 0;
                yeni.toplam_dakika = 0;
                yeni.toplam_sayac = 0;
                yeni.iptal = false;
                yeni.makine_id = (int)hes.makine_id;
                yeni.maliyet = 0;
                yeni.serviceid = servis.ServiceID;
                yeni.tarife_kodu = hes.tarife_kodu;
                yeni.yekun = 0;
                servis.servicemakines.Add(yeni);

                db.services.Add(servis);

                KaydetmeIslemleri.kaydetR(db);

                //şimdi bu servis hesaplarının tamamını onaylayacağız
                List<servicehesap> hesaplar = db.servicehesaps.Where(x => x.servis_kimlik == kimlik && (x.iptal == false || x.iptal == null) && (x.onay == null || x.onay == false)).ToList();

                foreach (var hesss in hesaplar)
                {
                    servisKararOnayCalismaTipli(hesss, hes.kullanici);


                }

                KaydetmeIslemleri.kaydetR(db);
            }

        }

        public void serviceKararEkleOperatorCT(int serviceID, int? musteriID, string islem, decimal kdvOran, decimal yekun, string aciklama, int makineID, string makine, DateTime karar_tarihi, string kullanici, string tarife_kodu, decimal baslangic, decimal bitis, decimal calisma_saati, DateTime baslangic_tarih, DateTime bitis_tarih, decimal son_sayac, int dakika, string tarife_tipi, int tarifeid, decimal sayac_farki)
        {
            service ser = db.services.FirstOrDefault(x => x.ServiceID == serviceID);
            if (ser != null)
            {
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = serviceID;
                hesap.MusteriID = musteriID;
                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.cihaz_adi = makine;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.inserted = kullanici;
                hesap.tarife_tipi = tarife_tipi;
                hesap.tarifeid = tarifeid;
                hesap.TarihZaman = karar_tarihi;
                hesap.dakika = dakika;
                hesap.makine_id = makineID;
                hesap.calisma_saati = calisma_saati;
                hesap.baslangic = baslangic;
                hesap.bitis = bitis;
                hesap.baslangic_tarih = baslangic_tarih;
                hesap.bitis_tarih = bitis_tarih;
                hesap.tarife_kodu = tarife_kodu;
                hesap.toplam_sayac = sayac_farki;

                servicemakine m = db.servicemakines.FirstOrDefault(x => x.serviceid == serviceID && x.makine_id == makineID && x.tarife_kodu == tarife_kodu);
                if (m == null)
                {
                    servicemakine yeni = new servicemakine();
                    yeni.calisma_saati = 0;
                    yeni.toplam_dakika = 0;
                    yeni.toplam_sayac = 0;
                    yeni.iptal = false;
                    yeni.makine_id = makineID;
                    yeni.maliyet = 0;
                    yeni.serviceid = serviceID;
                    yeni.tarife_kodu = tarife_kodu;
                    yeni.yekun = 0;
                    db.servicemakines.Add(yeni);
                }

                db.servicehesaps.Add(hesap);

                KaydetmeIslemleri.kaydetR(db);

                //şimdi bu servis hesaplarının tamamını onaylayacağız
                List<servicehesap> hesaplar = db.servicehesaps.Where(x => x.ServiceID == serviceID && (x.iptal == false || x.iptal == null) && (x.onay == null || x.onay == false)).ToList();

                foreach (var hes in hesaplar)
                {
                    servisKararOnayCalismaTipli(hes, kullanici);


                }

                KaydetmeIslemleri.kaydetR(db);
            }

        }


        public void serviceKararGuncelleMakine(int hesap_id, string islem, decimal kdvOran, decimal yekun, string aciklama, int makineID, DateTime karar_tarihi, string kullanici, string tarife_kodu, decimal baslangic, decimal bitis, decimal calisma_saati, DateTime baslangic_tarih, DateTime bitis_tarih, int dakika, decimal sayac_farki, string tarife_tipi, int tarifeid, string makine)
        {


            //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
            TeknikServis.Radius.servicehesap hesap = db.servicehesaps.FirstOrDefault(x => x.HesapID == hesap_id);
            if (hesap != null)
            {

                hesap.IslemParca = islem;
                hesap.cihaz_adi = makine;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.inserted = kullanici;
                hesap.tarife_tipi = tarife_tipi;
                hesap.tarifeid = tarifeid;
                hesap.TarihZaman = karar_tarihi;
                hesap.dakika = dakika;
                hesap.makine_id = makineID;
                hesap.calisma_saati = calisma_saati;
                hesap.baslangic = baslangic;
                hesap.bitis = bitis;
                hesap.baslangic_tarih = baslangic_tarih;
                hesap.bitis_tarih = bitis_tarih;
                hesap.tarife_kodu = tarife_kodu;
                hesap.toplam_sayac = sayac_farki;

                KaydetmeIslemleri.kaydetR(db);
            }

        }

        public TeknikServis.Radius.servicehesap tekHesapR(int hesapID)
        {
            return db.servicehesaps.Where(s => s.HesapID == hesapID).FirstOrDefault();
        }

        public void serviceKararGuncelleR(int id, string islem, string aciklama, decimal kdvOran, decimal yekun, string kullanici)
        {

            TeknikServis.Radius.servicehesap hesap = tekHesapR(id);


            hesap.IslemParca = islem;
            hesap.Aciklama = aciklama;
            hesap.Yekun = yekun;
            decimal tutar = (100 * yekun) / (100 + kdvOran);
            hesap.Tutar = tutar;
            hesap.updated = kullanici;
            hesap.KDV = (tutar * kdvOran) / 100;
            hesap.TarihZaman = DateTime.Now;
            KaydetmeIslemleri.kaydetR(db);
        }
        public void serviceKararGuncelleCihazli(int id, string islem, string aciklama, decimal kdvOran, decimal yekun, string cihaz_adi, int cihaz_id, int adet, string kullanici)
        {

            TeknikServis.Radius.servicehesap hesap = tekHesapR(id);

            hesap.cihaz_adi = cihaz_adi;
            hesap.cihaz_id = cihaz_id;
            hesap.adet = adet;
            hesap.IslemParca = islem;
            hesap.Aciklama = aciklama;
            hesap.Yekun = yekun;
            decimal tutar = (100 * yekun) / (100 + kdvOran);
            hesap.Tutar = tutar;
            hesap.updated = kullanici;
            hesap.KDV = (tutar * kdvOran) / 100;
            hesap.TarihZaman = DateTime.Now;
            KaydetmeIslemleri.kaydetR(db);
        }



        public List<taksitimiz> TaksitOlustur(DateTime baslamaTarihi, int periyot, int taksitSayisi, decimal tutar)
        {
            DateTime[] tarihler = new DateTime[(taksitSayisi)];
            List<taksitimiz> taksitler = new List<taksitimiz>();
            //son taksit hariç hepsi taksit miktarı olacak
            decimal taksitMiktari = Math.Ceiling(tutar / taksitSayisi);

            decimal sonTaksit = tutar - (taksitMiktari * (taksitSayisi - 1));

            for (int i = 0; i < taksitSayisi; i++)
            {
                tarihler[i] = baslamaTarihi;

                taksitimiz tak = new taksitimiz();
                tak.taksitTarihi = baslamaTarihi;
                tak.taksitNo = i;
                tak.taksitTutari = taksitMiktari;
                taksitler.Add(tak);

                baslamaTarihi = baslamaTarihi.AddDays(periyot);
            }
            taksitler[(taksitSayisi - 1)].taksitTutari = sonTaksit;
            return taksitler;
        }
        public List<taksitimiz> TaksitKaydet(int hesapID, DateTime baslamaTarihi, int periyot, int taksitSayisi, decimal tutar)
        {
            TeknikServis.Radius.servicehesap hesap = tekHesapR(hesapID);
            List<taksitimiz> taksitler = TaksitOlustur(baslamaTarihi, periyot, taksitSayisi, tutar);
            foreach (taksitimiz tak in taksitler)
            {

                tekTaksitKaydet(hesap, tak.taksitNo, tak.taksitTutari, tak.taksitTarihi, tak.taksitTutari);
            }
            hesap.onay = true;
            hesap.Onay_tarih = DateTime.Now;
            KaydetmeIslemleri.kaydetR(db);

            return taksitler;
        }
        private void tekTaksitKaydet(TeknikServis.Radius.servicehesap hesap, int taksitNo, decimal bakiye, DateTime tarih, decimal taksitMiktar)
        {

            TeknikServis.Radius.customer mu = db.customers.Where(x => x.CustID == hesap.MusteriID).FirstOrDefault();

            fatura fat = new fatura();
            fat.bakiye = bakiye;
            fat.sattis_tarih = DateTime.Now;
            fat.service_id = hesap.ServiceID;
            fat.servicehesap_id = hesap.HesapID;
            fat.son_odeme_tarihi = tarih;
            fat.MusteriID = hesap.MusteriID;
            fat.no = hesap.HesapID.ToString();
            fat.taksit_no = taksitNo;
            fat.odenen = 0;
            fat.Firma = hesap.Firma;
            fat.tc = mu.TC;
            fat.telefon = mu.telefon;
            fat.tutar = taksitMiktar;

            fat.tur = "Taksit";
            fat.iptal = false;
            fat.islem_tarihi = DateTime.Now;
            db.faturas.Add(fat);

        }

        public musteri_bilgileri servisKararOnayCalismaTipli(int id, string kullanici)
        {
            //tamirci_idyi ekleyeceğiz
            TeknikServis.Radius.servicehesap hesap = tekHesapR(id);
            int? tamirci_id = hesap.tamirci_id;

            musteri_bilgileri mu = new musteri_bilgileri();
            if (hesap.cihaz_id != null)
            {
                var fifo = (from f in db.cihaz_fifos

                            where f.cihaz_id == hesap.cihaz_id && f.bakiye > 0
                            orderby f.tarih ascending
                            select f).FirstOrDefault();
                if (fifo != null)
                {
                    hesap.stok_id = fifo.id;
                    hesap.birim_maliyet = fifo.fiyat;
                    hesap.toplam_maliyet = 0;
                    if (hesap.sinirsiz == true)
                    {
                        hesap.toplam_maliyet = hesap.adet * fifo.fiyat;
                    }
                    //tek taksit oluşturalım
                    mu = (from m in db.customers
                          where m.CustID == hesap.MusteriID
                          select new musteri_bilgileri
                          {
                              ad = m.Ad,
                              email = m.email,
                              tel = m.telefon,
                              tc = m.TC,
                              caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == m.CustID).ToplamBakiye
                          }).FirstOrDefault();


                    fatura fatik = new fatura();
                    fatik.bakiye = (decimal)hesap.Yekun;
                    fatik.tamirci_id = tamirci_id;
                    fatik.no = hesap.HesapID.ToString();
                    fatik.taksit_no = 0;
                    fatik.odenen = 0;
                    fatik.Firma = hesap.Firma;
                    fatik.tc = mu.tc;
                    fatik.telefon = mu.tel;
                    fatik.tutar = (decimal)hesap.Yekun;
                    fatik.tamirci_maliyet = hesap.toplam_maliyet;
                    fatik.MusteriID = hesap.MusteriID;
                    fatik.tur = "Taksit";
                    fatik.iptal = false;
                    fatik.service_id = hesap.ServiceID;
                    fatik.servicehesap_id = id;
                    fatik.sattis_tarih = DateTime.Now;
                    fatik.son_odeme_tarihi = DateTime.Now;
                    fatik.islem_tarihi = DateTime.Now;
                    fatik.inserted = kullanici;

                    db.faturas.Add(fatik);

                    //buradaki servisid tahsilatta tahsilat türü için kullanıyorum, triggerla service hesapsa kart ise yazdırıyorum


                    hesap.onay = true;
                    hesap.updated = kullanici;
                    hesap.Onay_tarih = DateTime.Now;
                    KaydetmeIslemleri.kaydetR(db);
                    return mu;
                }
                else
                {
                    //stok yokmuş
                    return mu;
                }


            }
            else
            {
                if (hesap.makine_id != null)
                {
                    var masraf_tanimlar = db.makine_masraf_tanims2.Where(x => x.iptal == false && x.tarifeid == hesap.tarifeid).ToList();

                    decimal toplam_maliyet = 0;

                    foreach (var m in masraf_tanimlar)
                    {
                        makine_masraf_hesaps2 h = new makine_masraf_hesaps2();
                        h.aciklama = "";
                        h.birim = m.birim;
                        h.serviceid = hesap.ServiceID;
                        h.calisma_saati = hesap.calisma_saati;
                        h.tarifekodu = hesap.tarife_kodu;
                        h.tarih = DateTime.Now;
                        h.dakika = hesap.dakika;

                        //decimal saat_basina_masraf = m.masraf_saat;
                        //burada sadece saat bölümüne yazıyoruz diğer yerler iptal

                        h.miktar = m.masraf_saat * hesap.calisma_saati;
                        decimal mal = m.makine_masrafs.birim_maliyet * m.masraf_saat * hesap.calisma_saati;
                        h.tutar = mal;
                        toplam_maliyet = toplam_maliyet + mal;


                        h.gelir = (decimal)hesap.Yekun;
                        h.hesap_id = hesap.HesapID;
                        h.makine_id = (int)hesap.makine_id;
                        h.masraf_id = m.masraf_id;
                        h.son_sayac = hesap.yeni_sayac;

                        db.makine_masraf_hesaps2.Add(h);

                    }

                    var sayaclar = db.makine_servis_sayacs.Where(x => x.makine_id == hesap.makine_id).ToList();
                    foreach (var s in sayaclar)
                    {
                        s.sayac = s.sayac - hesap.toplam_sayac;
                    }

                    hesap.birim_maliyet = toplam_maliyet;
                    hesap.toplam_maliyet = toplam_maliyet;

                }
                //cihazlı değil
                //tek taksit oluşturalım
                mu = (from m in db.customers
                      where m.CustID == hesap.MusteriID
                      select new musteri_bilgileri
                      {
                          ad = m.Ad,
                          email = m.email,
                          tel = m.telefon,
                          tc = m.TC,
                          caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == m.CustID).ToplamBakiye
                      }).FirstOrDefault();


                fatura fatik = new fatura();
                fatik.bakiye = (decimal)hesap.Yekun;
                fatik.tamirci_id = tamirci_id;
                fatik.no = hesap.HesapID.ToString();
                fatik.taksit_no = 0;
                fatik.odenen = 0;
                fatik.Firma = hesap.Firma;
                fatik.tc = mu.tc;
                fatik.telefon = mu.tel;
                fatik.tutar = (decimal)hesap.Yekun;
                fatik.tamirci_maliyet = hesap.toplam_maliyet;
                fatik.MusteriID = hesap.MusteriID;
                fatik.tur = "Taksit";
                fatik.iptal = false;
                fatik.service_id = hesap.ServiceID;
                fatik.servicehesap_id = id;
                fatik.sattis_tarih = DateTime.Now;
                fatik.son_odeme_tarihi = DateTime.Now;
                fatik.islem_tarihi = DateTime.Now;
                fatik.inserted = kullanici;
                db.faturas.Add(fatik);

                //buradaki servisid tahsilatta tahsilat türü için kullanıyorum, triggerla service hesapsa kart ise yazdırıyorum


                hesap.onay = true;

                hesap.updated = kullanici;
                hesap.Onay_tarih = DateTime.Now;
                KaydetmeIslemleri.kaydetR(db);
                //db.SaveChanges();
                return mu;
            }



        }

        public void servisKararOnay(servicehesap hesap, string kullanici)
        {
            //tamirci_idyi ekleyeceğiz
            int? tamirci_id = hesap.tamirci_id;
            musteri_bilgileri mu = new musteri_bilgileri();
            if (hesap.cihaz_id != null)
            {
                var fifo = (from f in db.cihaz_fifos

                            where f.cihaz_id == hesap.cihaz_id && f.bakiye > 0
                            orderby f.tarih ascending
                            select f).FirstOrDefault();
                if (fifo != null)
                {
                    hesap.stok_id = fifo.id;
                    hesap.birim_maliyet = fifo.fiyat;
                    hesap.toplam_maliyet = 0;
                    if (hesap.sinirsiz == true)
                    {
                        hesap.toplam_maliyet = hesap.adet * fifo.fiyat;
                    }

                    //tek taksit oluşturalım
                    mu = (from m in db.customers
                          where m.CustID == hesap.MusteriID
                          select new musteri_bilgileri
                          {
                              ad = m.Ad,
                              email = m.email,
                              tel = m.telefon,
                              tc = m.TC,
                              caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == m.CustID).ToplamBakiye
                          }).FirstOrDefault();


                    fatura fatik = new fatura();
                    fatik.bakiye = (decimal)hesap.Yekun;
                    fatik.tamirci_id = tamirci_id;
                    fatik.no = hesap.HesapID.ToString();
                    fatik.taksit_no = 0;
                    fatik.odenen = 0;
                    fatik.Firma = hesap.Firma;
                    fatik.tc = mu.tc;
                    fatik.telefon = mu.tel;
                    fatik.tutar = (decimal)hesap.Yekun;
                    fatik.tamirci_maliyet = hesap.toplam_maliyet;
                    fatik.MusteriID = hesap.MusteriID;
                    fatik.tur = "Taksit";
                    fatik.iptal = false;
                    fatik.service_id = hesap.ServiceID;
                    fatik.servicehesap_id = hesap.HesapID;
                    fatik.sattis_tarih = DateTime.Now;
                    fatik.son_odeme_tarihi = DateTime.Now;
                    fatik.islem_tarihi = DateTime.Now;
                    fatik.inserted = kullanici;

                    db.faturas.Add(fatik);

                    //buradaki servisid tahsilatta tahsilat türü için kullanıyorum, triggerla service hesapsa kart ise yazdırıyorum


                    hesap.onay = true;
                    hesap.updated = kullanici;
                    hesap.Onay_tarih = DateTime.Now;
                    KaydetmeIslemleri.kaydetR(db);

                }
                //toplu onay
            }
            else
            {
                if (hesap.makine_id != null)
                {
                    var masraf_tanimlar = db.makine_masraf_tanims2.Where(x => x.iptal == false && x.makine_id == hesap.makine_id && x.tarifeid == hesap.tarifeid).ToList();

                    decimal toplam_maliyet = 0;

                    foreach (var m in masraf_tanimlar)
                    {
                        makine_masraf_hesaps2 h = new makine_masraf_hesaps2();
                        h.aciklama = "";
                        h.birim = m.birim;
                        h.serviceid = hesap.ServiceID;
                        h.calisma_saati = hesap.calisma_saati;
                        h.tarifekodu = hesap.tarife_kodu;
                        h.tarih = DateTime.Now;
                        h.dakika = hesap.dakika;

                        //decimal saat_basina_masraf = m.masraf_saat;
                        //burada sadece saat bölümüne yazıyoruz diğer yerler iptal

                        h.miktar = m.masraf_saat * hesap.calisma_saati;
                        decimal mal = m.makine_masrafs.birim_maliyet * m.masraf_saat * hesap.calisma_saati;
                        h.tutar = mal;
                        toplam_maliyet = toplam_maliyet + mal;


                        h.gelir = (decimal)hesap.Yekun;
                        h.hesap_id = hesap.HesapID;
                        h.makine_id = (int)hesap.makine_id;
                        h.masraf_id = m.masraf_id;
                        h.son_sayac = hesap.yeni_sayac;

                        db.makine_masraf_hesaps2.Add(h);

                    }

                    var sayaclar = db.makine_servis_sayacs.Where(x => x.makine_id == hesap.makine_id).ToList();
                    foreach (var s in sayaclar)
                    {
                        s.sayac = s.sayac - hesap.dakika;
                    }

                    hesap.birim_maliyet = toplam_maliyet;
                    hesap.toplam_maliyet = toplam_maliyet;

                }
                //cihazlı değil
                //tek taksit oluşturalım
                mu = (from m in db.customers
                      where m.CustID == hesap.MusteriID
                      select new musteri_bilgileri
                      {
                          ad = m.Ad,
                          email = m.email,
                          tel = m.telefon,
                          tc = m.TC,
                          caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == m.CustID).ToplamBakiye
                      }).FirstOrDefault();


                fatura fatik = new fatura();
                fatik.bakiye = (decimal)hesap.Yekun;
                fatik.tamirci_id = tamirci_id;
                fatik.no = hesap.HesapID.ToString();
                fatik.taksit_no = 0;
                fatik.odenen = 0;
                fatik.Firma = hesap.Firma;
                fatik.tc = mu.tc;
                fatik.telefon = mu.tel;
                fatik.tutar = (decimal)hesap.Yekun;
                fatik.tamirci_maliyet = hesap.toplam_maliyet;
                fatik.MusteriID = hesap.MusteriID;
                fatik.tur = "Taksit";
                fatik.iptal = false;
                fatik.service_id = hesap.ServiceID;
                fatik.servicehesap_id = hesap.HesapID;
                fatik.sattis_tarih = DateTime.Now;
                fatik.son_odeme_tarihi = DateTime.Now;
                fatik.islem_tarihi = DateTime.Now;
                fatik.inserted = kullanici;
                db.faturas.Add(fatik);

                //buradaki servisid tahsilatta tahsilat türü için kullanıyorum, triggerla service hesapsa kart ise yazdırıyorum

                hesap.onay = true;
                hesap.updated = kullanici;
                hesap.Onay_tarih = DateTime.Now;
                KaydetmeIslemleri.kaydetR(db);
            }



        }


        public void servisKararOnayCalismaTipli(servicehesap hesap, string kullanici)
        {
            //tamirci_idyi ekleyeceğiz
            int? tamirci_id = hesap.tamirci_id;
            musteri_bilgileri mu = new musteri_bilgileri();
            if (hesap.cihaz_id != null)
            {
                var fifo = (from f in db.cihaz_fifos

                            where f.cihaz_id == hesap.cihaz_id && f.bakiye > 0
                            orderby f.tarih ascending
                            select f).FirstOrDefault();
                if (fifo != null)
                {
                    hesap.stok_id = fifo.id;
                    hesap.birim_maliyet = fifo.fiyat;
                    hesap.toplam_maliyet = hesap.adet * fifo.fiyat;

                    //tek taksit oluşturalım
                    mu = (from m in db.customers
                          where m.CustID == hesap.MusteriID
                          select new musteri_bilgileri
                          {
                              ad = m.Ad,
                              email = m.email,
                              tel = m.telefon,
                              tc = m.TC,
                              caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == m.CustID).ToplamBakiye
                          }).FirstOrDefault();


                    fatura fatik = new fatura();
                    fatik.bakiye = (decimal)hesap.Yekun;
                    fatik.tamirci_id = tamirci_id;
                    fatik.no = hesap.HesapID.ToString();
                    fatik.taksit_no = 0;
                    fatik.odenen = 0;
                    fatik.Firma = hesap.Firma;
                    fatik.tc = mu.tc;
                    fatik.telefon = mu.tel;
                    fatik.tutar = (decimal)hesap.Yekun;
                    fatik.tamirci_maliyet = hesap.toplam_maliyet;
                    fatik.MusteriID = hesap.MusteriID;
                    fatik.tur = "Taksit";
                    fatik.iptal = false;
                    fatik.service_id = hesap.ServiceID;
                    fatik.servicehesap_id = hesap.HesapID;
                    fatik.sattis_tarih = DateTime.Now;
                    fatik.son_odeme_tarihi = DateTime.Now;
                    fatik.islem_tarihi = DateTime.Now;
                    fatik.inserted = kullanici;

                    db.faturas.Add(fatik);

                    //buradaki servisid tahsilatta tahsilat türü için kullanıyorum, triggerla service hesapsa kart ise yazdırıyorum


                    hesap.onay = true;
                    hesap.updated = kullanici;
                    hesap.Onay_tarih = DateTime.Now;
                    KaydetmeIslemleri.kaydetR(db);

                }

            }
            else
            {
                if (hesap.makine_id != null)
                {
                    var masraf_tanimlar = db.makine_masraf_tanims2.Where(x => x.iptal == false && x.makine_id == hesap.makine_id && x.tarifeid == hesap.tarifeid).ToList();

                    decimal toplam_maliyet = 0;

                    foreach (var m in masraf_tanimlar)
                    {
                        makine_masraf_hesaps2 h = new makine_masraf_hesaps2();
                        h.aciklama = "";
                        h.birim = "BR";
                        h.serviceid = hesap.ServiceID;
                        h.calisma_saati = hesap.calisma_saati;
                        h.tarifekodu = hesap.tarife_kodu;
                        h.tarih = DateTime.Now;
                        h.dakika = hesap.dakika;

                        //decimal saat_basina_masraf = m.masraf_saat;
                        //burada sadece saat bölümüne yazıyoruz diğer yerler iptal
                        h.calisma_saati = hesap.calisma_saati;

                        h.miktar = m.masraf_saat * hesap.calisma_saati;
                        decimal mal = m.makine_masrafs.birim_maliyet * m.masraf_saat * hesap.calisma_saati;
                        h.tutar = mal;
                        toplam_maliyet = toplam_maliyet + mal;


                        h.gelir = (decimal)hesap.Yekun;
                        h.hesap_id = hesap.HesapID;
                        h.makine_id = (int)hesap.makine_id;
                        h.masraf_id = m.masraf_id;
                        h.son_sayac = hesap.yeni_sayac;

                        db.makine_masraf_hesaps2.Add(h);

                    }

                    var sayaclar = db.makine_servis_sayacs.Where(x => x.makine_id == hesap.makine_id).ToList();
                    foreach (var s in sayaclar)
                    {
                        s.sayac = s.sayac - hesap.toplam_sayac;
                    }

                    hesap.birim_maliyet = toplam_maliyet;
                    hesap.toplam_maliyet = toplam_maliyet;

                }
                //cihazlı değil
                //tek taksit oluşturalım
                mu = (from m in db.customers
                      where m.CustID == hesap.MusteriID
                      select new musteri_bilgileri
                      {
                          ad = m.Ad,
                          email = m.email,
                          tel = m.telefon,
                          tc = m.TC,
                          caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == m.CustID).ToplamBakiye
                      }).FirstOrDefault();


                fatura fatik = new fatura();
                fatik.bakiye = (decimal)hesap.Yekun;
                fatik.tamirci_id = tamirci_id;
                fatik.no = hesap.HesapID.ToString();
                fatik.taksit_no = 0;
                fatik.odenen = 0;
                fatik.Firma = hesap.Firma;
                fatik.tc = mu.tc;
                fatik.telefon = mu.tel;
                fatik.tutar = (decimal)hesap.Yekun;
                fatik.tamirci_maliyet = hesap.toplam_maliyet;
                fatik.MusteriID = hesap.MusteriID;
                fatik.tur = "Taksit";
                fatik.iptal = false;
                fatik.service_id = hesap.ServiceID;
                fatik.servicehesap_id = hesap.HesapID;
                fatik.sattis_tarih = DateTime.Now;
                fatik.son_odeme_tarihi = DateTime.Now;
                fatik.islem_tarihi = DateTime.Now;
                fatik.inserted = kullanici;
                db.faturas.Add(fatik);

                //buradaki servisid tahsilatta tahsilat türü için kullanıyorum, triggerla service hesapsa kart ise yazdırıyorum

                hesap.onay = true;
                hesap.updated = kullanici;
                hesap.Onay_tarih = DateTime.Now;
                //KaydetmeIslemleri.kaydetR(db);
            }



        }
        public int kararOnayTopluYeni(int servisid, string kullanici)
        {
            List<servicehesap> hesaplar = db.servicehesaps.Where(x => x.ServiceID == servisid && (x.iptal == false || x.iptal == null) && (x.onay == null || x.onay == false)).ToList();
            int i = 0;
            foreach (var hesap in hesaplar)
            {
                servisKararOnay(hesap, kullanici);
                i = i + 1;
            }

            return i;
        }


        public void servisEkleMakineli(int musID, string kullaniciID, string aciklama, string kimlik, string baslik, DateTime acilma_zamani, karar_wrap_makine karar, string kullanici)
        {
            TeknikServis.Radius.service servis = new TeknikServis.Radius.service();
            servis.CustID = (int)musID;
            servis.Baslik = baslik;
            servis.Firma = "firma";
            servis.olusturan_Kullanici = kullaniciID;
            servis.SonAtananID = "0";
            servis.AcilmaZamani = acilma_zamani;
            servis.iptal = false;
            servis.Aciklama = aciklama;
            servis.KapanmaZamani = acilma_zamani;
            servis.Servis_Kimlik_No = kimlik;
            servis.tip_id = -1;
            servis.inserted = kullanici;

            if (karar != null)
            {
                //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = servis.ServiceID;

                hesap.MusteriID = (int)musID;

                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = karar.islemParca;
                hesap.servis_kimlik = kimlik;
                hesap.cihaz_adi = karar.cihaz_adi;
                hesap.iptal = false;
                decimal tutar = (100 * karar.yekun) / (100 + karar.kdv);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * karar.kdv) / 100;
                hesap.Yekun = karar.yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.inserted = kullanici;
                hesap.TarihZaman = karar.tarih;
                hesap.tarifeid = karar.tarifeid;
                hesap.tarife_tipi = karar.tarife_tipi;
                hesap.sure_aciklama = karar.sure_aciklama;
                hesap.dakika = karar.dakika;
                hesap.makine_id = karar.makine_id;
                hesap.calisma_saati = karar.calisma_saati;
                hesap.baslangic = karar.baslangic;
                hesap.bitis = karar.bitis;
                hesap.baslangic_tarih = karar.baslangic_tarih;
                hesap.bitis_tarih = karar.bitis_tarih;
                hesap.tarife_kodu = karar.tarife_kodu;
                hesap.birim_fiyat = (decimal)(karar.yekun / karar.calisma_saati);
                hesap.toplam_sayac = karar.toplam_sayac;
                servis.servicehesaps.Add(hesap);

                servicemakine yeni = new servicemakine();
                yeni.calisma_saati = 0;
                yeni.toplam_sayac = 0;
                yeni.toplam_dakika = 0;
                yeni.iptal = false;
                yeni.makine_id = (int)karar.makine_id;
                yeni.maliyet = 0;
                yeni.serviceid = servis.ServiceID;
                yeni.tarife_kodu = karar.tarife_kodu;
                yeni.yekun = 0;
                servis.servicemakines.Add(yeni);

                db.services.Add(servis);

                KaydetmeIslemleri.kaydetR(db);


                //şimdi bu servis hesaplarının tamamını onaylayacağız
                List<servicehesap> hesaplar = db.servicehesaps.Where(x => x.servis_kimlik == kimlik && (x.iptal == false || x.iptal == null) && (x.onay == null || x.onay == false)).ToList();


                foreach (var hes in hesaplar)
                {
                    servisKararOnayCalismaTipli(hes, kullanici);

                }


                KaydetmeIslemleri.kaydetR(db);
            }

        }
        public void SerbestOnay(int hesapid)
        {
            var hes = db.servicehesap_ops.FirstOrDefault(x => x.HesapID == hesapid);
            if (hes != null)
            {
                hes.onay = true;
                hes.Onay_tarih = DateTime.Now;
                KaydetmeIslemleri.kaydetR(db);
            }
        }


        public string servisKararIptalR(int id, string kullanici)
        {
            string sonuc = "";
            TeknikServis.Radius.servicehesap hesap = tekHesapR(id);
            hesap.iptal = true;
            hesap.deleted = kullanici;

            KaydetmeIslemleri.kaydetR(db);
            return sonuc;

        }

        public void servisKapatR(int servisID, string kullanici)
        {
            var servis = db.services.FirstOrDefault(x => x.ServiceID == servisID);
            if (servis != null)
            {
                servis.KapanmaZamani = DateTime.Now;
                servis.kullanici = kullanici;
                KaydetmeIslemleri.kaydetR(db);
            }
        }
        #endregion

        #region iş emirleri
        public void Atama(string gorevli_id, string kimlik)
        {
            service s = db.services.FirstOrDefault(x => x.Servis_Kimlik_No == kimlik);
            if (s != null)
            {
                s.SonAtananID = gorevli_id;
                KaydetmeIslemleri.kaydetR(db);
            }
        }
        #endregion

        #region cihaz işlemleri

        public List<cihaz> CihazGoster()
        {
            return db.cihazs.Take(4).ToList();
        }
        public List<cihaz> CihazGoster(string arama_terimi)
        {
            return db.cihazs.Where(x => (x.cihaz_adi.Contains(arama_terimi) || x.aciklama.Contains(arama_terimi))).Take(4).ToList();
        }
        public bool CihazEkle(string cihaz_adi, string aciklama, string garanti_suresi)
        {
            bool flag = false;
            //bakalım bu adda cihaz var mı?
            int sure = Int32.Parse(garanti_suresi);
            cihaz c = db.cihazs.FirstOrDefault(x => x.cihaz_adi == cihaz_adi);
            if (c == null)
            {
                cihaz yeni = new cihaz();
                yeni.cihaz_adi = cihaz_adi;
                yeni.aciklama = aciklama;
                yeni.garanti_suresi = sure;
                yeni.Firma = "firma";

                db.cihazs.Add(yeni);
                KaydetmeIslemleri.kaydetR(db);
                flag = true;
            }
            return flag;
        }
        #endregion
        #region ürün işlemleri




        public void emanetUrunVerR(int musteriID, string acilama, string kullanici)
        {
            TeknikServis.Radius.yedek_uruns yedek = new TeknikServis.Radius.yedek_uruns();
            yedek.musteri_id = musteriID;
            yedek.Firma = "firma";
            yedek.urun_aciklama = acilama;
            yedek.tarih_verilme = DateTime.Now;
            yedek.inserted = kullanici;
            db.yedek_uruns.Add(yedek);
            KaydetmeIslemleri.kaydetR(db);

        }

        public TeknikServis.Radius.yedek_uruns tekEmanetR(int emanetID)
        {
            return (from u in db.yedek_uruns
                    where u.yedek_id == emanetID
                    select u).FirstOrDefault();
        }

        public void emanetAlR(int id, string kullanici)
        {
            TeknikServis.Radius.yedek_uruns emanet = tekEmanetR(id);
            emanet.tarih_donus = DateTime.Now;
            emanet.updated = kullanici;
            KaydetmeIslemleri.kaydetR(db);

        }



        //bayi için
        public List<yedekUrunRepo> emanettekiUrunlerimizHepsiR()
        {
            return (from u in db.yedek_uruns
                    where u.tarih_donus == null
                    orderby u.tarih_verilme descending
                    select new yedekUrunRepo
                    {

                        musteriAdi = u.customer.Ad,
                        musteriID = u.musteri_id,
                        urunAciklama = u.urun_aciklama,
                        verilmeTarihi = u.tarih_verilme,
                        yedekID = u.yedek_id,
                        donusTarih = u.tarih_donus,
                        donmeDurumu = u.tarih_donus == null ? "Müşteride" : "Döndü",
                        kullanici = u.inserted
                    }).ToList();
        }

        public List<yedekUrunRepo> emanettekiUrunlerimizHepsiR(int cust_id)
        {
            return (from u in db.yedek_uruns
                    where u.tarih_donus == null && u.musteri_id == cust_id
                    orderby u.tarih_verilme descending
                    select new yedekUrunRepo
                    {
                        musteriAdi = u.customer.Ad,
                        musteriID = u.musteri_id,
                        urunAciklama = u.urun_aciklama,
                        verilmeTarihi = u.tarih_verilme,
                        yedekID = u.yedek_id,
                        donusTarih = u.tarih_donus,
                        donmeDurumu = u.tarih_donus == null ? "Müşteride" : "Döndü",
                        kullanici = u.inserted
                    }).ToList();
        }

        #endregion


        //manager ve kullanici için
        public List<cariHesapRepo> butunBorcuOlanHesaplarR(string sonMesaj, string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                //son mesaja göre  sırala
                if (String.IsNullOrEmpty(sonMesaj) || sonMesaj.Equals("hepsi"))
                {
                    return (from h in db.carihesaps
                            where h.MusteriID > 0 && h.ToplamBakiye > 0
                            select new cariHesapRepo
                            {
                                musteriID = h.MusteriID,
                                musteriAdi = h.adi,
                                tel = h.telefon,
                                son_mesaj = h.son_mesaj,
                                netBakiye = h.ToplamBakiye,
                                netBorclanma = h.NetBorc,
                                netAlacak = h.NetAlacak

                            }
                           ).ToList();
                }
                else
                {
                    return (from h in db.carihesaps
                            where h.MusteriID > 0 && h.ToplamBakiye > 0
                            orderby h.son_mesaj ascending
                            select new cariHesapRepo
                            {
                                musteriID = h.MusteriID,
                                musteriAdi = h.adi,
                                tel = h.telefon,
                                son_mesaj = h.son_mesaj,
                                netBakiye = h.ToplamBakiye,
                                netBorclanma = h.NetBorc,
                                netAlacak = h.NetAlacak

                            }
                           ).ToList();
                }
            }
            else
            {
                //son mesaja göre  sırala
                if (String.IsNullOrEmpty(sonMesaj) || sonMesaj.Equals("hepsi"))
                {
                    return (from h in db.carihesaps
                            where h.MusteriID > 0 && h.ToplamBakiye > 0 && h.adi.Contains(s)
                            select new cariHesapRepo
                            {
                                musteriID = h.MusteriID,
                                musteriAdi = h.adi,
                                tel = h.telefon,
                                son_mesaj = h.son_mesaj,
                                netBakiye = h.ToplamBakiye,
                                netBorclanma = h.NetBorc,
                                netAlacak = h.NetAlacak

                            }
                           ).ToList();
                }
                else
                {
                    return (from h in db.carihesaps
                            where h.MusteriID > 0 && h.ToplamBakiye > 0 && h.adi.Contains(s)
                            orderby h.son_mesaj ascending
                            select new cariHesapRepo
                            {
                                musteriID = h.MusteriID,
                                musteriAdi = h.adi,
                                tel = h.telefon,
                                son_mesaj = h.son_mesaj,
                                netBakiye = h.ToplamBakiye,
                                netBorclanma = h.NetBorc,
                                netAlacak = h.NetAlacak

                            }
                           ).ToList();
                }
            }


        }
        //iş emirleri eklenecek
        // emirler servislere bağlı olacak
        //böylece servislerle ilgili ek emir düzenlenebilecek
        // kullanıcı emri yerine getirrise onaylayacak
        // kullanıcının işlemi servis detayı olarak otomatik eklenecek
    }
    public class Musteri_Mesaj
    {
        public string musteri_adi { get; set; }
        public string telefon { get; set; }
        public string email { get; set; }
        public string islemler { get; set; }
        public string tutar { get; set; }
        public string tc { get; set; }
        public string username { get; set; }
        public decimal caribakiye { get; set; }
    }

    public class karar_wrap
    {
        public string islemParca { get; set; }
        public decimal tutar { get; set; }
        public decimal kdv { get; set; }
        public decimal yekun { get; set; }
        public string aciklama { get; set; }
        public int? cihaz_id { get; set; }
        public string cihaz_adi { get; set; }
        public decimal adet { get; set; }
        public int? cihaz_gsure { get; set; }
        public string birim { get; set; }
        public bool sinirsiz { get; set; }
    }
    public class karar_wrap_makine
    {
        public string islemParca { get; set; }
        public string tarife_kodu { get; set; }
        public decimal baslangic { get; set; }
        public decimal bitis { get; set; }
        public decimal calisma_saati { get; set; }
        public decimal yeni_sayac { get; set; }
        public decimal toplam_sayac { get; set; }
        public int dakika { get; set; }
        public DateTime baslangic_tarih { get; set; }
        public DateTime bitis_tarih { get; set; }
        public decimal tutar { get; set; }
        public decimal kdv { get; set; }
        public decimal yekun { get; set; }
        public string aciklama { get; set; }
        public string sure_aciklama { get; set; }
        public int? makine_id { get; set; }
        public string cihaz_adi { get; set; }
        public DateTime tarih { get; set; }
        public string tarife_tipi { get; set; }
        public int? tarifeid { get; set; }

    }
    public class musteri_bilgileri
    {
        public string email { get; set; }
        public string ad { get; set; }
        public string tel { get; set; }
        public string tc { get; set; }
        public decimal caribakiye { get; set; }
        public string durum { get; set; }

    }
}
