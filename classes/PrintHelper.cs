using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

namespace QOnT.classes
{
  public class PrintHelper
  {
    public PrintHelper()
    {
    }

    public static void PrintWebControl(Control pCtrl)
    {
      PrintWebControl(pCtrl, string.Empty);
    }

    public static void PrintWebControl(Control pCtrl, string pScript)
    {
      StringWriter stringWrite = new StringWriter();
      System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
      if (pCtrl is WebControl)
      {
        Unit w = new Unit(100, UnitType.Percentage); ((WebControl)pCtrl).Width = w;
      } 
      Page pg = new Page();
      pg.EnableEventValidation = false;
      if (pScript != string.Empty)
      {
        pg.ClientScript.RegisterStartupScript(pg.GetType(), "PrintJavaScript", pScript);
      }
      HtmlForm frm = new HtmlForm();
      pg.Controls.Add(frm);
      frm.Attributes.Add("runat", "server");
      frm.Controls.Add(pCtrl);
      pg.DesignerInitialize();
      pg.RenderControl(htmlWrite);
      string strHTML = stringWrite.ToString();
      HttpContext.Current.Response.Clear();
      HttpContext.Current.Response.Write(strHTML);
      HttpContext.Current.Response.Write("<script>window.print();</script>");
      HttpContext.Current.Response.End();
    }
  }
}