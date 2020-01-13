using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QOnT.classes;

namespace QOnT.control
{

  public class OrderCls
  {
      private OrderHeaderData _Header;
    private List<QOnT.classes. OrderDetailData> _Items;

    public OrderCls()
    {
      _Header = new OrderHeaderData();
      List<OrderDetailData> _Items = new List<OrderDetailData>();
    }

    public OrderHeaderData HeaderData      
    { 
      get {return _Header; } 
      set {_Header = value; } 
    }
    public List<OrderDetailData> ItemsData 
    {
      get {return _Items; } 
      set {_Items = value; } 
    }
  }

}