/// --- auto generated class for table: PackagingTbl
using System;   // for DateTime variables
using System.Collections.Generic;      // for data stuff
using System.Data.OleDb;
using System.Configuration;

namespace QOnT.control
{
  public class PackagingTbl
  {
    // internal variable declarations
    private int _PackagingID;
    private string _Description;
    private string _AdditionalNotes;
    private string _Symbol;
    private int _Colour;
    private string _BGColour;
    // class definition
    public PackagingTbl()
    {
      _PackagingID = 0;
      _Description = string.Empty;
      _AdditionalNotes = string.Empty;
      _Symbol = string.Empty;
      _Colour = 0;
      _BGColour = string.Empty;
    }
    // get and sets of public
    public int PackagingID { get { return _PackagingID;}  set { _PackagingID = value;} }
    public string Description { get { return _Description;}  set { _Description = value;} }
    public string AdditionalNotes { get { return _AdditionalNotes;}  set { _AdditionalNotes = value;} }
    public string Symbol { get { return _Symbol;}  set { _Symbol = value;} }
    public int Colour { get { return _Colour;}  set { _Colour = value;} }
    public string BGColour { get { return _BGColour;}  set { _BGColour = value;} }


  #region ConstantDeclarations
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_SELECT = "SELECT PackagingID, Description, AdditionalNotes, Symbol, Colour, BGColour FROM PackagingTbl";
  #endregion

    public List<PackagingTbl> GetAll(string SortBy)
    {
      List<PackagingTbl> _DataItems = new List<PackagingTbl>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_SELECT;
        if (!String.IsNullOrEmpty(SortBy)) _sqlCmd += " ORDER BY " + SortBy;     // Add order by string
        else _sqlCmd += " ORDER BY Description";   // add default order
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          PackagingTbl _DataItem = new PackagingTbl();

          _DataItem.PackagingID = (_DataReader["PackagingID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["PackagingID"]);
          _DataItem.Description = (_DataReader["Description"] == DBNull.Value) ? string.Empty : _DataReader["Description"].ToString();
          _DataItem.AdditionalNotes = (_DataReader["AdditionalNotes"] == DBNull.Value) ? string.Empty : _DataReader["AdditionalNotes"].ToString();
          _DataItem.Symbol = (_DataReader["Symbol"] == DBNull.Value) ? string.Empty : _DataReader["Symbol"].ToString();
          _DataItem.Colour = (_DataReader["Colour"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["Colour"]);
          _DataItem.BGColour = (_DataReader["BGColour"] == DBNull.Value) ? string.Empty : _DataReader["BGColour"].ToString();
          _DataItems.Add(_DataItem);
        }
      }
      return _DataItems;
    }
  }
}
