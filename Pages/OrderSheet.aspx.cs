using System;
using System.Data;
using System.Web.UI.WebControls;
using QOnT.classes;
//using QOnT. Helpers.GridViewHelper;

namespace QOnT.Pages
{
  public partial class OrderSheet : System.Web.UI.Page
  {
    // DropDownValues
    const int DDL_VALUE_COMPANY_NAME = 1;
    const int DDL_VALUE_DELIVERY_BY = 2;
    const int DDL_VALUE_DELIVERY_DATE = 3;
    const int DDL_VALUE_ROAST_DATE = 4;
    const int DDL_VALUE_DELIVERYBY_AND_DELIVERY_DATE = 5;
    const int DDL_VALUE_DELIVERYBY_AND_ROAST_DATE = 6;

    // column numvers
    const int COL_NUMCOMMANDCOLS = 1;
    const int COL_NAME_MAX = 20;
    const int COL_ROASTDATE = 0 + COL_NUMCOMMANDCOLS;
    const int COL_BYDATE = 1 + COL_NUMCOMMANDCOLS;
    const int COL_DELVERYBY = 2 + COL_NUMCOMMANDCOLS;
    const int COL_DELVERYTO = 3 + COL_NUMCOMMANDCOLS;
    const int COL_ITEMSSTART = 4 + COL_NUMCOMMANDCOLS;

    // header maniulation stuff
    const int COL_ITEM_MAXLEN = 6 + COL_NUMCOMMANDCOLS;
    const int COL_START_SPECIAL_ITEMS = 5;   // the special items sort number of the last none generic item
    const string COL_ITEM_DELIMITER = "_";   // to handle the sort order we have added the sort order before the item text delimited by ?

    // the raost date calculation stuff
    DateTime dtThisRoastWeekStart;
    DateTime dtThisRoastWeekEnd;

    // stuff to maniuplate the item colums 
    int SpecialItemsColStart;
    double[] ColTotals;   // for the item totals
    Int32 CountOrders;

