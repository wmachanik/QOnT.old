using System;
using System.Collections.Generic;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using System.Data;

namespace TrackerDotNet.test
{
  public partial class AutoClassMaker : System.Web.UI.Page
  {
    const string CONST_CONSTRING = "Tracker08ConnectionString";

    private OleDbConnection OpenTrackerOleDBConnection()
    {
      OleDbConnection pConn = null;
      string _connectionString;

      if (ConfigurationManager.ConnectionStrings[CONST_CONSTRING] == null ||
          ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString.Trim() == "")
      {
        throw new Exception("A connection string named " + CONST_CONSTRING + " with a valid connection string " +
                            "must exist in the <connectionStrings> configuration section for the application.");
      }
      _connectionString = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;
      pConn = new OleDbConnection(_connectionString);

      return pConn;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        // load the drop down list with table names
        OleDbConnection _TableNamesConn = OpenTrackerOleDBConnection();
        if (_TableNamesConn != null)
        {
          try
          {
            _TableNamesConn.Open();

            DataTable _TableNamesSchema = _TableNamesConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            ddlTables.DataSource = _TableNamesSchema;
            ddlTables.DataTextField = "TABLE_NAME";
            ddlTables.DataBind();
          }
          catch (Exception ex)
          {
            throw new Exception("Error: " + ex.Message);
          }
          finally
          {
            _TableNamesConn.Close();
          }
        }

      }

    }

    protected void ddlTables_SelectedIndexChanged(object sender, EventArgs e)
    {
      // set the name of the class file
      tbxORMClassFileName.Text = ddlTables.SelectedValue + "Data.cs";
    }
      
    public struct dbTypesDef
    {
      public string typeName;  
      public string typeNil;
      public string typeConvert;
    }

