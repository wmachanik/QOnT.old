using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace QOnT.classes
{
  public class OrderDetailDAL
  {
    // connection String
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    private string _connectionString;
    public string lastError = "";

    public OrderDetailDAL()
    {
      Initialize();
    }
    public void Initialize()
    {
      // Initialize data source. Use "Tracker08" connection string from configuration.

      if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
          ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
      {
        lastError = "A connection string named " + CONST_CONSTRING + " with a valid connection string " +
                            "must exist in the <connectionStrings> configuration section for the application.";
        throw new Exception(lastError);
      }
      _connectionString =
        ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;
    }
    public List<OrderDetailData> LoadOrderDetailData(Int32 CustomerId, DateTime DeliveryDate, String Notes, int MaximumRows, int StartRowIndex)
    {
      List<OrderDetailData> oDetails = new List<OrderDetailData>();

      OleDbConnection _conn = new OleDbConnection(_connectionString);
      // custoemr ZZname = 9 is a general customer name so condition must be to that
      string _sqlCmd = "SELECT [ItemTypeID], [QuantityOrdered], [PackagingID], [OrderID] FROM [OrdersTbl] WHERE ";
      if (CustomerId == 9)
        _sqlCmd += "([CustomerId] = 9) AND ([RequiredByDate] = ?) AND ([Notes] = ?)";
      else
        _sqlCmd += "([CustomerId] = ?) AND ([RequiredByDate] = ?)";
      // attach the command
      OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
      if (CustomerId == 9)
      {
        _cmd.Parameters.Add(new OleDbParameter { Value = DeliveryDate });
        _cmd.Parameters.Add(new OleDbParameter { Value = Notes });
      }
      else
      {
        _cmd.Parameters.Add(new OleDbParameter { Value = CustomerId });
        _cmd.Parameters.Add(new OleDbParameter { Value = DeliveryDate });
      }
      try
      {
        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          OrderDetailData od = new OrderDetailData();

          od.ItemTypeID = (_DataReader["ItemTypeID"] == DBNull.Value) ? 0 : (Int32)_DataReader["ItemTypeID"];
          od.PackagingID = (_DataReader["PackagingID"] == DBNull.Value) ? 0 : (Int32)_DataReader["PackagingID"];
          od.OrderID = (Int32)_DataReader["OrderId"];   // this is the PK cannot be null
          od.QuantityOrdered = (_DataReader["QuantityOrdered"] == DBNull.Value) ? 1 : Convert.ToDouble(_DataReader["QuantityOrdered"].ToString());

          oDetails.Add(od);
        }
        _DataReader.Close();
      }
      catch (Exception _ex)
      {
        lastError = _ex.Message;
      }
      finally
      {
        _conn.Close();
      }
      return oDetails;
    }
    /// <summary>
    /// Update Order Details, using the orderID update the line info.
    /// </summary>
    /// <param name="ItemTypeID"></param>
    /// <param name="QuantityOrdered"></param>
    /// <param name="PackagingID"></param>
    /// <param name="OrderID"></param>
    /// <returns></returns>
    public bool UpdateOrderDetails(Int32 OrderID, Int32 ItemTypeID, double QuantityOrdered, Int32 PackagingID)
    {
      string _sqlCmd = "UPDATE OrdersTbl SET ItemTypeID = ?, QuantityOrdered = ?, PackagingID = ? WHERE (OrderId = ?)";
      OleDbConnection _conn = new OleDbConnection(_connectionString);

       // add parameters in the order they appear in the update command
      OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
      _cmd.Parameters.Add(new OleDbParameter { Value = ItemTypeID });
      _cmd.Parameters.Add(new OleDbParameter { Value = QuantityOrdered });
      _cmd.Parameters.Add(new OleDbParameter { Value = PackagingID });
      _cmd.Parameters.Add(new OleDbParameter { Value = OrderID });

      try
      {
        _conn.Open();
        if (_cmd.ExecuteNonQuery() > 0)
          return false;
      }
      catch (OleDbException ex)
      {
        return ex.Message == "";
      }
      finally
      {
        _conn.Close();
      }

      return true;
    }

    public bool InsertOrderDetails(Int32 CustomerID, DateTime OrderDate, DateTime RoastDate, Int32 ToBeDeliveredBy, 
                                   DateTime RequiredByDate, Boolean Confirmed, Boolean Done, String Notes, 
                                   double QuantityOrdered, Int32 PackagingID, Int32 ItemTypeID)
    {
      string _sqlCmd = "INSERT INTO OrdersTbl (CustomerId, OrderDate, RoastDate, RequiredByDate, ToBeDeliveredBy, Confirmed, Done, Notes, " +
                                              " ItemTypeID, QuantityOrdered, PackagingID) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"; 
      OleDbConnection _conn = new OleDbConnection(_connectionString);                           //1  2  3  4  5  6  7  8  9  10 11

      // add parameters in the order they appear in the update command
      OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
      // first summary data
      _cmd.Parameters.Add(new OleDbParameter { Value = CustomerID });
      _cmd.Parameters.Add(new OleDbParameter { Value = OrderDate });
      _cmd.Parameters.Add(new OleDbParameter { Value = RoastDate });
      _cmd.Parameters.Add(new OleDbParameter { Value = RequiredByDate });
      _cmd.Parameters.Add(new OleDbParameter { Value = ToBeDeliveredBy });
      _cmd.Parameters.Add(new OleDbParameter { Value = Confirmed });
      _cmd.Parameters.Add(new OleDbParameter { Value = Done });
      _cmd.Parameters.Add(new OleDbParameter { Value = Notes });

      // Now line data
      _cmd.Parameters.Add(new OleDbParameter { Value = ItemTypeID });
      _cmd.Parameters.Add(new OleDbParameter { Value = QuantityOrdered });
      _cmd.Parameters.Add(new OleDbParameter { Value = PackagingID });

      try
      {
        _conn.Open();
        if (_cmd.ExecuteNonQuery() > 0)
          return false;
      }
      catch (OleDbException ex)
      {
        return ex.Message == "";        // Handle exception.
      }
      finally
      {
        _conn.Close();
      }

      return true;
    }
    public bool DeleteOrderDetails(string OrderID)
    {
      bool _Success = false;

      string _sqlDeleteCmd = "DELETE FROM OrdersTbl WHERE (OrderID = ?)";
      OleDbConnection _conn = new OleDbConnection(_connectionString);

      // add parameters in the order they appear in the update command
      OleDbCommand _cmd = new OleDbCommand(_sqlDeleteCmd, _conn);
      _cmd.Parameters.Add(new OleDbParameter { Value = OrderID });

      try
      {
        _conn.Open();
        _Success = (_cmd.ExecuteNonQuery() > 0);
      }
      catch (OleDbException ex)
      {
        return ex.Message == "";
      }
      finally
      {
        _conn.Close();
      }

      return _Success;
    }
  
  }
}