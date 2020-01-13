/// --- auto generated class for table: OrdersTbl
using System;   // for DateTime variables

namespace QOnT.classes
{
  public class OrdersTblData
  {
    // internal variable declarations
    private int _OrderID;
    private int _CustomerId;
    private DateTime _OrderDate;
    private DateTime _RoastDate;
    private int _ItemTypeID;
    private double _QuantityOrdered;
    private DateTime _RequiredByDate;
    private int _PrepTypeID;
    private int _PackagingID;
    private int _ToBeDeliveredBy;
    private bool _Confirmed;
    private bool _Done;
    private bool _Packed;
    private string _Notes;
    // class definition
    public OrdersTblData()
    {
      _OrderID = 0;
      _CustomerId = 0;
      _OrderDate = System.DateTime.Now;
      _RoastDate = System.DateTime.Now;
      _ItemTypeID = 0;
      _QuantityOrdered = 0.0;
      _RequiredByDate = System.DateTime.Now;
      _PrepTypeID = 0;
      _PackagingID = 0;
      _ToBeDeliveredBy = 0;
      _Confirmed = false;
      _Done = false;
      _Packed = false;
      _Notes = string.Empty;
    }
    // get and sets of public
    public int OrderID { get { return _OrderID;}  set { _OrderID = value;} }
    public int CustomerId { get { return _CustomerId;}  set { _CustomerId = value;} }
    public DateTime OrderDate { get { return _OrderDate;}  set { _OrderDate = value;} }
    public DateTime RoastDate { get { return _RoastDate;}  set { _RoastDate = value;} }
    public int ItemTypeID { get { return _ItemTypeID;}  set { _ItemTypeID = value;} }
    public double QuantityOrdered { get { return _QuantityOrdered;}  set { _QuantityOrdered = value;} }
    public DateTime RequiredByDate { get { return _RequiredByDate;}  set { _RequiredByDate = value;} }
    public int PrepTypeID { get { return _PrepTypeID;}  set { _PrepTypeID = value;} }
    public int PackagingID { get { return _PackagingID;}  set { _PackagingID = value;} }
    public int ToBeDeliveredBy { get { return _ToBeDeliveredBy;}  set { _ToBeDeliveredBy = value;} }
    public bool Confirmed { get { return _Confirmed;}  set { _Confirmed = value;} }
    public bool Done { get { return _Done;}  set { _Done = value;} }
    public bool Packed { get { return _Packed;}  set { _Packed = value;} }
    public string Notes { get { return _Notes;}  set { _Notes = value;} }
  }
}
