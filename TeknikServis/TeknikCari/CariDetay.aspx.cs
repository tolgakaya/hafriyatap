using ServisDAL;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class CariDetay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || User.IsInRole("servis"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
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
                    goster(dc);
                }

            }

        }

        private void goster(radiusEntities dc)
        {
            Hareket s = new Hareket(dc);
            string cust_id = Request.QueryString["custid"];
            int gun = 30;
            string gunS = Request.QueryString["gun"];
            if (!String.IsNullOrEmpty(gunS))
            {
                gun = Int32.Parse(gunS);
            }
            //sadece müşteriye göre bulmak için
            if (!String.IsNullOrEmpty(cust_id))
            {
                btnMusteriDetayim.Visible = true;
                int custid = Int32.Parse(cust_id);
                GridView2.DataSource = s.CariDetayRYeni(custid, gun);
                DateTime bas = DateTime.Now.AddDays(-gun);

                MusteriIslemleri m = new MusteriIslemleri(dc);
                baslik.InnerHtml = m.musteriTekAdR(custid) + " Extresi-" + bas.ToString() + " den itibaren";
            }
            GridView2.DataBind();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Extre-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView2, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Extre-" + DateTime.Now.ToString();
            ExportHelper.ToWord(GridView2, isim);
        }
        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);

        }
        protected void btnPrnt_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            if (!String.IsNullOrEmpty(custidd))
            {
                int custid = Int32.Parse(custidd);
                int gun = 3;
                string gunS = Request.QueryString["gun"];
                if (!String.IsNullOrEmpty(gunS))
                {
                    gun = Int32.Parse(gunS);
                }
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    FaturaBas bas = new FaturaBas(dc);
                    AyarCurrent ay = new AyarCurrent(dc);

                    extre extreBilgi = bas.ExtreBilgileri(custid, gun, ay.get());
                    Session["extre"] = extreBilgi;
                    string uri = "/Baski.aspx?tip=extre";
                    Response.Redirect(uri);
                }

            }


            //Session["ctrl"] = GridView2;
            //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('../Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>");
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                goster(dc);
            }
        }

        protected void drdKritik_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cust_id = Request.QueryString["custid"];

            string v = drdKritik.SelectedValue;
            if (v != "0")
            {
                Response.Redirect("/TeknikCari/CariDetay.aspx?custid=" + cust_id + "&gun=" + v);
            }
        }



        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string borc = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "borc")).Trim();
                string alacak = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "alacak")).Trim();

                if (!String.IsNullOrEmpty(borc))
                {
                    if (borc.Equals("0"))
                    {
                        e.Row.CssClass = "danger";
                    }
                    else
                    {
                        e.Row.CssClass = "info";
                    }

                }

            }
        }

        protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnDonemsel_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#topluModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "TopluModalScript", sb.ToString(), false);
        }

        protected void btnDonemselKaydet_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            if (!String.IsNullOrEmpty(custidd))
            {
                int custid = Int32.Parse(custidd);
                DateTime basla = DateTime.Now.AddDays(-30);
                DateTime son = DateTime.Now;
                if (!String.IsNullOrEmpty(datetimepicker6.Value))
                {
                    basla = DateTime.Parse(datetimepicker6.Value);
                }
                if (!String.IsNullOrEmpty(datetimepicker7.Value))
                {
                    son = DateTime.Parse(datetimepicker7.Value);
                }
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    FaturaBas bas = new FaturaBas(dc);
                    AyarCurrent ay = new AyarCurrent(dc);

                    extre extreBilgi = bas.ExtreBilgileri(custid, ay.get(), basla, son);
                    Session["extredonem"] = extreBilgi;
                    string uri = "/Baski.aspx?tip=extredonem";
                    Response.Redirect(uri);
                }

            }
        }
    }
}