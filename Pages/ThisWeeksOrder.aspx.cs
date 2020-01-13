using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QOnT.Pages
{
  public partial class ThisWeeksOrder : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ddlOrdersPerPage_SelectedIndexChanged(object sender, EventArgs e)
    {
      gvOutstandingOrders.PageSize = Convert.ToInt16(ddlOrdersPerPage.SelectedValue);
    }
  }
}