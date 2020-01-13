using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Configuration;

namespace QOnT.Pages
{
  public partial class OrdersEdit : System.Web.UI.Page
  {
    public const string QS_FORCENEW = "NewOrder";
    public const string QS_ROASTDATE = "RoastDate";
    public const string QS_COMPANYNAME = "CompanyName";
     
    // DropDownValues
    const int DDL_VALUE_COMPANY_NAME = 1;
    const int DDL_VALUE_DELIVERY_BY = 2;
    const int DDL_VALUE_NOTES = 3;
    const int DDL_VALUE_DELIVERY_DATE = 4;
    const int DDL_VALUE_ROAST_DATE = 5;

    // controls to set default values
    const string DV_CONTROL_CUSTOMER = "ddlCustomer";
    const string DV_CONTROL_ORDER_DATE = "tbxDetailOrderDate";
    const string DV_CONTROL_ROAST_DATE = "tbxDetailRoastDate";
    const string DV_CONTROL_REQUIRED_BY_DATE = "tbxRequiredByDate";
    const string DV_CONTROL_QTY = "tbxQuantity";
    const string DV_CONTROL_DELIVERY_BY = "ddlDeliveryBy";
    const string DV_CONTROL_CONFIRMED = "cbxConfirmed";
    const string DV_CONTROL_NOTES = "tbxNotes";

    const string SV_ORDERVALUES = "Confirmed";

    // ODS values
    const string GV_ALLORDERS_Method = "GetData";
    const string GV_DONEORDERS_Method = "GetDataByDone";

    struct OrderValues
    {
      public int CustomerID;
      public DateTime dtOrder;
      public DateTime dtRoast;
      public DateTime dtRequiredBy;
      public int DeliverByID;
      public bool IsConfirmed;
      public string Notes;
    }

    private void InitializeOrderVars()
    {
      QOnT.classes.TrackerTools tt = new classes.TrackerTools();
      DateTime dt = tt.GetClosestNextRoastDate(DateTime.Now);

      OrderValues _MyOrderValues = new OrderValues();

      _MyOrderValues.CustomerID = 0;
      _MyOrderValues.dtOrder = DateTime.Now;
      _MyOrderValues.dtRoast = dt;
      _MyOrderValues.dtRequiredBy = dt.AddDays(1);
      _MyOrderValues.DeliverByID = Convert.ToInt16(ConfigurationManager.AppSettings["DefaultDeliveryID"]);   // mzukisi?
      _MyOrderValues.IsConfirmed = true;
      _MyOrderValues.Notes = "";

      Session[SV_ORDERVALUES] = _MyOrderValues;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (Session[SV_ORDERVALUES] == null)
          InitializeOrderVars();

        // is this a request to add a new records?
        if (Request.QueryString[QS_FORCENEW] == "Yes")
        {
          dvOrderEdit.ChangeMode(DetailsViewMode.Insert);
        }
        else if (Request.QueryString[QS_ROASTDATE] != null)
        {
          // they have ask to filter the results by roast date and company
          string _FilterStr = String.Format("RoastDate = '{0}'", Request.QueryString[QS_ROASTDATE]);
          if (Request.QueryString[QS_COMPANYNAME] != null)
          {
            string _CoNameStr = Request.QueryString[QS_COMPANYNAME];
            int _IsGeneralName = _CoNameStr.IndexOf("C/D");

            if (_IsGeneralName >= 0)
            {
              _CoNameStr = _CoNameStr.Remove(_IsGeneralName, 4);
              _FilterStr += String.Format("AND Notes LIKE '%{0}%'", _CoNameStr);
            }
            else
              _FilterStr += String.Format("AND CompanyName LIKE '%{0}%'", _CoNameStr);
          }
          // set filter and expression
          odsOrdersTbl.FilterExpression = _FilterStr;
          // set the filter string
          ltrlFilter.Text = String.Format("<b>Filtered by</b>: [{0}]", _FilterStr);
          BindGridViewData(sender, e);          
        }
      }
    }
    protected void dvOrderEdit_OnDataBound(object sender, EventArgs e)
    {
      DetailsView myDetailsView = (DetailsView)sender;
      if (myDetailsView.CurrentMode == DetailsViewMode.Insert)
      {
        //      QOnT.classes.TrackerTools tt = new classes.TrackerTools();
        //      DateTime dt = tt.GetClosestNextRoastDate(DateTime.Now);

        // if there session has lost vars re-initializa
        if (Session[SV_ORDERVALUES] == null)
          InitializeOrderVars();

        // retrieve values
        OrderValues _MyOrderValues = (OrderValues)Session[SV_ORDERVALUES];

        // take values from session and set them
        ((DropDownList)myDetailsView.FindControl(DV_CONTROL_CUSTOMER)).SelectedIndex = _MyOrderValues.CustomerID;
        ((TextBox)myDetailsView.FindControl(DV_CONTROL_ORDER_DATE)).Text = _MyOrderValues.dtOrder.ToString("d");
        ((TextBox)myDetailsView.FindControl(DV_CONTROL_ROAST_DATE)).Text = _MyOrderValues.dtRoast.ToString("d");
        ((TextBox)myDetailsView.FindControl(DV_CONTROL_REQUIRED_BY_DATE)).Text = _MyOrderValues.dtRequiredBy.ToString("d");
        ((DropDownList)myDetailsView.FindControl(DV_CONTROL_DELIVERY_BY)).SelectedIndex = _MyOrderValues.DeliverByID;
        ((TextBox)myDetailsView.FindControl(DV_CONTROL_QTY)).Text = "1";
        ((CheckBox)myDetailsView.FindControl(DV_CONTROL_CONFIRMED)).Checked = _MyOrderValues.IsConfirmed;
        ((TextBox)myDetailsView.FindControl(DV_CONTROL_NOTES)).Text = _MyOrderValues.Notes;
      }
    }

