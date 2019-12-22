using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;

namespace ServisDAL
{

    public class AyarIslemleri
    {

        radiusEntities db;

        public AyarIslemleri(radiusEntities db)
        {

            this.db = db;

        }

        #region mail ayar metotları
        public TeknikServis.Radius.ayar MailAyarR()
        {

            return db.ayars.Where(a => a.tur.Equals("mail")).FirstOrDefault();

        }
        public TeknikServis.Radius.ayar SmsAyarR()
        {
            return db.ayars.Where(a => a.tur.Equals("sms")).FirstOrDefault();

        }

 

        public void MailAyarKaydetR(string server, string kimden, int port, string username, string pw, string aktif)
        {

            TeknikServis.Radius.ayar ayarimiz = db.ayars.Where(a => a.tur.Equals("mail")).FirstOrDefault();
            if (ayarimiz != null)
            {
                //update
                ayarimiz.Mail_PW = pw;
                ayarimiz.Mail_Port = port;
                ayarimiz.Mail_Server = server.Trim().ToLower();
                ayarimiz.Mail_UserName = username;
                ayarimiz.Mail_Kimden = kimden;
                ayarimiz.aktif_adres = aktif;
                KaydetmeIslemleri.kaydetR(db);


            }
            else
            {
                ayarimiz = new TeknikServis.Radius.ayar();
                ayarimiz.Mail_PW = pw;
                ayarimiz.Mail_Port = port;
                ayarimiz.Mail_Server = server.Trim().ToLower();
                ayarimiz.Mail_UserName = username;
                ayarimiz.Mail_Kimden = kimden;
                ayarimiz.Firma = "firma";

                ayarimiz.tur = "mail";
                ayarimiz.aktif_adres = aktif;
                db.ayars.Add(ayarimiz);
                KaydetmeIslemleri.kaydetR(db);
                //yeni kayıt
            }


        }
        public void SmsAyarKaydetR(string saglayici, string kimden, string username, string pw, string aktif)
        {

            TeknikServis.Radius.ayar ayarimiz = db.ayars.Where(a => a.tur.Equals("sms")).FirstOrDefault();
            if (ayarimiz != null)
            {
                //update
                ayarimiz.Mail_PW = pw;
                ayarimiz.Mail_Port = 0;
                ayarimiz.Mail_Server = saglayici;
                ayarimiz.Mail_UserName = username;
                ayarimiz.Mail_Kimden = kimden;
                ayarimiz.aktif_adres = aktif;
                ayarimiz.gonderen = kimden;
                KaydetmeIslemleri.kaydetR(db);


            }
            else
            {
                ayarimiz = new TeknikServis.Radius.ayar();
                ayarimiz.Mail_PW = pw;
                ayarimiz.Mail_Port = 0;
                ayarimiz.Mail_Server = saglayici;
                ayarimiz.Mail_UserName = username;
                ayarimiz.Mail_Kimden = kimden;
                ayarimiz.Firma = "firma";
                ayarimiz.gonderen = kimden;
                ayarimiz.tur = "sms";
                ayarimiz.aktif_adres = aktif;
                db.ayars.Add(ayarimiz);
                KaydetmeIslemleri.kaydetR(db);

            }


        }
        #endregion

        #region servis tip

        public void tipEkleR(string ad, string aciklama, string css)
        {
            if (string.IsNullOrEmpty(css))
            {
                css = "#5367ce";
            }
            TeknikServis.Radius.service_tips tip = new TeknikServis.Radius.service_tips();
            tip.tip_ad = ad;
            tip.Firma = "firma";
            tip.aciklama = aciklama;
            tip.css = css;
            db.service_tips.Add(tip);
            KaydetmeIslemleri.kaydetR(db);



        }
        public void masrafEkleR(string ad, string aciklama, string css)
        {

            if (string.IsNullOrEmpty(css))
            {
                css = "#b8afaf";
            }

            TeknikServis.Radius.masraf_tips tip = new TeknikServis.Radius.masraf_tips();
            tip.tip_adi = ad;
            tip.Firma = "firma";

            tip.aciklama = aciklama;
            tip.css = css;
            db.masraf_tips.Add(tip);
            KaydetmeIslemleri.kaydetR(db);


        }
        public TeknikServis.Radius.service_tips tekTipR(int id)
        {
            return db.service_tips.Where(t => t.tip_id == id).FirstOrDefault();
        }
        public TeknikServis.Radius.masraf_tips masrafTipR(int id)
        {

            return db.masraf_tips.Where(t => t.tip_id == id).FirstOrDefault();
        }