    protected OleDbType GetOleDBType(string pRowDBType)
    {
      return (OleDbType)Int32.Parse(pRowDBType);
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
      
      // create a Dictionary of types using the common DBType, add more if required
      Dictionary<OleDbType, dbTypesDef> _ColDBTypes = new Dictionary<OleDbType, dbTypesDef>();
      _ColDBTypes.Add(OleDbType.Binary, new dbTypesDef { typeName = "bool", typeNil = "false" , typeConvert = "Convert.ToBoolean" });
      _ColDBTypes.Add(OleDbType.Boolean, new dbTypesDef { typeName = "bool", typeNil = "false" , typeConvert = "Convert.ToBoolean" });
      _ColDBTypes.Add(OleDbType.BigInt, new dbTypesDef { typeName = "long", typeNil = "0", typeConvert = "Convert.ToInt64" });
      _ColDBTypes.Add(OleDbType.UnsignedBigInt, new dbTypesDef { typeName = "long", typeNil = "0", typeConvert = "Convert.ToInt64" });
      _ColDBTypes.Add(OleDbType.UnsignedTinyInt, new dbTypesDef { typeName = "byte", typeNil = "0", typeConvert = "Convert.ToInt64" });
      _ColDBTypes.Add(OleDbType.TinyInt, new dbTypesDef { typeName = "int16", typeNil = "0", typeConvert = "Convert.ToInt16" });
      _ColDBTypes.Add(OleDbType.Integer, new dbTypesDef { typeName = "int", typeNil = "0", typeConvert = "Convert.ToInt32" });
      _ColDBTypes.Add(OleDbType.Currency, new dbTypesDef { typeName = "double", typeNil = "0.0", typeConvert = "Convert.ToDouble" });
      _ColDBTypes.Add(OleDbType.Date, new dbTypesDef { typeName = "DateTime", typeNil = "System.DateTime.Now", typeConvert = "Convert.ToDateTime" });
      _ColDBTypes.Add(OleDbType.DBDate, new dbTypesDef { typeName = "DateTime", typeNil = "System.DateTime.Now", typeConvert = "Convert.ToDateTime" });
      _ColDBTypes.Add(OleDbType.DBTime, new dbTypesDef { typeName = "DateTime", typeNil = "System.DateTime.Now", typeConvert = "Convert.ToDateTime" });
      _ColDBTypes.Add(OleDbType.Double, new dbTypesDef { typeName = "double", typeNil = "0.0", typeConvert = "Convert.ToDouble" });
      _ColDBTypes.Add(OleDbType.Guid, new dbTypesDef { typeName = "long", typeNil = "0", typeConvert = "Convert.ToInt64" });
      _ColDBTypes.Add(OleDbType.Char, new dbTypesDef { typeName = "string", typeNil = "string.Empty" , typeConvert = "" });
      _ColDBTypes.Add(OleDbType.WChar, new dbTypesDef { typeName = "string", typeNil = "string.Empty", typeConvert = "" });
      _ColDBTypes.Add(OleDbType.VarChar, new dbTypesDef { typeName = "string", typeNil = "string.Empty", typeConvert = "" });
      _ColDBTypes.Add(OleDbType.LongVarChar, new dbTypesDef { typeName = "string", typeNil = "string.Empty", typeConvert = "" });
      _ColDBTypes.Add(OleDbType.LongVarWChar, new dbTypesDef { typeName = "string", typeNil = "string.Empty", typeConvert = "" });
      _ColDBTypes.Add(OleDbType.Single, new dbTypesDef { typeName = "double", typeNil = "0.0", typeConvert = "Convert.ToDouble" });
      _ColDBTypes.Add(OleDbType.SmallInt, new dbTypesDef { typeName = "int16", typeNil = "0", typeConvert = "Convert.ToInt16" });
      _ColDBTypes.Add(OleDbType.Numeric, new dbTypesDef { typeName = "double", typeNil = "0.0", typeConvert = "Convert.ToDouble" });
      _ColDBTypes.Add(OleDbType.Decimal, new dbTypesDef { typeName = "double", typeNil = "0.0", typeConvert = "Convert.ToDouble" });
      _ColDBTypes.Add(OleDbType.IUnknown, new dbTypesDef { typeName = "var", typeNil = "null", typeConvert = "" });
      _ColDBTypes.Add(OleDbType.Empty, new dbTypesDef { typeName = "var", typeNil = "null", typeConvert = "" });
      
      // open a file
      string _fName = "c:\\temp\\" + tbxORMClassFileName.Text  ;
      StreamWriter _ColsStream = new StreamWriter(_fName, false);  // create new file
      // first Write Header information
      _ColsStream.WriteLine("/// --- auto generated class for table: " + ddlTables.SelectedValue);
      _ColsStream.WriteLine("using System;   // for DateTime variables");
      _ColsStream.WriteLine("using System.Collections.Generic;      // for data stuff");
      _ColsStream.WriteLine("using System.Data.OleDb;");
      _ColsStream.WriteLine("using System.Configuration;");

      _ColsStream.WriteLine();
      _ColsStream.WriteLine("namespace TrackerDotNet.classes");     // modify this if you gonna store in different location
      _ColsStream.WriteLine("{");

      _ColsStream.WriteLine("  public class " + ddlTables.SelectedValue + "Data");
      _ColsStream.WriteLine("  {");
      _ColsStream.WriteLine("    // internal variable declarations");

      // ExportTableColumns to file
      OleDbConnection _ColsConn = OpenTrackerOleDBConnection();
      try
      {
        _ColsConn.Open();
        DataTable _ColsSchema = _ColsConn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null , ddlTables.SelectedValue, null });
        // sort the schema ordinally so that the column names are in the same order as they appear in the database
        DataRow[] _rows = _ColsSchema.Select(null, "ORDINAL_POSITION", DataViewRowState.CurrentRows);
        foreach (DataRow _row in _rows)
        {
          // for do the private definitions
          OleDbType _thisType = GetOleDBType(_row["DATA_TYPE"].ToString());
          _ColsStream.Write("    private ");
          if (_ColDBTypes.ContainsKey(_thisType))
          {
            _ColsStream.Write(_ColDBTypes[_thisType].typeName);
          }
          else
            _ColsStream.Write(_thisType.ToString());

          _ColsStream.WriteLine(" _" + _row["COLUMN_NAME"].ToString() + ";");
        }
        // now define the class initializer
        _ColsStream.WriteLine("    // class definition");
        _ColsStream.WriteLine("    public " + ddlTables.SelectedValue + "Data()");
        _ColsStream.WriteLine("    {");
        // now do the intialization class;
        foreach (DataRow _row in _rows)
        {
          // for do the private definitions
          OleDbType _thisType = GetOleDBType(_row["DATA_TYPE"].ToString());

          if (_ColDBTypes.ContainsKey(_thisType))
            _ColsStream.WriteLine(String.Format("      _{0} = {1};", _row["COLUMN_NAME"].ToString(), _ColDBTypes[_thisType].typeNil));
          else
            _ColsStream.WriteLine(String.Format("      _{0} = {1};", _row["COLUMN_NAME"].ToString(), "1"));
        }
        _ColsStream.WriteLine("    }");
        // now each get and set
        _ColsStream.WriteLine("    // get and sets of public");
        foreach (DataRow _row in _rows)
        {
          // for do the private definitions
          OleDbType _thisType = GetOleDBType(_row["DATA_TYPE"].ToString());
          _ColsStream.Write("    public ");
          if (_ColDBTypes.ContainsKey(_thisType))
          {
            _ColsStream.Write(_ColDBTypes[_thisType].typeName);
          }
          else
            _ColsStream.Write(_thisType.ToString());

          _ColsStream.Write(" "+_row["COLUMN_NAME"].ToString());
          _ColsStream.Write(" { get { return _");
          _ColsStream.Write(_row["COLUMN_NAME"].ToString());
          _ColsStream.Write(";}  set { _");
          _ColsStream.Write(_row["COLUMN_NAME"].ToString());
          _ColsStream.WriteLine(" = value;} }");
        }
        // close class
        _ColsStream.WriteLine("  }");
        // now a list all class with constants defining the SQL
        _ColsStream.WriteLine("  public class " + ddlTables.SelectedValue + "DAL");
        _ColsStream.WriteLine("  {");
        _ColsStream.WriteLine("  #region ConstantDeclarations");
        _ColsStream.WriteLine("    const string CONST_CONSTRING = \"" + CONST_CONSTRING + "\";");
        _ColsStream.Write    ("    const string CONST_SQL_SELECT = \"SELECT ");
        // add each line
        string _selectRows = "";
        foreach (DataRow _row in _rows)
          _selectRows +=  _row["COLUMN_NAME"].ToString() + ", ";

