using System;
using System.Data.OleDb;
using System.Collections.Generic;
using QOnT.control;
using System.Configuration;

namespace QOnT.classes
{
  public class GeneralTrackerDbTools
  {

    const int CONST_MAXROLLINGAVEVALUES = 6;

    public GeneralTrackerDbTools()
    {
      // ininialization stuff if needed
    }

    /// <summary>
    /// Client Usage Data from the usage line tables
    /// </summary>
    public class LineUsageData
    {

      private long _LastCount;
      private double _LastQty;
      private DateTime _UsageDate;

      public LineUsageData()
      {
        _LastCount = 0;
        _LastQty = 0.0;
        _UsageDate = DateTime.MinValue;  // was DateTime.Now;
      }

      public long LastCount { get { return _LastCount; } set { _LastCount = value; } }
      public double LastQty { get { return _LastQty; } set { _LastQty = value; } }
      public DateTime UsageDate { get { return _UsageDate; } set { _UsageDate = value; } }

    }

    public class ClientServiceItem
    {
      private string _UsageDateFieldName;
      private string _UsageAveFieldName;
      private DateTime _NextUsageDate;
      private double _ThisItemsAverage;

      public ClientServiceItem()
      {
        _UsageDateFieldName = "";
        _UsageAveFieldName = "";
        _NextUsageDate = DateTime.MaxValue;
        _ThisItemsAverage = TrackerTools.CONST_TYPICALAVECONSUMPTION;
      }
      
      public string UsageDateFieldName { get { return _UsageDateFieldName; } set { _UsageDateFieldName = value; } }
      public string UsageAveFieldName { get { return _UsageAveFieldName; } set { _UsageAveFieldName = value; } }
      public DateTime NextUsageDate { get { return _NextUsageDate; } set { _NextUsageDate = value; } }
      public double ThisItemsAverage { get { return _ThisItemsAverage; } set { _ThisItemsAverage = value; } }

    }

    // --- ALL Pure retrieval rotuines below, the do a simple Query and return the data, and a prefixed with Get
    
    /// <summary>
    /// Get the latest usage data from the client usage table
    /// </summary>
    /// <param name="pCustomerID">which customer</param>
    /// <param name="pServiceTypeID">which service type of data , or "" for no service type</param>
    /// <returns></returns>
    public LineUsageData GetLatestUsageData(long pCustomerID,int pServiceTypeID)
    {
      LineUsageData _LatestUsageData = new LineUsageData();

      ClientUsageLinesTbl _UsageData = new ClientUsageLinesTbl();
      _UsageData = _UsageData.GetLatestUsageData(pCustomerID,pServiceTypeID);

      if (_UsageData.CupCount > 0)
        {
          _LatestUsageData.LastCount = _UsageData.CupCount;
          _LatestUsageData.LastQty = _UsageData.Qty;
          _LatestUsageData.UsageDate = _UsageData.LineDate;
        }

      return _LatestUsageData;
    }
    
    /// <summary>
    /// Get the Installation date is assumed to be the date of the customer's first record in the usage line table. Added for compatiblity reasons
    /// </summary>
    /// <param name="pCustomerID">customer ID</param>
    /// <returns>The install date</returns>
    public DateTime GetInstallDate(long pCustomerID)
    {
      ClientUsageLinesTbl _ClientUsageTbl = new ClientUsageLinesTbl();

      return _ClientUsageTbl.LookupCustomerInstallDate(pCustomerID);
    }

