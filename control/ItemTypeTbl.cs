/// --- auto generated class for table: ItemType
using System;   // for DateTime variables
using System.Collections.Generic;
using System.Data.OleDb;
using System.Configuration;

namespace QOnT.control
{
  public class ItemTypeTbl
  {
    // internal variable declarations
    private int _ItemTypeID;
    private string _SKU;
    private string _ItemDesc;
    private bool _ItemEnabled;
    private string _ItemsCharacteritics;
    private string _ItemDetail;
    private int _ServiceTypeID;
    private int _ReplacementID;
    private string _ItemShortName;
    private int _SortOrder;
    // class definition
    public ItemTypeTbl()
    {
      _ItemTypeID = 0;
      _SKU = string.Empty;
      _ItemDesc = string.Empty;
      _ItemEnabled = false;
      _ItemsCharacteritics = string.Empty;
      _ItemDetail = string.Empty;
      _ServiceTypeID = 0;
      _ReplacementID = 0;
      _ItemShortName = string.Empty;
      _SortOrder = 0;
    }
    // get and sets of public
    public int ItemTypeID { get { return _ItemTypeID;}  set { _ItemTypeID = value;} }
    public string SKU { get { return _SKU;}  set { _SKU = value;} }
    public string ItemDesc { get { return _ItemDesc;}  set { _ItemDesc = value;} }
    public bool ItemEnabled { get { return _ItemEnabled;}  set { _ItemEnabled = value;} }
    public string ItemsCharacteritics { get { return _ItemsCharacteritics;}  set { _ItemsCharacteritics = value;} }
    public string ItemDetail { get { return _ItemDetail;}  set { _ItemDetail = value;} }
    public int ServiceTypeID { get { return _ServiceTypeID;}  set { _ServiceTypeID = value;} }
    public int ReplacementID { get { return _ReplacementID;}  set { _ReplacementID = value;} }
    public string ItemShortName { get { return _ItemShortName;}  set { _ItemShortName = value;} }
    public int SortOrder { get { return _SortOrder;}  set { _SortOrder = value;} }

  #region ConstantDeclarations
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_SELECT = "SELECT ItemTypeID, SKU, ItemDesc, ItemEnabled, ItemsCharacteritics, ItemDetail, ServiceTypeID, ReplacementID, ItemShortName, SortOrder FROM ItemTypeTbl";
    const string CONST_SQL_SELECTSERVICETYPEID = "SELECT ServiceTypeID FROM ItemTypeTbl WHERE ItemTypeID = ?";
  #endregion

    public List<ItemTypeTbl> GetAll(string SortBy)
    {
      List<ItemTypeTbl> _DataItems = new List<ItemTypeTbl>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_SELECT;
        _sqlCmd += (!String.IsNullOrEmpty(SortBy)) ? " ORDER BY " + SortBy : " ORDER BY ItemDesc";   // add default order
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          ItemTypeTbl _DataItem = new ItemTypeTbl();

          _DataItem.ItemTypeID = (_DataReader["ItemTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ItemTypeID"]);
          _DataItem.SKU = (_DataReader["SKU"] == DBNull.Value) ? string.Empty : _DataReader["SKU"].ToString();
          _DataItem.ItemDesc = (_DataReader["ItemDesc"] == DBNull.Value) ? string.Empty : _DataReader["ItemDesc"].ToString();
          _DataItem.ItemEnabled = (_DataReader["ItemEnabled"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["ItemEnabled"]);
          _DataItem.ItemsCharacteritics = (_DataReader["ItemsCharacteritics"] == DBNull.Value) ? string.Empty : _DataReader["ItemsCharacteritics"].ToString();
          _DataItem.ItemDetail = (_DataReader["ItemDetail"] == DBNull.Value) ? string.Empty : _DataReader["ItemDetail"].ToString();
          _DataItem.ServiceTypeID = (_DataReader["ServiceTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ServiceTypeID"]);
          _DataItem.ReplacementID = (_DataReader["ReplacementID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ReplacementID"]);
          _DataItem.ItemShortName = (_DataReader["ItemShortName"] == DBNull.Value) ? string.Empty : _DataReader["ItemShortName"].ToString();
          _DataItem.SortOrder = (_DataReader["SortOrder"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["SortOrder"]);
          _DataItems.Add(_DataItem);
        }
      }
      return _DataItems;
    }

    public int GetServiceID(int pItemID)
    {
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        int _ServiceID = 0;
        OleDbCommand _cmd = new OleDbCommand(CONST_SQL_SELECTSERVICETYPEID, _conn);                    // run the qurey we have built
        _cmd.Parameters.Add(new OleDbParameter { Value = pItemID });
        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        if (_DataReader.Read())
        {
          _ServiceID = (_DataReader["ServiceTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ServiceTypeID"]);
        }
        return _ServiceID;
      }
    }
  }
}
