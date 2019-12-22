using ServisDAL.NetGsm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ServisDAL
{
    public class SmsNetGsm
    {

        private string kullanici_adi;
        private string sifre;
        public string mesaj { get; set; }
        private string gonderen { get; set; }
        public string[] tel_nolar { get; set; }

        public SmsNetGsm(string kullaniciParam, string sifreParam, string gonderen)
        {
            kullanici_adi = kullaniciParam;
            sifre = sifreParam;
            this.gonderen = gonderen;


        }

        public string TekMesajGonder()
        {

            smsnnClient sms_gonder = new smsnnClient();

            return sms_gonder.sms_gonder_1n(kullanici_adi, sifre, "", gonderen, mesaj, tel_nolar, "TR", "", "");

        }



    }
    public class NidaSms
    {

        private string kullanici_adi;
        private string sifre;
        public string mesaj { get; set; }
        private string gonderen { get; set; }
        public string[] tel_nolar { get; set; }

        public NidaSms(string kullaniciParam, string sifreParam, string gonderen)
        {
            kullanici_adi = kullaniciParam;
            sifre = sifreParam;
            this.gonderen = gonderen;

        }

        public string TekMesajGonder()
        {
            var tels = "";

            foreach (var t in this.tel_nolar)
            {
                tels += "<YOLLA><MESAJ>";
                tels += mesaj;
                tels += "</MESAJ>";
                tels += "<NO>";
                tels += t;
                tels += "</NO>";
                tels += "</YOLLA>";
            }
            string xml = "<SERVIS><BILGI><KULLANICI_ADI>" + this.kullanici_adi + "</KULLANICI_ADI><SIFRE>" + this.sifre + "</SIFRE>" +
 "<GONDERIM_TARIH></GONDERIM_TARIH><BASLIK>" + this.gonderen + "</BASLIK><ACTION>1</ACTION></BILGI><ISLEM>" + tels +
      "</ISLEM></SERVIS>";

            var request = (HttpWebRequest)WebRequest.Create("http://www.nidamesaj.com/smsapi_post.php");

            var postData = "xml=" + xml;

            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;

        }



    }
    public class SmsIslemleri
    {

        TeknikServis.Radius.radiusEntities dc;
        public SmsIslemleri(TeknikServis.Radius.radiusEntities dc)
        {

            this.dc = dc;
        }

        public void CariMesajKaydet(string[] teller)
        {
            //tel listesine göre mesaj atılma tarihini kaydediyoruz
            if (teller.Length > 0)
            {
                foreach (string tel in teller)
                {
                    TeknikServis.Radius.carihesap hesap = dc.carihesaps.FirstOrDefault(x => x.telefon.Equals(tel));
                    hesap.son_mesaj = DateTime.Now;
                }
                KaydetmeIslemleri.kaydetR(dc);
            }
        }


        public string SmsGenel(AyarIslemleri ayarimiz, string mesaj, string[] teller)
        {
            string mesajim = "";

            //SMS api ayarlarını çekelim

            TeknikServis.Radius.ayar smsApi = ayarimiz.SmsAyarR();

            //herhangi bir smsApi tanımlanmışsa buradaki ayarı kullanarak gönderme yapacaz

            if (smsApi != null)
            {

                if (smsApi.Mail_Server == "NETGSM")
                {
                    SmsNetGsm gsm = new SmsNetGsm(smsApi.Mail_UserName, smsApi.Mail_PW, smsApi.gonderen);
                    gsm.mesaj = mesaj;
                    gsm.tel_nolar = teller;
                    mesajim += gsm.TekMesajGonder();

                }
                else if (smsApi.Mail_Server == "NİDASMS")
                {
                    NidaSms gsm = new NidaSms(smsApi.Mail_UserName, smsApi.Mail_PW, smsApi.gonderen);

                    gsm.mesaj = mesaj;
                    gsm.tel_nolar = teller;
                    mesajim += gsm.TekMesajGonder();

                }
            }


            return mesajim;

        }
        public string MesajSonucu(string donen_mesaj)
        {
            donen_mesaj = donen_mesaj.Trim();
            string sonuc = "";

            if (donen_mesaj.Equals("20"))
            {
                sonuc = "Mesaj Gönderilemedi! Mesaj metninizdeki problemden dolayı mesajınız gönderilemedi. Bir mesaj metninde  en fazla 917 karakter bulunabilir.";
            }
            else if (donen_mesaj.Equals("30"))
            {
                sonuc = "Mesaj gönderilemedi! Api kullanıcı adınız veya şifreniz hatalı!";
            }
            else if (donen_mesaj.Equals("40"))
            {
                sonuc = "Mesaj gönderilemeledi! Mesaj başlığınızın (gönderici adınızın) sistemde tanımlı değil. Gönderici adlarınızı API ile sorgulayarak kontrol edebilirsiniz.";
            }

            else if (donen_mesaj.Equals("70"))
            {
                sonuc = "Mesaj gönderilemeledi! Hatalı sorgulama. Gönderdiğiniz parametrelerden birisi hatalı veya zorunlu alanlardan biri eksik.";
            }
            else if (donen_mesaj.Equals("100") || donen_mesaj.Equals("101"))
            {
                sonuc = "Mesaj gönderilemedi! Servis sağlayıcınızda bir sistem hatası var";
            }
            else
            {
                sonuc = "Mesajınız Gönderildi";
            }
            return sonuc;
        }

    }


}
