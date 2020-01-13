using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QOnT.Pages
{
  public partial class CoffeeRequired : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    protected void gvPreperationDay_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        Label _GroupTitleLabel = (Label)e.Row.FindControl("lblGroupTitle");
        Label _QtyLabel = (Label)e.Row.FindControl("lblQty");
        string _strVal = _GroupTitleLabel.Text;
        double _dblQty = Convert.ToDouble(_QtyLabel.Text);
        string _strGroupTitle = (string)ViewState["GroupTitle"];
        double _dblGroupTotal = (ViewState["GroupTotal"]==null) ? 0 : (double)ViewState["GroupTotal"];
        if (_strGroupTitle == _strVal)
        {
          // calculate totals
          _dblGroupTotal += _dblQty;
          _GroupTitleLabel.Visible = false;
          _GroupTitleLabel.Text = string.Empty;
        }
        else
        {
          string _strHTML = "";
          // Seperate Rows totals
          _strGroupTitle = _strVal;
          ViewState["GroupTitle"] = _strGroupTitle;
          _GroupTitleLabel.Visible = true;
          if (_dblGroupTotal != 0)
            _strHTML = String.Format("<b>Total</b></td><td align='right'><b>{0}</b></td>",_dblGroupTotal);
          else
            _strHTML = "</td><td></td>";
          // now add the title
          // this is a header so move all goodies to the next row.
          _strHTML += "</tr><tr><td colspan='2'><b>Prep Date</b>: " + _strGroupTitle + "</td></tr><tr><td>";

          _GroupTitleLabel.Text = _strHTML;
          _dblGroupTotal = _dblQty;  // add this quantity as it is the first one.
        }
        ViewState["GroupTotal"] = _dblGroupTotal;
      }
      else if (e.Row.RowType == DataControlRowType.Footer)
      {
        // Rows totals
        double _dblGroupQty = (ViewState["GroupTotal"] == null) ? 0 : (double)ViewState["GroupTotal"];
        Label _GroupFooterQty = (Label)e.Row.FindControl("lblFooterQty");
        _GroupFooterQty.Text = _dblGroupQty.ToString();
      }
    }
    protected void gvCoffeeRequireByDay_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        Label _ByGroupTitleLabel = (Label)e.Row.FindControl("lblByGroupTitle");
        Label _ByAbreviationLabel = (Label)e.Row.FindControl("lblByAbreviation");
        Label _ByQtyLabel = (Label)e.Row.FindControl("lblByQty");
        string _strByVal = String.Format("{0} ({1})", _ByGroupTitleLabel.Text, _ByAbreviationLabel.Text);
        double _dblByQty = Convert.ToDouble(_ByQtyLabel.Text);
        string _strByGroupTitle = (string)ViewState["GroupByTitle"];
        double _dblByGroupTotal = (ViewState["GroupByTotal"] == null) ? 0 : (double)ViewState["GroupByTotal"];
        if (_strByGroupTitle == _strByVal)
        {
          // calculate totals
          _dblByGroupTotal += _dblByQty;
          _ByGroupTitleLabel.Visible = false;
          _ByGroupTitleLabel.Text = string.Empty;
        }
        else
        {
          string _strHTML = "";
          // Seperate Rows totals
          _strByGroupTitle = _strByVal;
          ViewState["GroupByTitle"] = _strByGroupTitle;
          _ByGroupTitleLabel.Visible = true;
          if (_dblByGroupTotal != 0)
            _strHTML = String.Format("<b>Total</b></td><td colspan='2' align='right'><b>{0}</b></td>", _dblByGroupTotal);
          else
            _strHTML = "</td><td></td>";
          // now add the title
          // this is a header so move all goodies to the next row.
          _strHTML += "</tr><tr><td colspan='3'><b>Required Date (By)</b>: " + _strByGroupTitle + "</td></tr><tr><td>";

          _ByGroupTitleLabel.Text = _strHTML;
          _dblByGroupTotal = _dblByQty;  // add this quantity as it is the first one.
        }
        ViewState["GroupByTotal"] = _dblByGroupTotal;

      }
      else if (e.Row.RowType == DataControlRowType.Footer)
      {
        // Seperate Rows totals
        double _dblByGroupQty = (ViewState["GroupByTotal"] == null) ? 0 : (double)ViewState["GroupByTotal"];
        Label _ByGroupFooterQty = (Label)e.Row.FindControl("lblByFooterQty");
        _ByGroupFooterQty.Text = _dblByGroupQty.ToString();
      }

    }
  }
}