    public static List<string> AddQtyAutoValues(string prefixText, int count)
    {
      double dQty = 0.25;
      List<string> QtyStrList = new List<string>();

      for (int i = 0; i < 100; i++)
      {
        QtyStrList.Add(Convert.ToString(dQty));
        dQty = dQty + .025;
      }
      return QtyStrList;
    }

    protected void dvOrderEdit_OnItemInserted(object sender, EventArgs e)
    {
      DetailsView myDetailsView = (DetailsView)sender;
      //      QOnT.classes.TrackerTools tt = new classes.TrackerTools();
      //      DateTime dt = tt.GetClosestNextRoastDate(DateTime.Now);

      // retrieve values
      OrderValues _MyOrderValues = new OrderValues();

      // take values from session and set them
      DropDownList _ddlCustomers = ((DropDownList)myDetailsView.FindControl(DV_CONTROL_CUSTOMER));
      _MyOrderValues.CustomerID = _ddlCustomers.SelectedIndex;
      _MyOrderValues.dtOrder = Convert.ToDateTime(((TextBox)myDetailsView.FindControl(DV_CONTROL_ORDER_DATE)).Text);
      _MyOrderValues.dtRoast = Convert.ToDateTime(((TextBox)myDetailsView.FindControl(DV_CONTROL_ROAST_DATE)).Text);
      _MyOrderValues.dtRequiredBy = Convert.ToDateTime(((TextBox)myDetailsView.FindControl(DV_CONTROL_REQUIRED_BY_DATE)).Text);
      _MyOrderValues.DeliverByID = ((DropDownList)myDetailsView.FindControl(DV_CONTROL_DELIVERY_BY)).SelectedIndex;
      _MyOrderValues.IsConfirmed = ((CheckBox)myDetailsView.FindControl(DV_CONTROL_CONFIRMED)).Checked;
      _MyOrderValues.Notes = ((TextBox)myDetailsView.FindControl(DV_CONTROL_NOTES)).Text;
      // save the values
      Session[SV_ORDERVALUES] = _MyOrderValues;
  
      ltrlStatus.Text = "Order added for customer: " + _ddlCustomers.SelectedValue;
      // now bind the GridView
      BindGridViewData(sender, e);
      ddlFilterBy.Focus();
      this.Form.DefaultButton = btnGo.UniqueID;
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
      if (dvOrderEdit.CurrentMode != DetailsViewMode.Insert)
      {
        dvOrderEdit.ChangeMode(DetailsViewMode.Insert);
      }
      // Response.Redirect("NewOrder.aspx");
    }
    protected void dvOrderEdit_OnModeChanged(object sender, EventArgs e)
    {
      // focus control onto the ddl
      DetailsView myDetailsView = (DetailsView)sender;
      if ((dvOrderEdit.CurrentMode == DetailsViewMode.Edit) || (dvOrderEdit.CurrentMode == DetailsViewMode.Insert))
        myDetailsView.FindControl(DV_CONTROL_CUSTOMER).Focus();
    }