    //Table
    Table _TotalsTable = null;

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        QOnT.classes.TrackerTools tt = new TrackerTools();
        // now.AddDays(-7)
        dtThisRoastWeekStart = tt.GetClosestNextRoastDate(DateTime.Now.AddDays(-7), DayOfWeek.Monday);
        dtThisRoastWeekEnd = dtThisRoastWeekStart.AddDays(7);  // 5 days in a working week;
        tbxFilter.Text = dtThisRoastWeekStart.ToShortDateString();
        ddlFilterBy.Focus();
        this.Form.DefaultButton = btnGo.UniqueID;
      }
    }
    // grid stuff
    protected void gvOrderSheet_OnRowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.Header)
      {
        // Create a new summary table if this is the first time, otherwise start a new one
        if (_TotalsTable == null)
          _TotalsTable = new Table();
        else
          _TotalsTable.Rows.Clear();

        string _HeaderStr = "";
        int _DelimiterPos = 0;

        for (int i = COL_ITEMSSTART; i < e.Row.Cells.Count; i++)
        {
          TableCell tc = (TableCell)e.Row.Cells[i];
          LinkButton lnkBtn = (LinkButton) tc.Controls[0];
          _HeaderStr = lnkBtn.Text;

          // delete the data upto the delimiter
          _DelimiterPos = _HeaderStr.LastIndexOf(COL_ITEM_DELIMITER);
          if (_DelimiterPos > 0)
          {
            // change the format of the header colums for none special items
            if (Convert.ToInt16(_HeaderStr.Substring(0, _DelimiterPos)) < COL_START_SPECIAL_ITEMS)
            {
              e.Row.Cells[i].ForeColor = System.Drawing.Color.OliveDrab;
              SpecialItemsColStart = i;  // not sure where the specialitems col starts but this is not one of them
            }
            _HeaderStr = _HeaderStr.Remove(0, _DelimiterPos + 1);
          }
          // remove vowels then shrink the text if still to be
          string[] vowels = {"a","e","i","o","u"," "};
          if (_HeaderStr.Length > COL_ITEM_MAXLEN) 
          {
            int j = 0;
            while ((_HeaderStr.Length > COL_ITEM_MAXLEN) && (j < vowels.Length))
            {
              _HeaderStr = _HeaderStr.Replace(vowels[j], "");
              j++;
            }
            // still to big then trunc it further
            if (_HeaderStr.Length > COL_ITEM_MAXLEN)
              _HeaderStr = _HeaderStr.Remove(COL_ITEM_MAXLEN);
          }
          lnkBtn.Text = _HeaderStr;

//          _TotalsTable.Rows.Add(
        }
      }
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        if (e.Row.DataItem != null)
        {
          DataRow myRow = ((DataRowView)e.Row.DataItem).Row;
          // set the colour of the date column depending on whick week it is in
          if (myRow.ItemArray[COL_ROASTDATE] != null)
          {
            DateTime dt = Convert.ToDateTime(myRow.ItemArray[COL_ROASTDATE - COL_NUMCOMMANDCOLS]);

            if (dt < dtThisRoastWeekStart)
              e.Row.Cells[COL_ROASTDATE].ForeColor = System.Drawing.Color.DeepPink;
            else if ((dtThisRoastWeekStart <= dt) && (dt < dtThisRoastWeekEnd))
            {
              e.Row.Cells[COL_ROASTDATE].ForeColor = System.Drawing.Color.DarkOliveGreen;
              e.Row.Cells[COL_ROASTDATE].Font.Bold = true;
            }
            else
            {
              e.Row.Cells[COL_ROASTDATE].ForeColor = System.Drawing.Color.DarkOrchid;
//              e.Row.Cells[COL_ROASTDATE].Font.Italic = true;
            }
            // centre align the colums that need to 
            e.Row.Cells[COL_DELVERYBY].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            // now the item colums format and calc
            // for check that the array has enough space, and if not that means the is the first row so initialize
            if (ColTotals == null)
            {
              ColTotals = new double[e.Row.Cells.Count - COL_ITEMSSTART];
              for (int i = 0; i < ColTotals.Length; i++)
                ColTotals[i] = 0;
              CountOrders = 0;
            }
            // now format and add values
            for (int i = COL_ITEMSSTART; i <  e.Row.Cells.Count; i++)
            {
              // change format of the numbers
              e.Row.Cells[i].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
              // change background for non special colums odd and even catering
              if (i <= SpecialItemsColStart)
              {
                if ((e.Row.RowIndex % 2) == 0)
                  e.Row.Cells[i].BackColor = System.Drawing.Color.WhiteSmoke;
                else
                  e.Row.Cells[i].BackColor = System.Drawing.Color.Snow;
              }
              //check the column values to determing its formatting and do totals
              if ((myRow.ItemArray[i - COL_NUMCOMMANDCOLS].ToString().Length > 0))
              {
                double dQty = Convert.ToDouble(myRow.ItemArray[i - COL_NUMCOMMANDCOLS]);
                // if needs packaging, then change background
                if ((dQty % 1) != 0)
                {                  //              if ((e.Row.RowIndex % 2) == 0)
                  e.Row.Cells[i].BackColor = System.Drawing.Color.MistyRose;
                }
                ColTotals[i - COL_ITEMSSTART] += dQty;
                CountOrders++;  // increment the number of orders
              }
            }
          }
        }
      }
      if (e.Row.RowType == DataControlRowType.Footer)
      {
        double _TotalSum = 0;
        // when the grid is print it has some funnies, so this Totals need to be recalculated
        if (ColTotals != null)
        {
          for (int i = COL_ITEMSSTART; i < e.Row.Cells.Count; i++)
          {
            if (i > SpecialItemsColStart)
              _TotalSum += ColTotals[i - COL_ITEMSSTART];
            e.Row.Cells[i].Text = String.Format("{0:#,0.##}", ColTotals[i - COL_ITEMSSTART]);
            e.Row.Cells[i].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
          }
        }
        // write the totals and text
        e.Row.Cells[COL_ROASTDATE].Text = "Count:";
        e.Row.Cells[COL_BYDATE].Text = CountOrders.ToString();
        e.Row.Cells[COL_BYDATE].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
        e.Row.Cells[COL_DELVERYTO].Text = String.Format("Total ({0})", _TotalSum);
      }
    }
    protected void gvOrderSheet_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.Header)
      {//  format stuff here
      }
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        if (e.Row.Cells.Count > 0)
        {
          // for the date columns format them to show short date
          DataRow myRow = ((DataRowView)e.Row.DataItem).Row;
          e.Row.Cells[COL_ROASTDATE].Text = String.Format("{0:d}", (DateTime)myRow[COL_ROASTDATE - COL_NUMCOMMANDCOLS]);
          e.Row.Cells[COL_BYDATE].Text = String.Format("{0:d}", (DateTime)myRow[COL_BYDATE - COL_NUMCOMMANDCOLS]);
          // Check if co name to big, then change font size
          if (e.Row.Cells[COL_DELVERYTO].Text.Length > COL_NAME_MAX)
            e.Row.Cells[COL_DELVERYTO].Font.Size = FontUnit.Smaller;
        }
      }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
      Session["PrintCtrl"] = gvOrderSheet;
      ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>");
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
      string _FilterStr = "";

      switch (ddlFilterBy.SelectedIndex)
      {
        case DDL_VALUE_COMPANY_NAME:         // 1
          _FilterStr = String.Format("DeliveryTo LIKE '%{0}%'", tbxFilter.Text);
          break;
        case DDL_VALUE_DELIVERY_BY:        // 4; 
          _FilterStr = String.Format("DlvryBy LIKE '{0}%'", ddlDeliveryBy.SelectedValue);
          break;
        case DDL_VALUE_DELIVERY_DATE:        // 2
          _FilterStr = String.Format("ByDate = '{0}'", Convert.ToDateTime(tbxFilter.Text).ToShortDateString());
          break;
        case DDL_VALUE_ROAST_DATE:        // 3;
          _FilterStr = String.Format("RoastDT = '{0}'", Convert.ToDateTime(tbxFilter.Text).ToShortDateString());
          break;
        case DDL_VALUE_DELIVERYBY_AND_DELIVERY_DATE: // 5
          _FilterStr = String.Format("(DlvryBy LIKE '{0}%') AND (ByDate = '{1}')", ddlDeliveryBy.SelectedValue, Convert.ToDateTime(tbxFilter.Text).ToShortDateString());
          break;
        case DDL_VALUE_DELIVERYBY_AND_ROAST_DATE: // 6
          _FilterStr = String.Format("(DlvryBy LIKE '{0}%') AND (RoastDT = '{1}')", ddlDeliveryBy.SelectedValue, Convert.ToDateTime(tbxFilter.Text).ToShortDateString());
          break;

        default:
          _FilterStr = "";
          break;
      }

      odsOrderSheet.FilterExpression = _FilterStr;
      ltrlFilter.Text = String.Format("<b>Filtered by</b>: [{0}]", _FilterStr);
      odsOrderSheet.DataBind();
    }
    protected void ddlFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
      // hide and display controls depending on what is selected
      bool _showDDL = ((ddlFilterBy.SelectedIndex == DDL_VALUE_DELIVERY_BY) ||
        (ddlFilterBy.SelectedIndex == DDL_VALUE_DELIVERYBY_AND_DELIVERY_DATE) ||
        (ddlFilterBy.SelectedIndex == DDL_VALUE_DELIVERYBY_AND_ROAST_DATE));
      bool _showText = (ddlFilterBy.SelectedIndex != DDL_VALUE_DELIVERY_BY);

      tbxFilter.Visible = _showText;
      tbxFilter.Enabled = _showText;
      ddlDeliveryBy.Visible = _showDDL;
    }

    protected void cbxOrdersToDo_CheckedChanged(object sender, EventArgs e)
    {
      bool _OrdersDone = !cbxOrdersToDo.Checked;

      // set params
      odsOrderSheet.SelectParameters.Clear();
      Parameter _OrderParams = new Parameter();
      _OrderParams.DbType =   DbType.Boolean;
      _OrderParams.DefaultValue =  _OrdersDone.ToString();
      _OrderParams.Name = "Done";
      odsOrderSheet.SelectParameters.Add(new Parameter());
      odsOrderSheet.DataBind();
      gvOrderSheet.DataBind();
    }

    protected void gvOrderSheet_SelectedIndexChanged(object sender, EventArgs e)
    {
      string _strRoast = gvOrderSheet.SelectedRow.Cells[COL_ROASTDATE].Text;
      string _strDeliveryTo = gvOrderSheet.SelectedRow.Cells[COL_DELVERYTO].Text;

      int _RemoveFrom = _strDeliveryTo.IndexOf("{");
      if (_RemoveFrom > 0)
        _strDeliveryTo = _strDeliveryTo.Remove(_RemoveFrom - 1);      

      Response.Redirect(String.Format("OrdersEdit.aspx?{0}={1}&{2}={3}", OrdersEdit.QS_ROASTDATE, _strRoast, OrdersEdit.QS_COMPANYNAME, _strDeliveryTo));
    }

  }
}