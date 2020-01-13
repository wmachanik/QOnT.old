/// --- auto generated class for table: TempOrdersLinesTbl
using System;   // for DateTime variables
using System.Collections.Generic;      // for data stuff
using System.Data.OleDb;
using System.Configuration;
using QOnT.classes;

namespace QOnT.control
{
  public class ClientUsageFromTempOrder
  {
    public const string CONST_STR_SELECT = "SELECT TempOrdersHeaderTbl.CustomerID, TempOrdersLinesTbl.ItemID, TempOrdersLinesTbl.ServiceTypeID, " +
                                         "TempOrdersLinesTbl.Qty,  ItemTypeTbl.UnitsPerQty, TempOrdersLinesTbl.PackagingID" +
                                   " FROM  ((TempOrdersHeaderTbl INNER JOIN TempOrdersLinesTbl ON TempOrdersHeaderTbl.TOHeaderID = TempOrdersLinesTbl.TOHeaderID) " +
                                           " LEFT OUTER JOIN ItemTypeTbl ON TempOrdersLinesTbl.ItemID = ItemTypeTbl.ItemTypeID)" +
                                   " WHERE TempOrdersHeaderTbl.CustomerID = ? AND TempOrdersLinesTbl.ServiceTypeID <> " + TrackerTools.CONST_STRING_SERVTYPENOTAPPLICABLE;

    // internal variable declarations
    private long _CustomerID;
    private int _ItemID;
    private int _ServiceTypeID;
    private double _Qty;
    private double _UnitsPerQty;
    private int _PackagingID;
    // class definition
    public ClientUsageFromTempOrder()
    {
      _CustomerID = 0;
      _ItemID = 0;
      _ServiceTypeID = 0;
      _Qty = 0.0;
      _UnitsPerQty = 0;
      _PackagingID = 0;
    }
    // get and sets of public
    public long CustomerID { get { return _CustomerID; } set { _CustomerID = value; } }
    public int ItemID { get { return _ItemID;}  set { _ItemID = value;} }
    public int ServiceTypeID { get { return _ServiceTypeID;}  set { _ServiceTypeID = value;} }
    public double Qty { get { return _Qty; } set { _Qty = value; } }
    public double UnitsPerQty { get { return _UnitsPerQty; } set { _UnitsPerQty = value; } }
    public int PackagingID { get { return _PackagingID; } set { _PackagingID = value; } }

    // routines
    public List<ClientUsageFromTempOrder> GetAll(long pCustomerID) { return GetAll(pCustomerID, "TempOrdersLinesTbl.ServiceTypeID"); }
    public List<ClientUsageFromTempOrder> GetAll(long pCustomerID, string SortBy)
    {
      List<ClientUsageFromTempOrder> _DataItems = new List<ClientUsageFromTempOrder>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_STR_SELECT;
        if (!String.IsNullOrEmpty(SortBy)) _sqlCmd += " ORDER BY " + SortBy;     // Add order by string if required
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        // Add parameter
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });

        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          ClientUsageFromTempOrder _DataItem = new ClientUsageFromTempOrder();

          _DataItem.CustomerID = (_DataReader["CustomerID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["CustomerID"]);
          _DataItem.ItemID = (_DataReader["ItemID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ItemID"]);
          _DataItem.ServiceTypeID = (_DataReader["ServiceTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ServiceTypeID"]);
          _DataItem.Qty = (_DataReader["Qty"] == DBNull.Value) ? 0.0 : Convert.ToDouble(_DataReader["Qty"]);
          _DataItem.UnitsPerQty = (_DataReader["UnitsPerQty"] == DBNull.Value) ? 1 : Convert.ToDouble(_DataReader["UnitsPerQty"]);
          _DataItem.PackagingID = (_DataReader["PackagingID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["PackagingID"]);
          _DataItems.Add(_DataItem);
        }
      }
      return _DataItems;
    }
  }
}
