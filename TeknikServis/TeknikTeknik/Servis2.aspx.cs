using ServisDAL;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Web.UI;
using TeknikServis.Radius;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Web;
using System.IO;
using System.Text;
using ServisDAL.Repo;

namespace TeknikServis.TeknikTeknik
{
    public partial class Servis2 : System.Web.UI.Page
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

                    ortak(dc);
                    if (ay.calismaTipli() == true)
                    {

                        btnEkleH.Visible = false;
                      
                    }
                    else
                    {
                        btnEkleH.Visible = true;
                       
                        cihazGoster(dc);
                        birimler(dc);
                        grdOperator.DataSource = KullaniciIslem.FirmaOperatorleri();
                        grdOperator.DataBind();
                    }


                    makineGoster(dc);


                }



            }
            if (!User.IsInRole("Admin") && !User.IsInRole("mudur"))
            {
                btnSonlandir.Visible = false;
                btnSonlandirK.Visible = false;
                txtHesapMaliyet.Visible = false;
            }

        }

        private void ortak(radiusEntities dc)
        {
            string servis_id = Request.QueryString["servisid"];
            string cust_id = Request.QueryString["custid"];
            string kimlik = Request.QueryString["kimlik"];

            if (!String.IsNullOrEmpty(cust_id))
            {
                if (!String.IsNullOrEmpty(servis_id) || !String.IsNullOrEmpty(kimlik))
                {
                    int servisid = -1;
                    if (!String.IsNullOrEmpty(servis_id))
                    {
                        servisid = Int32.Parse(servis_id);
                    }

                    TekServis tek = new TekServis(dc, servisid, kimlik);
                    ServisInfo s = tek.servis();
                    ServisRepo genel = s.genel;
                    hdnAciklama.Value = genel.aciklama;
                    btnTopluOnay.Visible = true;
                    btnMusteriDetayim.Visible = true;
                    btnMusteriDetayimK.Visible = true;

                    var liste = s.kararlar;
                    int adet = 0;
                    decimal mal = 0;
                    decimal tutar = 0;

                    var giris = s.girisler;
                    int adetg = 0;
                    decimal miktarG = 0;
                    decimal tutarG = 0;

                    var teorik = s.teorikler;
                    int adett = 0;
                    decimal miktart = 0;
                    decimal tutart = 0;

                    if (s.girisler.Count > 0)
                    {
                        adetg = s.girisler.Count;
                        miktarG = s.girisler.Sum(x => x.miktar);
                        tutarG = s.girisler.Sum(x => x.tutar);
                    }

                    txtAdetG.InnerHtml = "Adet: " + adetg.ToString();
                    txtMiktarG.InnerHtml = "Miktar: " + miktarG.ToString();
                    txtTutarG.InnerHtml = "Maliyet: " + tutarG.ToString("C");
                    if (liste.Count > 0)
                    {
                        adet = liste.Count;
                        mal = (decimal)liste.Sum(x => x.toplam_maliyet);
                        tutar = (decimal)liste.Sum(x => x.yekun);
                    }


                    if (teorik.Count > 0)
                    {
                        adett = teorik.Count;
                        miktart = teorik.Sum(x => x.miktar);
                        tutart = teorik.Sum(x => x.tutar);

                    }

                    txtAdetT.InnerHtml = "Adet: " + adett.ToString();
                    txtMiktarT.InnerHtml = "Miktar: " + miktart.ToString();
                    txtTutarT.InnerHtml = "Maliyet: " + tutart.ToString("C");


                    txtHesapAdet.InnerHtml = " Adet: " + adet.ToString();
                    txtHesapMaliyet.InnerHtml = "Maliyet:" + mal.ToString("C");
                    txtHesapTutar.InnerHtml = "Tutar: " + tutar.ToString("C");
                    txtServisTutar.Value = tutar.ToString("C");
                    GridView1.DataSource = liste;

                    grdAlimlarTeorik.DataSource = teorik;
                    grdAlimlarGirisler.DataSource = giris;
                    grdAtamalar.DataSource = s.atamalar;

                    if (s.genel.kapanmaZamani != null)
                    {
                        btnTopluOnay.Visible = false;
                        btnTamirciye.Visible = false;
                        btnEkleH.Visible = false;

                    }


                    //listview kısmı

                    ServisIslemleri islem = new ServisIslemleri(dc);


                    txtKimlikNo.Value = kimlik;
                    txtMusteri.InnerHtml = genel.musteriAdi + " - " + genel.telefon;
                    txtKonu.InnerHtml = genel.baslik;

                    //int sayi = ser.aciklama.Split(new[] { '\r', '\n' }).Length;
                    //txtServisAciklama.Rows = sayi + 1;
                    txtServisAciklama.InnerHtml = genel.aciklama;
                    txtServisAdresi.InnerHtml = genel.adres;

                    txtTarih.Value = genel.acilmaZamani.ToString();
                    txtServisID.Value = genel.serviceID.ToString();


                    txtKullanici.Value = genel.kullanici;
                    if (genel.kapanmaZamani != null)
                    {
                        btnSonlandir.Visible = false;
                        btnEkle.Visible = false;
                        btnEkleHMakine.Visible = false;
                        btnSonlandirK.Visible = false;
                        btnEkleK.Visible = false;
                        hdnKapanma.Value = genel.kapanmaZamani.ToString();
                    }

                    hdnCustID.Value = genel.custID.ToString();
                    hdnAtananID.Value = genel.sonGorevliID.ToString();
                    hdnServisID.Value = genel.serviceID.ToString();
                    //bakalım görevli değişmiş mi?
                    string yeniGorevli = Request.QueryString["eleman"];
                    if (!String.IsNullOrEmpty(yeniGorevli))
                    {
                        if (yeniGorevli != hdnAtananID.Value)
                        {

                            islem.Atama(yeniGorevli, kimlik);
                        }
                    }
                    if (s.detaylar.Count > 0)
                    {
                        ListView1.DataSource = s.detaylar;
                        ListView1.DataBind();
                    }


                }
            }
            cihazGoster(dc);

            GridView1.DataBind();
            grdAlimlarTeorik.DataBind();
            grdAlimlarGirisler.DataBind();
            grdAtamalar.DataBind();

        }

        private void BindList(radiusEntities dc)
        {
            string kimlikNo = Request.QueryString["kimlik"];
            string servis_id = Request.QueryString["servisid"];
            if (!String.IsNullOrEmpty(kimlikNo) && !String.IsNullOrWhiteSpace(kimlikNo))
            {
                ServisIslemleri islem = new ServisIslemleri(dc);


                ServisDAL.Repo.ServisRepo ser = islem.servisAraKimlikDetayTekR(kimlikNo);
                if (ser != null)
                {
                    txtKimlikNo.Value = kimlikNo;
                    txtMusteri.InnerHtml = ser.musteriAdi + " - " + ser.telefon;
                    txtKonu.InnerHtml = ser.baslik;

                    //int sayi = ser.aciklama.Split(new[] { '\r', '\n' }).Length;
                    //txtServisAciklama.Rows = sayi + 1;
                    txtServisAciklama.InnerHtml = ser.aciklama;
                    txtServisAdresi.InnerHtml = ser.adres;

                    txtTarih.Value = ser.acilmaZamani.ToString();
                    txtServisID.Value = ser.serviceID.ToString();
                    if (ser.kapanmaZamani != null)
                    {
                        btnSonlandir.Visible = false;

                        btnEkle.Visible = false;
                        btnSonlandirK.Visible = false;

                        btnEkleK.Visible = false;
                    }

                    hdnCustID.Value = ser.custID.ToString();
                    hdnAtananID.Value = ser.sonGorevliID.ToString();
                    //bakalım görevli değişmiş mi?
                    string yeniGorevli = Request.QueryString["eleman"];
                    if (!String.IsNullOrEmpty(yeniGorevli))
                    {
                        if (yeniGorevli != hdnAtananID.Value)
                        {

                            islem.Atama(yeniGorevli, kimlikNo);
                        }
                    }


                    ListView1.DataSource = islem.detayListesiDetayKimlikR(kimlikNo);
                    ListView1.DataBind();

                }
            }


        }


        protected void btnKaydetMakine_Click(object sender, EventArgs e)
        {
            //string servisidd = Request.QueryString["servisid"];
            string servisidd = hdnServisID.Value;
            string custidd = Request.QueryString["custid"];
            string hesapidd = hdnHesapIDDuzen.Value;
            string makinehesapidd = hdnHesapIDDuzenMakine.Value;
            string kimlik = Request.QueryString["kimlik"];
            int? custid = null;
            string sure_aciklama = txtSaatBilgi.Text;

            if (!String.IsNullOrEmpty(custidd))
            {
                custid = Int32.Parse(custidd);

            }

            //HESAP ID HDNYE GÖRE DÜZENLEME YAPILIYOR
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {

                if (!String.IsNullOrEmpty(servisidd) && String.IsNullOrEmpty(hesapidd) && String.IsNullOrEmpty(makinehesapidd))
                {
                    //yeni ekleme
                    ServisIslemleri s = new ServisIslemleri(dc);

                    int servisid = Int32.Parse(servisidd);

                    string islem = txtIslemParcaMakine.Value;

                    decimal kdv = Decimal.Parse(txtKDVOraniDuzenleMakine.Text);
                    decimal yekun = Decimal.Parse(txtYekunMakine.Text);
                    string aciklama = txtAciklamaMakine.Text;

                    string makine = txtMakineAdiGoster.Value;

                    int makine_id = Convert.ToInt32(grdMakine.SelectedValue);

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

                        string tarife_tipi = hdnTarifeTipi.Value;
                        decimal sayac_farki = son - baslangic;

                        decimal fiyat = (decimal)(yekun / sure_saat);

                        //sayac_farkını ekledim
                        //sayac farkını burada ekleyip triggerda servis makinelerinin toplamlarını güncellemekte kullanacağız.
                        //servis makinelerinde toplam dakikayı da ekleyebiliriz
                        s.serviceKararEkleCalismaTipli(servisid, custid, islem, kdv, yekun, aciklama, makine_id, makine, karar_tarihi, User.Identity.Name, tarife_kodu, tarife_tipi, tarifeid, baslangic, son, sure_saat, baslama_tarih, bitis_tarih, son, dakika, sayac_farki, fiyat, sure_aciklama);

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append(" alertify.success('Hesap eklendi!');");
                        sb.Append("$('#yeniMakineModal').modal('hide');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniHideModalScript", sb.ToString(), false);

                    }



                }
                else
                {
                    ServisGuncelle(servisidd, custidd, hesapidd, makinehesapidd, kimlik, dc, "makine");
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");

                    sb.Append(" alertify.success('Hesap güncellendi!');");
                    sb.Append("$('#yeniMakineModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniHideModalScript", sb.ToString(), false);
                }

                //goster(dc);
                ortak(dc);
            }
        }
        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            string servisidd = hdnServisID.Value;
            string custidd = Request.QueryString["custid"];
            string hesapidd = hdnHesapIDDuzen.Value;
            string kimlik = Request.QueryString["kimlik"];
            int? custid = null;
            if (!String.IsNullOrEmpty(custidd))
            {
                custid = Int32.Parse(custidd);

            }
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {

                if (!String.IsNullOrEmpty(servisidd) && String.IsNullOrEmpty(hesapidd))
                {

                    decimal adet = 1;
                    string adet_s = txtAdet.Text;
                    if (!String.IsNullOrEmpty(adet_s))
                    {
                        adet = Decimal.Parse(adet_s);
                    }

                    //yeni ekleme
                    ServisIslemleri s = new ServisIslemleri(dc);

                    int servisid = Int32.Parse(servisidd);

                    string islem = txtIslemParca.Value;

                    decimal kdv = Decimal.Parse(txtKDVOraniDuzenle.Text);
                    decimal yekun = Decimal.Parse(txtYekun.Text);
                    string aciklama = txtAciklama.Text;
                    decimal fiyat = (decimal)(yekun / adet);
                    int cihaz_id = -1;
                    string cihaz = txtCihazAdiGoster.Value;
                    bool sinir = false;
                    if (grdCihaz.SelectedIndex > -1)
                    {
                        cihaz_id = Convert.ToInt32(grdCihaz.SelectedValue);
                        string sin = grdCihaz.SelectedRow.Cells[6].Text.Trim();
                        if (sin.Equals("sinirsiz"))
                        {
                            sinir = true;
                        }
                    }

                    DateTime karar_tarihi = DateTime.Now;
                    string tarS = tarih2.Value;
                    if (!String.IsNullOrEmpty(tarS))
                    {
                        karar_tarihi = DateTime.Parse(tarS);
                    }

                    string birim = drdBirim.SelectedItem.Text;

                    if (cihaz_id > -1)
                    {
                        string sure = hdnGarantiSure.Value;

                        s.serviceKararEkleRYeniCihazli(servisid, custid, islem, kdv, yekun, aciklama, (int)cihaz_id, adet, cihaz, "12", karar_tarihi, User.Identity.Name, birim, fiyat, sinir);


                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append(" alertify.success('Hesap Eklendi!');");
                        sb.Append("$('#yeniModal').modal('hide');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniHideModalScript", sb.ToString(), false);


                    }
                    else
                    {

                        try
                        {
                            s.serviceKararEkleRYeni(servisid, custid, islem, kdv, yekun, aciklama, adet, karar_tarihi, User.Identity.Name);
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");

                            sb.Append(" alertify.success('Hesap Kaydedildi!');");
                            sb.Append("$('#yeniModal').modal('hide');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniHideModalScript", sb.ToString(), false);

                        }
                        catch (DbEntityValidationException ex)
                        {
                            Dictionary<string, string> mesajlar = new Dictionary<string, string>();
                            foreach (var errs in ex.EntityValidationErrors)
                            {
                                foreach (var err in errs.ValidationErrors)
                                {
                                    string propName = err.PropertyName;
                                    string errMess = err.ErrorMessage;
                                    mesajlar.Add(propName, errMess);
                                }
                            }
                            HttpContext.Current.Session["mesaj"] = mesajlar;
                            HttpContext.Current.Response.Redirect("/Deneme.aspx");
                        }

                    }



                }
                else
                {
                    ServisGuncelle(servisidd, custidd, hesapidd, "", kimlik, dc, "cihaz");
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");

                    sb.Append(" alertify.success('Hesap güncellendi!');");
                    sb.Append("$('#yeniModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniHideModalScript", sb.ToString(), false);
                }

                //goster(dc);
                ortak(dc);
            }
        }

        private void ServisGuncelle(string servisidd, string custidd, string hesapidd, string makinehesapidd, string kimlik, radiusEntities dc, string modal)
        {
            if (modal.Equals("cihaz"))
            {

                int? custid = null;
                if (String.IsNullOrEmpty(custidd))
                {
                    custid = Int32.Parse(custidd);

                }
                if (!String.IsNullOrEmpty(hesapidd))
                {
                    //update işlemi

                    ServisIslemleri s = new ServisIslemleri(dc);
                    int adet = 1;
                    int cihaz_id = -1;
                    string adet_s = txtAdet.Text;

                    if (!String.IsNullOrEmpty(adet_s))
                    {
                        adet = Int32.Parse(adet_s);
                    }
                    string cihaz = txtCihazAdiGoster.Value;
                    if (grdCihaz.SelectedValue != null)
                    {
                        cihaz_id = Convert.ToInt32(grdCihaz.SelectedValue);
                    }
                    int hesapid = Int32.Parse(hesapidd);
                    string islem = txtIslemParca.Value;
                    decimal kdvOran = Decimal.Parse(txtKDVOraniDuzenle.Text);

                    decimal yekun = Decimal.Parse(txtYekun.Text);
                    string aciklama = txtAciklama.Text;



                    //urun bilgisi yok normal güncelleme yapıyoruz
                    if (cihaz_id > -1)
                    {
                        //cihazlı
                        s.serviceKararGuncelleCihazli(hesapid, islem, aciklama, kdvOran, yekun, cihaz, cihaz_id, adet, User.Identity.Name);
                    }
                    else
                    {
                        s.serviceKararGuncelleR(hesapid, islem, aciklama, kdvOran, yekun, User.Identity.Name);
                    }
                    ortak(dc);
                }

            }
            else
            {

                if (!String.IsNullOrEmpty(makinehesapidd))
                {
                    //update işlemi

                    ServisIslemleri s = new ServisIslemleri(dc);

                    int hesapid = Int32.Parse(makinehesapidd);

                    string islem = txtIslemParcaMakine.Value;

                    decimal kdv = Decimal.Parse(txtKDVOraniDuzenleMakine.Text);
                    decimal yekun = Decimal.Parse(txtYekunMakine.Text);
                    string aciklama = txtAciklamaMakine.Text;

                    string makine = txtMakineAdiGoster.Value;

                    int makine_id = Convert.ToInt32(grdMakine.SelectedValue);

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

                        string tarife_tipi = hdnTarifeTipi.Value;
                        decimal sayac_farki = son - baslangic;
                        s.serviceKararGuncelleMakine(hesapid, islem, kdv, yekun, aciklama, makine_id, karar_tarihi, User.Identity.Name, tarife_kodu, baslangic, son, sure_saat, baslama_tarih, bitis_tarih, dakika, sayac_farki, tarife_tipi, tarifeid, makine);



                    }

                    ortak(dc);
                }
            }
        }



        protected void grdCihaz_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                cihazGoster(dc);
                CihazMalzeme c = new CihazMalzeme(dc);
                if (grdCihaz.SelectedValue != null)
                {
                    int id = Convert.ToInt32(grdCihaz.SelectedValue);
                    var repo = c.tekCihaz(id);
                    txtFiyatMalzeme.Text = repo.satis.ToString();
                    drdBirim.SelectedValue = repo.satilan_birim_id.ToString();
                    tutarHesap();
                }
            }

            string cihaz = (grdCihaz.SelectedRow.FindControl("btnRandom") as LinkButton).Text;
            string sure = grdCihaz.SelectedRow.Cells[4].Text;
            txtCihazAdiGoster.Value = cihaz;
            hdnGarantiSure.Value = sure.ToString();

        }

        protected void grdMakine_SelectedIndexChanged(object sender, EventArgs e)
        {

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                tarifeCek(dc);
            }

        }

        protected void btnAddCihaz_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#yeniModal').modal('hide');");
            sb.Append("$('#addModalCihaz').modal('show');");

            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
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
        protected void btnAddCihazRecord_Click(object sender, EventArgs e)
        {
            string gs = txtGarantiSuresi.Text;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);
                s.CihazEkle(txtCihazAdi.Text, txtCihazAciklama.Text, gs);
                cihazGoster(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            sb.Append(" alertify.success('Kayıt Eklendi!');");

            sb.Append("$('#addModalCihaz').modal('hide');");
            //sb.Append("$('#yeniModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        protected void btnAddMakineRecord_Click(object sender, EventArgs e)
        {
            //string gs = txtGarantiSuresi.Text;
            //using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            //{
            //    ServisIslemleri s = new ServisIslemleri(dc);
            //    s.CihazEkle(txtCihazAdi.Text, txtCihazAciklama.Text, gs);
            //    cihazGoster(dc);
            //}

            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append(@"<script type='text/javascript'>");

            //sb.Append(" alertify.success('Kayıt Eklendi!');");

            //sb.Append("$('#addModalCihaz').modal('hide');");
            ////sb.Append("$('#yeniModal').modal('show');");
            //sb.Append(@"</script>");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        protected void CihazAra(object sender, EventArgs e)
        {

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);

                cihazGoster(dc);
            }
        }

        protected void MakineAra(object sender, EventArgs e)
        {

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);

                makineGoster(dc);
            }
        }
        private void cihazGoster(radiusEntities dc)
        {
            CihazMalzeme c = new CihazMalzeme(dc);

            string arama_terimi = txtAra.Value;
            grdCihaz.DataSource = c.CihazListesi2(arama_terimi);
            grdCihaz.DataBind();

        }
        private void birimler(radiusEntities dc)
        {
            CihazMalzeme c = new CihazMalzeme(dc);
            drdBirim.DataSource = c.birimler();
            drdBirim.DataValueField = "id";
            drdBirim.DataTextField = "birim";
            drdBirim.DataBind();


        }

        private void makineGoster(radiusEntities dc)
        {
            MakineIslem c = new MakineIslem(dc);

            string arama_terimi = txtMakineAra.Value;
            grdMakine.DataSource = c.makineler(arama_terimi);
            grdMakine.DataBind();

        }

        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);

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
                        s.servisKararIptalR(hesapID, User.Identity.Name);
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

            else if (e.CommandName.Equals("onay"))
            {
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int hesapID = Convert.ToInt32(arg[0]);
                int musteriID = Convert.ToInt32(arg[2]);

                int index = Convert.ToInt32(arg[1]);
                GridViewRow row = GridView1.Rows[index];
                string islem = row.Cells[2].Text;
                string yekun = row.Cells[5].Text;
                string servisid = row.Cells[10].Text;


                hdnHesapIDH.Value = hesapID.ToString();
                hdnMusteriIDH.Value = musteriID.ToString();

                hdnServisIDDH.Value = servisid;
                hdnYekunnH.Value = yekun;
                hdnIslemmH.Value = islem;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#onayModalH').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
            }

            else if (e.CommandName.Equals("duzenle"))
            {
                //burada tamircili yada normal hesap olduğu kontrol edilecek
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int hesapid = Convert.ToInt32(arg[0]);

                grdCihaz.SelectedIndex = -1;

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    ServisIslemleri s = new ServisIslemleri(dc);
                    TeknikServis.Radius.servicehesap hesap = s.tekHesapR(hesapid);
                    if (hesap.tamirci_id == null)
                    {
                        if (hesap.makine_id == null)
                        {
                            hdnHesapIDDuzen.Value = hesapid.ToString();
                            txtAciklama.Text = hesap.Aciklama;
                            txtIslemParca.Value = hesap.IslemParca;
                            //burda normal KDV miktarı varama text alanında sadece oranını gösteriyoruz

                            txtKDVDuzenle.Text = hesap.KDV.ToString();

                            decimal orand = (decimal)((hesap.KDV * 100) / (hesap.Yekun - hesap.KDV));
                            string oran = Math.Round(orand, 2).ToString();
                            txtKDVOraniDuzenle.Text = oran;
                            //txtTutar.Text = hesap.Tutar.ToString();

                            txtYekun.Text = hesap.Yekun.ToString();
                            txtAdet.Text = hesap.adet.ToString();
                            txtCihazAdiGoster.Value = hesap.cihaz_adi;
                            hdnGarantiSure.Value = hesap.cihaz_gsure.ToString();

                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append("$('#yeniModal').modal('show');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniShowModalScript", sb.ToString(), false);
                        }
                        else
                        {
                            //makine hesabı açılacak
                            string tarife = hesap.tarife_tipi;
                            numara_aralik.Visible = false;
                            tarih_aralik.Visible = false;
                            if (tarife.Equals("saat"))
                            {

                                hdnSaatlik.Value = "yes";

                                numara_aralik.Visible = true;
                                tarih_aralik.Visible = false;
                            }
                            else
                            {
                                numara_aralik.Visible = false;
                                tarih_aralik.Visible = true;
                                hdnSaatlik.Value = "";
                            }
                            int id = (int)hesap.makine_id;
                            MakineIslem m = new MakineIslem(dc);
                            var tarifeler = m.tarifeler(id);
                            //if (tarifeler.Count > 0)
                            //{
                            drdTarife.DataSource = tarifeler;
                            drdTarife.DataValueField = "id";
                            drdTarife.DataTextField = "ad";
                            drdTarife.DataBind();

                            drdTarife.SelectedValue = hesap.tarifeid.ToString();

                            hdnHesapIDDuzenMakine.Value = hesapid.ToString();
                            txtAciklamaMakine.Text = hesap.Aciklama;
                            txtIslemParcaMakine.Value = hesap.IslemParca;
                            //burda normal KDV miktarı varama text alanında sadece oranını gösteriyoruz
                            txtKDVDuzenleMakine.Text = hesap.KDV.ToString();

                            decimal orand = (decimal)((hesap.KDV * 100) / (hesap.Yekun - hesap.KDV));
                            string oran = Math.Round(orand, 2).ToString();
                            txtKDVOraniDuzenleMakine.Text = oran;

                            txtYekunMakine.Text = hesap.Yekun.ToString();

                            Tarife t = m.tarife_tek((int)hesap.tarifeid);
                            txtFiyat.Text = t.tutar.ToString();

                            datetimepicker6.Text = hesap.baslangic_tarih.ToShortDateString();
                            datetimepicker7.Text = hesap.bitis_tarih.ToShortDateString();
                            txtSonNumara.Text = hesap.baslangic.ToString();
                            txtYeniNumara.Text = hesap.bitis.ToString();
                            txtSure.Text = hesap.calisma_saati.ToString();
                            txtDakika.Text = hesap.dakika.ToString();

                            txtMakineAdiGoster.Value = hesap.makine_caris.adi;

                            TimeSpan ts = TimeSpan.FromMinutes(hesap.dakika);
                            txtSaatBilgi.Text = ts.Hours + " saat " + ts.Minutes + " dakika";



                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append("$('#yeniMakineModal').modal('show');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniShowModalScript", sb.ToString(), false);
                        }

                    }
                    else
                    {

                        hdnHesapIDDuzenTamirci.Value = hesapid.ToString();
                        txtAciklamaTamirci.Text = hesap.Aciklama;
                        txtIslemParcaTamirci.Value = hesap.IslemParca;
                        hdnTamirciID.Value = hesap.tamirci_id.ToString();
                        decimal orand = (decimal)((hesap.KDV * 100) / (hesap.Yekun - hesap.KDV));
                        string oran = Math.Round(orand, 2).ToString();
                        txtKDVOraniDuzenleTamirci.Text = oran;
                        txtKDVDuzenleTamirci.Text = hesap.KDV.ToString();
                        txtYekunTamirci.Text = hesap.Yekun.ToString();
                        txtTamirciMaliyet.Text = hesap.toplam_maliyet.ToString();


                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#tamirciModal').modal('show');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tamirciShowModalScript", sb.ToString(), false);

                    }

                }

            }

        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string m = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "makine_id"));
                string servisid = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "servisID"));

                string kimlik = Request.QueryString["kimlik"];
                string custid = Request.QueryString["custid"];
                LinkButton lnk = (e.Row.FindControl("btnMasrafGir") as LinkButton);

                if (!String.IsNullOrEmpty(m))
                {
                    (e.Row.FindControl("btnMasrafGir") as LinkButton).PostBackUrl = "~/TeknikMakine/MasrafGir?makineid=" + m + "&back=servis&" + "kimlik=" + kimlik + "&custid=" + custid + "&servisid=" + servisid;
                    (e.Row.FindControl("btnMasrafGirk") as LinkButton).PostBackUrl = "~/TeknikMakine/MasrafGir?makineid=" + m + "&back=servis&" + "kimlik=" + kimlik + "&custid=" + custid + "&servisid=" + servisid;

                }
                else
                {
                    lnk.Visible = false;
                }

                if (!User.IsInRole("Admin"))
                {
                    (e.Row.FindControl("delLink") as LinkButton).Visible = false;
                    (e.Row.FindControl("delLinkk") as LinkButton).Visible = false;

                }

            }
        }
        protected void btnEkle_ClickH(object sender, EventArgs e)
        {
            string servisidd = hdnServisID.Value;
            string custidd = Request.QueryString["custid"];
            hdnHesapIDDuzen.Value = "";
            grdCihaz.SelectedIndex = -1;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                birimler(dc);
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#yeniModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniModalScript", sb.ToString(), false);

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
            hdnHesapIDDuzenMakine.Value = string.Empty;
            txtYekunMakine.Text = "";

            datetimepicker6.Text = "";
            datetimepicker7.Text = "";
            txtSonNumara.Text = "";
            txtYeniNumara.Text = "";
            txtSure.Text = "";
            txtDakika.Text = "";
            //hdnSaatlik.Value = "";
            txtMakineAdiGoster.Value = "";


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#yeniMakineModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniMakineModalScript", sb.ToString(), false);


        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Servis Kararları-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Servis Kararları-" + DateTime.Now.ToString();
            ExportHelper.ToWord(GridView1, isim);
        }

        protected void btnPrnt_Click(object sender, EventArgs e)
        {
            //Session["ctrl"] = GridView1;
            //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>");

            GridView1.AllowPaging = false;
            GridView1.RowStyle.ForeColor = System.Drawing.Color.Black;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
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

            ScriptManager.RegisterStartupScript(GridView1, this.GetType(), "GridPrint", sb.ToString(), false);
            GridView1.AllowPaging = true;



        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string onay = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "onayDurumu"));
                LinkButton link = e.Row.Cells[0].Controls[3] as LinkButton;
                LinkButton link2 = e.Row.Cells[0].Controls[5] as LinkButton;
                LinkButton link3 = e.Row.Cells[0].Controls[7] as LinkButton;


                LinkButton linkk = e.Row.Cells[1].Controls[3] as LinkButton;
                LinkButton linkk2 = e.Row.Cells[1].Controls[5] as LinkButton;
                LinkButton linkk3 = e.Row.Cells[1].Controls[7] as LinkButton;

                if (onay == "EVET")
                {
                    link.Visible = false;
                    link2.Visible = false;

                    linkk.Visible = false;
                    linkk2.Visible = false;
                    link3.Visible = false;
                }

            }
            if (!string.IsNullOrEmpty(hdnKapanma.Value))
            {
                e.Row.Cells[0].Visible = false;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ortak(dc);
            }

        }

        protected void btnServisDetaylari_Click(object sender, EventArgs e)
        {

            //string servis_id = Request.QueryString["servisid"];
            //string cust_id = Request.QueryString["custid"];
            //string kimlik = Request.QueryString["kimlik"];
            //if (!String.IsNullOrEmpty(servis_id) && !String.IsNullOrEmpty(cust_id) && !String.IsNullOrEmpty(kimlik))
            //{
            //    var url = "/TeknikTeknik/ServisDetayList.aspx?servisid=" + servis_id + "&kimlik=" + kimlik;
            //    Response.Redirect(url);
            //}
        }


        protected void btnOnay_ClickH(object sender, EventArgs e)
        {
            string hesapS = hdnHesapIDH.Value;
            string musteriID = hdnMusteriIDH.Value.Trim();
            int custid = Int32.Parse(musteriID);
            int hesapID = Int32.Parse(hesapS);

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri ser = new ServisIslemleri(dc);

                if (musteriID != "-99")
                {
                    //burada cariyi çekecez
                    musteri_bilgileri musteri_bilgileri = ser.servisKararOnayCalismaTipli(hesapID, User.Identity.Name);
                    //karar onayında stok kontrolü yapılıyor
                    //eğer stok yoksa müşteri bilgileri boş döndürülüyor
                    if (!string.IsNullOrEmpty(musteri_bilgileri.ad))
                    {

                        //if (chcMailH.Checked == true || chcSmsH.Checked == true)
                        //{
                        //    int servisid = Int32.Parse(hdnServisIDDH.Value);
                        //    string islem = hdnIslemmH.Value;
                        //    string yekun = hdnYekunnH.Value;
                        //    Radius.service serr = ser.servisTekR(servisid);
                        //}

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#onayModalH').modal('hide');");
                        sb.Append("alertify.success('Hesap onaylandı!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);
                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#onayModalH').modal('hide');");
                        sb.Append("alertify.error('Malzeme stoğu sıfır görünüyor!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);
                    }

                }


                ortak(dc);
            }

        }

        protected void btnTopluOnay_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            sb.Append("$('#topluModal').modal('show');");

            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "TopluModalScript", sb.ToString(), false);
        }

        protected void btnTopluOnayKaydet_Click(object sender, EventArgs e)
        {
            //string servis_id = Request.QueryString["servisid"];
            string servis_id = hdnServisID.Value;
            string cust_id = Request.QueryString["custid"];

            if (!String.IsNullOrEmpty(servis_id) && !String.IsNullOrEmpty(cust_id))
            {
                int servisid = Int32.Parse(servis_id);
                int custid = Int32.Parse(cust_id);

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    ServisIslemleri s = new ServisIslemleri(dc);
                    if (custid != -99)
                    {
                        int i = s.kararOnayTopluYeni(servisid, User.Identity.Name);
                        if (i > 0)
                        {
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append("$('#topluModal').modal('hide');");
                            sb.Append("alertify.success('onaylandı!');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PModalScript", sb.ToString(), false);
                        }
                        else
                        {
                            //stok kontrolüünde bazı hesaplar çakılmış
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append("$('#topluModal').modal('hide');");
                            sb.Append("alertify.error('Bazı ürün stokları sıfır görünüyor!');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PModalScript", sb.ToString(), false);
                        }


                    }


                    ortak(dc);


                }
            }
        }

        protected void grdMusteri_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                musteriGoster(dc);
                hdnTamirciID.Value = grdMusteri.SelectedValue.ToString();
            }

        }

        protected void grdMusteri_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnKaydetTamirci_Click(object sender, EventArgs e)
        {
            //string servisidd = Request.QueryString["servisid"];
            string servisidd = hdnServisID.Value;
            string custidd = Request.QueryString["custid"];
            string hesapidd = hdnHesapIDDuzenTamirci.Value;
            string kimlik = Request.QueryString["kimlik"];
            int? custid = null;
            if (!String.IsNullOrEmpty(custidd))
            {
                custid = Int32.Parse(custidd);

            }
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);

                int servisid = Int32.Parse(servisidd);

                string islem = txtIslemParcaTamirci.Value;

                decimal kdv = Decimal.Parse(txtKDVOraniDuzenleTamirci.Text);
                decimal yekun = Decimal.Parse(txtYekunTamirci.Text);
                string aciklama = txtAciklamaTamirci.Text;
                decimal maliyet = Decimal.Parse(txtTamirciMaliyet.Text);
                int tamirci_id = Convert.ToInt32(hdnTamirciID.Value);
                DateTime tarihi = DateTime.Now;
                if (!String.IsNullOrEmpty(tarihtamirci.Value))
                {
                    tarihi = DateTime.Parse(tarihtamirci.Value);
                }

                if (!String.IsNullOrEmpty(servisidd) && String.IsNullOrEmpty(hesapidd))
                {
                    //string firma = KullaniciIslem.firma();

                    //yeni ekleme

                    s.serviceKararEkleTamirci(servisid, custid, tamirci_id, islem, kdv, yekun, maliyet, aciklama, tarihi, User.Identity.Name);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");

                    sb.Append(" alertify.success('Hesap!');");
                    sb.Append("$('#tamirciModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tamirciModalScript", sb.ToString(), false);
                }
                else if (!String.IsNullOrEmpty(servisidd) && !String.IsNullOrEmpty(hesapidd))
                {
                    int hesapid = Int32.Parse(hesapidd);
                    s.serviceKararGuncelleTamirci(hesapid, tamirci_id, islem, kdv, yekun, maliyet, aciklama, tarihi, User.Identity.Name);

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    sb.Append(@"<script type='text/javascript'>");

                    sb.Append(" alertify.success('Güncelledi!');");
                    sb.Append("$('#tamirciModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tamirciModalScript", sb.ToString(), false);
                }

                ortak(dc);
            }
        }

        protected void btnEkle_Click(object sender, EventArgs e)
        {
            string id = txtServisID.Value;
            string custid = Request.QueryString["custid"];
            string atanan = hdnAtananID.Value;
            string kimlik = Request.QueryString["kimlik"];
            Response.Redirect("/TeknikTeknik/ServisDetay.aspx?id=" + id + "&atanan=" + atanan + "&kimlik=" + kimlik + "&custid=" + custid);

        }
        protected void btnTamirciye_Click(object sender, EventArgs e)
        {
            //string servisidd = Request.QueryString["servisid"];
            //string custidd = Request.QueryString["custid"];
            hdnHesapIDDuzenTamirci.Value = "";
            hdnTamirciID.Value = "";
            //hdnHesapIDDuzen.Value = "";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#tamirciModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tamirciModalScript", sb.ToString(), false);


        }
        protected void MusteriAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                musteriGoster(dc);
            }

        }
        private void musteriGoster(radiusEntities dc)
        {
            string s = txtMusteriSorgu.Value;
            MusteriIslemleri m = new MusteriIslemleri(dc);
            if (!String.IsNullOrEmpty(s))
            {
                grdMusteri.DataSource = m.musteriAraR2(s, "tamirci");
                grdMusteri.DataBind();

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
        protected void ListView1_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            //set current page startindex, max rows and rebind to false
            DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            DataPager2.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                BindList(dc);
            }

        }
        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            pager.Visible = DataPager1.TotalRowCount > DataPager1.MaximumRows;
            pager.Visible = DataPager2.TotalRowCount > DataPager2.MaximumRows;
        }
        protected void btnOnay_Click(object sender, EventArgs e)
        {


            string id = txtServisID.Value;
            int servisid = Int32.Parse(id);
            int custID = Int32.Parse(hdnCustID.Value);

            string kimlikNo = Request.QueryString["kimlik"];

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri ser = new ServisIslemleri(dc);
                ser.servisKapatR(servisid, User.Identity.Name);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('İş şantiye hesabı kapatıldı!');");
                sb.Append("$('#onayModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onayModalScript", sb.ToString(), false);

                Response.Redirect("/TeknikTeknik/Servis2?custid=" + custID.ToString() + "&servisid=" + servisid.ToString() + "&kimlik=" + kimlikNo);
            }


            ////kapatma belgesi yazdırılacak/yada burada olmadan yazdırılabilir.
            //string url = "/TeknikTeknik/ServisDetayList.aspx?kimlik=" + kimlikNo;
            //Response.Redirect(url);
        }
        protected void btnSonlandir_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#onayModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);

        }
        protected void btnBelge_Click(object sender, EventArgs e)
        {

            //string id = txtServisID.Value;
            //int servisid = Int32.Parse(id);

            //burada servis başlama için bir geçiş sınıfıyla Session'a atıp Baski.aspx'e gönder
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                FaturaBas bas = new FaturaBas(dc);
                AyarCurrent ay = new AyarCurrent(dc);
                Servis_Baslama servisBilgisi = bas.ServisBilgileri(txtKimlikNo.Value.Trim(), ay.get());
                Session["Servis_Baslama"] = servisBilgisi;
            }

            string uri = "/Baski.aspx?tip=baslama";
            Response.Redirect(uri);

            //string kimlikNo = Request.QueryString["kimlik"];
            //string url = "/TeknikTeknik/ServisBelgesi.aspx?kimlik=" + kimlikNo;
            //Response.Redirect(url);
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
                    hdnTarifeTipi.Value = t.tarife_kodu;
                    sure_hesapla();
                }

                //fiyatCek(dc);
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

        protected void grdAtamalar_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        Operator al = new Operator(dc, "-");
                        al.ServisOperatorIptal(id);
                        ortak(dc);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.error('Kayıt silindi!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }

                }

            }

            else if (e.CommandName.Equals("cikis"))
            {
                hdnServisOpID.Value = Convert.ToString(e.CommandArgument);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#cikisModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CikisShowModalScript", sb.ToString(), false);
            }
        }

        protected void grdOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kullanici = grdOperator.SelectedValue.ToString();
            string servisids = hdnServisID.Value;

            if (!String.IsNullOrEmpty(kullanici) && !String.IsNullOrEmpty(servisids))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    Operator op = new Operator(dc, kullanici);
                    string isim = txtMusteri.InnerHtml;
                    string acik = hdnAciklama.Value;
                    string tel = grdOperator.SelectedRow.Cells[2].Text;
                    string mesaj = "Şantiyeye atandınız: " + "Müşteri ismi: " + isim + " Şantiye detayları: " + acik;

                    op.ServisOperatorAta(Int32.Parse(servisids));
                    AyarIslemleri ayarimiz = new AyarIslemleri(dc);
                    SmsIslemleri sms = new SmsIslemleri(dc);
                    sms.SmsGenel(ayarimiz, mesaj, new string[] { tel });
                    ortak(dc);


                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#atamaModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AtaShowModalScript", sb.ToString(), false);
                }
            }
        }

        protected void btnAta_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#atamaModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AtaShowModalScript", sb.ToString(), false);
        }

        protected void OperatorAra(object sender, EventArgs e)
        {
            string s = txtOperatorAra.Value;
            if (!String.IsNullOrEmpty(s))
            {
                grdOperator.DataSource = KullaniciIslem.FirmaOperatorleri(s);
                grdOperator.DataBind();
            }
        }

        protected void btnCikarKaydet_Click(object sender, EventArgs e)
        {
            string ids = hdnServisOpID.Value;
            if (!String.IsNullOrEmpty(ids))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    Operator op = new Operator(dc, "-");
                    op.ServisOperatorCikar(Int32.Parse(ids));
                    ortak(dc);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#cikisModal').modal('hide');");
                    sb.Append(" alertify.error('Çıkış kaydedildi!');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AtaShowModalScript", sb.ToString(), false);
                }
            }

        }

        protected void txtYeniNumara_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtFiyatMalzeme_TextChanged(object sender, EventArgs e)
        {
            tutarHesap();

        }

        private void tutarHesap()
        {
            decimal fiyat = 0;
            decimal adet = 0;
            decimal tutar = 0.00M;
            if (!String.IsNullOrEmpty(txtFiyatMalzeme.Text))
            {
                fiyat = Decimal.Parse(txtFiyatMalzeme.Text);
            }
            if (!String.IsNullOrEmpty(txtAdet.Text))
            {
                adet = Decimal.Parse(txtAdet.Text);
            }

            tutar = (decimal)(adet * fiyat);
            txtYekun.Text = String.Format("{0:0.00}", tutar);
        }

        protected void txtAdet_TextChanged(object sender, EventArgs e)
        {
            tutarHesap();
        }

    }
}