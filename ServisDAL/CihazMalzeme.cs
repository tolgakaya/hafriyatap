using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeknikServis.Radius;


namespace ServisDAL
{
    public class CihazMalzeme
    {


        radiusEntities dc;
        public CihazMalzeme(radiusEntities dc)
        {

            this.dc = dc;
        }

        public List<cihaz_birims> birimler()
        {
            return dc.cihaz_birims.ToList();
        }
        public List<cihaz_rp> CihazListesiComp()
        {
            return (from c in dc.cihazs

                    select new cihaz_rp
                    {
                        ID = c.ID,
                        cihaz_adi = c.cihaz_adi,
                        aciklama = c.aciklama,
                        seri_no = c.seri_no,
                        garanti_suresi = c.garanti_suresi,
                        alinan_birim = c.cihaz_birims.birim,
                        satilan_birim = c.cihaz_birims1.birim

                    }).Take(3).ToList();
        }

        public List<cihaz_rp> CihazListesiSinirli()
        {
            return (from c in dc.cihazs
                    where c.sinirsiz == false
                    select new cihaz_rp
                    {
                        ID = c.ID,
                        cihaz_adi = c.cihaz_adi,
                        aciklama = c.aciklama,
                        seri_no = c.seri_no,
                        garanti_suresi = c.garanti_suresi,
                        alinan_birim = c.cihaz_birims.birim,
                        satilan_birim = c.cihaz_birims1.birim

                    }).Take(3).ToList();
        }
        public List<makine_masrafs> MasrafListesi(string terim)
        {
            if (String.IsNullOrEmpty(terim))
            {
                return dc.makine_masrafs.Where(x => x.iptal == false).Take(3).ToList();
            }
            else
            {
                return dc.makine_masrafs.Where(x => x.iptal == false && x.adi.Contains(terim)).Take(3).ToList();
            }

        }

        public List<makine_masrafs> MasrafListe()
        {

            return dc.makine_masrafs.Where(x => x.iptal == false).ToList();


        }

        public List<Masraf> MasrafListesi()
        {

            return (from m in dc.makine_masrafs
                    where m.iptal == false
                    select new Masraf
                    {
                        MasrafID = m.MasrafID,
                        adi = m.adi,
                        aciklama = m.aciklama,
                        giris = m.giris,
                        cikis = m.cikis,
                        bakiye = m.bakiye,
                        birim_maliyet = m.birim_maliyet
                    }).Take(3).ToList();

        }
        public List<cihaz_rp> CihazListesiComp(string terim)
        {

            return (from c in dc.cihazs

                    where (c.cihaz_adi.Contains(terim) || c.aciklama.Contains(terim))
                    select new cihaz_rp
                    {
                        ID = c.ID,
                        cihaz_adi = c.cihaz_adi,
                        aciklama = c.aciklama,
                        seri_no = c.seri_no,
                        garanti_suresi = c.garanti_suresi,
                        alinan_birim = c.cihaz_birims.birim,
                        satilan_birim = c.cihaz_birims1.birim
                    }).Take(3).ToList();


        }
        public List<cihaz_rp> CihazListesiSinirli(string terim)
        {

            return (from c in dc.cihazs

                    where (c.cihaz_adi.Contains(terim) || c.aciklama.Contains(terim)) && c.sinirsiz == false
                    select new cihaz_rp
                    {
                        ID = c.ID,
                        cihaz_adi = c.cihaz_adi,
                        aciklama = c.aciklama,
                        seri_no = c.seri_no,
                        garanti_suresi = c.garanti_suresi,
                        alinan_birim = c.cihaz_birims.birim,
                        satilan_birim = c.cihaz_birims1.birim
                    }).Take(3).ToList();


        }

        //public decimal fiyat(int cihazid)
        //{
        //    return (decimal)(dc.cihaz_stoks.FirstOrDefault(x => x.cihaz_id == cihazid).satis_fiyati);
        //}

