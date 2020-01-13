using System;
using System.Collections.Generic;
using System.IO;
using TrackerDotNet.classes;
using TrackerDotNet.control;

namespace TrackerDotNet.Tools
{
  public partial class SystemTools : System.Web.UI.Page
  {

    const int CONST_MINMONTHS = 3;   // minimum number of months a contact can be a client until we can make a call

    StreamWriter _ColsStream;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    class ContactsUpdated
    {
      string _ContactName;
      int _ContactTypeID;
      int _origContactTypeID;
      bool _PredictionDisabled;


      public ContactsUpdated()
      {
        _ContactName = String.Empty;
        _ContactTypeID = _origContactTypeID = int.MinValue;
        _PredictionDisabled = false;
      }

      public string ContactName { get { return _ContactName; } set { _ContactName = value; } }
      public int ContactTypeID { get { return _ContactTypeID; } set { _ContactTypeID = value; } }
      public int origContactTypeID { get { return _origContactTypeID; } set { _origContactTypeID = value; } }
      public bool PredictionDisabled { get { return _PredictionDisabled; } set { _PredictionDisabled = value; } }
    }

    List<int> GetAllCoffeeClientTypes()
    {
      List<int> _CoffeeClientTypes = new List<int> {CustomerTypeTbl.CONST_COFFEE_ONLY,
        CustomerTypeTbl.CONST_COFFEEANDMAINT,
        CustomerTypeTbl.CONST_GREEN_COFFEE_ONLY,
        CustomerTypeTbl.CONST_OUTRIGHT_PURCHASE,
        CustomerTypeTbl.CONST_RENTAL,
        CustomerTypeTbl.CONST_SERVICE_CONTRACT
      };
      _CoffeeClientTypes.Sort();   // sort it in order

      return _CoffeeClientTypes;
    }
    List<int> GetAllServiceOnlyClientTypes()
    {
      List<int> _ServiceOnlyClientTypes = new List<int> { CustomerTypeTbl.CONST_SERVICE_ONLY };
      _ServiceOnlyClientTypes.Sort();   // sort it in order

      return _ServiceOnlyClientTypes;
    }