        _selectRows = _selectRows.Remove(_selectRows.Length -2,2);
        _selectRows += " FROM " + ddlTables.SelectedValue + "\";";
        _ColsStream.WriteLine(_selectRows);
        _ColsStream.WriteLine("  #endregion");
        _ColsStream.WriteLine();
        _ColsStream.WriteLine("    public List<" + ddlTables.SelectedValue + "Data> GetAll(string SortBy)");
        _ColsStream.WriteLine("    {");
        _ColsStream.WriteLine("      List<" + ddlTables.SelectedValue + "Data> _DataItems = new List<" + ddlTables.SelectedValue +"Data>();");
        _ColsStream.WriteLine("      string _connectionStr = ConfigurationManager.ConnectionStrings[CONST_CONSTRING].ConnectionString;");
        _ColsStream.WriteLine();
        _ColsStream.WriteLine("      using (OleDbConnection _conn = new OleDbConnection(_connectionStr))");
        _ColsStream.WriteLine("      {");
        _ColsStream.WriteLine("        string _sqlCmd = CONST_SQL_SELECT;");
        _ColsStream.WriteLine("        if (!String.IsNullOrEmpty(SortBy)) _sqlCmd += \" ORDER BY \" + SortBy;     // Add order by string");
        _ColsStream.WriteLine("        OleDbCommand _cmd = new OleDbCommand(_sqlCmd, _conn);                    // run the qurey we have built");
        _ColsStream.WriteLine("        _conn.Open();");
        _ColsStream.WriteLine("        OleDbDataReader _DataReader = _cmd.ExecuteReader();");
        _ColsStream.WriteLine("        while (_DataReader.Read())");
        _ColsStream.WriteLine("        {");
        _ColsStream.WriteLine("          " + ddlTables.SelectedValue + "Data _DataItem = new " + ddlTables.SelectedValue + "Data();");
        _ColsStream.WriteLine();
        // for each item assign the value
        foreach (DataRow _row in _rows)
        {
          OleDbType _thisType = GetOleDBType(_row["DATA_TYPE"].ToString());
          _ColsStream.Write("          _DataItem." + _row["COLUMN_NAME"].ToString() + " = (_DataReader[\"" + _row["COLUMN_NAME"].ToString() + "\"");
          _ColsStream.Write("] == DBNull.Value) ? " + _ColDBTypes[_thisType].typeNil + " : ");
          if (String.IsNullOrEmpty(_ColDBTypes[_thisType].typeConvert))
            _ColsStream.WriteLine("_DataReader[\"" + _row["COLUMN_NAME"].ToString() + "\"].ToString();");
          else
            _ColsStream.WriteLine(_ColDBTypes[_thisType].typeConvert  + "(_DataReader[\"" + _row["COLUMN_NAME"].ToString() + "\"]);");
        }
        _ColsStream.WriteLine("          _DataItems.Add(_DataItem);");
          //// ---- change Items _DataItems
        _ColsStream.WriteLine("        }");
        _ColsStream.WriteLine("      }");
        _ColsStream.WriteLine("      return _DataItems;");
        _ColsStream.WriteLine("    }");
        _ColsStream.WriteLine("  }");
        _ColsStream.WriteLine("}");
        _ColsStream.Close();
      }
      catch (Exception ex)
      {
        
        throw new Exception("Error: " + ex.Message);
      }
      finally
      {
        _ColsConn.Close();
      }

    }
  }
}