/// --- auto generated class for table: ClientUsageLinesTbl
using System;   // for DateTime variables
using System.Collections.Generic;
using System.Data.OleDb;
using System.Configuration;
using QOnT.classes;

namespace QOnT.control
{
  public class ClientUsageLinesTbl
  {
    // internal variable declarations
    private int _ClientUsageLineNo;
    private long _CustomerID;
    private DateTime _LineDate;
    private long _CupCount;
    private int _ServiceTypeID;
    private double _Qty;
    private string _Notes;
    // class definition
    public ClientUsageLinesTbl()
    {
      _ClientUsageLineNo = 0;
      _CustomerID = 0;
      _LineDate = System.DateTime.Now;
      _CupCount = 0;
      _ServiceTypeID = 0;
      _Qty = 0.0;
      _Notes = string.Empty;
    }
    // get and sets of public
    public int ClientUsageLineNo { get { return _ClientUsageLineNo; } set { _ClientUsageLineNo = value; } }
    public long CustomerID { get { return _CustomerID; } set { _CustomerID = value; } }
    public DateTime LineDate { get { return _LineDate; } set { _LineDate = value; } }
    public long CupCount { get { return _CupCount; } set { _CupCount = value; } }
    public int ServiceTypeID { get { return _ServiceTypeID; } set { _ServiceTypeID = value; } }
    public double Qty { get { return _Qty; } set { _Qty = value; } }
    public string Notes { get { return _Notes; } set { _Notes = value; } }
    
    #region ConstantDeclarations
    const string CONST_SQL_SELECT = "SELECT ClientUsageLineNo, [Date] AS LineDate, CupCount, ServiceTypeID, Qty, Notes FROM ClientUsageLinesTbl ";
    const string CONST_SQL_UPDATE = "UPDATE ClientUsageLinesTbl SET CustomerID = ?, [Date] = ?, CupCount = ?, ServiceTypeID = ? , Qty = ?, " +
                                    "Notes = ? WHERE ClientUsageLineNo = ? ";
    const string CONST_SQL_INSERT = "INSERT INTO ClientUsageLinesTbl (CustomerID, [Date], CupCount, ServiceTypeID, Qty, Notes) " +
                                                     " VALUES (?, ?, ?, ?, ?, ?)";
    #endregion

