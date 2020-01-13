
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/**
 * Class created since original missing, should actuall be OrderHeader not Detail from what I can see.
 * **/

namespace QOnT.classes
{
  public class OrderDetails
  {

    public OrderDetails()
    {
      _CustomerID = 0;
      _CompanyName = string.Empty;
      _OrderDate = _RoastDate = _RequiredByDate = DateTime.Now;
      _Confirmed = false;
      _Abreviation = _Notes = string.Empty;
    }

    private long _CustomerID;
    private string _CompanyName;
    private DateTime _OrderDate, _RoastDate, _RequiredByDate;
    private Boolean _Confirmed;
    private string _Abreviation, _Notes;

    public long CustomerID { get { return _CustomerID; } set { _CustomerID = value; } }
    public DateTime OrderDate { get { return _OrderDate; } set { _OrderDate = value; } }
    public DateTime RoastDate { get { return _RoastDate; } set { _RoastDate = value; } }
    public DateTime RequiredByDate { get { return _RequiredByDate; } set { _RequiredByDate = value; } }
    public string CompanyName { get { return _CompanyName; } set { _CompanyName = value; } }
    public Boolean Confirmed { get { return _Confirmed; } set { _Confirmed = value; } }
    public string Abreviation { get { return _Abreviation; } set { _Abreviation = value; } }
    public string Notes { get { return _Notes; } set { _Notes = value; } }

  }
}