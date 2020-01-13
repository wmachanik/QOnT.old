/// --------------------------------------------------------------------------------------
/// Class to handle data from the Company / Contact Table and associated classes
/// --------------------------------------------------------------------------------------
/// Built from:
/// SELECT CustomerID, CompanyName, ContactTitle, ContactFirstName, ContactLastName, ContactAltFirstName, ContactAltLastName, Department
///        BillingAddress, City, StateOrProvince, PostalCode, [Country/Region], PhoneNumber, Extension, FaxNumber, CellNumber, EmailAddress,
///        AltEmailAddress, ContractNo, CustomerTypeID, EquipType, CoffeePreference, PriPrefQty, PrefPrepTypeID, PrefPackagingID, 
///        SecondaryPreference, SecPrefQty, TypicallySecToo, PreferedAgent, SalesAgentID, MachineSN, UsesFilter, autofulfill, enabled, 
///        PredictionDisabled, AlwaysSendChkUp, NormallyResponds, ReminderCount, Notes
/// ---------------------------------------------------
/// Assoicated tables are the "ID" fields except for  CustomerID which it the unique identifier for this class

using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections.Generic;


namespace QOnT.classes
{
  public class CustomerData
  {
    long _CustomerID; 
    string _CompanyName, _ContactTitle, _ContactFirstName, _ContactLastName, _ContactAltFirstName, _ContactAltLastName;
    string _Department, _BillingAddress;
    long _City;
    string _StateOrProvince, _PostalCode, _Region, _PhoneNumber, _Extension, _FaxNumber, _CellNumber, _EmailAddress, _AltEmailAddress, _ContractNo;
    long _CustomerTypeID, _EquipType, _CoffeePreference;
    double _PriPrefQty;
    long _PrefPrepTypeID, _PrefPackagingID, _SecondaryPreference;
    double _SecPrefQty;
    bool _TypicallySecToo;
    long _PreferedAgent, _SalesAgentID;
    string _MachineSN;
    bool _UsesFilter, _autofulfill, _enabled, _PredictionDisabled, _AlwaysSendChkUp, _NormallyResponds;
    int _ReminderCount;
    string _Notes;

    public CustomerData()
    {
      _CustomerID = 0;
      _CompanyName = _ContactTitle =  _ContactFirstName =  _ContactLastName = _ContactAltFirstName = _ContactAltLastName = _Department = _BillingAddress = "";
     _City = 0;
      _StateOrProvince = _PostalCode = _Region = _PhoneNumber = _Extension = _FaxNumber = _CellNumber = _EmailAddress = _AltEmailAddress = _ContractNo = "";
      _CustomerTypeID = _EquipType = _CoffeePreference = 0;
      _PriPrefQty = 0.0;
      _PrefPrepTypeID = _PrefPackagingID = _SecondaryPreference = 0;
      _SecPrefQty = 0.0;
      _TypicallySecToo = false;
      _PreferedAgent =_SalesAgentID = 0;
      _MachineSN = "";
      _UsesFilter = _autofulfill = _enabled = _PredictionDisabled = _AlwaysSendChkUp = _NormallyResponds = false;
      _ReminderCount = 0;
      _Notes = "";
    }

