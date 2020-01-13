/// = Class to implete comple table update for CustomerTbl
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Configuration;
using QOnT.classes;

namespace QOnT.control
{
  public class CustomersTbl
  {
    // internal variable declarations
#region  InternalVars
    private long _CustomerID;
    private string _CompanyName;
    private string _ContactTitle;
    private string _ContactFirstName;
    private string _ContactLastName;
    private string _ContactAltFirstName;
    private string _ContactAltLastName;
    private string _Department;
    private string _BillingAddress;
    private int _City;
    private string _Province;
    private string _PostalCode;
    private string _Region;
    private string _PhoneNumber;
    private string _Extension;
    private string _FaxNumber;
    private string _CellNumber;
    private string _EmailAddress;
    private string _AltEmailAddress;
    private string _ContractNo;
    private int _CustomerTypeID;
    private int _EquipType;
    private int _CoffeePreference;
    private double _PriPrefQty;
    private int _PrefPrepTypeID;
    private int _PrefPackagingID;
    private int _SecondaryPreference;
    private double _SecPrefQty;
    private bool _TypicallySecToo;
    private int _PreferedAgent;
    private int _SalesAgentID;
    private string _MachineSN;
    private bool _UsesFilter;
    private bool _autofulfill;
    private bool _enabled;
    private bool _PredictionDisabled;
    private bool _AlwaysSendChkUp;
    private bool _NormallyResponds;
    private int _ReminderCount;
    private string _Notes;
#endregion 

#region classdefinition