        public cihaz_rp tekCihaz(int cihazid)
        {
            return (from c in dc.cihazs
                    from s in dc.cihaz_stoks
                    where c.ID == cihazid && s.cihaz_id == cihazid
                    select new cihaz_rp
                    {
                        ID = c.ID,
                        grupid = c.grupid,
                        cihaz_adi = c.cihaz_adi,
                        aciklama = c.aciklama,
                        seri_no = c.seri_no,
                        garanti_suresi = c.garanti_suresi,
                        bakiye = s.bakiye,
                        cikis = s.cikis,
                        giris = s.giris,
                        fiyat = (Decimal)s.son_alis_fiyati,
                        satis = (Decimal)s.satis_fiyati,
                        barkod = c.barkod,
                        alinan_birim = c.cihaz_birims.birim,
                        satilan_birim = c.cihaz_birims1.birim,
                        alinan_birim_id = c.alinan_birim,
                        satilan_birim_id = c.satilan_birim,
                        sinirsiz = c.sinirsiz == true ? "sinirsiz" : "sinirli"
                    }).FirstOrDefault();
        }
        public List<cihaz_rp> CihazListesi2(string terim = "")
        {
            if (terim == "")
            {
                return (from c in dc.cihazs
                        from s in dc.cihaz_stoks
                        where c.ID == s.cihaz_id
                        select new cihaz_rp
                        {
                            ID = c.ID,
                            grupid = c.grupid,
                            cihaz_adi = c.cihaz_adi,
                            aciklama = c.aciklama,
                            seri_no = c.seri_no,
                            garanti_suresi = c.garanti_suresi,
                            bakiye = s.bakiye,
                            cikis = s.cikis,
                            giris = s.giris,
                            fiyat = (Decimal)s.son_alis_fiyati,
                            satis = (Decimal)s.satis_fiyati,
                            barkod = c.barkod,
                            alinan_birim = c.cihaz_birims.birim,
                            satilan_birim = c.cihaz_birims1.birim,
                            alinan_birim_id = c.alinan_birim,
                            satilan_birim_id = c.satilan_birim,
                            sinirsiz = c.sinirsiz == true ? "sinirsiz" : "sinirli"
                        }).Take(10).ToList();
            }
            else
            {
                return (from c in dc.cihazs
                        from s in dc.cihaz_stoks
                        where c.ID == s.cihaz_id && (c.cihaz_adi.Contains(terim) || c.aciklama.Contains(terim))
                        select new cihaz_rp
                        {
                            ID = c.ID,
                            grupid = c.grupid,
                            cihaz_adi = c.cihaz_adi,
                            aciklama = c.aciklama,
                            seri_no = c.seri_no,
                            garanti_suresi = c.garanti_suresi,
                            bakiye = s.bakiye,
                            cikis = s.cikis,
                            giris = s.giris,
                            fiyat = (Decimal)s.son_alis_fiyati,
                            satis = (Decimal)s.satis_fiyati,
                            barkod = c.barkod,
                            alinan_birim = c.cihaz_birims.birim,
                            satilan_birim = c.cihaz_birims1.birim,
                            sinirsiz = c.sinirsiz == true ? "sinirsiz" : "sinirli"
                        }).Take(10).ToList();
            }

        }
        public cihaz_rp CihazBarkod(string barkod)
        {
            return (from c in dc.cihazs
                    from s in dc.cihaz_stoks
                    where c.ID == s.cihaz_id && c.barkod.Equals(barkod)
                    select new cihaz_rp
                    {
                        ID = c.ID,
                        grupid = c.grupid,
                        cihaz_adi = c.cihaz_adi,
                        aciklama = c.aciklama,
                        seri_no = c.seri_no,
                        garanti_suresi = c.garanti_suresi,
                        bakiye = s.bakiye,
                        cikis = s.cikis,
                        giris = s.giris,
                        fiyat = (Decimal)s.son_alis_fiyati,
                        satis = (Decimal)s.satis_fiyati,
                        sinirsiz = c.sinirsiz == true ? "sinirsiz" : "sinirli"
                    }).FirstOrDefault();
        }


        public void Yeni(string ad, string aciklama, int sure, int grupid, string barkod, int birimAlinan, int birimSatilan, bool sinir)
        {
            cihaz c = new cihaz();
            c.aciklama = aciklama;
            c.grupid = grupid;
            c.cihaz_adi = ad;
            c.Firma = "firma";
            c.garanti_suresi = sure;
            c.barkod = barkod;
            c.seri_no = "-";
            c.alinan_birim = birimAlinan;
            c.satilan_birim = birimSatilan;
            c.sinirsiz = sinir;
            dc.cihazs.Add(c);

            KaydetmeIslemleri.kaydetR(dc);

        }

