using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System;
using System.Web.Security;
using QOnT.classes;
using QOnT.control;

namespace QOnT.Pages
{

  public partial class NewOrderDetail : System.Web.UI.Page
  {
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_DELIVERY_DEFAULT = "SS";
    const string CONST_ZZNAME_DEFAULTID = "9";
    const string CONST_UPDATELINES = "UpdateOrderLines";
    const string CONST_LINESADDED = "OrderLinesAdded";
    const string CONST_ORDERLINEIDS = "OrderLineIDS";
    const int CONST_ORDERIDCOL = 4;
    //  URL request strings
    public const string CONST_URL_REQUEST_CUSTOMERID = "CoID";
    public const string CONST_URL_REQUEST_NAME = "Name";
    public const string CONST_URL_REQUEST_COMPANYNAME = "CoName";
    public const string CONST_URL_REQUEST_LASTORDER = "LastOrder";
    public const string CONST_URL_REQUEST_SKU1 = "SKU1";

    private OleDbConnection OpenTrackerOleDBConnection()
    {
      OleDbConnection pConn = null;
      string _connectionString;

      if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
          ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
      {
        throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
                            "must exist in the <connectionStrings> configuration section for the application.");
      }
      _connectionString = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;
      pConn = new OleDbConnection(_connectionString);

      return pConn;
    }
    protected void SetContactByID(string pCoNameID)
    {
      // Set the Contact Name to string passed
      int i = 0;
      // force null string 
      if (!string.IsNullOrEmpty(pCoNameID))
      {
        // now find it
        while ((i < ddlContacts.Items.Count) && (!pCoNameID.Equals(ddlContacts.Items[i].Value, StringComparison.OrdinalIgnoreCase)))
          i++;

        if ((i < ddlContacts.Items.Count) && (pCoNameID.Equals(ddlContacts.Items[i].Value, StringComparison.OrdinalIgnoreCase)))
          ddlContacts.SelectedValue = ddlContacts.Items[i].Value;
        else
        { // did not find company to set it to generic
            ddlContacts.SelectedValue = CONST_ZZNAME_DEFAULTID;
            tbxNotes.Text += "ID note found: " + pCoNameID + ": ";
        }
      }
    }

    protected void SetContactValue(string pCoName, string pName)
    {
      // Set the Contact Name to string passed
      int i = 0;
      // force null string 
      if (pCoName == null) pCoName = "";
      if (pName == null) pName = "";
      // no find it
      while ((i < ddlContacts.Items.Count) && (!pCoName.Equals(ddlContacts.Items[i].Text, StringComparison.OrdinalIgnoreCase)))
        i++;

      if ((i < ddlContacts.Items.Count) && (pCoName.Equals(ddlContacts.Items[i].Text, StringComparison.OrdinalIgnoreCase)))
        ddlContacts.SelectedValue = ddlContacts.Items[i].Value;
      else
      {
        // did not find co, now look for name
        if (pCoName != pName) 
        {
          i = 0;
          while ((i < ddlContacts.Items.Count) && (!pName.Equals(ddlContacts.Items[i].Text, StringComparison.OrdinalIgnoreCase)))
            i++;

          if ((i < ddlContacts.Items.Count) && (pName.Equals(ddlContacts.Items[i].Text, StringComparison.OrdinalIgnoreCase)))
            ddlContacts.SelectedValue = ddlContacts.Items[i].Value;
          else
          { // did not find company to set it to generic
            ddlContacts.SelectedValue = CONST_ZZNAME_DEFAULTID;
            tbxNotes.Text += pCoName + ", " + pName + ": ";
          }

        }
        else
        { // did not find company to set it to generic
          ddlContacts.SelectedValue = CONST_ZZNAME_DEFAULTID;
          tbxNotes.Text += pCoName+": ";
        }
      }
    }

