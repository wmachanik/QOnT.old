using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QOnT.control
{
  /// SQL Command for this class that is used in the delivery sheet
  /// SELECT OrdersTbl.OrderID, CustomersTbl.CompanyName, OrdersTbl.CompanyId, OrdersTbl.OrderDate, OrdersTbl.RoastDate, OrdersTbl.ItemTypeID, ItemTypeTbl.ItemDesc,
  ///        OrdersTbl.QuantityOrdered, ItemTypeTbl.ItemShortName, ItemTypeTbl.ItemEnabled, ItemTypeTbl.ReplacementID, CityPrepDaysTbl.DeliveryOrder
  ///        ItemTypeTbl.SortOrder, OrdersTbl.RequiredByDate, OrdersTbl.ToBeDeliveredBy, OrdersTbl.Confirmed, OrdersTbl.Done, OrdersTbl.Notes
  ///        PackagingTbl.Description AS PackDesc, PackagingTbl.BGColour, PersonsTbl.Abreviation
  /// 

  public class DeliveryItemsTbl
  {
    public DeliveryItemsTbl()
    {
      _otOrderID = _otCompanyId = _otItemTypeID = _itReplacementID = 0;
      _cpdDeliveryOrder = _itSortOrder = _ptBGColour = 0;
      _ctCompanyName = _itItemDesc = _itItemShortName = _otToBeDeliveredBy = _otNotes = _PackDesc = _ptAbreviation = "";
      _otQuantityOrdered = 0.00;
      _otOrderDate = _otRoastDate = _otRequiredDate = DateTime.Now;
      _itItemEnabled = _otConfirmed = true;
      _otDone = false;
    }

    private long _otOrderID, _otCompanyId, _otItemTypeID, _itReplacementID;
    private int _cpdDeliveryOrder, _itSortOrder, _ptBGColour;
    private string _ctCompanyName, _itItemDesc, _itItemShortName, _otToBeDeliveredBy, _otNotes, _PackDesc, _ptAbreviation;
    private double _otQuantityOrdered;
    private DateTime _otOrderDate, _otRoastDate, _otRequiredDate;
    private bool _itItemEnabled, _otConfirmed, _otDone;

    public long otOrderID { get { return _otOrderID; } set { _otOrderID = value; } }
    public long otCompanyId { get {return _otCompanyId; } set { _otCompanyId = value; } }
    public long otItemTypeID { get {return _otItemTypeID; } set { _otItemTypeID = value; } }
    public long itReplacementID { get {return _itReplacementID; } set { _itReplacementID = value; } }
    public int ptBGColour { get { return _ptBGColour; } set { _ptBGColour = value; } }
    public int cpdDeliveryOrder { get { return _cpdDeliveryOrder; } set { _cpdDeliveryOrder = value; } }
    public int itSortOrder { get { return _itSortOrder; } set { _itSortOrder = value; } }
    public string ctCompanyName { get { return _ctCompanyName; } set { _ctCompanyName = value; }}
    public string itItemDesc { get { return _itItemDesc; } set { _itItemDesc = value; }}
    public string itItemShortName { get {return _itItemShortName; } set { _itItemShortName = value; }}
    public string otToBeDeliveredBy { get {return _otToBeDeliveredBy; } set { _otToBeDeliveredBy = value; }}
    public string otNotes { get {return _otNotes;} set { _otNotes = value;}}
    public string PackDesc { get { return _PackDesc;} set { _PackDesc = value; }}
    public string ptAbreviation { get { return _ptAbreviation; } set { _ptAbreviation = value; } }
    public double otQuantityOrdered { get { return _otQuantityOrdered; } set { _otQuantityOrdered = value;}}
    public DateTime otOrderDate { get { return _otOrderDate; } set { _otOrderDate = value;}}
    public DateTime otRoastDate { get { return _otRoastDate;} set { _otRoastDate = value;}}
    public DateTime otRequiredDate { get { return _otRequiredDate; } set { _otRequiredDate = value; } }
    public bool itItemEnabled { get { return _itItemEnabled; } set { _itItemEnabled = value; } }
    public bool otConfirmed { get { return _otConfirmed; } set { _otConfirmed = value; } }
    public bool otDone { get { return _otDone; } set { _otDone = value; } }
  }
}