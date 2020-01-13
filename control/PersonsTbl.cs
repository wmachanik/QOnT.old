/// --- auto generated class for table: PersonsTbl
using System;   // for DateTime variables
using System.Collections.Generic;      // for data stuff
using System.Data.OleDb;
using System.Configuration;

namespace QOnT.control
{
  public class PersonsTbl
  {
    // internal variable declarations
    private int _PersonID;
    private string _Person;
    private string _Abreviation;
    private bool _Enabled;
    // class definition
    public PersonsTbl()
    {
      _PersonID = 0;
      _Person = string.Empty;
      _Abreviation = string.Empty;
      _Enabled = false;
    }
    // get and sets of public
    public int PersonID { get { return _PersonID;}  set { _PersonID = value;} }
    public string Person { get { return _Person;}  set { _Person = value;} }
    public string Abreviation { get { return _Abreviation;}  set { _Abreviation = value;} }
    public bool Enabled { get { return _Enabled;}  set { _Enabled = value;} }

  #region ConstantDeclarations
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_SELECT = "SELECT PersonID, Person, Abreviation, Enabled FROM PersonsTbl";
  #endregion

    public List<PersonsTbl> GetAll(string SortBy)
    {
      List<PersonsTbl> _DataItems = new List<PersonsTbl>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_SELECT;
        if (!String.IsNullOrEmpty(SortBy)) _sqlCmd += " ORDER BY " + SortBy;     // Add order by string
        else _sqlCmd += " ORDER BY Abreviation";   // add default order
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          PersonsTbl _DataItem = new PersonsTbl();

          _DataItem.PersonID = (_DataReader["PersonID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["PersonID"]);
          _DataItem.Person = (_DataReader["Person"] == DBNull.Value) ? string.Empty : _DataReader["Person"].ToString();
          _DataItem.Abreviation = (_DataReader["Abreviation"] == DBNull.Value) ? string.Empty : _DataReader["Abreviation"].ToString();
          _DataItem.Enabled = (_DataReader["Enabled"] == DBNull.Value) ? false : Convert.ToBoolean(_DataReader["Enabled"]);
          _DataItems.Add(_DataItem);
        }
      }
      return _DataItems;
    }
  }
}
