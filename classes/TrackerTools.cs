using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Web;

namespace QOnT.classes
{

  public class TrackerTools
  {
    
    public const string CONST_STR_NULLDATE = "1980/01/01";
    public DateTime CONST_NULLDATE = DateTime.MinValue; // Convert.ToDateTime("1980/01/01"); // should have been a const but we cannot us that
    public const string CONST_SESSION_DATAACCESSERROR = "DataAccessError";

    // Number in constants for the Item Types
    public const int CONST_SERVTYPECLEAN = 1;      //The machine was cleaned (normally 200 cups)
    public const int CONST_SERVTYPECOFFEE = 2;      //Coffee was supplied
    public const int CONST_SERVTYPECOUNT = 3;      //A cup count was taken from the machine
    public const int CONST_SERVTYPEDESCALE = 4;      //The machine was descaled
    public const int CONST_SERVTYPEFILTER = 5;      //A filter was added to the machine
    public const int CONST_SERVTYPESWOPCOLLECT = 6;      //Clients machine;      //s count when swopped out for a temporary machine
    public const int CONST_SERVTYPESWOPSTART = 7;      //Swop out machine;      //s cup count at the start of the swop
    public const int CONST_SERVTYPESWOPSTOP = 8;      //Swop out machine;      //s cup count when machine is removed
    public const int CONST_SERVTYPESWOPRETRUN = 9;      //The client machine;      //s count once it is returned
    public const int CONST_SERVTYPESERVICE = 10;      //The machine was serviced (normally 5000 cups)
    public const int CONST_SERVTYPE1WKHOLI = 11;      //1 Week Holiday
    public const int CONST_SERVTYPE2WKHOLI = 12;      //2 Week Holiday
    public const int CONST_SERVTYPE3WKHOLI = 13;      //3 Week Holiday
    public const int CONST_SERVTYPE1MTHHOLI = 14;      //1 Month Holiday
    public const int CONST_SERVTYPE6WKHOLI = 15;      //6 Weeks Holiday
    public const int CONST_SERVTYPE2MTHHOLI = 16;      //2 Months Holiday
    public const int CONST_SERVTYPENOTAPPLICABLE = 17;      //A service type that is not important
    public const int CONST_SERVTYPEMAINTENANCE = 18;
    public const int CONST_SERVTYPEGREENBEAN = 19;