    /// <summary>
    /// Gets the average consumption as per the clients data
    /// </summary>
    /// <param name="pCustomerID">Customer ID to retrieve data of</param>
    /// <returns>Average consumption as stored in the ClientUsageTbl or if none calculates it</returns>
    public double GetAveConsumption(long pCustomerID)
    {
      ClientUsageTbl _ClientUsageTbl = new ClientUsageTbl();
      return _ClientUsageTbl.GetAverageConsumption(pCustomerID);
    }
    /// <summary>
    /// Find the latest record with service type = service typ[e
    /// </summary>
    /// <param name="pCustomerID">the customer to get the last cup count from</param>
    /// <param name="pServiceTypeID">the service type to search for default coffee</param>
    /// <returns></returns>
    public LineUsageData GetLastCupCount(long pCustomerID) { return GetLastCupCount(pCustomerID, TrackerTools.CONST_SERVTYPECOFFEE); }
    public LineUsageData GetLastCupCount(long pCustomerID,int pServiceTypeID)
    {
      return GetLatestUsageData(pCustomerID, pServiceTypeID);

      #region oldVBCode
      /// replaced this below since routine above does the same thign
       //GetLastCupCount = 0
       //'Find the latest record with service type = "Coffee"
       //strSQL = "SELECT ClientUsageLinesTbl.CupCount, ClientUsageLinesTbl.Qty"
       //strSQL = strSQL + " FROM ClientUsageLinesTbl"
       //strSQL = strSQL + " WHERE ClientUsageLinesTbl.CustomerID = " + pCustomerID
       //'' if they past a type then add to select statment
       //If sType <> "" Then
       //  strSQL = strSQL + " AND ClientUsageLinesTbl.ServiceTypeID = " & sType
       //End If
       //strSQL = strSQL + " ORDER BY ClientUsageLinesTbl.Date DESC"
   
       //Set rstClientUsageLines = CurrentDb.OpenRecordset(strSQL)
   
       //If Not rstClientUsageLines.EOF Then
       //   rstClientUsageLines.MoveFirst  ' order is desc
       //   If Not IsNull(rstClientUsageLines(0)) Then
       //      If Not IsNull(rstClientUsageLines("Qty")) Then   ' for the older records that are not updated
       //        pQty = rstClientUsageLines("Qty")
       //      End If
       //      GetLastCupCount = rstClientUsageLines("CupCount")
       //   End If
       //End If
      //rstClientUsageLines.Close
      #endregion
    }
    //-------------- ALL Calculation type queries below
    /// <summary>
    /// Get the number of days to remove based on a holiday period, return 0 if not found
    /// </summary>
    /// <param name="pServiceTypeID">the service type holiday period</param>
    /// <returns>days to remove (negative)</returns>
    public int DaysToAddToDate(int pServiceTypeID)
    {
      int _DaysToAdd = 0;
      switch (pServiceTypeID)
      {
        case TrackerTools.CONST_SERVTYPE1WKHOLI:
          _DaysToAdd = -7;
          break;
        case TrackerTools.CONST_SERVTYPE2WKHOLI:
          _DaysToAdd = -14;
          break;
        case TrackerTools.CONST_SERVTYPE3WKHOLI:
          _DaysToAdd = -21;
          break;
        case TrackerTools.CONST_SERVTYPE1MTHHOLI:
          _DaysToAdd = -31;
          break;
        case TrackerTools.CONST_SERVTYPE6WKHOLI:
          _DaysToAdd = -(7 * 6);
          break;
        case TrackerTools.CONST_SERVTYPE2MTHHOLI:
          _DaysToAdd = -61; // -(2 * 30.5);
          break;
        default:
          break;
      }
      return _DaysToAdd;
    }
    /// <summary>
    /// Check if the last record in the usage list means they are taking a break, then add the break to the date
    /// </summary>
    /// <param name="pCustomerID">for which customer</param>
    /// <param name="pDT">date to adjust</param>
    /// <param name="pServiceTypeID">the service type</param>
    /// <returns></returns>
    public DateTime AddHolidayExtension(long pCustomerID, DateTime pDT,int pServiceTypeID)
    {
      
      // select all the usage lines order DESC so that the last record is the latest
      ClientUsageLinesTbl _UsageDAL = new ClientUsageLinesTbl();
      List<ClientUsageLinesTbl> _UsageLines = _UsageDAL.GetLast10UsageLines(pCustomerID);
      int _NumValues = 0;

       // For every holiday period int he last 6 remove the period from the data difference
      while ((_UsageLines.Count > _NumValues) && (_NumValues < 6))
      {
        if (_UsageLines[_NumValues].ServiceTypeID != 0)
        {
          if ((_UsageLines[_NumValues].ServiceTypeID >=TrackerTools.CONST_SERVTYPE1WKHOLI) || (_UsageLines[_NumValues].ServiceTypeID <=TrackerTools.CONST_SERVTYPE2MTHHOLI))
            pDT = pDT.AddMonths(DaysToAddToDate(_UsageLines[_NumValues].ServiceTypeID));
          else if (_UsageLines[_NumValues].ServiceTypeID == pServiceTypeID)
            _NumValues = 6;
        }
        _NumValues++;
      }

      return pDT;
    }
    /// <summary>
    /// Get the number of days to remove based on a holiday period, return 0 if not found
    /// </summary>
    /// <param name="pServiceTypeID">the service type holiday period</param>
    /// <returns>days to remove (negative)</returns>
    public int DaysToRemoveFromDate(int pServiceTypeID)
    {
      int _DaysToRemove = 0;
      switch (pServiceTypeID)
      {
        case TrackerTools.CONST_SERVTYPE1WKHOLI:
          _DaysToRemove = -7;
          break;
        case TrackerTools.CONST_SERVTYPE2WKHOLI:
          _DaysToRemove = -14;
          break;
        case TrackerTools.CONST_SERVTYPE3WKHOLI:
          _DaysToRemove = -21;
          break;
        case TrackerTools.CONST_SERVTYPE1MTHHOLI:
          _DaysToRemove = -31;
          break;
        case TrackerTools.CONST_SERVTYPE6WKHOLI:
          _DaysToRemove = -(7 * 6);
          break;
        case TrackerTools.CONST_SERVTYPE2MTHHOLI:
          _DaysToRemove = -61; // -(2 * 30.5);
          break;
        default:
          break;
      }
      return _DaysToRemove;
    }
    /// <summary>
    /// Remove any holidays form the date passed
    /// </summary>
    /// <param name="pCustomerID">which customer</param>
    /// <param name="pDT">the date to remvoe from</param>
    /// <param name="pServiceTypeID">for which service type Id</param>
    /// <returns></returns>
    public DateTime RemoveHolidayPeriodFromDate(long pCustomerID, DateTime pDT,int pServiceTypeID)
    {
      // select all the usage lines order DESC so that the last record is the latest
      ClientUsageLinesTbl _UsageDAL = new ClientUsageLinesTbl();
      List<ClientUsageLinesTbl> _UsageLines = _UsageDAL.GetLast10UsageLines(pCustomerID);
      int _NumValues = 0;

       // For every holiday period int he last 6 remove the period from the data difference
      while ((_UsageLines.Count > _NumValues) && (_NumValues < 6))
      {
        if (_UsageLines[_NumValues].ServiceTypeID != 0)
        {
          if ((_UsageLines[_NumValues].ServiceTypeID >=TrackerTools.CONST_SERVTYPE1WKHOLI) || (_UsageLines[_NumValues].ServiceTypeID <=TrackerTools.CONST_SERVTYPE2MTHHOLI))
            pDT = pDT.AddMonths(DaysToRemoveFromDate(_UsageLines[_NumValues].ServiceTypeID));
          else if (_UsageLines[_NumValues].ServiceTypeID == pServiceTypeID)
            _NumValues = 6;
        }
        _NumValues++;
      }

      return pDT;
    }

