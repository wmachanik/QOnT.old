using System;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Web.Security;
using QOnT.App_Code;
using QOnT.classes;
using QOnT.control;
using System.Collections.Generic;

namespace QOnT.Pages
{
  public partial class OrderDetail : System.Web.UI.Page
  {
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQLGETCONTACTEMAILDETAILS = "SELECT ContactFirstName, ContactLastName, ContactAltFirstName, ContactAltLastName, EmailAddress, AltEmailAddress, CustomerID " +
                                                   " FROM CustomersTbl WHERE (CustomerID = ?)";
    const string CONST_FROMEMAIL = "orders@quaffee.co.za";
    const string CONST_DELIVERYTYPEISCOLLECTION = "Cllct";
    const string CONST_DELIVERYTYPEISCOURIER = "Cour";

    private void OpenTrackerOleDBConnection(ref OleDbConnection pConn)
    {
      pConn = null;
      string _connectionString;

      if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
          ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
      {
        throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
                            "must exist in the <connectionStrings> configuration section for the application.");
      }
      _connectionString = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;
      pConn = new OleDbConnection(_connectionString);                           
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Int32 _BoundCustomerId = 1;
        DateTime _BoundDeliveryDate = DateTime.Now;
        String _BoundNotes = String.Empty;

        if (Request.QueryString["CustomerID"] != null)
          _BoundCustomerId = Convert.ToInt32(Request.QueryString["CustomerID"].ToString());
        if (Request.QueryString["DeliveryDate"] != null)
          _BoundDeliveryDate = Convert.ToDateTime(Request.QueryString["DeliveryDate"]);
        if (Request.QueryString["Notes"] != null)
          _BoundNotes = Request.QueryString["Notes"].ToString();
        
        Session[OrderHeaderData.CONST_BOUNDCUSTOMERID] = _BoundCustomerId;
        Session[OrderHeaderData.CONST_BOUNDDELIVERYDATE] = _BoundDeliveryDate;
        Session[OrderHeaderData.CONST_BOUNDNOTES] = _BoundNotes;

