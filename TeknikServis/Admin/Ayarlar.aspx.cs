using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using ServisDAL;
using System.Linq;
using TeknikServis.Radius;
using System.Collections.Generic;

namespace TeknikServis.Admin
{
    public partial class Ayarlar : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }


            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                if (!IsPostBack)
                {
                    MailAyarGoster(ayarlar);
                    SmsAyarGoster(ayarlar);

                }


                tipGoster(ayarlar);
                birimGoster(ayarlar);
                masrafGoster(ayarlar);

            }


        }

        private void birimGoster(AyarIslemleri ayarlar)
        {
            grdBirim.DataSource = ayarlar.birimler();
            grdBirim.DataBind();
        }

        private void tipGoster(AyarIslemleri ayarlar)
        {
            grdTip.DataSource = ayarlar.tipListesiGrid();
            grdTip.DataBind();
        }
        private void masrafGoster(AyarIslemleri ayarlar)
        {

            grdTipM.DataSource = ayarlar.masrafListesiGrid();
            grdTipM.DataBind();
        }

        private void MailAyarGoster(AyarIslemleri ayarlar)
        {

            //AyarIslemleri ayarlarimiz = new AyarIslemleri(firma);
            Radius.ayar mail_ayar = ayarlar.MailAyarR();
            if (mail_ayar != null)
            {
                txtMailKimden.Text = mail_ayar.Mail_Kimden;
                txtMailKullanici.Text = mail_ayar.Mail_UserName;
                txtMailPort.Text = mail_ayar.Mail_Port.ToString();
                txtMailServer.Value = mail_ayar.Mail_Server;
                txtMailSifre.Text = mail_ayar.Mail_PW;
                txtMailAktif.Text = mail_ayar.aktif_adres;
            }

        }

        private void SmsAyarGoster(AyarIslemleri ayarlar)
        {

            Radius.ayar mail_ayar = ayarlar.SmsAyarR();
            if (mail_ayar != null)
            {
                txtMailKimden2.Text = mail_ayar.Mail_Kimden;
                txtMailKullanici2.Text = mail_ayar.Mail_UserName;

                drdSaglayici.SelectedValue = mail_ayar.Mail_Server;
                txtMailSifre2.Text = mail_ayar.Mail_PW;
                txtSmsAktif.Text = mail_ayar.aktif_adres;
            }

        }



        protected void btnAdd_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript2", sb.ToString(), false);

        }


        protected void btnMailKaydet_Click(object sender, EventArgs e)
        {

            //AyarIslemleri ayarimiz = new AyarIslemleri(firma);
            string serverimiz = txtMailServer.Value;
            string kimden = txtMailKimden.Text;
            int port = Int32.Parse(txtMailPort.Text);
            string username = txtMailKullanici.Text;
            string pw = txtMailSifre.Text;
            string adres = txtMailAktif.Text;
            string aktif = txtMailAktif.Text;
            if (!String.IsNullOrEmpty(serverimiz))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarIslemleri ayarlar = new AyarIslemleri(dc);
                    ayarlar.MailAyarKaydetR(serverimiz, kimden, port, username, pw, aktif);
                }


            }

        }

        protected void btnSmsKaydet_Click(object sender, EventArgs e)
        {
            string saglayici = drdSaglayici.SelectedValue;
            string gonderen = txtMailKimden2.Text;

            string kull = txtMailKullanici2.Text;
            string sifre = txtMailSifre2.Text;
            string aktif = txtSmsAktif.Text;
            if (!String.IsNullOrEmpty(saglayici))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarIslemleri ayarlar = new AyarIslemleri(dc);
                    ayarlar.SmsAyarKaydetR(saglayici, gonderen, kull, sifre, aktif);
                }


            }

        }
        protected void grdBirim_RowCommand(object sender, GridViewCommandEventArgs e)
        {
 
             if (e.CommandName.Equals("del"))
            {

                string confirmValue = Request.Form["confirm_value3"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {
                    int code2 = Int32.Parse(e.CommandArgument.ToString().Trim());

                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        AyarIslemleri ayarlar = new AyarIslemleri(dc);
                        ayarlar.birim_sil(code2); ;

                        grdBirim.DataSource = ayarlar.birimler();
                        grdBirim.DataBind();

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTip", sb.ToString(), false);
                    }

                }

            }
           
        }
        protected void grdTip_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("detail"))
            {

                int code2 = Int32.Parse(e.CommandArgument.ToString().Trim());
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarIslemleri ayarlar = new AyarIslemleri(dc);
                    DetailsViewTip.DataSource = ayarlar.tipListesiTekliR(code2);
                    DetailsViewTip.DataBind();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#detailModalTip').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScriptTip", sb.ToString(), false);
                }

            }
            else if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {
                    int code2 = Int32.Parse(e.CommandArgument.ToString().Trim());

                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        AyarIslemleri ayarlar = new AyarIslemleri(dc);
                        ayarlar.tipSilR(code2); ;

                        grdTip.DataSource = ayarlar.tipListesiGrid();
                        grdTip.DataBind();

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTip", sb.ToString(), false);
                    }

                }

            }
            else if (e.CommandName.Equals("editRecord"))
            {

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow gvrow = grdTip.Rows[index];

                LinkButton link = gvrow.Cells[2].Controls[1] as LinkButton;

                txtTipID.Value = HttpUtility.HtmlDecode(gvrow.Cells[1].Text);
                txtTipAd.Text = HttpUtility.HtmlDecode(link.Text);
                txtTipAciklama.Text = HttpUtility.HtmlDecode(gvrow.Cells[3].Text);
                txtCssGuncelle.Value = HttpUtility.HtmlDecode(gvrow.Cells[4].Text);

                Label2.Visible = false;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModalTip').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScriptTip2", sb.ToString(), false);

            }
        }
        protected void grdTipM_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("detail"))
            {

                int code2 = Int32.Parse(e.CommandArgument.ToString().Trim());
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarIslemleri ayarlar = new AyarIslemleri(dc);
                    DetailsViewTipM.DataSource = ayarlar.masrafListesiTekliR(code2);
                    DetailsViewTipM.DataBind();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#detailModalTipM').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScriptTip", sb.ToString(), false);
                }

            }
            else if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_valueM"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {
                    int code2 = Int32.Parse(e.CommandArgument.ToString().Trim());

                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        AyarIslemleri ayarlar = new AyarIslemleri(dc);
                        ayarlar.masrafSilR(code2); ;

                        masrafGoster(ayarlar);

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTipM", sb.ToString(), false);
                    }

                }

            }
            else if (e.CommandName.Equals("editRecord"))
            {

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow gvrow = grdTipM.Rows[index];

                LinkButton link = gvrow.Cells[2].Controls[1] as LinkButton;

                hdnTipIDM.Value = HttpUtility.HtmlDecode(gvrow.Cells[1].Text);
                txtTipAdM.Text = HttpUtility.HtmlDecode(link.Text);
                txtTipAciklamaM.Text = HttpUtility.HtmlDecode(gvrow.Cells[3].Text);
                txtCssGuncelleM.Value = HttpUtility.HtmlDecode(gvrow.Cells[4].Text);

                Label2.Visible = false;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModalTipM').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScriptTip2M", sb.ToString(), false);

            }
        }

        protected void btnAddTip_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModalTip').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScriptTip2", sb.ToString(), false);
        }
        protected void btnAddBirim_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModalBirim').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScriptTip2", sb.ToString(), false);
        }
        protected void btnAddTipM_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModalTipM').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScriptTip2M", sb.ToString(), false);
        }
 
        protected void btnSaveTip_Click(object sender, EventArgs e)
        {

            int id = Int32.Parse(txtTipID.Value);
            string ad = txtTipAd.Text;
            string aciklama = txtTipAciklama.Text;
            string css = txtCssGuncelle.Value.Trim();
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.tipGuncelleR(id, ad, aciklama, css);

                tipGoster(ayarlar);
            }


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Kaydedildi!');");
            sb.Append("$('#editModalTip').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTip", sb.ToString(), false);

        }
        protected void btnSaveTipM_Click(object sender, EventArgs e)
        {

            int id = Int32.Parse(hdnTipIDM.Value);
            string ad = txtTipAdM.Text;
            string aciklama = txtTipAciklamaM.Text;
            string css = txtCssGuncelleM.Value.Trim();
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.masrafGuncelleR(id, ad, aciklama, css);

                masrafGoster(ayarlar);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kaydedildi!');");
                sb.Append("$('#editModalTipM').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTipM", sb.ToString(), false);
            }


        }
        protected void btnDelTip_Click(object sender, EventArgs e)
        {
            int code2 = Int32.Parse(txtTipID.Value);
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.tipSilR(code2); ;

                tipGoster(ayarlar);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kayıt silindi!');");
                sb.Append("$('#editModalTip').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTip", sb.ToString(), false);
            }

        }
        protected void btnDelTipM_Click(object sender, EventArgs e)
        {
            int code2 = Int32.Parse(hdnTipIDM.Value);
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.masrafSilR(code2); ;

                masrafGoster(ayarlar);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kayıt silindi!');");
                sb.Append("$('#editModalTipM').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTipM", sb.ToString(), false);
            }

        }

        protected void btnAddRecordBirim_Click(object sender, EventArgs e)
        {

            //int id = Int32.Parse(txtTipIDGoster.Text);
            string ad = txtBirimAdGoster.Text;
          
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.birim_ekle(ad);

                birimGoster(ayarlar);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kayıt yapıldı!');");
                // sb.Append(alert);
                sb.Append("$('#addModalBirim').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScriptTip2", sb.ToString(), false);
            }



        }
        protected void btnAddRecordTip_Click(object sender, EventArgs e)
        {

            //int id = Int32.Parse(txtTipIDGoster.Text);
            string ad = txtTipAdGoster.Text;
            string aciklama = txtTipAciklamaGoster.Text;
            string css = txtCss.Value.Trim();
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.tipEkleR(ad, aciklama, css);

                tipGoster(ayarlar);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kayıt yapıldı!');");
                // sb.Append(alert);
                sb.Append("$('#addModalTip').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScriptTip2", sb.ToString(), false);
            }



        }
        protected void btnAddRecordTipM_Click(object sender, EventArgs e)
        {

            //int id = Int32.Parse(txtTipIDGoster.Text);
            string ad = txtTipAdGosterM.Text;
            string aciklama = txtTipAciklamaGosterM.Text;
            string css = txtCssM.Value.Trim();
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.masrafEkleR(ad, aciklama, css);

                masrafGoster(ayarlar);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kayıt yapıldı!');");
                // sb.Append(alert);
                sb.Append("$('#addModalTipM').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScriptTip2M", sb.ToString(), false);
            }



        }
        protected void btnPayPal_Click(object sender, EventArgs e)
        {

        }
    }
}