    ContactsUpdated CheckCoffeeCustomerIsOne(ContactType pCustomer, ContactsUpdated pContact)
    {
      // if the customer type is set not to be reminded about coffee  
      ClientUsageLinesTbl _LatestUsageData = new ClientUsageLinesTbl(); 
      ItemUsageTbl _LatestItemData = new ItemUsageTbl();
      DateTime _InstallDate = DateTime.MinValue;
      _LatestUsageData = _LatestUsageData.GetLatestUsageData(pCustomer.CustomerID, TrackerDotNet.classes.TrackerTools.CONST_SERVTYPECOFFEE);
      if (_LatestUsageData != null)
      {
        _InstallDate = _LatestUsageData.GetCustomerInstallDate(pCustomer.CustomerID);
        if (_LatestUsageData.LineDate <= _InstallDate)
        {
          // they have not ordered since the first time, so are they a service client?
          _LatestItemData = _LatestItemData.GetLastMaintenanceItem(pCustomer.CustomerID);
          if (_LatestItemData == null)
          { // they have not ordered since the first order so set prediction to disabled
            pContact.ContactTypeID = CustomerTypeTbl.CONST_INFO_ONLY;
            pContact.PredictionDisabled = true;
          }
          else    // they have ordered other stuff from us but not coffee
          {
            pContact.ContactTypeID = CustomerTypeTbl.CONST_SERVICE_ONLY;   // set it to the first time of service ony client
          }
        }
        else
        {
          // they have ordered coffee have they ordered anything else, and they have been a client for long enough?
          if (_InstallDate.AddMonths(CONST_MINMONTHS) <= _LatestUsageData.LineDate)
          {
            // we have a client that has been ordering for a while if they have not ordered maint stuff then sert them as coffee only or green only
            _LatestItemData = _LatestItemData.GetLastMaintenanceItem(pCustomer.CustomerID);
            if (_LatestItemData == null)
            {
              pContact.ContactTypeID = CustomerTypeTbl.CONST_COFFEE_ONLY;
            }
            else
            {   // they have ordered other stuff if they are set to coffee only set them to something else
              if (pContact.ContactTypeID == CustomerTypeTbl.CONST_COFFEE_ONLY)
              {
                pContact.ContactTypeID = CustomerTypeTbl.CONST_COFFEEANDMAINT; // not they will be reminded of maintenance too
              }
            }
          }
          else
          {
          }
        }
      }
      else
      {
        // they have no coffee in thier list are they a none coffee client?
        _LatestItemData = _LatestItemData.GetLastMaintenanceItem(pCustomer.CustomerID);
        if (_LatestItemData != null)
        {
          // they have ordered maitenance stuff but not coffee stuff before
          pContact.ContactTypeID = CustomerTypeTbl.CONST_SERVICE_ONLY;   // set it to the first time of service ony client
        }
        else
          pContact.ContactTypeID = CustomerTypeTbl.CONST_INFO_ONLY;   // nothing has been ordered so set as prediction
      }
      return pContact;
      //_ColsStream.WriteLine("CheckCoffeeCustomerIsOne: 16");

    }
    ContactsUpdated CheckNoneCoffeeCustomer(ContactType pCustomer, ContactsUpdated pContact)
    {
      // customer is a none coffee customer check if this is correct
      ClientUsageLinesTbl _LatestUsageData = new ClientUsageLinesTbl(); 
      ItemUsageTbl _LatestItemData = new ItemUsageTbl();
      // the client is set to not have coffee tracked but has taken coffee check if they have taken anything else
      List<ItemUsageTbl> _LatestCoffeeItems = _LatestItemData.GetLastItemsUsed(pCustomer.CustomerID, TrackerTools.CONST_SERVTYPECOFFEE);
      _LatestItemData = _LatestItemData.GetLastMaintenanceItem(pCustomer.CustomerID);
      if (_LatestCoffeeItems.Count > 0)
      { // they ordered coffee from us but are marked as a none coffee client
        DateTime _InstallDate = _LatestUsageData.GetCustomerInstallDate(pCustomer.CustomerID);
        if (_LatestUsageData.LineDate >= _InstallDate)
        {
          // there is only coffee for the installation date
          if (_LatestItemData == null)
            pContact.ContactTypeID = CustomerTypeTbl.CONST_INFO_ONLY;  // they have no coffee or maint items so they are info only
        }
        else
        {
          // they have take coffee past the install date
          if (_LatestItemData == null)
            pContact.ContactTypeID = CustomerTypeTbl.CONST_COFFEE_ONLY;  // they have no maint items so they are coffee only
        }
        // else they should be set correctly
      }
      else
      {
        if (_LatestItemData == null)
          pContact.ContactTypeID = CustomerTypeTbl.CONST_INFO_ONLY;  // they have no coffee or main items so they are info only
      }

      _LatestCoffeeItems.Clear();
      return pContact;
    }
    protected void btnSetClientType_Click(object sender, EventArgs e)
    {
      pnlSetClinetType.Visible = true;
      gvCustomerTypes.Visible = true;

      List<ContactsUpdated> _ContactsUpdated = new List<ContactsUpdated>();

      ClientUsageLinesTbl _ClientUsageLines = new ClientUsageLinesTbl();
      ItemUsageTbl _ItemUsageTbl = new ItemUsageTbl();
      ContactType _Customer = new ContactType();

      string _fName = "c:\\temp\\" + String.Format("SetClientType_{0:ddMMyyyy hh mm}.txt", DateTime.Now);
      _ColsStream = new StreamWriter(_fName, false);  // create new file
      _ColsStream.WriteLine("Task, Company Name, origType, newType, PredDisabled");
      List<ContactType> _Customers = null;
      int i = 0;
      try
      {
        _Customers = _Customer.GetAllContacts("CompanyName");   // get all client sort by Company name
//        _Customers.RemoveAll(x => !x.IsEnabled);   // delete all the disabled clients

        List<int> _CoffeeClients = GetAllCoffeeClientTypes();
        List<int> _ServiceOnlyClient = GetAllServiceOnlyClientTypes();

        // for each client check if they have ordered stuff and if so then set them as a particular client

        while (i < _Customers.Count)
        {
          ContactsUpdated _Contact = new ContactsUpdated();
          // only if they are enabled and not set to info only
          if (_Customers[i].CustomerTypeID != CustomerTypeTbl.CONST_INFO_ONLY)
          {
            _Contact.ContactName = _Customers[i].CompanyName;
            _Contact.ContactTypeID = _Customers[i].CustomerTypeID;
            _Contact.origContactTypeID = _Customers[i].CustomerTypeID;
            _Contact.PredictionDisabled = _Customers[i].PredictionDisabled;

            if (_Contact.ContactTypeID == 0)   // type not set then assume coffee client.
              _Contact.ContactTypeID = CustomerTypeTbl.CONST_COFFEE_ONLY;
            // if they are currently marked as coffee client then check if they have ordered since their install date and with in the last year
            if (_CoffeeClients.Contains(_Contact.ContactTypeID))
            {
              _Contact = CheckCoffeeCustomerIsOne(_Customers[i], _Contact);
            }
            else      // customer is set to only be info or something so lets check if that is true
            {
             _Contact = CheckNoneCoffeeCustomer(_Customers[i], _Contact);
            }
            /// Has it changed? is fo update
            if (!_Contact.ContactTypeID.Equals(_Contact.origContactTypeID))
            {
              // copy the values that could have change across since C cannot clone with out a class def so we use the temp class instead
              _Customers[i].CustomerTypeID = _Contact.ContactTypeID;
              _Customers[i].PredictionDisabled = _Contact.PredictionDisabled;
              string _Result = _Customers[i].UpdateContact(_Customers[i]);
              _ContactsUpdated.Add(_Contact);
              if (String.IsNullOrEmpty(_Result))
                _ColsStream.WriteLine("Added {0}-{1}: {2}, {3}, {4}, {5}", i, _ContactsUpdated.Count, _Contact.ContactName, _Contact.origContactTypeID, _Contact.ContactTypeID, _Contact.PredictionDisabled);
              else
                _ColsStream.WriteLine("Error {0} Adding: {1}, {2}, {3}, {4}, {5}", _Result, i, _Result, _Contact.ContactName, _Contact.origContactTypeID, _Contact.ContactTypeID, _Contact.PredictionDisabled);
            }
          }
          i++; 
        }
      }
      catch (Exception _ex)
      {
        string _errStr = _ex.Message;
        
        TrackerTools _TT = new TrackerTools();
        string _TTErr = _TT.GetTrackerSessionErrorString();
        if (!String.IsNullOrWhiteSpace(_TTErr))
          _errStr += " TTError: " + _TTErr;
        
        showMessageBox _exMsg = new showMessageBox(this.Page, "Error", _errStr);
        if (_Customers != null)
          _ColsStream.WriteLine("ERROR AT: {0}, Name: {1}, ID: {2}, Pred: {3}", i, _Customers[i].CompanyName, _Customers[i].CustomerTypeID, _Customers[i].PredictionDisabled);
        else
          _ColsStream.WriteLine("null customers");
        _ColsStream.WriteLine("Error:" + _errStr);
        throw;
      }
      finally
      {
        _ColsStream.Close();
      }

      showMessageBox _sMsg = new showMessageBox(this.Page, "Info", String.Format("A Total of {0}, contacts were updated", _ContactsUpdated.Count));

      ltrlStatus.Text = String.Format("A Total of {0}, contacts were updated", _ContactsUpdated.Count);
      ltrlStatus.Visible = true;
      ResultsTitleLabel.Text = "Set client type results";
      gvResults.DataSource = _ContactsUpdated;
      gvResults.DataBind();
      // upnlSystemToolsButtons.Update();
    }