    // class definition
    public CustomersTbl()
    {
      _CustomerID = 0;
      _CompanyName = string.Empty;
      _ContactTitle = string.Empty;
      _ContactFirstName = string.Empty;
      _ContactLastName = string.Empty;
      _ContactAltFirstName = string.Empty;
      _ContactAltLastName = string.Empty;
      _Department = string.Empty;
      _BillingAddress = string.Empty;
      _City = 0;
      _Province = string.Empty;
      _PostalCode = string.Empty;
      _Region = string.Empty;
      _PhoneNumber = string.Empty;
      _Extension = string.Empty;
      _FaxNumber = string.Empty;
      _CellNumber = string.Empty;
      _EmailAddress = string.Empty;
      _AltEmailAddress = string.Empty;
      _ContractNo = string.Empty;
      _CustomerTypeID = 0;
      _EquipType = 0;
      _CoffeePreference = 0;
      _PriPrefQty = 0.0;
      _PrefPrepTypeID = 0;
      _PrefPackagingID = 0;
      _SecondaryPreference = 0;
      _SecPrefQty = 0.0;
      _TypicallySecToo = false;
      _PreferedAgent = 0;
      _SalesAgentID = 0;
      _MachineSN = string.Empty;
      _UsesFilter = false;
      _autofulfill = false;
      _enabled = false;
      _PredictionDisabled = false;
      _AlwaysSendChkUp = false;
      _NormallyResponds = false;
      _ReminderCount = 0;
      _Notes = string.Empty;
    }
#endregion

#region public vars
    // get and sets of public
    public long CustomerID { get { return _CustomerID;}  set { _CustomerID = value;} }
    public string CompanyName { get { return _CompanyName;}  set { _CompanyName = value;} }
    public string ContactTitle { get { return _ContactTitle;}  set { _ContactTitle = value;} }
    public string ContactFirstName { get { return _ContactFirstName;}  set { _ContactFirstName = value;} }
    public string ContactLastName { get { return _ContactLastName;}  set { _ContactLastName = value;} }
    public string ContactAltFirstName { get { return _ContactAltFirstName;}  set { _ContactAltFirstName = value;} }
    public string ContactAltLastName { get { return _ContactAltLastName;}  set { _ContactAltLastName = value;} }
    public string Department { get { return _Department;}  set { _Department = value;} }
    public string BillingAddress { get { return _BillingAddress;}  set { _BillingAddress = value;} }
    public int City { get { return _City;}  set { _City = value;} }
    public string Province { get { return _Province;}  set { _Province = value;} }
    public string PostalCode { get { return _PostalCode;}  set { _PostalCode = value;} }
    public string Region { get { return _Region;}  set { _Region = value;} }
    public string PhoneNumber { get { return _PhoneNumber;}  set { _PhoneNumber = value;} }
    public string Extension { get { return _Extension;}  set { _Extension = value;} }
    public string FaxNumber { get { return _FaxNumber;}  set { _FaxNumber = value;} }
    public string CellNumber { get { return _CellNumber;}  set { _CellNumber = value;} }
    public string EmailAddress { get { return _EmailAddress;}  set { _EmailAddress = value;} }
    public string AltEmailAddress { get { return _AltEmailAddress;}  set { _AltEmailAddress = value;} }
    public string ContractNo { get { return _ContractNo;}  set { _ContractNo = value;} }
    public int CustomerTypeID { get { return _CustomerTypeID;}  set { _CustomerTypeID = value;} }
    public int EquipType { get { return _EquipType;}  set { _EquipType = value;} }
    public int CoffeePreference { get { return _CoffeePreference;}  set { _CoffeePreference = value;} }
    public double PriPrefQty { get { return _PriPrefQty;}  set { _PriPrefQty = value;} }
    public int PrefPrepTypeID { get { return _PrefPrepTypeID;}  set { _PrefPrepTypeID = value;} }
    public int PrefPackagingID { get { return _PrefPackagingID;}  set { _PrefPackagingID = value;} }
    public int SecondaryPreference { get { return _SecondaryPreference;}  set { _SecondaryPreference = value;} }
    public double SecPrefQty { get { return _SecPrefQty;}  set { _SecPrefQty = value;} }
    public bool TypicallySecToo { get { return _TypicallySecToo;}  set { _TypicallySecToo = value;} }
    public int PreferedAgent { get { return _PreferedAgent;}  set { _PreferedAgent = value;} }
    public int SalesAgentID { get { return _SalesAgentID;}  set { _SalesAgentID = value;} }
    public string MachineSN { get { return _MachineSN;}  set { _MachineSN = value;} }
    public bool UsesFilter { get { return _UsesFilter;}  set { _UsesFilter = value;} }
    public bool autofulfill { get { return _autofulfill;}  set { _autofulfill = value;} }
    public bool enabled { get { return _enabled;}  set { _enabled = value;} }
    public bool PredictionDisabled { get { return _PredictionDisabled;}  set { _PredictionDisabled = value;} }
    public bool AlwaysSendChkUp { get { return _AlwaysSendChkUp;}  set { _AlwaysSendChkUp = value;} }
    public bool NormallyResponds { get { return _NormallyResponds;}  set { _NormallyResponds = value;} }
    public int ReminderCount { get { return _ReminderCount;}  set { _ReminderCount = value;} }
    public string Notes { get { return _Notes;}  set { _Notes = value;} }
#endregion


#region ConstantDeclarations
    const int CONST_MAXREMINDERS = 10;

    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_CUSTOMERS_SELECT = "SELECT CustomerID, CompanyName, ContactTitle, ContactFirstName, ContactLastName, ContactAltFirstName," +
                                          " ContactAltLastName, Department, BillingAddress, City, StateOrProvince AS Province, PostalCode, " +
                                          " [Country/Region] AS Region, PhoneNumber, Extension, FaxNumber, CellNumber, EmailAddress, AltEmailAddress, ContractNo," +
                                          " CustomerTypeID, EquipType, CoffeePreference, PriPrefQty, PrefPrepTypeID, PrefPackagingID, " +
                                          " SecondaryPreference, SecPrefQty, TypicallySecToo, PreferedAgent, SalesAgentID, MachineSN, " +
                                          " UsesFilter, autofulfill, enabled, PredictionDisabled, AlwaysSendChkUp, NormallyResponds, " +
                                          " ReminderCount, Notes" +
                                           " FROM CustomersTbl";
    const string CONST_SQL_CUSTOMERS_INSERT = "INSERT INTO CustomersTbl (CompanyName, ContactTitle, ContactFirstName, ContactLastName, ContactAltFirstName," +
                                                             " ContactAltLastName, Department, BillingAddress, City, StateOrProvince," +  //2
                                                             " PostalCode, [Country/Region], PhoneNumber, Extension, FaxNumber," +     // 3
                                                             " CellNumber, EmailAddress, AltEmailAddress, ContractNo, CustomerTypeID," +  // 4
                                                             " EquipType, CoffeePreference, PriPrefQty, PrefPrepTypeID, PrefPackagingID, " + // 5
                                                             " SecondaryPreference, SecPrefQty, TypicallySecToo, PreferedAgent, SalesAgentID," + // 6
                                                             " MachineSN, UsesFilter, autofulfill, enabled, PredictionDisabled, " +  // 7
                                                             " AlwaysSendChkUp, NormallyResponds, ReminderCount, Notes) " + // 8
                                                      "VALUES (?,?,?,?,?, ?,?,?,?,?, ?,?,?,?,?, ?,?,?,?,?, ?,?,?,?,?, ?,?,?,?,?, ?,?,?,?,?, ?,?,?,?)";
                                                            //     1          2          3          4         5           6            7      8

