/// --- auto generated class for table: TempOrdersLinesTbl
using System;   // for DateTime variables
using System.Collections.Generic;      // for data stuff
using System.Data.OleDb;
using System.Configuration;
using QOnT.classes;

namespace QOnT.control
{
  public class TempOrdersLinesTbl
  {
    // internal variable declarations
    private long _TOLineID;
    private long _TOHeaderID;
    private int _ItemID;
    private int _ServiceTypeID;
    private double _Qty;
    private int _PackagingID;
    private long _OriginalOrderID;
    // class definition
    public TempOrdersLinesTbl()
    {
      _TOLineID = 0;
      _TOHeaderID = 0;
      _ItemID = 0;
      _ServiceTypeID = 0;
      _Qty = 0.0;
      _PackagingID = 0;
      _OriginalOrderID = 0;
    }
    // get and sets of public
    public long TOLineID { get { return _TOLineID; } set { _TOLineID = value; } }
    public long TOHeaderID { get { return _TOHeaderID; } set { _TOHeaderID = value; } }
    public int ItemID { get { return _ItemID;}  set { _ItemID = value;} }
    public int ServiceTypeID { get { return _ServiceTypeID;}  set { _ServiceTypeID = value;} }
    public double Qty { get { return _Qty;}  set { _Qty = value;} }
    public int PackagingID { get { return _PackagingID;}  set { _PackagingID = value;} }
    public long OriginalOrderID { get { return _OriginalOrderID; } set { _OriginalOrderID = value; } }


    #region ConstantDeclarations
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_SELECT = "SELECT TOLineID, TOHeaderID, ItemID, ServiceTypeID, Qty, PackagingID, OriginalOrderID FROM TempOrdersLinesTbl";
    const string CONST_SQL_INSERT = "INSERT INTO TempOrdersLinesTbl (TOHeaderID, ItemID, ServiceTypeID, Qty, PackagingID, OriginalOrderID) " +
                                                            "VALUES (?, ?, ?, ?, ?, ?)";
    const string CONST_SQL_DELETEALL = "DELETE * FROM TempOrdersLinesTbl";
  #endregion

    public List<TempOrdersLinesTbl> GetAll(string SortBy)
    {
      List<TempOrdersLinesTbl> _DataItems = new List<TempOrdersLinesTbl>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_SELECT;
        if (!String.IsNullOrEmpty(SortBy)) _sqlCmd += " ORDER BY " + SortBy;     // Add order by string
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          TempOrdersLinesTbl _DataItem = new TempOrdersLinesTbl();

          _DataItem.TOLineID = (_DataReader["TOLineID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["TOLineID"]);
          _DataItem.TOHeaderID = (_DataReader["TOHeaderID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["TOHeaderID"]);
          _DataItem.ItemID = (_DataReader["ItemID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ItemID"]);
          _DataItem.ServiceTypeID = (_DataReader["ServiceTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ServiceTypeID"]);
          _DataItem.Qty = (_DataReader["Qty"] == DBNull.Value) ? 0.0 : Convert.ToDouble(_DataReader["Qty"]);
          _DataItem.PackagingID = (_DataReader["PackagingID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["PackagingID"]);
          _DataItem.OriginalOrderID = (_DataReader["OriginalOrderID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["OriginalOrderID"]);
          _DataItems.Add(_DataItem);
        }
      }
      return _DataItems;
    }
    public bool Insert(TempOrdersLinesTbl pLineData)
    {
      bool _Success = false;

      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        OleDbCommand _cmd = new OleDbCommand(CONST_SQL_INSERT, _conn);
        #region Parameters
        // Add data sent   in order: TOHeaderID, ItemID, ServiceTypeID, Qty, PackagingID, OriginalOrderID
        _cmd.Parameters.Add(new OleDbParameter { Value = pLineData.TOHeaderID });
        _cmd.Parameters.Add(new OleDbParameter { Value = pLineData.ItemID});
        _cmd.Parameters.Add(new OleDbParameter { Value = pLineData.ServiceTypeID });
        _cmd.Parameters.Add(new OleDbParameter { Value = pLineData.Qty });
        _cmd.Parameters.Add(new OleDbParameter { Value = pLineData.PackagingID});
        _cmd.Parameters.Add(new OleDbParameter { Value = pLineData.OriginalOrderID});
        #endregion
        try
        {
          _conn.Open();
          _Success = (_cmd.ExecuteNonQuery() > 0);
        }
        catch (OleDbException oleErr)
        { _Success = oleErr.ErrorCode != 0; }
        finally
        { _conn.Close(); }

        _cmd.Dispose();
      }
      return _Success;
    }

    public bool DeleteAllRecords()
    {
      bool _Success = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        OleDbCommand _cmd = new OleDbCommand(CONST_SQL_DELETEALL, _conn);                    // run the qurey we have built
        _conn.Open();
        _Success = (_cmd.ExecuteNonQuery() > 0);
      }

      return _Success;
    }
  }
}
