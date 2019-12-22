using ServisDAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Linq;
using TeknikServis.Radius;

namespace TeknikServis.TeknikOperator
{
    public partial class ServislerCanli : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }

            kullanici_repo kullanici = KullaniciIslem.currentKullanici();

            string firma = kullanici.Firma;
            using (radiusEntities dc = MyContext.Context(firma))
            {
                AyarCurrent ay = new AyarCurrent(dc);
                if (ay.lisansKontrol() == false)
                {
                    Response.Redirect("/LisansError");
                }
                AyarIslemleri ayarimiz = new AyarIslemleri(dc);
                if (!IsPostBack)
                {
                   
                    gosterHepsi(kullanici, dc);
                }


            }


        }

        private void gosterHepsi(kullanici_repo kull, radiusEntities dc)
        {
           
            string kelime = "";
      
            if (!String.IsNullOrEmpty(txtAra.Value))
            {
                kelime = txtAra.Value;
            }
       
                ServisIslemleri ser = new ServisIslemleri(dc);
                GridView1.DataSource = ser.ServisListesiOperator(kelime, User.Identity.Name);
                GridView1.DataBind();
    
        
        }


     

        public void MusteriAra(object sender, EventArgs e)
        {
            kullanici_repo kullanici = KullaniciIslem.currentKullanici();
            using (radiusEntities dc = MyContext.Context(kullanici.Firma))
            {
                gosterHepsi(kullanici, dc);
            }


        }
   

        protected void GridView1_OnRowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnServis") as LinkButton).PostBackUrl = "~/TeknikOperator/Servis2.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");
                (e.Row.FindControl("btnKucuk") as LinkButton).PostBackUrl = "~/TeknikOperator/Servis2.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");
 
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

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
       
     
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            kullanici_repo kullanici = KullaniciIslem.currentKullanici();
            using (radiusEntities dc = MyContext.Context(kullanici.Firma))
            {
                gosterHepsi(kullanici, dc);
            }
            

        }

    
    }
}