using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QOnT.control
{
  public class OrderHeaderData
  {
    //  CustomerId, OrderDate, RoastDate, RequiredByDate, ToBeDeliveredBy, Confirmed, Done, Notes
    public const string CONST_BOUNDCUSTOMERID = "BoundCustomerId";
    public const string CONST_BOUNDDELIVERYDATE = "BoundDeliveryDate";
    public const string CONST_BOUNDNOTES = "BoundNotes";

    public OrderHeaderData()
    {
      _otCustomerID = _otToBeDeliveredBy = 0;
      _otNotes = "";
      _otOrderDate = _otRoastDate = _otRequiredByDate = DateTime.Now;
      _otConfirmed = true;
      _otDone = false;
    }

    private long _otCustomerID;
    private long _otToBeDeliveredBy;
    private DateTime _otOrderDate;
    private DateTime _otRoastDate;
    private DateTime _otRequiredByDate;
    private bool _otConfirmed;
    private bool _otDone;
    private string _otNotes;

    public long CustomerID { get { return _otCustomerID; } set { _otCustomerID = value; } }
    public long ToBeDeliveredBy { get { return _otToBeDeliveredBy; } set { _otToBeDeliveredBy = value; } }
    public DateTime OrderDate { get { return _otOrderDate; } set { _otOrderDate = value; } }
    public DateTime RoastDate { get { return _otRoastDate; } set { _otRoastDate = value; } }
    public DateTime RequiredByDate { get { return _otRequiredByDate; } set { _otRequiredByDate = value; } }
    public bool Confirmed { get { return _otConfirmed; } set { _otConfirmed = value; } }
    public bool Done { get { return _otDone; } set { _otDone = value; } }
    public string Notes { get { return _otNotes; } set { _otNotes = value; } }

  }
}