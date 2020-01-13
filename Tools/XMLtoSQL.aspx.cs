using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Xml;
using TrackerDotNet.classes;
using System.Web.UI;
using System.IO;

namespace TrackerDotNet.test
{
  public partial class XMLtoSQL : System.Web.UI.Page
  {

    class SQLCommand
    {
      string _type;
      string _sql;
      string _errString;
      bool _result;

      public SQLCommand()
      {
        _type = "";
        _sql = "";
        _errString = "";
        _result = false;
      }
      public string type { get { return _type; } set { _type = value; } }
      public string sql { get { return _sql; } set { _sql = value; } }      
      public string errString  { get { return _errString ; } set { _errString  = value; } }
      public bool result { get { return _result; } set { _result = value; } }
    }

    const string CONST_DEFAULT_PREFIX = "SQLCommands";
    private void SetDefaultFileName()
    {
      string _Path = "~\\Tools";
      try
      {
        DirectoryInfo _Dir = new DirectoryInfo(Server.MapPath(_Path));
        FileInfo[] _FileList = _Dir.GetFiles(CONST_DEFAULT_PREFIX + "*.xml", SearchOption.TopDirectoryOnly);
        List<int> _FileNumbers = new List<int>();
        if (_FileList.Length > 0)
        {
          string _FilePath = _FileList[0].FullName.Substring(0, _FileList[0].FullName.IndexOf(CONST_DEFAULT_PREFIX) + CONST_DEFAULT_PREFIX.Length);

          foreach (FileInfo _FI in _FileList)
          {
            string _Number = _FI.FullName.Substring(_FilePath.Length, _FI.FullName.IndexOf(".") - _FilePath.Length);

            int i = 0;
            if (int.TryParse(_Number, out i))
              _FileNumbers.Add(i);
          }

          _FileNumbers.Sort();

          FileNameTextBox.Text = _FilePath + _FileNumbers[_FileNumbers.Count - 1].ToString() + ".xml";

        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        SetDefaultFileName();
      }
    }

    private void showMsgBox(string pTitle, string pMessage)
    {
      string _ScriptToRun = "showAppMessage('" + pMessage + "');";
      System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), pTitle, _ScriptToRun, true);
    }
    protected void GoButton_Click(object sender, EventArgs e)
    {
      List<SQLCommand> _SQLCommands = new List<SQLCommand>();
      string _FileName = FileNameTextBox.Text;
      _FileName = _FileName.Replace(@"\",@"\\");

      XmlReader _XmlReader = XmlReader.Create(_FileName);
      try
      {

        while (_XmlReader.Read())
        {
          if ((_XmlReader.NodeType == XmlNodeType.Element) && (_XmlReader.Name == "command"))
          {
            SQLCommand _SQLCommand = new SQLCommand();

            _SQLCommand.type = _XmlReader.GetAttribute("type");
            _XmlReader.Read();      // next should be value
            _SQLCommand.sql = _XmlReader.Value.Replace("\n", ""); // remove new line characters

            _SQLCommands.Add(_SQLCommand);
          }
        }
        _XmlReader.Close();

        for (int i = 0; i < _SQLCommands.Count; i++)
        {
          if (_SQLCommands[i].type == "select")
          {
            GridView _gvSelectResult = new GridView();
            System.Data.DataSet _ds = RunSelect(_SQLCommands[i].sql);

            _SQLCommands[i].result = (_ds != null);

            _gvSelectResult.DataSource = _ds;

            _gvSelectResult.DataBind();
            pnlSQLResults.Controls.Add(_gvSelectResult);
          }
          else if (_SQLCommands[i].type != "disabled")
          {
            _SQLCommands[i].errString = RunCommand(_SQLCommands[i].sql);
            _SQLCommands[i].result = String.IsNullOrWhiteSpace(_SQLCommands[i].errString);
          }

          TrackerTools _TT = new TrackerTools();
          string _err = _TT.GetTrackerSessionErrorString();
          if (!string.IsNullOrEmpty(_err))
          {
            showMessageBox _msg = new showMessageBox(this.Page, "err", _err);
            _TT.SetTrackerSessionErrorString("");
          }
          
        }
        // assign resutls to result panel
        gvSQLResults.DataSource = _SQLCommands;
        gvSQLResults.DataBind();
      }
      catch (Exception _ex)
      {
        showMessageBox _showMsg = new showMessageBox(this.Page, "Error", "File access error: \n" + _ex.Message);
      }
      finally
      {
        _XmlReader.Close();
      }
    }

    private System.Data.DataSet RunSelect(string pSQL)
    {
      TrackerDb _TrackerDb = new TrackerDb();

      System.Data.DataSet _DataSet = _TrackerDb.ReturnDataSet(pSQL);

      return _DataSet;
    }
    private string RunCommand(string pSQL)
    {
      TrackerDb _TrackerDb = new TrackerDb();

      return _TrackerDb.ExecuteNonQuerySQL(pSQL);
    }
  }
}