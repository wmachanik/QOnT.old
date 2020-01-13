/// --- auto generated class for table: ClientUsageTbl
using System;   // for DateTime variables
using System.Collections.Generic;      // for data stuff
using System.Data.OleDb;
using System.Configuration;
using System.Web;
using QOnT.classes;
using System.Data;

namespace QOnT.control
{
  public class ClientUsageTbl
  {
    // internal variable declarations
    private long _CustomerID;
    private long _LastCupCount;
    private DateTime _NextCoffeeBy;
    private DateTime _NextCleanOn;
    private DateTime _NextFilterEst;
    private DateTime _NextDescaleEst;
    private DateTime _NextServiceEst;
    private double _DailyConsumption;
    private double _FilterAveCount;
    private double _DescaleAveCount;
    private double _ServiceAveCount;
    private double _CleanAveCount;
    // class definition
    public ClientUsageTbl()
    {
      _CustomerID = 0;
      _LastCupCount = 0;
      _NextCoffeeBy = System.DateTime.Now;
      _NextCleanOn = System.DateTime.Now;
      _NextFilterEst = System.DateTime.Now;
      _NextDescaleEst = System.DateTime.Now;
      _NextServiceEst = System.DateTime.Now;
      _DailyConsumption = 0.0;
      _FilterAveCount = 0.0;
      _DescaleAveCount = 0.0;
      _ServiceAveCount = 0.0;
      _CleanAveCount = 0.0;
    }
    // get and sets of public
    public long CustomerID { get { return _CustomerID; } set { _CustomerID = value; } }
    public long LastCupCount { get { return _LastCupCount;}  set { _LastCupCount = value;} }
    public DateTime NextCoffeeBy { get { return _NextCoffeeBy;}  set { _NextCoffeeBy = value;} }
    public DateTime NextCleanOn { get { return _NextCleanOn;}  set { _NextCleanOn = value;} }
    public DateTime NextFilterEst { get { return _NextFilterEst;}  set { _NextFilterEst = value;} }
    public DateTime NextDescaleEst { get { return _NextDescaleEst;}  set { _NextDescaleEst = value;} }
    public DateTime NextServiceEst { get { return _NextServiceEst;}  set { _NextServiceEst = value;} }
    public double DailyConsumption { get { return _DailyConsumption;}  set { _DailyConsumption = value;} }
    public double FilterAveCount { get { return _FilterAveCount;}  set { _FilterAveCount = value;} }
    public double DescaleAveCount { get { return _DescaleAveCount;}  set { _DescaleAveCount = value;} }
    public double ServiceAveCount { get { return _ServiceAveCount;}  set { _ServiceAveCount = value;} }
    public double CleanAveCount { get { return _CleanAveCount;}  set { _CleanAveCount = value;} }

    #region ConstantDeclarations
    const string CONST_CONSTRING = QOnT.classes.TrackerDb.CONST_CONSTRING;
    const string CONST_SQL_SELECT = "SELECT TOP 1 LastCupCount, NextCoffeeBy, NextCleanOn, NextFilterEst, NextDescaleEst, NextServiceEst, DailyConsumption, FilterAveCount, DescaleAveCount, ServiceAveCount, CleanAveCount " +
                                          " FROM ClientUsageTbl WHERE CustomerID = ?";
    const string CONST_SQL_ISARECORD = "SELECT TOP 1 LastCupCount FROM ClientUsageTbl WHERE CustomerID = ?";
    const string CONST_SQL_SELECTDAILYCONSUMPTION = "SELECT TOP 1 DailyConsumption FROM ClientUsageTbl WHERE CustomerID = ?";
    const string CONST_SQL_UPDATE = "UPDATE ClientUsageTbl SET LastCupCount = ?, NextCoffeeBy= ?, NextCleanOn= ?, NextFilterEst= ?, NextDescaleEst= ?, NextServiceEst= ?," +
                                                              "DailyConsumption= ?, CleanAveCount = ?, FilterAveCount = ?, DescaleAveCount = ?, ServiceAveCount = ? " +
                                                         " WHERE CustomerID = ?";
    const string CONST_SQL_FORCENEXCOFFEETBY = "UPDATE ClientUsageTbl SET NextCoffeeBy = ? WHERE CustomerID = ?";
    #endregion