    // Number in constants for the Item Types string versions
    public const string CONST_STRING_SERVTYPECLEAN = "1";      //The machine was cleaned (normally 200 cups)
    public const string CONST_STRING_SERVTYPECOFFEE = "2";      //Coffee was supplied
    public const string CONST_STRING_SERVTYPECOUNT = "3";      //A cup count was taken from the machine
    public const string CONST_STRING_SERVTYPEDESCALE = "4";      //The machine was descaled
    public const string CONST_STRING_SERVTYPEFILTER = "5";      //A filter was added to the machine
    public const string CONST_STRING_SERVTYPESWOPCOLLECT = "6";      //Clients machine";      //s count when swopped out for a temporary machine
    public const string CONST_STRING_SERVTYPESWOPSTART = "7";      //Swop out machine";      //s cup count at the start of the swop
    public const string CONST_STRING_SERVTYPESWOPSTOP = "8";      //Swop out machine";      //s cup count when machine is removed
    public const string CONST_STRING_SERVTYPESWOPRETRUN = "9";      //The client machine";      //s count once it is returned
    public const string CONST_STRING_SERVTYPESERVICE = "10";      //The machine was serviced (normally 5000 cups)
    public const string CONST_STRING_SERVTYPE1WKHOLI = "11";      //1 Week Holiday
    public const string CONST_STRING_SERVTYPE2WKHOLI = "12";      //2 Week Holiday
    public const string CONST_STRING_SERVTYPE3WKHOLI = "13";      //3 Week Holiday
    public const string CONST_STRING_SERVTYPE1MTHHOLI = "14";      //1 Month Holiday
    public const string CONST_STRING_SERVTYPE6WKHOLI = "15";      //6 Weeks Holiday
    public const string CONST_STRING_SERVTYPE2MTHHOLI = "16";      //2 Months Holiday
    public const string CONST_STRING_SERVTYPENOTAPPLICABLE = "17";
    public const string CONST_STRING_SERVTYPEMAINTENANCE = "18";
    public const string CONST_STRING_SERVTYPEGREENBEAN = "19";
    // String descriptions versions of the above
    public const string CONST_DESC_SERVTYPECLEANSTR = "Clean";
    public const string CONST_DESC_SERVTYPECOFFEESTR = "Coffee";
    public const string CONST_DESC_SERVTYPECOUNTSTR = "Count";
    public const string CONST_DESC_SERVTYPEDESCALESTR = "Descale";
    public const string CONST_DESC_SERVTYPEFILTERSTR = "Filter";
    public const string CONST_DESC_SERVTYPESWOPCOLLECTSTR = "SwopCollect";
    public const string CONST_DESC_SERVTYPESWOPSTARTSTR = "SwopStart";
    public const string CONST_DESC_SERVTYPESWOPSTOPSTR = "SwopStop";
    public const string CONST_DESC_SERVTYPESWOPRETURNSTR = "SwopReturn";
    public const string CONST_DESC_SERVTYPESERVICESTR = "Service";
    public const string CONST_DESC_SERVTYPENOTAPPLICABLE = "N/A";
    // Other constants
    public const int CONST_TYPICALNUMCUPSPERKG = 100;
    public const double CONST_TYPICALAVECONSUMPTION = 5;  // 5 cups a day consumption is an average
    public const double CONST_TYPICALCLEAN_CONSUMPTION = 200;  // 200 between cleans is the default
    public const double CONST_TYPICALDECAL_CONSUMPTION = 500;  // 5 cups a day consumption is an average
    public const double CONST_TYPICALFILTER_CONSUMPTION = 300;  // 5 cups a day consumption is an average

    // Service types with their order
    public enum ServiceType
    {
      stNone,
      STypeClean, // "1"      'The machine was cleaned (normally 200 cups)
      STypeCoffee, // "2"     'Coffee was supplied
      STypeCount, // "3"      'A cup count was taken from the machine
      STypeDescale, // "4"    'The machine was descaled
      STypeFilter, // "5"     'A filter was added to the machine
      STypeSwopCollect, // "6"'Clients machine's count when swopped out for a temporary machine
      STypeSwopStart, // "7"  'Swop out machine's cup count at the start of the swop
      STypeSwopStop, // "8"   'Swop out machine's cup count when machine is removed
      STypeSwopRetrun, // "9" 'The client machine's count once it is returned
      STypeService, // "10"   'The machine was serviced (normally 5000 cups)
      SType1WkHoli, // "11"   '1 Week Holiday
      SType2WkHoli, // "12"   '2 Week Holiday
      SType3WkHoli, // "13"   '3 Week Holiday
      SType1MthHoli, // "14"  '1 Month Holiday
      SType6WkHoli, // "15"   '6 Weeks Holiday
      SType2MthHoli
    } // "16"  '2 Months Holiday

    // SQL Data Values
    public string TToolsConnStr = ConfigurationManager.ConnectionStrings["Tracker08ConnectionString"].ConnectionString;
    System.Data.OleDb.OleDbConnection TToolCon = null;
    
    /// <summary>
    /// Initializer
    /// </summary>
    public TrackerTools()
    {
    }