    public List<ClientUsageLinesTbl> GetAllClientUsageLinesTbl(long pCustomerID, string SortBy)
    {
      List<ClientUsageLinesTbl> _DataItems = new List<ClientUsageLinesTbl>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_SELECT + " WHERE CustomerID = " + pCustomerID.ToString();
        if (!String.IsNullOrEmpty(SortBy)) _sqlCmd += " ORDER BY " + SortBy;     // Add order by string
        
        TrackerDb _TrackerDB = new TrackerDb();
        OleDbDataReader _DataReader = _TrackerDB.ReturnDataReader(_sqlCmd);
        while (_DataReader.Read())
        {
          ClientUsageLinesTbl _DataItem = new ClientUsageLinesTbl();

          _DataItem.ClientUsageLineNo = (_DataReader["ClientUsageLineNo"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ClientUsageLineNo"]);
          _DataItem.CustomerID = (_DataReader["CustomerID"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["CustomerID"]);
          _DataItem.LineDate = (_DataReader["LineDate"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["LineDate"]);
          _DataItem.CupCount = (_DataReader["CupCount"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["CupCount"]);
          _DataItem.ServiceTypeID = (_DataReader["ServiceTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ServiceTypeID"]);
          _DataItem.Qty = (_DataReader["Qty"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["Qty"]);
          _DataItem.Notes = (_DataReader["Notes"] == DBNull.Value) ? string.Empty : _DataReader["Notes"].ToString();
          _DataItems.Add(_DataItem);
        }
      }
      return _DataItems;
    }
    public bool InsertItemsUsed(ClientUsageLinesTbl pClientUsageLine)
    {
      bool _Success = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_INSERT;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        #region Parameters
        // Add data sent
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.CustomerID });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.LineDate });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.CupCount });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.ServiceTypeID });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.Qty });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.Notes });
        /// -> auto generater _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLineNo });
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
        return _Success;
      }
    }
    public bool UpdateItemsUsed(ClientUsageLinesTbl pClientUsageLine, long OriginalClientUsageLineNo)
    {
      bool _Success = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_UPDATE;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        #region Parameters
        // Add data sent
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.CustomerID });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.LineDate });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.CupCount });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.ServiceTypeID });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.Qty });
        _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLine.Notes });
        //                                     " WHERE ClientUsageLineNo = ?)";
        _cmd.Parameters.Add(new OleDbParameter { Value = OriginalClientUsageLineNo });
        #endregion
        try
        {
          _conn.Open();
          _Success = (_cmd.ExecuteNonQuery() == 0);
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
        return _Success;
      }
    }

    /* 
     ****** Below are all the routine to do specific data retrival from ClientUsageLinesTBL  *****
     */ 
    /// <summary>
    /// Lookup the customers Install Date, ie the first date they used us. Otherwise set to min
    /// </summary>
    /// <param name="pCustomerID">the customer ID whose install date you want</param>
    /// <returns>the first date the customer has in record</returns>
    public DateTime LookupCustomerInstallDate(long pCustomerID)
    {
      DateTime _GetInstallDate = DateTime.MinValue;     // NULLDATE
      string _sqlCmd = "SELECT MIN(ClientUsageLinesTbl.Date) FROM ClientUsageLinesTbl WHERE ClientUsageLinesTbl.CustomerID = ?";
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });

        try
        {
          _conn.Open();
          OleDbDataReader _DataReader = _cmd.ExecuteReader();
          if (_DataReader.Read() && (_DataReader[0] != null))
            _GetInstallDate = (DateTime)_DataReader[0];
        }
        catch (Exception _ex)
        {
          // Handle exception.
          TrackerTools _Tools = new TrackerTools();
          _Tools.SetTrackerSessionErrorString(_ex.Message);
        }
        finally
        {
          _conn.Close();
        }
      }

      return _GetInstallDate;
    }
    /// <summary>
    /// Return a list of client data for a 
    /// </summary>
    /// <param name="pCustomerID">for which customer</param>
    /// <param name="pServiceTypeID">what service type (blank for coffee)</param>
    /// <param name="pSortBy">How to sort normally (blank fo default "ClientUsageLinesTbl.Date DESC") </param>
    /// <returns>List of ClientUsageTbl</returns>
    public List<ClientUsageLinesTbl> GetAllCustomerServiceLines(long pCustomerID)
    { return GetAllCustomerServiceLines(pCustomerID, TrackerTools.CONST_SERVTYPECOFFEE, "ClientUsageLinesTbl.Date DESC"); }
    public List<ClientUsageLinesTbl> GetAllCustomerServiceLines(long pCustomerID, int pServiceTypeID)
    { return GetAllCustomerServiceLines(pCustomerID, pServiceTypeID, "ClientUsageLinesTbl.Date DESC"); }
    // real routine
    public List<ClientUsageLinesTbl> GetAllCustomerServiceLines(long pCustomerID, int pServiceTypeID, string pSortBy)
    {
      string CONST_SQL_SELECT = "SELECT ClientUsageLineNo, CustomerID, Date, CupCount, ServiceTypeID, Qty, Notes "
                                + " FROM ClientUsageLinesTbl"
                                + " WHERE ClientUsageLinesTbl.CustomerID = ?"   // + pCustomerID
                                + " AND ClientUsageLinesTbl.ServiceTypeID = ?" ;

      List<ClientUsageLinesTbl> _DataItems = new List<ClientUsageLinesTbl>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;
      string _sqlCmd = CONST_SQL_SELECT;
      if (!String.IsNullOrEmpty(pSortBy)) _sqlCmd += " ORDER BY " + pSortBy;     // Add order by string

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });
        _cmd.Parameters.Add(new OleDbParameter { Value = pServiceTypeID });

        try
        {
          _conn.Open();
          OleDbDataReader _DataReader = _cmd.ExecuteReader();
          while (_DataReader.Read())
          {
            ClientUsageLinesTbl _DataItem = new ClientUsageLinesTbl();
            _DataItem.CustomerID = pCustomerID;
            _DataItem.ClientUsageLineNo = (_DataReader["ClientUsageLineNo"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ClientUsageLineNo"]);
            _DataItem.CustomerID = (_DataReader["CustomerID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["CustomerID"]);
            _DataItem.LineDate = (_DataReader["Date"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["Date"]);
            _DataItem.CupCount = (_DataReader["CupCount"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["CupCount"]);
            _DataItem.ServiceTypeID = (_DataReader["ServiceTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ServiceTypeID"]);
            _DataItem.Qty = (_DataReader["Qty"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["Qty"]);
            _DataItem.Notes = (_DataReader["Notes"] == DBNull.Value) ? string.Empty : _DataReader["Notes"].ToString();
            _DataItems.Add(_DataItem);
          }
        }
        catch (Exception _ex)
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
    /// <summary>
    /// Get the last 10 usage line of a clients data, pass ServiceType for the last 10 of that service type
    /// </summary>
    /// <param name="pCustomerID">customer id to get the data of</param>
    /// <param name="pServiceTypeId">for which service type</param>
    /// <returns>a list of Client usage lines</returns>}
    public List<ClientUsageLinesTbl> GetLast10UsageLines(long pCustomerID)  { return GetLast10UsageLines(pCustomerID,0); }
    public List<ClientUsageLinesTbl> GetLast10UsageLines(long pCustomerID, int pServiceTypeId)
    {
      List<ClientUsageLinesTbl> _DataItems = new List<ClientUsageLinesTbl>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      string _sqlCmd = "SELECT TOP 10 ClientUsageLineNo, CustomerID, [Date] AS LineDate, CupCount, ServiceTypeID " +
                       "FROM ClientUsageLinesTbl WHERE ClientUsageLinesTbl.CustomerID = ? ";

      if (pServiceTypeId > 0) 
        _sqlCmd +=  " AND ClientUsageLinesTbl.ServiceTypeID = ?" ;

      _sqlCmd += " ORDER BY ClientUsageLinesTbl.Date DESC";

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });
        if (pServiceTypeId > 0)
          _cmd.Parameters.Add(new OleDbParameter { Value = pServiceTypeId});

        try
        {
          _conn.Open();
          OleDbDataReader _DataReader = _cmd.ExecuteReader();
          while (_DataReader.Read())
          {
            ClientUsageLinesTbl _DataItem = new ClientUsageLinesTbl();
            _DataItem.CustomerID = pCustomerID;
            _DataItem.ClientUsageLineNo = (_DataReader["ClientUsageLineNo"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ClientUsageLineNo"]);
            _DataItem.CustomerID = (_DataReader["CustomerID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["CustomerID"]);
            _DataItem.LineDate = (_DataReader["LineDate"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["LineDate"]);
            _DataItem.CupCount = (_DataReader["CupCount"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["CupCount"]);
            _DataItem.ServiceTypeID = (_DataReader["ServiceTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ServiceTypeID"]);
            //_DataItem.Qty = (_DataReader["Qty"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["Qty"]);
            //_DataItem.Notes = (_DataReader["Notes"] == DBNull.Value) ? string.Empty : _DataReader["Notes"].ToString();
            _DataItems.Add(_DataItem);
          }
        }
        catch (Exception _ex)
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
    /// <summary>
    /// Return the latest data for the customer for a service type, or 0 for last item
    /// </summary>
    /// <param name="pCustomerID">the custoerm id</param>
    /// <param name="pServiceTypeID">default 0, means any service type otherwise pass the type.</param>
    /// <returns>data from that line</returns>
    public ClientUsageLinesTbl GetLatestUsageData(long pCustomerID, int pServiceTypeID)
    {
      ClientUsageLinesTbl _DataItem = new ClientUsageLinesTbl();

      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;
      string _sqlCmd = "SELECT TOP 1 ClientUsageLineNo, CustomerID, [Date] As LineDate, CupCount, ServiceTypeID, Qty " +
                            " FROM ClientUsageLinesTbl WHERE CustomerID = ? ";
      if (pServiceTypeID > 0)
        _sqlCmd += "AND ServiceTypeID = ?";
      
      // now make sure it is the last one
      _sqlCmd += " ORDER BY [Date] DESC";
      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _cmd.Parameters.Add(new OleDbParameter { Value = pCustomerID });
        if (pServiceTypeID > 0)
          _cmd.Parameters.Add(new OleDbParameter { Value = pServiceTypeID });

        try
        {
          _conn.Open();
          OleDbDataReader _DataReader = _cmd.ExecuteReader();
          if (_DataReader.Read() && (_DataReader[0] != null))
          {
            _DataItem.CustomerID = pCustomerID;
            _DataItem.ClientUsageLineNo = (_DataReader["ClientUsageLineNo"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ClientUsageLineNo"]);
            _DataItem.CustomerID = (_DataReader["CustomerID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["CustomerID"]);
            _DataItem.LineDate = (_DataReader["LineDate"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["LineDate"]);
            _DataItem.CupCount = (_DataReader["CupCount"] == DBNull.Value) ? 0 : Convert.ToInt64(_DataReader["CupCount"]);
            _DataItem.ServiceTypeID = (_DataReader["ServiceTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ServiceTypeID"]);
            _DataItem.Qty = (_DataReader["Qty"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["Qty"]);
          }
        }
        catch (Exception _ex)
        {
          // Handle exception.
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

  }

}