    /// <summary>
    /// Add the Last ORder for this client
    /// </summary>
    protected void AddLastOrder()
    {
      string _LastOrderSQL, _sFrom, _sWhere;
      string _sCustID; 

      if (ddlContacts.SelectedValue != null)
      {
        SetUpdateBools();
        // retrieve the last order items from the database.
        _sCustID = ddlContacts.SelectedValue;

        _sFrom = "FROM ItemTypeTbl INNER JOIN ItemUsageTbl ON ItemTypeTbl.ItemTypeID = ItemUsageTbl.ItemProvided ";
        _sWhere = " WHERE ItemTypeTbl.ServiceTypeID=2 AND CustomerID=" + _sCustID; //2 = coffee
        _LastOrderSQL = "SELECT ItemUsageTbl.ItemProvided, ItemUsageTbl.AmountProvided, ItemUsageTbl.Date AS LastOrderDate, ItemUsageTbl.PrepTypeID, ItemUsageTbl.PackagingID " +
           _sFrom + _sWhere + " AND (ItemUsageTbl.Date = (SELECT Max(ItemUsageTbl.Date) " + _sFrom + _sWhere + "))";


        OleDbConnection _LastOrderConnection = OpenTrackerOleDBConnection();
        OleDbCommand _LastOrderCommand = new OleDbCommand(_LastOrderSQL, _LastOrderConnection);
        _LastOrderConnection.Open();
        OleDbDataReader _LastOrderReader;
        _LastOrderReader = _LastOrderCommand.ExecuteReader();
        // call Read to start accessing data.
        string _LastOrderItemProvidedID, _LastOrderPackagingID;
        double _LastOrderQty;
        bool _LastOrder = (Session[CONST_LINESADDED] != null) ? (bool)Session[CONST_LINESADDED] : false;

        while (_LastOrderReader.Read())
        {
          if (_LastOrderReader["ItemProvided"] != null)
          {
            _LastOrderItemProvidedID = _LastOrderReader["ItemProvided"].ToString();
            _LastOrderQty = Convert.ToDouble(_LastOrderReader["AmountProvided"].ToString());
            if (_LastOrderReader["PackagingID"] != null)
              _LastOrderPackagingID = _LastOrderReader["PackagingID"].ToString();
            else
              _LastOrderPackagingID = "0";

            // now a a order line
            _LastOrder = (AddNewOrderLine(_LastOrderItemProvidedID, _LastOrderQty, _LastOrderPackagingID) || (_LastOrder));
          }
        }
        Session[CONST_LINESADDED] = _LastOrder;
        SetUpdateBools();
      }
    }

    protected OrderDetailData GetNewOrderItemFromSKU(string pSKU, double pSKUQTY)
    {
      // retrieve item from stock database and add to order table
      // string _connectionString, 
      string _ItemProvidedID;
      string _ItemsSQL = "SELECT ItemTypeID, SKU, ItemEnabled FROM ItemTypeTbl WHERE (SKU = ?)";
      OrderDetailData _OrderDetailItem = null; // assume not added

      if (ddlContacts.SelectedValue != null)
      {
        OleDbConnection _ItemsConnection = OpenTrackerOleDBConnection();
        OleDbCommand _ItemsCommand = new OleDbCommand(_ItemsSQL, _ItemsConnection);
        _ItemsCommand.Parameters.Add(new OleDbParameter { Value = pSKU });
        _ItemsConnection.Open();
        OleDbDataReader _ItemsReader;
        _ItemsReader = _ItemsCommand.ExecuteReader();
     
        // there should only be one record, if not something strange, but add only the first one
        if ((_ItemsReader.Read()) && (_ItemsReader["ItemTypeID"] != null))
        {
          _ItemProvidedID = _ItemsReader["ItemTypeID"].ToString();
          // ? May need to get default packaging from client data base?
          // now a a order line
          //AddNewOrderLine(_ItemProvidedID, pSKUQTY, "");
          
          _OrderDetailItem = new OrderDetailData { ItemTypeID = Convert.ToInt32(_ItemProvidedID), QuantityOrdered = pSKUQTY, PackagingID = 0 };
        }
        else
        {
          // sku not found, maybe add delivery next time
          tbxNotes.Text += "SKU Not Found: " + pSKU + " QTY: " + pSKUQTY.ToString();
        }
      }
      // now return the item
      return _OrderDetailItem;

    }

