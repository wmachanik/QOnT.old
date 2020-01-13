/// --- auto generated class for table: CustomerTypeTbl
using System;   // for DateTime variables

namespace QOnT.classes
{
  public class CustomerTypeTblData
  {
    // internal variable declarations
    private int _CustTypeID;
    private string _CustTypeDesc;
    private string _Notes;
    // class definition
    public CustomerTypeTblData()
    {
      _CustTypeID = 0;
      _CustTypeDesc = string.Empty;
      _Notes = string.Empty;
    }
    // get and sets of public
    public int CustTypeID { get { return _CustTypeID;}  set { _CustTypeID = value;} }
    public string CustTypeDesc { get { return _CustTypeDesc;}  set { _CustTypeDesc = value;} }
    public string Notes { get { return _Notes;}  set { _Notes = value;} }
  }
}
