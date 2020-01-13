// #define IsLocal   // Remove when not testing
using System;
using System.Collections.Generic;
using System.Web;
using System.Net;   // for email stuff
using System.Net.Mail;  // for email stuff
using System.Web.UI.WebControls;    // for url stuff
using System.Text;
using System.Configuration;

/// <summary>
/// A singular class to handle the sending of emails from all the forms, will store info to database
/// </summary>
public class EmailCls
{

  public const string CONST_APPSETTING_FROMEMAILKEY = "SysEmailFrom";

  ///--------------------- Public Structures and Variables ---------------------
  /// <summary>
  /// structure of results for this object, accessed via myResults
  /// </summary>
  public struct SendMailResults
  {
    public string sResult;
    public string sID;
  }
  /// <summary>
  /// Want to know the error result, or the Id stored in the database, this is where you look
  /// </summary>
  public SendMailResults myResults;

  ///--------------------- Private Structures and Variables ---------------------
  /// <summary>
  /// ternal mail message variables
  /// </summary>
  MailMessage myMsg = null;
  private System.Text.StringBuilder sbMsgBody = new StringBuilder();

  ///--------------------- Code ---------------------
  /// <summary>
  /// Initialize Email class
  /// </summary>
  public EmailCls()
  {
    //
    // Initialize internal and public Variables
    //
    if (myMsg == null)
    {
      myMsg = new MailMessage();
      myMsg.IsBodyHtml = true;
      myMsg.Body = "";

      myResults.sResult = "";
      myResults.sID = "";

      myMsg.From = new MailAddress(ConfigurationManager.AppSettings[CONST_APPSETTING_FROMEMAILKEY]);  // set the defualt from
    }
  }
  /// <summary>
  /// Set the from email address and the to email address, returns true if addess is fine
  /// </summary>
  /// <param name="sFrom">from email address</param>
  /// <param name="sTo">To address</param>
  public virtual bool SetEmailFromTo(string sFrom, string sTo)
  {
    try
    {
      // first trim the field
      sFrom = sFrom.Trim();
      sTo = sTo.Trim();
      if ((sFrom.Contains("@")) && (sTo.Contains("@")))
      {
        myMsg.From = new MailAddress(sFrom);
        myMsg.To.Add(new MailAddress(sTo));
        return true;
      }
      else
        return false;
    }
    catch
    {
      return false;
    }
  }
  /// <summary>
  /// Set the from email address and the to email address, returns true if addess is fine
  /// </summary>
  /// <param name="sFrom">from email address</param>
  /// <param name="sTo">To address</param>
  public virtual bool SetEmailFrom(string sFrom)
  {
    try
    {
      // first trim the field
      sFrom = sFrom.Trim();
      if (sFrom.Contains("@"))
      {
        myMsg.From = new MailAddress(sFrom);
        return true;
      }
      else
        return false;
    }
    catch
    {
      return false;
    }
  }
  /// <summary>
  /// Set the from email address and the to email address, returns true if addess is fine
  /// </summary>
  /// <param name="sTo">To address</param>
  /// <param name="overwrite">overwrite to email address</param>
  public virtual bool SetEmailTo(string sTo) { return SetEmailTo(sTo, false); }
  public virtual bool SetEmailTo(string sTo, bool pOverWrite)
  {
    try
    {
      // first trim the field
      sTo = sTo.Trim();
      if (sTo.Contains("@"))
      {
        // clear the too message if required.
        if (pOverWrite)
          myMsg.To.Clear();
        myMsg.To.Add(new MailAddress(sTo));
        return true;
      }
      else
        return false;
    }
    catch
    {
      return false;
    }
  }
  /// <summary>
  /// Set the CC for the email
  /// </summary>
  /// <param name="sCC">string of the email address to CC</param>
  public virtual void SetEmailCC(string sCC)
  {
    myMsg.CC.Add(new MailAddress(sCC));
  }
  /// <summary>
  /// Set the BCC for the email
  /// </summary>
  /// <param name="sBCC">string of the email address to BCC</param>
  public virtual void SetEmailBCC(string sBCC)
  {
    myMsg.Bcc.Add(new MailAddress(sBCC));
  }

