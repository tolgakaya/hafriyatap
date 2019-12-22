using ServisDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis.TeknikOperator
{
    public partial class SarfKayitlar : System.Web.UI.Page
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
                    goster(dc);
                    MasrafAraa(dc);
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

        protected void grdMasraf_SelectedIndexChanged(object sender, EventArgs e)
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

        public void goster(radiusEntities dc)
        {
            MakineIslem al = new MakineIslem(dc);
            string id = Request.QueryString["makineid"];
            grdAlimlarGirisler.DataSource = al.OperatorGirisleri(id, User.Identity.Name);
            grdAlimlarGirisler.DataBind();

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
                        goster(dc);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.error('Kayıt silindi!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }

                }

            }
        }

        protected void btnSarfKaydet_Click(object sender, EventArgs e)
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

                int adet = Int32.Parse(txtAdetMasraf.Text);
                string ad = "";

                int masraf_id = -1;

                List<MakineGiris> detaylar = new List<MakineGiris>();


                if (grdMasraf.SelectedValue != null)
                {

                    masraf_id = Convert.ToInt32(grdMasraf.SelectedValue);
                    ad = grdMasraf.SelectedRow.Cells[2].Text;

                }
                decimal birim_maliyet = Decimal.Parse(grdMasraf.SelectedRow.Cells[7].Text);

                bool sifirla = false;

                MakineGiris detay = new MakineGiris();
                detay.aciklama = txtDetayAciklamaMasraf.Text;
                detay.belge_no = "bakarız";
                detay.makine_adi = "önemsiz";
                detay.makine_id = makineid;
                detay.makine_plaka = "ödemsiz";
                detay.masraf_adi = ad;
                detay.masraf_id = masraf_id;
                detay.miktar = adet;
                detay.tarih = DateTime.Now;
                detay.tutar = adet * birim_maliyet;
                detay.id = 0;
                detay.kullanici = User.Identity.Name;
                detay.sifirla = sifirla;
                detaylar.Add(detay);

                DateTime islem_tarih = DateTime.Now;
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    MakineIslem a = new MakineIslem(dc);

                    if (detaylar.Count > 0)
                    {
                        a.masraf_girisi(detaylar);
                        goster(dc);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Sarf Kaydı Eklendi!');");
                        sb.Append("$('#detayModalMasraf').modal('hide');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalMasrafScript", sb.ToString(), false);

                    }

                }

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
    }
}