    private void BindRowQueryParameters()
    {
      Int32 _BoundCustomerId = Convert.ToInt32(ddlContacts.SelectedValue);
      Session[OrderHeaderData.CONST_BOUNDCUSTOMERID] = _BoundCustomerId;
      DateTime _BoundDeliveryDate = Convert.ToDateTime(tbxRequiredByDate.Text);
      Session[OrderHeaderData.CONST_BOUNDDELIVERYDATE] = _BoundDeliveryDate;
      String _BoundNotes = tbxNotes.Text;
      Session[OrderHeaderData.CONST_BOUNDNOTES] = _BoundNotes;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        DateTime dtThis = DateTime.Now;
        int NumDaysToAdd = 0;
        tbxOrderDate.Text = dtThis.ToShortDateString();

        // Set prep date to Monday if it in neither Wed or Thursday
        dtThis = DateTime.Now;
        // set roast date to be either Monday or Wed
        if ((dtThis.DayOfWeek > DayOfWeek.Tuesday) && (dtThis.DayOfWeek < DayOfWeek.Friday))
          NumDaysToAdd = (int)DayOfWeek.Wednesday - (int)dtThis.DayOfWeek;
        else
        {
          if (dtThis.DayOfWeek < DayOfWeek.Wednesday)  // we are still before the next prep date so set to Monday
            NumDaysToAdd = (int)DayOfWeek.Monday - (int)dtThis.DayOfWeek;
          else if (dtThis.DayOfWeek < DayOfWeek.Friday)
            NumDaysToAdd = (int)DayOfWeek.Wednesday - (int)dtThis.DayOfWeek;
          else // this is in the next week so set it to Monday next week)
            NumDaysToAdd = 8 - (int)dtThis.DayOfWeek;
        }
        dtThis = dtThis.AddDays(NumDaysToAdd);
        tbxRoastDate.Text = dtThis.ToShortDateString();
        // now set delivery date
        if (dtThis.DayOfWeek < DayOfWeek.Friday)
          tbxRequiredByDate.Text = dtThis.AddDays(1).ToShortDateString(); // the next day
        else
          tbxRequiredByDate.Text = dtThis.AddDays(3).ToShortDateString();  // Monday

        // set the page session variables
        bool _UpdateLines = false;   // if true then we will up date all lines
        bool _LinesAdded = false;   // if true then we added lines to the order
        Session[CONST_UPDATELINES] = _UpdateLines;
        Session[CONST_LINESADDED] = _LinesAdded;

        BindRowQueryParameters();

      }
      else
      {
        // check that the parameters have not changed
        bool _LinesAdded = (Session[CONST_UPDATELINES] == null) ? true : (bool)Session[CONST_UPDATELINES];
        if (!_LinesAdded)
        {   // check if there have been changes
          Int32 _BoundCustomerId = (Int32)Session[OrderHeaderData.CONST_BOUNDCUSTOMERID];
          DateTime _BoundDeliveryDate = (DateTime)Session[OrderHeaderData.CONST_BOUNDDELIVERYDATE];
          String _BoundNotes = (String)Session[OrderHeaderData.CONST_BOUNDNOTES];
          if ((_BoundCustomerId != Convert.ToInt32(ddlContacts.SelectedValue)) || (_BoundDeliveryDate != Convert.ToDateTime(tbxRequiredByDate.Text)) || (_BoundNotes != tbxNotes.Text))
            BindRowQueryParameters();
        }
      }
    }
    void Page_Unload (Object sender , EventArgs e)
    {
    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)  //Complete
    {
      if (!IsPostBack) 
      {
        // hand query string if sent
        // Look for first Customer Name, and then ann stock items sent, also allow or Last Order.
        if (Request.QueryString.Count > 0)
        {
          if (Request.QueryString[CONST_URL_REQUEST_CUSTOMERID] != null)
            SetContactByID(Request.QueryString[CONST_URL_REQUEST_CUSTOMERID]); 
          else if (Request.QueryString[CONST_URL_REQUEST_NAME] != null)
            SetContactValue(Request.QueryString[CONST_URL_REQUEST_COMPANYNAME], Request.QueryString[CONST_URL_REQUEST_NAME]);

          // check if they want last order
          if (Request.QueryString[CONST_URL_REQUEST_LASTORDER ] != null)
          {
              if (Request.QueryString[CONST_URL_REQUEST_LASTORDER ] == "Y")
                AddLastOrder();
            // now add stock item
          }
          if (Request.QueryString[CONST_URL_REQUEST_SKU1] != null)
          {
            List<QOnT.classes
              .OrderDetailData> _newOrderItems = new List<OrderDetailData>();

            // loop through all the SKUs and add them
            int i = 1;
            while (Request.QueryString["SKU" + i.ToString()] != null)
            {
              // check if there is such an item and return it if not null
              OrderDetailData _newItem = GetNewOrderItemFromSKU(Request.QueryString["SKU" + i], Convert.ToDouble(Request.QueryString["SKUQTY" + i]));
              if (_newItem != null)
                _newOrderItems.Add(_newItem);
              i++;
            }
            tbxNotes.Text += String.Format(">{0} items added", i - 1);
            // now add the items to the database
            foreach (OrderDetailData _newItem in _newOrderItems)
              AddNewOrderLine(_newItem.ItemTypeID.ToString(), _newItem.QuantityOrdered, _newItem.PackagingID.ToString());

            bool _LinesAdded = true;   // lines should have been added since tthere are items.
            Session[CONST_LINESADDED] = _LinesAdded;

///////////////
// should look at reopening the page in order detail here so that items are not readded.
///////////////

          }
        }
      }
    }

    //public string Check4Null(string pID)
    //{
    //  if (pID == null)
    //    return "0";
    //  else
    //    return pID;
    //}
    public void DeleteOrderItem(string pOrderID)
    {
      string _connectionString;
      string _ErrorStr = "";

      if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
          ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
      {
        throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
                            "must exist in the <connectionStrings> configuration section for the application.");
      }
      _connectionString = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

      string _sqlCmd = "DELETE FROM OrdersTbl WHERE (OrderId = ?)";
      OleDbConnection _conn = new OleDbConnection(_connectionString);

      // add parameters in the order they appear in the update command
      OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
      _cmd.Parameters.Add(new OleDbParameter { Value = pOrderID }); 
      
      Label _OrderIDLabel = (Label)gvOrderLines.FindControl("lblOrderID");
      _cmd.Parameters.Add(new OleDbParameter { Value = _OrderIDLabel.Text});

      try
      {
        _conn.Open();
        if (_cmd.ExecuteNonQuery() > 0)
          _ErrorStr="No records deleted";
      }
      catch (OleDbException oleDbErr)
      {
        // Handle exception.
        _ErrorStr = "Error: " + oleDbErr.Message;
      }
      finally
      {
        _conn.Close();
      }

      ltrlStatus.Text = (_ErrorStr.Length == 0) ? "Item deleted" : _ErrorStr;
    }

    protected void btnNewItem_Click(object sender, EventArgs e)
    {
      // show all buttons and text entry
      btnAdd.Visible = true;
      btnCancel.Visible = true;
      pnlNewItem.Visible = true;
      // now hide new button
      btnNewItem.Visible = false;
      upnlNewOrderItem.Update();

//      ddlNewItemDesc.DataBind();
//      ddlNewPackaging.DataBind();

    }
    private void HideNewOrderItemPanel()
    {
      // hide all buttons and text entry
      btnAdd.Visible = false;
      btnCancel.Visible = false;
      pnlNewItem.Visible = false;
      // now show new button
      btnNewItem.Visible = true;
      //update panels
      upnlNewOrderItem.Update();
      upnlOrderLines.Update();
      odsOrderDetail.DataBind();
      gvOrderLines.DataBind();
    }
    
    //private string dvOrderHeaderGetDDLControlSelectedValue(string pDDLControlName)
    //{
    //  DropDownList thisDDL = (DropDownList)dvOrderHeader.FindControl(pDDLControlName);

    //  return (thisDDL.SelectedValue != null) ? thisDDL.SelectedValue : "0"; 
    //}
    //private string dvOrderHeaderGetTextBoxValue(string pTextBoxControlName)
    //{
    //  TextBox thisTextBox = (TextBox)dvOrderHeader.FindControl(pTextBoxControlName);

    //  return thisTextBox.Text;
    //}
    //private string dvOrderHeaderGetLabelValue(string pTextBoxControlName)
    //{
    //  Label thisLabel = (Label)dvOrderHeader.FindControl(pTextBoxControlName);

    //  return thisLabel.Text;
    //}
    //private bool dvOrderHeaderGetCheckBoxValue(string pCheckBoxControlName)
    //{
    //  CheckBox thisCheckBox = (CheckBox)dvOrderHeader.FindControl(pCheckBoxControlName);

    //  return thisCheckBox.Checked;
    //}

    protected bool AddNewOrderLine(string pNewItemID, double pNewQuantityOrdered, string pNewPackagingID)
    {
      bool _ItemAdded = false;
      string _connectionString;
      string _ErrorStr = "";

      if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null || ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
      {
        throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
                            "must exist in the <connectionStrings> configuration section for the application.");
      }
      _connectionString = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;
      string _sqlCmd = "INSERT INTO OrdersTbl (CustomerId, OrderDate, RoastDate, RequiredByDate, ToBeDeliveredBy, Confirmed, Done, Notes, " +
                                              " ItemTypeID, QuantityOrdered, PackagingID) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
      OleDbConnection _conn = new OleDbConnection(_connectionString);                           //1  2  3  4  5  6  7  8  9  10 11

      // add parameters in the order they appear in the update command
      OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
      // first summary data
      _cmd.Parameters.Add(new OleDbParameter { Value = ddlContacts.SelectedValue });
      _cmd.Parameters.Add(new OleDbParameter { Value = tbxOrderDate.Text  });
      _cmd.Parameters.Add(new OleDbParameter { Value = tbxRoastDate.Text  });
      _cmd.Parameters.Add(new OleDbParameter { Value = tbxRequiredByDate.Text  });
      _cmd.Parameters.Add(new OleDbParameter { Value = ddlToBeDeliveredBy.SelectedValue });
      _cmd.Parameters.Add(new OleDbParameter { Value = cbxConfirmed.Checked });
      _cmd.Parameters.Add(new OleDbParameter { Value = cbxDone.Checked });
      _cmd.Parameters.Add(new OleDbParameter { Value = tbxNotes.Text });

      // Now line data
      _cmd.Parameters.Add(new OleDbParameter { Value = pNewItemID });
      _cmd.Parameters.Add(new OleDbParameter { Value = pNewQuantityOrdered });
      if (pNewPackagingID == String.Empty)
        _cmd.Parameters.Add(new OleDbParameter { Value = DBNull.Value });
      else
      _cmd.Parameters.Add(new OleDbParameter { Value = pNewPackagingID });

      try
      {
        _conn.Open();
        _ItemAdded = (_cmd.ExecuteNonQuery() > 0);
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
      return _ItemAdded;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
      bool _ItemAdded = 
        AddNewOrderLine(ddlNewItemDesc.SelectedValue, Convert.ToDouble(tbxNewQuantityOrdered.Text), ddlNewPackaging.SelectedValue);

      Session[CONST_LINESADDED] = _ItemAdded; // a line has been added

      //bool _ItemAdded = false;
      //string _connectionString;
      //string _ErrorStr = "";

      //if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
      //    ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
      //{
      //  throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
      //                      "must exist in the <connectionStrings> configuration section for the application.");
      //}
      //_connectionString =
      //  ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;
      //string _sqlCmd = "INSERT INTO OrdersTbl (CustomerId, OrderDate, RoastDate, RequiredByDate, ToBeDeliveredBy, Confirmed, Done, Notes, " +
      //                                        " ItemTypeID, QuantityOrdered, PackagingID) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
      //OleDbConnection _conn = new OleDbConnection(_connectionString);                           //1  2  3  4  5  6  7  8  9  10 11

      //// add parameters in the order they appear in the update command
      //OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
      //// first summary data
      //_cmd.Parameters.Add(new OleDbParameter { Value = ddlContacts.SelectedValue });
      //_cmd.Parameters.Add(new OleDbParameter { Value = tbxOrderDate.Text  });
      //_cmd.Parameters.Add(new OleDbParameter { Value = tbxRoastDate.Text  });
      //_cmd.Parameters.Add(new OleDbParameter { Value = tbxRequiredByDate.Text  });
      //_cmd.Parameters.Add(new OleDbParameter { Value = ddlToBeDeliveredBy.SelectedValue });
      //_cmd.Parameters.Add(new OleDbParameter { Value = cbxConfirmed.Checked });
      //_cmd.Parameters.Add(new OleDbParameter { Value = cbxDone.Checked });
      //_cmd.Parameters.Add(new OleDbParameter { Value = tbxNotes.Text });

      //// Now line data
      //_cmd.Parameters.Add(new OleDbParameter { Value = ddlNewItemDesc.SelectedValue });
      //_cmd.Parameters.Add(new OleDbParameter { Value = tbxNewQuantityOrdered.Text });
      //_cmd.Parameters.Add(new OleDbParameter { Value = ddlNewPackaging.SelectedValue });

      //try
      //{
      //  _conn.Open();
      //  _ItemAdded = (_cmd.ExecuteNonQuery() > 0);
      //}
      //catch (OleDbException oleErr)
      //{
      //  // Handle exception.
      //  _ErrorStr = oleErr.Message;
      //}
      //finally
      //{
      //  _conn.Close();
      //}
      ltrlStatus.Text = (_ItemAdded == true ? "Item Added" : "Error adding item"); //: " + _ErrorStr);
      HideNewOrderItemPanel();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
      HideNewOrderItemPanel();
    }

    protected void gvOrderLines_OnItemDelete(object sender, EventArgs e)
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
      
      CommandEventArgs cea =  (CommandEventArgs)e;
      string _OrderId = cea.CommandArgument.ToString();
      string _sqlDeleteCmd = "DELETE FROM OrdersTbl WHERE (OrderID = ?)";

      OleDbConnection _conn = new OleDbConnection(_connectionString);                           //1  2  3  4  5  6  7  8  9  10 11
      // add parameters in the order they appear in the update command
      OleDbCommand _cmd = new OleDbCommand(_sqlDeleteCmd, _conn);
      _cmd.Parameters.Add(new OleDbParameter { Value = _OrderId });

      try
      {
        _conn.Open();
        _ItemAdded = (_cmd.ExecuteNonQuery() > 0);
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

    }

    protected virtual void dvOrderHeader_OnItemUpdated(Object sender, DetailsViewUpdatedEventArgs e)
    {
      gvOrderLines.DataBind();
      upnlOrderLines.Update();
    }

    protected void ddlToBeDeliveredBy_OnDataBound(object sender, EventArgs e)
    {
      // Find Sino as the preferred default
      int i = 0;
      while ((i < ddlToBeDeliveredBy.Items.Count) && (ddlToBeDeliveredBy.Items[i].Text != CONST_DELIVERY_DEFAULT))
        i++;

      if (i < ddlToBeDeliveredBy.Items.Count)
        ddlToBeDeliveredBy.SelectedValue = ddlToBeDeliveredBy.Items[i].Value; // should be Sino

    }

    protected void btnAddLastOrder_Click(object sender, EventArgs e)
    {
      AddLastOrder();
      gvOrderLines.DataBind();
      upnlOrderLines.Update();
      upnlNewOrderItem.Update();
    }

    protected void btnCancelled_Click(object sender, EventArgs e)
    {
      MembershipUser _currMember = Membership.GetUser();

      if (_currMember.UserName.ToLower() == "warren")
      {
        // only if it is warren allow this
        foreach (GridViewRow gvr in gvOrderLines.Rows)
        {
          Label myOrderIDLablel = (Label)gvr.FindControl("lblOrderID"); //find contol since it is a template field
          DeleteOrderItem(myOrderIDLablel.Text);
        }
  
      }
    }

    protected void tbxNotes_TextChanged(object sender, EventArgs e)
    {
      SetUpdateBools();
    }

    protected void gvOrderLines_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnRefreshDetails_Click(object sender, EventArgs e)
    {
      gvOrderLines.DataBind();
      ddlContacts.DataBind();
      upnlNewOrderItem.Update();
      upnlOrderLines.Update();
    }

    protected void tmrOrderItem_OnTick(object sender, EventArgs e)
    {
      // only do this once quickly since the pre-render cannto bind the grid.
      bool _LinesAdded = (Session[CONST_LINESADDED] != null) ? (bool)Session[CONST_LINESADDED] : false;
      if (_LinesAdded)
        btnRefreshDetails_Click(sender, e);
      tmrOrderItem.Enabled = false;
    }
    protected void SetUpdateBools()
    {
      // Update all the orders to reflect this. 
      bool _LinesAdded = (Session[CONST_LINESADDED] != null) ? (bool)Session[CONST_LINESADDED] : false;
      bool _UpdateLines = (Session[CONST_UPDATELINES] != null) ? (bool)Session[CONST_UPDATELINES] : false;
      if ((_LinesAdded) && (!_UpdateLines))
      {
        // items need to be updated set the button to visible
        _UpdateLines = true;
        btnUpdate.Visible = _UpdateLines;
        upnlOrderSummary.Update();
        Session[CONST_UPDATELINES] = _UpdateLines;
      }
      else // lines have not been added or update has 
        BindRowQueryParameters();
    }
    /// <summary>
    /// Something in the header data has changed so do a generic update
    /// </summary>
    protected void DoHeaderUpdate()
    {
      List<string> _OrderIds = (List<string>)Session[CONST_ORDERLINEIDS];
      //List<string> _OrderIds = new List<string>();

      //DataTable _dtOrders = gvOrderLines.DataSource as DataTable;

      //foreach (GridViewRow gvr in _dtOrders.Rows)
      //{
      //  Label myOrderIDLablel = (Label)gvr.FindControl("lblOrderID");    //find contol since it is a template field
      //  _OrderIds.Add(myOrderIDLablel.Text);
      //}
      if (_OrderIds.Count > 0)
      {
        OrderDBAgent _myDB = new OrderDBAgent();
        OrderHeaderData _OrderHeader = new OrderHeaderData();

        _OrderHeader.CustomerID = Convert.ToInt64(ddlContacts.SelectedValue);
        _OrderHeader.OrderDate = Convert.ToDateTime(tbxOrderDate.Text);
        _OrderHeader.RoastDate = Convert.ToDateTime(tbxRoastDate.Text);
        _OrderHeader.ToBeDeliveredBy = Convert.ToInt64(ddlToBeDeliveredBy.SelectedValue);
        _OrderHeader.RequiredByDate = Convert.ToDateTime(tbxRequiredByDate.Text);
        _OrderHeader.Confirmed = cbxConfirmed.Checked;
        _OrderHeader.Done = cbxDone.Checked;
        _OrderHeader.Notes = tbxNotes.Text;

        _myDB.UpdateOrderHeader(_OrderHeader,_OrderIds);
      }
      BindRowQueryParameters();
    }
    protected void ddlContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      QOnT.classes.TrackerTools tt = new classes.TrackerTools();
      DropDownList _ddlCustomers = (DropDownList)sender;
      // roast day vars
      DateTime _dtNextRoastDay;   
      DateTime _dtDelivery = DateTime.Now;
      // preference vars;
      Int64 _CustID = Convert.ToInt64(_ddlCustomers.SelectedValue);
      // int _preferredDlvryBy = 3; 

      _dtNextRoastDay = tt.GetNextRoastDateByCustomerID(_CustID, ref _dtDelivery);

      tbxRoastDate.Text = String.Format("{0:d}", _dtNextRoastDay);
      tbxRequiredByDate.Text = String.Format("{0:d}", _dtDelivery);
      
      // setup values
      SetUpdateBools();
    }

    protected void tbxOrderDate_TextChanged(object sender, EventArgs e)
    {
      SetUpdateBools();
    }

    protected void tbxRoastDate_TextChanged(object sender, EventArgs e)
    {
      SetUpdateBools();
    }

    protected void ddlToBeDeliveredBy_SelectedIndexChanged(object sender, EventArgs e)
    {
      SetUpdateBools();
    }

    protected void tbxRequiredByDate_TextChanged(object sender, EventArgs e)
    {
      SetUpdateBools();
    }

    protected void cbxConfirmed_CheckedChanged(object sender, EventArgs e)
    {
      SetUpdateBools();
    }

    protected void cbxDone_CheckedChanged(object sender, EventArgs e)
    {
      SetUpdateBools();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        DoHeaderUpdate();
        btnUpdate.Visible = false;
        // now set update to false for next time.
        bool _UpdateLines = false;
        Session[CONST_UPDATELINES] = _UpdateLines;
    }

    protected void gvOrderLines_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.Header)
      {
        //  clear the list of order ids
        List<string> _OrderIds = new List<string>();
        Session[CONST_ORDERLINEIDS] = _OrderIds;

      }
      else if(e.Row.RowType == DataControlRowType.DataRow)
      {
        // For each line store the OrderID
        List<string> _OrderIds = (List<string>)Session[CONST_ORDERLINEIDS];
        if (_OrderIds == null)
          _OrderIds = new List<string>();

        Label _lblOrderID = (Label)e.Row.FindControl("lblOrderId");

        _OrderIds.Add(_lblOrderID.Text);
        Session[CONST_ORDERLINEIDS] = _OrderIds;
      }
    }


  }
}