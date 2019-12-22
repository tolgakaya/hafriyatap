using ServisDAL;
using System;
using System.Web;
using TeknikServis.Logic;


namespace TeknikServis
{
    public partial class ServisDetay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }
            if (!IsPostBack)
            {
                hdnKul.Value = User.Identity.Name;
             

                string id = Request.QueryString["durum"];
                string atanan = Request.QueryString["atanan"];
                
                //baslikDetay.InnerText += "-" + atanan;
                if (HttpContext.Current.User.IsInRole("Admin") || User.IsInRole("mudur"))
                {
                    kullaniciSecim.Visible = true;
                    if (!IsPostBack)
                    {
                        drdKullanici.AppendDataBoundItems = true;

                        drdKullanici.DataSource = KullaniciIslem.firmaKullanicilari();

                        drdKullanici.DataValueField = "id";
                        drdKullanici.DataTextField = "userName";
                        drdKullanici.DataBind();

                        if (!String.IsNullOrEmpty(atanan))
                        {
                            drdKullanici.SelectedValue = atanan;

                        }

                    }
                }

            }

        }
        protected void Button1_Click(object sender, EventArgs e)
        {

        }

    }
}