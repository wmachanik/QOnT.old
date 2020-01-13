using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Configuration;
using QOnT.App_Code;

namespace QOnT.control
{
  public class OrderDBAgent
  {

    // constants
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_ORDERUPDATEHEADER_SQL = "UPDATE OrdersTbl SET CustomerId = ?, OrderDate= ?, RoastDate= ?, RequiredByDate= ?, ToBeDeliveredBy= ?, Confirmed= ?, Done= ?, Notes = ?";  // Where added later WHERE (OrderId = ?)";
    const string CONST_ORDERUPDATEITEMS_SQL = "UPDATE OrdersTbl SET ItemTypeID = ?, QuantityOrdered = ?, PackagingID = ? WHERE (OrderId = ?)";
    const string CONST_ORDERUPDATEALL_SQL = "UPDATE OrdersTbl SET CustomerId = ?, OrderDate= ?, RoastDate= ?, RequiredByDate= ?, ToBeDeliveredBy= ?, Confirmed= ?, Done= ?, Notes = ?, ItemTypeID = ?, QuantityOrdered = ?, PackagingID = ? WHERE (OrderId = ?)";


    private OleDbConnection _TrackerDbConn = null;

    public OleDbConnection TrackerDbConn
    {
      get { return this._TrackerDbConn; }
    }

    public OrderDBAgent()
    {
      Initialize();
    }

    private void Initialize()
    {
      if (_TrackerDbConn != null)
      {
        _TrackerDbConn.Dispose();
        _TrackerDbConn = null;
      }

      string _connectionString;

      if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
          ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
      {
        throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
                            "must exist in the <connectionStrings> configuration section for the application.");
      }
      _connectionString = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;
      this._TrackerDbConn = new OleDbConnection(_connectionString);
    }
    /// <summary>
    /// Execute the SQL statement does not return results, such as: delete, update, insert operation 
    /// </summary>
    /// <param name="strSQL">SQL String of a non Query Type</param>
    /// <returns>success or failure</returns>
    public bool UpdateOrderHeader(OrderHeaderData pOrderHeader, List<string> pOrders) 
    {
      bool _resultState = false;
      string _strSQL = CONST_ORDERUPDATEHEADER_SQL + " WHERE ";

      // for all the OrderIds passed create a where clause
      for (int i = 0; i < pOrders.Count-1; i++)
      {
        _strSQL += " OrderID = "+pOrders[i] + " OR";
      }
      _strSQL += " OrderID = " + pOrders[pOrders.Count-1];
        
      _TrackerDbConn.Open();
      OleDbTransaction _myTrans = _TrackerDbConn.BeginTransaction();
      // UPDATE order CustomerId = ?, OrderDate= ?, RoastDate= ?, RequiredByDate= ?, ToBeDeliveredBy= ?, Confirmed= ?, Done= ?, Notes = ? WHERE (OrderId = ?)";
      OleDbCommand _command = new OleDbCommand(_strSQL, _TrackerDbConn, _myTrans);
      // add parameters in the order of the SQL command
      _command.Parameters.Add(new OleDbParameter { Value = pOrderHeader.CustomerID });
      _command.Parameters.Add(new OleDbParameter { Value = pOrderHeader.OrderDate });
      _command.Parameters.Add(new OleDbParameter { Value = pOrderHeader.RoastDate });
      _command.Parameters.Add(new OleDbParameter { Value = pOrderHeader.RequiredByDate });
      _command.Parameters.Add(new OleDbParameter { Value = pOrderHeader.ToBeDeliveredBy });
      _command.Parameters.Add(new OleDbParameter { Value = pOrderHeader.Confirmed });
      _command.Parameters.Add(new OleDbParameter { Value = pOrderHeader.Done });
      _command.Parameters.Add(new OleDbParameter { Value = pOrderHeader.Notes });
//      _command.Parameters.Add(new OleDbParameter { Value =pOrderId});

      try 
      { 
        _command.ExecuteNonQuery (); 
        _myTrans.Commit (); 
        _resultState = true; 
      } 
      catch 
      { 
        _myTrans.Rollback (); 
        _resultState = false; 
      } 
      finally 
      { 
        _TrackerDbConn.Close (); 
      } 
      return _resultState; 
    }

  }
}