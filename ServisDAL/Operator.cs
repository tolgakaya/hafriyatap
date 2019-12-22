using ServisDAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class Operator
    {

        private radiusEntities dc;
        private string kullanici;

        public Operator(radiusEntities dc, string kullanici)
        {
            this.dc = dc;
            this.kullanici = kullanici;
        }


        public List<ServisHesapRepo> SerbestCalismaListesi()
        {

            return (from s in dc.servicehesap_ops
                    where s.iptal == false && s.onay == false
                    orderby s.TarihZaman descending
                    select new ServisHesapRepo
                    {
                        hesapID = s.HesapID,
                        aciklama = s.sure_aciklama,
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
                        baslangic = s.baslangic,
                        son = s.bitis,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                    }).ToList();

        }
        public void serbest_sil(int hesapid)
        {
            var hes = dc.servicehesap_ops.FirstOrDefault(x => x.HesapID == hesapid);
            if (hes!=null)
            {
                hes.iptal = true;
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
        public ServisHesapRepo SerbestCalismaTek(int hesapid)
        {

            return (from s in dc.servicehesap_ops
                    where s.iptal == false && s.HesapID == hesapid
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
                        tarifeid = (int)s.tarifeid,
                        onaylimi = s.onay,
                        onayTarih = s.Onay_tarih,
                        tarihZaman = s.TarihZaman,
                        tutar = s.Tutar,
                        yekun = (decimal)s.Yekun,
                        cihaz = s.cihaz_adi,
                        birim_maliyet = s.birim_maliyet,
                        toplam_maliyet = s.toplam_maliyet,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                        baslangic = s.baslangic,
                        son = s.bitis

                    }).FirstOrDefault();

        }
        //makinede bütün operatörleri çıkararak yeni operatör atama
        public void MakineOperatorKontrollu(int makine_id)
        {
            //makinede operatör var mı bakalım

            //cikarildi=false ekleyelim
            var m = dc.makine_kullanicis.Where(x => x.makine_id == makine_id && x.iptal == false && x.cikarma == null).ToList();
            if (m.Count > 0)
            {
                //hepsini çıkaralım
                foreach (var k in m)
                {
                    k.cikarma = DateTime.Now;
                }

                //yeni atamayı yapalım

            }

            makine_kullanicis mk = new makine_kullanicis();
            mk.atama = DateTime.Now;
            mk.iptal = false;
            mk.kullanici = kullanici;
            mk.makine_id = makine_id;
            dc.makine_kullanicis.Add(mk);
            KaydetmeIslemleri.kaydetR(dc);
        }

        //makinede yeni operatör atama
        public void MakineOperatorAta(int makine_id)
        {
            //makinede operatör var mı bakalım
            var m = dc.makine_kullanicis.FirstOrDefault(x => x.makine_id == makine_id && x.iptal == false && x.cikarma == null && x.kullanici == kullanici);
            if (m == null)
            {
                //cikarildi=false ekleyelim
                makine_kullanicis mk = new makine_kullanicis();
                mk.atama = DateTime.Now;
                mk.iptal = false;
                mk.kullanici = kullanici;
                mk.makine_id = makine_id;
                dc.makine_kullanicis.Add(mk);
                KaydetmeIslemleri.kaydetR(dc);

            }

        }

        public void MakineOperatorCikar(int id)
        {
            //makinede operatör var mı bakalım
            var m = dc.makine_kullanicis.FirstOrDefault(x => x.id == id);
            if (m != null)
            {
                //cikarildi=false ekleyelim
                m.cikarma = DateTime.Now;
                KaydetmeIslemleri.kaydetR(dc);

            }

        }

        public void MakineOperatorIptal(int id)
        {
            //makinede operatör var mı bakalım
            var m = dc.makine_kullanicis.FirstOrDefault(x => x.id == id);
            if (m != null)
            {
                //cikarildi=false ekleyelim
                m.iptal = true;
                KaydetmeIslemleri.kaydetR(dc);

            }

        }


        // servis kullanıcı
        public void ServisOperatorAta(int servis_id)
        {
            //makinede operatör var mı bakalım
            var m = dc.servis_kullanicis.FirstOrDefault(x => x.servis_id == servis_id && x.iptal == false && x.cikarma == null && x.kullanici == kullanici);
            if (m == null)
            {
                //cikarildi=false ekleyelim
                servis_kullanicis mk = new servis_kullanicis();
                mk.atama = DateTime.Now;
                mk.iptal = false;
                mk.kullanici = kullanici;
                mk.servis_id = servis_id;
                dc.servis_kullanicis.Add(mk);
                KaydetmeIslemleri.kaydetR(dc);

            }

        }

        public void ServisOperatorCikar(int id)
        {
            //makinede operatör var mı bakalım
            var m = dc.servis_kullanicis.FirstOrDefault(x => x.id == id);

            if (m != null)
            {
                m.cikarma = DateTime.Now;
                KaydetmeIslemleri.kaydetR(dc);
            }

        }

        public void ServisOperatorIptal(int id)
        {
            var m = dc.servis_kullanicis.FirstOrDefault(x => x.id == id);
            if (m != null)
            {
                //cikarildi=false ekleyelim
                m.iptal = true;
                KaydetmeIslemleri.kaydetR(dc);

            }
        }


    }
}
