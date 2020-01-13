/// --- auto generated class for table: TrackedServiceItem
using System;   // for DateTime variables
using System.Collections.Generic;      // for data stuff
using System.Data.OleDb;
using System.Configuration;
using QOnT.classes;

namespace QOnT.control
{
  public class TrackedServiceItemTbl
  {
    // internal variable declarations
    private int _TrackerServiceItemID;
    private int _ServiceTypeID;
    private double _TypicalAvePerItem;
    private string _UsageDateFieldName;
    private string _UsageAveFieldName;
    private bool _ThisItemSetsDailyAverage;
    private string _Notes;
    // class definition
    public TrackedServiceItemTbl()
    {
      _TrackerServiceItemID = 0;
      _ServiceTypeID = 0;
      _TypicalAvePerItem = 0.0;
      _UsageDateFieldName = string.Empty;
      _UsageAveFieldName = string.Empty;
      _ThisItemSetsDailyAverage = false;
      _Notes = string.Empty;
    }
    // get and sets of public
    public int TrackerServiceItemID { get { return _TrackerServiceItemID;}  set { _TrackerServiceItemID = value;} }
    public int ServiceTypeID { get { return _ServiceTypeID;}  set { _ServiceTypeID = value;} }
    public double TypicalAvePerItem { get { return _TypicalAvePerItem;}  set { _TypicalAvePerItem = value;} }
    public string UsageDateFieldName { get { return _UsageDateFieldName;}  set { _UsageDateFieldName = value;} }
    public string UsageAveFieldName { get { return _UsageAveFieldName;}  set { _UsageAveFieldName = value;} }
    public bool ThisItemSetsDailyAverage { get { return _ThisItemSetsDailyAverage;}  set { _ThisItemSetsDailyAverage = value;} }
    public string Notes { get { return _Notes;}  set { _Notes = value;} }

  #region ConstantDeclarations
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_SELECT = "SELECT TrackerServiceItemID, ServiceTypeID, TypicalAvePerItem, UsageDateFieldName, UsageAveFieldName, ThisItemSetsDailyAverage, Notes FROM TrackedServiceItemTbl";
  #endregion

    public List<TrackedServiceItemTbl> GetAll(string SortBy)
    {
      List<TrackedServiceItemTbl> _DataItems = new List<TrackedServiceItemTbl>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_SELECT;
        if (!String.IsNullOrEmpty(SortBy)) _sqlCmd += " ORDER BY " + SortBy;     // Add order by string
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built

        try
        {
          _conn.Open();
          OleDbDataReader _DataReader = _cmd.ExecuteReader();
          while (_DataReader.Read())
          {
            TrackedServiceItemTbl _DataItem = new TrackedServiceItemTbl();

            _DataItem.TrackerServiceItemID = (_DataReader["TrackerServiceItemID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["TrackerServiceItemID"]);
            _DataItem.ServiceTypeID = (_DataReader["ServiceTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ServiceTypeID"]);
            _DataItem.TypicalAvePerItem = (_DataReader["TypicalAvePerItem"] == DBNull.Value) ? 0.0 : Convert.ToDouble(_DataReader["TypicalAvePerItem"]);
            _DataItem.UsageDateFieldName = (_DataReader["UsageDateFieldName"] == DBNull.Value) ? string.Empty : _DataReader["UsageDateFieldName"].ToString();
            _DataItem.UsageAveFieldName = (_DataReader["UsageAveFieldName"] == DBNull.Value) ? string.Empty : _DataReader["UsageAveFieldName"].ToString();
            _DataItem.ThisItemSetsDailyAverage = (_DataReader["ThisItemSetsDailyAverage"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["ThisItemSetsDailyAverage"]);
            _DataItem.Notes = (_DataReader["Notes"] == DBNull.Value) ? string.Empty : _DataReader["Notes"].ToString();
            _DataItems.Add(_DataItem);
          }
        }
        catch (OleDbException _ex)
        {
          // Handle exception.
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
          _DataItems.Clear();
        }
        finally
        {
          _conn.Close();
        }

      }
      return _DataItems;
    }
  }
}
