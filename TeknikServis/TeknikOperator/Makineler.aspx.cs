using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServisDAL;
using TeknikServis.Logic;
using System.IO;
using System.Text;
using TeknikServis.Radius;

namespace TeknikServis.TeknikOperator
{
    public partial class Makineler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
                    Ara(dc);
                }

            }

        }

        private void Ara(radiusEntities dc)
        {
            MakineIslem s = new MakineIslem(dc);
            string terim = txtAra.Value;
            grdAlimlar.DataSource = s.makinelerOperator(terim, User.Identity.Name);
            grdAlimlar.DataBind();

        }

        protected void MakineAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                Ara(dc);
            }

        }


        protected void grdAlimlar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlimlar.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                Ara(dc);
            }
        }



        protected void grdAlimlar_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //detay için makine masraf hesaplarına bakılabilir
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnDetay") as LinkButton).PostBackUrl = "~/TeknikOperator/SarfKayitlar?makineid=" + DataBinder.Eval(e.Row.DataItem, "makine_id");
            }
        }





    }
}