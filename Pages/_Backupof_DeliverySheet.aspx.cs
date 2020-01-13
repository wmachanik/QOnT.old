using System;
using System.Configuration;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using QOnT.classes;
using System.Collections;
using System.Linq;

namespace QOnT.Pages
{

  public partial class DeliverySheet : System.Web.UI.Page
  {
    // global declarations
    //const string GET_ORDER_ITEM_NAMES = "SELECT DISTINCT ItemTypeTbl.ItemDesc, ItemTypeTbl.ItemShortName, ItemTypeTbl.SortOrder " +
    //                              "FROM (OrdersTbl LEFT OUTER JOINItemTypeTbl ON OrdersTbl.ItemTypeID = ItemTypeTbl.ItemTypeID) " +
    //                              " WHERE (OrdersTbl.Done = 0) ORDER BY ItemTypeTbl.SortOrder";
    //const string GET_ACTIVE_ORDER = "SELECT OrdersTbl.OrderID, CustomersTbl.CompanyName, OrdersTbl.OrderDate, OrdersTbl.RoastDate, " +
    //                     "ItemTypeTbl.ItemDesc, OrdersTbl.QuantityOrdered, ItemTypeTbl.ItemShortName, ItemTypeTbl.ItemEnabled, " +
    //                     "ItemTypeTbl.ReplacementID, ItemTypeTbl.SortOrder, OrdersTbl.RequiredByDate, OrdersTbl.ToBeDeliveredBy, OrdersTbl.Confirmed," +
    //                     "OrdersTbl.Done, OrdersTbl.Notes, PackagingTbl.Description, PackagingTbl.BGColour, PersonsTbl.Abreviation" +
    //                     "FROM ((((OrdersTbl LEFT OUTER JOIN PersonsTbl ON OrdersTbl.ToBeDeliveredBy = PersonsTbl.PersonID) LEFT OUTER JOIN" +
    //                     "      CustomersTbl ON OrdersTbl.CustomerId = CustomersTbl.CustomerID) LEFT OUTER JOIN " +
    //                     "      PackagingTbl ON OrdersTbl.PackagingID = PackagingTbl.PackagingID) LEFT OUTER JOIN " +
    //                     "      ItemTypeTbl ON OrdersTbl.ItemTypeID = ItemTypeTbl.ItemTypeID) " +
    //                     " WHERE OrdersTbl.RequiredByDate = ? ORDER BY ItemTypeTbl.SortOrder";
    
    // Constants
    const int CONST_NEEDDESCRIPTION_SORT_ORDER = 7;
    
    // delivery Details
    class deliveryDetails
    {
      public string Companies { get; set; }
      public string Details { get; set; }
      public string Items { get; set; }
      public string OrderDetailURL { get; set; }
    }


    // class used for creating Item Totals table at the bottom of the form
    class ItemTotals
    {
      public string ItemID { get; set; }
      public string ItemDesc { get; set; }
      public double TotalsQty { get; set; }
      public int ItemOrder { get; set; }
    }


  private void Page_PreInit(object sender, EventArgs e)
  {
    bool bPrintForm = false;

    if (Request.QueryString["Print"] != null)
      bPrintForm = (Request.QueryString["Print"].ToString() == "Y");

    if (bPrintForm)
    {
      this.MasterPageFile = "~/Print.master";
      Session["SheetIsPrinting"] = "Y";
    }
    else
    {
      this.MasterPageFile = "~/Site.master";
      Session["SheetIsPrinting"] = "N";
    }
  }