        public void YeniMakine(string adi, string aciklama, string plaka, decimal son_sayac, decimal toplam_calisma_saat,
            decimal toplam_calisma_gun, decimal toplam_calisma_hafta, decimal toplam_calisma_ay, decimal toplam_masraf_teorik, decimal toplam_masraf_gercek, decimal toplam_gelir, decimal servis_sayaci)
        {
            makine_caris m = new makine_caris();
            m.adi = adi;
            m.aciklama = aciklama;
            m.plaka = plaka;
            m.son_sayac = son_sayac;
            m.tarife_saat = 0;
            m.tarife_gun = 0;
            m.tarife_hafta = 0;
            m.tarife_ay = 0;
            m.toplam_calisma_saat = toplam_calisma_saat;
            m.toplam_calisma_gun = toplam_calisma_gun;
            m.toplam_calisma_hafta = toplam_calisma_hafta;
            m.toplam_calisma_ay = toplam_calisma_ay;
            m.toplam_masraf_teorik = toplam_masraf_teorik;
            m.toplam_masraf_gercek = toplam_masraf_gercek;
            m.toplam_gelir = toplam_gelir;
            m.servis_sayaci = servis_sayaci;
            dc.makine_caris.Add(m);
            KaydetmeIslemleri.kaydetR(dc);
        }

        public int makineatif()
        {
            return dc.makine_caris.Where(x => x.iptal != true).Count();
        }
        public void MakineGuncelle(int id, string adi, string aciklama, string plaka, decimal son_sayac, decimal toplam_calisma_saat,
        decimal toplam_calisma_gun, decimal toplam_calisma_hafta, decimal toplam_calisma_ay, decimal toplam_masraf_teorik, decimal toplam_masraf_gercek, decimal toplam_gelir, decimal servis_sayaci)
        {
            makine_caris m = dc.makine_caris.FirstOrDefault(x => x.makine_id == id);
            if (m != null)
            {
                m.adi = adi;
                m.aciklama = aciklama;
                m.plaka = plaka;
                m.son_sayac = son_sayac;
                m.toplam_calisma_saat = toplam_calisma_saat;
                m.toplam_calisma_gun = toplam_calisma_gun;
                m.toplam_calisma_hafta = toplam_calisma_hafta;
                m.toplam_calisma_ay = toplam_calisma_ay;
                m.toplam_masraf_teorik = toplam_masraf_teorik;
                m.toplam_masraf_gercek = toplam_masraf_gercek;
                m.toplam_gelir = toplam_gelir;
                m.servis_sayaci = servis_sayaci;

                KaydetmeIslemleri.kaydetR(dc);
            }

        }

