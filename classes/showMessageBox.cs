using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace QOnT.classes
{
  public class showMessageBox
  {
    /// <summary>
    /// Uses the showAppMessage in Java script that is part of the Site.Master to display an alert"
    /// </summary>
    /// <param name="pPage">this.Page, a reference to the web page</param>
    /// <param name="pTitle">the title to use for the message</param>
    /// <param name="pMessage">The actual message string to display</param>
    public showMessageBox(Page pPage, string pTitle, string pMessage)
    {
      string _ScriptToRun = "showAppMessage('" +pMessage + "');";
      ScriptManager.RegisterStartupScript(pPage, pPage.GetType(), pTitle, _ScriptToRun, true);
    }

  }
}