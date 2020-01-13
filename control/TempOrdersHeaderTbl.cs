/// --- auto generated class for table: TempOrdersHeaderTbl
using System;   // for DateTime variables
using System.Collections.Generic;      // for data stuff
using System.Data.OleDb;
using System.Configuration;
using QOnT.classes;

namespace QOnT.control
{
  public class TempOrdersHeaderTbl
  {
    // internal variable declarations
    private long _TOHeaderID;
    private long _CustomerID;
    private DateTime _OrderDate;
    private DateTime _RoastDate;
    private DateTime _RequiredByDate;
    private int _ToBeDeliveredByID;
    private bool _Confirmed;
    private bool _Done;
    private string _Notes;
    // class definition
    public TempOrdersHeaderTbl()
    {
      _TOHeaderID = 0;
      _CustomerID = 0;
      _OrderDate = System.DateTime.Now;
      _RoastDate = System.DateTime.Now;
      _RequiredByDate = System.DateTime.Now;
      _ToBeDeliveredByID = 0;
      _Confirmed = false;
      _Done = false;
      _Notes = string.Empty;
    }
    // get and sets of public
    public long TOHeaderID { get { return _TOHeaderID; } set { _TOHeaderID = value; } }
    public long CustomerID { get { return _CustomerID; } set { _CustomerID = value; } }
    public DateTime OrderDate { get { return _OrderDate;}  set { _OrderDate = value;} }
    public DateTime RoastDate { get { return _RoastDate;}  set { _RoastDate = value;} }
    public DateTime RequiredByDate { get { return _RequiredByDate;}  set { _RequiredByDate = value;} }
    public int ToBeDeliveredByID { get { return _ToBeDeliveredByID;}  set { _ToBeDeliveredByID = value;} }
    public bool Confirmed { get { return _Confirmed;}  set { _Confirmed = value;} }
    public bool Done { get { return _Done;}  set { _Done = value;} }
    public string Notes { get { return _Notes;}  set { _Notes = value;} }

  #region ConstantDeclarations
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_SELECT = "SELECT TOHeaderID, CustomerID, OrderDate, RoastDate, RequiredByDate, ToBeDeliveredByID, Confirmed, Done, Notes FROM TempOrdersHeaderTbl";
    const string CONST_SQL_GETLASTHEADERID = "SELECT TOP 1 TOHeaderID FROM TempOrdersHeaderTbl ORDER By TOHeaderID DESC";

    const string CONST_SQL_INSERT = "INSERT INTO TempOrdersHeaderTbl (CustomerId, OrderDate, RoastDate, RequiredByDate, ToBeDeliveredByID, Confirmed, Done, Notes) " +
                                                        " VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

    const string CONST_SQL_MARKTEMPORDERSASDONE = "UPDATE OrdersTbl SET OrdersTbl.Done = True" +
                                                  " WHERE CustomderId = ? AND EXISTS (SELECT RequiredByDate FROM TempOrdersHeaderTbl " +
                                                                                    " WHERE (RequiredByDate = OrdersTbl.RequiredByDate))";
    const string CONST_SQL_DELETEALL = "DELETE * FROM TempOrdersHeaderTbl";
  #endregion

    public List<TempOrdersHeaderTbl> GetAll(string SortBy)
    {
      List<TempOrdersHeaderTbl> _DataItems = new List<TempOrdersHeaderTbl>();
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
          TempOrdersHeaderTbl _DataItem = new TempOrdersHeaderTbl();

          _DataItem.TOHeaderID = (_DataReader["TOHeaderID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["TOHeaderID"]);
          _DataItem.CustomerID = (_DataReader["CustomerID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["CustomerID"]);
          _DataItem.OrderDate = (_DataReader["OrderDate"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["OrderDate"]);
          _DataItem.RoastDate = (_DataReader["RoastDate"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["RoastDate"]);
          _DataItem.RequiredByDate = (_DataReader["RequiredByDate"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["RequiredByDate"]);
          _DataItem.ToBeDeliveredByID = (_DataReader["ToBeDeliveredByID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ToBeDeliveredByID"]);
          _DataItem.Confirmed = (_DataReader["Confirmed"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["Confirmed"]);
          _DataItem.Done = (_DataReader["Done"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["Done"]);
          _DataItem.Notes = (_DataReader["Notes"] == DBNull.Value) ? string.Empty : _DataReader["Notes"].ToString();
          _DataItems.Add(_DataItem);
        }
      }
      return _DataItems;
    }
    public bool Insert(TempOrdersHeaderTbl pHeaderData)
    {
      bool _Success = false;

      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        OleDbCommand _cmd = new OleDbCommand(CONST_SQL_INSERT, _conn);
        #region Parameters
        // Add data sent CustomerId, OrderDate, RoastDate, RequiredByDate, ToBeDeliveredByID, Confirmed, Done, Notes
        _cmd.Parameters.Add(new OleDbParameter { Value = pHeaderData.CustomerID });
        _cmd.Parameters.Add(new OleDbParameter { Value = pHeaderData.OrderDate });
        _cmd.Parameters.Add(new OleDbParameter { Value = pHeaderData.RoastDate });
        _cmd.Parameters.Add(new OleDbParameter { Value = pHeaderData.RequiredByDate });
        _cmd.Parameters.Add(new OleDbParameter { Value = pHeaderData.ToBeDeliveredByID });
        _cmd.Parameters.Add(new OleDbParameter { Value = pHeaderData.Confirmed });
        _cmd.Parameters.Add(new OleDbParameter { Value = pHeaderData.Done });
        _cmd.Parameters.Add(new OleDbParameter { Value = pHeaderData.Notes });
        #endregion
        try
        {
          _conn.Open();
          _Success = (_cmd.ExecuteNonQuery() >= 0);
        }
        catch (OleDbException oleErr)
        { _Success = oleErr.ErrorCode != 0; }
        finally
        { _conn.Close(); }

        _cmd.Dispose();
      }
      return _Success;
    }
    /// <summary>
    /// Return the last added TempHeaderOrder ID
    /// </summary>
    /// <returns></returns>
    public long GetCurrentTOHeaderID()
    {
      long _TOHeaderID = 0;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        OleDbCommand _cmd = new OleDbCommand(CONST_SQL_GETLASTHEADERID, _conn);                    // run the qurey we have built
        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        if (_DataReader.Read())
        {
          _TOHeaderID = (_DataReader["TOHeaderID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["TOHeaderID"]);
        }

        return _TOHeaderID;
      }
    }
    /// <summary>
    /// Delete all the records in the database
    /// </summary>
    public bool DeleteAllRecords()
    {
      bool _Success = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        OleDbCommand _cmd = new OleDbCommand(CONST_SQL_DELETEALL, _conn);                    // run the qurey we have built
        _conn.Open();
        _cmd.ExecuteNonQuery();
        _Success = _cmd.ExecuteNonQuery() > 0;
      }
      return _Success;
    }
  }
}