    public long CalcEstCupCount(long pCustomerID,  LineUsageData pClientUsageData, bool pIsCoffee)
    {
      double _newCupCount = 1;

      if (pClientUsageData.UsageDate > DateTime.MinValue)
      {

        double _AveConsump = GetAveConsumption(pCustomerID);
        // Calculate the number of datys difference
        DateTime _AdjustedDate = RemoveHolidayPeriodFromDate(pCustomerID, pClientUsageData.UsageDate, QOnT.classes.TrackerTools.CONST_SERVTYPECOFFEE);
        int _DateDiff = Convert.ToInt32((DateTime.Now - _AdjustedDate).TotalDays);
      
        if ((!pIsCoffee) || (pClientUsageData.LastQty == 0))
          _newCupCount= pClientUsageData.LastCount + (_DateDiff * _AveConsump);
        else
          _newCupCount= (pClientUsageData.LastCount + (pClientUsageData.LastQty * TrackerTools.CONST_TYPICALNUMCUPSPERKG));
      }

      return Convert.ToInt32(Math.Round(_newCupCount));

    }
    public long CoffeeCupsLeft(long pCustomerID, LineUsageData pCoffeeUsageData)
    { return CoffeeCupsLeft(pCustomerID, pCoffeeUsageData, TrackerTools.CONST_SERVTYPECOFFEE, TrackerTools.CONST_TYPICALNUMCUPSPERKG); }
    public long CoffeeCupsLeft(long pCustomerID, LineUsageData pCoffeeUsageData, int pServiceTypeID,  int pTypicalPerKg)
    {
      long _CupsLeft = 0;
      LineUsageData _LatestData = GetLatestUsageData(pCustomerID, pServiceTypeID);  // get the last record in the database
      _CupsLeft = (long)Math.Round((_LatestData.LastQty * pTypicalPerKg));
      if (_LatestData.LastCount > pCoffeeUsageData.LastCount)
        _CupsLeft = _CupsLeft - (_LatestData.LastCount - pCoffeeUsageData.LastCount);

      return _CupsLeft;
    }
    /// <summary>
    /// Calculates the average consumption of an item, either perday or per cups consumed
    /// </summary>
    /// <param name="pCustomerID">for wich customer id</param>
    /// <returns>average consumption of the type passed</returns>
    public double CalcAveConsumption(long pCustomerID)
    { return CalcAveConsumption(pCustomerID, TrackerTools.CONST_SERVTYPECOFFEE, TrackerTools.CONST_TYPICALAVECONSUMPTION, true); }
    /// <summary>x
    /// Calculates the average consumption of an item, either perday or per cups consumed
    /// </summary>
    /// <param name="pCustomerID">for wich customer id</param>
    /// <param name="pServiceTypeID">for what type of service type default is coffee</param>
    /// <param name="pTypicalAverageConsumption">What is the typical average consumption to set as default</param>
    /// <param name="pPerDayCalc">Is this a per day or per cup calculation</param>
    /// <returns>average consumption of the type passed</returns>
    public double CalcAveConsumption(long pCustomerID,int pServiceTypeID, double pTypicalAverageConsumption, bool pPerDayCalc)
    {
      
      double _AveConsumption = pTypicalAverageConsumption;   // average cups divided by cups used
      List<double> _AverageConsumptions = new List<double>();
      double _ThisAverage = 0, _TotalAverages = 0;
      int _NumValues = 0,_DaysSincePrev = 0;
      int _ThisServiceTypeID = 0, _UsageLineNo = 1;  // start at one since we need at leqast 2 dates to calc
      DateTime _PrevDate = DateTime.MinValue;     // NULLDATE
      long _PrevCupCount = 0, _CupsSincePrev = 0;


      ClientUsageLinesTbl _ClientUsageLinesTbl = new ClientUsageLinesTbl();
      List<ClientUsageLinesTbl> _UsageData = _ClientUsageLinesTbl.GetLast10UsageLines(pCustomerID, pServiceTypeID);

      // sort the data so that the first record is the oldest date is priority
      _UsageData.Sort((x,y) => x.LineDate.CompareTo(y.LineDate));
      /// Go through the records starting at the top in descending order and calculate the average cup count excluding SwopStart and SwopStop
      /// and catering for holiday periods

      if (_UsageData.Count > 1)   // must have at least to values to calcualte an average
      {
        _PrevDate = _UsageData[0].LineDate;
        _PrevCupCount = _UsageData[0].CupCount;

        while ((_UsageData.Count > _UsageLineNo) && (_NumValues < CONST_MAXROLLINGAVEVALUES))
        {
          _ThisServiceTypeID = _UsageData[_UsageLineNo].ServiceTypeID;

          // Only process correctly sequenced entries - otherwise we get impossible results (remember we are reading backwards)
          if (_PrevCupCount < _UsageData[_UsageLineNo].CupCount)  // is this bogus data so ignore it
          {
            if ((_ThisServiceTypeID == pServiceTypeID) && (_UsageData[_UsageLineNo].LineDate > _PrevDate))
            {
              // we are at the next record so cacl difference
              _CupsSincePrev = (_UsageData[_UsageLineNo].CupCount - _PrevCupCount);      // number of cups consumed
              if (pPerDayCalc)
              {
                _DaysSincePrev = (_UsageData[_UsageLineNo].LineDate - _PrevDate).Days;     // number of days between types
                _ThisAverage = Math.Round((double)(_CupsSincePrev / _DaysSincePrev), 5);
                _AverageConsumptions.Add(_ThisAverage);   // round to the nearest 5th decimal
              }
              else
              {
                _AverageConsumptions.Add(_CupsSincePrev);   // round to the nearest 5th decimal
              }
              // now add an average per day              
              _NumValues++;
              _PrevDate = _UsageData[_UsageLineNo].LineDate;
              _PrevCupCount = _UsageData[_UsageLineNo].CupCount;
            }
            // Check if they were on holiday and deduct that from total days
            else if ((_ThisServiceTypeID >= TrackerTools.CONST_SERVTYPE1WKHOLI) && (_ThisServiceTypeID <= TrackerTools.CONST_SERVTYPE2MTHHOLI))
            {
              _PrevDate = _PrevDate.AddDays(DaysToAddToDate(_ThisServiceTypeID));
            }
          }
          _UsageLineNo++;   // move to next record
        }  // while

        // now if there are more than 5 values kill the smallest
        if (_AverageConsumptions.Count >= CONST_MAXROLLINGAVEVALUES)
        {
          double _MaxAve = _AverageConsumptions[0];
          for (int i = 1; i < _AverageConsumptions.Count; i++)
          {
            if (_AverageConsumptions[i] > _MaxAve)
              _MaxAve = _AverageConsumptions[i];
          }
          _AverageConsumptions.Remove(_MaxAve);
        }
        // now calc the Average
        _TotalAverages = 0;
        for (int i = 0; i < _AverageConsumptions.Count; i++)
          _TotalAverages += _AverageConsumptions[i];

        if (_TotalAverages > 0)
          _AveConsumption = Math.Round(_TotalAverages / _AverageConsumptions.Count, 3);
      }
      else
      {
        if (pPerDayCalc)
          _AveConsumption = TrackerTools.CONST_TYPICALAVECONSUMPTION;
        else
          _AveConsumption = pTypicalAverageConsumption;
      }

    
      // check for negative or 0
      if (_AveConsumption <= 0)
        _AveConsumption = pTypicalAverageConsumption;
      
      return _AveConsumption;
    }

//    public double CalcDailyAverageForServiceItem(double pTypicalAvePerItem, int pServiceTypeID)
//    {
        