    /// <summary>
    /// Get the number of Days until the next roast day, dependent optional RoastDayOfWeek or Tuesday
    /// </summary>
    /// <param name="pThisDate">This date</param>
    /// <returns>Days until the next roast date</returns>
    public int GetDaysToRoastDate(DateTime pThisDate)
    { 
      DayOfWeek _ThisDay = DayOfWeek.Tuesday;

      if (((pThisDate.DayOfWeek == DayOfWeek.Tuesday) && (pThisDate.Hour >= 10)) ||
          (pThisDate.DayOfWeek == DayOfWeek.Wednesday) |
          ((pThisDate.DayOfWeek == DayOfWeek.Thursday) && (pThisDate.Hour < 10)))
      {
        _ThisDay = DayOfWeek.Thursday;   // order placed after 10 on a Tuesday vefore 10 Thurs done on Thursday
      }
      return GetDaysToRoastDate(pThisDate, _ThisDay); 
    }
    public int GetDaysToRoastDate(DateTime pThisDate, DayOfWeek pRoastDayOfWeek)
    {
      DayOfWeek _ThisDOW = pThisDate.DayOfWeek;
      int _iAddDays = 0;

      // Check if they passed a roast day
      if ((pRoastDayOfWeek < DayOfWeek.Sunday) || (pRoastDayOfWeek > DayOfWeek.Saturday))
        pRoastDayOfWeek = DayOfWeek.Tuesday;

      // Make sure it Wraps over a week by check it is not in this week
      _iAddDays = (int)pRoastDayOfWeek - (int)_ThisDOW;
      if (_ThisDOW <= pRoastDayOfWeek)
        return (7 + _iAddDays);
      else
        return (14 + _iAddDays);
    }
    /// <summary>
    /// Get the number of days till the next roast date dependant on day of Week past  (optional standard = Tues)
    /// </summary>
    /// <returns></returns>
    public int NumDaysTillNextRoast()
    { return GetDaysToRoastDate(DateTime.Now); }
    public int NumDaysTillNextRoast(DayOfWeek pRoastDayOfWeek)
    { return GetDaysToRoastDate(DateTime.Now, pRoastDayOfWeek); }


    public DateTime RemoveTimePortion(DateTime pDate) { return pDate.Date; }
    /// <summary>
    /// Get the closets roast date to the date past, depending on the roastday (optional standard = Tues)
    /// </summary>
    /// <param name="pThisDate">Date to check the closest roast date</param>
    /// <returns></returns>
    public DateTime GetClosestNextRoastDate(DateTime pThisDate)
    { return RemoveTimePortion(pThisDate.AddDays(GetDaysToRoastDate(pThisDate) - 7)); }
    public DateTime GetClosestNextRoastDate(DateTime pThisDate, DayOfWeek pRoastDayOfWeek)
    { return RemoveTimePortion(pThisDate.AddDays(GetDaysToRoastDate(pThisDate, pRoastDayOfWeek) - 7)); }


    public bool RoastDateIsBtw(DateTime pRoastDate)
    { return RoastDateIsBtw(pRoastDate, 1); }
    public bool RoastDateIsBtw(DateTime pRoastDate, long pOrderId)
    {
      // Intitialize variables
      DateTime dtStart = GetClosestNextRoastDate(DateTime.Now.AddDays(-7), DayOfWeek.Monday);
      DateTime dtEnd = GetClosestNextRoastDate(DateTime.Now, DayOfWeek.Monday);

      // check if the data past is between these
      return (dtStart <= pRoastDate) && (pRoastDate < dtEnd);
    }
    /// -----------------------------------------------
    /// Tools that use the database
    /// -----------------------------------------------
    
    /// <summary>Open the TTools connection if it is not already open</summary>
    private void OpenTToolsConnection()
    {
     // check if the connect is open and not null
      if (TToolCon == null)
        TToolCon = new OleDbConnection(TToolsConnStr);          // for SQL we would use       SqlConnection +Con = new SqlConnection(_connStr);
      if (TToolCon.State != System.Data.ConnectionState.Open)
        TToolCon.Open();
    }
    
