using ServisDAL;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Web.UI;
using TeknikServis.Radius;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Web;
using System.IO;
using System.Text;
using ServisDAL.Repo;

namespace TeknikServis.TeknikMakine
{
    public partial class MakineTek : System.Web.UI.Page
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
                    //if (ay.calismaTipli() == true)
                    //{
                        tarife_listesi(dc);
                        //drdTarifeTanim.Visible = true;
                    //}
                    //else
                    //{
                    //    drdTarifeTanim.Visible = false;
                    //}


                    ortak(dc, "", "");
                    grdOperator.DataSource = KullaniciIslem.FirmaOperatorleri();
                    grdOperator.DataBind();

                }

            }
            if (!User.IsInRole("Admin") && !User.IsInRole("mudur"))
            {
                txtHesapMaliyet.Visible = false;
            }

        }

        private void tarife_listesi(radiusEntities dc)
        {
            string makine_id = Request.QueryString["makineid"];

            if (!String.IsNullOrEmpty(makine_id))
            {
                int makineid = Int32.Parse(makine_id);
                MakineIslem m = new MakineIslem(dc);
                //drdTarifeTanim.AppendDataBoundItems = true;
                drdTarifeTanim.DataSource = m.tarifeler_saatlik(makineid);
                drdTarifeTanim.DataValueField = "id";
                drdTarifeTanim.DataTextField = "ad";
                drdTarifeTanim.DataBind();
            }
        }

        private void ortak(radiusEntities dc, string bas, string son)
        {
            string makine_id = Request.QueryString["makineid"];

            if (!String.IsNullOrEmpty(makine_id))
            {
                int makineid = Int32.Parse(makine_id);

                TekMakine tek = new TekMakine(dc, makineid, bas, son);
                MakineInfo s = tek.servis();
                Makine genel = s.genel;

                var liste = s.kararlar;
                int adet = 0;
                decimal mal = 0;
                decimal tutar = 0;

                var giris = s.girisler;
                int adetg = 0;
                decimal miktarG = 0;
                decimal tutarG = 0;

                var teorik = s.teorikler;
                int adett = 0;
                decimal miktart = 0;
                decimal tutart = 0;

                if (s.girisler.Count > 0)
                {
                    adetg = s.girisler.Count;
                    miktarG = s.girisler.Sum(x => x.miktar);
                    tutarG = s.girisler.Sum(x => x.tutar);
                }

                txtAdetG.InnerHtml = "Adet: " + adetg.ToString();
                txtMiktarG.InnerHtml = "Miktar: " + miktarG.ToString("C");
                txtTutarG.InnerHtml = "Maliyet: " + tutarG.ToString("C");
                if (liste.Count > 0)
                {
                    adet = liste.Count;
                    mal = (decimal)liste.Sum(x => x.toplam_maliyet);
                    tutar = (decimal)liste.Sum(x => x.yekun);
                }


                if (teorik.Count > 0)
                {
                    adett = teorik.Count;
                    miktart = teorik.Sum(x => x.miktar);
                    tutart = teorik.Sum(x => x.tutar);

                }

                txtAdetT.InnerHtml = "Adet: " + adett.ToString();
                txtMiktarT.InnerHtml = "Miktar: " + miktart.ToString("C");
                txtTutarT.InnerHtml = "Maliyet: " + tutart.ToString("C");


                txtHesapAdet.InnerHtml = " Adet: " + adet.ToString();
                txtHesapMaliyet.InnerHtml = "Maliyet:" + mal.ToString("C");
                txtHesapTutar.InnerHtml = "Tutar: " + tutar.ToString("C");
                txtMakine_plaka.InnerHtml = genel.adi + " - " + genel.plaka;

                txtSonSayac.InnerHtml = genel.son_sayac.ToString();


                txtToplamCalismaAy.Value = genel.toplam_calisma_ay.ToString();
                txtToplamCalismaGun.Value = genel.toplam_calisma_gun.ToString();
                txtToplamCalismaHafta.Value = genel.toplam_calisma_hafta.ToString();
                txtToplamCalismaSaat.Value = genel.toplam_calisma_saat.ToString();

                txtToplamMasrafGercek.Value = genel.toplam_masraf_gercek.ToString("C");
                txtToplamMasrafTeorik.Value = genel.toplam_masraf_teorik.ToString("C");
                txtToplamGelir.Value = genel.toplam_gelir.ToString("C");
                txtServisSayaci.Value = genel.servis_sayaci.ToString();



                GridView1.DataSource = liste;
                grdAlimlarTeorik.DataSource = teorik;
                grdAlimlarGirisler.DataSource = giris;
                grdSayac.DataSource = s.sayaclar;
                grdTanim.DataSource = s.tanimlar;
                grdAtamalar.DataSource = s.atamalar;
                grdTarifeler.DataSource = s.tarifeler;

            }

            DataBind();
        }
        private void raporcu(radiusEntities dc, string bas, string son)
        {
            string makine_id = Request.QueryString["makineid"];

            if (!String.IsNullOrEmpty(makine_id))
            {
                int makineid = Int32.Parse(makine_id);

                MakineRapor tek = new MakineRapor(dc, makineid);

                MakineAnaliz s = tek.get(bas, son);

                Makine genel = s.genel;

                var liste = s.kararlar;
                int adet = 0;
                decimal mal = 0;
                decimal tutar = 0;

                var giris = s.girisler;
                int adetg = 0;
                decimal miktarG = 0;
                decimal tutarG = 0;

                var teorik = s.teorikler;
                int adett = 0;
                decimal miktart = 0;
                decimal tutart = 0;

                if (s.girisler.Count > 0)
                {
                    adetg = s.girisler.Count;
                    miktarG = s.girisler.Sum(x => x.miktar);
                    tutarG = s.girisler.Sum(x => x.tutar);
                }

                txtAdetG.InnerHtml = "Adet: " + adetg.ToString();
                txtMiktarG.InnerHtml = "Miktar: " + miktarG.ToString("C");
                txtTutarG.InnerHtml = "Maliyet: " + tutarG.ToString("C");
                if (liste.Count > 0)
                {
                    adet = liste.Count;
                    mal = (decimal)liste.Sum(x => x.toplam_maliyet);
                    tutar = (decimal)liste.Sum(x => x.yekun);
                }


                if (teorik.Count > 0)
                {
                    adett = teorik.Count;
                    miktart = teorik.Sum(x => x.miktar);
                    tutart = teorik.Sum(x => x.tutar);

                }

                txtAdetT.InnerHtml = "Adet: " + adett.ToString();
                txtMiktarT.InnerHtml = "Miktar: " + miktart.ToString("C");
                txtTutarT.InnerHtml = "Maliyet: " + tutart.ToString("C");


                txtHesapAdet.InnerHtml = " Adet: " + adet.ToString();
                txtHesapMaliyet.InnerHtml = "Maliyet:" + mal.ToString("C");
                txtHesapTutar.InnerHtml = "Tutar: " + tutar.ToString("C");
                txtMakine_plaka.InnerHtml = genel.adi + " - " + genel.plaka;

                txtSonSayac.InnerHtml = genel.son_sayac.ToString();



                txtToplamCalismaAy.Value = s.toplam_calisma_ay.ToString();
                txtToplamCalismaGun.Value = s.toplam_calisma_gun.ToString();
                txtToplamCalismaHafta.Value = s.toplam_calisma_hafta.ToString();
                txtToplamCalismaSaat.Value = s.toplam_calisma_saat.ToString();

                txtToplamMasrafGercek.Value = s.toplam_masraf_gercek.ToString("C");
                txtToplamMasrafTeorik.Value = s.toplam_masraf_teorik.ToString("C");
                txtToplamGelir.Value = s.toplam_gelir.ToString("C");
                txtServisSayaci.Value = genel.servis_sayaci.ToString();


                GridView1.DataSource = liste;
                grdAlimlarTeorik.DataSource = teorik;
                grdAlimlarGirisler.DataSource = giris;
                grdSayac.DataSource = s.sayaclar;

            }
            DataBind();
        }

        protected void Ara(object sender, EventArgs e)
        {

            string basS = datetimepicker6.Value;
            string sonS = datetimepicker7.Value;

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                raporcu(dc, basS, sonS);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string onay = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "onayDurumu"));
                LinkButton link = e.Row.Cells[0].Controls[3] as LinkButton;
                LinkButton link2 = e.Row.Cells[0].Controls[5] as LinkButton;
                //LinkButton link3 = e.Row.Cells[0].Controls[7] as LinkButton;
                if (onay == "EVET")
                {
                    link.Visible = false;
                    link2.Visible = false;
                    //link3.Visible = false;
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                string basS = datetimepicker6.Value;
                string sonS = datetimepicker7.Value;
                ortak(dc, basS, sonS);
            }

        }


        protected void btnBelge_Click(object sender, EventArgs e)
        {
            string makine_id = Request.QueryString["makineid"];
            if (!String.IsNullOrEmpty(makine_id))
            {
                int makineid = Int32.Parse(makine_id);
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    MakineRapor m = new MakineRapor(dc, makineid);
                    string basS = datetimepicker6.Value;
                    string sonS = datetimepicker7.Value;
                    Session["makineanaliz"] = m.get(basS, sonS);
                    string uri = "/Baski.aspx?tip=makineanaliz";
                    Response.Redirect(uri);
                }
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

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.error('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }

                }

            }
        }

        protected void grdSayac_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int tipi = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "sayac"));
                if (tipi > 0)
                {
                    e.Row.CssClass = "info";

                }
                else
                {
                    e.Row.CssClass = "danger";
                }

            }

        }

        protected void btnyeniSayac_Click(object sender, EventArgs e)
        {
            hdnSayacID.Value = "";
            txtSayac.Text = "";
            grdMasraf.SelectedIndex = -1;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#yeniSayacModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniSayaceModalScript", sb.ToString(), false);

        }

        protected void btnyeniTanim_Click(object sender, EventArgs e)
        {
            hdnTanimID.Value = "";
            txtSayac2.Text = "";
            txtMiktar2.Text = "";
            grdMasraf2.SelectedIndex = -1;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#yeniTanimModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniSayactModalScript", sb.ToString(), false);

        }

        protected void grdMasraf_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void grdMasraf2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void btnYeniMasraf_Click(object sender, EventArgs e)
        {

        }

        protected void btnSayacKaydet_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(hdnSayacID.Value))
            {
                if (grdMasraf.SelectedValue != null)
                {
                    int makineid = Int32.Parse(Request.QueryString["makineid"]);

                    int id = Convert.ToInt32(grdMasraf.SelectedValue);
                    string birim = grdMasraf.SelectedRow.Cells[4].Text;
                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        decimal sayac = Decimal.Parse(txtSayac.Text);
                        decimal sayac_alarm = Decimal.Parse(txtSayacAlarm.Text);
                        MakineIslem m = new MakineIslem(dc);
                        bool tamam = m.sayacekle(makineid, id, sayac, birim, sayac_alarm);
                        if (tamam == true)
                        {
                            m.sayacdelete(id);
                            string basS = datetimepicker6.Value;
                            string sonS = datetimepicker7.Value;

                            ortak(dc, basS, sonS);

                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append(" alertify.success('Sayaç kaydedildi');");
                            sb.Append("$('#yeniSayacModal').modal('hide');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniSayaceModalScript", sb.ToString(), false);
                        }
                        else
                        {
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append(" alertify.error('Bu sayaç tanımı zaten yapılmış');");
                            sb.Append("$('#yeniSayacModal').modal('hide');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniSayaceModalScript", sb.ToString(), false);
                        }
                    }
                }
            }
            else
            {
                int id = Int32.Parse(hdnSayacID.Value);

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    MakineIslem m = new MakineIslem(dc);
                    m.sayacupdate(id, Int32.Parse(txtSayac.Text));
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Sayaç kaydedildi');");
                    sb.Append("$('#yeniSayacModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniSayaceModalScript", sb.ToString(), false);


                }
            }

        }

        protected void btnTanimKaydet_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(hdnTanimID.Value))
            {
                if (grdMasraf2.SelectedValue != null)
                {
                    int makineid = Int32.Parse(Request.QueryString["makineid"]);

                    int id = Convert.ToInt32(grdMasraf2.SelectedValue);
                    string birim = grdMasraf2.SelectedRow.Cells[4].Text;
                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        int sayac = Int32.Parse(txtSayac2.Text);
                        decimal miktar = Decimal.Parse(txtMiktar2.Text);

                        decimal masraf_saat = (decimal)(miktar / sayac);

                        MakineIslem m = new MakineIslem(dc);
                        bool tamam = false;
                        AyarCurrent ay = new AyarCurrent(dc);

                        int tarifeid = Convert.ToInt32(drdTarifeTanim.SelectedValue);
                        tamam = m.tanimekle_tipli(makineid, id, tarifeid, masraf_saat, txtAciklama.Text, birim);



                        if (tamam == true)
                        {
                            //m.sayacdelete(id);
                            string basS = datetimepicker6.Value;
                            string sonS = datetimepicker7.Value;

                            ortak(dc, basS, sonS);

                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append(" alertify.success('Tanım kaydedildi');");
                            sb.Append("$('#yeniTanimModal').modal('hide');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniSayactModalScript", sb.ToString(), false);
                        }
                        else
                        {
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append(" alertify.error('Bu tanım zaten yapılmış');");
                            sb.Append("$('#yeniTanimModal').modal('hide');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniSayacetModalScript", sb.ToString(), false);
                        }
                    }
                }
            }
            else
            {
                int id = Int32.Parse(hdnTanimID.Value);

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    MakineIslem m = new MakineIslem(dc);

                    int sayac = Int32.Parse(txtSayac2.Text);
                    decimal miktar = Decimal.Parse(txtMiktar2.Text);
                    decimal masraf_saat = (decimal)(miktar / sayac);

                    m.tanimupdate(id, masraf_saat, txtAciklama.Text);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Tanım kaydedildi');");
                    sb.Append("$('#yeniTanimModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniSayacetModalScript", sb.ToString(), false);


                }
            }

        }


        public void MasrafAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                MasrafAraa(dc);
            }

        }
        public void MasrafAra2(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                MasrafAraa2(dc);
            }

        }
        protected void MasrafAraa(radiusEntities dc)
        {
            string firma = KullaniciIslem.firma();
            CihazMalzeme c = new CihazMalzeme(dc);
            string terim = txtMasrafAra.Value;
            grdMasraf.DataSource = c.MasrafListesi(terim);
            grdMasraf.DataBind();
        }
        protected void MasrafAraa2(radiusEntities dc)
        {
            string firma = KullaniciIslem.firma();
            CihazMalzeme c = new CihazMalzeme(dc);
            string terim = txtMasrafAra2.Value;
            grdMasraf2.DataSource = c.MasrafListesi(terim);
            grdMasraf2.DataBind();
        }
        protected void grdSayac_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        MakineIslem s = new MakineIslem(dc);
                        s.sayacdelete(id);
                        string basS = datetimepicker6.Value;
                        string sonS = datetimepicker7.Value;

                        ortak(dc, basS, sonS);

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
            else if (e.CommandName.Equals("duzenle"))
            {

                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');

                int index = Convert.ToInt32(arg[1]);
                GridViewRow row = grdSayac.Rows[index];
                string sayac = row.Cells[2].Text;

                hdnSayacID.Value = Convert.ToString(arg[0]);
                txtSayac.Text = sayac;

                grdMasraf.SelectedIndex = -1;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#yeniSayacModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniSayaceModalScript", sb.ToString(), false);

            }



        }

        protected void grdSayac_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void grdTanim_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {

                    string[] arg = new string[2];
                    arg = e.CommandArgument.ToString().Split(';');

                    int id = Convert.ToInt32(arg[0]);


                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {

                        MakineIslem s = new MakineIslem(dc);

                        s.tanimdelete_tipli(id);



                        string basS = datetimepicker6.Value;
                        string sonS = datetimepicker7.Value;

                        ortak(dc, basS, sonS);

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
            else if (e.CommandName.Equals("duzenle"))
            {

                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');

                int index = Convert.ToInt32(arg[1]);
                GridViewRow row = grdTanim.Rows[index];
                string sayac = row.Cells[2].Text;

                hdnTanimID.Value = Convert.ToString(arg[0]);


                grdMasraf.SelectedIndex = -1;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#yeniTanimModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniSayacetModalScript", sb.ToString(), false);

            }



        }

        protected void grdTanim_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void grdAlimlarTeorik_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                string basS = datetimepicker6.Value;
                string sonS = datetimepicker7.Value;
                ortak(dc, basS, sonS);
            }
        }

        protected void btnMasrafGir_Click(object sender, EventArgs e)
        {
            string makineid = Request.QueryString["makineid"];
            if (!String.IsNullOrEmpty(makineid))
            {
                Response.Redirect("/TeknikMakine/MasrafGir?makineid=" + makineid + "&back=makine");
            }
        }

        protected void grdAtamalar_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        Operator al = new Operator(dc, "-");
                        al.MakineOperatorIptal(id);
                        string basS = datetimepicker6.Value;
                        string sonS = datetimepicker7.Value;
                        grdOperator.DataSource = KullaniciIslem.FirmaOperatorleri();
                        grdOperator.DataBind();
                        ortak(dc, basS, sonS);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.error('Kayıt silindi!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }

                }

            }

            else if (e.CommandName.Equals("cikis"))
            {
                hdnServisOpID.Value = Convert.ToString(e.CommandArgument);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#cikisModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CikisShowModalScript", sb.ToString(), false);
            }
        }

        protected void grdOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kullanici = grdOperator.SelectedValue.ToString();
            string makineids = Request.QueryString["makineid"];

            if (!String.IsNullOrEmpty(kullanici) && !String.IsNullOrEmpty(makineids))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    Operator op = new Operator(dc, kullanici);
                    if (chcHepsi.Checked == true)
                    {
                        op.MakineOperatorKontrollu(Int32.Parse(makineids));
                    }
                    else
                    {
                        op.MakineOperatorAta(Int32.Parse(makineids));
                    }

                    string basS = datetimepicker6.Value;
                    string sonS = datetimepicker7.Value;
                    grdOperator.DataSource = KullaniciIslem.FirmaOperatorleri();
                    grdOperator.DataBind();
                    ortak(dc, basS, sonS);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#atamaModal').modal('hide');");
                    sb.Append(" alertify.error('Operatör atandı!');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AtaShowModalScript", sb.ToString(), false);
                }
            }
        }

        protected void btnAta_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#atamaModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AtaShowModalScript", sb.ToString(), false);
        }

        protected void OperatorAra(object sender, EventArgs e)
        {
            string s = txtOperatorAra.Value;
            if (!String.IsNullOrEmpty(s))
            {
                grdOperator.DataSource = KullaniciIslem.FirmaOperatorleri(s);
                grdOperator.DataBind();
            }

        }

        protected void btnCikarKaydet_Click(object sender, EventArgs e)
        {
            string ids = hdnServisOpID.Value;
            if (!String.IsNullOrEmpty(ids))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    Operator op = new Operator(dc, "-");
                    op.MakineOperatorCikar(Int32.Parse(ids));
                    string basS = datetimepicker6.Value;
                    string sonS = datetimepicker7.Value;
                    grdOperator.DataSource = KullaniciIslem.FirmaOperatorleri();
                    grdOperator.DataBind();
                    ortak(dc, basS, sonS);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#cikisModal').modal('hide');");
                    sb.Append(" alertify.error('Çıkış kaydedildi!');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AtaShowModalScript", sb.ToString(), false);
                }
            }

        }

        protected void btnAtamaKaydet_Click(object sender, EventArgs e)
        {
            string kullanici = grdOperator.SelectedValue.ToString();
            string makineids = Request.QueryString["makineid"];

            if (!String.IsNullOrEmpty(kullanici) && !String.IsNullOrEmpty(makineids))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    Operator op = new Operator(dc, kullanici);
                    if (chcHepsi.Checked == true)
                    {
                        op.MakineOperatorKontrollu(Int32.Parse(makineids));
                    }
                    else
                    {
                        op.MakineOperatorAta(Int32.Parse(makineids));
                    }

                    string basS = datetimepicker6.Value;
                    string sonS = datetimepicker7.Value;
                    grdOperator.DataSource = KullaniciIslem.FirmaOperatorleri();
                    grdOperator.DataBind();
                    ortak(dc, basS, sonS);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#atamaModal').modal('hide');");
                    sb.Append(" alertify.error('Operatör atandı!');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AtaShowModalScript", sb.ToString(), false);
                }
            }
        }

        protected void btnAddRecordTarife_Click(object sender, EventArgs e)
        {
            string tarife_tipi = drdTarifeTipi.SelectedValue;
            string makineids = Request.QueryString["makineid"];
            string calismatipi = txtCalismaTipi.Text.Trim().ToLower();
            string fiyats = txtTarifeFiyat.Text;

            if (!String.IsNullOrEmpty(makineids) && !String.IsNullOrEmpty(tarife_tipi))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {

                    MakineIslem m = new MakineIslem(dc);
                    int i = m.tarife_ekle(Int32.Parse(makineids), tarife_tipi, calismatipi, Decimal.Parse(fiyats));
                    if (i == 1)
                    {
                        string basS = datetimepicker6.Value;
                        string sonS = datetimepicker7.Value;

                        ortak(dc, basS, sonS);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#addTarifeModal').modal('hide');");
                        sb.Append(" alertify.error('Tarife tanımlandı!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AtaShowModalScript", sb.ToString(), false);
                    }
                    else if (i == -1)
                    {
                        //string basS = datetimepicker6.Value;
                        //string sonS = datetimepicker7.Value;

                        //ortak(dc, basS, sonS);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        //sb.Append("$('#addTarifeModal').modal('hide');");
                        sb.Append(" alertify.error('Bu tarife tipi ve çalışma tipi için tarife tanımı mevcut!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AtaShowModalScript", sb.ToString(), false);
                    }

                }
            }
        }

        protected void btnYeniTarife_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addTarifeModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "TarifeShowModalScript", sb.ToString(), false);
        }

        protected void grdTarifeler_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        al.tarife_iptal(id);

                        string basS = datetimepicker6.Value;
                        string sonS = datetimepicker7.Value;
                        ortak(dc, basS, sonS);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.error('Kayıt silindi!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }

                }

            }
        }


    }
}