        public void YeniMasraf(string ad, string aciklama, int birimid, string birim)
        {
            makine_masrafs m = new makine_masrafs();
            m.adi = ad;
            m.aciklama = aciklama;
            m.bakiye = 0;
            m.birim_maliyet = 0;
            m.cikis = 0;
            m.iptal = false;
            m.birim = birim;
            m.birim_id = birimid;
            dc.makine_masrafs.Add(m);
            KaydetmeIslemleri.kaydetR(dc);
        }
        public void CihazGuncelle(string ad, string aciklama, int sure, int cihazid, int grupid, string barkod, int birimAlinan, int birimSatilan, bool sinir)
        {
            cihaz c = dc.cihazs.FirstOrDefault(x => x.ID == cihazid);
            if (c != null)
            {
                c.aciklama = aciklama;
                c.grupid = grupid;
                c.cihaz_adi = ad;
                c.Firma = "firma";
                c.garanti_suresi = sure;
                c.barkod = barkod;
                c.seri_no = "-";
                c.alinan_birim = birimAlinan;
                c.satilan_birim = birimSatilan;
                c.sinirsiz = sinir;

                var fifos = dc.cihaz_fifos.Where(x => x.cihaz_id == cihazid && x.bakiye > 0 && x.iptal == false);
                foreach (var f in fifos)
                {

                    f.sinirsiz = sinir;
                }


                KaydetmeIslemleri.kaydetR(dc);
            }
        }
        public void StokGuncelle(decimal stok, int cihazid, decimal birim_maliyet, bool sinir)
        {

            cihaz_stoks cs = dc.cihaz_stoks.FirstOrDefault(x => x.cihaz_id == cihazid);

            decimal simdikiStok = cs.bakiye;
            decimal simdikiGiris = cs.giris;
            decimal simdikiBakiye = cs.bakiye;

            decimal girilecek = 0;
            decimal cikilacak = 0;
            if (stok > simdikiBakiye)
            {
                //yeni giriş yapılacak
                decimal fark = stok - simdikiBakiye;
                girilecek = fark;
            }
            else if (stok < simdikiBakiye)
            {
                //çıkış yapılacak
                decimal fark = simdikiBakiye - stok;
                cikilacak = fark;
            }


            cs.bakiye = stok;
            cs.cikis += cikilacak;
            cs.giris += girilecek;
            cs.son_alis_fiyati = birim_maliyet;
            //cihaz fifonun da güncellenmesi gerek
            //bunun için varsa daha önceki bakiyesi olan bütün fifolar iptal edilir
            //ve yukarıdaki bakiye bilgisi fioya ogünün tarihi ile girilir
            var fifos = dc.cihaz_fifos.Where(x => x.cihaz_id == cihazid && x.bakiye > 0 && x.iptal == false);
            foreach (var f in fifos)
            {
                f.iptal = true;
            }

            cihaz_fifos cf = new cihaz_fifos();
            cf.cihaz_id = cihazid;
            cf.cikis = 0;
            cf.fiyat = birim_maliyet;
            cf.giris = stok;
            cf.bakiye = stok;
            cf.tarih = DateTime.Now;
            cf.sinirsiz = sinir;
            cf.iptal = false;
            dc.cihaz_fifos.Add(cf);
            KaydetmeIslemleri.kaydetR(dc);



        }

