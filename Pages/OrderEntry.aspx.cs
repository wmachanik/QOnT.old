using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;

namespace QOnT.Pages
{
  public partial class OrderEntry : System.Web.UI.Page
  {

    const int CONST_CUSTIDCOL = 2;
    const int CONST_ROASTDATECOL = 4;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void gvCurrent_SelectedIndexChanged(object sender, EventArgs e)
    {
      string _CustomerId = gvListOfOrders.SelectedRow.Cells[CONST_CUSTIDCOL].Text;
      string _PrepDate = gvListOfOrders.SelectedRow.Cells[CONST_ROASTDATECOL].Text;
       
      Response.Redirect(String.Format("~/Pages/OrderDetail.aspx?CustomerID={0}&PrepDate={1}", _CustomerId, _PrepDate));

      //string _strSQL = "SELECT OrdersTbl.OrderID, OrdersTbl.ItemTypeID, OrdersTbl.QuantityOrdered, ItemTypeTbl.ItemDesc, " +
      //                 "       OrdersTbl.Notes, PrepTypesTbl.PrepType " +
      //                 "FROM ((OrdersTbl LEFT OUTER JOIN ItemTypeTbl ON OrdersTbl.ItemTypeID = ItemTypeTbl.ItemTypeID) " +
      //                 "                 LEFT OUTER JOIN PrepTypesTbl ON OrdersTbl.PrepTypeID = PrepTypesTbl.PrepID) " +
      //                 "WHERE (OrdersTbl.CustomerId = ?) AND (OrdersTbl.RoastDate = ?)";

      //string _TrackerConnStr = ConfigurationManager.ConnectionStrings["Tracker08ConnectionString"].ConnectionString;
      //// use connection string and SQL srting to retrieve data, and assign to the GridView DataSource.
      //OleDbConnection _conn = new OleDbConnection(_TrackerConnStr);
      //OleDbDataAdapter _dataAdpter = new OleDbDataAdapter(_strSQL, _conn);
      //// add parameters
      //_dataAdpter.SelectCommand.Parameters.Clear();
      //_dataAdpter.SelectCommand.Parameters.AddWithValue("?", Convert.ToInt32(gvListOfOrders.SelectedRow.Cells[CONST_CUSTIDCOL].Text));
      //_dataAdpter.SelectCommand.Parameters.AddWithValue("?", Convert.ToDateTime(gvListOfOrders.SelectedRow.Cells[CONST_ROASTDATECOL].Text));
      //// Set values and variable for grid
      //DataSet _ds = new DataSet();
      //_conn.Open();
      //_dataAdpter.Fill(_ds);
      //_conn.Close();
      //gvOrderDetails.DataSource = _ds;
      //gvOrderDetails.DataBind();     
    }
    protected void gvOrderDetails_OnRowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
      //Retrieve the table from the session object.
      DataTable dt = (DataTable)Session["TaskTable"];

      ////Update the values.
      //GridViewRow row = TaskGridView.Rows[e.RowIndex];
      //dt.Rows[row.DataItemIndex]["Id"] = ((TextBox)(row.Cells[1].Controls[0])).Text;
      //dt.Rows[row.DataItemIndex]["Description"] = ((TextBox)(row.Cells[2].Controls[0])).Text;
      //dt.Rows[row.DataItemIndex]["IsComplete"] = ((CheckBox)(row.Cells[3].Controls[0])).Checked;

      ////Reset the edit index.
      //TaskGridView.EditIndex = -1;

      ////Bind data to the GridView control.
      //BindData();
    }

    protected void gvOrderDetails_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
      ////Set the edit index.
      //TaskGridView.EditIndex = e.NewEditIndex;
      ////Bind data to the GridView control.
      //BindData();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
      gvOrderDetails.DataBind();
    }

    protected void tbxSearchFor_TextChanged(object sender, EventArgs e)
    {
      if (ddlSearchFor.SelectedIndex == 1)
        ddlSearchFor.SelectedIndex = 2;
    }
  }
}