        OrderItemTbl mine_ = new OrderItemTbl();

      }
    }

    //public string Check4Null(string pID)
    //{
    //  if (pID == null)
    //    return "0";
    //  else
    //    return pID;
    //}
    //public void DeleteOrderItem()
    //{
    //  string _connectionString;
    //  string _ErrorStr = "";

    //  if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
    //      ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
    //  {
    //    throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
    //                        "must exist in the <connectionStrings> configuration section for the application.");
    //  }
    //  _connectionString =
    //    ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

    //  string _sqlCmd = "DELETE FROM OrdersTbl WHERE (OrderId = ?)";
    //  OleDbConnection _conn = new OleDbConnection(_connectionString);

    //  // add parameters in the order they appear in the update command
    //  OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
      
    //  Label _OrderIDLabel = (Label)gvOrderLines.FindControl("lblOrderID");
    //  _cmd.Parameters.Add(new OleDbParameter { Value = _OrderIDLabel.Text});

    //  try
    //  {
    //    _conn.Open();
    //    if (_cmd.ExecuteNonQuery() > 0)
    //      _ErrorStr="No records deleted";
    //  }
    //  catch (OleDbException oleDbErr)
    //  {
    //    // Handle exception.
    //    _ErrorStr = "Error: " + oleDbErr.Message;
    //  }
    //  finally
    //  {
    //    _conn.Close();
    //  }

    //  ltrlStatus.Text = (_ErrorStr.Length == 0) ? "Item deleted" : _ErrorStr;
    //}


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
    
    private string dvOrderHeaderGetDDLControlSelectedValue(string pDDLControlName)
    {
      DropDownList thisDDL = (DropDownList)dvOrderHeader.FindControl(pDDLControlName);

      return (thisDDL.SelectedValue != null) ? thisDDL.SelectedValue : "0"; 
    }
    private string dvOrderHeaderGetTextBoxValue(string pTextBoxControlName)
    {
      TextBox thisTextBox = (TextBox)dvOrderHeader.FindControl(pTextBoxControlName);

      return thisTextBox.Text;
    }
    private string dvOrderHeaderGetLabelValue(string pTextBoxControlName)
    {
      Label thisLabel = (Label)dvOrderHeader.FindControl(pTextBoxControlName);

      return thisLabel.Text;
    }
    private bool dvOrderHeaderGetCheckBoxValue(string pCheckBoxControlName)
    {
      CheckBox thisCheckBox = (CheckBox)dvOrderHeader.FindControl(pCheckBoxControlName);

      return thisCheckBox.Checked;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
      bool _ItemAdded = false;
      string _ErrorStr = "Connection not openned";

      OleDbConnection _conn = null;
      OpenTrackerOleDBConnection(ref _conn);
      if (_conn != null)
      {

        string _sqlCmd = "INSERT INTO OrdersTbl (CustomerId, OrderDate, RoastDate, RequiredByDate, ToBeDeliveredBy, Confirmed, Done, Notes, " +
                                                " ItemTypeID, QuantityOrdered, PackagingID) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

        // add parameters in the order they appear in the update command
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        // first summary data
        _cmd.Parameters.Add(new OleDbParameter { Value = dvOrderHeaderGetDDLControlSelectedValue("ddlContacts") });
        _cmd.Parameters.Add(new OleDbParameter { Value = dvOrderHeaderGetLabelValue("lblOrderDate") });
        _cmd.Parameters.Add(new OleDbParameter { Value = dvOrderHeaderGetLabelValue("lblRoastDate") });
        _cmd.Parameters.Add(new OleDbParameter { Value = dvOrderHeaderGetLabelValue("lblRequiredByDate") });
        _cmd.Parameters.Add(new OleDbParameter { Value = dvOrderHeaderGetDDLControlSelectedValue("ddlToBeDeliveredBy") });
        _cmd.Parameters.Add(new OleDbParameter { Value = dvOrderHeaderGetCheckBoxValue("cbxConfirmed") });
        _cmd.Parameters.Add(new OleDbParameter { Value = dvOrderHeaderGetCheckBoxValue("cbxDone") });
        _cmd.Parameters.Add(new OleDbParameter { Value = dvOrderHeaderGetLabelValue("lblNotes") });

        // Now line data
        _cmd.Parameters.Add(new OleDbParameter { Value = ddlNewItemDesc.SelectedValue });
        _cmd.Parameters.Add(new OleDbParameter { Value = tbxNewQuantityOrdered.Text });
        _cmd.Parameters.Add(new OleDbParameter { Value = ddlNewPackaging.SelectedValue });

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
      }
      ltrlStatus.Text = (_ItemAdded == true ? "Item Added" : "Error adding item: " + _ErrorStr);
      HideNewOrderItemPanel();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
      HideNewOrderItemPanel();
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
      CheckBox thisCheckBox = (CheckBox)dvOrderHeader.FindControl("cbxDone");

      if (thisCheckBox != null)
        btnOrderDelivered.Enabled = btnOrderCancelled.Enabled = thisCheckBox.Checked;
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

      try
      {
        _conn.Open();
        if (_cmd.ExecuteNonQuery() != 0)
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
    protected void btnCancelled_Click(object sender, EventArgs e)
    {
      MembershipUser _currMember = Membership.GetUser();

      if (_currMember.UserName.ToLower() == "warren")
      {
        // only if it is warren allow this
        Label OrderLbl;
        foreach (GridViewRow gvr in gvOrderLines.Rows) 
        {
          OrderLbl = (Label)gvr.Cells[4].FindControl("lblOrderID");
          DeleteOrderItem(OrderLbl.Text);
        }

        gvOrderLines.DataBind();
        upnlOrderLines.Update();
      }
    }
    private QOnT.App_Code.ContactDetails.ContactEmailDetails GetContactsEmailDetails(string pContactsID)
    {
      ContactDetails.ContactEmailDetails _contactEmailDetails = null;
      string _ErrorStr = "connection not openned";
      OleDbConnection _conn = null;
      OpenTrackerOleDBConnection(ref _conn);
      if (_conn != null)
      {
        // add parameters in the order they appear in the update command
        OleDbCommand _cmd = new OleDbCommand(CONST_SQLGETCONTACTEMAILDETAILS, _conn);
        _cmd.Parameters.Add(new OleDbParameter { Value = pContactsID });

        try
        {
          _conn.Open();
          OleDbDataReader _drEmailDetails = _cmd.ExecuteReader();
          if (_drEmailDetails.Read())
          {
            _contactEmailDetails = new ContactDetails.ContactEmailDetails();
            if (_drEmailDetails["ContactFirstName"] != null)
              _contactEmailDetails.FirstName = _drEmailDetails["ContactFirstName"].ToString();
            if (_drEmailDetails["ContactLastName"] != null)
              _contactEmailDetails.LastName = _drEmailDetails["ContactLastName"].ToString();
            if (_drEmailDetails["EmailAddress"] !=null)
              _contactEmailDetails.EmailAddress = _drEmailDetails["EmailAddress"].ToString();
            if (_drEmailDetails["ContactAltFirstName"] != null)
              _contactEmailDetails.altFirstName = _drEmailDetails["ContactAltFirstName"].ToString();
            if (_drEmailDetails["ContactAltLastName"] != null)
              _contactEmailDetails.altLastName = _drEmailDetails["ContactAltLastName"].ToString();
            if (_drEmailDetails["AltEmailAddress"] != null)
              _contactEmailDetails.altEmailAddress = _drEmailDetails["AltEmailAddress"].ToString();
          }
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
        return _contactEmailDetails;
      }
      else
      {
        ltrlStatus.Text = _ErrorStr;
        return _contactEmailDetails;
      }
    }

    /// <summary>
    /// Send email to the current client to confirm the items in the current order, and the deliver date
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnConfirmOrder_Click(object sender, EventArgs e)
    {
      DropDownList _ddlThisContact = (DropDownList)dvOrderHeader.FindControl("ddlContacts");

      ContactDetails.ContactEmailDetails _thisContact = GetContactsEmailDetails(_ddlThisContact.SelectedItem.Value);
      if (_thisContact != null)
      {
        EmailCls _email = new EmailCls();
        DropDownList _ddlDeliveryBy = (DropDownList)dvOrderHeader.FindControl("ddlToBeDeliveredBy");  // who is delivering this
        Label _lblDeliveryDate = (Label)dvOrderHeader.FindControl("lblRequiredByDate");  // date it will be dispatched / delivered"

        if (_thisContact.EmailAddress != "")
        {
          _email.SetEmailFromTo(CONST_FROMEMAIL, _thisContact.EmailAddress);
          if (_thisContact.altEmailAddress != "")
            _email.SetEmailCC(_thisContact.altEmailAddress);
        }
        else if (_thisContact.altEmailAddress != "")
          _email.SetEmailFromTo(CONST_FROMEMAIL, _thisContact.altEmailAddress);
        else
          return;  // no email address so quit

        // send a BCC to orders to confirm
        _email.SetEmailBCC(CONST_FROMEMAIL);
        // set subject and body
        _email.SetEmailSubject("Order Confirmation");
        _email.AddStrAndNewLineToBody("Dear " + ((_thisContact.FirstName != "") ? _thisContact.FirstName : ((_thisContact.altFirstName != "") ? _thisContact.altFirstName : "Coffee Lover")) + ",<br />");
        _email.AddStrAndNewLineToBody("We confirm the following order for "+_ddlThisContact.SelectedItem.Text+":");
        _email.AddToBody("<ul>");
        foreach (GridViewRow _gv in gvOrderLines.Rows)
        {
          DropDownList _gvItemDesc = (DropDownList)_gv.FindControl("ddlItemDesc");
          Label _gvItemQty = (Label)_gv.FindControl("lblQuantityOrdered");
          DropDownList _gvItemPackaging = (DropDownList)_gv.FindControl("ddlPackaging");
          // need to check for serivce / note and add the note using the same logic as we have for the delivery sheet
          if (_gvItemPackaging.SelectedIndex == 0)
            _email.AddFormatToBody("<li>{0} x {1}</li>", _gvItemQty.Text, _gvItemDesc.SelectedItem.Text);
          else
            _email.AddFormatToBody("<li>{0} x {1} - Preperation note: {2}</li>", _gvItemQty.Text, _gvItemDesc.SelectedItem.Text, _gvItemPackaging.SelectedItem.Text);
        }
        _email.AddStrAndNewLineToBody("</ul>");

        if (_ddlDeliveryBy.SelectedItem.Text == CONST_DELIVERYTYPEISCOLLECTION)
          _email.AddStrAndNewLineToBody("Will be ready for collection on: " + _lblDeliveryDate.Text);
        else if (_ddlDeliveryBy.SelectedItem.Text == CONST_DELIVERYTYPEISCOURIER)
          _email.AddStrAndNewLineToBody("Will be dispatched on: " + _lblDeliveryDate.Text + ".");
        else
          _email.AddStrAndNewLineToBody("Will be delivered on: " + _lblDeliveryDate.Text + ".");

        // Add a footer
        _email.AddStrAndNewLineToBody("<br />Sent automatically by Quaffee's order and tracking System.<br /><br />Sincerely Quaffee Team (orders@quaffee.co.za)");
        if (_email.SendEmail())
          ltrlStatus.Text = "Email Sent to: " + _thisContact.EmailAddress;
        else
          ltrlStatus.Text = "Email was not sent!";
      }
    }

    protected void OnDataBinding_ddlToBeDeliveredBy(object sender, EventArgs e)
    {
      DropDownList ddl = (DropDownList)sender; 
      if (ddl != null)
      {
        //if ((ddl.SelectedValue == null) || (ddl.SelectedValue == "") || (ddl.SelectedValue=="0"))
        //  ddl.SelectedValue = "3"; // should be sino
      }

    }
    protected void odsOrderSummary_OnUpdated(object sender, ObjectDataSourceStatusEventArgs e)
    {
      // get data stored on the forw
      //  ViewState["dvOrderHeader"]


      ////// stuck here trying to get the project to run, trying to update the session variables so that on update the record does not dissappear.

      if ((bool)e.ReturnValue == true)
      {
        DropDownList _ddlContacts = (DropDownList)dvOrderHeader.FindControl("ddlContacts");
        TextBox _tbxRequiredByDate = (TextBox)dvOrderHeader.FindControl("tbxRequiredByDate");
        TextBox _tbxNotes = (TextBox)dvOrderHeader.FindControl("tbxNotes");
        // records where updated so update the session variables.
        if ((_ddlContacts != null) && (_tbxRequiredByDate != null) && (_tbxNotes != null))
        {
          // redirect website using query string ?
          string _OrderDetailURL = String.Format("{0}?CustomerID={1}&DeliveryDate={2:d}&Notes={3}", 
                                                 ResolveUrl("~/Pages/OrderDetail.aspx"), _ddlContacts.SelectedValue,
                                                 Convert.ToDateTime(_tbxRequiredByDate.Text), _tbxNotes.Text);
          Response.Redirect(_OrderDetailURL);
        }
        
      }
    }
    /// <summary>
    /// From the client selected create a temporary table from the items in the order
    /// Then diosplay another for for the user to select how to close the order
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOrderDelivered_Click(object sender, EventArgs e)
    {
      // General VARs
      TempOrdersData _TempOrdersData = new TempOrdersData();
      TempOrdersDAL _TempOrdersDAL  = new TempOrdersDAL();
      // DELETE data from TempOrdersHeaderTblData
      if (!_TempOrdersDAL.KillTempOrdersData())
      { ltrlStatus.Text = "Error deleting Temp Table"; }
        
      // add parameters in the order they appear in the update command
      // first summary / header data
      _TempOrdersData.HeaderData.CustomerID = Convert.ToInt32(dvOrderHeaderGetDDLControlSelectedValue("ddlContacts"));
      _TempOrdersData.HeaderData.OrderDate = Convert.ToDateTime(dvOrderHeaderGetLabelValue("lblOrderDate"));
      _TempOrdersData.HeaderData.RoastDate = Convert.ToDateTime(dvOrderHeaderGetLabelValue("lblRoastDate"));
      _TempOrdersData.HeaderData.RequiredByDate = Convert.ToDateTime(dvOrderHeaderGetLabelValue("lblRequiredByDate"));
      _TempOrdersData.HeaderData.ToBeDeliveredByID = Convert.ToInt32(dvOrderHeaderGetDDLControlSelectedValue("ddlToBeDeliveredBy") );
      _TempOrdersData.HeaderData.Confirmed = dvOrderHeaderGetCheckBoxValue("cbxConfirmed");
      _TempOrdersData.HeaderData.Done = dvOrderHeaderGetCheckBoxValue("cbxDone");
      _TempOrdersData.HeaderData.Notes = dvOrderHeaderGetLabelValue("lblNotes");

      // now the line data (the TO header is set when we add both using the one class TempOrders
      ItemTypeTbl _ItemType = new ItemTypeTbl();
      foreach (GridViewRow _gv in gvOrderLines.Rows)
      {
        TempOrdersLinesTbl _LineData = new TempOrdersLinesTbl();

        DropDownList _gvItemDesc = (DropDownList)_gv.FindControl("ddlItemDesc");
        Label _gvItemQty = (Label)_gv.FindControl("lblQuantityOrdered");
        DropDownList _gvItemPackaging = (DropDownList)_gv.FindControl("ddlPackaging");
        Label _gvOrderID = (Label)_gv.FindControl("lblOrderID");

        _LineData.ItemID = Convert.ToInt32(_gvItemDesc.SelectedValue);
        _LineData.Qty = Convert.ToDouble(_gvItemQty.Text);
        _LineData.PackagingID = Convert.ToInt32(_gvItemPackaging.SelectedValue);
        _LineData.ServiceTypeID = _ItemType.GetServiceID(_LineData.ItemID);
        _LineData.OriginalOrderID = Convert.ToInt64(_gvOrderID.Text);
           
        _TempOrdersData.OrdersLines.Add(_LineData);
      }

      // now add all the data to the database
      _TempOrdersDAL.Insert(_TempOrdersData);
      // open new form with database, p 
      Response.Redirect("OrderDone.aspx");
    }

  }
}