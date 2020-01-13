using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Configuration;

namespace QOnT.classes
{
  public class TrackerDb
  {
    public const string CONST_CONSTRING = "Tracker08ConnectionString";
 
    private OleDbConnection _TrackerDbConn = null;

    public OleDbConnection TrackerDbConn
    {
      get { return this._TrackerDbConn; }
    }

    public TrackerDb()
    {
      Initialize();
    }

    private void Initialize()
    {
      if (_TrackerDbConn != null)
      {
        _TrackerDbConn.Dispose();
        _TrackerDbConn = null;
      }

      string _connectionString;

      if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
          ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
      {
        throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
                            "must exist in the <connectionStrings> configuration section for the application.");
      }
      _connectionString = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;
      this._TrackerDbConn = new OleDbConnection(_connectionString);
    }
    /// <summary>
    /// Execute the SQL statement does not return results, such as: delete, update, insert operation 
    /// </summary>
    /// <param name="strSQL">SQL String of a non Query Type</param>
    /// <returns>success or failure</returns>
    public bool ExecuteNonQuerySQL (string strSQL) 
    {
      bool _resultState = false;
        
      _TrackerDbConn.Open();
      OleDbTransaction _myTrans = _TrackerDbConn.BeginTransaction();
      OleDbCommand _command = new OleDbCommand(strSQL, _TrackerDbConn, _myTrans); 
      try 
      { 
        _command.ExecuteNonQuery (); 
        _myTrans.Commit (); 
        _resultState = true; 
      } 
      catch 
      { 
        _myTrans.Rollback (); 
        _resultState = false; 
      } 
      finally 
      { 
        _TrackerDbConn.Close (); 
      } 
      return _resultState; 
    }

    /// <summary>
    /// Execute the SQL statement returns the result to the DataSet
    /// </summary>
    /// <param Name="strSQL"> </ param> 
    /// <returns> DataSet </ returns> 

    public DataSet ReturnDataSet (string strSQL) 
    {
      DataSet _DataSet = null;
      try
      {
        _TrackerDbConn.Open(); 
        _DataSet = new DataSet (); 
        OleDbDataAdapter _OleDbDA = new OleDbDataAdapter (strSQL, _TrackerDbConn); 
        _OleDbDA.Fill (_DataSet, "objDataSet");
      }
      catch (OleDbException _ex)
      {
        // Handle exception.
        TrackerTools _Tools = new TrackerTools();
        _Tools.SetTrackerSessionErrorString(_ex.Message);
        _DataSet.Dispose();
        throw;
      }
      finally
      {
        _TrackerDbConn.Close();
      }

      return _DataSet; 
    }

    public OleDbDataReader ReturnDataReader(string strSQL)
    {
      OleDbDataReader _OleDataReader = null;
      try
      {
        _TrackerDbConn.Open();
        OleDbCommand _cmd = new OleDbCommand(strSQL, TrackerDbConn);
        _OleDataReader = _cmd.ExecuteReader();
      }
      catch (OleDbException _ex)
      {
        // Handle exception.
        TrackerTools _Tools = new TrackerTools();
        _Tools.SetTrackerSessionErrorString(_ex.Message);
        _OleDataReader.Dispose();
      }
      finally
      {
        ///
      }
      return _OleDataReader;
    }
    public void Close()
    {
      TrackerDbConn.Close();
    }

    

  }
}