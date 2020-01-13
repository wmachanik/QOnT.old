using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QOnT.classes;
using System.Data.OleDb;
using QOnT.control;

namespace QOnT.Pages
{
  public partial class OrderDone : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// Add the actual items to the Usage table and Usage Detail Line from the temp tables checking that the customer
    /// Id is the one that the temp tables are populated with. Also exclude any n/a service types
    /// </summary>
    /// <param name="pCustomerID">for which customer</param>
    /// <param name="pIsActual">is this an actual count</param>
    /// <param name="pCupCount">what is teh starting CupCount</param>
    /// <returns>Cup Count</returns>
    private long AddItemsToClientUsageTbl(long pCustomerID, bool pIsActual, long pCupCount, double pStock, DateTime pDeliveryDate)
    {
      ClientUsageFromTempOrder _ClientUsageFromTempOrderDAL = new ClientUsageFromTempOrder();
      List<ClientUsageFromTempOrder> _TempOrderDataLines = _ClientUsageFromTempOrderDAL.GetAll(pCustomerID);
      // create null data records for the tables we are going to populate
      List<ItemUsageTbl> _ItemUsageLines = new List<ItemUsageTbl>();
      List<ClientUsageLinesTbl> _ClientUsageLines = new List<ClientUsageLinesTbl>();
      int _LineNo = 0;
      
      // use the note to make comments in the item usage table
      string _strNotes = (pIsActual) ? "actual count" : "estimate count";
      if (pStock > 0) {
        pCupCount = pCupCount - Convert.ToInt64(pStock * TrackerTools.CONST_TYPICALNUMCUPSPERKG);      // adjust cup count so that it reflects stock
        _strNotes += "; Stock of: " + pCupCount.ToString();
      }

      // For every holiday period int he last 6 remove the period from the data difference
      while (_TempOrderDataLines.Count > _LineNo)
      {
//        _strNotes = "";     // clear last notes, ready for new notes

        ClientUsageLinesTbl _ClientUsageItem = new ClientUsageLinesTbl();
        // now calc per item a total
        _ClientUsageItem.CustomerID = _TempOrderDataLines[_LineNo].CustomerID;
        _ClientUsageItem.LineDate = pDeliveryDate;
        _ClientUsageItem.ServiceTypeID = _TempOrderDataLines[_LineNo].ServiceTypeID;
        _ClientUsageItem.Qty = 0;
        _ClientUsageItem.CupCount = pCupCount;
        _ClientUsageItem.Notes = _strNotes;

        do
        {
          // add all quantities for this service type

          _ClientUsageItem.Qty += (_TempOrderDataLines[_LineNo].Qty * _TempOrderDataLines[_LineNo].UnitsPerQty);
          ItemUsageTbl _ItemUsageItem = new ItemUsageTbl();
          // copy line item to an new line in item usage
          _ItemUsageItem.CustomerID = _TempOrderDataLines[_LineNo].CustomerID;
          _ItemUsageItem.Date = pDeliveryDate;
          _ItemUsageItem.ItemProvided = _TempOrderDataLines[_LineNo].ItemID;
          _ItemUsageItem.AmountProvided = _TempOrderDataLines[_LineNo].Qty;
          _ItemUsageItem.PackagingID = _TempOrderDataLines[_LineNo].PackagingID;
          _ItemUsageItem.Notes = _strNotes;
          _ItemUsageLines.Add(_ItemUsageItem);
          _LineNo++;
        } while ((_TempOrderDataLines.Count > _LineNo) && (_ClientUsageItem.ServiceTypeID == _TempOrderDataLines[_LineNo].ServiceTypeID));         

        _ClientUsageLines.Add(_ClientUsageItem);

      }
      // add client usage lineslines
      for (int i = 0; i < _ClientUsageLines.Count; i++)
      {
        _ClientUsageLines[i].InsertItemsUsed(_ClientUsageLines[i]);
      }

      // add item usage lineslines
      for (int i = 0; i < _ItemUsageLines.Count; i++)
      {
        _ItemUsageLines[i].InsertItemsUsed(_ItemUsageLines[i]);
      }
      return pCupCount;
    }

    protected void ShowResults(string CustomerName, long pCustomerId, ClientUsageTbl pOriginalUsageData)
    {
      pnlOrderDetails.Visible = false;

      tbxCustomerName.Text = CustomerName;

      List<ClientUsageTbl> _NewAndOld = new List<ClientUsageTbl>();

      _NewAndOld.Add(pOriginalUsageData);
      _NewAndOld.Add(new ClientUsageTbl().GetUsageData(pCustomerId));

      dgCustomerUsage.AutoGenerateColumns = false;
      dgCustomerUsage.DataSource = _NewAndOld;
      dgCustomerUsage.DataBind();

      pnlCustomerDetailsUpdated.Visible = true;
    }


    //protected void MarkTempOrdersItemsAsDone(long pCustomerID)
    //{
    //  string _SQLUpdate = "UPDATE OrdersTbl SET OrdersTbl.Done = True WHERE CustomderId = " + pCustomerID.ToString() +
    //                         "AND EXISTS (SELECT RequiredByDate FROM TempOrdersHeaderTbl " +
    //                         "            WHERE (RequiredByDate = OrdersTbl.RequiredByDate))";

    //  TrackerDb _TrackerDb = new TrackerDb();
    //  _TrackerDb.ExecuteNonQuerySQL(_SQLUpdate);

