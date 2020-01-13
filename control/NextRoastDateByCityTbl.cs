using System;
using System.Collections.Generic;
using System.Web;

namespace QOnT.control
{

  public class NextRoastDateByCityTbl
  {

    private long _CityID;
    private DateTime _dtPrep;
    private int _DeliveryDelayDays;
    private int _DeliveryOrder;

    public NextRoastDateByCityTbl() {
      _CityID = 0;
      _DeliveryDelayDays = _DeliveryOrder = 0;
      _dtPrep = DateTime.Now.Date;
    }

    public long CityID
    {
      get { return _CityID; }
      set { _CityID = value;  }
    }

    public DateTime dtPrep
    {
      get { return _dtPrep.Date; }
      set { _dtPrep = value; }
    }

    public int DeliveryDelayDays
    {
      get { return _DeliveryDelayDays; }
      set { _DeliveryDelayDays = value; }
    }

    public int DeliveryOrder
    {
      get { return _DeliveryOrder; }
      set { _DeliveryOrder = value; }
    }

  }
}