    protected void dvOrderEdit_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
    {
    }

    protected void odsOrdersTbl_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

    }

    protected void BindOrdersGV(object sender, EventArgs e)
    {
      BindGridViewData(sender, e);
    }

    protected void dvOrderEdit_ItemInserted(object sender, EventArgs e)
    {
//      odsOrdersTbl.DataBind();
      BindGridViewData(sender, e);
    }

    protected void ddlOrdersPerPage_SelectedIndexChanged(object sender, EventArgs e)
    {
      gvOrdersEditList.PageSize = Convert.ToInt16(ddlOrdersPerPage.SelectedValue);
      BindGridViewData(sender, e);
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
      string _FilterStr = "";

      switch (ddlFilterBy.SelectedIndex)
      {
        case DDL_VALUE_COMPANY_NAME:         // 1
          _FilterStr = String.Format("CompanyName LIKE '%{0}%'", tbxFilter.Text);
          break;
        case DDL_VALUE_DELIVERY_BY:        // 2; 
          _FilterStr = String.Format("ToBeDeliveredBy = {0}", ddlDeliveryBy.SelectedValue);
          break;
        case DDL_VALUE_NOTES:        // 3; 
          _FilterStr = String.Format("Notes LIKE '%{0}%'", tbxFilter.Text);
          break;
        case DDL_VALUE_DELIVERY_DATE:        // 4
          _FilterStr = String.Format("RequiredByDate = #{0}#", tbxFilter.Text);
          break;
        case DDL_VALUE_ROAST_DATE:        // 3;
          break;
        default:
          _FilterStr = "";
          break;
      }
      // set method
      odsOrdersTbl.FilterExpression = _FilterStr;
      // set the filter string
      ltrlFilter.Text = String.Format("<b>Filtered by</b>: [{0}]", _FilterStr);
      BindGridViewData(sender, e);
    }

    protected void ddlFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
      // hide and display controls depending on what is selected
      tbxFilter.Visible = (ddlFilterBy.SelectedIndex != DDL_VALUE_DELIVERY_BY);
      tbxFilter.Enabled = tbxFilter.Visible;
      ddlDeliveryBy.Visible = !tbxFilter.Visible;
    }
    protected void BindGridViewData(object sender, EventArgs e)
    {
      // set method depending on check box
      odsOrdersTbl.SelectParameters.Clear();    // clear the parameters
      if (cbxOnlyToDo.Checked)
      {
        odsOrdersTbl.SelectMethod = GV_DONEORDERS_Method;
        Parameter pDone = new Parameter();
        pDone.DbType = System.Data.DbType.Boolean;
        pDone.DefaultValue = "false";
        pDone.Name = "Done";
        odsOrdersTbl.SelectParameters.Add(pDone);
      }
      else
        odsOrdersTbl.SelectMethod = GV_ALLORDERS_Method;
      odsOrdersTbl.DataBind();
      gvOrdersEditList.DataBind();

    }
    protected void cbxOnlyToDo_CheckedChanged(object sender, EventArgs e)
    {
      // ?
    }

  }
}