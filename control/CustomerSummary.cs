using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

/// Class to handle Return of customer summary data
///  SELECT   CustomersTbl.CustomerID, CustomersTbl.CompanyName, CustomersTbl.ContactFirstName, CustomersTbl.ContactLastName, CityTbl.City, 
///           CustomersTbl.PhoneNumber, CustomersTbl.EmailAddress, PersonsTbl.Abreviation AS DeliveryBy, EquipTypeTbl.EquipTypeName, CustomersTbl.MachineSN, 
///           CustomersTbl.autofulfill, CustomersTbl.enabled
///   FROM    (((CustomersTbl INNER JOIN
///                CityTbl ON CustomersTbl.City = CityTbl.ID) INNER JOIN
///                PersonsTbl ON CustomersTbl.PreferedAgent = PersonsTbl.PersonID) LEFT OUTER JOIN
///                EquipTypeTbl ON CustomersTbl.EquipType = EquipTypeTbl.EquipTypeId)

namespace QOnT.control
{
  
  public class CustomerSummary
  {
    public long CustomerID { get; set; }
    public string CompanyName { get; set; }
    public string ContactFirstName { get; set; }
    public string ContactLastName { get; set; }
    public string City { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public string DeliveryBy { get; set; }
    public string EquipTypeName { get; set; }
    public string MachineSN { get; set; }
    public bool autofulfill { get; set; }
    public bool enabled { get; set; }
  }
  
  public class CustomerSummaryDAL
  {
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_SUMMARYDATA = "SELECT CustomersTbl.CustomerID, CustomersTbl.CompanyName, CustomersTbl.ContactFirstName, CustomersTbl.ContactLastName, CityTbl.City AS City, " +
                                         " CustomersTbl.PhoneNumber, CustomersTbl.EmailAddress, PersonsTbl.Abreviation AS DeliveryBy, EquipTypeTbl.EquipTypeName, CustomersTbl.MachineSN, CustomersTbl.autofulfill, CustomersTbl.enabled" +
                                         " FROM (((CustomersTbl INNER JOIN CityTbl ON CustomersTbl.City = CityTbl.ID) INNER JOIN " +
                                                  "PersonsTbl ON CustomersTbl.PreferedAgent = PersonsTbl.PersonID) LEFT OUTER JOIN " +
                                                  "EquipTypeTbl ON CustomersTbl.EquipType = EquipTypeTbl.EquipTypeId)";

    // over load functions so that people can call without enable or fitler by where
    public static List<CustomerSummary> GetAllCustomerSummarys(string SortBy)
    { return GetAllCustomerSummarys(SortBy, -1,""); }
    public static List<CustomerSummary> GetAllCustomerSummarys(string SortBy, int IsEnabled)
    { return GetAllCustomerSummarys(SortBy, IsEnabled, ""); }
    /// <summary>
    /// Get all the customer summary infoation
    /// </summary>
    /// <param name="SortBy">(optional) sort the list by</param>
    /// <param name="IsEnabled">(optional) if -1 both enabled and disabled, if 0 disabled and if 1 enabled only</param>
    /// <returns></returns>
    public static List<CustomerSummary> GetAllCustomerSummarys(string SortBy, int IsEnabled, string WhereFilter)
    {
      List<CustomerSummary> _ListCustomers = new List<CustomerSummary>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_SUMMARYDATA;
        string _sqlWhere = "";

        // so they want enabled, disabled or both sets of clients.
        switch (IsEnabled)
        {
          case 0 :
            _sqlWhere = " WHERE CustomersTbl.enabled = false"; 
            break;
          case 1:
            _sqlWhere += " WHERE CustomersTbl.enabled = true";
            break;
          default:
            break;
        }
        // add where filter
        if (!String.IsNullOrWhiteSpace(WhereFilter))
        {
          if (String.IsNullOrWhiteSpace(_sqlWhere))
            _sqlWhere = " WHERE ";
          else
            _sqlWhere += " AND ";
          _sqlWhere += WhereFilter;
        }
        // if there is a where clause add it
        if (!String.IsNullOrWhiteSpace(_sqlWhere))
          _sqlCmd += _sqlWhere;
        // Add order by string
        if (!String.IsNullOrEmpty(SortBy))
        {
          _sqlCmd += " ORDER BY " + SortBy;
        }
        // run the qurey we have built
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);

        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          CustomerSummary _CustomerSummary = new CustomerSummary();

          _CustomerSummary.CustomerID = Convert.ToInt64(_DataReader["CustomerID"]);
          _CustomerSummary.CompanyName = (_DataReader["CompanyName"] == DBNull.Value) ? "" : _DataReader["CompanyName"].ToString();
          _CustomerSummary.ContactFirstName = (_DataReader["ContactFirstName"] == DBNull.Value) ? "" : _DataReader["ContactFirstName"].ToString();
          _CustomerSummary.ContactLastName = (_DataReader["ContactLastName"] == DBNull.Value) ? "" : _DataReader["ContactLastName"].ToString();
          _CustomerSummary.City = (_DataReader["City"] == DBNull.Value) ? "" : _DataReader["City"].ToString();
          _CustomerSummary.PhoneNumber = (_DataReader["PhoneNumber"] == DBNull.Value) ? "" : _DataReader["PhoneNumber"].ToString();
          _CustomerSummary.EmailAddress = (_DataReader["EmailAddress"] == DBNull.Value) ? "" : _DataReader["EmailAddress"].ToString();
          _CustomerSummary.DeliveryBy = (_DataReader["DeliveryBy"] == DBNull.Value) ? "" : _DataReader["DeliveryBy"].ToString();
          _CustomerSummary.EquipTypeName = (_DataReader["EquipTypeName"] == DBNull.Value) ? "" : _DataReader["EquipTypeName"].ToString();
          _CustomerSummary.MachineSN = (_DataReader["MachineSN"] == DBNull.Value) ? "" : _DataReader["MachineSN"].ToString();
          _CustomerSummary.autofulfill = (_DataReader["autofulfill"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["autofulfill"]);
          _CustomerSummary.enabled = (_DataReader["enabled"] == DBNull.Value) ? true : Convert.ToBoolean(_DataReader["enabled"]);

          _ListCustomers.Add(_CustomerSummary);
        }
      }

      return _ListCustomers;
    }

  }
}