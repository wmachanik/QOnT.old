/// --- auto generated class for table: ReoccuringOrderTbl
using System;   // for DateTime variables

namespace QOnT.classes
{
  public class ReoccuringOrderTblData
  {
    // internal variable declarations
    private int _ID;
    private int _CustomerID;
    private int _ReoccuranceType;
    private byte _Value;
    private int _ItemRequiredID;
    private double _QtyRequired;
    private DateTime _DateLastDone;
    private DateTime _NextDateRequired;
    private DateTime _RequireUntilDate;
    private bool _Enabled;
    private string _Notes;
    // class definition
    public ReoccuringOrderTblData()
    {
      _ID = 0;
      _CustomerID = 0;
      _ReoccuranceType = 0;
      _Value = 0;
      _ItemRequiredID = 0;
      _QtyRequired = 0.0;
      _DateLastDone = System.DateTime.Now;
      _NextDateRequired = System.DateTime.Now;
      _RequireUntilDate = System.DateTime.Now;
      _Enabled = false;
      _Notes = string.Empty;
    }
    // get and sets of public
    public int ID { get { return _ID;}  set { _ID = value;} }
    public int CustomerID { get { return _CustomerID;}  set { _CustomerID = value;} }
    public int ReoccuranceType { get { return _ReoccuranceType;}  set { _ReoccuranceType = value;} }
    public byte Value { get { return _Value;}  set { _Value = value;} }
    public int ItemRequiredID { get { return _ItemRequiredID;}  set { _ItemRequiredID = value;} }
    public double QtyRequired { get { return _QtyRequired;}  set { _QtyRequired = value;} }
    public DateTime DateLastDone { get { return _DateLastDone;}  set { _DateLastDone = value;} }
    public DateTime NextDateRequired { get { return _NextDateRequired;}  set { _NextDateRequired = value;} }
    public DateTime RequireUntilDate { get { return _RequireUntilDate;}  set { _RequireUntilDate = value;} }
    public bool Enabled { get { return _Enabled;}  set { _Enabled = value;} }
    public string Notes { get { return _Notes;}  set { _Notes = value;} }
  }
}
