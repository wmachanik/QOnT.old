using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QOnT.classes;


/*    PageLogic
Customer Reminder:

Create class of items to be displayed

First the recoccuring table 
1. For each customerid and itemtypeid in the ReoccuringTbl set the at least DateLastDone to the date the Item of of the servicetype was supplied

SELECT        CustomerID, ItemRequiredID, MAX(LastDate) AS LastDatePerItem
FROM            (SELECT        ReoccuringOrderTbl.CustomerID, ReoccuringOrderTbl.ItemRequiredID, ClientUsageLinesTbl.[Date] AS LastDate
                          FROM            ((ClientUsageLinesTbl INNER JOIN
                                                    ReoccuringOrderTbl ON ClientUsageLinesTbl.CustomerID = ReoccuringOrderTbl.CustomerID AND 
                                                    ClientUsageLinesTbl.[Date] > ReoccuringOrderTbl.DateLastDone) INNER JOIN
                                                    ItemTypeTbl ON ReoccuringOrderTbl.ItemRequiredID = ItemTypeTbl.ItemTypeID)) LastDateTbl
GROUP BY CustomerID, ItemRequiredID

FOR EACH CustomerID and ItemRequired found update the Date to LastDatePerItem if the Date is < LastDatePerItem


2. For each enabled record in the ReoccuringTbl:

if the LastDatePerItem + DateAdd(ReocurranceType) < NextCityDeliveryDate then add them into the temporary reminder table, but set the value in the tempoary reminder table to remember that it is a reoccuring and an auto fulfil.

3. Then for all the other clients not in the temp table, and have not received reminders today (see below) and whose nextrequireddate for each serviceitem is < NextCityDeliveryDate add to temp table

4. Dispay form and templ table

5. Wait for user to enter body header then click send.

On send for each client:

Send email explaining why they are getting they email and wether they are a reocuring client, auto fulfill client or etc. Explain bulk roasting and top up roasting. Add a link to the bottom of the email to allowing us to add the order by clicking the link, or delay.

- if autofulful add to order table,and add comment append comment "reoccuring" if also a reoccuring client




For each client that receives a reminder:
1.  add last remidner date to the customer table, to make sure we do not resend
2. Increment the reminder count.
3. for all reminder counts >= 5 set the next reminder date to today + 14 days. 
   */

namespace QOnT.Pages
{
  class ContactToRemind
  {
#region  InternalVars
    private long _CustomerID;
    private string _CompanyName;
    private string _ContactFirstName;
    private string _ContactAltFirstName;
    private int _City;
    private string _EmailAddress;
    private string _AltEmailAddress;
    private int _CustomerTypeID;
    private int _EquipType;
    private bool _TypicallySecToo;
    private int _PreferedAgent;
    private int _SalesAgentID;
    private bool _UsesFilter;
    private bool _enabled;
    private bool _AlwaysSendChkUp;
    private int _ReminderCount;
#endregion 

#region classdefinition

    // class definition
    public ContactToRemind()
    {
      _CustomerID = 0;
      _CompanyName = string.Empty;
      _ContactFirstName = string.Empty;
      _ContactAltFirstName = string.Empty;
      _City = 0;
      _EmailAddress = string.Empty;
      _AltEmailAddress = string.Empty;
      _CustomerTypeID = 0;
      _EquipType = 0;
      _TypicallySecToo = false;
      _PreferedAgent = 0;
      _SalesAgentID = 0;
      _UsesFilter = false;
      //_AutoFulfill = _ReoccurOrder = false;
      _enabled = false;
      _AlwaysSendChkUp = false;
      _ReminderCount = 0;
    }

#endregion

#region public vars
    // get and sets of public
    public long CustomerID { get { return _CustomerID;}  set { _CustomerID = value;} }
    public string CompanyName { get { return _CompanyName;}  set { _CompanyName = value;} }
    public string ContactFirstName { get { return _ContactFirstName;}  set { _ContactFirstName = value;} }    public string ContactAltFirstName { get { return _ContactAltFirstName;}  set { _ContactAltFirstName = value;} }
    public int City { get { return _City;}  set { _City = value;} }
    public string EmailAddress { get { return _EmailAddress;}  set { _EmailAddress = value;} }
    public string AltEmailAddress { get { return _AltEmailAddress;}  set { _AltEmailAddress = value;} }
    public int CustomerTypeID { get { return _CustomerTypeID;}  set { _CustomerTypeID = value;} }
    public int EquipType { get { return _EquipType;}  set { _EquipType = value;} }
    public bool TypicallySecToo { get { return _TypicallySecToo;}  set { _TypicallySecToo = value;} }
    public int PreferedAgent { get { return _PreferedAgent;}  set { _PreferedAgent = value;} }
    public int SalesAgentID { get { return _SalesAgentID;}  set { _SalesAgentID = value;} }
    public bool UsesFilter { get { return _UsesFilter;}  set { _UsesFilter = value;} }
    public bool enabled { get { return _enabled; } set { _enabled = value; } }
    public bool AlwaysSendChkUp { get { return _AlwaysSendChkUp;}  set { _AlwaysSendChkUp = value;} }
    public int ReminderCount { get { return _ReminderCount;}  set { _ReminderCount = value;} }
#endregion

  }
  // line items
  class ItemContactRequires
  {
#region InternalItems
    int _ItemID;
    double _ItemQty;
    int _ItemPrepID;
    int _ItemPackagID;
    private bool _AutoFulfill;
    private bool _ReoccurOrder;
    DateTime _NextDateRequired;
#endregion

#region ClassInit
    public ItemContactRequires()
    {
      _ItemID = 0;
      _ItemQty = 0.0;
      _ItemPrepID = _ItemPackagID = 0;
      _AutoFulfill = _ReoccurOrder = false;
      _NextDateRequired = DateTime.Now;
    }
#endregion

#region PublicItems
    public int ItemID { get { return _ItemID;}  set { _ItemID = value;} }
    public double ItemQty { get { return _ItemQty;}  set { _ItemQty = value;} }
    public int ItemPrepID { get { return _ItemPrepID; } set { _ItemPrepID = value; } }
    public int ItemPackagID { get { return _ItemPackagID; } set { _ItemPackagID = value; } }
    public bool AutoFulfill { get { return _AutoFulfill; } set { _AutoFulfill = value; } }
    public bool ReoccurOrder { get { return _ReoccurOrder; } set { _ReoccurOrder = value; } }
#endregion  
  }


