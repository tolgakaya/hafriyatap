using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Radius;
using ServisDAL;
using TeknikServis.Logic;

namespace TeknikServis
{
    public partial class Alarmlar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AlarmIslemleri a = new AlarmIslemleri(dc);
                GridView1.DataSource = a.AlarmListesi();
                GridView1.DataBind();
            }
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("bag"))
            {
                string lnk = e.CommandArgument.ToString();
                Response.Redirect(lnk);

            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}