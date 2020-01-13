using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;

namespace QOnT.control
{
  public class OrderItemTbl
  {
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_ORDERSLINES_SELECT = "SELECT ItemTypeID, QuantityOrdered, PrepTypeID FROM OrdersTbl WHERE ";
    // connection String
    private string _connectionString;

    public OrderItemTbl()
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

    // SELECT COMMAND USED IN LoadinOfORderSummary
    const string CONST_ORDERSSUMMARY_SELECT = "SELECT DISTINCT CustomerId, OrderDate, RoastDate, RequiredByDate, ToBeDeliveredBy, Confirmed, Done, Notes " +
                                              " FROM OrdersTbl WHERE ";

    public List<OrderHeaderData> LoadOrderSummary(Int32 CustomerId, DateTime DeliveryDate, String Notes, int MaximumRows, int StartRowIndex)
    {
      List<OrderHeaderData> ohSummarys = new List<OrderHeaderData>();

      string _sqlCmd = CONST_ORDERSSUMMARY_SELECT;
      //OleDbConnection _conn = new OleDbConnection(_connectionString);

      //OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
      //_cmd.Parameters.Add(new OleDbParameter { Value = CustomerId });
      //_cmd.Parameters.Add(new OleDbParameter { Value = PrepDate } );

      OleDbConnection _conn = new OleDbConnection(_connectionString);
      // customer ZZname = 9 is a general customer name so condition must be to that
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
      _conn.Open();
      OleDbDataReader _DataReader = _cmd.ExecuteReader();

      while (_DataReader.Read()) 
      {
        OrderHeaderData ohSummary = new OrderHeaderData();

        ohSummary.CustomerID = (Int32)_DataReader["CustomerId"];
        ohSummary.OrderDate = (DateTime)_DataReader["OrderDate"];
        ohSummary.RoastDate = (DateTime)_DataReader["RoastDate"];
        ohSummary.RequiredByDate = (DateTime)_DataReader["RequiredByDate"];
        ohSummary.ToBeDeliveredBy = (_DataReader["ToBeDeliveredBy"] == DBNull.Value) ? 3 :  (Int32)_DataReader["ToBeDeliveredBy"];  // 3 is the default
        ohSummary.Confirmed = (bool)_DataReader["Confirmed"];
        ohSummary.Done = (bool)_DataReader["Done"];
        ohSummary.Notes = (_DataReader["Notes"] ==   DBNull.Value) ? ""  : (string) _DataReader["Notes"] ;
        
        ohSummarys.Add(ohSummary);
      }
      _DataReader.Close();
      _conn.Close();

      return ohSummarys;
    }

    public bool UpdateOrderDetails(Int32 CustomerId, DateTime OrderDate, DateTime RoastDate, Int32 ToBeDeliveredBy, DateTime RequiredByDate,
                                   bool Confirmed, bool Done, string Notes, Int32 OriginalCustomerId, DateTime OriginalDeliveryDate, String OriginalNotes)
    {
      bool _Success = false;
      string _sqlCmd = "UPDATE OrdersTbl " +
                       " SET  CustomerId = ?, OrderDate = ?, RoastDate = ?, ToBeDeliveredBy = ?, RequiredByDate = ?, Confirmed = ?, Done = ?, Notes = ? WHERE ";

      OleDbConnection _conn = new OleDbConnection(_connectionString);

      // customer ZZname = 9 is a general customer name so condition must be to that
      if (OriginalCustomerId == 9)
        _sqlCmd += "([CustomerId] = 9) AND ([RequiredByDate] = ?) AND ([Notes] = ?)";
      else
        _sqlCmd += "([CustomerId] = ?) AND ([RequiredByDate] = ?)";
      // add parameters in the order they appear in the update command
      OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
      _cmd.Parameters.Add(new OleDbParameter { Value = CustomerId });
      _cmd.Parameters.Add(new OleDbParameter { Value = OrderDate });
      _cmd.Parameters.Add(new OleDbParameter { Value = RoastDate });
      _cmd.Parameters.Add(new OleDbParameter { Value = ToBeDeliveredBy });
      _cmd.Parameters.Add(new OleDbParameter { Value = RequiredByDate });
      _cmd.Parameters.Add(new OleDbParameter { Value = Confirmed });
      _cmd.Parameters.Add(new OleDbParameter { Value = Done });
      if (Notes == null)
        _cmd.Parameters.Add(new OleDbParameter { Value = String.Empty } );
      else
        _cmd.Parameters.Add(new OleDbParameter { Value = Notes });
      // attach the command
      if (OriginalCustomerId == 9)
      {
        _cmd.Parameters.Add(new OleDbParameter { Value = OriginalDeliveryDate });
        _cmd.Parameters.Add(new OleDbParameter { Value = OriginalNotes });
      }
      else
      {
        _cmd.Parameters.Add(new OleDbParameter { Value = OriginalCustomerId });
        _cmd.Parameters.Add(new OleDbParameter { Value = OriginalDeliveryDate });
      }

      try
      {
        _conn.Open();
        _Success = (_cmd.ExecuteNonQuery() > 0);

      }
//      catch (OleDbException em)
//      {
//        Response.Write("Error: ", em.Message);
//       return false;
//      }
      finally
      {
        _conn.Close();
      }

      return _Success;
    }

  }
}