    protected void btnResetPrepDates_Click(object sender, EventArgs e)
    {
      pnlResetPrepDate.Visible = true;

      TrackerTools _TTools = new TrackerTools();
      _TTools.SetNextRoastDateByCity();

      sdsCityPrepDates.DataBind();
      gvCityPrepDates.DataBind();

    }

    protected void btnCreateUpdateLogTables_Click(object sender, EventArgs e)
    {
      TrackerTools _TT = new TrackerTools();
      _TT.ClearTrackerSessionErrorString();
      TrackerDb _TDB = new TrackerDb();
      // create LogTbl if it does not exist

      _TDB.CreateIfDoesNotExists(TrackerDb.SQLTABLENAME_LOGTBL);
      ltrlStatus.Visible = true;
      ltrlStatus.Text = "Log table checked";
      if (_TT.IsTrackerSessionErrorString())
      {
        ltrlStatus.Text += " - Error: " + _TT.GetTrackerSessionErrorString();
        _TT.ClearTrackerSessionErrorString();
      }

      // list all the users that are not added
      PersonsTbl _PersonsTbl = new PersonsTbl();
      List<string> _SecurityUsersNotInPeopleTbl = _PersonsTbl.SecurityUsersNotInPeopleTbl();

      pnlSetClinetType.Visible = true;
      gvCustomerTypes.Visible = false;
      gvResults.DataSource = _SecurityUsersNotInPeopleTbl;
      ResultsTitleLabel.Text = "Security Users not in People Table ";
      if (_TT.IsTrackerSessionErrorString())
      {
        ltrlStatus.Text += " - Error: " + _TT.GetTrackerSessionErrorString();
        _TT.ClearTrackerSessionErrorString();
      }

      gvResults.DataBind();

      // create SectionTypesTbl if it does not exist
      if (_TDB.CreateIfDoesNotExists(TrackerDb.SQLTABLENAME_SECTIONTYPESTBL))
      {
        ltrlStatus.Text += "; Section Types table checked";
        if (_TT.IsTrackerSessionErrorString())
        {
          ltrlStatus.Text += " - Error: " + _TT.GetTrackerSessionErrorString();
          _TT.ClearTrackerSessionErrorString();
        }
        // add sections that do
        SectionTypesTbl _SectionTypeTbl = new SectionTypesTbl();
        if (_SectionTypeTbl.InsertDefaultSections())
          ltrlStatus.Text += " - default sections added.";
        if (_TT.IsTrackerSessionErrorString())
        {
          ltrlStatus.Text += " - Error: " + _TT.GetTrackerSessionErrorString();
          _TT.ClearTrackerSessionErrorString();
        }
      }
      // create TransactionTypesTbl if it does not exists
      if (_TDB.CreateIfDoesNotExists(TrackerDb.SQLTABLENAME_TRANSACTIONTYPESTBL))
      {
        ltrlStatus.Text += "; Transaction Types table checked";
        if (_TT.IsTrackerSessionErrorString())
        {
          ltrlStatus.Text += " - Error: " + _TT.GetTrackerSessionErrorString();
          _TT.ClearTrackerSessionErrorString();
        }
        // add Transactions that do
        TransactionTypesTbl _TransactionTypeTbl = new TransactionTypesTbl();
        if (_TransactionTypeTbl.InsertDefaultTransactions())
          ltrlStatus.Text += " - default Transactions added.";
        if (_TT.IsTrackerSessionErrorString())
        {
          ltrlStatus.Text += " - Error: " + _TT.GetTrackerSessionErrorString();
          _TT.ClearTrackerSessionErrorString();
        }
      }

      _TDB.Close();
    }
  }
}