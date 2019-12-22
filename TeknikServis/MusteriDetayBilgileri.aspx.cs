using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServisDAL;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class MusteriDetayBilgileri : System.Web.UI.Page
    {
        string firma;

        public MusteriDetayBilgileri()
        {
            firma = KullaniciIslem.firma();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }


            using (radiusEntities dc = MyContext.Context(firma))
            {
                goster(dc);

                if (!IsPostBack)
                {
                    AyarCurrent ay = new AyarCurrent(dc);

                    if (ay.calismaTipli() == true)
                    {

                        btnSatis.Visible = false;
                        btnFatura.Visible = false;
                    }
                    else
                    {
                        btnSatis.Visible = true;
                        btnFatura.Visible = true;

                    }
                }

            }


            if (User.IsInRole("servis"))
            {
                btnTahsilat.Visible = false;
                btnOde.Visible = false;
                btnExtre.Visible = false;
                btnFatura.Visible = false;

                panelOdeme.Visible = false;
                panelAlim.Visible = false;
            }
            else if (User.IsInRole("dukkan"))
            {
                btnFatura.Visible = false;
            }


        }

        private void goster(radiusEntities dc)
        {
            string idd = Request.QueryString["custID"];
            if (!String.IsNullOrEmpty(idd))
            {

                int id = Int32.Parse(idd);

                TekMusteri tek = new TekMusteri(dc, id);
                MusteriDetay bilgi = tek.DetayGoster();
                if (bilgi != null)
                {
                    int servis_sayisi = 0;
                    int onay_bekleyen = 0;
                    int emanet = 0;

                    if (bilgi.servis.Count > 0)
                    {
                        panelServis.Visible = true;
                        servis_sayisi = bilgi.servis.Count();
                        grdServis.DataSource = bilgi.servis;
                        grdServis.DataBind();
                    }
                    if (bilgi.kararlar.Count > 0)
                    {
                        panelKarar.Visible = true;
                        onay_bekleyen = bilgi.kararlar.Count();
                        grdKarar.DataSource = bilgi.kararlar;
                        grdKarar.DataBind();
                    }
                    if (bilgi.tamirler.Count > 0)
                    {
                        panelTamir.Visible = true;

                        grdTamir.DataSource = bilgi.tamirler;
                        grdTamir.DataBind();
                    }
                    if (bilgi.yedekler.Count > 0)
                    {
                        panelYedek.Visible = true;
                        emanet = bilgi.yedekler.Count();
                        grdYedek.DataSource = bilgi.yedekler;
                        grdYedek.DataBind();
                    }

                    if (bilgi.odemeler.Count > 0)
                    {
                        panelOdeme.Visible = true;
                        grdOdeme.DataSource = bilgi.odemeler;
                        grdOdeme.DataBind();
                    }
                    if (bilgi.alimlar.Count > 0)
                    {
                        panelAlim.Visible = true;
                        grdAlimlar.DataSource = bilgi.alimlar;
                        grdAlimlar.DataBind();
                    }



                    if (bilgi.urunler.Count > 0)
                    {
                        panelUrun.Visible = true;
                        grdUrun.DataSource = bilgi.urunler;
                        grdUrun.DataBind();
                    }
                    DateTime bugun = DateTime.Now.Date;
                    // DateTime exp = bilgi.musteri.expiration;

                    if (!String.IsNullOrEmpty(bilgi.musteri.istihbarat))
                    {
                        istihbarat.Visible = true;
                        alarm.InnerHtml = bilgi.musteri.istihbarat;
                    }
                    if (!string.IsNullOrEmpty(bilgi.eksikler))
                    {
                        eksikbilgiler.Visible = true;
                        eksikler.InnerHtml = bilgi.eksikler;
                    }
                    else
                    {
                        istihbarat.Visible = false;
                    }

                    //txtKalan.InnerHtml = kalanGun.ToString();
                    //txtExp.InnerHtml = exp.ToShortDateString();
                    //txtDurum2.InnerHtml = bilgi.musteri.durum;
                    //spnPaket.InnerHtml = bilgi.musteri.paket;
                    spnAdres.InnerHtml = bilgi.musteri.adres;
                    spnTc.InnerHtml = bilgi.musteri.tc;
                    spnTel.InnerHtml = bilgi.musteri.tel;

                    baslik.InnerHtml = bilgi.musteri.adi;

                    servisSayisi.InnerHtml = servis_sayisi.ToString();
                    onayBekleyen.InnerHtml = onay_bekleyen.ToString();
                    emanetSayisi.InnerHtml = emanet.ToString();

                    decimal bak = bilgi.cari.netBakiye;

                    if (bak > 0)
                    {
                        netBakiye.InnerHtml = bilgi.cari.netBakiye.ToString("C");
                        bakiye_bilgi.InnerHtml = "Borcu Var!";
                    }
                    else if (bak == 0)
                    {
                        netBakiye.InnerHtml = bilgi.cari.netBakiye.ToString("C");
                        bakiye_bilgi.InnerHtml = "Alacak Borç Yok!";
                    }
                    else if (bak < 0)
                    {
                        netBakiye.InnerHtml = (-bilgi.cari.netBakiye).ToString("C");
                        bakiye_bilgi.InnerHtml = "Alacağı Var!";
                    }



                }
                decimal bor = bilgi.cari.netBakiye;
                decimal al = bilgi.cari.netBakiye;
                decimal alacak_mahsup = bilgi.cari.netAlacak;
                if (al < 0)
                {
                    bor = 0;
                    //alacağı var ise mahsup miktarı net borcu kadardır
                    Session["alacak_mahsup"] = bilgi.cari.netBorclanma.ToString();
                    al = -al;
                }
                else
                {
                    //borcu varsa mahsup miktarı net alacağı kadardır
                    Session["alacak_mahsup"] = bilgi.cari.netAlacak.ToString();
                }



                //Session["borc"] = bor.ToString();

                //Session["alacak"] = al.ToString();


                linkEmanetler.HRef = "/TeknikEmanet/Emanetler.aspx?custid=" + idd;
                linkEmanetYeni.HRef = "/TeknikEmanet/EmanetVer.aspx?custid=" + idd;
                linkOdemeler.HRef = "/TeknikCari/Odemeler.aspx?custid=" + idd;
                linkOdemeYeni.HRef = "/TeknikCari/OdemeEkle.aspx?custid=" + idd;

                linkSatinAlma.HRef = "/TeknikAlim/Alimlar.aspx?custid=" + idd;
                linkSatinDetay.HRef = "/TeknikAlim/AlimDetaylar.aspx?custid=" + idd;
                linkServis.HRef = "/TeknikTeknik/ServislerCanli.aspx?custid=" + idd;


            }
        }
        protected void grdUrun_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {

                string confirmValue = Request.Form["confirm_value2"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {

                    using (radiusEntities dc = MyContext.Context(firma))
                    {
                        int urunID = Convert.ToInt32(e.CommandArgument);
                        CihazMalzeme s = new CihazMalzeme(dc);
                        s.garanti_iptal(urunID);
                        goster(dc);
                    }

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('Kayıt silindi!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript4", sb.ToString(), false);

                }


                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('" + deger + "');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript5", sb.ToString(), false);
                }


            }
            else if (e.CommandName.Equals("iade"))
            {
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int urunID = Convert.ToInt32(arg[0]);

                int index = Convert.ToInt32(arg[1]);
                GridViewRow row = grdUrun.Rows[index];
                txtIadeTutar.Text = row.Cells[7].Text;
                hdnCustID.Value = row.Cells[9].Text;

                hdnGarantiID.Value = urunID.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#onayModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
            }
        }
        protected void grdUrun_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string tipi = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "durum"));
                if (tipi.Equals("iade"))
                {
                    e.Row.CssClass = "danger";
                    LinkButton link = e.Row.Cells[0].Controls[1] as LinkButton;
                    LinkButton link2 = e.Row.Cells[0].Controls[3] as LinkButton;
                    link.Visible = false;
                    link2.Visible = false;
                }

            }

        }

        protected void btnIade_Click(object sender, EventArgs e)
        {
            //kullanici_repo kullanici = KullaniciIslem.currentKullanici();
            int urunID = Int32.Parse(hdnGarantiID.Value);
            decimal iade_tutar = Decimal.Parse(txtIadeTutar.Text);
            string aciklama = txtIadeAciklama.Text;
            int custid = Int32.Parse(hdnCustID.Value);


            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme s = new CihazMalzeme(dc);
                s.garanti_iade(urunID, iade_tutar, aciklama, custid, User.Identity.Name);
                goster(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.error('Malzeme iade alındı!');");
            sb.Append("$('#onayModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

        }

        protected void btnOdeme_Click(object sender, EventArgs e)
        {
            string custID = Request.QueryString["custID"];
            if (!String.IsNullOrEmpty(custID))
            {
                string url = "/TeknikCari/OdemeEkle.aspx?custid=" + custID;
                Response.Redirect(url);
            }

        }



        protected void pesin_tahsilat(radiusEntities dc, DateTime odeme_tarihi, decimal tutar, string aciklama, int custid)
        {

            //fat.FaturaOdeTur(custid, tutar, "Nakit", null, aciklama, null, "", null, false, "", null, odeme_tarihi, User.Identity.Name);

            Tahsilat t = new Tahsilat(dc);
            t.Aciklama = aciklama;
            t.kullanici = User.Identity.Name;
            t.KullaniciID = User.Identity.Name;
            t.mahsup = false;
            t.Musteri_ID = custid;
            t.OdemeMiktar = tutar;
            t.OdemeTarih = odeme_tarihi;
            t.Nakit(User.Identity.Name);
        }

        protected void btnFatura_Click(object sender, EventArgs e)
        {
            string custID = Request.QueryString["custID"];
            string url = "~/TeknikFatura/FaturaManuel.aspx?custID=" + custID;
            Response.Redirect(url);
        }


        protected void btnMesaj_Click(object sender, EventArgs e)
        {
            string custID = Request.QueryString["custID"];
            if (!String.IsNullOrEmpty(custID))
            {
                string tel = string.Empty;

                using (radiusEntities dc = MyContext.Context(firma))
                {
                    MusteriIslemleri mu = new MusteriIslemleri(dc);
                    tel = mu.musteriTEL(Int32.Parse(custID));
                }

                if (!String.IsNullOrEmpty(tel))
                {
                    string teller = tel + ",";
                    Session["teller"] = teller;
                    Response.Redirect("/MesajGonder.aspx?tur=sms&tip=gnltp");
                }


            }

        }

        protected void btnMail_Click(object sender, EventArgs e)
        {
            string custID = Request.QueryString["custID"];
            if (!String.IsNullOrEmpty(custID))
            {
                string mail = string.Empty;
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    MusteriIslemleri mu = new MusteriIslemleri(dc);
                    mail = mu.musteriMail(Int32.Parse(custID));
                }

                if (!String.IsNullOrEmpty(mail))
                {
                    string mailler = mail + ";";
                    Session["mailler"] = mailler;
                    Response.Redirect("/MesajGonder.aspx?tur=mail&tip=gnltp");
                }


            }

        }

        protected void grdServis_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //(e.Row.FindControl("btnDetay") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServisDetayList.aspx?kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");
                //bütün servis hesaplarını gösterecek //oradan hesap eklenebilir yada detaytan hesap eklenebilir
                (e.Row.FindControl("btnHesap") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis2.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");
                (e.Row.FindControl("btnKucuk") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis2.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");

            }
        }
        protected void grdAlimlar_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnDetay") as LinkButton).PostBackUrl = "~/TeknikAlim/AlimDetaylar.aspx?alimid=" + DataBinder.Eval(e.Row.DataItem, "alim_id");

            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string css = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "css"));
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(css);
            }
        }



        protected void grdYedek_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int id = Convert.ToInt32(e.CommandArgument);


                using (radiusEntities dc = MyContext.Context(firma))
                {
                    ServisIslemleri s = new ServisIslemleri(dc);
                    s.emanetAlR(id, User.Identity.Name);
                    goster(dc);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Emanet teslimi alındı!');");

                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
            }
        }

        protected void btnOde_Click(object sender, EventArgs e)
        {
            string custID = Request.QueryString["custID"];
            if (!String.IsNullOrEmpty(custID))
            {
                string url = "/TeknikCari/Ode.aspx?custid=" + custID;
                Response.Redirect(url);
            }
        }

        protected void btnExtre_Click(object sender, EventArgs e)
        {
            string custID = Request.QueryString["custID"];
            if (!String.IsNullOrEmpty(custID))
            {
                string url = "/TeknikCari/CariDetay.aspx?custid=" + custID;
                Response.Redirect(url);
            }
        }

        protected void btnIstihbarat_Click(object sender, EventArgs e)
        {
            string custID = Request.QueryString["custID"];

            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri mu = new MusteriIslemleri(dc);
                int id = Int32.Parse(custID);
                mu.istihbarat_kaydet(id, txtNot.Text.Trim());
                goster(dc);
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Kayıt yapıldı!');");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowwModalScript", sb.ToString(), false);
        }

        protected void btnIstihbaratSil_Click(object sender, EventArgs e)
        {
            string custID = Request.QueryString["custID"];

            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri mu = new MusteriIslemleri(dc);
                int id = Int32.Parse(custID);
                mu.istihbarat_kaydet(id, "");
                goster(dc);
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Kayıt silindi!');");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHModalScript", sb.ToString(), false);
        }

        protected void btnYeniIstihbarat_Click(object sender, EventArgs e)
        {
            string not = alarm.InnerHtml;
            txtNot.Text = not;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }

        protected void btnSatis_Click(object sender, EventArgs e)
        {
            string custID = Request.QueryString["custID"];
            Response.Redirect("/TeknikTeknik/SatisEkle.aspx?custid=" + custID);
        }

        protected void grdUrun_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!User.IsInRole("Admin"))
                {
                    (e.Row.FindControl("delUrun") as LinkButton).Visible = false;
                }

            }
        }

        protected void btnKirala_Click(object sender, EventArgs e)
        {
            string custID = Request.QueryString["custID"];

            //Response.Redirect("/TeknikTeknik/SatisEkle2?custid=" + custID);
            Response.Redirect("/TeknikMakine/HizliKirala2");

            //using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
            //{
            //    AyarCurrent ay = new AyarCurrent(dc);
            //    if (ay.calismaTipli() == true)
            //    {
            //        Response.Redirect("/TeknikTeknik/SatisEkle2?custid=" + custID);
            //    }
            //    else
            //    {
            //        Response.Redirect("/TeknikMakine/SatisEkleMakine?custid=" + custID);
            //    }
            //}


        }





    }
}