    //  // ResetCustomerReminderCount(pCustomerId);
    //  //'''''''''''''
    //  //' mark orders as done - should do this last
    //  //'''
    //  //lblStatus.Caption = "Marking orders as done"
    //  //dbs.Execute ("UPDATE OrdersTbl SET OrdersTbl.Done = True " + _
    //  //             "WHERE (((Exists (Select TempOrdersTbl.OrderId from TempOrdersTbl where  " + _
    //  //                       "TempOrdersTbl.OrderId = OrdersTbl.OrderId))<>False))")
    //  //'''''''
    //  //'  Resetting count and enable, only if coffee item
    //  //'
    //  //dbs.Execute ("UPDATE DISTINCTROW CustomersTbl SET ReminderCount = 0, enabled = True WHERE CustomerID=" + lblCustomerID.Caption)
    //}

    // Logic for processing order as done:
    // 1. move items consumed from temp aable to the items consumed for the contact
    // 2. using the summary of the data on the form do calculations around predictions
    //    on when the contact needs items again.
    // 3. Notify client of items deliverred (add option to contacts table)
    // 4. return to the previous page.
    protected void btnDone_Click(object sender, EventArgs e)
    {
      const long MAXQTYINSTOCK = 50;
      
      TrackerTools _TrackerTools = new TrackerTools();
      _TrackerTools.SetTrackerSessionErrorString(string.Empty);

      Label _CustomerIDLabel =  (Label)(fvOrderDone.FindControl("CustomerIDLabel"));
      Label _CustomerNameLabel =  (Label)(fvOrderDone.FindControl("CompanyNameLabel"));
      long _CustomerID = Convert.ToInt64(_CustomerIDLabel.Text);
      TextBox _OrderDateLabel = (TextBox)(fvOrderDone.FindControl("ByDateTextBox"));
      DateTime _OrderDate = Convert.ToDateTime(_OrderDateLabel.Text);
      double _CoffeeStock = 0;
      // store the current data for display later:
      ClientUsageTbl _OriginalUsageDAL = new ClientUsageTbl();
      ClientUsageTbl _OriginalUsageData = _OriginalUsageDAL.GetUsageData(_CustomerID);

      if (!string.IsNullOrEmpty(_TrackerTools.GetTrackerSessionErrorString()))
      {
        Response.Write(_TrackerTools.GetTrackerSessionErrorString());
      }

      TempOrdersDAL _TempOrdersDAL = new TempOrdersDAL();   //
      bool _HasCoffeeInTempOrder = _TempOrdersDAL.HasCoffeeInTempOrder();
      if (!string.IsNullOrEmpty(_TrackerTools.GetTrackerSessionErrorString()))
      {
        Response.Write(_TrackerTools.GetTrackerSessionErrorString());
      }

      /* NEED TO ADD Code to check for zzname */
      
      // check cup count not in wrong place
      if ((tbxStock.Text.Length > 0) && (Convert.ToInt64(tbxStock.Text) > MAXQTYINSTOCK))
      {
        ltrlStatus.Text = "<b>The stock quantity appears very high please check that you have enterred the correct value in kilograms.</b>";
      }
      else
      {
        _CoffeeStock = String.IsNullOrEmpty(tbxStock.Text) ? 0 : Convert.ToDouble(tbxStock.Text);
          
        // First we must get the latest cup count
        ltrlStatus.Text = "Calculating the latest cup count";
        GeneralTrackerDbTools _GeneralTrackerDb = new GeneralTrackerDbTools();

        GeneralTrackerDbTools.LineUsageData _LatestCustomerData = _GeneralTrackerDb.GetLatestUsageData(_CustomerID, TrackerTools.CONST_SERVTYPECOFFEE);
        if (!string.IsNullOrEmpty(_TrackerTools.GetTrackerSessionErrorString()))
        {
          Response.Write(_TrackerTools.GetTrackerSessionErrorString());
        }

        bool _bIsActual = (tbxCount.MaxLength > 0);  // if there is a cup count
        long _lCupCount = 0;
        if (tbxCount.MaxLength > 0)
          _lCupCount = Convert.ToInt64(tbxCount.Text);

        // Calculate the cup count if we do not have it on the form or they entered a value that makes no sense
        if ((_lCupCount < 1) || (_lCupCount < _LatestCustomerData.LastCount))
        {
          ltrlStatus.Text = "Calculating the latest est cup count";
          _lCupCount = _GeneralTrackerDb.CalcEstCupCount(_CustomerID, _LatestCustomerData, _HasCoffeeInTempOrder);
          _bIsActual = false;
        }
        
        //' add items to consumption log
        _lCupCount = AddItemsToClientUsageTbl(_CustomerID, _bIsActual, _lCupCount, _CoffeeStock, _OrderDate);

        // update the last cup count for the client
        if (!_OriginalUsageDAL.UpdateUsageCupCount(_CustomerID, _lCupCount))
          ltrlStatus.Text = "error updating last count";
        
        // now update the predictions
        _GeneralTrackerDb.UpdatePredictions(_CustomerID, _lCupCount);

        // update all items in the order table as done that are done
        _TempOrdersDAL.MarkTempOrdersItemsAsDone();

        // reset count and enable client
        _GeneralTrackerDb.ResetCustomerReminderCount(_CustomerID, _HasCoffeeInTempOrder);
        // destroy the temp table that we used to create this temp orders 
        _TempOrdersDAL.KillTempOrdersData();

        ShowResults(_CustomerNameLabel.Text,_CustomerID,_OriginalUsageData);
      }
    }

    protected void btnReturnToDeliveres_Click(object sender, EventArgs e)
    {
      //' Close the form and return to delivery sheet - should somehow check session vars or something
      Response.Redirect("DeliverySheet.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
      Response.Redirect(Request.UrlReferrer.ToString());
    }
  }
}