        public void FiyatGuncelle(int cihazid, decimal fiyat)
        {
            cihaz_stoks c = dc.cihaz_stoks.FirstOrDefault(x => x.cihaz_id == cihazid);
            if (c != null)
            {
                c.satis_fiyati = fiyat;
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
        public void MasrafGuncelle(decimal stok, int cihazid, decimal birim_maliyet, string ad, string aciklama, int birimid, string birim)
        {
            makine_masrafs cs = dc.makine_masrafs.FirstOrDefault(x => x.MasrafID == cihazid);
            cs.adi = ad;
            cs.aciklama = aciklama;
            cs.birim = birim;
            cs.birim_id = birimid;

            decimal simdikiStok = cs.bakiye;
            decimal simdikiGiris = cs.giris;
            decimal simdikiBakiye = cs.bakiye;

            decimal girilecek = 0;
            decimal cikilacak = 0;
            if (stok > simdikiBakiye)
            {
                //yeni giriş yapılacak
                decimal fark = stok - simdikiBakiye;
                girilecek = fark;
            }
            else if (stok < simdikiBakiye)
            {
                //çıkış yapılacak
                decimal fark = simdikiBakiye - stok;
                cikilacak = fark;
            }


            cs.bakiye = stok;
            cs.cikis += cikilacak;
            cs.giris += girilecek;
            cs.birim_maliyet = birim_maliyet;
            KaydetmeIslemleri.kaydetR(dc);
        }
        public List<CihazRepo> cihazListesi(int musID)
        {
            return (from u in dc.cihaz_garantis
                    where u.CustID == musID && u.iptal == false
                    select new CihazRepo
                    {
                        urunID = u.ID,
                        cihaz_id = u.cihaz_id,
                        cinsi = u.cihaz.cihaz_adi,
                        garantiBaslangic = (DateTime)u.baslangic,
                        garantiBitis = u.bitis,
                        garantiSuresi = u.cihaz.garanti_suresi,
                        aciklama = u.cihaz.aciklama,
                        durum = u.durum,
                        musteriID = (int)u.CustID,
                        iade_tutar = u.iade_tutar,
                        satis_tutar = u.satis_tutar
                    }).ToList();
        }

        public List<CihazRepo> cihaz_listesi(string terim)
        {
            return (from u in dc.cihaz_garantis
                    where u.iptal == false && u.cihaz.cihaz_adi.Contains(terim)
                    select new CihazRepo
                    {
                        urunID = u.ID,
                        cihaz_id = u.cihaz_id,
                        musteriAdi = u.CustID == null ? "Demirbaş" : u.customer.Ad,
                        cinsi = u.cihaz.cihaz_adi,
                        garantiBaslangic = (DateTime)u.baslangic,
                        garantiBitis = u.bitis,
                        garantiSuresi = u.cihaz.garanti_suresi,
                        aciklama = u.cihaz.aciklama,
                        durum = u.durum,
                        musteriID = u.CustID == null ? -99 : (int)u.CustID,
                        iade_tutar = u.iade_tutar,
                        satis_tutar = u.satis_tutar
                    }).ToList();

        }
        public List<CihazRepo> cihaz_listesi(int id)
        {
            return (from u in dc.cihaz_garantis
                    where u.iptal == false && u.cihaz_id == id
                    select new CihazRepo
                    {
                        urunID = u.ID,
                        cihaz_id = u.cihaz_id,
                        musteriAdi = u.CustID == null ? "Demirbaş" : u.customer.Ad,
                        cinsi = u.cihaz.cihaz_adi,
                        garantiBaslangic = (DateTime)u.baslangic,
                        garantiBitis = u.bitis,
                        garantiSuresi = u.cihaz.garanti_suresi,
                        aciklama = u.cihaz.aciklama,
                        durum = u.durum,
                        musteriID = u.CustID == null ? -99 : (int)u.CustID,
                        iade_tutar = u.iade_tutar,
                        satis_tutar = u.satis_tutar
                    }).ToList();

        }
        public string urun_teller(string terim)
        {
            List<string> musteriler = (from u in dc.cihaz_garantis
                                       where u.iptal == false && u.cihaz.cihaz_adi.Contains(terim) && !String.IsNullOrEmpty(u.customer.telefon)
                                       select u.customer.telefon).Distinct().ToList();

            string mailler = "";
            foreach (string c in musteriler)
            {
                mailler += c + ",";
            }
            return mailler;

        }
        public string urun_teller(int id)
        {
            List<string> musteriler = (from u in dc.cihaz_garantis
                                       where u.iptal == false && u.cihaz_id == id && !String.IsNullOrEmpty(u.customer.telefon)
                                       select u.customer.telefon).Distinct().ToList();

            string mailler = "";
            foreach (string c in musteriler)
            {
                mailler += c + ",";
            }
            return mailler;

        }
        public string urun_mailler(string terim)
        {
            List<string> musteriler = (from u in dc.cihaz_garantis
                                       where u.iptal == false && u.cihaz.cihaz_adi.Contains(terim) && !String.IsNullOrEmpty(u.customer.email)
                                       select u.customer.email).Distinct().ToList();

            string mailler = "";
            foreach (string c in musteriler)
            {
                mailler += c + ";";
            }
            return mailler;

        }
        public string urun_mailler(int id)
        {
            List<string> musteriler = (from u in dc.cihaz_garantis
                                       where u.iptal == false && u.cihaz_id == id && !String.IsNullOrEmpty(u.customer.email)
                                       select u.customer.email).Distinct().ToList();

            string mailler = "";
            foreach (string c in musteriler)
            {
                mailler += c + ";";
            }
            return mailler;

        }
        public void garanti_iptal(int garanti_id)
        {
            cihaz_garantis c = dc.cihaz_garantis.FirstOrDefault(x => x.ID == garanti_id);
            if (c != null)
            {
                c.iptal = true;
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
        public void garanti_iade(int garanti_id, decimal iade_tutar, string aciklama, int CustID, string kullanici)
        {
            //iade alındığında yeni bir stok_fifos girer ve açıklama olarak da iade gireriz-
            cihaz_garantis c = dc.cihaz_garantis.FirstOrDefault(x => x.ID == garanti_id);
            if (c != null)
            {
                //faturaya iade_id'yi işaretle
                //fatura iptal edilirken cari güncelleniyor(triggerda)
                //önce iade alınıp sonra fatura iptal edildiğinde cari de çift kayıt oluyordu
                //bunun için faturaya service hesabına göre arayıp iade_id'yi yazdırıyorum
                //triggerda cari hesap güncellenirken iade_id' null ise cari güncelleme yapıyor.
                List<fatura> hesapFaturalari = (from f in dc.faturas
                                                where f.servicehesap_id == c.servicehesap_id && (f.iptal == null || f.iptal == false)
                                                select f).ToList();
                foreach (fatura fati in hesapFaturalari)
                {
                    fati.iade_id = garanti_id;
                    fati.updated = kullanici;
                }

                musteriodemeler mo = new musteriodemeler();
                mo.Aciklama = c.adet + " Adet " + c.cihaz.cihaz_adi + " " + "Cihaz iadesi " + aciklama;
                mo.belge_no = "";
                mo.iade_id = garanti_id;
                mo.iptal = false;
                mo.kullanici = kullanici;
                mo.KullaniciID = kullanici;
                mo.mahsup_key = "";
                mo.Musteri_ID = CustID;
                mo.no = "-";
                mo.OdemeMiktar = iade_tutar;
                mo.OdemeTarih = DateTime.Now;
                mo.islem_tarihi = DateTime.Now;
                mo.inserted = kullanici;
                mo.tahsilat_odeme = "tahsilat";
                mo.tahsilat_turu = "iade";
                mo.taksit_no = 0;
                mo.Firma = "firma";
                dc.musteriodemelers.Add(mo);

                c.durum = "iade";
                c.iade_tutar = iade_tutar;
                c.aciklama = aciklama;
                //iadeyi cihaz fifo girelim
                cihaz_fifos fifo = new cihaz_fifos();
                fifo.bakiye = 1;
                fifo.cihaz_id = c.cihaz_id;
                fifo.cikis = 0;
                fifo.fiyat = iade_tutar;
                fifo.giris = 1;
                fifo.iptal = false;
                fifo.tarih = DateTime.Now;
                dc.cihaz_fifos.Add(fifo);


                KaydetmeIslemleri.kaydetR(dc);
            }
        }

        //cihaz grupları(vergiler için)
        public void GrupEkle(string grupadi, decimal kdv, decimal otv, decimal oiv)
        {
            cihaz_grups g = new cihaz_grups();
            g.grupadi = grupadi;
            g.kdv = kdv;
            g.oiv = oiv;
            g.otv = otv;
            dc.cihaz_grups.Add(g);
            KaydetmeIslemleri.kaydetR(dc);
        }
        public void GrupDuzenle(int grupid, string grupadi, decimal kdv, decimal otv, decimal oiv)
        {
            var grup = dc.cihaz_grups.FirstOrDefault(x => x.grupid == grupid);
            if (grup != null)
            {
                grup.grupadi = grupadi;
                grup.kdv = kdv;
                grup.otv = otv;
                grup.oiv = oiv;
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
        public List<cihaz_grups> CihazGruplar()
        {
            return dc.cihaz_grups.ToList();
        }
        public List<cihaz_grups> gruplar(string terim = "")
        {
            if (terim == "")
            {
                return (dc.cihaz_grups).ToList();
            }
            else
            {
                return dc.cihaz_grups.Where(x => x.grupadi.Contains(terim)).Take(10).ToList();
            }

        }
    }
    public class CihazRepo
    {
        public int urunID { get; set; }
        public int musteriID { get; set; }
        public int cihaz_id { get; set; }
        public string musteriAdi { get; set; }
        public string cinsi { get; set; }
        public DateTime garantiBaslangic { get; set; }
        public DateTime garantiBitis { get; set; }
        public int garantiSuresi { get; set; }
        public string aciklama { get; set; }
        public string durum { get; set; }
        public decimal? satis_tutar { get; set; }
        public decimal? iade_tutar { get; set; }

    }

    public class Masraf
    {
        public int MasrafID { get; set; }
        public string adi { get; set; }
        public string aciklama { get; set; }
        public decimal giris { get; set; }
        public decimal cikis { get; set; }
        public decimal bakiye { get; set; }
        public decimal birim_maliyet { get; set; }

    }
    public class cihaz_rp
    {
        public int ID { get; set; }
        public int grupid { get; set; }

        public string barkod { get; set; }
        public string cihaz_adi { get; set; }
        public string aciklama { get; set; }
        public string seri_no { get; set; }
        public int garanti_suresi { get; set; }
        public decimal? giris { get; set; }
        public decimal? cikis { get; set; }
        public decimal? bakiye { get; set; }
        public decimal fiyat { get; set; }
        public decimal satis { get; set; }
        public int alinan_birim_id { get; set; }
        public int satilan_birim_id { get; set; }
        public string alinan_birim { get; set; }
        public string satilan_birim { get; set; }
        public string sinirsiz { get; set; }
    }


}