          #region OldVBACodeCalc
        //   Dim rstClientUsageLines As Recordset
        //   Dim strSQL As String
        //   Dim dt As Date
        //   Dim iLastCount, iCurrCount, iLastQty, iQty As Long
        //   Dim dtCurr As Date

        //   'Find the latest record with service type = "Filter"
        //'   GetNextRequireDate = ConstNullDate

        //   strSQL = "SELECT TOP 6 ClientUsageLinesTbl.Date, ClientUsageLinesTbl.CupCount, ClientUsageLinesTbl.Qty "
        //   strSQL = strSQL + " FROM ClientUsageLinesTbl"
        //   strSQL = strSQL + " WHERE ClientUsageLinesTbl.CustomerID = " + pCustomerID
        //   strSQL = strSQL + " AND ClientUsageLinesTbl.ServiceTypeID = " + sType
        //   strSQL = strSQL + " ORDER BY ClientUsageLinesTbl.Date DESC"

        //   Set rstClientUsageLines = CurrentDb.OpenRecordset(strSQL)

        //   ' set the variables to default so that
        //   nAve = 0
        //   iCurrCount = 0
        //   dtCurr = ConstNullDate

        //   ' Get the initial data if available
        //   If Not rstClientUsageLines.EOF Then
        //     rstClientUsageLines.MoveFirst ' Order is DESC

