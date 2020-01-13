using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace QOnT.Pages
{

  public partial class DeliverySheet : System.Web.UI.Page
  {

    public const string CONST_SESSION_SHEETDATE = "DeliverySheetDate";
    public const string CONST_SESSION_DELIVERTBY = "DeliverySheetDeliveryBy";
    public const string CONST_SESSION_DDLSHEETDATE_SELECTED = "DeliverySheetDateItemSelected";
    public const string CONST_SESSION_DDLDELIVERTBY_SELECTED = "DeliverySheetDeliveryByItemSelected";
    public const string CONST_SESSION_SHEETISPRINTING = "SheetIsPrinting";

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
    const int CONST_NEEDDESCRIPTION_SORT_ORDER = 10;
    const string CONST_ZZNAME_PREFIX = "_*:";
    const string CONST_IS_ZZNAME = "ZZ";

    // delivery Details
    class deliveryItems
    {
      public deliveryItems()
      { }
      public string ContactID { get; set; }
      public string ContactCompany { get; set; }
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
        Session[CONST_SESSION_SHEETISPRINTING] = "Y";
      }
      else
      {
        this.MasterPageFile = "~/Site.master";
        Session[CONST_SESSION_SHEETISPRINTING] = "N";
      }
    }

    protected void PageInitialize(bool pPrintForm)
    {
      btnPrint.Visible = !pPrintForm;
      pnlDeliveryDate.Visible = !pPrintForm;
      ltrlWhichDate.Visible = pPrintForm;

      // set vars for the form
      string strDate = ((Request.QueryString["DateValue"] == null) ? "" : Request.QueryString["DateValue"]);
      string strDeliveryBy = ((Request.QueryString["DeliveryBy"] == null) ? "" : Request.QueryString["DeliveryBy"]);

      if (string.IsNullOrEmpty(strDate))
        strDate = (string)Session[CONST_SESSION_SHEETDATE];
      if (string.IsNullOrEmpty(strDeliveryBy))
        strDeliveryBy = (string)Session[CONST_SESSION_DELIVERTBY];

      if (!string.IsNullOrEmpty(strDate))
      {
        BuildDeliverySheet(pPrintForm, strDate, strDeliveryBy);
      }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      bool bPrintForm = (Request.QueryString["Print"] != null) ? (Request.QueryString["Print"].ToString() == "Y") : false;
      string _DeliveryDateSelectedValue = (Session[CONST_SESSION_DDLSHEETDATE_SELECTED] != null) ?  (string)Session[CONST_SESSION_DDLSHEETDATE_SELECTED]: "";
      string _DeliveryByValue = (Session[CONST_SESSION_DDLDELIVERTBY_SELECTED] != null) ? (string)Session[CONST_SESSION_DDLDELIVERTBY_SELECTED] : "";

      if (!IsPostBack)
      {
        Button _findButton = (Button)pnlDeliveryDate.FindControl("btnFind");

        if (_findButton != null)
          this.Form.DefaultButton = _findButton.UniqueID;

        PageInitialize(bPrintForm);
        // set the drop down value dependant on if there was a session variable saved, but rememeber that if we are returning to this page
        // the item may be gone.
        if (!string.IsNullOrEmpty(_DeliveryDateSelectedValue))
        {
          if (ddlActiveRoastDates.Items.FindByValue(_DeliveryDateSelectedValue) != null)
            ddlActiveRoastDates.SelectedValue = _DeliveryDateSelectedValue;
        }
        if (!string.IsNullOrEmpty(_DeliveryByValue))
          if (ddlDeliveryBy.Items.FindByValue(_DeliveryByValue) != null)
            ddlDeliveryBy.SelectedValue = _DeliveryByValue;
      }
      // if this in not being printed add the edit button
      if (!bPrintForm)
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

    protected OleDbConnection GetTrackerOLEConnection()
    {
      string _TrackerConnStr = ConfigurationManager.ConnectionStrings["Tracker08ConnectionString"].ConnectionString;
      OleDbConnection oleConn = new OleDbConnection(_TrackerConnStr);
      return oleConn;
    }

    protected void BuildDeliverySheet()
    {
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
      Session[CONST_SESSION_DELIVERTBY] = pOnlyDeliveryBy;

      string _strSQL = "SELECT DISTINCT OrdersTbl.OrderID, CustomersTbl.CompanyName AS CoName," +
                       " OrdersTbl.CustomerId, OrdersTbl.OrderDate, OrdersTbl.RoastDate, OrdersTbl.ItemTypeID, ItemTypeTbl.ItemDesc," +
                       " OrdersTbl.QuantityOrdered, ItemTypeTbl.ItemShortName, ItemTypeTbl.ItemEnabled, ItemTypeTbl.ReplacementID,  CityPrepDaysTbl.DeliveryOrder, " +
                       " ItemTypeTbl.SortOrder, OrdersTbl.RequiredByDate, OrdersTbl.ToBeDeliveredBy, OrdersTbl.Confirmed, OrdersTbl.Done, OrdersTbl.Notes," +
                       " PackagingTbl.Description AS PackDesc, PackagingTbl.BGColour, PersonsTbl.Abreviation" +
                       " FROM ((((CityPrepDaysTbl RIGHT OUTER JOIN CustomersTbl ON CityPrepDaysTbl.CityID = CustomersTbl.City) RIGHT OUTER JOIN " +
                       " (OrdersTbl LEFT OUTER JOIN PersonsTbl ON OrdersTbl.ToBeDeliveredBy = PersonsTbl.PersonID) ON CustomersTbl.CustomerID = OrdersTbl.CustomerId) LEFT OUTER JOIN " +
                       "  PackagingTbl ON OrdersTbl.PackagingID = PackagingTbl.PackagingID) LEFT OUTER JOIN ItemTypeTbl ON OrdersTbl.ItemTypeID = ItemTypeTbl.ItemTypeID)" +
                       " WHERE (OrdersTbl.RequiredByDate = #" + pActiveDeliveryDate + "#" + ")" + ((pOnlyDeliveryBy != "") ? " AND OrdersTbl.ToBeDeliveredBy=" + pOnlyDeliveryBy : "") +
                       " ORDER BY OrdersTbl.RequiredByDate, OrdersTbl.ToBeDeliveredBy, CityPrepDaysTbl.DeliveryOrder, CustomersTbl.CompanyName, ItemTypeTbl.SortOrder";
      // sort order needs to depend on day choosen, since at the moment if there are 2 different sort orders on a day even if the day of prep is different it duplicates the item

      OleDbConnection oleConn = GetTrackerOLEConnection();
      // open connection
      oleConn.Open();
      OleDbCommand oleCommand = new OleDbCommand(_strSQL, oleConn);

      OleDbDataReader oleData = oleCommand.ExecuteReader();

      BuildDeliveryTable(oleData, pPrintForm);
    }

    void BuildDeliveryTable(OleDbDataReader oleData, bool pPrintForm)
    {
      // delete the old data
      while (1 < tblDeliveries.Rows.Count)
      {
        tblDeliveries.Rows.RemoveAt(1);
      }
      tblTotals.Rows.Clear();

      //casting null check
      int iSortOrder;

      // string lists to store the company names and the items to be delivered to them
      List<deliveryItems> deliveryDetailsList = new List<DeliverySheet.deliveryItems>();
      // Delivery Persons list, we use a dictionary for easy of use
      SortedDictionary<string, string> ListOfDeliveryBy = new SortedDictionary<string, string>();
      // The Totals Table stuff
      string _strItemID = "";
      string _strItemDesc = "";
      string _strTotalItemDesc = "";
      bool _isUnknownClient = false;
      Dictionary<string, ItemTotals> sumItemTotals = new Dictionary<string, ItemTotals>();
      // how many items will be deliverred, this will be the counter
      int numDeliveryItems = 0;
      // read the data from the SQL query and add data to delivery list
      while (oleData.Read())
      {
        _isUnknownClient = false;
        // for each line read the data, and then if the compnay name has changed add that line to the table
        deliveryItems _tDeliveryItem = new deliveryItems();
        _tDeliveryItem.ContactID = oleData["CustomerId"].ToString();     // ID is used for link to ediot of customer
        _tDeliveryItem.ContactCompany = oleData["CoName"].ToString();
        // do some basic manipluation of names depending on what is selected, add notes.
        if (_tDeliveryItem.ContactCompany.StartsWith("ZZName"))
        { // add the ZZname but check if there is a ":" which is the end of name delimiter
          _tDeliveryItem.ContactID = CONST_IS_ZZNAME;
          _isUnknownClient = true;
          var _strNotes = oleData["Notes"].ToString();
          if (_strNotes.Contains(":"))
            _strNotes = _strNotes.Remove(_strNotes.IndexOf(":")).Trim();

          _tDeliveryItem.ContactCompany = CONST_ZZNAME_PREFIX + " " + _strNotes;
        }
        else if (_tDeliveryItem.ContactCompany.StartsWith("Stock"))
          _tDeliveryItem.ContactCompany = "STK: " + oleData["Notes"].ToString();
        // if the notes have a "+" to start with append that to the name
        if (oleData["Notes"].ToString().StartsWith("+"))
          _tDeliveryItem.ContactCompany += "[" + oleData["Notes"].ToString() + "]";

        // Check if the deliver is done, and mark it so
        if (Boolean.Parse(oleData["Done"].ToString()))
          _tDeliveryItem.ContactCompany = "<b>DONE</b>-> " + _tDeliveryItem.ContactCompany;

        // get delivery person, and store them for the ddl only if not printing
        if (!pPrintForm)
        {
          if (!ListOfDeliveryBy.ContainsKey(oleData["ToBeDeliveredBy"].ToString()))
            ListOfDeliveryBy[oleData["ToBeDeliveredBy"].ToString()] = oleData["Abreviation"].ToString();
          /// add url
          _tDeliveryItem.OrderDetailURL = ResolveUrl("~/Pages/OrderDetail.aspx") + "?" +
                                          String.Format("CustomerID={0}&DeliveryDate={1:d}&Notes={2}",
                                                         HttpContext.Current.Server.UrlEncode(oleData["CustomerID"].ToString()), oleData["RequiredByDate"],
                                                         HttpContext.Current.Server.UrlEncode(oleData["Notes"].ToString()));
        }

        // add delviery details
        _tDeliveryItem.Details = String.Format("{0:d}, {1}", oleData["RequiredByDate"], oleData["Abreviation"]);

        // get the item Description, may need to do some replaceme3nt calculation here
        _strItemID = oleData["ItemTypeID"].ToString();
        _strItemDesc = ((oleData["ItemShortName"].ToString().Length > 0) ? oleData["ItemShortName"].ToString() : oleData["ItemDesc"].ToString());
        _strTotalItemDesc = _strItemDesc;
        // check to see if item available if not added error colours
        if (!Boolean.Parse(oleData["ItemEnabled"].ToString()))
        {
          _strItemDesc = "<span style='background-color: RED; color: WHITE'>SOLD OUT</span> " + _strItemDesc;
          _strTotalItemDesc = ">" + _strTotalItemDesc + "<";
        }
        // now add description if required
        iSortOrder = (oleData["SortOrder"] == DBNull.Value) ? 0 : (int)oleData["SortOrder"];
        if (iSortOrder == CONST_NEEDDESCRIPTION_SORT_ORDER)
        {
          // if we are already use the notes field for name, check if there is a ":" seperator, and then only use what is after
          var _strNotes = oleData["Notes"].ToString();
          if ((_isUnknownClient) && (_strNotes.Contains(":")))
            _strNotes = _strNotes.Substring(_strNotes.IndexOf(":") + 1).Trim();

          _strItemDesc += ": " + _strNotes;
        }
        if (oleData["PackDesc"].ToString().Length > 0)
          _tDeliveryItem.Items = String.Format("<span style='background-color:{0}; padding-top: 1px; padding-bottom:2px'>{1}X{2} ({3})</span>", oleData["BGColour"], oleData["QuantityOrdered"],
            _strItemDesc, oleData["PackDesc"]);
        else
          _tDeliveryItem.Items = String.Format("<span style='background-color:{0}'>{1}X{2}</span>", oleData["BGColour"], oleData["QuantityOrdered"], _strItemDesc);
        // if Item needs to be added and ID exists the increment the total, otherwise add new item
        if (iSortOrder != CONST_NEEDDESCRIPTION_SORT_ORDER)
        {
          if (sumItemTotals.ContainsKey(_strItemID))
          {
            sumItemTotals[_strItemID].TotalsQty += Convert.ToDouble(oleData["QuantityOrdered"]);
          }
          else
          {
            // Remove notes
            if (_strItemDesc.Contains(":"))
              _strItemDesc = _strItemDesc.Remove(_strItemDesc.IndexOf(":"));

            sumItemTotals[_strItemID] = new ItemTotals
            {
              ItemID = _strItemID,
              ItemDesc = _strTotalItemDesc,
              TotalsQty = Convert.ToDouble(oleData["QuantityOrdered"].ToString()),
              ItemOrder = (oleData["SortOrder"] == DBNull.Value) ? 0 : Convert.ToInt32(oleData["SortOrder"].ToString())
            };
          }
        }
        // add to list of deliveries
        deliveryDetailsList.Add(_tDeliveryItem);

        // increment deliveries now
        numDeliveryItems++;

      }
      oleData.Close();

      // Do a bubble sort on the ZZnames, so that they are all together
      for (int idx1 = 0; idx1 < numDeliveryItems; idx1++)
      {
        if (deliveryDetailsList[idx1].ContactCompany.StartsWith(CONST_ZZNAME_PREFIX))
          for (int idx2 = idx1 + 2; idx2 < numDeliveryItems; idx2++)
          {
            if (deliveryDetailsList[idx2].ContactCompany.Equals(deliveryDetailsList[idx1].ContactCompany))
            {
              // this equals the last one so move it to just below the last one
              deliveryItems thisItem = deliveryDetailsList[idx2];
              deliveryDetailsList.RemoveAt(idx2);
              deliveryDetailsList.Insert(idx1 + 1, thisItem);
            }
          }
      }

      // sort the deliveries to make sure that we have all the company names one after the other
      int _items = 0;
      string _strURL = "";

      while (_items < numDeliveryItems)
      {
        TableRow _tblRow = new TableRow();

        // now add details
        // add delivery and date
        TableCell _tblCellBy = new TableCell();
        _tblCellBy.Text = deliveryDetailsList[_items].Details;
        if (pPrintForm)
        {
          _tblCellBy.Font.Size = FontUnit.XSmall;
          _tblCellBy.Text = _tblCellBy.Text.Remove(0, _tblCellBy.Text.IndexOf(",") + 1);
        }
        else  // add a hyoerlink to edit
          _tblCellBy.Text = String.Format("<a class='plain' href='{0}'>{1}</a>", deliveryDetailsList[_items].OrderDetailURL, _tblCellBy.Text.Trim());

        _tblRow.Cells.Add(_tblCellBy);
        // add company name and a link to the customer edit form.
        TableCell _tblCell = new TableCell();
        if ((pPrintForm) || (deliveryDetailsList[_items].ContactID == CONST_IS_ZZNAME))
          _tblCell.Text = deliveryDetailsList[_items].ContactCompany;
        else
          _tblCell.Text = String.Format("<a href='./CustomerDetails.aspx?ID={0}&'>{1}</a>", deliveryDetailsList[_items].ContactID, deliveryDetailsList[_items].ContactCompany);
        _tblRow.Cells.Add(_tblCell);
        // Add received and signed by
        _tblRow.Cells.Add(new TableCell());
        _tblRow.Cells.Add(new TableCell());
        // Now the items
        TableCell _tblCellItems = new TableCell();
        _strURL = String.Format("<a href='{0}'>Edit</a>", deliveryDetailsList[_items].OrderDetailURL);
        do
        {
          if (_tblCellItems.Text.Length == 0)
            _tblCellItems.Text = deliveryDetailsList[_items].Items.ToString();
          else
            _tblCellItems.Text = _tblCellItems.Text + "; " + deliveryDetailsList[_items].Items.ToString();

          _items++;
        } while ((_items < numDeliveryItems) && (deliveryDetailsList[_items - 1].ContactCompany == deliveryDetailsList[_items].ContactCompany));
        // add items
        _tblRow.Cells.Add(_tblCellItems);

        // Add in stock
        _tblRow.Cells.Add(new TableCell());
        // now add edit url
        if (!pPrintForm)
          _tblRow.Cells.Add(new TableCell { Text = _strURL });

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
        _tblCellItemTotal.Text = String.Format("{0:0.00}", _pair.Value.TotalsQty);
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
      upnlDeliveryItems.Update();

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
      if ((ddlActiveRoastDates != null) && (ddlActiveRoastDates.SelectedIndex > 0)) 
      {
        Session[CONST_SESSION_SHEETDATE] = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ddlActiveRoastDates.SelectedValue));
        //        string strDeliveryBy = Session[CONST_SESSION_DELIVERTBY].ToString();
        Response.Redirect("~/Pages/DeliverySheet.aspx?Print=Y");    // &DateValue=" + strDate + "&DeliveryBy=" + strDeliveryBy);
      }
    }

    protected void ddlActiveRoastDates_SelectedIndexChanged(object sender, EventArgs e)
    {
      Session[CONST_SESSION_DDLSHEETDATE_SELECTED] = ddlActiveRoastDates.SelectedValue;
      ltrlWhichDate.Text = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ddlActiveRoastDates.SelectedValue));
      Session[CONST_SESSION_SHEETDATE] = ltrlWhichDate.Text;
      ltrlWhichDate.Visible = true;
      if (!string.IsNullOrEmpty(ltrlWhichDate.Text))
      {
        Button _GoButton = (Button)pnlDeliveryDate.FindControl("btnGo");

        if (_GoButton != null)
          this.Form.DefaultButton = _GoButton.UniqueID;
      }

    }

    protected void ddlDeliveryBy_SelectedIndexChanged(object sender, EventArgs e)
    {
      Session[CONST_SESSION_DDLDELIVERTBY_SELECTED] = ddlDeliveryBy.SelectedValue;
      BuildDeliverySheet();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
      // refresh the data in the drop down list
      //odsActiveRoastDates.DataBind();
      //ddlActiveRoastDates.Items.Clear();
      //ddlActiveRoastDates.DataBind();
      //      ddlActiveRoastDates.SelectedIndex = 0;
      Session[CONST_SESSION_DELIVERTBY] = "";
      Session[CONST_SESSION_SHEETDATE] = "";
      Response.Redirect("DeliverySheet.aspx");
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
      if ((ddlActiveRoastDates != null) && (ddlActiveRoastDates.SelectedIndex > 0))
      {
        ltrlWhichDate.Text = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ddlActiveRoastDates.SelectedValue));
        Session[CONST_SESSION_SHEETDATE] = ltrlWhichDate.Text;
        BuildDeliverySheet();
      }
    }

    protected void btnFind_Click(object sender, EventArgs e)
    {

      string _strSQL = "SELECT DISTINCT OrdersTbl.OrderID, CustomersTbl.CompanyName AS CoName," +
                       " OrdersTbl.CustomerId, OrdersTbl.OrderDate, OrdersTbl.RoastDate, OrdersTbl.ItemTypeID, ItemTypeTbl.ItemDesc," +
                       " OrdersTbl.QuantityOrdered, ItemTypeTbl.ItemShortName, ItemTypeTbl.ItemEnabled, ItemTypeTbl.ReplacementID,  CityPrepDaysTbl.DeliveryOrder, " +
                       " ItemTypeTbl.SortOrder, OrdersTbl.RequiredByDate, OrdersTbl.ToBeDeliveredBy, OrdersTbl.Confirmed, OrdersTbl.Done, OrdersTbl.Notes," +
                       " PackagingTbl.Description AS PackDesc, PackagingTbl.BGColour, PersonsTbl.Abreviation" +
                       " FROM ((((CityPrepDaysTbl RIGHT OUTER JOIN CustomersTbl ON CityPrepDaysTbl.CityID = CustomersTbl.City) RIGHT OUTER JOIN " +
                       " (OrdersTbl LEFT OUTER JOIN PersonsTbl ON OrdersTbl.ToBeDeliveredBy = PersonsTbl.PersonID) ON CustomersTbl.CustomerID = OrdersTbl.CustomerId) LEFT OUTER JOIN " +
                       "  PackagingTbl ON OrdersTbl.PackagingID = PackagingTbl.PackagingID) LEFT OUTER JOIN ItemTypeTbl ON OrdersTbl.ItemTypeID = ItemTypeTbl.ItemTypeID)" +
                       " WHERE (CustomersTbl.CompanyName LIKE '%" + tbxFindClient.Text + "%') AND (OrdersTbl.Done = false)" +
                       " ORDER BY OrdersTbl.RequiredByDate, OrdersTbl.ToBeDeliveredBy, CityPrepDaysTbl.DeliveryOrder, CustomersTbl.CompanyName, ItemTypeTbl.SortOrder";
      // sort order needs to depend on day choosen, since at the moment if there are 2 different sort orders on a day even if the day of prep is different it duplicates the item
      OleDbConnection oleConn = GetTrackerOLEConnection();
      // open connection
      oleConn.Open();
      OleDbCommand oleCommand = new OleDbCommand(_strSQL, oleConn);
      OleDbDataReader oleData = oleCommand.ExecuteReader();

      // now build the standard table
      BuildDeliveryTable(oleData, false);

    }

    protected void tbxFindClient_OnTextChanged(object sender, EventArgs e)
    {
      Button _findButton = (Button)pnlDeliveryDate.FindControl("btnFind");

      if (_findButton != null)
        this.Form.DefaultButton = _findButton.UniqueID;

      btnFind_Click(sender, e);
    }
  }
}