using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RelatorioFinanceiroV5
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblData.Text = DateTime.Today.ToShortDateString();
        }

        protected void linkRelGrupo_Click(object sender, EventArgs e)
        {
            liGrupo.Attributes["class"] = "active";
        }

        protected void linkBrandName_Click(object sender, EventArgs e)
        {
            liGrupo.Attributes["class"] = "";
            liEditora.Attributes["class"] = "";
        }
    }
}