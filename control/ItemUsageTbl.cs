/// --- auto generated class for table: ItemUsageTbl
using System;   // for DateTime variables
using System.Collections.Generic;      // for data stuff
using System.Data.OleDb;
using System.Configuration;
using QOnT.classes;

namespace QOnT.control
{
  public class ItemUsageTbl
  {
    // internal variable declarations
    private int _ClientUsageLineNo;
    private long _CustomerID;
    private DateTime _Date;
    private int _ItemProvided;
    private double _AmountProvided;
    private int _PrepTypeID;
    private int _PackagingID;
    private string _Notes;
    // class definition
    public ItemUsageTbl()
    {
      _ClientUsageLineNo = 0;
      _CustomerID = 0;
      _Date = System.DateTime.Now;
      _ItemProvided = 0;
      _AmountProvided = 0.0;
      _PrepTypeID = 0;
      _PackagingID = 0;
      _Notes = string.Empty;
    }
    // get and sets of public
    public int ClientUsageLineNo { get { return _ClientUsageLineNo;}  set { _ClientUsageLineNo = value;} }
    public long CustomerID { get { return _CustomerID; } set { _CustomerID = value; } }
    public DateTime Date { get { return _Date;}  set { _Date = value;} }
    public int ItemProvided { get { return _ItemProvided;}  set { _ItemProvided = value;} }
    public double AmountProvided { get { return _AmountProvided;}  set { _AmountProvided = value;} }
    public int PrepTypeID { get { return _PrepTypeID;}  set { _PrepTypeID = value;} }
    public int PackagingID { get { return _PackagingID;}  set { _PackagingID = value;} }
    public string Notes { get { return _Notes;}  set { _Notes = value;} }

  #region ConstantDeclarations
    const string CONST_SQL_SELECT = "SELECT TOP 10 ClientUsageLineNo, CustomerID, Date, ItemProvided, AmountProvided, PrepTypeID, PackagingID, Notes FROM ItemUsageTbl";
    const string CONST_SQL_UPDATE = "UPDATE ItemUsageTbl SET CustomerID = ?, Date = ?, ItemProvided = ?, AmountProvided = ? , PrepTypeID = ?, " +
                                    "PackagingID = ?, Notes = ? WHERE ClientUsageLineNo = ? ";
    const string CONST_SQL_INSERT = "INSERT INTO ItemUsageTbl (CustomerID, [Date], ItemProvided, AmountProvided, PrepTypeID, PackagingID, Notes)"  +
                                                     " VALUES (?, ?, ?, ?, ?, ?, ?)";
  #endregion

    public List<ItemUsageTbl> GetAllItemsUsed(long pCustomerID, string SortBy)
    {
      List<ItemUsageTbl> _DataItems = new List<ItemUsageTbl>();
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_SELECT + " WHERE CustomerID = " + pCustomerID.ToString();
        if (!String.IsNullOrEmpty(SortBy)) _sqlCmd += " ORDER BY " + SortBy;     // Add order by string
        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the query we have built
        _conn.Open();
        OleDbDataReader _DataReader = _cmd.ExecuteReader();
        while (_DataReader.Read())
        {
          ItemUsageTbl _DataItem = new ItemUsageTbl();

          _DataItem.ClientUsageLineNo = (_DataReader["ClientUsageLineNo"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ClientUsageLineNo"]);
          _DataItem.CustomerID = (_DataReader["CustomerID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["CustomerID"]);
          _DataItem.Date = (_DataReader["Date"] == DBNull.Value) ? System.DateTime.Now : Convert.ToDateTime(_DataReader["Date"]);
          _DataItem.ItemProvided = (_DataReader["ItemProvided"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["ItemProvided"]);
          _DataItem.AmountProvided = (_DataReader["AmountProvided"] == DBNull.Value) ? 0.0 : Convert.ToDouble(_DataReader["AmountProvided"]);
          _DataItem.PrepTypeID = (_DataReader["PrepTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["PrepTypeID"]);
          _DataItem.PackagingID = (_DataReader["PackagingID"] == DBNull.Value) ? 0 : Convert.ToInt32(_DataReader["PackagingID"]);
          _DataItem.Notes = (_DataReader["Notes"] == DBNull.Value) ? string.Empty : _DataReader["Notes"].ToString();
          _DataItems.Add(_DataItem);
        }
      }
      return _DataItems;
    }
    public string InsertItemsUsed(ItemUsageTbl ItemUsageLine)
    {
      string errString = "";
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_INSERT;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        #region Parameters
        // Add data sent
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.CustomerID });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.Date });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.ItemProvided });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.AmountProvided });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.PrepTypeID });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.PackagingID });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.Notes });
        /// -> auto generater _cmd.Parameters.Add(new OleDbParameter { Value = pClientUsageLineNo });
        #endregion
        try
        {
          _conn.Open();
          if (_cmd.ExecuteNonQuery() != 0)
            errString = "";
          else
            errString += " - error ";
        }
        catch (OleDbException oleErr)
        {
          // Handle exception.
          errString += " ERROR: " + oleErr.Message;
        }
        finally
        {
          _conn.Close();
        }
        return errString;
      }
    }
    public string UpdateItemsUsed(ItemUsageTbl ItemUsageLine, long OriginalClientUsageLineNo)
    {
      string errString = "";
      string _connectionStr = ConfigurationManager.ConnectionStrings[TrackerDb.CONST_CONSTRING].ConnectionString;

      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))
      {
        string _sqlCmd = CONST_SQL_UPDATE;

        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);
        #region Parameters
        // Add data sent
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.CustomerID });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.Date });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.ItemProvided });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.AmountProvided });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.PrepTypeID });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.PackagingID });
        _cmd.Parameters.Add(new OleDbParameter { Value = ItemUsageLine.Notes });
        //                                     " WHERE ClientUsageLineNo = ?)";
        _cmd.Parameters.Add(new OleDbParameter { Value = OriginalClientUsageLineNo });
        #endregion
        try
        {
          _conn.Open();
          if (_cmd.ExecuteNonQuery() != 0)
            errString = "";
          else
            errString += " - error ";
        }
        catch (OleDbException oleErr)
        {
          // Handle exception.
          errString += " ERROR: " + oleErr.Message;
        }
        finally
        {
          _conn.Close();
        }
        return errString;
      }
    }

  }
}
