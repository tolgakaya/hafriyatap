using ServisDAL;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Web.UI;
using TeknikServis.Radius;
using System.Collections.Generic;
using ServisDAL.Repo;

namespace TeknikServis.TeknikMakine
{
    public partial class SerbestOperatorCalismalari : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }


            if (!IsPostBack)
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }

                    ortak(dc);

                }
            }

        }

        private void ortak(radiusEntities dc)
        {
            string kullanici = User.Identity.Name;

            Operator tek = new Operator(dc, kullanici);

            var liste = tek.SerbestCalismaListesi();

            GridView1.DataSource = liste;

            GridView1.DataBind();


        }


        protected void btnKaydetMakine_Click(object sender, EventArgs e)
        {

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {

                int hesapid = Int32.Parse(hdnHesapID.Value);
                ServisIslemleri s = new ServisIslemleri(dc);
                var hesap = s.tekserbest(hesapid);

                string custidd = hdnCari.Value;
                if (!String.IsNullOrEmpty(custidd))
                {
                    int custid = Int32.Parse(custidd);

                    string kimlik = Araclar.KimlikUret(10);

                    string islem = hesap.IslemParca;

                    //string aciklama = hesap.Aciklama;

                    string aciklama = "";
                    if (!String.IsNullOrEmpty(txtAciklama.Value))
                    {
                        aciklama = txtAciklama.Value;
                    }
                    else
                    {
                        aciklama = hesap.Aciklama;
                    }


                    string makine = txtMakine.Value;

                    int makine_id = (int)hesap.makine_id;

                    DateTime karar_tarihi = hesap.TarihZaman;

                    string tarife_kodu = hesap.tarife_kodu;
                    int tarifeid = (int)hesap.tarifeid;
                    decimal sure_saat = hesap.calisma_saati;
                    decimal son = hesap.bitis;
                    decimal baslangic = hesap.baslangic;
                    DateTime baslama_tarih = hesap.baslangic_tarih; DateTime bitis_tarih = hesap.bitis_tarih;

                    if (makine_id > -1)
                    {

                        int dakika = hesap.dakika;

                        son = hesap.bitis;

                        string tarife_tipi = hesap.tarife_tipi;
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
                        w.kdv = (decimal)hesap.KDV;
                        w.makine_id = makine_id;
                        w.tarife_kodu = tarife_kodu;
                        w.tarih = DateTime.Now;
                        w.tutar = (decimal)hesap.Tutar;
                        w.yekun = (decimal)hesap.Yekun;
                        w.yeni_sayac = son;
                        w.tarife_tipi = tarife_tipi;
                        w.tarifeid = tarifeid;
                        w.toplam_sayac = sayac_farki;
                        w.sure_aciklama = hesap.sure_aciklama;

                        s.servisEkleMakineli(custid, hesap.kullanici, aciklama, kimlik, aciklama, DateTime.Now, w, hesap.kullanici);
                        s.SerbestOnay(hesapid);

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Hesap onaylandı!');");
                        sb.Append("$('#yeniMakineModal').modal('hide');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniiiiMakineModalScript", sb.ToString(), false);

                        ortak(dc);

                    }



                }
            }
        }



        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("del"))
            {

                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {

                    int hesapID = Convert.ToInt32(e.CommandArgument);
                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        Operator o = new Operator(dc, User.Identity.Name);
                        o.serbest_sil(hesapID);
                        ortak(dc);
                    }


                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('Kayıt silindi!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                }


                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('" + deger + "');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                }


            }
            else if (e.CommandName.Equals("cikis"))
            {

                int hesapID = Convert.ToInt32(e.CommandArgument);
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    Operator s = new Operator(dc, User.Identity.Name);
                    var hesap = s.SerbestCalismaTek(hesapID);

                    hdnMakineID.Value = hesap.makine_id.ToString();
                    hdnHesapID.Value = hesap.hesapID.ToString();
                    tarifeCek(dc);

                    drdTarife.SelectedValue = hesap.tarifeid.ToString();
                    string secilen = hdnSaatlik.Value;
                    if (secilen.Equals("yes"))
                    {
                        numara_aralik.Visible = true;
                        tarih_aralik.Visible = false;
                    }
                    else
                    {
                        numara_aralik.Visible = false;
                        tarih_aralik.Visible = true;
                    }


                    txtIslemParcaMakine.Value = hesap.islemParca;


                    //datetimepicker6.Text = "";
                    //datetimepicker7.Text = "";
                    txtSonNumara.Text = hesap.baslangic.ToString();
                    txtYeniNumara.Text = hesap.son.ToString();
                    txtSure.Text = hesap.sure.ToString();
                    txtTarihMakine.Value = hesap.tarihZaman.ToShortDateString();
                    txtMakine.Value = hesap.cihaz;

                    //txtMakineAdiGoster.Value = "";
                }







                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#yeniMakineModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniiiiMakineModalScript", sb.ToString(), false);
            }

        }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ortak(dc);
            }

        }




        protected void drdTarife_SelectedIndexChanged(object sender, EventArgs e)
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

                }
                else
                {
                    numara_aralik.Visible = false;
                    tarih_aralik.Visible = true;
                    hdnSaatlik.Value = string.Empty;
                    hdnTarifeTipi.Value = t.tarife_kodu;

                }


            }

        }

        private void tarifeCek(radiusEntities dc)
        {

            int id = Convert.ToInt32(hdnMakineID.Value);
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


            if (tarifeler.Count > 0)
            {

                int tarifeid = Convert.ToInt32(drdTarife.SelectedValue);

                Tarife t = m.tarife_tek(tarifeid);

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

                hdnSaatlik.Value = string.Empty;
            }

        }




        protected void grdAlimlarGirisler_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {
                    string firma = KullaniciIslem.firma();
                    using (radiusEntities dc = MyContext.Context(firma))
                    {
                        int id = Convert.ToInt32(e.CommandArgument);
                        //alım iptal ediliyor
                        MakineIslem al = new MakineIslem(dc);
                        al.GirisIptal(id);
                        ortak(dc);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.error('Kayıt silindi!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }

                }

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    string onay = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "onayDurumu"));
            //    LinkButton link = e.Row.Cells[0].Controls[3] as LinkButton;


            //    if (onay == "EVET")
            //    {
            //        link.Visible = false;

            //            //e.Row.Cells[0].Visible = false;

            //    }

            //}

        }

        protected void Button1_ServerClick(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#musteriModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
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
        protected void MusteriAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                musteriGoster(dc);
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
    }
}