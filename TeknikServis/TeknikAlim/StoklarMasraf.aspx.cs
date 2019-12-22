using System;
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

namespace TeknikServis.TeknikAlim
{
    public partial class StoklarMasraf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
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

                    CihazMalzeme c = new CihazMalzeme(dc);
                    Ara(c);
                    birimGoster(c);
                }
            }



        }

        private void birimGoster(CihazMalzeme s)
        {
            var birimler = s.birimler();

            drdBirim.DataSource = birimler;
            drdBirim.DataValueField = "id";
            drdBirim.DataTextField = "birim";
            drdBirim.DataBind();

            drdBirimUp.DataSource = birimler;
            drdBirimUp.DataValueField = "id";
            drdBirimUp.DataTextField = "birim";
            drdBirimUp.DataBind();
        }
        private void Ara(CihazMalzeme s)
        {

            string terim = txtAra.Value;
            if (!String.IsNullOrEmpty(terim))
            {
                grdAlimlar.DataSource = s.MasrafListesi(terim);
            }
            else
            {
                grdAlimlar.DataSource = s.MasrafListe();
            }

            grdAlimlar.DataBind();


        }

        protected void CihazAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                CihazMalzeme c = new CihazMalzeme(dc);
                Ara(c);
            }

        }


        protected void grdAlimlar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlimlar.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                CihazMalzeme c = new CihazMalzeme(dc);
                Ara(c);
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
            string isim = "Masraf Stok Listesi-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(grdAlimlar, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Stok Listesi-" + DateTime.Now.ToString();
            ExportHelper.ToWord(grdAlimlar, isim);
        }
        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);

        }

        protected void grdAlimlar_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnDetay") as LinkButton).PostBackUrl = "~/TeknikAlim/AlimDetaylar.aspx?masrafid=" + DataBinder.Eval(e.Row.DataItem, "MasrafID");
            }
        }

        protected void grdAlimlar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //güncelleme modalı açılacak
            if (e.CommandName == "guncelle")
            {
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int ID = Convert.ToInt32(arg[0]);
                int index = Convert.ToInt32(arg[1]);

                GridViewRow row = grdAlimlar.Rows[index];
                hdnCihazID.Value = arg[0].ToString();
                cihaz_adi_up.Text = row.Cells[1].Text;
                aciklama_up.Text = row.Cells[2].Text;
                stok_up.Text = row.Cells[5].Text;
                hdnCihazStok.Value = row.Cells[5].Text;
                hdnCihazMaliyet.Value = row.Cells[6].Text;
                maliyet_up.Text = row.Cells[6].Text;
                drdBirimUp.SelectedValue = row.Cells[8].Text;


                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");

                sb.Append("$('#updateModal').modal('show');");
                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
            }
            else if (e.CommandName == "musteri")
            {
                string id = e.CommandArgument.ToString();
                Response.Redirect("/TeknikAlim/MusteriUrunAra.aspx?cihazid=" + id);
            }
        }

        protected void btnYeni_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#cihazModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
        }
        protected void btnCihazKaydet_Click(object sender, EventArgs e)
        {
            string ad = cihaz_adi.Text;
            string acik = aciklama.Text;
            string firma = KullaniciIslem.firma();
            int id = Int32.Parse(drdBirim.SelectedValue);
            string birim = drdBirim.SelectedItem.Text;

            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme m = new CihazMalzeme(dc);
                m.YeniMasraf(ad, acik, id, birim);
                Ara(m);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Cihaz tanımlandı!');");
            sb.Append("$('#cihazModal').modal('hide');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);


        }

        protected void btnCihazUpdate_Click(object sender, EventArgs e)
        {
            string ad = cihaz_adi_up.Text;
            string acik = aciklama_up.Text;

            decimal bak = Decimal.Parse(stok_up.Text);
            int id = Int32.Parse(hdnCihazID.Value.Trim());

            string firma = KullaniciIslem.firma();
            string stokS = hdnCihazStok.Value;
            string yeniStoks = stok_up.Text;
            string yenimalS = maliyet_up.Text;
            string malS = hdnCihazMaliyet.Value;

            int birimid = Int32.Parse(drdBirimUp.SelectedValue);
            string birim = drdBirimUp.SelectedItem.Text;

            decimal maliyet = 0;
            if (!string.IsNullOrEmpty(yenimalS))
            {
                maliyet = Decimal.Parse(yenimalS);
            }

            decimal stok = 0;
            if (!string.IsNullOrEmpty(yeniStoks))
            {
                stok = Decimal.Parse(yeniStoks);
            }

            using (radiusEntities dc = MyContext.Context(firma))
            {

                CihazMalzeme m = new CihazMalzeme(dc);

                m.MasrafGuncelle(stok, id, maliyet, ad, acik, birimid, birim);
                Ara(m);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Güncelleme tamamlandı!');");
            sb.Append("$('#updateModal').modal('hide');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateShowModalScript", sb.ToString(), false);
        }

        protected void grdAlimlar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int tipi = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "bakiye"));
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



    }
}