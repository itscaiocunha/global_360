using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace global
{
    public partial class principal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    lblUser.Text = Session["nomeusuario"].ToString();
                }
                catch
                {
                    Response.Redirect("../sessao.aspx", false);
                }
            }
        }
    }
}