/// --- auto generated class for table: ReoccuringOrderTbl
using System;   // for DateTime variables
using System.Collections.Generic;      // for data stuff
using System.Data.OleDb;
using System.Configuration;

namespace QOnT.control
{
  public class ReoccuringOrderTbl
  {
    // internal variable declarations
    private int _ID;
    private int _CustomerID;
    private int _ReoccuranceType;
    private int _ReoccuranceValue;
    private int _ItemRequiredID;
    private double _QtyRequired;
    private DateTime _DateLastDone;
    private DateTime _NextDateRequired;
    private DateTime _RequireUntilDate;
    private bool _Enabled;
    private string _Notes;
    // class definition
    public ReoccuringOrderTbl()
    {
      _ID = 0;
      _CustomerID = 0;
      _ReoccuranceType = 0;
      _ReoccuranceValue = 0;
      _ItemRequiredID = 0;
      _QtyRequired = 0.0;
      _DateLastDone = System.DateTime.Now;
      _NextDateRequired = System.DateTime.Now;
      _RequireUntilDate = System.DateTime.Now;
      _Enabled = false;
      _Notes = string.Empty;
    }
    // get and sets of public
    public int ID { get { return _ID;}  set { _ID = value;} }
    public int CustomerID { get { return _CustomerID;}  set { _CustomerID = value;} }
    public int ReoccuranceType { get { return _ReoccuranceType;}  set { _ReoccuranceType = value;} }
    public int ReoccuranceValue { get { return _ReoccuranceValue; } set { _ReoccuranceValue = value; } }
    public int ItemRequiredID { get { return _ItemRequiredID;}  set { _ItemRequiredID = value;} }
    public double QtyRequired { get { return _QtyRequired;}  set { _QtyRequired = value;} }
    public DateTime DateLastDone { get { return _DateLastDone;}  set { _DateLastDone = value;} }
    public DateTime NextDateRequired { get { return _NextDateRequired;}  set { _NextDateRequired = value;} }
    public DateTime RequireUntilDate { get { return _RequireUntilDate;}  set { _RequireUntilDate = value;} }
    public bool Enabled { get { return _Enabled;}  set { _Enabled = value;} }
    public string Notes { get { return _Notes;}  set { _Notes = value;} }

  #region ConstantDeclarations
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_SELECT = "SELECT ID, CustomerID, ReoccuranceType, Value, ItemRequiredID, QtyRequired, DateLastDone, NextDateRequired, RequireUntilDate, Enabled, Notes FROM ReoccuringOrderTbl";
    const string CONST_SQL_GETITEMSLASTDATE =
      " SELECT CustomerID, ItemRequiredID, MAX(LastDate) AS LastDatePerItem" +
      " FROM (SELECT ReoccuringOrderTbl.CustomerID, ReoccuringOrderTbl.ItemRequiredID, ClientUsageLinesTbl.[Date] AS LastDate " +
            "FROM  ((ClientUsageLinesTbl INNER JOIN" +
                    "ReoccuringOrderTbl ON ClientUsageLinesTbl.CustomerID = ReoccuringOrderTbl.CustomerID AND " +
                    "ClientUsageLinesTbl.[Date] > ReoccuringOrderTbl.DateLastDone) INNER JOIN " +
                    "ItemTypeTbl ON ReoccuringOrderTbl.ItemRequiredID = ItemTypeTbl.ItemTypeID)) ";
    const string CONST_UPDATE_ITEMSLASTDATE = "UPDATE ReoccuringOrderTbl SET DateLastDone " +
            "WHERE (CustomerID = ?) AND (ItemRequiredID = ?)";

