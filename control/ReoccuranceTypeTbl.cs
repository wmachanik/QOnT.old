/// --- auto generated class for table: ReoccuranceTypeTbl
using System;   // for DateTime variables
using System.Collections.Generic;      // for data stuff
using System.Data.OleDb;
using System.Configuration;

namespace QOnT.control
{
  public class ReoccuranceTypeTbl
  {
    // internal variable declarations
    private int _ID;
    private string _Type;
    // class definition
    public ReoccuranceTypeTbl()
    {
      _ID = 0;
      _Type = string.Empty;
    }
    // get and sets of public
    public int ID { get { return _ID;}  set { _ID = value;} }
    public string Type { get { return _Type;}  set { _Type = value;} }

  #region ConstantDeclarations
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_SELECT = "SELECT ID, Type FROM ReoccuranceTypeTbl";
  #endregion

    public List<ReoccuranceTypeTbl> GetAll(string SortBy)
    {
      List<ReoccuranceTypeTbl> _DataItems = new List<ReoccuranceTypeTbl>();
      string _connectionStr = CONST_CONSTRING;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_SELECT;
        if (!String.IsNullOrEmpty(SortBy)) _sqlCmd += " ORDER BY " + SortBy;     // Add order by string
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          ReoccuranceTypeTbl _DataItem = new ReoccuranceTypeTbl();

          _DataItem.ID = (_DataReader["ID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ID"]);
          _DataItem.Type = (_DataReader["Type"] == DBNull.Value) ? string.Empty : _DataReader["Type"].ToString();
          _DataItems.Add(_DataItem);
        }
      }
      return _DataItems;
    }
  }
}