    public long CustomerID { get {return _CustomerID;} set { _CustomerID = value; } }
    public string CompanyName { get {return _CompanyName; } set { _CompanyName = value; } } 
    public string ContactTitle { get {return _ContactTitle; } set { _ContactTitle = value; } }
    public string ContactFirstName { get {return _ContactFirstName; } set { _ContactFirstName = value; } }
    public string ContactLastName { get { return _ContactLastName; } set { _ContactLastName = value; } }
    public string ContactAltFirstName { get { return _ContactAltFirstName; } set { _ContactAltFirstName = value; } }
    public string ContactAltLastName { get { return _ContactAltLastName; } set { _ContactAltLastName = value; } }
    public string Department { get { return _Department; } set { _Department = value; } }
    public string BillingAddress { get { return _BillingAddress; } set { _BillingAddress = value; } }
    public long City { get { return _City; } set { _City = value; }}
    public string StateOrProvince { get { return _StateOrProvince; } set { _StateOrProvince = value; } }
    public string PostalCode { get { return _PostalCode; } set { _PostalCode = value; } }
    public string Region { get { return _Region; } set { _Region = value;} }
    public string PhoneNumber { get { return _PhoneNumber; } set { _PhoneNumber = value; } } 
    public string Extension { get { return _Extension; } set { _Extension = value; } } 
    public string FaxNumber { get { return _FaxNumber; } set { _FaxNumber = value; } } 
    public string CellNumber { get { return _CellNumber; } set { _CellNumber = value; } } 
    public string EmailAddress { get { return _EmailAddress; } set { _EmailAddress = value; } }
    public string AltEmailAddress { get { return _AltEmailAddress; } set { _AltEmailAddress = value; } }
    public string ContractNo { get { return _ContractNo; } set { _ContractNo = value; } }
    public long CustomerTypeID { get { return _CustomerTypeID; } set { _CustomerTypeID = value; } }
    public long EquipType { get { return _EquipType; } set { _EquipType = value; } }
    public long CoffeePreference { get { return _CoffeePreference; } set { _CoffeePreference = value; } }
    public double PriPrefQty { get { return _PriPrefQty; } set { _PriPrefQty = value; } }
    public long PrefPrepTypeID { get { return _PrefPrepTypeID; } set { _PrefPrepTypeID = value; } }
    public long PrefPackagingID { get { return _PrefPackagingID; } set { _PrefPackagingID = value; } }
    public long SecondaryPreference { get { return _SecondaryPreference; } set { _SecondaryPreference= value; } }
    public double SecPrefQty { get { return _SecPrefQty; } set { _SecPrefQty = value; } } 
    public bool TypicallySecToo  { get { return _TypicallySecToo; } set { _TypicallySecToo = value; } }
    public long PreferedAgent { get { return _PreferedAgent; } set { _PreferedAgent = value; } }
    public long SalesAgentID  { get { return _SalesAgentID ; } set { _SalesAgentID = value; } }
    public string MachineSN { get { return _MachineSN; } set { _MachineSN = value; } }
    public bool UsesFilter { get { return _UsesFilter; } set { _UsesFilter = value; } }
    public bool autofulfill { get { return _autofulfill; } set { _autofulfill = value; } }
    public bool enabled  { get { return _enabled; } set { _enabled = value; } }
    public bool PredictionDisabled { get { return _PredictionDisabled; } set { _PredictionDisabled = value; } }
    public bool AlwaysSendChkUp { get { return _AlwaysSendChkUp; } set { _AlwaysSendChkUp = value; } }
    public bool NormallyResponds { get { return _NormallyResponds ; } set { _NormallyResponds = value; } }
    public int ReminderCount { get { return _ReminderCount; } set { _ReminderCount = value; } }
    public string Notes  { get { return _Notes; } set { _Notes = value; } }

    // list of items taken and list of deliveries?
  }

  public class CustomerDAL
  {
    const string CONST_CONSTRING = "Tracker08ConnectionString";

    public string GetConnectionString()
    {
      // Initialize data source. Use "Tracker08" connection string from configuration.

      if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
          ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
      {
        throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
                            "must exist in the <connectionStrings> configuration section for the application.");
      }
      
      return ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;
    }


    const string CONST_SELECTCUSTOMERS = "SELECT CustomerID, CompanyName, ContactTitle, ContactFirstName, ContactLastName, ContactAltFirstName, " +
      "ContactAltLastName, Department, BillingAddress, City, StateOrProvince, PostalCode, [Country/Region] AS Region, PhoneNumber, Extension, FaxNumber, " +
      "CellNumber, EmailAddress, AltEmailAddress, ContractNo, CustomerTypeID, EquipType, CoffeePreference, PriPrefQty, PrefPrepTypeID, " +
      "PrefPackagingID, SecondaryPreference, SecPrefQty, TypicallySecToo, PreferedAgent, SalesAgentID, MachineSN, UsesFilter, autofulfill, enabled," +
      "PredictionDisabled, AlwaysSendChkUp, NormallyResponds, ReminderCount, Notes FROM CustomersTbl"; 

    public static List<CustomerData> GetAllCustomers(string SortBy)
    {
      List<CustomerData> _ListCustomers = new List<CustomerData>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        
        string _sqlCmd = CONST_SELECTCUSTOMERS;

        if (!String.IsNullOrEmpty(SortBy))
        {
          _sqlCmd += " ORDER BY " + SortBy;
        }
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);

        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          CustomerData _CustomerData = new CustomerData();