    const string CONST_SQL_CUSTOMERS_UPDATE = "UPDATE CustomersTbl SET CompanyName = ?, ContactTitle = ?, ContactFirstName = ?, ContactLastName = ?, " +
                                      " ContactAltFirstName = ?, ContactAltLastName = ?, Department = ?, BillingAddress = ?, City = ?, " +
                                      " StateOrProvince = ?, PostalCode = ?, [Country/Region] = ?, PhoneNumber = ?, Extension = ?, " +
                                      " FaxNumber = ?, CellNumber = ?, EmailAddress = ?, AltEmailAddress = ?, ContractNo = ?, CustomerTypeID = ?," +
                                      " EquipType = ?, CoffeePreference = ?, PriPrefQty = ?, PrefPrepTypeID = ?, PrefPackagingID = ?, " +
                                      " SecondaryPreference = ?, SecPrefQty = ?, TypicallySecToo = ?, PreferedAgent = ?, SalesAgentID = ?, " +
                                      " MachineSN = ?, UsesFilter = ?, autofulfill = ?, enabled = ?, PredictionDisabled = ?, " +
                                      " AlwaysSendChkUp = ?, NormallyResponds = ?, ReminderCount = ?, Notes = ?" +
                                 " WHERE CustomersTbl.CustomerID = ?";
    const string CONST_SQL_CUSTOMERS_SELECT_REMINDERCOUNT = "SELECT ReminderCount FROM CustomersTbl WHERE CustomersTbl.CustomerID = ?";
    const string CONST_SQL_CUSTOMERS_INC_REMINDERCOUNT = "UPDATE CustomersTbl SET ReminderCount = ReminderCount + 1 WHERE CustomersTbl.CustomerID = ?";
    const string CONST_SQL_CUSTOMERS_DISABLEIFREMINDERTOHIGH = "UPDATE CustomersTbl SET enabled = false WHERE (CustomerID = ?) AND (ReminderCount > ?)";

#endregion