        //     iLastCount = rstClientUsageLines("CupCount")
        //     iCurrCount = iLastCount
        //     dtCurr = rstClientUsageLines("Date")
        //     iLastQty = rstClientUsageLines("Qty")
        //     If IsNull(iLastQty) Then
        //       iLastQty = 1
        //     End If
        //   Else
        //     ' Default Value is a rough estimate when they will need this type
        //     nAve = iDefVal
        //   End If

        //   ' take the last cup count and deducting the current count, dividing by the last qty calculate the
        //   ' difference and then roll the average

        //   Do While Not rstClientUsageLines.EOF
        //     'Calc a rolling average
        //     If nAve = 0 Then
        //       nAve = (iLastCount - rstClientUsageLines("CupCount")) / iLastQty
        //     Else
        //       nAve = (nAve + (iLastCount - rstClientUsageLines("CupCount")) / iLastQty) / 2
        //     End If

        //     iLastCount = rstClientUsageLines("CupCount")
        //     iLastQty = rstClientUsageLines("Qty")

        //     If IsNull(iLastQty) Then
        //       iLastQty = 1
        //     End If
        //     rstClientUsageLines.MoveNext

        //   Loop

        //   ' At this point we either have a average or zeros
        //   If nAve = 0 Then
        //     ' if 0- then one record was found, so set it to the last qty
        //     nAve = iLastCount / iLastQty
        //   End If

        //   ' if CurrCount is still zero there where no records, so next required date is initial date + nAve
        //   If iCurrCount = 0 Then
        //     dtCurr = LookupInstallDate(pCustomerID)
        //   End If

        //   ' Now either we have the default value or an average
        //   dtCurr = DateAdd("d", Round(nAve / nCupAve, 0), dtCurr)
        //'   d = dtCurr
        //   CalcNextRequiredDate = RemoveTimePortion(dtCurr)

        //   ' close the record set
        //   rstClientUsageLines.Close