    /// <summary>Has Next Roast Date by City be calculated today?  </summary>
    /// <returns>Yes if it has been</return s>
    public bool IsNextRoastDateByCityTodays()
    {
      bool _hasBeenDone = false;
      if (TToolCon == null)
        OpenTToolsConnection();
      // query the system table to find out if todays next roast date has been calculated
      string _sqlStr = "SELECT DateLastPrepDateCalcd FROM SysDataTbl WHERE (ID = 1)";
      OleDbCommand _Cmd = new OleDbCommand(_sqlStr, TToolCon);
      OleDbDataReader _Reader = _Cmd.ExecuteReader();
      if (_Reader.Read())
        _hasBeenDone = (DateTime.Now.Date == Convert.ToDateTime(_Reader["DateLastPrepDateCalcd"].ToString()).Date);
      // kill memory used
      _Cmd.Dispose();
      _Reader.Close();
      _Reader.Dispose();

      // return result
      return _hasBeenDone;
    }
    private bool UpdateOrInsertCityNextRstDate(int pCityID, DateTime pPrepDate, DateTime pDeliveryDate, short pOrder)
    {
      const string CITYNEXTRT_UPDATE = "UPDATE NextRoastDateByCityTbl SET PreperationDate = ?, DeliveryDate = ?, DeliveryOrder = ? WHERE CityID = ?";
      const string CITYNEXTRT_INS = "INSERT INTO NextRoastDateByCityTbl (PreperationDate, DeliveryDate, DeliveryOrder, CityID) VALUES (?,?,?,?)";

      bool _Success = false;
      string _sqlStr = "SELECT CityID FROM NextRoastDateByCityTbl WHERE CityID = " + pCityID;
      OleDbCommand _Cmd = new OleDbCommand(_sqlStr, TToolCon);
      OleDbDataReader _Reader = _Cmd.ExecuteReader();

      // if a record exists update command otherwise insert
      if (_Reader.Read())
        _sqlStr = CITYNEXTRT_UPDATE;
      else 
        _sqlStr = CITYNEXTRT_INS;
      // close the reader
      _Reader.Close();

      _Cmd.CommandText = _sqlStr;    // Set the SQL string
      _Cmd.Parameters.Add(new OleDbParameter { Value = pPrepDate, DbType = System.Data.DbType.Date });
      _Cmd.Parameters.Add(new OleDbParameter { Value = pDeliveryDate, DbType = System.Data.DbType.Date });
      _Cmd.Parameters.Add(new OleDbParameter { Value = pOrder, DbType = System.Data.DbType.Int16 });
      _Cmd.Parameters.Add(new OleDbParameter { Value = pCityID, DbType = System.Data.DbType.Int32 });
      try
      {
        _Success = _Cmd.ExecuteNonQuery() > 0;
      }
      catch (Exception E)
      {
        if (E.Message != "")
          throw;
      }
      finally
      {
        _Cmd.Dispose();
      }

      return _Success;    
    }
    private byte GetCorrectedDOW(byte pDOW)
    {
      // we need to adjust the DOW since VBA has Sunday as the first day = 1 and C# has Sunday as the first day  = 0 so 
      // each time we set dow we must do this calc
      return (byte)((pDOW == 0) ? 1 : pDOW - 1);
    }