    public ClientUsageTbl GetUsageData(long pCustomerID)
    {
      ClientUsageTbl _DataItem = new ClientUsageTbl();
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        try
        {
          OleDbCommand _cmd = new OleDbCommand(CONST_SQL_SELECT, _conn);                    // run the qurey we have built
          _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });
          _conn.Open();
          OleDbDataReader _DataReader = _cmd.ExecuteReader();
          if (_DataReader.Read())
          {
            _DataItem.CustomerID = pCustomerID;
            _DataItem.LastCupCount = (_DataReader["LastCupCount"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["LastCupCount"]);
            _DataItem.NextCoffeeBy = (_DataReader["NextCoffeeBy"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["NextCoffeeBy"]);
            _DataItem.NextCleanOn = (_DataReader["NextCleanOn"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["NextCleanOn"]);
            _DataItem.NextFilterEst = (_DataReader["NextFilterEst"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["NextFilterEst"]);
            _DataItem.NextDescaleEst = (_DataReader["NextDescaleEst"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["NextDescaleEst"]);
            _DataItem.NextServiceEst = (_DataReader["NextServiceEst"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["NextServiceEst"]);
            _DataItem.DailyConsumption = (_DataReader["DailyConsumption"] == DBNull.Value) ? 0.0 : Convert.ToDouble(_DataReader["DailyConsumption"]);
            _DataItem.FilterAveCount = (_DataReader["FilterAveCount"] == DBNull.Value) ? 0.0 : Convert.ToDouble(_DataReader["FilterAveCount"]);
            _DataItem.DescaleAveCount = (_DataReader["DescaleAveCount"] == DBNull.Value) ? 0.0 : Convert.ToDouble(_DataReader["DescaleAveCount"]);
            _DataItem.ServiceAveCount = (_DataReader["ServiceAveCount"] == DBNull.Value) ? 0.0 : Convert.ToDouble(_DataReader["ServiceAveCount"]);
            _DataItem.CleanAveCount = (_DataReader["CleanAveCount"] == DBNull.Value) ? 0.0 : Convert.ToDouble(_DataReader["CleanAveCount"]);
          }
        }
        catch (Exception _ex)
        {
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
          throw;
        }
        finally
        {
          _conn.Close();
        }
      }
      return _DataItem;
    }
    public bool UsageDataExists(long pCustomerID)
    {
      bool _Exists = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        try
        {
          OleDbCommand _cmd = new OleDbCommand(CONST_SQL_ISARECORD, _conn);                    // run the qurey we have built
          _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });
          _conn.Open();
          OleDbDataReader _DataReader = _cmd.ExecuteReader();
          _Exists = _DataReader.HasRows;
        }
        catch (Exception _ex)
        {
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
          throw;
        }
        finally
        {
          _conn.Close();
        }
      }
      return _Exists;
    }
    public double GetAverageConsumption(long pCustomerID)
    {
      double _Ave = QOnT.classes.TrackerTools.CONST_TYPICALAVECONSUMPTION;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;
      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        try
        {
          OleDbCommand _cmd = new OleDbCommand(CONST_SQL_SELECTDAILYCONSUMPTION, _conn);                    // run the qurey we have built
          _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });
          _conn.Open();
          OleDbDataReader _DataReader = _cmd.ExecuteReader();
          if (_DataReader.Read())
          {
            _Ave = (_DataReader["DailyConsumption"] == DBNull.Value) ? 0.0 : Convert.ToDouble(_DataReader["DailyConsumption"]);
          }
        }
        catch (Exception _ex)
        {
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
          throw;
        }
        finally
        {
          _conn.Close();
        }
      }
      return _Ave;
    }
    /// <summary>
    /// Updatge the cup count for this client
    /// </summary>
    /// <param name="pCustomerID">client customerid</param>
    /// <param name="pLastCupCount">cup count to be set</param>
    /// <returns></returns>
    public bool UpdateUsageCupCount(long pCustomerID, long pLastCupCount)
    {
      bool _Success = true;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = "UPDATE ClientUsageTbl SET LastCupCount = ? WHERE CustomerId = ?";

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // prepare the qurey we have built
        #region Parameters
        _cmd.Parameters.Add(new OleDbParameter { Value = pLastCupCount });
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
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
          _Success = false;
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
    /// Update the Customer Usage (ID passed as part of ClientUsage data) to reflect new values
    /// </summary>
    /// <param name="pClientUsage">Client Usage data for ClientUsage.CustomerID</param>
    /// <returns>blank or error strin</returns>
    public bool Update(ClientUsageTbl pClientUsage)
    {
      bool _Success = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_UPDATE;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        #region Parameters
        // Add data sent LastCupCount, NextCoffeeBy, NextCleanOn, NextFilterEst, NextDescaleEst, NextServiceEst,
        //               DailyConsumption, CleanAveCount, FilterAveCount, DescaleAveCount, ServiceAveCount
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsage.LastCupCount });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsage.NextCoffeeBy });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsage.NextCleanOn});
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsage.NextFilterEst });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsage.NextDescaleEst });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsage.DailyConsumption });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsage.CleanAveCount });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsage.FilterAveCount });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsage.DescaleAveCount });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsage.ServiceAveCount });
        //     WHERE CustomerID = ?
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsage.CustomerID });
        #endregion
        try
        {
          _conn.Open();
          _Success = (_cmd.ExecuteNonQuery() >= 0);
        }
        catch (OleDbException _ex )
        {
          // Handle exception.
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
          _Success = false;
        }
        finally
        {
          _conn.Close();
        }
        return _Success;
      }
    }
    /// <summary>
    /// Update the Customer Usage (ID passed as part of ClientUsage data) to reflect new values
    /// </summary>
    /// <param name="pClientUsage">Client Usage data for ClientUsage.CustomerID</param>
    /// <returns>blank or error strin</returns>
    public bool ForceNextCoffeeDate(DateTime pNextDate, long pCustomerID)
    {
      bool _Success = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[QOnT.classes.TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_FORCENEXCOFFEETBY;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        #region Parameters
        // Add data sent NextCoffeeBy
        _cmd.Parameters.Add(new OleDbParameter { Value = pNextDate, OleDbType = OleDbType.Date });
        //     WHERE CustomerID = ?
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID, OleDbType = OleDbType.Integer});
        #endregion
        try
        {
          _conn.Open();
          _Success = (_cmd.ExecuteNonQuery() >= 0);
        }
        catch (OleDbException _ex)
        {
          // Handle exception.
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
          _Success = false;
        }
        finally
        {
          _conn.Close();
        }
      }
      return _Success;
    }

  }
}
