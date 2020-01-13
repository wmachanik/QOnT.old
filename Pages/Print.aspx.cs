using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QOnT.classes;

namespace QOnT.Pages
{
  public partial class Print : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Control PrintCtrl = (Control)Session["PrintCtrl"];
      QOnT.classes.PrintHelper.PrintWebControl(PrintCtrl);
    }
  }
}