  /// <summary>
  /// Set the Subject of the Email
  /// </summary>
  /// <param name="sSubject">The Subject String</param>
  public virtual void SetEmailSubject(string sSubject)
  {
    myMsg.Subject = sSubject;
  }
  /// <summary>
  /// Add a string to the body, not that the message support html
  /// </summary>
  /// <param name="sBody">String with embedded html to be added</param>
  public virtual void AddToBody(string sBody)
  {
    sbMsgBody.Append(sBody);
  }
  /// <summary>
  /// Add a string to the body and add a break line to the end
  /// </summary>
  /// <param name="sBody">String with embedded html</param>
  public virtual void AddStrAndNewLineToBody(string sBody)
  {
    sbMsgBody.Append(sBody);
    sbMsgBody.Append("<br />");
  }
  /// <summary>
  /// Add a C# Format string to the message body
  /// </summary>
  /// <param name="pFormat">format string</param>
  /// <param name="pObj1">Objects (up to 3) to add to the format </param>
  public virtual void AddFormatToBody(string pFormat, object pObj1)
  { sbMsgBody.AppendFormat(pFormat, pObj1); }
  public virtual void AddFormatToBody(string pFormat, object pObj1, object pObj2)
  { sbMsgBody.AppendFormat(pFormat, pObj1, pObj2); }
  public virtual void AddFormatToBody(string pFormat, object pObj1, object pObj2, object Obj3)
  { sbMsgBody.AppendFormat(pFormat, pObj1, pObj2, Obj3); }
  /// <summary>
  /// Add An Attachment to the email
  /// </summary>
  /// <param name="pRelativePath">the relative path of the attachment</param>
  public virtual void AddPDFAttachment(string pRelativePath)
  {
    //        string strFullPath = System.Web.VirtualPathUtility.ToAbsolute(pItemPath);
    string strFullPath = System.Web.HttpContext.Current.Server.MapPath(pRelativePath);
    Attachment myAttachment = new Attachment(strFullPath, System.Net.Mime.MediaTypeNames.Application.Pdf);
    myMsg.Attachments.Add(myAttachment);
  }
  /// <summary>
  /// Send the email using the credentials setup in SetFromTo, SetEmailSubject and AddToBodyX
  /// </summary>
  /// <returns>true if emnpty string if no errors when sending, otherwise the error </returns>
  public virtual bool SendEmail() { return SendEmail(false); }
  public virtual bool SendEmail(bool bUseGoogle)
  {
    try
    {
      // define variables here since C# does not picup definition in if ... else
      SmtpClient client;
      System.Net.NetworkCredential NTLMAuthentication;

      // Send the email using the credentials, using google or quaffee
      if (bUseGoogle)
      {
        string _strGMailLogIn = ConfigurationManager.AppSettings["GMailLogIn"];
        string _strGMailPassword = ConfigurationManager.AppSettings["GMailPassword"];
        string _strGMailSMTP = ConfigurationManager.AppSettings["GMailSMTP"];

        myMsg.CC.Add(new MailAddress(myMsg.From.Address, "CC to " + myMsg.From.Address));  // cc to their address, since Gmail nneds to be sent from gmail
        myMsg.Subject = "FROM: " + myMsg.From.Address + " sent: "
            + DateTime.Now.Date.ToShortDateString() + " re:" + myMsg.Subject;
        myMsg.From = new MailAddress(_strGMailLogIn, "Reply to " + myMsg.From.Address);
        NTLMAuthentication = new System.Net.NetworkCredential(_strGMailLogIn, _strGMailPassword);
        client = new SmtpClient(_strGMailSMTP, 587);   //// ("196.46.70.2");  // ("smtp.iburst.co.za");
        client.EnableSsl = true;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
      }
      else
      {
        string _strEMailLogIn = ConfigurationManager.AppSettings["EMailLogIn"];
        string _strEMailPassword = ConfigurationManager.AppSettings["EmailPassword"];
        string _strEMailSMTP = ConfigurationManager.AppSettings["EMailSMTP"];
        int _EMailPort = (ConfigurationManager.AppSettings["EmailPort"] != null) ? Convert.ToInt32(ConfigurationManager.AppSettings["EmailPort"]) : 0;
        bool _EMailSSLEnabled = (ConfigurationManager.AppSettings["EMailSSLEnabled"] != null) ? String.Equals(ConfigurationManager.AppSettings["EMailSSLEnabled"], "true", StringComparison.OrdinalIgnoreCase) : false;
        // set the SMTP client to point the SMTP address they passed
        myMsg.Sender = new MailAddress(_strEMailLogIn);
        if (_EMailPort == 0)
          client = new SmtpClient(_strEMailSMTP);
        else
        {
          client = new SmtpClient(_strEMailSMTP, _EMailPort);   //// ("196.46.70.2");  // ("smtp.iburst.co.za");
          client.EnableSsl = _EMailSSLEnabled;
        }
        client.UseDefaultCredentials = (_strEMailPassword != "");
        client.DeliveryMethod = SmtpDeliveryMethod.Network;

        if (client.UseDefaultCredentials)
          NTLMAuthentication = new System.Net.NetworkCredential(_strEMailLogIn, _strEMailPassword);
        else
          NTLMAuthentication = new System.Net.NetworkCredential();
      }
      client.Credentials = NTLMAuthentication;
      // copy the string build version of the body across to Body
      myMsg.Body = sbMsgBody.ToString();
      client.Send(myMsg);

      // if you get here then it has been a success
      myResults.sID = "xxx";   // this should be the ID of the item in the database
      myResults.sResult = "";

      return true; // no error so return blank string
    }
    catch (Exception ex)
    {
      myResults.sResult = "ERROR: " + ex.Message;
      if (!bUseGoogle)
        return SendEmail(true);
      else
      {
        return false;
      }
    }

  }
  /// <summary>
  /// Gets or sets the Message internal to the class
  /// </summary>
  public string MsgBody
  {
    get
    {
      if (myMsg != null)
        return sbMsgBody.ToString();
      else
        return "";
    }
    set
    {
      if (myMsg != null)
      {
        if (sbMsgBody.Length >= 1)
          sbMsgBody.Remove(0, sbMsgBody.Length - 1);
        sbMsgBody.Append(value);
      }
    }
  }
  /// <summary>
  /// Kill the memory used by message if created
  /// </summary>
  public virtual void Kill()
  {
    if (myMsg != null)
    {
      myMsg.Dispose();
      myMsg = null;
    }
  }

}
