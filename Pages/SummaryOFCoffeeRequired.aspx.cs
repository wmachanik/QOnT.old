using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QOnT.Pages
{

  /// <summary>
  /// This page is designed to display the coffee to be roasted and give a guide if it is enough.
  /// The page allows the person to select a roasting week and then display a summary of the coffees that are required.
  /// 
  /// Then underneath there it displays an average sale over the last 6 weeks, if the quantity of coffee over those weeks was greater than 2kgs.
  /// </summary>
  public partial class SummaryOFCoffeeRequired : System.Web.UI.Page
  {

    class DateRange {
      public int idx;
      public DateTime StartDate;
      public DateTime EndDate;
    }

    List<DateRange> PrepDates = new List<DateRange>();


    protected void Page_Load(object sender, EventArgs e)
    {
      
      if (!IsPostBack) {
        // get the first date of the week
        DateTime dtStart = DateTime.Now.AddDays(- (int)DateTime.Now.DayOfWeek);

        for (int i = 0; i < 5; i++)
        {
          PrepDates.Add(new DateRange{
            idx = i,
            StartDate = dtStart,
            EndDate = dtStart.AddDays(7)  // 7 days a week, perhaps may change?
          });
          ddlWhichWeek.Items.Add(new ListItem {
            Text = String.Format("{0:d} to {1:d}",PrepDates[i].StartDate, PrepDates[i].EndDate),
            Value = i.ToString()
          });
        }
      }
    }
    /// <summary>
    /// Using the week select create a summary dependant on the dates in the week.
    /// </summary>
    void CreateSummaryforWeek()
    {


    }

    protected void ddlWhichWeek_SelectedIndexChanged(object sender, EventArgs e)
    {
      CreateSummaryforWeek();
    }
  }
}