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

namespace TeknikServis.TeknikMakine
{
    public partial class Makineler : System.Web.UI.Page
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
                if (!IsPostBack)
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }
                    Ara(dc);
                }

            }

        }

        private void Ara(radiusEntities dc)
        {
            MakineIslem s = new MakineIslem(dc);
            string terim = txtAra.Value;

            grdAlimlar.DataSource = s.makineler(terim);
            grdAlimlar.DataBind();

        }

        protected void MakineAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                Ara(dc);
            }

        }


        protected void grdAlimlar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlimlar.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
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
            string isim = "Stok Listesi-" + DateTime.Now.ToString();
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
            //detay için makine masraf hesaplarına bakılabilir
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnDetay") as LinkButton).PostBackUrl = "~/TeknikMakine/MasrafGirisler.aspx?makineid=" + DataBinder.Eval(e.Row.DataItem, "makine_id");
            }
        }

        protected void grdAlimlar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //güncelleme modalı açılacak
            if (e.CommandName == "guncelle")
            {
                string[] arg = new string[1];
                arg = e.CommandArgument.ToString().Split(';');
                int ID = Convert.ToInt32(arg[0]);
                int index = Convert.ToInt32(arg[1]);

                GridViewRow row = grdAlimlar.Rows[index];
                hdnCihazID.Value = ID.ToString();
                dadi.Text = row.Cells[1].Text;
                dplaka.Text = row.Cells[2].Text;
                daciklama.Text = row.Cells[3].Text;
                dson_sayac.Text = row.Cells[4].Text;

                dtoplam_calisma_saat.Text = row.Cells[5].Text;
                dtoplam_calisma_gun.Text = row.Cells[6].Text;
                dtoplam_calisma_ay.Text = row.Cells[7].Text;
                dtoplam_calisma_hafta.Text = row.Cells[8].Text;
                dtoplam_calisma_ay.Text = row.Cells[9].Text;
                dtoplam_masraf_teorik.Text = row.Cells[10].Text;
                dtoplam_gelir.Text = row.Cells[11].Text;
                dservis_sayaci.Text = row.Cells[12].Text;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");

                sb.Append("$('#updateModal').modal('show');");
                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
            }
            else if (e.CommandName == "musteri")
            {
                //makineyle ilgili detaylara bakılabilir
                string id = e.CommandArgument.ToString();
                Response.Redirect("/TeknikMakine/MakineTek?makineid=" + id);
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

            string ddadi = adi.Text;
            string acik = aciklama.Text;
            string ddplaka = plaka.Text;
            string ddson_sayac = son_sayac.Text;
            decimal dddson_sayac = 0;
            if (!String.IsNullOrEmpty(ddson_sayac))
            {
                dddson_sayac = Decimal.Parse(ddson_sayac);
            }

            string ddtoplam_calisma_ay = toplam_calisma_ay.Text;
            decimal dddtoplam_calisma_ay = 0;
            if (!string.IsNullOrEmpty(ddtoplam_calisma_ay))
            {
                dddtoplam_calisma_ay = decimal.Parse(ddtoplam_calisma_ay);
            }

            string ddtoplam_calisma_hafta = toplam_calisma_hafta.Text;
            decimal dddtoplam_calisma_hafta = 0;
            if (!string.IsNullOrEmpty(ddtoplam_calisma_hafta))
            {
                dddtoplam_calisma_hafta = decimal.Parse(ddtoplam_calisma_hafta);
            }

            string ddtoplam_calisma_gun = toplam_calisma_gun.Text;
            decimal dddtoplam_calisma_gun = 0;
            if (!string.IsNullOrEmpty(ddtoplam_calisma_gun))
            {
                dddtoplam_calisma_gun = decimal.Parse(ddtoplam_calisma_gun);
            }

            string ddtoplam_calisma_saat = toplam_calisma_saat.Text;
            decimal dddtoplam_calisma_saat = 0;
            if (!string.IsNullOrEmpty(ddtoplam_calisma_saat))
            {
                dddtoplam_calisma_saat = decimal.Parse(ddtoplam_calisma_saat);
            }

            string ddtoplam_masraf_teorik = toplam_masraf_teorik.Text;
            decimal dddtoplam_masraf_teorik = 0;
            if (!String.IsNullOrEmpty(ddtoplam_masraf_teorik))
            {
                dddtoplam_masraf_teorik = decimal.Parse(ddtoplam_masraf_teorik);
            }

            string ddtoplam_masraf_gercek = toplam_masraf_gercek.Text;
            decimal dddtoplam_masraf_gercek = 0;
            if (!string.IsNullOrEmpty(ddtoplam_masraf_gercek))
            {
                dddtoplam_masraf_gercek = decimal.Parse(ddtoplam_masraf_gercek);
            }

            string ddtoplam_gelir = toplam_gelir.Text;
            decimal dddtoplam_gelir = 0;
            if (!string.IsNullOrEmpty(ddtoplam_gelir))
            {
                dddtoplam_gelir = decimal.Parse(ddtoplam_gelir);
            }

            string ddservis_sayaci = servis_sayaci.Text;
            decimal dddservis_sayaci = 0;
            if (!string.IsNullOrEmpty(ddservis_sayaci))
            {
                dddservis_sayaci = Decimal.Parse(ddservis_sayaci);
            }

            string firma = KullaniciIslem.firma();

            using (radiusEntities dc = MyContext.Context(firma))
            {
                AyarCurrent ay = new AyarCurrent(dc);
                int sinir = ay.makinesinir();
                CihazMalzeme m = new CihazMalzeme(dc);
                int aktif = m.makineatif();
                if (aktif < sinir)
                {
                    m.YeniMakine(ddadi, acik, ddplaka, dddson_sayac, dddtoplam_calisma_ay, dddtoplam_calisma_gun,
                dddtoplam_calisma_hafta, dddtoplam_calisma_saat, dddtoplam_masraf_teorik, dddtoplam_masraf_gercek, dddtoplam_gelir, dddservis_sayaci);
                    Ara(dc);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Makine tanımlandı!');");
                    sb.Append("$('#cihazModal').modal('hide');");
                    sb.Append(@"</script>");

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('Maksimum makine sayısına ulaştınız!');");
                    sb.Append("$('#cihazModal').modal('hide');");
                    sb.Append(@"</script>");

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript3", sb.ToString(), false);
                }


            }




        }

        protected void btnCihazUpdate_Click(object sender, EventArgs e)
        {

            string ddadi = dadi.Text;
            string acik = daciklama.Text;
            string ddplaka = dplaka.Text;
            string ddson_sayac = dson_sayac.Text;
            decimal dddson_sayac = 0;
            if (!String.IsNullOrEmpty(ddson_sayac))
            {
                dddson_sayac = Decimal.Parse(ddson_sayac);
            }



            string ddtoplam_calisma_ay = dtoplam_calisma_ay.Text;
            decimal dddtoplam_calisma_ay = 0;
            if (!string.IsNullOrEmpty(ddtoplam_calisma_ay))
            {
                dddtoplam_calisma_ay = decimal.Parse(ddtoplam_calisma_ay);
            }

            string ddtoplam_calisma_hafta = dtoplam_calisma_hafta.Text;
            decimal dddtoplam_calisma_hafta = 0;
            if (!string.IsNullOrEmpty(ddtoplam_calisma_hafta))
            {
                dddtoplam_calisma_hafta = decimal.Parse(ddtoplam_calisma_hafta);
            }

            string ddtoplam_calisma_gun = dtoplam_calisma_gun.Text;
            decimal dddtoplam_calisma_gun = 0;
            if (!string.IsNullOrEmpty(ddtoplam_calisma_gun))
            {
                dddtoplam_calisma_gun = decimal.Parse(ddtoplam_calisma_gun);
            }

            string ddtoplam_calisma_saat = dtoplam_calisma_saat.Text;
            decimal dddtoplam_calisma_saat = 0;
            if (!string.IsNullOrEmpty(ddtoplam_calisma_saat))
            {
                dddtoplam_calisma_saat = decimal.Parse(ddtoplam_calisma_saat);
            }

            string ddtoplam_masraf_teorik = dtoplam_masraf_teorik.Text;
            decimal dddtoplam_masraf_teorik = 0;
            if (!String.IsNullOrEmpty(ddtoplam_masraf_teorik))
            {
                dddtoplam_masraf_teorik = decimal.Parse(ddtoplam_masraf_teorik);
            }

            string ddtoplam_masraf_gercek = dtoplam_masraf_gercek.Text;
            decimal dddtoplam_masraf_gercek = 0;
            if (!string.IsNullOrEmpty(ddtoplam_masraf_gercek))
            {
                dddtoplam_masraf_gercek = decimal.Parse(ddtoplam_masraf_gercek);
            }

            string ddtoplam_gelir = dtoplam_gelir.Text;
            decimal dddtoplam_gelir = 0;
            if (!string.IsNullOrEmpty(ddtoplam_gelir))
            {
                dddtoplam_gelir = decimal.Parse(ddtoplam_gelir);
            }

            string ddservis_sayaci = dservis_sayaci.Text;
            decimal dddservis_sayaci = 0;
            if (!string.IsNullOrEmpty(ddservis_sayaci))
            {
                dddservis_sayaci = Decimal.Parse(ddservis_sayaci);
            }

            if (!string.IsNullOrEmpty(hdnCihazID.Value))
            {
                int id = Int32.Parse(hdnCihazID.Value);
                string firma = KullaniciIslem.firma();

                using (radiusEntities dc = MyContext.Context(firma))
                {
                    CihazMalzeme m = new CihazMalzeme(dc);
                    m.MakineGuncelle(id, ddadi, acik, ddplaka, dddson_sayac, dddtoplam_calisma_ay, dddtoplam_calisma_gun,
                        dddtoplam_calisma_hafta, dddtoplam_calisma_saat, dddtoplam_masraf_teorik, dddtoplam_masraf_gercek, dddtoplam_gelir, dddservis_sayaci);
                    Ara(dc);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Güncelleme tamamlandı!');");
                sb.Append("$('#updateModal').modal('hide');");
                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateShowModalScript", sb.ToString(), false);
            }

        }

        protected void grdAlimlar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int tipi = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "servis_sayaci"));
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