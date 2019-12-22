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

namespace TeknikServis.TeknikAlim
{
    public partial class SatinAl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }


            if (!IsPostBack)
            {
                string firma = KullaniciIslem.firma();
                using (radiusEntities dc = MyContext.Context(firma))
                {

                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }
                    CihazAraa(dc);
                    MasrafAraa(dc);
                    makineGoster(dc);
                    string s = txtAra.Value;

                    if (!String.IsNullOrEmpty(s) && !String.IsNullOrWhiteSpace(s))
                    {
                        MusteriIslemleri m = new MusteriIslemleri(dc);

                        GridView1.DataSource = m.musteriAraR2(s, "tedarikci");
                        GridView1.DataBind();

                    }

                }
            }



            // detaylara bakalım
            //DetayGoster();



        }

        private void DetayGoster()
        {
            if (Session["alimdetay"] != null)
            {

                List<DetayRepo> detaylar = (List<DetayRepo>)Session["alimdetay"];

                //deratların özelliklerini toplama atalım
                decimal kdv = 0;
                decimal tutar = 0;
                decimal yekun = 0;
                //decimal adet = 0;

                if (detaylar.Count > 0)
                {
                    kdv = detaylar.Sum(x => x.kdv);
                    tutar = detaylar.Sum(x => x.tutar);
                    yekun = detaylar.Sum(x => x.yekun);
                    var makineye = detaylar.Where(x => x.masraf_id != null).Count();
                    if (makineye > 0)
                    {
                        btnAlimMakine.Visible = true;
                    }
                    else
                    {
                        btnAlimMakine.Visible = false;
                    }
                }
                toplam_kdv.Text = kdv.ToString();
                toplam_tutar.Text = tutar.ToString();
                toplam_yekun.Text = yekun.ToString();

                grdDetay.DataSource = detaylar;
                grdDetay.DataBind();
                //upBilgi.Update();


            }
        }
        private void makineGoster(radiusEntities dc)
        {
            MakineIslem c = new MakineIslem(dc);

            string arama_terimi = txtMakineAra.Value;
            grdMakine.DataSource = c.makineler(arama_terimi);
            grdMakine.DataBind();

        }
        public void MusteriAra(object sender, EventArgs e)
        {
            string s = txtAra.Value;
            string firma = KullaniciIslem.firma();

            if (!String.IsNullOrEmpty(s) && !String.IsNullOrWhiteSpace(s))
            {
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    MusteriArama(dc, s);
                    GridView1.DataBind();
                }

            }
        }


        public void CihazAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                CihazAraa(dc);
            }

        }

        public void MasrafAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                MasrafAraa(dc);
            }

        }
        protected void CihazAraa(radiusEntities dc)
        {

            CihazMalzeme c = new CihazMalzeme(dc);
            string terim = txtCihazAra.Value;
            if (string.IsNullOrEmpty(terim))
            {
                grdCihaz.DataSource = c.CihazListesiSinirli();

            }
            else
            {
                grdCihaz.DataSource = c.CihazListesiSinirli(terim);

            }
            if (!IsPostBack)
            {
                var gruplar = c.CihazGruplar();
                drdGrup.DataSource = gruplar;
                drdGrup.DataValueField = "grupid";
                drdGrup.DataTextField = "grupadi";
                drdGrup.DataBind();


                var birimler = c.birimler();

                drdAlinanBirim.DataSource = birimler;
                drdAlinanBirim.DataValueField = "id";
                drdAlinanBirim.DataTextField = "birim";
                drdAlinanBirim.DataBind();

                drdSatilanBirim.DataSource = birimler;
                drdSatilanBirim.DataValueField = "id";
                drdSatilanBirim.DataTextField = "birim";
                drdSatilanBirim.DataBind();


            }
            grdCihaz.DataBind();
        }
        protected void MasrafAraa(radiusEntities dc)
        {
            string firma = KullaniciIslem.firma();
            CihazMalzeme c = new CihazMalzeme(dc);
            string terim = txtMasrafAra.Value;
            grdMasraf.DataSource = c.MasrafListesi(terim);
            grdMasraf.DataBind();
        }
        private void MusteriArama(radiusEntities dc, string s)
        {
            MusteriIslemleri m = new MusteriIslemleri(dc);

            GridView1.DataSource = m.musteriAraR2(s, "tedarikci");

        }



        protected void btnAdd_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript2", sb.ToString(), false);

        }
        //Handles Add button click in add modal popup
        protected void btnAddRecord_Click(object sender, EventArgs e)
        {
            //musteri ekleme yöntemi sonra radius yöntemi olarak burada tekrarlanacak.
            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                //string id = lblMusID.Text;
                string ad = txtAdi.Text;
                string soyad = txtSoyAdi.Text;
                string adres = txtAdress.Text;
                string email = txtEmail.Text;
                string tel = txtTell.Text;
                string tc = Araclar.KimlikUret(11);
                string kullaniciAdi = Araclar.KimlikUret(10);
                string kim = txtKim.Text;
                string sifre = Araclar.KimlikUret(10);

                string unvan = txtDuzenUnvan.Text;
                if (string.IsNullOrEmpty(unvan))
                {
                    unvan = ad + " " + soyad;
                }

                m.musteriEkleR(ad, soyad, unvan, adres, tel, tel, email, kim, tc, "0", "0", false, true, false, false, null);
                MusteriArama(dc, ad);
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
                string firma = KullaniciIslem.firma();
                using (radiusEntities dc = MyContext.Context(firma))
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
            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                if (!String.IsNullOrEmpty(s) && !String.IsNullOrWhiteSpace(s))
                {

                    MusteriArama(dc, s);

                }
            }


            txtAciklama.Text = GridView1.SelectedValue.ToString();
            List<DetayRepo> detaylar = new List<DetayRepo>();
            if (Session["alimdetay"] != null)
            {
                int musteri_id = Convert.ToInt32(GridView1.SelectedValue);

                detaylar = (List<DetayRepo>)Session["alimdetay"];
                List<DetayRepo> musteriSorgusu = detaylar.Where(x => x.cust_id != musteri_id).ToList();
                if (musteriSorgusu.Count > 0)
                {
                    //hepsini değiştirelim
                    foreach (DetayRepo rep in detaylar)
                    {
                        rep.cust_id = musteri_id;
                    }
                    Session["alimdetay"] = detaylar;
                }
            }

        }



        protected void grdCihaz_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void grdMasraf_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grdCihaz_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                CihazAraa(dc);
                if (grdCihaz.SelectedValue != null)
                {
                    string satilan = grdCihaz.SelectedRow.Cells[5].Text.Trim();
                    string alinan = grdCihaz.SelectedRow.Cells[4].Text.Trim();
                    lblBirimSatilan.Text = satilan;
                    lblBirimAlinan.Text = alinan;
                    if (alinan.Equals(satilan))
                    {
                        div_malzeme.Visible = false;
                    }
                    else
                    {
                        div_malzeme.Visible = true;
                    }
                }
            }

        }

        protected void grdMasraf_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                MasrafAraa(dc);
                if (grdMasraf.SelectedValue != null)
                {

                    lblBirimMasraf.Text = grdMasraf.SelectedRow.Cells[4].Text;


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
                        List<DetayRepo> detaylar = (List<DetayRepo>)Session["alimdetay"];

                        int id = Convert.ToInt32(e.CommandArgument);

                        DetayRepo d = detaylar.FirstOrDefault(x => x.detay_id == id);
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

        protected void btnDetayEkle_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detayModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalScript", sb.ToString(), false);
        }

        protected void btnDetayEkleMasraf_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detayModalMasraf').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalMasrafScript", sb.ToString(), false);
        }

        protected void btnDetayKaydet_Click(object sender, EventArgs e)
        {
            if (GridView1.SelectedValue != null)
            {
                int? cihaz_id = null;
                int musteri_id = Convert.ToInt32(GridView1.SelectedValue);
                decimal kdvOran = decimal.Parse(txtKdv.Text) / 100;
                decimal tutar = decimal.Parse(txtTutar.Text);
                //buradaki oranı alarakkdv miktarını hesaplayalım
                //ve yekunu de buna göre hesaplayalım
                decimal kdv = tutar * kdvOran;
                decimal yekun = tutar + kdv;
                decimal adet_satilan = Decimal.Parse(txtAdetSatilan.Text);
                decimal adet_alinan = adet_satilan;
                string birim_satilan = lblBirimSatilan.Text.Trim();
                string birim_alinan = lblBirimAlinan.Text.Trim();
                if (!birim_alinan.Equals(birim_satilan))
                {
                    adet_alinan = Decimal.Parse(txtAdetAlinan.Text);
                }

                string ad = "";
                int detay_id = 0;
                List<DetayRepo> detaylar = new List<DetayRepo>();
                if (Session["alimdetay"] != null)
                {
                    detaylar = (List<DetayRepo>)Session["alimdetay"];
                    detay_id = detaylar.LastOrDefault().detay_id + 1;
                }
                string birim = "birim";
                if (grdCihaz.SelectedValue != null)
                {

                    cihaz_id = Convert.ToInt32(grdCihaz.SelectedValue);
                    ad = grdCihaz.SelectedRow.Cells[2].Text;
                    birim = grdCihaz.SelectedRow.Cells[4].Text;

                }

                DetayRepo detay = new DetayRepo();
                detay.detay_id = detay_id;
                detay.aciklama = txtDetayAciklama.Text;
                detay.adet_satilan = adet_satilan;
                detay.adet_alinan = adet_alinan;
                detay.birim_satilan = birim_satilan;
                detay.birim_alinan = birim_alinan;
                detay.alim_id = 0;
                detay.cihaz_adi = ad;
                detay.cihaz_id = cihaz_id;
                detay.cust_id = musteri_id;
                detay.kdv = kdv;
                detay.tutar = tutar;
                detay.yekun = yekun;

                detaylar.Add(detay);
                Session["alimdetay"] = detaylar;
                DetayGoster();
                upBilgi.Update();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kalem Eklendi!');");
                sb.Append("$('#detayModal').modal('hide');");
                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalScript", sb.ToString(), false);

            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.error('Lütfen önce kişi seçiniz!');");

                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalScript", sb.ToString(), false);
            }

        }

        protected void btnDetayKaydetMasraf_Click(object sender, EventArgs e)
        {
            if (GridView1.SelectedValue != null)
            {
                int? masraf_id = null;
                int musteri_id = Convert.ToInt32(GridView1.SelectedValue);
                decimal kdvOran = decimal.Parse(txtKdvMasraf.Text) / 100;
                decimal tutar = decimal.Parse(txtTutarMasraf.Text);
                //buradaki oranı alarakkdv miktarını hesaplayalım
                //ve yekunu de buna göre hesaplayalım
                decimal kdv = tutar * kdvOran;
                decimal yekun = tutar + kdv;
                decimal adet = Decimal.Parse(txtAdetMasraf.Text);
                string ad = "";
                string birim = lblBirimMasraf.Text;
                int detay_id = 0;
                List<DetayRepo> detaylar = new List<DetayRepo>();
                if (Session["alimdetay"] != null)
                {
                    detaylar = (List<DetayRepo>)Session["alimdetay"];
                    detay_id = detaylar.LastOrDefault().detay_id + 1;
                }

                if (grdMasraf.SelectedValue != null)
                {

                    masraf_id = Convert.ToInt32(grdMasraf.SelectedValue);
                    ad = grdMasraf.SelectedRow.Cells[2].Text;

                }
                DetayRepo detay = new DetayRepo();
                detay.detay_id = detay_id;
                detay.aciklama = txtDetayAciklamaMasraf.Text;
                detay.adet_satilan = adet;
                detay.birim_satilan = birim;
                detay.adet_alinan = adet;
                detay.birim_alinan = birim;
                detay.alim_id = 0;
                detay.cihaz_adi = ad;
                detay.masraf_id = masraf_id;
                detay.cust_id = musteri_id;
                detay.kdv = kdv;
                detay.tutar = tutar;
                detay.yekun = yekun;

                detaylar.Add(detay);
                Session["alimdetay"] = detaylar;
                DetayGoster();

                upBilgi.Update();
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
                sb.Append(" alertify.error('Lütfen önce kişi seçiniz!');");

                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalMasrafScript", sb.ToString(), false);
            }

        }
        protected void btnCihazKaydet_Click(object sender, EventArgs e)
        {
            string ad = cihaz_adi.Text;
            string acik = aciklama.Text;
            int sure = 12;
            int grupid = Int32.Parse(drdGrup.SelectedValue);
            string bar = barkod.Text;
            int birimAlinan = Int32.Parse(drdAlinanBirim.SelectedValue);
            int birimSatilan = Int32.Parse(drdSatilanBirim.SelectedValue);
            bool sinir = chcSinirsiz.Checked;

            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme m = new CihazMalzeme(dc);
                m.Yeni(ad, acik, sure, grupid, bar, birimAlinan, birimSatilan, sinir);
                CihazAraa(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Cihaz tanımlandı!');");
            sb.Append("$('#cihazModal').modal('hide');");

            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);


        }


        protected void btnMasrafKaydet_Click(object sender, EventArgs e)
        {
            string ad = cihaz_adi.Text;
            string acik = aciklama.Text;
            int sure = 12;
            int grupid = Int32.Parse(drdGrup.SelectedValue);
            string bar = barkod.Text;
            int birimAlinan = Int32.Parse(drdAlinanBirim.SelectedValue);
            int birimSatilan = Int32.Parse(drdSatilanBirim.SelectedValue);

            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme m = new CihazMalzeme(dc);
                m.Yeni(ad, acik, sure, grupid, bar, birimAlinan, birimSatilan, false);
                MasrafAraa(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Cihaz tanımlandı!');");
            sb.Append("$('#cihazModal').modal('hide');");

            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);


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
            if (GridView1.SelectedValue != null)
            {

                int custid = Int32.Parse(GridView1.SelectedValue.ToString());

                DateTime islem_tarih = DateTime.Now;
                string tars = tarih2.Value;

                if (!String.IsNullOrEmpty(tars))
                {
                    islem_tarih = DateTime.Parse(tars);
                }
                string aciklama = txtAciklama.Text;
                string konu = txtKonu.Text;

                if (Session["alimdetay"] != null)
                {
                    List<DetayRepo> detaylar = (List<DetayRepo>)Session["alimdetay"];

                    if (detaylar.Count > 0)
                    {

                        if (String.IsNullOrEmpty(aciklama))
                        {
                            string kalemler = "";
                            foreach (var d in detaylar)
                            {
                                if (d.cihaz_id != null)
                                {
                                    kalemler += " " + d.adet_satilan.ToString() + " " + d.birim_satilan.ToString() + " " + d.cihaz_adi + "-";
                                }
                                else
                                {
                                    kalemler += " " + d.adet_satilan.ToString() + " " + d.birim_satilan.ToString() + " " + d.cihaz_adi + "-";
                                }

                            }
                            if (String.IsNullOrEmpty(aciklama))
                            {
                                aciklama = kalemler;
                            }

                        }

                        AlimRepo hesap = new AlimRepo();


                        hesap.aciklama = aciklama;
                        hesap.konu = konu;
                        hesap.alim_tarih = islem_tarih;
                        hesap.belge_no = txtBelgeNo.Text;
                        hesap.CustID = custid;


                        string kdvS = toplam_kdv2.Text;
                        string tutarS = toplam_tutar2.Text;
                        string yekunS = toplam_yekun2.Text;

                        if (!String.IsNullOrEmpty(kdvS))
                        {
                            hesap.toplam_kdv = Decimal.Parse(kdvS);
                        }
                        else
                        {
                            hesap.toplam_kdv = Decimal.Parse(toplam_kdv.Text);
                        }
                        if (!String.IsNullOrEmpty(tutarS))
                        {
                            hesap.toplam_tutar = Decimal.Parse(tutarS);
                        }
                        else
                        {
                            hesap.toplam_tutar = Decimal.Parse(toplam_tutar.Text);
                        }

                        if (!String.IsNullOrEmpty(yekunS))
                        {
                            hesap.toplam_yekun = decimal.Parse(yekunS);
                        }
                        else
                        {
                            hesap.toplam_yekun = decimal.Parse(toplam_yekun.Text);
                        }
                        string firma = KullaniciIslem.firma();
                        using (radiusEntities dc = MyContext.Context(firma))
                        {
                            SatinAlim a = new SatinAlim(dc);
                            a.detay = detaylar;
                            a.hesap = hesap;
                            string s = a.alim_kaydet(User.Identity.Name);
                        }

                    }
                }

                Session["alimdetay"] = null;

                Response.Redirect("/TeknikAlim/Alimlar");

            }
        }

        protected void btnAlimMakine_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#atamaModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AtaShowModalScript", sb.ToString(), false);
        }

        protected void btnMakineAra_Click(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                makineGoster(dc);
            }
        }

        protected void grdMakine_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (GridView1.SelectedValue != null && grdMakine.SelectedValue != null)
            {

                int custid = Int32.Parse(GridView1.SelectedValue.ToString());
                int makineid = Int32.Parse(grdMakine.SelectedValue.ToString());
                DateTime islem_tarih = DateTime.Now;
                string tars = tarih2.Value;

                if (!String.IsNullOrEmpty(tars))
                {
                    islem_tarih = DateTime.Parse(tars);
                }
                string aciklama = txtAciklama.Text;
                string konu = txtKonu.Text;

                if (Session["alimdetay"] != null)
                {
                    List<DetayRepo> detaylar = (List<DetayRepo>)Session["alimdetay"];

                    if (detaylar.Count > 0)
                    {

                        if (String.IsNullOrEmpty(aciklama))
                        {
                            string kalemler = "";
                            foreach (var d in detaylar)
                            {
                                if (d.cihaz_id != null)
                                {
                                    kalemler += " " + d.adet_satilan.ToString() + " " + d.birim_satilan.ToString() + " " + d.cihaz_adi + "-";
                                }
                                else
                                {
                                    kalemler += " " + d.adet_satilan.ToString() + " " + d.birim_satilan.ToString() + " " + d.cihaz_adi + "-";
                                }

                            }
                            if (String.IsNullOrEmpty(aciklama))
                            {
                                aciklama = kalemler;
                            }

                        }

                        AlimRepo hesap = new AlimRepo();


                        hesap.aciklama = aciklama;
                        hesap.konu = konu;
                        hesap.alim_tarih = islem_tarih;
                        hesap.belge_no = txtBelgeNo.Text;
                        hesap.CustID = custid;


                        string kdvS = toplam_kdv2.Text;
                        string tutarS = toplam_tutar2.Text;
                        string yekunS = toplam_yekun2.Text;

                        if (!String.IsNullOrEmpty(kdvS))
                        {
                            hesap.toplam_kdv = Decimal.Parse(kdvS);
                        }
                        else
                        {
                            hesap.toplam_kdv = Decimal.Parse(toplam_kdv.Text);
                        }
                        if (!String.IsNullOrEmpty(tutarS))
                        {
                            hesap.toplam_tutar = Decimal.Parse(tutarS);
                        }
                        else
                        {
                            hesap.toplam_tutar = Decimal.Parse(toplam_tutar.Text);
                        }

                        if (!String.IsNullOrEmpty(yekunS))
                        {
                            hesap.toplam_yekun = decimal.Parse(yekunS);
                        }
                        else
                        {
                            hesap.toplam_yekun = decimal.Parse(toplam_yekun.Text);
                        }
                        string firma = KullaniciIslem.firma();
                        using (radiusEntities dc = MyContext.Context(firma))
                        {
                            SatinAlim a = new SatinAlim(dc);
                            a.detay = detaylar;
                            a.hesap = hesap;
                            string s = a.alim_kaydet_makineye(User.Identity.Name, makineid);
                        }

                    }
                }

                Session["alimdetay"] = null;

                Response.Redirect("/TeknikAlim/Alimlar");

            }

            //Operator op = new Operator(dc, kullanici);
            //op.ServisOperatorAta(Int32.Parse(servisids));
            //ortak(dc);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#atamaModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AtaShowModalScript", sb.ToString(), false);


        }
    }
}