  protected void Page_Load(object sender, EventArgs e)
  {
    bool bPrintForm = false;

    //for the first time initialize the session variable
    if (!IsPostBack)
      Session["DeliverySheetDeliveryBy"] = "";

    if (Request.QueryString["Print"] != null)
      bPrintForm = (Request.QueryString["Print"].ToString() == "Y");

    btnPrint.Visible = !bPrintForm;
    pnlDeliveryDate.Visible = !bPrintForm;
    ltrlWhichDate.Visible = bPrintForm;

    if (bPrintForm)
    {
      // set values from aparameters past
      string strDate = ((Request.QueryString["DateValue"] == null) ? "2001/01/01" : Request.QueryString["DateValue"]);
      string strDeliveryBy = ((Request.QueryString["DeliveryBy"] == null) ? "" : Request.QueryString["DeliveryBy"]);
      BuildDeliverySheet(true, strDate, strDeliveryBy);
    }
    else
    {
      tblDeliveries.Rows[0].Cells.Add(new TableHeaderCell { Text = "Edit" });
    }

  }
    /// <summary>
    /// Build the deliver sheet using the select date
    /// </summary>
#region LogicOfBuildDeliverySheet
    /// Logic of the building of the delivery sheet7
    /// A. First create the tabbed panel
    /// 1. Select agents with open orders
    /// 2. Create enough tabs for each agent and date
    /// B. For each agent and date
    /// 1. Select all the active items on order do for the agent and date
    /// 2. Create a table header using distinct items by sort order
    /// C. Populate the tabs
    /// 1. Select all open deliveries for the roast date specified (later agent)
    /// 2. Create table of the data using header arrays created in B
    /// 3. Populate the table with the data:
    /// 3a. Check for packaging information and highlight as per the data
    /// 3b. Group by client
    /// 3c. Totals
    /// 4. Add printing support per table and summary
#endregion