  #endregion
    public List<ReoccuringOrderTbl> GetAll() { return GetAll(false, String.Empty); }
    public List<ReoccuringOrderTbl> GetAll(string SortBy) { return GetAll(false, SortBy); }
    public List<ReoccuringOrderTbl> GetAll(bool pOnlyEnabled) { return GetAll(pOnlyEnabled, String.Empty); }
    public List<ReoccuringOrderTbl> GetAll(bool pOnlyEnabled, string SortBy)
    {
      List<ReoccuringOrderTbl> _DataItems = new List<ReoccuringOrderTbl>();
      string _connectionStr = CONST_CONSTRING;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_SELECT;
        if (pOnlyEnabled) _sqlCmd += " WHERE ([Enabled] = true) ";               // add where if required
        if (!String.IsNullOrEmpty(SortBy)) _sqlCmd += " ORDER BY " + SortBy;     // Add order by string
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          ReoccuringOrderTbl _DataItem = new ReoccuringOrderTbl();

          _DataItem.ID = (_DataReader["ID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ID"]);
          _DataItem.CustomerID = (_DataReader["CustomerID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["CustomerID"]);
          _DataItem.ReoccuranceType = (_DataReader["ReoccuranceType"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ReoccuranceType"]);
          _DataItem.ReoccuranceValue = (_DataReader["Value"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["Value"]);
          _DataItem.ItemRequiredID = (_DataReader["ItemRequiredID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ItemRequiredID"]);
          _DataItem.QtyRequired = (_DataReader["QtyRequired"] == DBNull.Value) ? 0.0 : Convert.ToDouble(_DataReader["QtyRequired"]);
          _DataItem.DateLastDone = (_DataReader["DateLastDone"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["DateLastDone"]);
          _DataItem.NextDateRequired = (_DataReader["NextDateRequired"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["NextDateRequired"]);
          _DataItem.RequireUntilDate = (_DataReader["RequireUntilDate"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["RequireUntilDate"]);
          _DataItem.Enabled = (_DataReader["Enabled"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["Enabled"]);
          _DataItem.Notes = (_DataReader["Notes"] == DBNull.Value) ? string.Empty : _DataReader["Notes"].ToString();
          _DataItems.Add(_DataItem);
        }
      }
      return _DataItems;
    }

    public bool SetReoccuringItemsLastDate()
    {
      bool _Success = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;;
      List<ReoccuringOrderTbl> _ReoccuringOrderItems = new List<ReoccuringOrderTbl>();

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_GETITEMSLASTDATE;
        // not needed if (!String.IsNullOrEmpty(SortBy)) _sqlCmd += " ORDER BY " + SortBy;     // Add order by string
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _conn.Open();
        // read all the data from the query and post it into the DataList
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          ReoccuringOrderTbl _DataItem = new ReoccuringOrderTbl();

          _DataItem.CustomerID = (_DataReader["CustomerID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["CustomerID"]);
          _DataItem.ItemRequiredID = (_DataReader["ItemRequiredID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ItemRequiredID"]);
          _DataItem.DateLastDone = (_DataReader["LastDatePerItem"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["LastDatePerItem"]);
          _ReoccuringOrderItems.Add(_DataItem);
        }  // while

        //now update the table
        _conn.Close();
        _conn.Dispose();
        _cmd.Dispose();
      }

      for (int i = 0; i < _ReoccuringOrderItems.Count; i++)
      {
        using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
        {

          OleDbCommand _cmd = new OleDbCommand(CONST_UPDATE_ITEMSLASTDATE, _conn);
          #region Parameters
          // Add data SET data DateLastDone
          _cmd.Parameters.Add(new OleDbParameter { Value = _ReoccuringOrderItems[i].DateLastDone});
          // Where data
          _cmd.Parameters.Add(new OleDbParameter { Value = _ReoccuringOrderItems[i].CustomerID});
          _cmd.Parameters.Add(new OleDbParameter { Value = _ReoccuringOrderItems[i].ItemRequiredID });
          #endregion
          try
          {
            _conn.Open();
            _Success = (_cmd.ExecuteNonQuery() >= 0);
          }
          catch (OleDbException oleErr)
          { _Success = oleErr.ErrorCode != 0; }
          finally
          { 
            _conn.Close();
            _conn.Dispose();
            _cmd.Dispose();
          }
        }        
      }

      return _Success;
    }
    
  }
}
