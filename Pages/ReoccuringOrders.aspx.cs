using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;

namespace QOnT.Pages
{
  public partial class ReoccuringOrders : System.Web.UI.Page
  {
    const string GET_DATA_SELECT =
        "SELECT CustomersTbl.CompanyName, ReoccuranceTypeTbl.Type, ReoccuringOrderTbl.[Value], ItemTypeTbl.ItemDesc, ReoccuringOrderTbl.QtyRequired, " +
               "ReoccuringOrderTbl.DateLastDone, ReoccuringOrderTbl.NextDateRequired, ReoccuringOrderTbl.RequireUntilDate, " +
               " ReoccuringOrderTbl.Enabled, ReoccuringOrderTbl.Notes " +
         "FROM (((ReoccuringOrderTbl INNER JOIN " +
               "ReoccuranceTypeTbl ON ReoccuringOrderTbl.ReoccuranceType = ReoccuranceTypeTbl.ID) INNER JOIN " +
               "ItemTypeTbl ON ReoccuringOrderTbl.ItemRequiredID = ItemTypeTbl.ItemTypeID) LEFT OUTER JOIN " +
               "CustomersTbl ON ReoccuringOrderTbl.CustomerID = CustomersTbl.CustomerID)";

    OleDbDataAdapter daReoccuringOrders;
    DataSet dsReoccuringOrders;

    private void LoadReoccuringOrdersData()
    {
      try
      {
        string QTrackerConnString = ConfigurationManager.ConnectionStrings["Tracker08ConnectionString"].ConnectionString;


//        SqlConnection objConn = new SqlConnection(QTrackerConnString);
        OleDbConnection objConn = new OleDbConnection(QTrackerConnString);

        objConn.Open();

        daReoccuringOrders = new OleDbDataAdapter(GET_DATA_SELECT, objConn);
        dsReoccuringOrders = new DataSet("ReocurringOrders");

        daReoccuringOrders.Fill(dsReoccuringOrders, "ReoccuringOrderTbl");

        gvReoccuringOrders.DataSource = dsReoccuringOrders;
        gvReoccuringOrders.DataBind();

      }
      catch (Exception ex)
      {
        if (ex.Message != "")
          ltrlMessage.Text = "<b>ERROR:</b> " + ex.Message + " - source: " + ex.Source;
        
      }

    }

    protected void Page_Load(object sender, EventArgs e)
    {

      if (!IsPostBack )
        LoadReoccuringOrdersData();
    }

    protected void Page_UnLoad(object sender, EventArgs e)
    {
      //dsReoccuringOrders.Dispose();
      //daReoccuringOrders.Dispose();
    }

  }
}