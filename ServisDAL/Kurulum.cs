using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class Kurulum
    {

        //string owner;
        radiusEntities dc;
        public Kurulum(radiusEntities dc)
        {

            this.dc = dc;
        }



      
        public void CariGuncelle(int custid, decimal netBakiye, bool borclu, string kullanici)
        {

            //önce cari hesabı güncelleyelim
            carihesap c = dc.carihesaps.FirstOrDefault(x => x.MusteriID == custid);



            if (c != null)
            {
                c.NetAlacak = 0;
                c.NetBorc = 0;
                c.ToplamAlacak = 0;
                c.ToplamBakiye = 0;
                c.ToplamBorc = 0;
                c.ToplamOdedigimiz = 0;
                c.ToplamOdenen = 0;

                if (borclu == false)
                {
                    //alacaklıymış o yüzden fatura oluşturmayacaz sadece carihesap kaydı yapacaz

                    c.ToplamOdedigimiz = 0;
                    c.ToplamAlacak = netBakiye;

                }
                else
                {


                    TeknikServis.Radius.customer mu = dc.customers.Where(x => x.CustID == custid).FirstOrDefault();


                    fatura fatik = new fatura();
                    fatik.bakiye = netBakiye;
                    fatik.son_odeme_tarihi = DateTime.Now;
                    fatik.sattis_tarih = DateTime.Now;
                    fatik.no = "-1";
                    fatik.taksit_no = 0;
                    fatik.odenen = 0;
                    fatik.Firma = "firma";
                    fatik.tc = mu.TC;
                    fatik.MusteriID = mu.CustID;
                    fatik.islem_tarihi = DateTime.Now;
                    fatik.telefon = mu.telefon;
                    fatik.tutar = netBakiye;
                    fatik.Firma = mu.Firma;
                    fatik.tur = "Devir";
                    fatik.iptal = false;
                    fatik.inserted = kullanici;
                    dc.faturas.Add(fatik);

                }
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
     
    }
}
