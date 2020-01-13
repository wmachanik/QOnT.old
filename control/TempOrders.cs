using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.OleDb;
using QOnT.classes;

namespace QOnT.control
{

  public class TempOrdersData
  {
    private TempOrdersHeaderTbl _TempOrdersHeaderTbl;
    private List<TempOrdersLinesTbl> _TempOrdersLines;

    public TempOrdersData()
    {
      _TempOrdersHeaderTbl = new TempOrdersHeaderTbl();
      _TempOrdersLines = new List<TempOrdersLinesTbl>();
    }

    public TempOrdersHeaderTbl HeaderData { get { return _TempOrdersHeaderTbl; } set { _TempOrdersHeaderTbl = value; }}
    public List<TempOrdersLinesTbl> OrdersLines { get { return _TempOrdersLines; } set { _TempOrdersLines = value; } }
  }

  public class TempOrdersDAL
  {
    #region SQLDefinations
    const string CONST_SQL_UPDATEORDERSASDONE = "UPDATE OrdersTbl SET Done = True WHERE EXISTS " +
                                            "(SELECT OriginalOrderID FROM TempOrdersLinesTbl WHERE (OriginalOrderID = OrdersTbl.OrderID))";
    #endregion

    public bool Insert(TempOrdersData pTempOrder)
    {
      bool _Success = false;
      int _LineNo = 0;
      TempOrdersHeaderTbl _HeaderDAL = new TempOrdersHeaderTbl();
      TempOrdersLinesTbl _LinesDAL = new TempOrdersLinesTbl();

      _Success = _HeaderDAL.Insert(pTempOrder.HeaderData);
      long _TOHeaderID = _HeaderDAL.GetCurrentTOHeaderID();

      while ((_Success) && (pTempOrder.OrdersLines.Count > _LineNo))
      {
        pTempOrder.OrdersLines[_LineNo].TOHeaderID = _TOHeaderID;
        _Success = _LinesDAL.Insert(pTempOrder.OrdersLines[_LineNo]);
        _LineNo++;
      }

      return _Success;
    }
    /// <summary>
    /// Do the current lines have service type coffee in it
    /// </summary>
    /// <returns>if there is a service item coffee</returns>
    public bool HasCoffeeInTempOrder()
    {
      string _sqlCmd = "SELECT ServiceTypeID " +
                       " FROM TempOrdersLinesTbl" +
                       " WHERE ServiceTypeID = " + TrackerTools.CONST_STRING_SERVTYPECOFFEE;

      bool _HasCoffeeInTemp = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        // now get data from database
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built
        _conn.Open();
        try
        {
          OleDbDataReader _DataReader = _cmd.ExecuteReader();
          _HasCoffeeInTemp = (_DataReader != null) ? _DataReader.HasRows : false;   // if it has rows then they exists
        }
        catch (OleDbException _ex)
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
      return _HasCoffeeInTemp;
    }
    /// <summary>
    /// Using the data in the Temp Header and Lines Table mark those items as done in the actual orders table
    /// </summary>
    /// <param name="pCustomerID">the client the temporary items are mean for</param>
    /// <returns>Success</returns>
    public bool MarkTempOrdersItemsAsDone()
    {
      bool _Success = false;
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        OleDbCommand _cmd = new OleDbCommand(CONST_SQL_UPDATEORDERSASDONE, _conn);                    // run the qurey we have built
        _conn.Open();
        _Success = _cmd.ExecuteNonQuery() > 0;
      }
      return _Success;
    }
    /// <summary>
    /// DeleteAll the data ion both TempOrderTables
    /// </summary>
    /// <returns></returns>
    public bool KillTempOrdersData()
    {
      bool _Success = false;

      TempOrdersHeaderTbl _HeaderDAL = new TempOrdersHeaderTbl();
      TempOrdersLinesTbl _LinesDAL = new TempOrdersLinesTbl();

      _Success = (_HeaderDAL.DeleteAllRecords()) ;
      _Success = (_LinesDAL.DeleteAllRecords()) && _Success;   // prevent short circut bool eval

      return _Success;
    }

  }

}