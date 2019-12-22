using ServisDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Linq;
using TeknikServis.Radius;

namespace TeknikServis.TeknikMakine
{
    public partial class MasrafGir : System.Web.UI.Page
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

                    MasrafAraa(dc);
                    MakineIslem m = new MakineIslem(dc);
                    string makine_id = Request.QueryString["makineid"];
                    if (!String.IsNullOrEmpty(makine_id))
                    {
                        int makineid = Int32.Parse(makine_id);
                        var mak = m.tekmakine(makineid);
                        baslik.InnerHtml = mak.adi + "-" + mak.plaka;
                    }

                }

                // detaylara bakalım
                DetayGoster();

            }

        }

        private void DetayGoster()
        {
            if (Session["alimdetay"] != null)
            {
                List<MakineGiris> detaylar = (List<MakineGiris>)Session["alimdetay"];
                grdDetay.DataSource = detaylar;
                grdDetay.DataBind();
                //upBilgi.Update();
            }
        }

        public void MasrafAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                MasrafAraa(dc);
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


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript2", sb.ToString(), false);

        }


        protected void grdMasraf_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }


        protected void grdMasraf_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                MasrafAraa(dc);
                if (grdMasraf.SelectedValue != null)
                {
                    lblBirimMasraf.Text = grdMasraf.SelectedRow.Cells[8].Text;
                }
            }

        }

        protected void grdDetay_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {

                    if (Session["alimdetay"] != null)
                    {
                        List<MakineGiris> detaylar = (List<MakineGiris>)Session["alimdetay"];

                        int id = Convert.ToInt32(e.CommandArgument);

                        MakineGiris d = detaylar.FirstOrDefault(x => x.masraf_id == id);
                        detaylar.Remove(d);
                        Session["alimdetay"] = detaylar;
                        DetayGoster();
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }


                }


            }
        }

        protected void grdDetay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void btnDetayEkleMasraf_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detayModalMasraf').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalMasrafScript", sb.ToString(), false);
        }


        protected void btnDetayKaydetMasraf_Click(object sender, EventArgs e)
        {
            string makineids = Request["makineid"];

            if (!String.IsNullOrEmpty(makineids))
            {
                int makineid = Int32.Parse(makineids);

                decimal adet = Decimal.Parse(txtAdetMasraf.Text);
                string ad = "";
                string birim = lblBirimMasraf.Text;
                int masraf_id = -1;

                List<MakineGiris> detaylar = new List<MakineGiris>();
                if (Session["alimdetay"] != null)
                {
                    detaylar = (List<MakineGiris>)Session["alimdetay"];
                }

                decimal birim_maliyet = 0;
                if (grdMasraf.SelectedValue != null)
                {

                    masraf_id = Convert.ToInt32(grdMasraf.SelectedValue);
                    ad = grdMasraf.SelectedRow.Cells[2].Text;
                    birim_maliyet = Decimal.Parse(grdMasraf.SelectedRow.Cells[7].Text);

                }


                bool sifirla = false;
                if (chcSayacSifirla.Checked)
                {
                    sifirla = true;
                }

                MakineGiris detay = new MakineGiris();
                detay.aciklama = txtDetayAciklamaMasraf.Text;
                detay.belge_no = "bakarız";
                detay.makine_adi = "önemsiz";
                detay.makine_id = makineid;
                detay.makine_plaka = "ödemsiz";
                detay.masraf_adi = ad;
                detay.birim = birim;
                detay.masraf_id = masraf_id;
                detay.miktar = adet;
                detay.tarih = DateTime.Now;
                detay.tutar = adet * birim_maliyet;
                detay.id = 0;
                detay.sifirla = sifirla;

                detaylar.Add(detay);
                Session["alimdetay"] = detaylar;
                DetayGoster();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kalem Eklendi!');");
                sb.Append("$('#detayModalMasraf').modal('hide');");
                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalMasrafScript", sb.ToString(), false);

            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.error('Lütfen önce makine seçiniz!');");

                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalMasrafScript", sb.ToString(), false);
            }

        }


        protected void btnMasrafKaydet_Click(object sender, EventArgs e)
        {
            //yeni masraf tanımlama eklenecek


        }
        protected void btnYeniCihaz_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detayModal').modal('hide');");
            sb.Append("$('#cihazModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
        }
        protected void btnYeniMasraf_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detayModal').modal('hide');");
            sb.Append("$('#cihazModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
        }
        protected void btnAlimKaydet_Click(object sender, EventArgs e)
        {
            string makineids = Request["makineid"];
            if (!String.IsNullOrEmpty(makineids))
            {

                int makineid = Int32.Parse(makineids);

                DateTime islem_tarih = DateTime.Now;
                //string tars = tarih2.Value;

                //if (!String.IsNullOrEmpty(tars))
                //{
                //    islem_tarih = DateTime.Parse(tars);
                //}


                string firma = KullaniciIslem.firma();
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    MakineIslem a = new MakineIslem(dc);


                    if (Session["alimdetay"] != null)
                    {
                        List<MakineGiris> detaylar = (List<MakineGiris>)Session["alimdetay"];

                        a.masraf_girisi(detaylar);
                        string back = Request.QueryString["back"];

                        if (back.Equals("servis"))
                        {
                            string kimlik = Request.QueryString["kimlik"];
                            string servisid = Request.QueryString["servisid"];
                            string custid = Request.QueryString["custid"];
                            Response.Redirect("/TeknikTeknik/Servis?servisid=" + servisid + "&kimlik=" + kimlik + "&custid=" + custid);

                        }
                        else if (back.Equals("makine"))
                        {

                            Response.Redirect("/TeknikMakine/MakineTek?makineid=" + makineid);
                        }
                        else
                        {
                            Response.Redirect("/TeknikMakine/Makineler");
                        }

                    }

                }

                Session["alimdetay"] = null;



            }
        }
    }
}