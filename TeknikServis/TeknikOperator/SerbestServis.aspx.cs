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

namespace TeknikServis.TeknikOperator
{
    public partial class SerbestServis : System.Web.UI.Page
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
                    makineGoster(dc);
                }
            }

        }

        private void ortak(radiusEntities dc)
        {
            string kullanici = User.Identity.Name;

            TekServisOperatorSerbest tek = new TekServisOperatorSerbest(dc, kullanici);
            ServisInfo s = tek.servis();
            ServisRepo genel = s.genel;


            var liste = s.kararlar;

            GridView1.DataSource = liste;


            GridView1.DataBind();


        }


        protected void btnKaydetMakine_Click(object sender, EventArgs e)
        {

            //HESAP ID HDNYE GÖRE DÜZENLEME YAPILIYOR
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {

                //yeni ekleme
                ServisIslemleri s = new ServisIslemleri(dc);

                string islem = txtIslemParcaMakine.Value;

                decimal kdv = Decimal.Parse(txtKDVOraniDuzenleMakine.Text);
                decimal yekun = Decimal.Parse(txtYekunMakine.Text);
                string aciklama = txtAciklamaMakine.Text;

                //int makine_id = -1;
                string makine = txtMakineAdiGoster.Value;
                //if (grdMakine.SelectedIndex > -1)
                //{
                int makine_id = Convert.ToInt32(grdMakine.SelectedValue);
                //}

                DateTime karar_tarihi = DateTime.Now;
                string tarS = txtTarihMakine.Value;
                if (!String.IsNullOrEmpty(tarS))
                {
                    karar_tarihi = DateTime.Parse(tarS);
                }
                string tarife_kodu = drdTarife.SelectedItem.Text;
                int tarifeid = Convert.ToInt32(drdTarife.SelectedValue);
                decimal sure_saat = 0;
                decimal son = Decimal.Parse(txtSonNumara.Text);
                decimal baslangic = Decimal.Parse(txtSonNumara.Text);
                DateTime baslama_tarih = DateTime.Now; DateTime bitis_tarih = DateTime.Now;

                if (makine_id > -1)
                {
                    if (!String.IsNullOrEmpty(datetimepicker6.Text) && !String.IsNullOrEmpty(datetimepicker7.Text))
                    {
                        baslama_tarih = DateTime.Parse(datetimepicker6.Text);
                        bitis_tarih = DateTime.Parse(datetimepicker7.Text);
                    }
                    int dakika = 0;

                    if (!String.IsNullOrEmpty(hdnSaatlik.Value))
                    {
                        string dakikaS = txtDakika.Text;

                        if (!String.IsNullOrEmpty(dakikaS))
                        {
                            sure_saat = Decimal.Parse(dakikaS) / 60;
                            dakika = Int32.Parse(dakikaS);
                        }
                        if (!String.IsNullOrEmpty(txtYeniNumara.Text))
                        {
                            son = Decimal.Parse(txtYeniNumara.Text);
                        }

                    }
                    else
                    {
                        string sureS = txtSure.Text;
                        if (!String.IsNullOrEmpty(sureS))
                        {
                            sure_saat = Decimal.Parse(sureS);
                        }
                    }

                    string sure_aciklama = txtSaatBilgi.Text;

                    string tarife_tipi = hdnTarifeTipi.Value;
                    decimal sayac_farki = son - baslangic;
                    s.kararekle_operator_seyyar(islem, kdv, yekun, aciklama, makine_id, makine, karar_tarihi, User.Identity.Name, tarife_kodu, baslangic, son, sure_saat, baslama_tarih, bitis_tarih, son, dakika, tarife_tipi, tarifeid, sayac_farki, sure_aciklama);

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");

                    sb.Append(" alertify.success('Hesap kaydedildi!');");
                    sb.Append("$('#yeniMakineModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniHideModalScript", sb.ToString(), false);

                }
                ortak(dc);
            }
        }


        protected void grdMakine_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (grdMakine.SelectedValue != null)
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    tarifeCek(dc);
                }
            }


        }

        private void fiyatCek(radiusEntities dc)
        {
            if (grdMakine.SelectedValue != null && drdTarife.SelectedValue != null)
            {
                int id = Convert.ToInt32(grdMakine.SelectedValue);
                int tarifeid = Convert.ToInt32(drdTarife.SelectedValue);

                MakineIslem m = new MakineIslem(dc);
                Makine mak = m.tekmakine(id);
                Tarife t = m.tarife_tek(tarifeid);

                txtFiyat.Text = t.tutar.ToString();

                txtSonNumara.Text = mak.son_sayac.ToString();

                txtMakineAdiGoster.Value = mak.adi;

            }
        }

        protected void btnAddMakine_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#yeniMakineModal').modal('hide');");
            sb.Append("$('#addModalMakine').modal('show');");

            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }

        protected void MakineAra(object sender, EventArgs e)
        {

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);

                makineGoster(dc);
            }
        }


        private void makineGoster(radiusEntities dc)
        {
            MakineIslem c = new MakineIslem(dc);

            string arama_terimi = txtMakineAra.Value;
            grdMakine.DataSource = c.makinelerOperator(arama_terimi, User.Identity.Name);
            grdMakine.DataBind();

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
                        ServisIslemleri s = new ServisIslemleri(dc);
                        s.serbest_sil(hesapID);
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

        }


        protected void btnEkleMakine_ClickH(object sender, EventArgs e)
        {

            //string secilen = drdTarife.SelectedValue;
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
            grdMakine.SelectedIndex = -1;

            txtAciklamaMakine.Text = "";
            txtIslemParcaMakine.Value = "";
            //burda normal KDV miktarı varama text alanında sadece oranını gösteriyoruz
            txtKDVDuzenleMakine.Text = "";

            txtKDVOraniDuzenleMakine.Text = "18";

            txtYekunMakine.Text = "";

            datetimepicker6.Text = "";
            datetimepicker7.Text = "";
            txtSonNumara.Text = "";
            txtYeniNumara.Text = "";
            txtSure.Text = "";
            txtDakika.Text = "";

            txtMakineAdiGoster.Value = "";


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#yeniMakineModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniMakineModalScript", sb.ToString(), false);


        }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ortak(dc);
            }

        }


   
        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                TextBox resim = (TextBox)e.Item.FindControl("txtYol");
                if (String.IsNullOrEmpty(resim.Text) || resim.Text == "-")
                {
                    HtmlGenericControl cerceve = (HtmlGenericControl)e.Item.FindControl("resimCerceve");
                    cerceve.Visible = false;
                }

            }
        }

        protected void drdTarife_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                int tarifeid = Convert.ToInt32(drdTarife.SelectedValue);
                MakineIslem m = new MakineIslem(dc);
                Tarife t = m.tarife_tek(tarifeid);

                fiyatCek(dc);

                if (t.saatlik == true)
                {
                    numara_aralik.Visible = true;
                    tarih_aralik.Visible = false;
                    hdnSaatlik.Value = "yes";
                    hdnTarifeTipi.Value = t.tarife_kodu;
                    dakika_hesapla();
                    tutar_hesapla();
                }
                else
                {
                    numara_aralik.Visible = false;
                    tarih_aralik.Visible = true;
                    hdnSaatlik.Value = string.Empty;
                    hdnTarifeTipi.Value = t.tarife_kodu;
                    sure_hesapla();
                }


            }

        }
        protected void txtBaslangicChanged(object sender, EventArgs e)
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
                    dakika_hesapla();
                    tutar_hesapla();
                }
                else
                {
                    numara_aralik.Visible = false;
                    tarih_aralik.Visible = true;
                    hdnSaatlik.Value = string.Empty;
                    hdnTarifeTipi.Value = string.Empty;
                    sure_hesapla();
                }

                //fiyatCek(dc);
            }

        }
        private void tarifeCek(radiusEntities dc)
        {
            if (grdMakine.SelectedValue != null)
            {
                int id = Convert.ToInt32(grdMakine.SelectedValue);
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
                txtMakineAdiGoster.Value = mak.adi;

                if (tarifeler.Count > 0)
                {

                    int tarifeid = Convert.ToInt32(drdTarife.SelectedValue);


                    Tarife t = m.tarife_tek(tarifeid);
                    txtFiyat.Text = t.tutar.ToString();

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
                    txtFiyat.Text = String.Empty;
                    hdnSaatlik.Value = string.Empty;
                }
            }
        }
        private void tutar_hesapla()
        {
            string fiyat = txtFiyat.Text;
            decimal f = 0;
            if (!String.IsNullOrEmpty(fiyat))
            {
                f = Decimal.Parse(fiyat) / 60;
                //dakika fiyatını hesaplıyor
            }
            string saatS = txtDakika.Text;
            decimal saat = 0;
            if (!String.IsNullOrEmpty(saatS))
            {
                saat = Decimal.Parse(saatS);
            }

            decimal tutar = Math.Round((f * saat), 2);
            txtYekunMakine.Text = tutar.ToString();

        }
        private void tutar_hesaplaSureli()
        {
            string fiyat = txtFiyat.Text;
            decimal f = 0;
            if (!String.IsNullOrEmpty(fiyat))
            {
                f = Decimal.Parse(fiyat);
                //dakika fiyatını hesaplıyor
            }
            string sureS = txtSure.Text;
            decimal sure = 0;
            if (!String.IsNullOrEmpty(sureS))
            {
                sure = Decimal.Parse(sureS);
            }

            decimal tutar = Math.Round((f * sure), 2);
            txtYekunMakine.Text = tutar.ToString();
        }

        private void dakika_hesapla()
        {
            string basS = txtSonNumara.Text;
            string sonS = txtYeniNumara.Text;
            int dakika = 0;
            string bilgi = "0 saat 0 dakika";

            if (!String.IsNullOrEmpty(basS) && !String.IsNullOrEmpty(sonS))
            {
                decimal bas = Decimal.Parse(basS);
                decimal son = Decimal.Parse(sonS);
                if (son > bas)
                {
                    decimal fark = son - bas;

                    int net = (int)fark;
                    decimal ondalik = fark % 1.0m;


                    dakika = net * 60 + (int)(ondalik * 10) * 6;

                    TimeSpan ts = TimeSpan.FromMinutes(dakika);
                    int gun = ts.Days;
                    int toplam_saat = gun * 24 + ts.Hours;
                    bilgi = toplam_saat + " saat " + ts.Minutes + " dakika";
                }
            }

            txtDakika.Text = dakika.ToString();
            txtSaatBilgi.Text = bilgi;

        }

        protected void txtSaat_TextChanged(object sender, EventArgs e)
        {
            tutar_hesapla();
            if (!String.IsNullOrEmpty(txtDakika.Text))
            {
                TimeSpan ts = TimeSpan.FromMinutes(Int32.Parse(txtDakika.Text));
                txtSaatBilgi.Text = ts.Hours + " saat " + ts.Minutes + " dakika";
            }

        }

        private void sure_hesapla()
        {
            string basS = datetimepicker6.Text;
            string sonS = datetimepicker7.Text;
            double sure = 0;
            if (!String.IsNullOrEmpty(basS) && !String.IsNullOrEmpty(sonS))
            {
                DateTime bas = DateTime.Parse(basS);
                DateTime son = DateTime.Parse(sonS);

                TimeSpan ts = son - bas;
                string kod = hdnTarifeTipi.Value;
                if (kod.Equals("gun"))
                {
                    sure = ts.TotalDays;
                }
                else if (kod.Equals("hafta"))
                {
                    sure = ts.TotalDays / 7;
                }
                else if (kod.Equals("ay"))
                {
                    sure = ts.TotalDays / 30;
                }

            }
            txtSure.Text = sure.ToString();
        }

        protected void btnSureHesapla_Click(object sender, EventArgs e)
        {
            sure_hesapla();
            tutar_hesaplaSureli();
        }

        protected void txtSure_TextChanged(object sender, EventArgs e)
        {
            tutar_hesaplaSureli();
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
    }
}