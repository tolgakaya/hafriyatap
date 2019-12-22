using System;
using System.Linq;
using System.Web;
using TeknikServis.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace TeknikServis.AdminSuper
{
    public partial class AdminDuzenle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void CreateUser_Click(object sender, EventArgs e)
        {


            //ilk admini oluştur
            //ayargeneli kaydet
            //notification


            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            string firmamiz = Request.QueryString["config"];
            
            string resimYol = "/Uploads/" + firmamiz.ToLower() + ".png";
            var user = new ApplicationUser() { UserName = UserName.Text, Email = Email.Text, Firma = firmamiz, Adres = "adres", Tel = "05069468693", Web = "web", TamFirma = "tam firma ismi", resimYol = resimYol };
            IdentityResult result = manager.Create(user, Password.Text);
            if (!manager.IsInRole(manager.FindByEmail(Email.Text).Id, "Admin"))
            {
                result = manager.AddToRole(manager.FindByEmail(Email.Text).Id, "Admin");
            }
            if (result.Succeeded)
            {

                Response.Redirect("/AdminSuper/Firmalar");

            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}