    protected void BuildDeliverySheet() {
      // the ltrl is set when the ddl of delivery dates is changed, it is formatted yyyy-MM-dd
      // have they selected a deliver person to filter by?
      string _ActiveDeliveryDate = ((ltrlWhichDate.Text.Length > 0) ? ltrlWhichDate.Text : "2012-01-01");
      BuildDeliverySheet(false,
                         ((ltrlWhichDate.Text.Length > 0) ? ltrlWhichDate.Text : "2012-01-01"),
                         (((ddlDeliveryBy.Items.Count > 1) && (ddlDeliveryBy.SelectedIndex > 0)) ? ddlDeliveryBy.SelectedValue : "")); 
         
    }
    protected void BuildDeliverySheet(bool pPrintForm, string pActiveDeliveryDate, string pOnlyDeliveryBy)
    {
      // set the session variable for DeliveryBy, for later
      Session["DeliverySheetDeliveryBy"] = pOnlyDeliveryBy;
        
      // construct the string with the where clause as per above parameters.
      //string _strSQL = "SELECT OrdersTbl.OrderID, CustomersTbl.CompanyName, OrdersTbl.OrderDate, OrdersTbl.RoastDate, OrdersTbl.ItemTypeID, " +
      //                 "ItemTypeTbl.ItemDesc, OrdersTbl.QuantityOrdered, ItemTypeTbl.ItemShortName, ItemTypeTbl.ItemEnabled, " +
      //                 "ItemTypeTbl.ReplacementID, ItemTypeTbl.SortOrder, OrdersTbl.RequiredByDate, OrdersTbl.ToBeDeliveredBy, OrdersTbl.Confirmed," +
      //                 "OrdersTbl.Done, OrdersTbl.Notes, PackagingTbl.Description AS PackDesc, PackagingTbl.BGColour, PersonsTbl.Abreviation " +
      //                 "FROM ((((OrdersTbl LEFT OUTER JOIN PersonsTbl ON OrdersTbl.ToBeDeliveredBy = PersonsTbl.PersonID) LEFT OUTER JOIN" +
      //                 "      CustomersTbl ON OrdersTbl.CustomerId = CustomersTbl.CustomerID) LEFT OUTER JOIN " +
      //                 "      PackagingTbl ON OrdersTbl.PackagingID = PackagingTbl.PackagingID) LEFT OUTER JOIN " +
      //                 "      ItemTypeTbl ON OrdersTbl.ItemTypeID = ItemTypeTbl.ItemTypeID) " +
      //                 " WHERE OrdersTbl.RequiredByDate = #" + pActiveDeliveryDate + "#" + ((pOnlyDeliveryBy != "") ? " AND OrdersTbl.ToBeDeliveredBy=" + pOnlyDeliveryBy : "") +
      //                 " ORDER BY OrdersTbl.RequiredByDate, " +" OrdersTbl.ToBeDeliveredBy, CustomersTbl.CompanyName, ItemTypeTbl.SortOrder"; // + _ActiveRoastDate ;

      // IIf([CompanyName]="ZZName________","Z:" & [OrdersTbl].[Notes],[CompanyName])

      string _strSQL = "SELECT OrdersTbl.OrderID, CustomersTbl.CompanyName AS CoName," +
                       " OrdersTbl.CustomerId, OrdersTbl.OrderDate, OrdersTbl.RoastDate, OrdersTbl.ItemTypeID, ItemTypeTbl.ItemDesc," +
                       " OrdersTbl.QuantityOrdered, ItemTypeTbl.ItemShortName, ItemTypeTbl.ItemEnabled, ItemTypeTbl.ReplacementID, CityPrepDaysTbl.DeliveryOrder, " +
                       " ItemTypeTbl.SortOrder, OrdersTbl.RequiredByDate, OrdersTbl.ToBeDeliveredBy, OrdersTbl.Confirmed, OrdersTbl.Done, OrdersTbl.Notes," +
                       " PackagingTbl.Description AS PackDesc, PackagingTbl.BGColour, PersonsTbl.Abreviation" +
                       " FROM ((((CityPrepDaysTbl RIGHT OUTER JOIN CustomersTbl ON CityPrepDaysTbl.CityID = CustomersTbl.City) RIGHT OUTER JOIN "+
                       " (OrdersTbl LEFT OUTER JOIN PersonsTbl ON OrdersTbl.ToBeDeliveredBy = PersonsTbl.PersonID) ON CustomersTbl.CustomerID = OrdersTbl.CustomerId) LEFT OUTER JOIN "+
                       "  PackagingTbl ON OrdersTbl.PackagingID = PackagingTbl.PackagingID) LEFT OUTER JOIN ItemTypeTbl ON OrdersTbl.ItemTypeID = ItemTypeTbl.ItemTypeID)" +
                       " WHERE (OrdersTbl.RequiredByDate = #" + pActiveDeliveryDate + "#" +")" + ((pOnlyDeliveryBy != "") ? " AND OrdersTbl.ToBeDeliveredBy=" + pOnlyDeliveryBy : "") +
                       " ORDER BY OrdersTbl.RequiredByDate, OrdersTbl.ToBeDeliveredBy, CityPrepDaysTbl.DeliveryOrder, CustomersTbl.CompanyName, ItemTypeTbl.SortOrder";

      string _TrackerConnStr = ConfigurationManager.ConnectionStrings["Tracker08ConnectionString"].ConnectionString;
      OleDbConnection oleConn = new OleDbConnection(_TrackerConnStr);
      // open connection
      oleConn.Open();
      OleDbCommand oleCommand = new OleDbCommand(_strSQL,oleConn);

      OleDbDataReader oleData = oleCommand.ExecuteReader();

      // string lists to store the company names and the items to be delivered to them
      List <string> deliveryCompanies = new List<string>();
      List <string> deliveryDetails = new List<string>();
      List <string> deliveryItems = new List<string>();
      // Delivery Persons list, we use a dictionary for easy of use
      SortedDictionary<string,string> ListOfDeliveryBy = new SortedDictionary<string,string>();
      // The Totals Table stuff
      string _strItemID = "";
      string _strItemDesc = "";
      Dictionary<string, ItemTotals> sumItemTotals = new Dictionary<string, ItemTotals>();
      // how many items will be deliverred, this will be the counter
      int numDeliveryItems = 0;

      while (oleData.Read())
      {
        // for each line read the data, and then if the compnay name has changed add that line to the table
        deliveryCompanies.Add(oleData["CoName"].ToString());
        // do some basic manipluation of names depending on what is selected, add notes.
        if (deliveryCompanies[numDeliveryItems].StartsWith("ZZName"))
          deliveryCompanies[numDeliveryItems] = "?: " + oleData["Notes"].ToString();
        else if (deliveryCompanies[numDeliveryItems].StartsWith("Stock"))
          deliveryCompanies[numDeliveryItems] = "STK: " + oleData["Notes"].ToString();
        // if the notes have a "+" to start with append that to the name
        if (oleData["Notes"].ToString().StartsWith("+"))
          deliveryCompanies[numDeliveryItems] += "[" + oleData["Notes"].ToString() + "]";
        
        // Check if the deliver is done, and mark it so
        if (Boolean.Parse(oleData["Done"].ToString()))
          deliveryCompanies[numDeliveryItems] = "<b>DONE</b>-> "+deliveryCompanies[numDeliveryItems] ;

        // get delivery person, and store them for the ddl only if not printing
        if (!pPrintForm)
        {
          if (!ListOfDeliveryBy.ContainsKey(oleData["ToBeDeliveredBy"].ToString()))
            ListOfDeliveryBy[oleData["ToBeDeliveredBy"].ToString()] = oleData["Abreviation"].ToString();
        }
        
        // add delviery details
        deliveryDetails.Add(String.Format("{0:d}, {1}", oleData["RequiredByDate"],oleData["Abreviation"]));
        
        // get the item Description, may need to do some replaceme3nt calculation here
        _strItemID = oleData["ItemTypeID"].ToString();
        _strItemDesc = ((oleData["ItemShortName"].ToString().Length > 0) ? oleData["ItemShortName"].ToString() : oleData["ItemDesc"].ToString());
        // check to see if item available if not added error colours
        if (!Boolean.Parse(oleData["ItemEnabled"].ToString()))
          _strItemDesc = "<span style='background-color: RED; color: WHITE'>SOLD OUT</span> " + _strItemDesc;
        // now add description if required
        if ((int)oleData["SortOrder"] == CONST_NEEDDESCRIPTION_SORT_ORDER)
          _strItemDesc += ": " + oleData["Notes"].ToString();

        if (oleData["PackDesc"].ToString().Length > 0)
          deliveryItems.Add(String.Format("<span style='background-color:{0}; padding-top: 1px; padding-bottom:2px'>{1}X{2} ({3})</span>", oleData["BGColour"], oleData["QuantityOrdered"],
            _strItemDesc, oleData["PackDesc"]));
        else
          deliveryItems.Add(String.Format("<span style='background-color:{0}'>{1}X{2}</span>", oleData["BGColour"], oleData["QuantityOrdered"], _strItemDesc));
        // if Item ID exists the increment the total, otherwise add new item
        if (sumItemTotals.ContainsKey(_strItemID))
        {
          sumItemTotals[_strItemID].TotalsQty += Convert.ToDouble(oleData["QuantityOrdered"]);
        }
        else
        {
          // Remove notes
          if (_strItemDesc.Contains(":"))
            _strItemDesc = _strItemDesc.Remove(_strItemDesc.IndexOf(":"));

          //ItemTotals _ItemTotal = new ItemTotals();
          //_ItemTotal.ItemDesc = oleData["ItemShortName"].ToString();
          //_ItemTotal.TotalsQty = Convert.ToDouble(oleData["QuantityOrdered"]);
          //sumItemTotals[_strItemTotalID] = _ItemTotal;
          sumItemTotals[_strItemID] = new ItemTotals
          {
            ItemID = _strItemID,
            ItemDesc = _strItemDesc,
            TotalsQty = Convert.ToDouble(oleData["QuantityOrdered"].ToString()),
            ItemOrder = Convert.ToInt32(oleData["SortOrder"].ToString())
          };
          }

        // increment deliveries now
        numDeliveryItems++;

      }
      oleData.Close();
      
      int _items = 0;
      
      while (_items < numDeliveryItems)
      {
        TableRow _tblRow = new TableRow();

        // now add details
        // add delivery and date
        TableCell _tblCellBy = new TableCell();
        _tblCellBy.Text = deliveryDetails[_items].ToString();
        if (pPrintForm)
        {
          _tblCellBy.Font.Size = FontUnit.XSmall;
          _tblCellBy.Text = _tblCellBy.Text.Remove(0, _tblCellBy.Text.IndexOf(",") + 1);
        }
        _tblRow.Cells.Add(_tblCellBy);
        // add company name
        TableCell _tblCell = new TableCell();
        _tblCell.Text = deliveryCompanies[_items].ToString();
        _tblRow.Cells.Add(_tblCell);
        // Add received and signed by
        _tblRow.Cells.Add(new TableCell());
        _tblRow.Cells.Add(new TableCell());
        // Now the items
        TableCell _tblCellItems = new TableCell();
        do 
        {
          if (_tblCellItems.Text.Length==0)
            _tblCellItems.Text = deliveryItems[_items].ToString();
          else
            _tblCellItems.Text = _tblCellItems.Text + "; " + deliveryItems[_items].ToString();

          _items ++;
         } while ((_items < numDeliveryItems) && (deliveryCompanies[_items-1] ==  deliveryCompanies[_items]));
        _tblRow.Cells.Add(_tblCellItems);
        // Add in stock
        _tblRow.Cells.Add(new TableCell());
        
        tblDeliveries.Rows.Add(_tblRow);

      }    // while we have delivery items

      //Display the totals table now
      Dictionary<string, ItemTotals> sortedItemTotals = (from entry in sumItemTotals orderby entry.Value.ItemOrder ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
      // now do the totals table
      TableRow _tblItemsNameRow = new TableHeaderRow();
      TableRow _tblItemsTotalRow = new TableRow();

      // add headers
      TableHeaderCell _tblItemHdr1 = new TableHeaderCell();
      _tblItemHdr1.Text = "Item";
      _tblItemHdr1.Font.Bold = true;
      _tblItemsNameRow.Cells.Add(_tblItemHdr1);
      TableCell _tblItemHdr2 = new TableCell();
      _tblItemHdr2.Text = "Total";
      _tblItemHdr2.Font.Bold = true;
      _tblItemsTotalRow.Cells.Add(_tblItemHdr2);

      // for each item in the totals table add a column and value
      foreach (KeyValuePair<string, ItemTotals> _pair in sortedItemTotals)
      {
//        _tblItemsNameRow.Cells.Add(new TableCell());
        // add the description
        TableHeaderCell _tblCellItemDesc = new TableHeaderCell();
        _tblCellItemDesc.Text = _pair.Value.ItemDesc;
        _tblCellItemDesc.Font.Bold = true;
        _tblItemsNameRow.Cells.Add(_tblCellItemDesc);
        // add the total
        TableCell _tblCellItemTotal = new TableCell();
        _tblCellItemTotal.Text = String.Format("{0:0.00}",_pair.Value.TotalsQty);
        _tblCellItemTotal.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
        _tblItemsTotalRow.Cells.Add(_tblCellItemTotal);
      }
      //now add Theme total rows.
      tblTotals.Rows.Add(_tblItemsNameRow);
      tblTotals.Rows.Add(_tblItemsTotalRow);
      if (pPrintForm)
        tblTotals.CssClass = tblTotals.CssClass + " small";
      else
      {
        // now add the drop down list items for delivery person
        bool bShowDeilveryBy = ListOfDeliveryBy.Count > 1;
        ddlDeliveryBy.Items.Clear();
        ddlDeliveryBy.Visible = bShowDeilveryBy;
        lblDeliveryBy.Visible = bShowDeilveryBy;

        if (bShowDeilveryBy)
        {
          ddlDeliveryBy.Items.Add(new ListItem { Text = "--- All ---", Value = "%", Selected = true });
          foreach (KeyValuePair<string, string> _deliveryByPair in ListOfDeliveryBy)
            ddlDeliveryBy.Items.Add(new ListItem { Text = _deliveryByPair.Value, Value = _deliveryByPair.Key });
        }
      }

    }

    protected void btnCreateSheet_Click(object sender, EventArgs e)
    {
      BuildDeliverySheet();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
      if (ddlActiveRoastDates != null)
      {
        string strDate = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ddlActiveRoastDates.SelectedValue));
        string strDeliveryBy = Session["DeliverySheetDeliveryBy"].ToString();
        Response.Redirect("~/Pages/DeliverySheet.aspx?Print=Y&DateValue=" + strDate + "&DeliveryBy=" + strDeliveryBy);
      }
    }

    protected void ddlActiveRoastDates_SelectedIndexChanged(object sender, EventArgs e)
    {
      ltrlWhichDate.Text = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ddlActiveRoastDates.SelectedValue));
      ltrlWhichDate.Visible = true;
    }

    protected void ddlDeliveryBy_SelectedIndexChanged(object sender, EventArgs e)
    {
      BuildDeliverySheet();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
      // refresh the data in the drop down list
      odsActiveRoastDates.DataBind();
      ddlActiveRoastDates.Items.Clear();
      ddlActiveRoastDates.DataBind();
      ddlActiveRoastDates.SelectedIndex = 1;
    }
  }
}