    static CustomersTbl MoveReaderDataToCustomersTblData(OleDbDataReader pDataReader)
    {
      CustomersTbl _CustomersTblData = new CustomersTbl();

      _CustomersTblData.CustomerID = Convert.ToInt64(pDataReader["CustomerID"]);
      _CustomersTblData.CompanyName = (pDataReader["CompanyName"] == DBNull.Value) ? "" : pDataReader["CompanyName"].ToString();
      _CustomersTblData.ContactTitle = (pDataReader["ContactTitle"] == DBNull.Value) ? "" : pDataReader["ContactTitle"].ToString();
      _CustomersTblData.ContactFirstName = (pDataReader["ContactFirstName"] == DBNull.Value) ? "" : pDataReader["ContactFirstName"].ToString();
      _CustomersTblData.ContactLastName = (pDataReader["ContactLastName"] == DBNull.Value) ? "" : pDataReader["ContactLastName"].ToString();
      _CustomersTblData.ContactAltFirstName = (pDataReader["ContactAltFirstName"] == DBNull.Value) ? "" : pDataReader["ContactAltFirstName"].ToString();
      _CustomersTblData.ContactAltLastName = (pDataReader["ContactAltLastName"] == DBNull.Value) ? "" : pDataReader["ContactAltLastName"].ToString();
      _CustomersTblData.Department = (pDataReader["Department"] == DBNull.Value) ? "" : pDataReader["Department"].ToString();
      _CustomersTblData.BillingAddress = (pDataReader["BillingAddress"] == DBNull.Value) ? "" : pDataReader["BillingAddress"].ToString();
      _CustomersTblData.City = (pDataReader["City"] == DBNull.Value) ? 0 : Convert.ToInt32(pDataReader["City"]);
      _CustomersTblData.Province = (pDataReader["Province"] == DBNull.Value) ? "" : pDataReader["Province"].ToString();
      _CustomersTblData.PostalCode = (pDataReader["PostalCode"] == DBNull.Value) ? "" : pDataReader["PostalCode"].ToString();
      _CustomersTblData.Region = (pDataReader["Region"] == DBNull.Value) ? "" : pDataReader["Region"].ToString();
      _CustomersTblData.PhoneNumber = (pDataReader["PhoneNumber"] == DBNull.Value) ? "" : pDataReader["PhoneNumber"].ToString();
      _CustomersTblData.Extension = (pDataReader["Extension"] == DBNull.Value) ? "" : pDataReader["Extension"].ToString();
      _CustomersTblData.FaxNumber = (pDataReader["FaxNumber"] == DBNull.Value) ? "" : pDataReader["FaxNumber"].ToString();
      _CustomersTblData.CellNumber = (pDataReader["CellNumber"] == DBNull.Value) ? "" : pDataReader["CellNumber"].ToString();
      _CustomersTblData.EmailAddress = (pDataReader["EmailAddress"] == DBNull.Value) ? "" : pDataReader["EmailAddress"].ToString();
      _CustomersTblData.AltEmailAddress = (pDataReader["AltEmailAddress"] == DBNull.Value) ? "" : pDataReader["AltEmailAddress"].ToString();
      _CustomersTblData.ContractNo = (pDataReader["ContractNo"] == DBNull.Value) ? "" : pDataReader["ContractNo"].ToString();
      _CustomersTblData.CustomerTypeID = (pDataReader["CustomerTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(pDataReader["CustomerTypeID"]);
      _CustomersTblData.EquipType = (pDataReader["EquipType"] == DBNull.Value) ? 0 : Convert.ToInt32(pDataReader["EquipType"]);
      _CustomersTblData.CoffeePreference = (pDataReader["CoffeePreference"] == DBNull.Value) ? 0 : Convert.ToInt32(pDataReader["CoffeePreference"]);
      _CustomersTblData.PriPrefQty = (pDataReader["PriPrefQty"] == DBNull.Value) ? 1 : Convert.ToDouble(pDataReader["PriPrefQty"]);
      _CustomersTblData.PrefPrepTypeID = (pDataReader["PrefPrepTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(pDataReader["PrefPrepTypeID"]);
      _CustomersTblData.PrefPackagingID = (pDataReader["PrefPackagingID"] == DBNull.Value) ? 0 : Convert.ToInt32(pDataReader["PrefPackagingID"]);
      _CustomersTblData.SecondaryPreference = (pDataReader["SecondaryPreference"] == DBNull.Value) ? 0 : Convert.ToInt32(pDataReader["SecondaryPreference"]);
      _CustomersTblData.SecPrefQty = (pDataReader["SecPrefQty"] == DBNull.Value) ? 1 : Convert.ToDouble(pDataReader["SecPrefQty"]);
      _CustomersTblData.TypicallySecToo = (pDataReader["TypicallySecToo"] == DBNull.Value) ? false : Convert.ToBoolean(pDataReader["TypicallySecToo"]);
      _CustomersTblData.PreferedAgent = (pDataReader["PreferedAgent"] == DBNull.Value) ? 0 : Convert.ToInt32(pDataReader["PreferedAgent"]);
      _CustomersTblData.SalesAgentID = (pDataReader["SalesAgentID"] == DBNull.Value) ? 0 : Convert.ToInt32(pDataReader["SalesAgentID"]);
      _CustomersTblData.MachineSN = (pDataReader["MachineSN"] == DBNull.Value) ? "" : pDataReader["MachineSN"].ToString();
      _CustomersTblData.UsesFilter = (pDataReader["UsesFilter"] == DBNull.Value) ? false : Convert.ToBoolean(pDataReader["UsesFilter"]);
      _CustomersTblData.autofulfill = (pDataReader["autofulfill"] == DBNull.Value) ? false : Convert.ToBoolean(pDataReader["autofulfill"]);
      _CustomersTblData.enabled = (pDataReader["enabled"] == DBNull.Value) ? false : Convert.ToBoolean(pDataReader["enabled"]);
      _CustomersTblData.PredictionDisabled = (pDataReader["PredictionDisabled"] == DBNull.Value) ? false : Convert.ToBoolean(pDataReader["PredictionDisabled"]);
      _CustomersTblData.AlwaysSendChkUp = (pDataReader["AlwaysSendChkUp"] == DBNull.Value) ? false : Convert.ToBoolean(pDataReader["AlwaysSendChkUp"]);
      _CustomersTblData.NormallyResponds = (pDataReader["NormallyResponds"] == DBNull.Value) ? false : Convert.ToBoolean(pDataReader["NormallyResponds"]);
      _CustomersTblData.ReminderCount = (pDataReader["ReminderCount"] == DBNull.Value) ? 0 : Convert.ToInt32(pDataReader["ReminderCount"]);
      _CustomersTblData.Notes = (pDataReader["Notes"] == DBNull.Value) ? "" : pDataReader["Notes"].ToString();

      return _CustomersTblData;
    }

    public List<CustomersTbl> GetAllCustomers(string SortBy)
    {
      List<CustomersTbl> _ListCustomersTblData = new List<CustomersTbl>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_CUSTOMERS_SELECT;

        if (!String.IsNullOrEmpty(SortBy))
          _sqlCmd += " ORDER BY " + SortBy;
        // run the qurey we have built
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);

        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          _ListCustomersTblData.Add(MoveReaderDataToCustomersTblData(_DataReader));  //_CustomersTblData);
        }
      }

      return _ListCustomersTblData;
    }
    public CustomersTbl GetCustomersByCustomerID(long pCustomerID) { return GetCustomersByCustomerID(pCustomerID, ""); }
    public CustomersTbl GetCustomersByCustomerID(long pCustomerID, string pSortBy)
    {
      List<CustomersTbl> _ListCustomersTblData = new List<CustomersTbl>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_CUSTOMERS_SELECT;
        if (pCustomerID > 0)
          _sqlCmd += " WHERE CustomerID = ? ";
        
        if (!String.IsNullOrEmpty(pSortBy))
          _sqlCmd += " ORDER BY " + pSortBy;

        // run the qurey we have built
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        if (pCustomerID > 0)
          _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });

        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
          _ListCustomersTblData.Add(MoveReaderDataToCustomersTblData(_DataReader));  //_CustomersTblData);
      }
      return _ListCustomersTblData[0];
    }

    public bool InsertCustomer(CustomersTbl ThisCustomerTblData, ref string pErrorStr)
    {
      bool _RecsInserted = false;
      pErrorStr = "";
      string _connectionStr = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_CUSTOMERS_INSERT;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        #region Parameters
        // first summary data 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.CompanyName });          // Line 1.1
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContactTitle });         // Line 1.2
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContactFirstName });     // Line 1.3
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContactLastName });      // Line 1.4
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContactAltFirstName });  // Line 1.5
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContactAltLastName });   // Line 2.1
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.Department });           // Line 2.2
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.BillingAddress });       // Line 2.3
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.City });                 // Line 2.4
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.Province });             // Line 2.5
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PostalCode }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.Region });
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PhoneNumber }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.Extension });
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.FaxNumber });            // End line 3
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.CellNumber });          
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.EmailAddress }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.AltEmailAddress }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContractNo });
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.CustomerTypeID });       // End Line 4
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.EquipType }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.CoffeePreference });    
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PriPrefQty }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PrefPrepTypeID }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PrefPackagingID});       // 5
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.SecondaryPreference }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.SecPrefQty }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.TypicallySecToo }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PreferedAgent }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.SalesAgentID });  // 6
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.MachineSN }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.UsesFilter }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.autofulfill }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.enabled });
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PredictionDisabled });   // 7
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.AlwaysSendChkUp });  
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.NormallyResponds }); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ReminderCount });
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.Notes }); 
        #endregion
        try
        {
          _conn.Open();
          int _numInsert = _cmd.ExecuteNonQuery();
          _RecsInserted = (_numInsert > 0);
        }
        catch (OleDbException oleErr)
        {
          // Handle exception.
          pErrorStr = oleErr.Message;
          _RecsInserted = false;
        }
        finally
        {
          _conn.Close();
        }
        return _RecsInserted;
      }
    }

    public string UpdateCustomer(CustomersTbl ThisCustomerTblData, long CustomerIDToUpdate)
    {
      string errString = "";
      string _connectionStr = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_CUSTOMERS_UPDATE;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        #region Parameters
        // Add data sent
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.CompanyName } );
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContactTitle } );
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContactFirstName} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContactLastName} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContactAltFirstName} );
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContactAltLastName} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.Department} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.BillingAddress} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.City} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.Province} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PostalCode} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.Region } ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PhoneNumber} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.Extension} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.FaxNumber} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.CellNumber} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.EmailAddress} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.AltEmailAddress} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ContractNo} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.CustomerTypeID} );
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.EquipType} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.CoffeePreference} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PriPrefQty} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PrefPrepTypeID} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PrefPackagingID} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.SecondaryPreference} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.SecPrefQty} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.TypicallySecToo} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PreferedAgent} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.SalesAgentID} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.MachineSN} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.UsesFilter} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.autofulfill} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.enabled} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.PredictionDisabled} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.AlwaysSendChkUp} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.NormallyResponds} ); 
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.ReminderCount} );
        _cmd.Parameters.Add(new OleDbParameter { Value = ThisCustomerTblData.Notes } );
        //                                     " WHERE CustomersTbl.CustomerID = ?)";
        _cmd.Parameters.Add(new OleDbParameter { Value = CustomerIDToUpdate });

        /// last paramatere Cust ID
        ///// others here
        #endregion
        try
        {
          _conn.Open();
          if (_cmd.ExecuteNonQuery() > 0)
            errString = "";
          else
            errString += " - error ";
        }
        catch (OleDbException oleErr)
        {
          // Handle exception.
          errString += " ERROR: " + oleErr.Message; 
        }
        finally
        {
          _conn.Close();
        }
        return errString;
      }
    }
