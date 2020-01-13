using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace QOnT.Pages
{
  public partial class DeleteOrderLine : System.Web.UI.Page
  {
    const string CONST_CONSTRING = "Tracker08ConnectionString";

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (Request.UrlReferrer == null)
        {
          Session["ReturnOrderURL"] = "";
          btnReturn.Enabled = false;
        }
        else
          Session["ReturnOrderURL"] = Request.UrlReferrer.OriginalString.ToString();
      }
    }
    protected void ReturnToDetailPage()
    {
      string returnURL = Session["ReturnOrderURL"].ToString();
      if (returnURL.Length > 0)
        Response.Redirect(returnURL);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
      if (Request.QueryString["OrderId"] != null)
      {

        bool _ItemAdded = false;
        string _connectionString;
        string _ErrorStr = "";

        if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
            ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
        {
          throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
                              "must exist in the <connectionStrings> configuration section for the application.");
        }
        _connectionString =
          ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

        // Label _lblOrderId = (Label)gvOrderLines.FindControl("lblOrderID");

        string _sqlDeleteCmd = "DELETE FROM OrdersTbl WHERE OrderID = " + Request.QueryString["OrderId"].ToString();

        OleDbConnection _conn = new OleDbConnection(_connectionString);                           //1  2  3  4  5  6  7  8  9  10 11
        // add parameters in the order they appear in the update command
        OleDbCommand _cmd = new OleDbCommand(_sqlDeleteCmd, _conn);

        try
        {
          _conn.Open();
          _ItemAdded = (_cmd.ExecuteNonQuery() != 0);
        }
        catch (OleDbException oleErr)
        {
          // Handle exception.
          _ErrorStr = oleErr.Message;
        }
        finally
        {
          _conn.Close();
        }
        ltrlStatus.Text = (_ItemAdded == true ? "Item Deleted" : "Error deleting item: " + _ErrorStr);

        dvDeleteOrderItem.DataBind();
        ReturnToDetailPage(); ;
      }
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
      ReturnToDetailPage(); ;
    }
  }
}