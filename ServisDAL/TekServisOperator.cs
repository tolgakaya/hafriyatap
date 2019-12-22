using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;
using ServisDAL.Repo;

namespace ServisDAL
{
    public class TekServisOperator
    {
        radiusEntities dc;
        int servisid;
        string kimlik;
        string kullanici;
        public TekServisOperator(radiusEntities dc, int servisid, string kimlik, string kullanici)
        {

            this.servisid = servisid;
            this.kimlik = kimlik;
            this.kullanici = kullanici;
            this.dc = dc;

        }

        public ServisInfo servis()
        {
            ServisInfo s = new ServisInfo();
            s.kararlar = this.kararlar();
            s.genel = this.genel();

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
                        where s.ServiceID == servisid && s.iptal == false && s.kullanici == kullanici
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
                            toplam_maliyet = s.toplam_maliyet,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                        }).ToList();
            }
            else
            {
                return (from s in dc.servicehesaps
                        where s.service.Servis_Kimlik_No == kimlik && s.iptal == false && s.kullanici == kullanici
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
                            yekun =(decimal)s.Yekun,
                            cihaz = s.cihaz_adi,
                            birim_maliyet = s.birim_maliyet,
                            toplam_maliyet = s.toplam_maliyet,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                        }).ToList();
            }


        }


    }
}
