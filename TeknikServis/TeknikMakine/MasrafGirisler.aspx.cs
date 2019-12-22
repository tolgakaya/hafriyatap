﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServisDAL;
using TeknikServis.Logic;
using System.IO;
using System.Text;
using TeknikServis.Radius;

namespace TeknikServis.TeknikMakine
{
    public partial class MasrafGirisler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                AyarCurrent ay = new AyarCurrent(dc);
                if (ay.lisansKontrol() == false)
                {
                    Response.Redirect("/LisansError");
                }
                if (!IsPostBack)
                {
                    Ara(dc);
                }

            }


        }

        private void Ara(radiusEntities dc)
        {

            DateTime baslangic = DateTime.Now.AddMonths(-3);
            if (!String.IsNullOrEmpty(datetimepicker6.Value))
            {
                baslangic = DateTime.Parse(datetimepicker6.Value);
            }

            DateTime bitis = DateTime.Now;
            if (!String.IsNullOrEmpty(datetimepicker7.Value))
            {
                bitis = DateTime.Parse(datetimepicker7.Value);
            }

            MakineIslem s = new MakineIslem(dc);

            string makine_id = Request.QueryString["makineid"];
            string masraf_id = Request.QueryString["masrafid"];


            GirisOzet ozet = s.Girisler(baslangic, bitis, makine_id, masraf_id);

            txtAdet.InnerHtml = "İşlem :" + ozet.adet.ToString();
            txtMiktar.InnerHtml = "Miktar :" + ozet.miktar.ToString();
            txtTutar.InnerHtml = "Tutar :" + ozet.tutar.ToString("C");

            grdAlimlar.DataSource = ozet.liste;
            grdAlimlar.DataBind();


        }

        protected void AlimAra(object sender, EventArgs e)
        {
            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                Ara(dc);
            }
        }


        protected void grdAlimlar_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        Ara(dc);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.error('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }

                }

            }
        }

        protected void grdAlimlar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlimlar.PageIndex = e.NewPageIndex;
            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                Ara(dc);
            }

        }
        protected void btnPrnt_Click(object sender, EventArgs e)
        {
            //Session["ctrl"] = GridView1;
            //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>");

            grdAlimlar.AllowPaging = false;
            grdAlimlar.RowStyle.ForeColor = System.Drawing.Color.Black;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            grdAlimlar.RenderControl(hw);
            string gridHTML = sw.ToString().Replace("\"", "'")
                .Replace(System.Environment.NewLine, "");
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload = new function(){");
            sb.Append("var printWin = window.open('', '', 'left=0");
            sb.Append(",top=0,width=1000,height=600,status=0');");
            sb.Append("printWin.document.write(\"");
            sb.Append(gridHTML);
            sb.Append("\");");
            sb.Append("printWin.document.close();");
            sb.Append("printWin.focus();");
            sb.Append("printWin.print();");
            sb.Append("printWin.close();};");
            sb.Append("</script>");

            ScriptManager.RegisterStartupScript(grdAlimlar, this.GetType(), "GridPrint", sb.ToString(), false);
            grdAlimlar.AllowPaging = true;

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Fatura Listesi-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(grdAlimlar, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Fatura Listesi-" + DateTime.Now.ToString();
            ExportHelper.ToWord(grdAlimlar, isim);
        }

        protected void grdAlimlar_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnTedarikci") as LinkButton).PostBackUrl = "~/TeknikMakine/MakineTek?makineid=" + DataBinder.Eval(e.Row.DataItem, "makine_id");
             
            }

        }

        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {
            //string custidd = Request.QueryString["custid"];
            //Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);

        }

        

        protected void grdAlimlar_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

    }
}