//    const string CONST_SQL_CUSTOMERS_SELECT_REMINDERCOUNT = "SELECT ReminderCount FROM CustomersTbl WHERE CustomersTbl.CustomerID = ?";
    public int GetReminderCount(long pCustomerID)
    {
      int _ReminderCount = -1;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_CUSTOMERS_SELECT_REMINDERCOUNT;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        //     WHERE CustomerID = ?
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID, OleDbType = OleDbType.Integer });
        try
        {
          _conn.Open();
          OleDbDataReader _DataReader = _cmd.ExecuteReader();
          if (_DataReader.Read())
            _ReminderCount = (_DataReader["ReminderCount"] == DBNull.Value) ? -1 : Convert.ToInt32(_DataReader["ReminderCount"]);
        }
        catch (OleDbException _ex)
        {
          // Handle exception.
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
        }
        finally
        {
          _conn.Close();
        }
      }
      return _ReminderCount;
    }
//    const string CONST_SQL_CUSTOMERS_INC_REMINDERCOUNT = "UPDATE CustomersTbl SET ReminderCount = ReminderCount + 1 WHERE CustomersTbl.CustomerID = ?";
    public bool IncrementReminderCount(long pCustomerID)
    {
      bool _Success = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_CUSTOMERS_INC_REMINDERCOUNT;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        #region Parameters
        //     WHERE CustomerID = ?
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID, OleDbType = OleDbType.Integer });
        #endregion
        try
        {
          _conn.Open();
          _Success = (_cmd.ExecuteNonQuery() >= 0);
        }
        catch (OleDbException _ex)
        {
          // Handle exception.
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
          _Success = false;
        }
        finally
        {
          _conn.Close();
        }
      }
      return _Success;
    }
    
    const string CONST_SQL_CUSTOMERS_RESET_REMINDERCOUNT = "UPDATE CustomersTbl SET ReminderCount = 0";
    const string CONST_SQL_CUSTOMERS_RESET_REMINDERCOUNT_FORCEENABLE = ", enabled = true";
    const string CONST_SQL_CUSTOMERS_RESET_REMINDERCOUNT_WHERE = " WHERE CustomerID = ?";
    public bool ResetReminderCount(long pCustomerID, bool pForceEnable)
    {
      bool _Success = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_CUSTOMERS_RESET_REMINDERCOUNT;
        if (pForceEnable)
          _sqlCmd += CONST_SQL_CUSTOMERS_RESET_REMINDERCOUNT_FORCEENABLE;

        _sqlCmd += CONST_SQL_CUSTOMERS_RESET_REMINDERCOUNT_WHERE;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        #region Parameters
        //     WHERE CustomerID = ?
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID, OleDbType = OleDbType.Integer });
        #endregion
        try
        {
          _conn.Open();
          _Success = (_cmd.ExecuteNonQuery() >= 0);
        }
        catch (OleDbException _ex)
        {
          // Handle exception.
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
          _Success = false;
        }
        finally
        {
          _conn.Close();
        }
      }
      return _Success;
    }
    //    const string CONST_SQL_CUSTOMERS_DISABLEIFREMINDERTOHIGH = "UPDATE CustomersTbl SET enabled = false WHERE (CustomerID = ?) AND (ReminderCount > ?)";
    public bool DisableCustomerIfReminderToHigh(long pCustomerID)
    {
      bool _Success = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_CUSTOMERS_DISABLEIFREMINDERTOHIGH;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        #region Parameters
        //     WHERE CustomerID = ?
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID, OleDbType = OleDbType.Integer });
        #endregion
        try
        {
          _conn.Open();
          _Success = (_cmd.ExecuteNonQuery() >= 0);
        }
        catch (OleDbException _ex)
        {
          // Handle exception.
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
          _Success = false;
        }
        finally
        {
          _conn.Close();
        }
      }
      return _Success;
    }
  }  // end of class
}
