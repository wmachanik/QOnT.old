using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Drawing;
using System.Text;
using System.Drawing.Printing;

namespace QOnT.classes
{

  public class WebPagePrint
  {
    public WebPagePrint()
    {
    }

    protected StringReader stringToPrint;
    protected Font printFont;


    public void PageCreate(string printerName, string pageTitle)
    {
      StringBuilder sb = new StringBuilder();
      string qs = "";
      try
      {
        // start creating page with title and date/time
        sb.Append(pageTitle + "\n\n");
        sb.Append("DateTime: " + DateTime.Now.ToString() + "\n\n");
        string fieldValue;
        // iterate submitted form fields, also gets field name
        foreach (string fieldName in HttpContext.Current.Request.Form)
        {
          // exclude viewstate and submit button
          if (fieldName == "__VIEWSTATE" || fieldName == "Submit") { }
          else
          {
            // get the field value
            fieldValue = HttpContext.Current.Request.Form[fieldName];
            // builds the querystring for results.aspx
            qs = qs + "&" + fieldName + "=" + fieldValue;
            // adds the field name and value to the page
            // breaks the field value into 50 character segments so it will fit on the paper
            // this example only accounts for fields of l50 characters or less 
            // issue: breaks in the middle of words instead of at spaces
            if (fieldValue.Length > 100)
            {
              sb.Append(fieldName + ": " + fieldValue.Substring(0, 50) + "\n");
              sb.Append("            " + fieldValue.Substring(50, 50) + "\n");
              sb.Append("            " + fieldValue.Substring(100, fieldValue.Length - 100) + "\n");
            }
            else if (fieldValue.Length > 50)
            {
              sb.Append(fieldName + ": " + fieldValue.Substring(0, 50) + "\n");
              sb.Append("            " + fieldValue.Substring(50, fieldValue.Length - 50) + "\n");
            }
            else
            {
              sb.Append(fieldName + ": " + fieldValue + "\n");
            }

          }
        }
        // place stringbuilder in string reader
        stringToPrint = new StringReader(sb.ToString());
        // set font and size here
        printFont = new Font("Arial", 12);
        PrintDocument doc = new PrintDocument();
        // set the printer name
        doc.PrinterSettings.PrinterName = printerName;
        // add print page event handler
        doc.PrintPage += new PrintPageEventHandler(this.PagePrint);
        // print the page
        doc.Print();
        // adds status to querystring
        qs = "Results.aspx?" + qs.Substring(1, qs.Length - 1) + "&Status=Success";
      }
      catch
      {
        qs = "Results.aspx?Status=Failed";
      }
      finally
      {
        stringToPrint.Close();
      }
      // redirects to result.aspx
      HttpContext.Current.Response.Redirect(qs);
    }

    private void PagePrint(object sender, PrintPageEventArgs e)
    {
      float linesPerPage = 0;
      float linePosition = 0;
      int lineCount = 0;
      float leftMargin = e.MarginBounds.Left;
      float topMargin = e.MarginBounds.Top;
      String line = null;
      // gets the number of lines per page
      linesPerPage = e.MarginBounds.Height / printFont.GetHeight(e.Graphics);
      // iterate lines in string
      while (lineCount < linesPerPage && ((line = stringToPrint.ReadLine()) != null))
      {
        // set line postion from top margin
        linePosition = topMargin + (lineCount * printFont.GetHeight(e.Graphics));
        // print line
        e.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, linePosition,
            new StringFormat());
        lineCount++;
      }
      // are there more lines?
      if (line != null)
        e.HasMorePages = true;
      else
        e.HasMorePages = false;
    }
  }
}