          _CustomerData.CustomerID = Convert.ToInt64(_DataReader["CustomerID"]);
          _CustomerData.CompanyName = (_DataReader["CompanyName"] == DBNull.Value) ? "" : _DataReader["CompanyName"].ToString();
          _CustomerData.ContactTitle = (_DataReader["ContactTitle"] == DBNull.Value) ? "" : _DataReader["ContactTitle"].ToString();
          _CustomerData.ContactFirstName = (_DataReader["ContactFirstName"] == DBNull.Value) ? "" : _DataReader["ContactFirstName"].ToString();
          _CustomerData.ContactLastName = (_DataReader["ContactLastName"] == DBNull.Value) ? "" : _DataReader["ContactLastName"].ToString();
          _CustomerData.ContactAltFirstName = (_DataReader["ContactAltFirstName"] == DBNull.Value) ? "" : _DataReader["ContactAltFirstName"].ToString();
          _CustomerData.ContactAltLastName = (_DataReader["ContactAltLastName"] == DBNull.Value) ? "" : _DataReader["ContactAltLastName"].ToString();
          _CustomerData.Department = (_DataReader["Department"] == DBNull.Value) ? "" : _DataReader["Department"].ToString();
          _CustomerData.BillingAddress = (_DataReader["BillingAddress"] == DBNull.Value) ? "" : _DataReader["BillingAddress"].ToString();
          _CustomerData.City = (_DataReader["City"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["City"]);
          _CustomerData.StateOrProvince = (_DataReader["StateOrProvince"] == DBNull.Value) ? "" : _DataReader["StateOrProvince"].ToString();
          _CustomerData.PostalCode = (_DataReader["PostalCode"] == DBNull.Value) ? "" : _DataReader["PostalCode"].ToString();
          _CustomerData.Region = (_DataReader["Region"] == DBNull.Value) ? "" : _DataReader["Region"].ToString();
          _CustomerData.PhoneNumber = (_DataReader["PhoneNumber"] == DBNull.Value) ? "" : _DataReader["PhoneNumber"].ToString();
          _CustomerData.Extension = (_DataReader["Extension"] == DBNull.Value) ? "" : _DataReader["Extension"].ToString();
          _CustomerData.FaxNumber = (_DataReader["FaxNumber"] == DBNull.Value) ? "" : _DataReader["FaxNumber"].ToString();
          _CustomerData.CellNumber = (_DataReader["CellNumber"] == DBNull.Value) ? "" : _DataReader["CellNumber"].ToString();
          _CustomerData.EmailAddress = (_DataReader["EmailAddress"] == DBNull.Value) ? "" : _DataReader["EmailAddress"].ToString();
          _CustomerData.AltEmailAddress = (_DataReader["AltEmailAddress"] == DBNull.Value) ? "" : _DataReader["AltEmailAddress"].ToString();
          _CustomerData.ContractNo = (_DataReader["ContractNo"] == DBNull.Value) ? "" :  _DataReader["ContractNo"].ToString();
          _CustomerData.CustomerTypeID = (_DataReader["CustomerTypeID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["CustomerTypeID"]);
          _CustomerData.EquipType = (_DataReader["EquipType"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["EquipType"]);
          _CustomerData.CoffeePreference = (_DataReader["CoffeePreference"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["CoffeePreference"]);
          _CustomerData.PriPrefQty = (_DataReader["PriPrefQty"] == DBNull.Value) ? 0 : Convert.ToDouble(_DataReader["PriPrefQty"]);
          _CustomerData.PrefPrepTypeID = (_DataReader["PrefPrepTypeID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["PrefPrepTypeID"]);
          _CustomerData.PrefPackagingID = (_DataReader["PrefPackagingID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["PrefPackagingID"]);
          _CustomerData.SecondaryPreference = (_DataReader["SecondaryPreference"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["SecondaryPreference"]);
          _CustomerData.SecPrefQty = (_DataReader["SecPrefQty"] == DBNull.Value) ? 0 : Convert.ToDouble(_DataReader["SecPrefQty"]);
          _CustomerData.TypicallySecToo = (_DataReader["TypicallySecToo"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["TypicallySecToo"]);
          _CustomerData.PreferedAgent = (_DataReader["PreferedAgent"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["PreferedAgent"]);
          _CustomerData.SalesAgentID = (_DataReader["SalesAgentID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["SalesAgentID"]);
          _CustomerData.MachineSN = (_DataReader["MachineSN"] == DBNull.Value) ? "" : _DataReader["MachineSN"].ToString();
          _CustomerData.UsesFilter = (_DataReader["UsesFilter"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["UsesFilter"]);
          _CustomerData.autofulfill = (_DataReader["autofulfill"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["autofulfill"]);
          _CustomerData.enabled = (_DataReader["enabled"] == DBNull.Value) ? true : Convert.ToBoolean(_DataReader["enabled"]);
          _CustomerData.PredictionDisabled = (_DataReader["PredictionDisabled"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["PredictionDisabled"]);
          _CustomerData.AlwaysSendChkUp = (_DataReader["AlwaysSendChkUp"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["AlwaysSendChkUp"]);
          _CustomerData.NormallyResponds = (_DataReader["NormallyResponds"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["NormallyResponds"]);
          _CustomerData.ReminderCount = (_DataReader["ReminderCount"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ReminderCount"]);
          _CustomerData.Notes = (_DataReader["ContactTitle"] == DBNull.Value) ? "" :  _DataReader["Notes"].ToString();

          // add this data to the list
          _ListCustomers.Add(_CustomerData);
        }

        return _ListCustomers;
      }

    }
  }
}