  public partial class SendCoffeeCheckup : System.Web.UI.Page
  {

    private List<ContactToRemind> GetReocurringContacts()
    {
      List<ContactToRemind> _ContactsToRemind = new List<ContactToRemind>();
      List<ItemContactRequires> ItemsContactRequires = new List<ItemContactRequires>();

      // make sure the dates in the Reoccuring table match the last delivery date
      QOnT.control.ReoccuringOrderTbl _ReoccuringOrder = new control.ReoccuringOrderTbl();
      _ReoccuringOrder.SetReoccuringItemsLastDate();

      // now make sure that only records the are enbabled and need to be added from Reoccuring Are added 
      List <control.ReoccuringOrderTbl> _ReoccuringOrders = _ReoccuringOrder.GetAll(true);
      control.CustomersTbl _Customer = new control.CustomersTbl();

      
      for (int i = 0; i < _ReoccuringOrders.Count; i++)
      {

        // if DateNext < NextDeliveryDate for client
        ItemContactRequires _ItemRequired = new ItemContactRequires();

 
/* !!!!!!!!!!!!!!!!!!!!!!!! 
  
  need to add this logic
  
 *  need to see if they need this item and then add a record if required marking as a reocurring item
*/

/* !!!!!!!!!!!!11
 * 
 *  If CustomerID exists then add to item list otherwise add to list
 *  
 */
        ContactToRemind _ContactToRemind = new ContactToRemind();

        _Customer = _Customer.GetCustomersByCustomerID(_ReoccuringOrder.CustomerID);
        _ContactToRemind.CustomerID = _ReoccuringOrder.CustomerID;
        _ContactToRemind.CompanyName = _Customer.CompanyName;
        _ContactToRemind.ContactFirstName = _Customer.ContactFirstName;
        _ContactToRemind.ContactAltFirstName = _Customer.ContactAltFirstName;
        _ContactToRemind.EmailAddress = _Customer.EmailAddress;
        _ContactToRemind.AltEmailAddress = _Customer.AltEmailAddress;
        _ContactToRemind.City = _Customer.City;
        _ContactToRemind.CustomerTypeID = _Customer.CustomerTypeID;
        _ContactToRemind.enabled = _Customer.enabled;
        _ContactToRemind.EquipType = _Customer.EquipType;
        _ContactToRemind.TypicallySecToo = _Customer.TypicallySecToo;
        _ContactToRemind.PreferedAgent = _Customer.PreferedAgent;
        _ContactToRemind.SalesAgentID = _Customer.SalesAgentID;
        _ContactToRemind.UsesFilter = _Customer.UsesFilter;
        _ContactToRemind.enabled = _Customer.enabled;
        _ContactToRemind.AlwaysSendChkUp = _Customer.AlwaysSendChkUp;
        _ContactToRemind.ReminderCount = _Customer.ReminderCount;

// now add the reocurring item
        ItemContactRequires _ItemContactRequires = new ItemContactRequires();

        ItemsContactRequires.Add(_ItemContactRequires);

        
      }

      return _ContactsToRemind;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
       // load the data into the grid view
        QOnT.classes.TrackerTools _TTools = new TrackerTools();
        if (!_TTools.IsNextRoastDateByCityTodays())
          _TTools.SetNextRoastDateByCity();           // The next roast days have not been calcualted, calculate them and save. When customers city is found save the details

        List<ContactToRemind> _ContactsToRemind = GetReocurringContacts();


        
        
        //QOnT.control.ClientsToReceiveReminderQry _ClientsControl = new control.ClientsToReceiveReminderQry();
        //List<control.ClientsToReceiveReminderQry> _ClientsToReceiveReminder = _ClientsControl.GetCustomersToReveiveReminder("NextCoffeeBy");
        //gvCustomerCheckup.DataSource = _ClientsToReceiveReminder;
        //gvCustomerCheckup.DataBind();

      }

    }

    /*
     * 
     * QOnT.classes.showMessageBox _showMessageBox = new QOnT.classes.showMessageBox(this.Page, "Test", "Is this showm"); 
     */
  }
}