        public void tipGuncelleR(int id, string ad, string aciklama, string css)
        {
            TeknikServis.Radius.service_tips tip = tekTipR(id);
            tip.tip_ad = ad;
            tip.aciklama = aciklama;
            if (!string.IsNullOrEmpty(css))
            {
                tip.css = css;
            }
            KaydetmeIslemleri.kaydetR(db);

        }
        public void masrafGuncelleR(int id, string ad, string aciklama, string css)
        {

            TeknikServis.Radius.masraf_tips tip = masrafTipR(id);
            tip.tip_adi = ad;
            tip.aciklama = aciklama;
            if (!string.IsNullOrEmpty(css))
            {
                tip.css = css;
            }

            KaydetmeIslemleri.kaydetR(db);

        }
        public void tipSilR(int id)
        {

            TeknikServis.Radius.service_tips tip = tekTipR(id);
            tip.iptal = true;
            KaydetmeIslemleri.kaydetR(db);

        }
        public void masrafSilR(int id)
        {
            TeknikServis.Radius.masraf_tips tip = masrafTipR(id);
            tip.iptal = true;
            KaydetmeIslemleri.kaydetR(db);
        }
        public List<TeknikServis.Radius.service_tips> tipListesiR()
        {

            return (from t in db.service_tips
                    where t.iptal == null
                    orderby t.tip_id descending
                    select t).ToList();
        }
        public List<TeknikServis.Radius.service_tips> tipListesiGrid()
        {

            return (from t in db.service_tips
                    where t.iptal == null && t.tip_id > 0
                    select t).ToList();
        }
        public List<TeknikServis.Radius.masraf_tips> masrafListesiR()
        {

            return (from t in db.masraf_tips
                    where t.iptal == false
                    select t).ToList();

        }
        public List<TeknikServis.Radius.masraf_tips> masrafListesiGrid()
        {

            return (from t in db.masraf_tips
                    where t.iptal == false && t.tip_id > 0
                    select t).ToList();

        }
        public List<cihaz_birims> birimler()
        {
            return (from t in db.cihaz_birims
                    where t.iptal == false
                    select t).ToList();
        }
        public void birim_ekle(string bir)
        {
            cihaz_birims b = new cihaz_birims();
            b.birim = bir;
            b.iptal = false;
            db.cihaz_birims.Add(b);
            KaydetmeIslemleri.kaydetR(db);
        }

        public void birim_sil(int id)
        {
            var b = db.cihaz_birims.FirstOrDefault(x => x.id == id);
            if (b != null)
            {
                b.iptal = true;
                KaydetmeIslemleri.kaydetR(db);
            }
        }


        public List<TeknikServis.Radius.service_tips> tipListesiTekliR(int id)
        {

            return (from t in db.service_tips
                    where t.tip_id == id
                    select t).ToList();
        }
        public List<TeknikServis.Radius.masraf_tips> masrafListesiTekliR(int id)
        {

            return (from t in db.masraf_tips
                    where t.tip_id == id
                    select t).ToList();

        }
        #endregion
        private bool kontrol(bool baslangic, bool son, bool karar, bool onay)
        {

            bool flag = false;
            if (baslangic == true || son == true || karar == true || onay == true)
            {

                if (baslangic == true)
                {
                    if (son != true && karar != true && onay != true)
                    {
                        flag = true;
                    }
                }
                if (son == true)
                {
                    if (baslangic != true && karar != true && onay != true)
                    {
                        flag = true;
                    }
                }
                if (karar == true)
                {
                    if (son != true && baslangic != true && onay != true)
                    {
                        flag = true;
                    }
                }
                if (onay == true)
                {
                    if (son != true && baslangic != true && karar != true)
                    {
                        flag = true;
                    }
                }
            }
            else
            {
                flag = true;
            }
            return flag;

        }

    }

}
