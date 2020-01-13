using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using QOnT.classes;


namespace QOnT.Pages
{
  public partial class Lookups : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
        tabcLookup.ActiveTabIndex = 0;  // items
      //{
      //  MembershipUser _usr = Membership.GetUser();
      //  if ((_usr.UserName == "warren") || (_usr.UserName == "admin") 
      //  {
      //    //        
      //  }
      //}

    }

    protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      if (e.CommandName.Equals("AddItem"))
      {
        try
        {
          TextBox tbxItem = (TextBox)gvItems.FooterRow.FindControl("tbxItem");
          CheckBox cbxItemEnabled = (CheckBox)gvItems.FooterRow.FindControl("cbxItemEnabled");
          TextBox tbxItemCharacteristics = (TextBox)gvItems.FooterRow.FindControl("tbxItemCharacteristics");
          TextBox tbxItemDetail = (TextBox)gvItems.FooterRow.FindControl("tbxItemDetail");
          DropDownList ddlServiceType = (DropDownList)gvItems.FooterRow.FindControl("ddlServiceType");
          DropDownList ddlReplacement = (DropDownList)gvItems.FooterRow.FindControl("ddlReplacement");
          TextBox tbxItemShortName = (TextBox)gvItems.FooterRow.FindControl("tbxItemShortName");
          TextBox tbxSortOrder = (TextBox)gvItems.FooterRow.FindControl("tbxSortOrder");

          // set values depending on if the item is null
          // "INSERT INTO [ItemTypeTbl] ([ItemDesc], [ItemEnabled], [ItemsCharacteritics], [ItemDetail], [ServiceTypeID], [ReplacementID], [ItemShortName], [SortOrder]) VALUES (?, ?, ?, ?, ?, ?, ?, ?)"
          sdsItems.InsertParameters.Clear();
          sdsItems.InsertParameters.Add("ItemDec", System.Data.DbType.String, tbxItem.Text);
          sdsItems.InsertParameters.Add("ItemEnabled", System.Data.DbType.Boolean, cbxItemEnabled.Enabled.ToString());
          sdsItems.InsertParameters.Add("ItemCharacteristics", System.Data.DbType.String, tbxItemCharacteristics.Text);
          sdsItems.InsertParameters.Add("ItemDetail", System.Data.DbType.String, tbxItemDetail.Text);
          sdsItems.InsertParameters.Add("ServiceTypeID", System.Data.DbType.Int32, ddlServiceType.SelectedValue.ToString());
          sdsItems.InsertParameters.Add("ReplacementID", System.Data.DbType.Int32, ddlReplacement.SelectedValue.ToString());   // none
          sdsItems.InsertParameters.Add("ItemShortName", System.Data.DbType.String, tbxItemShortName.Text);
          sdsItems.InsertParameters.Add("SortOrder", System.Data.DbType.Int32, tbxSortOrder.Text);  // roaster coffee
          sdsItems.Insert();

//          gvItems.DataSource = sdsItems;
          gvItems.DataBind();
        }
        catch (Exception ex)
        {
          lblStatus.Text = "Error adding record: " + ex.Message;
        }


        // from http://www.aspdotnetcodes.com/GridView_Insert_Edit_Update_Delete.aspx
      }
    }
    protected void dvItems_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
      gvItems.FooterRow.Enabled = false;
      gvItems.DataBind();

//      gvItems.DataSourceID = "sdsItems";
//      gvItems.DataBind();
    }

    protected void InsertItemButton_Click(object sender, EventArgs e)
    {
      gvItems.FooterRow.Enabled = true;
      gvItems.DataBind();

//      gvItems.DataSourceID = "";
//      gvItems.DataBind();

      // "INSERT INTO [ItemTypeTbl] ([ItemDesc], [ItemEnabled], [ItemsCharacteritics], [ItemDetail], [ServiceTypeID], [ReplacementID], [ItemShortName], [SortOrder]) VALUES (?, ?, ?, ?, ?, ?, ?, ?)"
      //sdsItems.InsertParameters.Clear();
      //sdsItems.InsertParameters.Add("ItemDec",System.Data.DbType.String,"NewItem");
      //sdsItems.InsertParameters.Add("ItemEnabled", System.Data.DbType.Boolean, "false");
      //sdsItems.InsertParameters.Add("ItemCharacteristics", System.Data.DbType.String, "some notes");
      //sdsItems.InsertParameters.Add("ItemDetail",System.Data.DbType.String,"detailed notes");
      //sdsItems.InsertParameters.Add("ServiceTypeID",System.Data.DbType.Int32,"2");  // coffee
      //sdsItems.InsertParameters.Add("ReplacementID",System.Data.DbType.Int32,"0");   // none
      //sdsItems.InsertParameters.Add("ItemShortName",System.Data.DbType.String,"New");
      //sdsItems.InsertParameters.Add("SortOrder",System.Data.DbType.Int32,"5");  // roaster coffee
      //sdsItems.Insert();
      //gvItems.DataBind();
    }
//    protected void gvEquipment_RowUpdating(object sender, GridViewUpdateEventArgs e)
//    {
//      QOnT.classes.EquipTypeTbl _EquipType = new EquipTypeTbl();
//      _EquipType.EquipTypeId = Convert.ToInt32(((Label)gvEquipment.Rows[e.RowIndex].FindControl("EquipTypeIdLabel")).Text);
//      _EquipType.EquipTypeName = ((TextBox)gvEquipment.Rows[e.RowIndex].FindControl("EquipTypeNameTextBox")).Text;
//      _EquipType.EquipTypeDesc = ((TextBox)gvEquipment.Rows[e.RowIndex].FindControl("EquipTypeDescTextBox")).Text;

////      odsEquipTypes.UpdateParameters.Add("objEquipType", _EquipType);
//      //odsEquipTypes.UpdateParameters.Add("EquipTypeName", _EquipType.EquipTypeName.ToString());
//      //odsEquipTypes.UpdateParameters.Add("EquipTypeDesc", _EquipType.EquipTypeName.ToString());
//      // odsEquipTypes.Update();
//    }
//    protected void gvEquipment_RowCommand(object sender, GridViewCommandEventArgs e)
//    {
//      switch (e.CommandName) 
//      {
//        case "Add" :
//          // Do add stuff
//          break;
//        case "Edit":
//          // Do edit stuff
//          break;
//        case "Update" :
//          int i = 1;
//          i += 1;
//          break;
//        default:
//          break;
//      }
//    }
    protected void gvEquipment_UpdateButton_Click(	EventArgs e)
    {
      //
      Response.Write("Do update");
    }
    protected void sdsCities_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }

  }
}