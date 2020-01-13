using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using QOnT.control;
using QOnT.classes;

namespace QOnT.Pages
{
  public partial class CustomerDetails : System.Web.UI.Page
  {
    static string prevPage = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        // get referring page
        if ((Request.UrlReferrer == null))
          prevPage = String.Empty;
        else
          prevPage = Request.UrlReferrer.ToString();
        // if the id is past and is not null then set it
        if ((Request.QueryString["ID"]) != null)
        {
          CustomersTbl _ctd = new CustomersTbl();
          PutDataFromForm(_ctd.GetCustomersByCustomerID(Convert.ToInt64(Request.QueryString["ID"]), ""));
          gvItems.Sort("Date",SortDirection.Descending);
        }
        else
        {
          btnUpdate.Enabled = false;
          btnUpdateAndReturn.Enabled = false;
          btnAddLasOrder.Enabled = false;
          btnForceNext.Enabled = false;
          btnInsert.Enabled = true;
          enabledCheckBox.Checked = true;   // if it is an insert enable the customer
        }

      }
    }
    private void PutDataFromForm(CustomersTbl pCustomersTblData)
    {
      CompanyNameTextBox.Text = pCustomersTblData.CompanyName;
      CompanyIDLabel.Text = pCustomersTblData.CustomerID.ToString();
      ContactFirstNameTextBox.Text = pCustomersTblData.ContactFirstName;
      ContactLastNameTextBox.Text = pCustomersTblData.ContactLastName;
      ContactTitleTextBox.Text = pCustomersTblData.ContactTitle;
      ContactAltFirstNameTextBox.Text = pCustomersTblData.ContactAltFirstName;
      ContactAltLastNameTextBox.Text = pCustomersTblData.ContactAltLastName;
      BillingAddressTextBox.Text = pCustomersTblData.BillingAddress;
      DepartmentTextBox.Text = pCustomersTblData.Department;
      PostalCodeTextBox.Text = pCustomersTblData.PostalCode;
      ddlCities.SelectedValue = pCustomersTblData.City.ToString();
      ProvinceTextBox.Text = pCustomersTblData.Province;
      PhoneNumberTextBox.Text = pCustomersTblData.PhoneNumber;
      CellNumberTextBox.Text = pCustomersTblData.CellNumber;
      FaxNumberTextBox.Text = pCustomersTblData.FaxNumber;
      EmailAddressTextBox.Text = pCustomersTblData.EmailAddress;
      AltEmailAddressTextBox.Text = pCustomersTblData.AltEmailAddress;
      ddlCustomerTypes.SelectedValue = pCustomersTblData.CustomerTypeID.ToString();
      ddlEquipTypes.SelectedValue = pCustomersTblData.EquipType.ToString();
      MachineSNTextBox.Text = pCustomersTblData.MachineSN;
      ddlFirstPreference.SelectedValue = pCustomersTblData.CoffeePreference.ToString();
      PriPrefQtyTextBox.Text = pCustomersTblData.PriPrefQty.ToString();
      ddlPackagingTypes.SelectedValue = pCustomersTblData.PrefPackagingID.ToString();
      ddlDeliveryBy.SelectedValue = pCustomersTblData.PreferedAgent.ToString();
      ddlAgent.SelectedValue = pCustomersTblData.SalesAgentID.ToString();
      ReminderCountLabel.Text = pCustomersTblData.ReminderCount.ToString();
      enabledCheckBox.Checked = pCustomersTblData.enabled;
      autofulfillCheckBox.Checked = pCustomersTblData.autofulfill;
      UsesFilterCheckBox.Checked = pCustomersTblData.UsesFilter;
      PredictionDisabledCheckBox.Checked = pCustomersTblData.PredictionDisabled;
      AlwaysSendChkUpCheckBox.Checked = pCustomersTblData.AlwaysSendChkUp;
      NormallyRespondsCheckBox.Checked = pCustomersTblData.NormallyResponds;
      NotesTextBox.Text = pCustomersTblData.Notes;

    }

    private CustomersTbl GetDataFromForm()
    {
      CustomersTbl _CustomersTblData = new CustomersTbl();

      _CustomersTblData.CompanyName = CompanyNameTextBox.Text;
      _CustomersTblData.CustomerID = (String.IsNullOrWhiteSpace(CompanyIDLabel.Text)) ? 0 : Convert.ToInt64(CompanyIDLabel.Text);  //0  for insert
      _CustomersTblData.ContactFirstName = ContactFirstNameTextBox.Text;
      _CustomersTblData.ContactLastName = ContactLastNameTextBox.Text;
      _CustomersTblData.ContactTitle = ContactTitleTextBox.Text;
      _CustomersTblData.ContactAltFirstName = ContactAltFirstNameTextBox.Text;
      _CustomersTblData.ContactAltLastName = ContactAltLastNameTextBox.Text;
      _CustomersTblData.BillingAddress = BillingAddressTextBox.Text;
      _CustomersTblData.Department = DepartmentTextBox.Text;
      _CustomersTblData.PostalCode = PostalCodeTextBox.Text;
      _CustomersTblData.City = Convert.ToInt32(ddlCities.SelectedValue);
      _CustomersTblData.Province = ProvinceTextBox.Text;
      _CustomersTblData.PhoneNumber = PhoneNumberTextBox.Text;
      _CustomersTblData.CellNumber = CellNumberTextBox.Text;
      _CustomersTblData.FaxNumber = FaxNumberTextBox.Text;
      _CustomersTblData.EmailAddress = EmailAddressTextBox.Text;
      _CustomersTblData.AltEmailAddress = AltEmailAddressTextBox.Text;
      _CustomersTblData.CustomerTypeID = Convert.ToInt32(ddlCustomerTypes.SelectedValue);
      _CustomersTblData.EquipType = Convert.ToInt32(ddlEquipTypes.SelectedValue); // CoffeePreferenceTextBox.Text);
      _CustomersTblData.MachineSN = MachineSNTextBox.Text;
      _CustomersTblData.CoffeePreference = Convert.ToInt32(ddlFirstPreference.SelectedValue); // CoffeePreferenceTextBox.Text);
      _CustomersTblData.PriPrefQty = String.IsNullOrWhiteSpace(PriPrefQtyTextBox.Text) ? 0 : Convert.ToDouble(PriPrefQtyTextBox.Text);
      _CustomersTblData.PrefPackagingID = Convert.ToInt32(ddlPackagingTypes.SelectedValue);
      _CustomersTblData.PreferedAgent = Convert.ToInt32(ddlDeliveryBy.SelectedValue);
      _CustomersTblData.SalesAgentID = Convert.ToInt32(ddlAgent.SelectedValue);
      _CustomersTblData.enabled = enabledCheckBox.Checked;
      _CustomersTblData.autofulfill = autofulfillCheckBox.Checked;
      _CustomersTblData.UsesFilter = UsesFilterCheckBox.Checked;
      _CustomersTblData.PredictionDisabled = PredictionDisabledCheckBox.Checked;
      _CustomersTblData.AlwaysSendChkUp = AlwaysSendChkUpCheckBox.Checked;
      _CustomersTblData.NormallyResponds = NormallyRespondsCheckBox.Checked;
      _CustomersTblData.Notes = NotesTextBox.Text;

      return _CustomersTblData;
    }

    void UpdateRecord()
    {
      CustomersTbl _ctd = new CustomersTbl();
      string resultStr = (_ctd.UpdateCustomer(GetDataFromForm(), Convert.ToInt64(CompanyIDLabel.Text)));
      if (resultStr == "")
        ltrlStatus.Text = "Record Updated";
      else
        ltrlStatus.Text = resultStr;
    }
    void ReturnToPrevPage() { ReturnToPrevPage(false); }

    void ReturnToPrevPage(bool GoToCustomers)
    {
      if ((GoToCustomers) || (String.IsNullOrWhiteSpace(prevPage)))
        Response.Redirect("~/Pages/Customers.aspx");
      else
        Response.Redirect(prevPage);
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
      UpdateRecord();
    }
    protected void btnUpdateAndReturn_Click(object sender, EventArgs e)
    {
      UpdateRecord();
      ReturnToPrevPage();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
      ReturnToPrevPage();
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
      string _ErrorStr = String.Empty;
      CustomersTbl _customerData = GetDataFromForm();
      CustomersTbl _ctd = new CustomersTbl();
      //      ClientScriptManager _csm = Page.ClientScript;

      if (_ctd.InsertCustomer(_customerData, ref _ErrorStr))
      {
        //        if (_csm.IsClientScriptBlockRegistered("alert"))

        //        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Customer Added!')", true);
        //        _csm.RegisterStartupScript(this.GetType(), "InsertMessage", "showInsert('Customer " + _customerData.CompanyName + " has been added');");
        ltrlStatus.Text = "Customer Added";
        string _ScriptToRun = "redirect('" + String.Format("{0}?CompanyName={1}", Page.ResolveUrl("~/Pages/Customers.aspx"), _customerData.CompanyName) + "');";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CustomerInserted", _ScriptToRun, true);

        // return to customer list
//         ReturnToPrevPage(true);
        
        // ClientScript.RegisterStartupScript(this.GetType(), "somekey", "redirect('" + String.Format("~/Pages/Customers.aspx?CompanyName={0}", _customerData.CompanyName) + "');");
      }
      else
      {
//        _msgBox.ShowSuccess("Error " + _ErrorStr + ". Customer not added.");

        ltrlStatus.Text = "ERROR: " + _ErrorStr;
      }
      
    }
    protected void ServerButton_Click(object sender, EventArgs e)
    {
      ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
    }

    protected void btnAddLasOrder_Click(object sender, EventArgs e)
    {
      string _AddLastURL = string.Format("~/Pages/NewOrderDetail.aspx?" + NewOrderDetail.CONST_URL_REQUEST_CUSTOMERID + "={0}&LastOrder=Y", CompanyIDLabel.Text);

      Response.Redirect(_AddLastURL);
    }

    protected void btnForceNext_Click(object sender, EventArgs e)
    {
      QOnT.classes.TrackerTools _TTools = new QOnT.classes.TrackerTools();
      DateTime _dt = _TTools.GetClosestNextRoastDate(DateTime.Now.AddDays(14)); // add a fortnight;
      ClientUsageTbl _ClientUsage = new ClientUsageTbl();

      _ClientUsage.ForceNextCoffeeDate(_dt.AddDays(3), Convert.ToInt64(CompanyIDLabel.Text));

      CustomersTbl _CustomerTbl = new CustomersTbl();
      _CustomerTbl.IncrementReminderCount(Convert.ToInt64(CompanyIDLabel.Text));

      // rebind the data for this client
//      dsCustomerUsage.Update();
      dgCustomerUsage.DataBind();

      string _ScriptToRun = "showMessage('" + String.Format("{0} force to skip a week of prediction", CompanyNameTextBox.Text) + "');";
      ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CustomerInserted", _ScriptToRun, true);
    }

    protected void btnRecalcAverage_Click(object sender, EventArgs e)
    {
      QOnT.classes.GeneralTrackerDbTools _TrackerDbTools = new GeneralTrackerDbTools();

      _TrackerDbTools.CalcAndSaveNextRequiredDates(Convert.ToInt64(CompanyIDLabel.Text));
      dgCustomerUsage.DataBind();
      upnlNextItems.Update();

      string _ScriptToRun = "showMessage('" + String.Format("{0} average calculations updated", CompanyNameTextBox.Text) + "');";
      ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CustomerAverageCalcDone", _ScriptToRun, true);
    }
    //public void odsItemUsage_OnSelected(object source, ObjectDataSourceStatusEventArgs e)
    //{
    //  if (e.ReturnValue != null)
    //  {
    //    XmlSerializer _xmlItemLine = new XmlSerializer(typeof(ItemTypeTbl));
    //    StringBuilder _sb = new StringBuilder();
    //    XmlWriter _writer = XmlWriter.Create(_sb);
    //    _xmlItemLine.Serialize(_writer, e.ReturnValue);
    //    _writer.Close();

    //    ViewState["OriginalImetLine"] = _sb.ToString();
    //  }
    //}
    //public void odsItemUsage_OnUpdating(object source, ObjectDataSourceMethodEventArgs e)
    //{
    //  XmlSerializer _xmlItemLine = new XmlSerializer(typeof(ItemTypeTbl));
    //  String _xmlData = ViewState["OriginalImetLine"].ToString();
    //  XmlReader _reader = XmlReader.Create(new StringReader(_xmlData));
    //  ItemTypeTbl _originalItem = (ItemTypeTbl)_xmlItemLine.Deserialize(_reader);
    //  _reader.Close();

    //  e.InputParameters.Add("ItemUsageLine", _originalItem);
    //  e.InputParameters.Add("OriginalClientUsageLineNo", _originalItem.ClientUsageLineNo);


    //}
  }
}