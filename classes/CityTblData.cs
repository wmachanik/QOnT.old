using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

/// --- auto generated class for table: CityTbl
namespace QOnT.classes
{
  public class CityTblData
  {
    // internal variable declarations
    private int _ID;
    private string _City;
    // not used //    private int _RoastingDay; //    private int _DeliveryDelay;
    // class definition
    public CityTblData()
    {
      _ID = 0;
      _City = string.Empty;
      // not used //      _RoastingDay = 0;   _DeliveryDelay = 0;
    }
    // get and sets of public
    public int ID { get { return _ID;}  set { _ID = value;} }
    public string City { get { return _City;}  set { _City = value;} }
  }


  public class CityTblDAL
  {
    const string CONST_CONSTRING = "Tracker08ConnectionString";
    const string CONST_SQL_SUMMARYDATA = "SELECT ID, City " +
                                         " FROM CityTbl";
    public static List<CityTblData> GetAllCityTblData(string SortBy)
    {
      List<CityTblData> _ListCitys = new List<CityTblData>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_SUMMARYDATA;
        // Add order by string
        _sqlCmd += " ORDER BY " + (!String.IsNullOrEmpty(SortBy) ? SortBy : " City");
        // run the qurey we have built
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);

        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          CityTblData _CityTblItem = new CityTblData();

          _CityTblItem.ID = Convert.ToInt32(_DataReader["ID"]);
          _CityTblItem.City = (_DataReader["City"] == DBNull.Value) ? "" : _DataReader["City"].ToString();

          _ListCitys.Add(_CityTblItem);
        }
      }

      return _ListCitys;
    }

  }

}
