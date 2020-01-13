using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace QOnT.control
{
  public class OrderTbl
  {
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_ORDERSHEADER_SELECT = "SELECT CustomersTbl.CompanyName, OrdersTbl.CustomerId As CustId, OrdersTbl.OrderDate, OrdersTbl.RoastDate, " +
                                             " OrdersTbl.RequiredByDate, PersonsTbl.Abreviation, PersonsTbl.PersonID,  OrdersTbl.Confirmed, OrdersTbl.Done, OrdersTbl.Notes " +
                                             " FROM ((OrdersTbl LEFT OUTER JOIN PersonsTbl ON OrdersTbl.ToBeDeliveredBy = PersonsTbl.PersonID)" +
                                             " LEFT OUTER JOIN CustomersTbl ON OrdersTbl.CustomerId = CustomersTbl.CustomerID)" +
                                             " WHERE ([CustomerId] = ?) AND ([RoastDate] = ?)";
    const string CONST_ORDERSLINES_SELECT = "SELECT ItemTypeID, QuantityOrdered, PrepTypeID FROM OrdersTbl WHERE ([OrdersTbl.CustomerId] = ?) AND ([RoastDate] = ?)";
    // connection String
    private string _connectionString;

    public OrderTbl()
    {
      Initialize();
    }
    public void Initialize()
    {
      // Initialize data source. Use "Tracker08" connection string from configuration.

      if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
          ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
      {
        throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
                            "must exist in the <connectionStrings> configuration section for the application.");
      }
      _connectionString =
        ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;
    }

    public List<OrderHeaderData> LoadOrderHeader(Int32 pCustomerID, DateTime pPrepDate)
    {
      List<OrderHeaderData> ohDetails = new List<OrderHeaderData>();

      string _sqlCmd = CONST_ORDERSHEADER_SELECT;
      OleDbConnection _conn = new OleDbConnection(_connectionString);

      OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
      _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });
      _cmd.Parameters.Add(new OleDbParameter { Value = pPrepDate } );

      _conn.Open();
      OleDbDataReader objReader = _cmd.ExecuteReader();
      
      while (objReader.Read()) 
      {
        OrderHeaderData ohDetail = new OrderHeaderData();
 
        ohDetail.CustomerID = (Int32)objReader["CustID"];
//        ohDetail.CompanyName = (string)objReader["CompanyName"];
//        ohDetail.Abreviation = (string)objReader["Abreviation "];
        ohDetail.ToBeDeliveredBy = (long)objReader["PersonsID"];
        ohDetail.Notes = (string)objReader["Notes"];
        ohDetail.OrderDate = (DateTime)objReader["OrderDate"];
        ohDetail.RoastDate = (DateTime)objReader["RoastDate"];
        ohDetail.RequiredByDate = (DateTime)objReader["RequiredByDate"];
        ohDetail.Confirmed = (bool)objReader["Confirmed"];
        ohDetail.Done = (bool)objReader["Done"];
        
        ohDetails.Add(ohDetail);
      }
      objReader.Close();
      _conn.Close();

      return ohDetails;
    }


  }
}