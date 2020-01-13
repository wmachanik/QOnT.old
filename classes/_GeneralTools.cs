using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QOnT.classes
{
  public class GeneralTools
  {
    public const string NullDate = "1980/01/01";
    // String versions of the above
    public const string STypeCleanStr = "Clean";
    public const string STypeCoffeeStr = "Coffee";
    public const string STypeCountStr = "Count";
    public const string STypeDescaleStr = "Descale";
    public const string STypeFilterStr = "Filter";
    public const string STypeSwopCollectStr = "SwopCollect";
    public const string STypeSwopStartStr = "SwopStart";
    public const string STypeSwopStopStr = "SwopStop";
    public const string STypeSwopRetrunStr = "SwopReturn";
    public const string STypeServiceStr = "Service";
    // Other constants
    public const int TypicalNumCupsPerKg = 100;
  
    // Service types with their order
    public enum ServiceType {stNone,
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
      SType2MthHoli} // "16"  '2 Months Holiday

    /// <summary>
    /// Get the number of Days until the next roast day, dependent optional RoastDayOfWeek or Tuesday
    /// </summary>
    /// <param name="pThisDate"></param>
    /// <returns>Days until the next roast date</returns>
    public int GetDaysToRoastDate(DateTime pThisDate) { return GetDaysToRoastDate(pThisDate, DayOfWeek.Tuesday);}
    public int GetDaysToRoastDate(DateTime pThisDate, DayOfWeek pRoastDayOfWeek ) 
    {
      DayOfWeek _ThisDOW = pThisDate.DayOfWeek;
      int _iAddDays = 0;

      // Check if they passed a roast day
      If(pRoastDayOfWeek < DayOfWeek.Sunday) || (pRoastDayOfWeek > DayOfWeek.Saturday);
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
      { return GetDaysToRoastDate(DateTime.Now,DayOfWeek.Tuesday);}
    public int NumDaysTillNextRoast(DayOfWeek pRoastDayOfWeek)
      { return GetDaysToRoastDate(DateTime.Now, pRoastDayOfWeek); }


    public DateTime RemoveTimePortion(DateTime pDate) { return pDate.Date; }
    /// <summary>
    /// Get the closets roast date to the date past, depending on the roastday (optional standard = Tues)
    /// </summary>
    /// <param name="pThisDate">Date to check the closest roast date</param>
    /// <returns></returns>
    public DateTime GetClosestNextRoastDate(DateTime pThisDate)
    { return RemoveTimePortion(GetClosestNextRoastDate(pThisDate, DayOfWeek.Tuesday)); }
    public DateTime GetClosestNextRoastDate(DateTime pThisDate, DayOfWeek pRoastDayOfWeek)
    { return RemoveTimePortion(pThisDate.AddDays(GetDaysToRoastDate(pThisDate, pRoastDayOfWeek) - 7)); }


    public bool RoastDateIsBtw(DateTime pRoastDate) 
      { return RoastDateIsBtw(pRoastDate, 1); }
    public bool RoastDateIsBtw(DateTime pRoastDate, long pOrderId)
    {
      // Intitialize variables
      bool dtStart = GetClosestNextRoastDate(DateTime.Now.AddDays(-7), DayOfWeek.Monday);
      bool dtEnd =  GetClosestNextRoastDate(DateTime.Now, DayOfWeek.Monday);

      // check if the data past is between these
      return (dtStart <= pRoastDate) && (pRoastDate < dtEnd);
    }
  }
}