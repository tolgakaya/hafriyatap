using System;
using System.Web;
using System.IO;
using System.Web.Script.Services;
using System.Web.Services;
using ServisDAL;
using TeknikServis.Logic;
using System.Linq;
using System.Collections.Generic;

namespace TeknikServis
{
    [ScriptService]
    public partial class ResimKaydet : System.Web.UI.Page
    {
        static string path = HttpContext.Current.Server.MapPath("/Uploads/");
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod()]

        public static void UploadPic(string imageData, string servis, string aciklama, bool ress, string baslik, string kullanici)
        {
            //resim yüklendiği zaman bütün işlemleri burada tamamlayacaz.
            //yani detay ekleme işlemleri burada tamamlanacak.
            //böylece veritabanınaresim yolu yazılırken bu isim kullanılacak.
            using (Radius.radiusEntities dc = Radius.MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri ser = new ServisIslemleri(dc);
                int servisID = Int32.Parse(servis);
                string resimYolu = "-";
                if (ress == true)
                {
                    string isim = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "- ").Replace(":", "") + ".png";
                    string fileNameWitPath = path + isim;
                    using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            byte[] data = Convert.FromBase64String(imageData);
                            bw.Write(data);
                            bw.Close();
                        }
                    }
                    resimYolu = "/Uploads/" + isim;
                }


                ser.servisDetayEkleR(servisID, resimYolu, aciklama, kullanici, baslik);
            }

        }

          

    }
 
}