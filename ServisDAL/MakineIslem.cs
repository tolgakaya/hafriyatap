using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;


namespace ServisDAL
{
    public class MakineIslem
    {
        radiusEntities dc;
        public MakineIslem(radiusEntities dc)
        {

            this.dc = dc;
        }
        public List<Makine> makineler(string terim = "")
        {
            if (terim == "")
            {
                return (from m in dc.makine_caris
                        where m.iptal == false
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

                        }).ToList();
            }
            else
            {
                return (from m in dc.makine_caris
                        where m.iptal == false && (m.adi.Contains(terim) || m.plaka.Contains(terim) || m.aciklama.Contains(terim))
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
                        }).ToList();
            }

        }

        public List<Tarife> tarifeler(int makine_id)
        {
            return (from t in dc.kiralama_tarifes
                    where t.makine_id == makine_id && t.iptal == false
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
        public List<Tarife> tarifeler_saatlik(int makine_id)
        {
            return (from t in dc.kiralama_tarifes
                    where t.makine_id == makine_id && t.iptal == false && t.saatlik == true
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
        public Tarife tarife_tek(int id)
        {
            return (from t in dc.kiralama_tarifes
                    where t.id == id && t.iptal == false
                    select new Tarife
                    {
                        id = t.id,
                        ad = t.tarife_kodu + "-" + t.calisma_tipi,
                        tarife_kodu = t.tarife_kodu,
                        calisma_tipi = t.calisma_tipi,
                        tutar = t.tutar,
                        saatlik = t.saatlik
                    }).FirstOrDefault();
        }
        public int tarife_ekle(int makineid, string tarife_kodu, string calisma_tipi, decimal tutar)
        {
            //mevcut tarife var mı bakalım

            bool saatlik = false;
            if (tarife_kodu == "saat")
            {
                saatlik = true;
            }
            var t = dc.kiralama_tarifes.Where(x => x.iptal == false && x.makine_id == makineid && x.tarife_kodu == tarife_kodu.ToLower().Trim() && x.calisma_tipi == calisma_tipi.ToLower().Trim()).FirstOrDefault();
            if (t == null)
            {
                kiralama_tarifes k = new kiralama_tarifes();
                k.calisma_tipi = calisma_tipi.ToLower().Trim();
                k.iptal = false;
                k.makine_id = makineid;
                k.saatlik = saatlik;
                k.tarife_kodu = tarife_kodu;
                k.tutar = tutar;
                dc.kiralama_tarifes.Add(k);
                KaydetmeIslemleri.kaydetR(dc);
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public void tarife_update(int tarifeid, string tarife_kodu, string calisma_tipi, decimal tutar)
        {
            //mevcut tarife var mı bakalım

            bool saatlik = false;
            if (tarife_kodu == "saat")
            {
                saatlik = true;
            }
            var t = dc.kiralama_tarifes.Where(x => x.id == tarifeid).FirstOrDefault();
            if (t != null)
            {
                kiralama_tarifes k = new kiralama_tarifes();
                k.calisma_tipi = calisma_tipi.ToLower().Trim();
                k.saatlik = saatlik;
                k.tarife_kodu = tarife_kodu;
                k.tutar = tutar;

                KaydetmeIslemleri.kaydetR(dc);
            }
        }

        public void tarife_iptal(int tarifeid)
        {
            //mevcut tarife var mı bakalım

            var t = dc.kiralama_tarifes.Where(x => x.id == tarifeid).FirstOrDefault();
            if (t != null)
            {
                t.iptal = true;
                KaydetmeIslemleri.kaydetR(dc);
            }
        }

        public List<Makine> makinelerOperator(string terim, string kullanici)
        {
            if (terim == "")
            {
                return (from m in dc.makine_kullanicis
                        where m.iptal == false && m.cikarma == null && m.kullanici == kullanici
                        select new Makine
                        {
                            makine_id = m.makine_id,
                            adi = m.makine_caris.adi,
                            plaka = m.makine_caris.plaka,
                            aciklama = m.makine_caris.aciklama,
                            son_sayac = m.makine_caris.son_sayac,
                            tarife_ay = m.makine_caris.tarife_ay,
                            tarife_gun = m.makine_caris.tarife_gun,
                            tarife_hafta = m.makine_caris.tarife_hafta,
                            tarife_saat = m.makine_caris.tarife_saat,
                        }).ToList();
            }
            else
            {
                return (from m in dc.makine_kullanicis
                        where m.iptal == false && m.cikarma == null && m.kullanici == kullanici && (m.makine_caris.adi.Contains(terim) || m.makine_caris.plaka.Contains(terim) || m.makine_caris.aciklama.Contains(terim))
                        select new Makine
                        {
                            makine_id = m.makine_id,
                            adi = m.makine_caris.adi,
                            plaka = m.makine_caris.plaka,
                            aciklama = m.makine_caris.aciklama,
                            son_sayac = m.makine_caris.son_sayac,
                            tarife_ay = m.makine_caris.tarife_ay,
                            tarife_gun = m.makine_caris.tarife_gun,
                            tarife_hafta = m.makine_caris.tarife_hafta,
                            tarife_saat = m.makine_caris.tarife_saat,
                        }).ToList();
            }

        }
        public Makine tekmakine(int makine_id)
        {
            return (from m in dc.makine_caris
                    where m.makine_id == makine_id
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

        public GirisOzet Girisler(DateTime bas, DateTime son, string makine_id, string masraf_id)
        {
            GirisOzet g = new GirisOzet();

            List<MakineGiris> liste = MakineGirisler(bas, son, makine_id, masraf_id);
            int adet = liste.Count;

            g.adet = adet;

            if (adet > 0)
            {
                decimal tutar = liste.Sum(x => x.tutar);
                decimal miktar = liste.Sum(x => x.miktar);

                g.tutar = tutar;
                g.miktar = miktar;
            }

            g.liste = liste;

            return g;

        }
        public void GirisIptal(int id)
        {
            var giris = dc.makine_masraf_girislers.FirstOrDefault(x => x.id == id);
            if (giris != null)
            {
                giris.iptal = true;
                KaydetmeIslemleri.kaydetR(dc);
            }
        }

        public List<MakineGiris> OperatorGirisleri(string makine_id, string kullanici)
        {

            int makineid = Int32.Parse(makine_id);
            return (from m in dc.makine_masraf_girislers
                    where m.iptal == false && m.@operator == kullanici && m.makine_id == makineid
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
        private List<MakineGiris> MakineGirisler(DateTime bas, DateTime son, string makine_id, string masraf_id)
        {
            if (!String.IsNullOrEmpty(makine_id))
            {
                int makineid = Int32.Parse(makine_id);
                return (from m in dc.makine_masraf_girislers
                        where m.iptal == false && m.tarih >= bas && m.tarih <= m.tarih && m.makine_id == makineid
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
            else if (!String.IsNullOrEmpty(masraf_id))
            {
                int masrafid = Int32.Parse(masraf_id);

                return (from m in dc.makine_masraf_girislers
                        where m.iptal == false && m.tarih >= bas && m.tarih <= m.tarih && m.masraf_id == masrafid
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
            else
            {
                return (from m in dc.makine_masraf_girislers
                        where m.iptal == false && m.tarih >= bas && m.tarih <= m.tarih
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

        }
        public void masraf_girisi(List<MakineGiris> liste)
        {
            if (liste.Count > 0)
            {
                foreach (var m in liste)
                {
                    makine_masraf_girislers g = new makine_masraf_girislers();
                    g.aciklama = m.aciklama;
                    g.belge_no = m.belge_no;
                    g.iptal = false;
                    g.makine_id = m.makine_id;
                    g.masraf_id = m.masraf_id;
                    g.miktar = m.miktar;
                    g.tarih = m.tarih;
                    g.tutar = m.tutar;
                    g.birim = m.birim;
                    g.sifirla = m.sifirla;
                    g.@operator = m.kullanici;
                    dc.makine_masraf_girislers.Add(g);
                }
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
      
    
        public bool tanimekle_tipli(int makineid, int masrafid, int tarifeid, decimal masraf_saat, string aciklama, string birim)
        {
            var tanim = dc.makine_masraf_tanims2.Where(x => x.iptal == false && x.tarifeid == tarifeid && x.masraf_id == masrafid).FirstOrDefault();
            if (tanim == null)
            {
                makine_masraf_tanims2 t = new makine_masraf_tanims2();
                t.iptal = false;
                t.makine_id = makineid;
                t.masraf_id = masrafid;
                t.masraf_saat = masraf_saat;
                t.birim = birim;
                t.aciklama = aciklama;
                t.tarifeid = tarifeid;
                dc.makine_masraf_tanims2.Add(t);
                KaydetmeIslemleri.kaydetR(dc);
                return true;
            }
            else
            {
                return false;
            }


        }
        public bool tanimupdate(int id, decimal masraf_saat, string aciklama)
        {
            var tanim = dc.makine_masraf_tanims2.Where(x => x.tanim_id == id).FirstOrDefault();
            if (tanim != null)
            {

                tanim.masraf_saat = masraf_saat;
                tanim.aciklama = aciklama;

                KaydetmeIslemleri.kaydetR(dc);
                return true;
            }
            else
            {
                return false;
            }


        }
       
        public bool tanimdelete_tipli(int id)
        {
            var tanim = dc.makine_masraf_tanims2.Where(x => x.tanim_id == id).FirstOrDefault();
            if (tanim != null)
            {
                tanim.iptal = true;
                KaydetmeIslemleri.kaydetR(dc);
                return true;
            }
            else
            {
                return false;
            }

        }

        public List<ServisSayaci> sayaclar(int makineid)
        {
            return (from m in dc.makine_servis_sayacs
                    where m.iptal == false && m.makine_id == makineid
                    select new ServisSayaci
                    {
                        sayac_id = m.sayac_id,
                        makine_id = m.makine_id,
                        makine_plaka = m.makine_caris.adi + "-" + m.makine_caris.plaka,
                        masraf_id = m.masraf_id,
                        masraf = m.makine_masrafs.adi,
                        sayac = m.sayac,
                        sayac_tanim = m.sayac_tanim,
                        birim = m.birim
                    }).ToList();
        }
        public bool sayacekle(int makineid, int masrafid, decimal sayac, string birim, decimal sayac_alarm)
        {
            var tanim = dc.makine_servis_sayacs.Where(x => x.iptal == false && x.makine_id == makineid && x.masraf_id == masrafid).FirstOrDefault();
            if (tanim == null)
            {
                makine_servis_sayacs t = new makine_servis_sayacs();
                t.iptal = false;
                t.makine_id = makineid;
                t.masraf_id = masrafid;
                t.sayac = sayac;
                t.birim = birim;
                t.sayac_tanim = sayac;
                t.alarm_sayac = sayac_alarm;
                dc.makine_servis_sayacs.Add(t);
                KaydetmeIslemleri.kaydetR(dc);
                return true;
            }
            else
            {
                return false;
            }


        }
        public bool sayacupdate(int id, decimal sayac)
        {
            var tanim = dc.makine_servis_sayacs.Where(x => x.sayac_id == id).FirstOrDefault();
            if (tanim != null)
            {
                decimal eski_sayac = tanim.sayac;//bin
                decimal eski_tanim = tanim.sayac_tanim;

                decimal kullanilan_sure = eski_tanim - eski_sayac;

                decimal yeni_sayac = sayac - kullanilan_sure;
                tanim.sayac = yeni_sayac;
                tanim.sayac_tanim = sayac;
                KaydetmeIslemleri.kaydetR(dc);
                return true;
            }
            else
            {
                return false;
            }


        }
        public bool sayacdelete(int id)
        {
            var tanim = dc.makine_servis_sayacs.Where(x => x.sayac_id == id).FirstOrDefault();
            if (tanim != null)
            {
                tanim.iptal = true;
                KaydetmeIslemleri.kaydetR(dc);
                return true;
            }
            else
            {
                return false;
            }

        }


    }

    public class Tarife
    {
        public int id { get; set; }
        public string ad { get; set; }
        public string tarife_kodu { get; set; }
        public string calisma_tipi { get; set; }
        public decimal tutar { get; set; }
        public bool saatlik { get; set; }

    }
    public class Makine
    {
        public int makine_id { get; set; }
        public string adi { get; set; }
        public string plaka { get; set; }
        public string aciklama { get; set; }
        public decimal son_sayac { get; set; }
        public decimal tarife_saat { get; set; }
        public decimal tarife_gun { get; set; }
        public decimal tarife_hafta { get; set; }
        public decimal tarife_ay { get; set; }
        public decimal toplam_calisma_saat { get; set; }
        public decimal toplam_calisma_gun { get; set; }
        public decimal toplam_calisma_hafta { get; set; }
        public decimal toplam_calisma_ay { get; set; }
        public decimal toplam_masraf_teorik { get; set; }
        public decimal toplam_masraf_gercek { get; set; }
        public decimal toplam_gelir { get; set; }
        public decimal servis_sayaci { get; set; }
        public bool iptal { get; set; }
    }

    public class MasrafHesap
    {
        public int id { get; set; }
        public int serviceid { get; set; }
        public int hesap_id { get; set; }
        public int makine_id { get; set; }
        public string makine_plaka { get; set; }
        public int masraf_id { get; set; }
        public string masraf { get; set; }
        public int dakika { get; set; }
        public int son_sayac { get; set; }
        public decimal calisma_saati { get; set; }
        public decimal calisma_gun { get; set; }
        public decimal calisma_hafta { get; set; }
        public decimal calisma_ay { get; set; }
        public decimal miktar { get; set; }
        public string birim { get; set; }
        public decimal tutar { get; set; }
        public decimal gelir { get; set; }
        public string aciklama { get; set; }
        public DateTime tarih { get; set; }
    }
    public class GirisOzet
    {
        public List<MakineGiris> liste { get; set; }
        public int adet { get; set; }
        public decimal miktar { get; set; }
        public decimal tutar { get; set; }
    }
    public class MakineGiris
    {
        public int id { get; set; }
        public int makine_id { get; set; }
        public string makine_adi { get; set; }
        public string makine_plaka { get; set; }
        public int masraf_id { get; set; }
        public string masraf_adi { get; set; }
        public decimal miktar { get; set; }
        public string birim { get; set; }
        public decimal tutar { get; set; }
        public System.DateTime tarih { get; set; }
        public string aciklama { get; set; }
        public string belge_no { get; set; }
        public bool sifirla { get; set; }
        public string kullanici { get; set; }

    }

    public class MakineMasrafTanimi
    {
        public int tanim_id { get; set; }
        public int makine_id { get; set; }
        public string makine_plaka { get; set; }
        public int masraf_id { get; set; }
        public string masraf { get; set; }
        public decimal masraf_saat { get; set; }
        public string birim { get; set; }
        public string tarife_kodu { get; set; }
        //public int servis_sayaci { get; set; }
        public string aciklama { get; set; }
        public int? tarifeid { get; set; }
        public bool calisma_tipli { get; set; }
    }
    public class ServisSayaci
    {
        public int sayac_id { get; set; }
        public int makine_id { get; set; }
        public string makine_plaka { get; set; }
        public int masraf_id { get; set; }
        public string masraf { get; set; }
        public decimal sayac { get; set; }
        public decimal sayac_tanim { get; set; }
        public string birim { get; set; }

    }
}
