using ServisDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class ServisEkle : System.Web.UI.Page
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
                    AyarIslemleri servis = new AyarIslemleri(dc);
                    List<Radius.service_tips> tipler = servis.tipListesiR();
                    if (tipler != null)
                    {
                        drdTip.AppendDataBoundItems = true;
                        drdTip.DataSource = tipler;
                        drdTip.DataValueField = "tip_id";
                        drdTip.DataTextField = "tip_ad";
                        drdTip.DataBind();
                    }



                    txtKimlikNo.Value = AletEdavat.KimlikUret(10);



                }
                if (User.IsInRole("Admin") || User.IsInRole("mudur"))
                {

                    kullaniciSecim.Visible = true;
                    if (!IsPostBack)
                    {
                        drdKullanici.AppendDataBoundItems = true;

                        drdKullanici.DataSource = KullaniciIslem.firmaKullanicilari();
                        drdKullanici.DataValueField = "id";
                        drdKullanici.DataTextField = "userName";
                        drdKullanici.DataBind();

                    }
                }

                string s = txtAra.Value;

                if (!String.IsNullOrEmpty(s))
                {
                    MusteriIslemleri m = new MusteriIslemleri(dc);

                    GridView1.DataSource = m.musteriAraR2(s, "musteri");

                    GridView1.SelectedIndex = 0;
                    GridView1.DataBind();

                }
            }

        }


        public void MusteriAra(object sender, EventArgs e)
        {
            string s = txtAra.Value;

            if (!String.IsNullOrEmpty(s))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    MusteriArama(s, dc);
                    GridView1.DataBind();

                }

            }
        }

        private void MusteriArama(string s, radiusEntities dc)
        {
            MusteriIslemleri m = new MusteriIslemleri(dc);
            GridView1.DataSource = m.musteriAraR2(s, "musteri");
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            sb.Append("document.getElementById('ContentPlaceHolder1_txtAra').value = '';");
            sb.Append("document.getElementById('ContentPlaceHolder1_chcMusteri').checked = 'checked';");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);

        }
        protected void btnAdd2_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal2').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript2", sb.ToString(), false);

        }
        //Handles Add button click in add modal popup
        protected void btnAddRecord_Click(object sender, EventArgs e)
        {

            //string id = lblMusID.Text;
            string ad = txtAdi.Text;
            string soyad = txtSoyAdi.Text;
            string adres = txtAdress.Text;
            string email = txtEmail.Text;
            string tel = txtTell.Text;
            string tc = txtTcAdd.Text;

            if (String.IsNullOrEmpty(tc))
            {
                tc = Araclar.KimlikUret(11);
            }


            string kim = txtKim.Text;
             
            string unvan = txtDuzenUnvan.Text;
            if (string.IsNullOrEmpty(unvan))
            {
                unvan = ad + " " + soyad;
            }

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                m.musteriEkleR(ad, soyad, unvan, adres, tel, tel, email, kim, tc, "", "", chcMusteri.Checked, chcTedarikci.Checked, false, chcDizServis.Checked, null);
                MusteriArama(ad, dc);
                GridView1.DataBind();
                GridView1.SelectedIndex = -1;
            }



            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(@"<script type='text/javascript'>");
            sb.Append("document.getElementById('ContentPlaceHolder1_txtAra').value = document.getElementById('ContentPlaceHolder1_txtAdi').value;");
            //sb.Append("alert('Record Added Successfully');");
            sb.Append(" alertify.success('Kayıt Eklendi!');");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("detail"))
            {
                int code = Convert.ToInt32(e.CommandArgument);
                // int code = Convert.ToInt32(GridView1.DataKeys[index].Value.ToString());
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    MusteriIslemleri m = new MusteriIslemleri(dc);

                    DetailsView1.DataSource = m.musteriTekListeR(code);
                    DetailsView1.DataBind();
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#detailModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScript", sb.ToString(), false);
            }


        }




        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = txtAra.Value;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                if (!String.IsNullOrEmpty(s))
                {

                    MusteriArama(s, dc);
                }

            }

        }


        protected void Button1_Click(object sender, EventArgs e)
        {


            kullanici_repo kullanici = KullaniciIslem.currentKullanici();
            string kimlik = txtKimlikNo.Value;
            Servis_Baslama servisBilgisi = null;
            int musID = Convert.ToInt32(GridView1.SelectedValue);
            using (radiusEntities dc = MyContext.Context(kullanici.Firma))
            {

                ServisIslemleri servis = new ServisIslemleri(dc);

                string kullaniciID = kullanici.id;
                string aciklama = txtServisAciklama.Text;

                int tipID = Convert.ToInt32(drdTip.SelectedValue);
                string atananID = "0";

                if (User.IsInRole("Admin") || User.IsInRole("mudur"))
                {
                    atananID = drdKullanici.SelectedValue;
                }


                string baslik = txtBaslik.Text;
                string firma = kullanici.Firma;

                int id = Convert.ToInt32(GridView1.SelectedValue);

                DateTime acilma_zamani = DateTime.Now;
                string zamanS = tarih2.Value;
                if (!String.IsNullOrEmpty(zamanS))
                {
                    acilma_zamani = DateTime.Parse(zamanS);
                }
                if (GridView1.SelectedIndex >= 0)
                {


                    servis.servisEkleGorevliR(musID, kullaniciID, aciklama, tipID, atananID, kimlik, baslik, acilma_zamani, User.Identity.Name);
                }




                if (cbYazdir.Checked == true)
                {
                    FaturaBas bas = new FaturaBas(dc);
                    AyarCurrent ay = new AyarCurrent(dc);
                    servisBilgisi = bas.ServisBilgileri(kimlik, ay.get());
                    Session["Servis_Baslama"] = servisBilgisi;

                }

            }




            if (cbYazdir.Checked == true && servisBilgisi != null)
            {

                string uri = "/Baski.aspx?tip=baslama";
                Response.Redirect(uri);
            }
            else
            {

                Response.Redirect("/TeknikTeknik/Servis2.aspx?kimlik=" + kimlik + "&custid=" + musID);
            }


        }


    }
}