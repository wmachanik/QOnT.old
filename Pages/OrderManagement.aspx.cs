using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using QOnT.classes;

namespace QOnT.Pages
{

  public partial class OrderManagement : System.Web.UI.Page
  {
    const string CONST_TRACKERDB_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_ORDERMAIN =
      "SELECT OrdersTbl.CustomerID, CustomersTbl.CompanyName, OrdersTbl.OrderDate, " +
             " OrdersTbl.RoastDate, OrdersTbl.RequiredByDate, OrdersTbl.Confirmed, OrdersTbl.Notes, " + 
             "PersonsTbl.Abreviation " +
      "FROM  ((OrdersTbl INNER JOIN CustomersTbl ON OrdersTbl.CustomerId = CustomersTbl.CustomerID) INNER JOIN " +
              "PersonsTbl ON OrdersTbl.ToBeDeliveredBy = PersonsTbl.PersonID)" +
      "WHERE OrdersTbl.Done = ?";
    const string CONST_SQL_ORDERLINES =
      "SELECT ItemTypeTbl.ItemDesc, OrdersTbl.QuantityOrdered, OrdersTbl.PackagingID, OrdersTbl.PrepTypeID, " +
             " OrdersTbl.ItemTypeID, PackagingTbl.Description, PrepTypesTbl.PrepType " +
      "FROM (((OrdersTbl INNER JOIN ItemTypeTbl ON OrdersTbl.ItemTypeID = ItemTypeTbl.ItemTypeID) " +
            " LEFT OUTER JOIN PrepTypesTbl ON OrdersTbl.PrepTypeID = PrepTypesTbl.PrepID) LEFT OUTER JOIN " +
            " PackagingTbl ON OrdersTbl.PackagingID = PackagingTbl.PackagingID) " +
      "WHERE (OrdersTbl.CustomerId = ?) AND (OrdersTbl.RoastDate = ?)";
      
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)

      {

        List<QOnT.classes.OrderDetails> myOrderList = new List<OrderDetails>(); 
        
        string StrConString = System.Configuration.ConfigurationManager.ConnectionStrings[CONST_TRACKERDB_CONSTRING].ToString();

        OleDbConnection oleDbCon = new OleDbConnection(StrConString);
        OleDbCommand oleDbCmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();
        oleDbCon.Open();
        oleDbCmd.Parameters.Clear();
        oleDbCmd = new OleDbCommand(CONST_SQL_ORDERMAIN, oleDbCon);

        OleDbParameter NewParam = new OleDbParameter("@Done", false);
        NewParam.OleDbType = OleDbType.Boolean;   
        oleDbCmd.Parameters.Add(NewParam);

//        OleDbCommand command = new OleDbCommand(queryString, connection);
//        command.Parameters.Add("@p1", OleDbType.Char, 3).Value = "a";
        OleDbDataReader myReader = oleDbCmd.ExecuteReader();
        /// get records into class
        while (myReader.Read())
        {
          OrderDetails thisOrderDetail = new OrderDetails();
          thisOrderDetail.CustomerID = Convert.ToInt32( myReader["CustomerID"]);
          thisOrderDetail.CompanyName = myReader["CompanyName"].ToString();
          thisOrderDetail.OrderDate = Convert.ToDateTime(myReader["OrderDate"].ToString());
          thisOrderDetail.RoastDate = (DateTime)myReader["RoastDate"];
          thisOrderDetail.RequiredByDate= (DateTime)myReader["RequiredByDate"];
          thisOrderDetail.Confirmed = (bool)myReader["Confirmed"];
          thisOrderDetail.Notes = myReader["Notes"].ToString();
          thisOrderDetail.Abreviation = myReader["Abreviation"].ToString();

          myOrderList.Add(thisOrderDetail);
        }

        // fvOrdersMain.DataSource = oleDbCmd.ExecuteReader();
        fvOrdersMain.DataSource = myOrderList;
        fvOrdersMain.DataBind();
        oleDbCon.Close();
        // now the item lines

        OleDbConnection oleDbLinesCon = new OleDbConnection(StrConString);
        OleDbCommand oleDbLinesCmd = new OleDbCommand();
        OleDbDataAdapter daLine = new OleDbDataAdapter();
        oleDbLinesCon.Open();
        oleDbLinesCmd.Parameters.Clear();
        oleDbLinesCmd = new OleDbCommand(CONST_SQL_ORDERLINES, oleDbLinesCon);

        Label lblCustId = (Label)fvOrdersMain.FindControl("CustomerIDLabel");
        OleDbParameter NewParamL1 = new OleDbParameter("@CustomerId", lblCustId.Text);
        NewParamL1.OleDbType = OleDbType.Integer;
        oleDbLinesCmd.Parameters.Add(NewParamL1);
        Label lblRoastDate = (Label)fvOrdersMain.FindControl("RoastDateLabel");
        OleDbParameter NewParamL2 = new OleDbParameter("@RoastDate", Convert.ToDateTime(lblRoastDate.Text) );
        NewParamL2.OleDbType = OleDbType.Date;
        oleDbLinesCmd.Parameters.Add(NewParamL2);

        gvOrdersDetail.DataSource = oleDbLinesCmd.ExecuteReader();
        gvOrdersDetail.DataBind();
        oleDbCon.Close();
      }

    }

   }
}