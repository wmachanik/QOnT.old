using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QOnT.Pages
{
  public partial class Customers : System.Web.UI.Page
  {

    const string CONST_WHERECLAUSE_SESSIONVAR = "CustomerSummaryWhereFilter";

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if ((Request.QueryString["CompanyName"]) != null)
        {
          tbxFilterBy.Text = Request.QueryString["CompanyName"].ToString();
          ddlFilterBy.SelectedValue = "CompanyName";

          Session[CONST_WHERECLAUSE_SESSIONVAR] = "CompanyName LIKE '" + tbxFilterBy.Text + "%'"; 
        }
        else
          Session[CONST_WHERECLAUSE_SESSIONVAR] = "";

        gvCustomers.Sort("CompanyName", SortDirection.Ascending);
      }
      else
        if (Session[CONST_WHERECLAUSE_SESSIONVAR]!=null)
          lblFilter.Text = Session[CONST_WHERECLAUSE_SESSIONVAR].ToString(); 
    }
    protected void btnGon_Click(object sender, EventArgs e)
    {
      if ((ddlFilterBy.SelectedValue != "0") && (!String.IsNullOrWhiteSpace (tbxFilterBy.Text)))
      {
        Session[CONST_WHERECLAUSE_SESSIONVAR] = (ddlFilterBy.SelectedValue + " LIKE '" + tbxFilterBy.Text + "%'"); 

        odsCustomerSummarys.DataBind();
      }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
      Session[CONST_WHERECLAUSE_SESSIONVAR] = "";
      
      ddlFilterBy.SelectedIndex = 0;
      tbxFilterBy.Text = "";
      odsCustomerSummarys.DataBind();
    }

    protected void tbxFilterBy_TextChanged(object sender, EventArgs e)
    {
      if ((!String.IsNullOrWhiteSpace(tbxFilterBy.Text)) && (ddlFilterBy.SelectedIndex == 0))
      {
        ddlFilterBy.SelectedIndex = 1;   // should be company
        upnlSelection.Update();
      }
    }

  }
}