        #endregion

//      return _Average;
//    }
    /// <summary>
    /// Set the service data for this client
    /// </summary>
    /// <param name="pCustomerID"></param>
    /// <param name="pTrackedServiceItemData"></param>
    /// <param name="pDailyAverage"></param>
    /// <returns></returns>
    private ClientServiceItem SetClientServiceData(long pCustomerID, TrackedServiceItemTbl pTrackedServiceItemData, double pDailyAverage)
    {
      int _DaysLeftForItem;
      bool _IsPerDayCalc =  (pDailyAverage == 0);

      ClientServiceItem _ClientServiceItem = new ClientServiceItem();

      _ClientServiceItem.UsageDateFieldName = pTrackedServiceItemData.UsageDateFieldName;
      _ClientServiceItem.UsageAveFieldName = pTrackedServiceItemData.UsageAveFieldName;
      _ClientServiceItem.ThisItemsAverage = 
        CalcAveConsumption(pCustomerID, pTrackedServiceItemData.ServiceTypeID, pTrackedServiceItemData.TypicalAvePerItem, _IsPerDayCalc);

      /*NextDate for item is equal to LastDate + DaysLeftOfItemUse; 
       * DaysLeftOfItemUse = (QtyLastProvided X TypicalUsePerKg) / KgDailConsumption */

      LineUsageData _LatestData = GetLatestUsageData(pCustomerID, pTrackedServiceItemData.ServiceTypeID);  // get the last record in the database

      // check for minb date, and allocate the result to install date
      if (_LatestData.UsageDate == DateTime.MinValue)
        _LatestData.UsageDate = GetInstallDate(pCustomerID);
      //// xxx
      //  this is wrong we need to calculate the days left so we need to times qty by typical use per qty and then divide by average
      // daysleft = (lastQty * lastAveForItem) / ThisItemsAverage
      if (_IsPerDayCalc)
        _DaysLeftForItem = (int)Math.Round(((_LatestData.LastQty * TrackerTools.CONST_TYPICALNUMCUPSPERKG) / _ClientServiceItem.ThisItemsAverage), 0);
      else
        _DaysLeftForItem = (int)Math.Round((_LatestData.LastQty * _ClientServiceItem.ThisItemsAverage) / pDailyAverage, 0);

      // set the date deepending on holidays
      _ClientServiceItem.NextUsageDate = AddHolidayExtension(pCustomerID, _LatestData.UsageDate.AddDays(_DaysLeftForItem), pTrackedServiceItemData.ServiceTypeID);

      return _ClientServiceItem;
    }
    public bool InsertClientUsageTable(long pCustomerID, List<ClientServiceItem> pClientServiceItems)
    {
      bool _Success = true;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlInsert = "INSERT INTO ClientUsageTbl (CustomerID,";
        string _sqlValues = ") VALUES (?, ";


        for (int i = 0; i < pClientServiceItems.Count; i++)
        {
          _sqlInsert += pClientServiceItems[i].UsageDateFieldName + ", ";
          _sqlInsert += pClientServiceItems[i].UsageAveFieldName + ", ";

          _sqlValues += "?, ?, ";
        }

        _sqlInsert = _sqlInsert.Remove(_sqlInsert.Length - 2, 2); // remove last ","
        _sqlValues = _sqlValues.Remove(_sqlValues.Length - 2, 2); // remove last ","
        _sqlInsert += _sqlValues + ")";
        OleDbCommand _cmd = new OleDbCommand(_sqlInsert, _conn);                    // prepare the qurey we have built
        #region Parameters
        // first the customer ID
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });
        for (int i = 0; i < pClientServiceItems.Count; i++)
        {
          _cmd.Parameters.Add(new OleDbParameter { Value = pClientServiceItems[i].NextUsageDate });
          _cmd.Parameters.Add(new OleDbParameter { Value = pClientServiceItems[i].ThisItemsAverage });
        }
        #endregion
        try
        {
          _conn.Open();
          _cmd.ExecuteNonQuery();
        }
        catch (Exception _ex)
        {
          _Success = string.IsNullOrEmpty(_ex.Message);
          throw;
        }
        finally
        {
          _conn.Close();
        }
        return _Success;
      }
    }
    //
    public bool UpdateClientUsageTable(long pCustomerID, List<ClientServiceItem> pClientServiceItems)
    {
      bool _Success = true;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlUpdate = "UPDATE ClientUsageTbl SET ";
        for (int i = 0; i < pClientServiceItems.Count; i++)
        {
          _sqlUpdate += pClientServiceItems[i].UsageDateFieldName + " = ?, ";
          _sqlUpdate += pClientServiceItems[i].UsageAveFieldName + " = ?, ";
        }

        _sqlUpdate = _sqlUpdate.Remove(_sqlUpdate.Length - 2,2); // remove last ","
        _sqlUpdate += " WHERE CustomerID = ?";
        OleDbCommand _cmd = new OleDbCommand(_sqlUpdate, _conn);                    // prepare the qurey we have built
        #region Parameters
        for (int i = 0; i < pClientServiceItems.Count; i++)
        {
          _cmd.Parameters.Add(new OleDbParameter { Value = pClientServiceItems[i].NextUsageDate });
          _cmd.Parameters.Add(new OleDbParameter { Value = pClientServiceItems[i].ThisItemsAverage });
        }
        // Where clause
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });
        #endregion
        try
        {
          _conn.Open();
          _cmd.ExecuteNonQuery();
        }
        catch (Exception _ex)
        {
          _Success = string.IsNullOrEmpty(_ex.Message);
          throw;
        }
        finally
        {
          _conn.Close();
        }
        return _Success;
      }
    }

    /// <summary>
    /// Generic Function to get the next date of a serivce type, depending on average cup count and last date an qty added
    /// </summary>
    /// <param name="pCustomerID">ID of the customer to check/param>
    /// <param name="iDefDate">default date value</param>
    /// <param name="pServiceType">the Service Type to Check</param>
    /// <returns>NextRequireDate of an item</returns>
    /// <remarks>
    /// Logic: using the customer see if the service type has been consumed, if so calculate the average between each date of use
    /// otherwise use the AverageConsumption and the install date to guess the next date. Return that date
    /// </remarks>
    public bool CalcAndSaveNextRequiredDates(long pCustomerID) // , long pLastCupCount)
    {
      bool _Success = false;
      #region LogicCalcAndSave
      /* 
       * logic:
       * 
       * 1. retrieve the list of items from TrackedServiceItemTbl, so that the ThisItemSetsDailyAverage is at the top an the others are below
       * for each item:
       * 2. retrieve the last item of this service type provided.
       * 
       * 3a. if the date it was last provided > install date, then:
       *   calculate the average use for that item and set the next_date_required it to the last_date + days to go
       *   if this is serviceitemcoffee then set daily average variable to be used for general averages
       * 3b. else
       *   using the typical average for the item add that to install date and using the daily average calculate when next will need
       *   if this date is < now make it now
       * 
       * 4. for each item save this to the summary table for the client, or at least save eac hitem to a list then use the list to
       * create an update command to update all items for this customer as per the fileds retrieved.
       * 
       */
      #endregion

      // 1. Retrieve list of TrackedServiceItemTbl, so that the Theh Item that SetsDailyAverage is at the top
      TrackedServiceItemTbl _TrackedServiceItemTbl = new TrackedServiceItemTbl();
      List<TrackedServiceItemTbl> _TrackerServiceItems = _TrackedServiceItemTbl.GetAll("ThisItemSetsDailyAverage, ServiceTypeID");

      double _DailyAverage = TrackerTools.CONST_TYPICALAVECONSUMPTION;
      int _ServiceItemNum = 0;
      List<ClientServiceItem> _ClientServiceItems = new List<ClientServiceItem>();

      // do the primary item
      while (_TrackerServiceItems[_ServiceItemNum].ThisItemSetsDailyAverage)     // should be only one but just in case
      {
        ClientServiceItem _ClientServiceItem = SetClientServiceData(pCustomerID, _TrackerServiceItems[_ServiceItemNum],0);  // 0 means set Daily Average
        _DailyAverage = _ClientServiceItem.ThisItemsAverage; 
        _ClientServiceItems.Add(_ClientServiceItem);
        _ServiceItemNum++;
      }
      // now do the secpndary items
      while (_ServiceItemNum < _TrackerServiceItems.Count)     // should be pnly one bu just in case
      {
        ClientServiceItem _ClientServiceItem = SetClientServiceData(pCustomerID, _TrackerServiceItems[_ServiceItemNum], _DailyAverage);  
        _ClientServiceItems.Add(_ClientServiceItem);
        _ServiceItemNum++;
      }
      //

      #region OldVBACodeCalc

      //_NumDays = (int)Math.Round((200 * _LastCleanUpdateData.LastQty) / _AveConsump);
      //if (_LastCleanUpdateData.UsageDate.CompareTo(DateTime.MinValue)     // NULLDATE == 0)
      //  _dtNextClean.AddDays(_NumDays);
      //// Adjust for holidays if neccessary
      //_dtNextClean = AddHolidayExtension(pCustomerID, _dtNextClean, TrackerTools.CONST_STRING_SERVTYPECLEAN)
      // //    lblStatus.Caption = "Doing next filter, decal and service calculations"
      ////    //Do FitlerDate calc
      //    // if they are clients the require us to predict when they will need fitlers or decalificaiton
      //    // tablets then get the Last Date
      //    dtNextFilter = CalcNextRequiredDate(pCustomerID, 300, AveConsump, CONST_SERVTYPEFilter, nFilterAve)
      //    dtNextDescal = CalcNextRequiredDate(pCustomerID, 350, AveConsump, CONST_SERVTYPEDescale, nDescaleAve)
      //    dtNextService = CalcNextRequiredDate(pCustomerID, 5500, AveConsump, CONST_SERVTYPEService, nServiceAve)
      //    //   LastServiceDate = LookupLastServiceDate()
      //    // if there has already been a filter then use average otherwise assume 220 as the cup count
      //Else
      //  // do default values
      //  If dtInstall <> ConstNullDate Then
      //    dtNextCoffee = DateAdd("d", 14, dtInstall)
      //    dtNextClean = DateAdd("d", 28, dtInstall)   // Assume it is at least 28 days away
      #endregion

      /* 4. for each item save this to the summary table for the client, or at least save eac hitem to a list then use the list to
       * create an update command to update all items for this customer as per the fileds retrieved.*/
      // check if a record exists in the client usage table
      ClientUsageTbl _ClientUsageTbl = new ClientUsageTbl();

      if (_ClientUsageTbl.UsageDataExists(pCustomerID))
        _Success = UpdateClientUsageTable(pCustomerID, _ClientServiceItems);
      else
        _Success = InsertClientUsageTable(pCustomerID, _ClientServiceItems);

      return _Success;
    }

    /// <summary>
    /// Update the predictive dates for the customer ID provided
    /// </summary>
    /// <param name="pCustomerID">customer di to update</param>
    /// <param name="pLastCount">the last count known</param>
    /// <param name="pDefaultDate">The date to set if the client has no orders</param>
    public bool UpdatePredictions(long pCustomerID, long pLastCount)
    {
      bool _Success = false;
      // We need to update all the items that are affected by the latest update. First select the last row in client usage table, 
      // then determine what has changed.  Now retrieve the last line selected clientusageline of client that is selected on the current form
      // round ave consumption up to the nearest 2 decimals

      DateTime _dtInstall = GetInstallDate(pCustomerID);
      if ((pLastCount > 0) && (_dtInstall != DateTime.MinValue))     // NULLDATE
      {
        CalcAndSaveNextRequiredDates(pCustomerID); //, pLastCount);
      }
      else // do defaults
      {
        double _AveConsump = CalcAveConsumption(pCustomerID);
        //---> this is stupid maths (x*y)/y = x     AveConsump = Round((AveConsump * ConstTypicalNumCupsPerKg) / ConstTypicalNumCupsPerKg, 2)
        if (_AveConsump <= 0)
          _AveConsump = TrackerTools.CONST_TYPICALAVECONSUMPTION;   // if the value makes not sens the force it; should perhaps look at a average that is system wide?

        DateTime _dtNextCoffee = DateTime.Now.AddDays(20);

        ClientUsageTbl _UsageData = new ClientUsageTbl();
        _UsageData.CustomerID = pCustomerID;
        _UsageData.LastCupCount = pLastCount;
        _UsageData.NextCoffeeBy = _dtNextCoffee;
        _UsageData.NextCleanOn =_dtNextCoffee.AddDays(20);
        _UsageData.NextFilterEst = _dtNextCoffee.AddDays(30);
        _UsageData.NextDescaleEst = _dtNextCoffee.AddDays(30);   // Descale
        _UsageData.NextServiceEst = _dtNextCoffee.AddYears(1);   // Service
        _UsageData.DailyConsumption = _AveConsump;   // DailyConsumption
        _UsageData.CleanAveCount= TrackerTools.CONST_TYPICALCLEAN_CONSUMPTION;   // CleanAve
        _UsageData.FilterAveCount = TrackerTools.CONST_TYPICALFILTER_CONSUMPTION;   // FilterAve
        _UsageData.DescaleAveCount = TrackerTools.CONST_TYPICALDECAL_CONSUMPTION;   // Descale Ave
        _UsageData.ServiceAveCount = 10000;   // Service Ave

        ClientUsageTbl _UsageDAL = new ClientUsageTbl();
        _UsageDAL.Update(_UsageData);
      }  // else

      return _Success;

    }
    /// <summary>
    /// Reset the custoemr reminder count and if needed force the client to be enables
    /// </summary>
    /// <param name="pCustomer">the customder ID </param>
    /// <param name="pForceEnable">Force the custoemr to be enabled?</param>
    /// <returns>success or failure</returns>
    public bool ResetCustomerReminderCount(long pCustomerID, bool pForceEnable)
    {
      CustomersTbl _CustomersTbl = new CustomersTbl();
      return _CustomersTbl.ResetReminderCount(pCustomerID, pForceEnable);
    }
  }
}