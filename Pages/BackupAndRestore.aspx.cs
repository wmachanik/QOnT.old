using System;
using System.Data.SqlClient;
using System.Data;

namespace QOnT.Pages
{
  public partial class BackupAndRestore : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSecurty2Local_Click(object sender, EventArgs e)
    {
      try
      {
        //string destDir = "c:\\backups";  //  tbxFileName.Text;
        //string StrConString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();

        //SqlConnection sqlcon = new SqlConnection(StrConString);
        //SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter da = new SqlDataAdapter();
        //DataTable dt = new DataTable();

        ////Enter destination directory where backup file stored

        ////Check that directory already there otherwise create 
        //if (!System.IO.Directory.Exists(destDir))
        //{
        //  System.IO.Directory.CreateDirectory(destDir);
        //}
        ////Open connection
        //sqlcon.Open();
        ////query to take backup database
        //string fName = "DBBackup" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak";
        //sqlcmd = new SqlCommand("backup database QOnTSecurity to disk='" + destDir + "\\" + fName + "'", sqlcon);
        //sqlcmd.ExecuteNonQuery();
        ////Close connection
        //sqlcon.Close();
        //ltrlMsg.Text = "Backup database successfully";

        // need to backup on server then copy the file
        //  Now do restore

        string FName = "C:\\backups\\QOnTSecurityBackup.bak";

        string StrConString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();

        SqlConnection sqlcon = new SqlConnection(StrConString);
        SqlCommand sqlcmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        sqlcon.Open();
        sqlcmd = new SqlCommand("restore database TrackerDoNetSecurity FROM DISK = '" + FName + "'",sqlcon);
        sqlcmd.ExecuteNonQuery();
        sqlcon.Close();
        ltrlMsg.Text = "Restore complete";
      }
      catch (Exception exp)
      {
        ltrlMsg.Text = "<b>ERROR:</b> " + exp.Message;
      }

    }
  }
}