    /// <summary> Set the next roast Date by city, notice this forces the calculation so if you do not want to run it every time check if 
    /// IsNextRoastDateByCityTodays, i.e. has it been calcualted already </summary>
    public void SetNextRoastDateByCity()
    {
      string _sqlStr = "SELECT  CityID, PrepDayOfWeekID, DeliveryDelayDays, DeliveryOrder FROM CityPrepDaysTbl ORDER BY CityID, PrepDayOfWeekID";

      int _iCityID = 0;
      byte _FirstPDOW;
      short _FirstDeliveryDelay, _FirstSortOrder;
      byte _ThisPDOW = (byte)System.DayOfWeek.Monday ;  // monday is the first day of the week in C# while in VBA it was Sunday
      short _ThisDeliveryDelay = 0, _ThisSortOrder = 0;
      short _NextDeliveryDelay = 0, _NextSortOrder = 0;
      DateTime dtNow = System.DateTime.Now;
      DateTime _dtPrep, _dtDelivery;
      // NOTE for the database Sunday = 1 for System.DateTime.Now.DayOfWeek Sunday = 0; so this need to be remembers when doing coparisons and calculations
      byte _TodaysDOW = (byte)System.DateTime.Now.DayOfWeek;  
      //  (byte)((byte)System.DateTime.Now.DayOfWeek+1);
      //if (_TodaysDOW > 7) _TodaysDOW = 1;    // fix Sunday;

      bool _bFirst, _bFound, _bIsSameCity = false, _EOF = false;

      // open a database connection if it is not open already
      if (TToolCon == null)
        OpenTToolsConnection();
      
      OleDbCommand _Cmd = new OleDbCommand(_sqlStr, TToolCon);
      OleDbDataReader _Reader = _Cmd.ExecuteReader();

      // count how many cites there are
      // List <NextRoastDateByCityTbl> _NRDbyCities = new List<NextRoastDateByCityTbl>();   // that would be the max

      // read the data if it is there
      //  while (_Reader.Read())  
      _EOF = !_Reader.Read();
      while (!_EOF)
      {
        _iCityID = (int)_Reader["CityID"];
        // set the first item of this collection
        _FirstPDOW = GetCorrectedDOW((byte)_Reader["PrepDayOfWeekID"]);
        // set the dow of week depending on what is the first day of the week

        _FirstDeliveryDelay = (short)_Reader["DeliveryDelayDays"] ;
        _FirstSortOrder = (short) ((_Reader["DeliveryOrder"] == DBNull.Value) ? 1 : _Reader["DeliveryOrder"]);
        _bFound = false;
        // find this cities next roast day, search until city changes or end of file
        while ((!_EOF) && (!_bFound))
        {
          _ThisPDOW = GetCorrectedDOW((byte)_Reader["PrepDayOfWeekID"]);
          _ThisDeliveryDelay = (short)_Reader["DeliveryDelayDays"];
          _ThisSortOrder = (short)((_Reader["DeliveryOrder"] == DBNull.Value) ? 1 : _Reader["DeliveryOrder"]);
          // go to next record exit if we are on another city
          _EOF = !_Reader.Read();
          if (!_EOF)
            _bFound = ((_TodaysDOW <= _ThisPDOW) || (_iCityID != (int)_Reader["CityID"]));  //  ' time check?
        }
          
        //
        if (! _EOF) {
          _bFirst = ((_iCityID != (int)_Reader["CityID"]) && (_TodaysDOW > _ThisPDOW));
          // go to the next City ID, if we get here then we have found a prep day for this City so we must skip ahead
          _bIsSameCity = (_iCityID == (int)_Reader["CityID"]);
          if (_bIsSameCity)     // we have found a day that is after the next day
          {
            while ((!_EOF) && (_bIsSameCity))
            {
              _EOF = ! _Reader.Read();
              if (!_EOF)
                _bIsSameCity = (_iCityID == (int)_Reader["CityID"]);
            }
          }    // not eof
          else if (!_bFound)
            _bFirst = true;   // must be the first record

          if (_bFirst)      // this means there are no pre days this week
          {         
            if (_FirstPDOW >= _ThisPDOW)
              _dtPrep = DateTime.Now.AddDays(_FirstPDOW - _TodaysDOW);
            else
              _dtPrep = DateTime.Now.AddDays((int)(7 - _TodaysDOW + _FirstPDOW ));

            _NextDeliveryDelay = _FirstDeliveryDelay;
            _NextSortOrder = _FirstSortOrder;
          }   // use the first
          else
          {
            _dtPrep = DateTime.Now.AddDays(_ThisPDOW - _TodaysDOW);
            _NextDeliveryDelay = _ThisDeliveryDelay;
            _NextSortOrder = _ThisSortOrder;
          }   //  use the next

          // insert or udpate the records depending on whether or not the records exists.
          _dtDelivery = _dtPrep.AddDays(_NextDeliveryDelay);

          bool _Success = UpdateOrInsertCityNextRstDate(_iCityID, _dtPrep, _dtDelivery, _NextSortOrder); // may want do do something here
        }
      }   // end loop while
      _Reader.Close();
      _Cmd.CommandText = "UPDATE SysDataTbl SET DateLastPrepDateCalcd = " + DateTime.Now.ToShortDateString() + " WHERE ID=1";
      _Cmd.ExecuteNonQuery();
      // kill memory used
      _Cmd.Dispose();
      _Reader.Close();
      _Reader.Dispose(); 
    }
    /// <summary>
    /// Get the next roast day using the customer's default city / area
    /// </summary>
    /// <param name="_CustID">CusotmerID as per Client Table</param>
    /// <param name="_deliveryDelay">Delivery days delay</param>
    /// <returns>The next roast day</returns>
    public DateTime GetNextRoastDateByCustomerID(long pCustID, ref DateTime pDelivery)
    {
      // generic variables
      DateTime _dtNextRoast = DateTime.Now;
      string _CustID = pCustID.ToString();
      string _CityID = "1";


      // prep the TTools connection, if it has not already been preped
      OpenTToolsConnection();
      
      // get the customers City
      string _sqlStr = "SELECT City FROM CustomersTbl WHERE CustomerID = " + _CustID;
      OleDbCommand _Cmd = new OleDbCommand(_sqlStr,TToolCon);                                   // for SQL we would use       SqlCommand _Cmd = new SqlCommand(_sqlStr);
      OleDbDataReader _Reader = _Cmd.ExecuteReader();                                 //  for SQL we would use       SqlDataReader _sqlReader = _Cmd.ExecuteReader();

      if (_Reader.Read())
        _CityID = _Reader["City"].ToString();
      _Reader.Close();
      _Cmd.Dispose();     // kill this so we can create a new one

      // find out if todays next roast date has been calculated
     if (!IsNextRoastDateByCityTodays())
        SetNextRoastDateByCity();           // The next roast days have not been calcualted, calculate them and save. When customers city is found save the details

      // the next roast days have been calcualted already today query table to find the details using the cutomer city
      _Cmd = new OleDbCommand("SELECT PreperationDate, DeliveryDate FROM NextRoastDateByCityTbl WHERE CityID = " + _CityID, TToolCon);
      // TToolCon.Open();   // opens the connection
      _Reader = _Cmd.ExecuteReader();
      if (_Reader.Read())
      {
        DateTime _dt = (DateTime)(_Reader["PreperationDate"]);
        _dtNextRoast = _dt.Date;
        _dt = (DateTime)_Reader["DeliveryDate"];
        pDelivery = _dt.Date;        
      }
      
      // kill memory used
      _Cmd.Dispose();
      _Reader.Close();
      _Reader.Dispose();

      return _dtNextRoast;
    }
    /// <summary>
    /// Retrieve the customer's defaults as per the ClientTbl
    /// </summary>
    /// <param name="_CustID">the customer id</param>
    /// <param name="_preferredDlvryBy">the preferede delviery person</param>
    /// <param name="_preferredItemId">the perfered Item</param>
    public void RetrieveCustomerPrefs(long _CustID, ref int pPreferredDlvryBy, ref int pPreferredItemId, ref double pPreferredQty )
    {
      // using custonmer id query the customers table to get the customers preferences.
      // prep the TTools connection, if it has not already been preped
      OpenTToolsConnection();
      
      // get the customers City
      string _sqlStr = "SELECT PreferedAgent , CoffeePreference, PriPrefQty FROM CustomersTbl WHERE CustomerID = " + _CustID.ToString();
      OleDbCommand _Cmd = new OleDbCommand(_sqlStr,TToolCon);                                   // for SQL we would use       SqlCommand _Cmd = new SqlCommand(_sqlStr);
      OleDbDataReader _Reader = _Cmd.ExecuteReader();                                 //  for SQL we would use       SqlDataReader _sqlReader = _Cmd.ExecuteReader();
      if (_Reader.Read())
      {
        pPreferredDlvryBy = (int)_Reader["PreferedAgent"];
        pPreferredItemId = (int)_Reader["CoffeePreference"];
        pPreferredQty = Convert.ToDouble(_Reader["PriPrefQty"]);
      }
      _Cmd.Dispose();
      _Reader.Close();
      _Reader.Dispose();
    }
    public void SetTrackerSessionErrorString(string pErrorString)
    {
      HttpContext _context = HttpContext.Current;   //       System.Web.HttpContext
      _context.Session[CONST_SESSION_DATAACCESSERROR] = pErrorString;
    }
    public String GetTrackerSessionErrorString()
    {
      HttpContext _context = HttpContext.Current;   //       System.Web.HttpContext
      string _errString = (_context.Session[CONST_SESSION_DATAACCESSERROR] != null) ? (string)_context.Session[CONST_SESSION_DATAACCESSERROR] : "";
      return _errString;
    }
  }

}