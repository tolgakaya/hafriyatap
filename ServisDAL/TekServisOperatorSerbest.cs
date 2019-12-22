using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;
using ServisDAL.Repo;

namespace ServisDAL
{
    public class TekServisOperatorSerbest
    {
        radiusEntities dc;
        string kullanici;
        public TekServisOperatorSerbest(radiusEntities dc, string kullanici)
        {

            this.kullanici = kullanici;
            this.dc = dc;

        }

        public ServisInfo servis()
        {
            ServisInfo s = new ServisInfo();
            s.kararlar = this.kararlar();

            return s;
        }

        private List<ServisHesapRepo> kararlar()
        {

            return (from s in dc.servicehesap_ops
                    where s.iptal == false && s.kullanici == kullanici && s.onay == false
                    orderby s.TarihZaman descending
                    select new ServisHesapRepo
                    {
                        hesapID = s.HesapID,
                        aciklama = s.Aciklama,
                        islemParca = s.IslemParca,
                        kdv = s.KDV,
                        makine_id = s.makine_id,
                        onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                        dakika = s.makine_id == null ? (s.adet.ToString() + " " + s.birim) : (s.dakika.ToString() + " dakika"),
                        sure = s.calisma_saati,
                        tarife = s.tarife_kodu,
                        onaylimi = s.onay,
                        onayTarih = s.Onay_tarih,
                        tarihZaman = s.TarihZaman,
                        tutar = s.Tutar,
                        yekun = (decimal)s.Yekun,
                        cihaz = s.cihaz_adi,
                        birim_maliyet = s.birim_maliyet,
                        toplam_maliyet = s.toplam_maliyet,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                    }).ToList();

        }


    }
}
