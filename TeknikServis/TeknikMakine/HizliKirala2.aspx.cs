using ServisDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;


namespace TeknikServis.TeknikMakine
{
    public partial class HizliKirala2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                if (!IsPostBack)
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }


                    makineGoster(dc);
                    //makineGoster(dc);

                }

            }


        }
        protected void btnAddMakineRecord_Click(object sender, EventArgs e)
        {
            //string gs = txtGarantiSuresi.Text;
            //using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            //{
            //    ServisIslemleri s = new ServisIslemleri(dc);
            //    s.CihazEkle(txtCihazAdi.Text, txtCihazAciklama.Text, gs);
            //    cihazGoster(dc);
            //}

            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append(@"<script type='text/javascript'>");

            //sb.Append(" alertify.success('Kayıt Eklendi!');");

            //sb.Append("$('#addModalCihaz').modal('hide');");
            ////sb.Append("$('#yeniModal').modal('show');");
            //sb.Append(@"</script>");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        protected void btnKaydetMakine_Click(object sender, EventArgs e)
        {
            //string servisidd = Request.QueryString["servisid"];
            string custidd = hdnCari.Value;



            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {

                if (!String.IsNullOrEmpty(custidd))
                {
                    ServisIslemleri s = new ServisIslemleri(dc);

                    int custid = Int32.Parse(custidd);

                    string kimlik = Araclar.KimlikUret(10);

                    string islem = txtIslemParcaMakine.Value;

                    decimal kdv = Decimal.Parse(txtKDVOraniDuzenleMakine.Text);
                    decimal yekun = Decimal.Parse(txtYekunMakine.Text);
                    string aciklama = txtAciklamaMakine.Text;

                    //int makine_id = -1;
                    string makine = txtMakineAdiGoster.Value;
                    //if (grdMakine.SelectedIndex > -1)
                    //{
                    int makine_id = Convert.ToInt32(grdMakine.SelectedValue);
                    //}

                    DateTime karar_tarihi = DateTime.Now;
                    string tarS = txtTarihMakine.Value;
                    if (!String.IsNullOrEmpty(tarS))
                    {
                        karar_tarihi = DateTime.Parse(tarS);
                    }
                    string tarife_kodu = drdTarife.SelectedItem.Text;
                    int tarifeid = Convert.ToInt32(drdTarife.SelectedValue);
                    decimal sure_saat = 0;
                    decimal son = Decimal.Parse(txtSonNumara.Text);
                    decimal baslangic = Decimal.Parse(txtSonNumara.Text);
                    DateTime baslama_tarih = DateTime.Now; DateTime bitis_tarih = DateTime.Now;

                    if (makine_id > -1)
                    {

                        if (!String.IsNullOrEmpty(datetimepicker6.Text) && !String.IsNullOrEmpty(datetimepicker7.Text))
                        {
                            baslama_tarih = DateTime.Parse(datetimepicker6.Text);
                            bitis_tarih = DateTime.Parse(datetimepicker7.Text);
                        }
                        int dakika = 0;

                        if (!String.IsNullOrEmpty(hdnSaatlik.Value))
                        {
                            string dakikaS = txtDakika.Text;

                            if (!String.IsNullOrEmpty(dakikaS))
                            {
                                sure_saat = Decimal.Parse(dakikaS) / 60;
                                dakika = Int32.Parse(dakikaS);
                            }
                            if (!String.IsNullOrEmpty(txtYeniNumara.Text))
                            {
                                son = Decimal.Parse(txtYeniNumara.Text);
                            }

                        }
                        else
                        {
                            string sureS = txtSure.Text;
                            if (!String.IsNullOrEmpty(sureS))
                            {
                                sure_saat = Decimal.Parse(sureS);
                            }
                        }

                        string tarife_tipi = hdnTarifeTipi.Value;
                        decimal sayac_farki = son - baslangic;

                        karar_wrap_makine w = new karar_wrap_makine();
                        w.aciklama = aciklama;
                        w.baslangic = baslangic;
                        w.baslangic_tarih = baslama_tarih;
                        w.bitis = son;
                        w.bitis_tarih = bitis_tarih;
                        w.calisma_saati = sure_saat;
                        w.cihaz_adi = makine;
                        w.dakika = dakika;
                        w.islemParca = islem;
                        w.kdv = kdv;
                        w.makine_id = makine_id;
                        w.tarife_kodu = tarife_kodu;
                        w.tarih = DateTime.Now;
                        w.tutar = yekun;
                        w.yekun = yekun;
                        w.yeni_sayac = son;
                        w.tarife_tipi = tarife_tipi;
                        w.tarifeid = tarifeid;
                        w.toplam_sayac = sayac_farki;
                        w.sure_aciklama = txtSaatBilgi.Text;

                        s.servisEkleMakineli(custid, User.Identity.Name, aciklama, kimlik, "hızlı kiralama", DateTime.Now, w, "admintol");

                        Response.Redirect("/MusteriDetayBilgileri?custid=" + custid);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Hesap!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniHideModalScript", sb.ToString(), false);

                    }



                }

            }
        }

        protected void grdMusteri_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                musteriGoster(dc);
                if (grdMusteri.SelectedValue != null)
                {
                    MusteriIslemleri m = new MusteriIslemleri(dc);
                    txtCariID.Value = m.musteriTek(Convert.ToInt32(grdMusteri.SelectedValue)).Ad;
                    hdnCari.Value = grdMusteri.SelectedValue.ToString();
                }


                //txtCariID.Text = grdMusteri.SelectedValue.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#musteriModal').modal('hide');");
                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);

            }

        }

        private void musteriGoster(radiusEntities dc)
        {
            string s = txtMusteriSorgu.Value;
            MusteriIslemleri m = new MusteriIslemleri(dc);
            if (!String.IsNullOrEmpty(s))
            {
                grdMusteri.DataSource = m.musteriAraR2(s, "musteri");
                grdMusteri.DataBind();

            }
        }

        protected void MusteriAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                musteriGoster(dc);
            }

        }

        protected void btnCari_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#musteriModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
        }

        protected void grdMakine_SelectedIndexChanged(object sender, EventArgs e)
        {

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                tarifeCek(dc);
            }

        }

        protected void MakineAra(object sender, EventArgs e)
        {

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);

                makineGoster(dc);
            }
        }

        private void makineGoster(radiusEntities dc)
        {
            MakineIslem c = new MakineIslem(dc);

            string arama_terimi = txtMakineAra.Value;
            grdMakine.DataSource = c.makineler(arama_terimi);
            grdMakine.DataBind();

        }

        private void tarifeCek(radiusEntities dc)
        {
            if (grdMakine.SelectedValue != null)
            {
                int id = Convert.ToInt32(grdMakine.SelectedValue);
                MakineIslem m = new MakineIslem(dc);
                var tarifeler = m.tarifeler(id);
                //if (tarifeler.Count > 0)
                //{
                drdTarife.DataSource = tarifeler;
                drdTarife.DataValueField = "id";
                drdTarife.DataTextField = "ad";
                drdTarife.DataBind();
                //}
                Makine mak = m.tekmakine(id);
                txtSonNumara.Text = mak.son_sayac.ToString();
                txtMakineAdiGoster.Value = mak.adi;

                if (tarifeler.Count > 0)
                {

                    int tarifeid = Convert.ToInt32(drdTarife.SelectedValue);


                    Tarife t = m.tarife_tek(tarifeid);
                    txtFiyat.Text = t.tutar.ToString();

                    if (t.saatlik == true)
                    {
                        numara_aralik.Visible = true;
                        hdnSaatlik.Value = "yes";
                        tarih_aralik.Visible = false;

                    }
                    else
                    {
                        numara_aralik.Visible = false;
                        hdnSaatlik.Value = string.Empty;
                        tarih_aralik.Visible = true;

                    }

                }
                else
                {
                    txtFiyat.Text = String.Empty;
                    hdnSaatlik.Value = string.Empty;
                }
            }
        }
        private void fiyatCek(radiusEntities dc)
        {
            if (grdMakine.SelectedValue != null && drdTarife.SelectedValue != null)
            {
                int id = Convert.ToInt32(grdMakine.SelectedValue);
                int tarifeid = Convert.ToInt32(drdTarife.SelectedValue);

                MakineIslem m = new MakineIslem(dc);
                Makine mak = m.tekmakine(id);
                Tarife t = m.tarife_tek(tarifeid);

                txtFiyat.Text = t.tutar.ToString();

                txtSonNumara.Text = mak.son_sayac.ToString();
                txtMakineAdiGoster.Value = mak.adi;
            }
        }
        protected void drdTarife_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                int tarifeid = Convert.ToInt32(drdTarife.SelectedValue);
                MakineIslem m = new MakineIslem(dc);
                Tarife t = m.tarife_tek(tarifeid);
                fiyatCek(dc);

                if (t.saatlik == true)
                {
                    numara_aralik.Visible = true;
                    tarih_aralik.Visible = false;
                    hdnSaatlik.Value = "yes";
                    hdnTarifeTipi.Value = t.tarife_kodu;
                    dakika_hesapla();
                    tutar_hesapla();
                }
                else
                {
                    numara_aralik.Visible = false;
                    tarih_aralik.Visible = true;
                    hdnSaatlik.Value = string.Empty;
                    hdnTarifeTipi.Value = string.Empty;
                    sure_hesapla();
                }
             
            }

        }
        protected void txtBaslangicChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                int tarifeid = Convert.ToInt32(drdTarife.SelectedValue);
                MakineIslem m = new MakineIslem(dc);
                Tarife t = m.tarife_tek(tarifeid);

                if (t.saatlik == true)
                {
                    numara_aralik.Visible = true;
                    tarih_aralik.Visible = false;
                    hdnSaatlik.Value = "yes";
                    hdnTarifeTipi.Value = t.tarife_kodu;
                    dakika_hesapla();
                    tutar_hesapla();
                }
                else
                {
                    numara_aralik.Visible = false;
                    tarih_aralik.Visible = true;
                    hdnSaatlik.Value = string.Empty;
                    hdnTarifeTipi.Value = string.Empty;
                    sure_hesapla();
                }

                //fiyatCek(dc);
            }

        }

        private void tutar_hesapla()
        {
            string fiyat = txtFiyat.Text;
            decimal f = 0;
            if (!String.IsNullOrEmpty(fiyat))
            {
                f = Decimal.Parse(fiyat) / 60;
                //dakika fiyatını hesaplıyor
            }
            string saatS = txtDakika.Text;
            decimal saat = 0;
            if (!String.IsNullOrEmpty(saatS))
            {
                saat = Decimal.Parse(saatS);
            }

            decimal tutar = Math.Round((f * saat), 2);
            txtYekunMakine.Text = tutar.ToString();
        }
        private void tutar_hesaplaSureli()
        {
            string fiyat = txtFiyat.Text;
            decimal f = 0;
            if (!String.IsNullOrEmpty(fiyat))
            {
                f = Decimal.Parse(fiyat);
                //dakika fiyatını hesaplıyor
            }
            string sureS = txtSure.Text;
            decimal sure = 0;
            if (!String.IsNullOrEmpty(sureS))
            {
                sure = Decimal.Parse(sureS);
            }

            decimal tutar = Math.Round((f * sure), 2);
            txtYekunMakine.Text = tutar.ToString();
        }
        private void dakika_hesapla()
        {
            string basS = txtSonNumara.Text;
            string sonS = txtYeniNumara.Text;
            int dakika = 0;
            string bilgi = "0 saat 0 dakika";

            if (!String.IsNullOrEmpty(basS) && !String.IsNullOrEmpty(sonS))
            {
                decimal bas = Decimal.Parse(basS);
                decimal son = Decimal.Parse(sonS);
                if (son > bas)
                {
                    decimal fark = son - bas;

                    int net = (int)fark;
                    decimal ondalik = fark % 1.0m;


                    dakika = net * 60 + (int)(ondalik * 10) * 6;

                    TimeSpan ts = TimeSpan.FromMinutes(dakika);
                    int gun = ts.Days;
                    int toplam_saat = gun * 24 + ts.Hours;
                    bilgi = toplam_saat + " saat " + ts.Minutes + " dakika";
                }
            }

            txtDakika.Text = dakika.ToString();
            txtSaatBilgi.Text = bilgi;
        }
        protected void txtSaat_TextChanged(object sender, EventArgs e)
        {
            tutar_hesapla();
            if (!String.IsNullOrEmpty(txtDakika.Text))
            {
                TimeSpan ts = TimeSpan.FromMinutes(Int32.Parse(txtDakika.Text));
                txtSaatBilgi.Text = ts.Hours + " saat " + ts.Minutes + " dakika";
            }

        }

        private void sure_hesapla()
        {
            string basS = datetimepicker6.Text;
            string sonS = datetimepicker7.Text;
            double sure = 0;
            if (!String.IsNullOrEmpty(basS) && !String.IsNullOrEmpty(sonS))
            {
                DateTime bas = DateTime.Parse(basS);
                DateTime son = DateTime.Parse(sonS);

                TimeSpan ts = son - bas;
                if (drdTarife.SelectedValue.Equals("gun"))
                {
                    sure = ts.TotalDays;
                }
                else if (drdTarife.SelectedValue.Equals("hafta"))
                {
                    sure = ts.TotalDays / 7;
                }
                else if (drdTarife.SelectedValue.Equals("ay"))
                {
                    sure = ts.TotalDays / 30;
                }

            }
            txtSure.Text = sure.ToString();
        }

        protected void btnSureHesapla_Click(object sender, EventArgs e)
        {
            sure_hesapla();
            tutar_hesaplaSureli();
        }

        protected void txtSure_TextChanged(object sender, EventArgs e)
        {
            tutar_hesaplaSureli();
        }
    }
}