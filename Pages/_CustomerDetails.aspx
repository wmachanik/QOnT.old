<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
  CodeBehind="CustomerDetails.aspx.cs" Inherits="QOnT.Pages.CustomerDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cntCustomerDetailsHdr" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="cntCustomerDetailsBdy" ContentPlaceHolderID="MainContent" runat="server">
  <h2 class="InputFrm">Customer Details</h2>
  <asp:Label ID="lblCustomerID" Visible="false" runat="server" />
  <asp:ScriptManager ID="smCustomerDetails" runat="server" />
  <asp:UpdateProgress ID="uprgCustomerDetails" runat="server">
    <ProgressTemplate>
      <img src="../images/animi/BlueArrowsUpdate.gif" alt="updating" width="16" height="16" />updating.....
    </ProgressTemplate>
  </asp:UpdateProgress>
  <asp:UpdatePanel ID="upnlCustomerDetails" runat="server">
    <ContentTemplate>
      <asp:DetailsView ID="dvCustomerDetails" runat="server" AllowPaging="True" 
        BackColor="White" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px"
        CellPadding="4" DataSourceID="odsCustomerDetails" ForeColor="Black"
        OnModeChanging="dvCustomerDetails_OnModeChanging" AutoGenerateRows="False">
        <AlternatingRowStyle BackColor="White" />
        <EditRowStyle BackColor="#5DCE5A" Font-Bold="True" ForeColor="White" />
        <Fields>
          <asp:CommandField ButtonType="Image" 
            CancelImageUrl="~/images/imgButtons/CancelItem.gif" DeleteImageUrl="~/images/imgButtons/DelItem.gif" 
            EditImageUrl="~/images/imgButtons/EditItem.gif" InsertImageUrl="~/images/imgButtons/AddItem.gif" 
            NewImageUrl="~/images/imgButtons/GreenPlus.gif" SelectImageUrl="~/images/imgButtons/SelectItem.gif" 
            ShowCancelButton="False" ShowEditButton="True" ShowInsertButton="True" 
            UpdateImageUrl="~/images/imgButtons/UpdateItem.gif">
          <HeaderStyle HorizontalAlign="Center" />
          <ItemStyle HorizontalAlign="Center" />
          </asp:CommandField>
          <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" 
            InsertVisible="False" SortExpression="CustomerID" Visible="False" />
          <asp:BoundField DataField="CompanyName" HeaderText="CompanyName" 
            SortExpression="CompanyName" />
          <asp:BoundField DataField="ContactTitle" HeaderText="ContactTitle" 
            SortExpression="ContactTitle" />
          <asp:BoundField DataField="ContactFirstName" HeaderText="ContactFirstName" 
            SortExpression="ContactFirstName" />
          <asp:BoundField DataField="ContactLastName" HeaderText="ContactLastName" 
            SortExpression="ContactLastName" />
          <asp:BoundField DataField="ContactAltFirstName" 
            HeaderText="ContactAltFirstName" SortExpression="ContactAltFirstName" />
          <asp:BoundField DataField="ContactAltLastName" HeaderText="ContactAltLastName" 
            SortExpression="ContactAltLastName" />
          <asp:BoundField DataField="Department" HeaderText="Department" 
            SortExpression="Department" />
          <asp:BoundField DataField="BillingAddress" HeaderText="BillingAddress" 
            SortExpression="BillingAddress" />
          <asp:TemplateField HeaderText="CityId" SortExpression="CityId">
            <EditItemTemplate>
              <asp:DropDownList ID="ddlCity" runat="server" DataSourceID="odsCity" 
                DataTextField="City" DataValueField="ID" SelectedValue='<%# Bind("CityId") %>'>
              </asp:DropDownList>
            </EditItemTemplate>
            <InsertItemTemplate>
              <asp:DropDownList ID="ddlCity" runat="server" DataSourceID="odsCity" 
                DataTextField="City" DataValueField="ID" SelectedValue='<%# Bind("CityId") %>'>
              </asp:DropDownList>
            </InsertItemTemplate>
            <ItemTemplate>
              <asp:Label ID="lblCityName" runat="server" Text='<%# Bind("CityId") %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:BoundField DataField="StateOrProvince" HeaderText="StateOrProvince" 
            SortExpression="StateOrProvince" />
          <asp:BoundField DataField="PostalCode" HeaderText="PostalCode" 
            SortExpression="PostalCode" />
          <asp:BoundField DataField="Region" HeaderText="Region" 
            SortExpression="Region" />
          <asp:BoundField DataField="PhoneNumber" HeaderText="PhoneNumber" 
            SortExpression="PhoneNumber" />
          <asp:BoundField DataField="Extension" HeaderText="Extension" 
            SortExpression="Extension" />
          <asp:BoundField DataField="FaxNumber" HeaderText="FaxNumber" 
            SortExpression="FaxNumber" />
          <asp:BoundField DataField="CellNumber" HeaderText="CellNumber" 
            SortExpression="CellNumber" />
          <asp:BoundField DataField="EmailAddress" HeaderText="EmailAddress" 
            SortExpression="EmailAddress" />
          <asp:BoundField DataField="AltEmailAddress" HeaderText="AltEmailAddress" 
            SortExpression="AltEmailAddress" />
          <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" 
            SortExpression="ContractNo" />
          <asp:TemplateField HeaderText="CustomerType" SortExpression="CustomerType">
            <EditItemTemplate>
              <asp:DropDownList ID="ddlCustomerType" runat="server" 
                DataSourceID="odsCustomerTypes" DataTextField="CustomerType" 
                DataValueField="CustomerType" SelectedValue='<%# Bind("CustomerType") %>'>
              </asp:DropDownList>
            </EditItemTemplate>
            <InsertItemTemplate>
              <asp:DropDownList ID="ddlCustomerType" runat="server" 
                DataSourceID="odsCustomerTypes" DataTextField="CustomerType" 
                DataValueField="CustomerType" SelectedValue='<%# Bind("CustomerType") %>'>
              </asp:DropDownList>
            </InsertItemTemplate>
            <ItemTemplate>
              <asp:Label ID="lblCustomerType" runat="server" 
                Text='<%# Bind("CustomerType") %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="EquipTypeId" SortExpression="EquipTypeId">
            <EditItemTemplate>
              <asp:DropDownList ID="ddlEquipTypes" runat="server" DataSourceID="odsEquipType" 
                DataTextField="EquipTypeName" DataValueField="EquipTypeId" SelectedValue='<%# Bind("EquipTypeId") %>'>
              </asp:DropDownList>
            </EditItemTemplate>
            <InsertItemTemplate>
              <asp:DropDownList ID="ddlEquipTypes" runat="server" DataSourceID="odsEquipType" 
                DataTextField="EquipTypeName" DataValueField="EquipTypeId" SelectedValue='<%# Bind("EquipTypeId") %>'>
              </asp:DropDownList>
            </InsertItemTemplate>
            <ItemTemplate>
              <asp:Label ID="lblEquipType" runat="server" Text='<%# Bind("EquipTypeId") %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="PrimaryItemId" SortExpression="PrimaryItemId">
            <EditItemTemplate>
              <asp:DropDownList ID="ddlPrimaryItems" runat="server" DataSourceID="odsItemTypes" DataTextField="ItemDesc" 
                DataValueField="ItemTypeID" SelectedValue='<%# Bind("PrimaryItemId") %>'>
              </asp:DropDownList>
            </EditItemTemplate>
            <InsertItemTemplate>
              <asp:DropDownList ID="ddlPrimaryItems" runat="server" DataSourceID="odsItemTypes" DataTextField="ItemDesc" 
                DataValueField="ItemTypeID" SelectedValue='<%# Bind("PrimaryItemId") %>'>
              </asp:DropDownList>
            </InsertItemTemplate>
            <ItemTemplate>
              <asp:Label ID="lblPrimaryItem" runat="server" 
                Text='<%# Bind("PrimaryItemId") %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:BoundField DataField="PriPrefQty" HeaderText="PriPrefQty" 
            SortExpression="PriPrefQty" />
          <asp:TemplateField HeaderText="SecondaryItemId" 
            SortExpression="SecondaryItemId">
            <EditItemTemplate>
              <asp:DropDownList ID="ddlSecondaryItem" runat="server" 
                DataSourceID="odsItemTypes" DataTextField="ItemDesc" 
                DataValueField="ItemTypeID" SelectedValue='<%# Bind("SecondaryItemId") %>'>
              </asp:DropDownList>
            </EditItemTemplate>
            <InsertItemTemplate>
              <asp:DropDownList ID="ddlSecondaryItem" runat="server" 
                DataSourceID="odsItemTypes" DataTextField="ItemDesc" 
                DataValueField="ItemTypeID" SelectedValue='<%# Bind("SecondaryItemId") %>'>
              </asp:DropDownList>
            </InsertItemTemplate>
            <ItemTemplate>
              <asp:Label ID="lblSecondaryItem" runat="server" 
                Text='<%# Bind("SecondaryItemId") %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:BoundField DataField="SecPrefQty" HeaderText="SecPrefQty" 
            SortExpression="SecPrefQty" />
          <asp:CheckBoxField DataField="TypicallySecToo" HeaderText="TypicallySecToo" 
            SortExpression="TypicallySecToo" />
          <asp:TemplateField HeaderText="DeliveryById" SortExpression="DeliveryById">
            <EditItemTemplate>
              <asp:DropDownList ID="ddlDeliveryPerson" runat="server" 
                DataSourceID="odsPeopleList" DataTextField="Person" DataValueField="PersonID" 
                SelectedValue='<%# Bind("DeliveryById") %>'>
              </asp:DropDownList>
            </EditItemTemplate>
            <InsertItemTemplate>
              <asp:DropDownList ID="ddlDeliveryPerson" runat="server" 
                DataSourceID="odsPeopleList" DataTextField="Person" DataValueField="PersonID" 
                SelectedValue='<%# Bind("DeliveryById") %>'>
              </asp:DropDownList>
            </InsertItemTemplate>
            <ItemTemplate>
              <asp:Label ID="lblDeliveryBy" runat="server" Text='<%# Bind("DeliveryById") %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="SalesAgentID" SortExpression="SalesAgentID">
            <EditItemTemplate>
              <asp:DropDownList ID="ddlSalesAgent" runat="server" 
                DataSourceID="odsPeopleList" DataTextField="Person" DataValueField="PersonID" 
                SelectedValue='<%# Bind("SalesAgentID") %>'>
              </asp:DropDownList>
            </EditItemTemplate>
            <InsertItemTemplate>
              <asp:DropDownList ID="ddlSalesAgent" runat="server" 
                DataSourceID="odsPeopleList" DataTextField="Person" DataValueField="PersonID" 
                SelectedValue='<%# Bind("SalesAgentID") %>'>
              </asp:DropDownList>
            </InsertItemTemplate>
            <ItemTemplate>
              <asp:Label ID="lblSalesAgent" runat="server" Text='<%# Bind("SalesAgentID") %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:BoundField DataField="MachineSN" HeaderText="MachineSN" 
            SortExpression="MachineSN" />
          <asp:CheckBoxField DataField="UsesFilter" HeaderText="UsesFilter" 
            SortExpression="UsesFilter" />
          <asp:CheckBoxField DataField="autofulfill" HeaderText="autofulfill" 
            SortExpression="autofulfill" />
          <asp:CheckBoxField DataField="enabled" HeaderText="enabled" 
            SortExpression="enabled" />
          <asp:CheckBoxField DataField="PredictionDisabled" 
            HeaderText="PredictionDisabled" SortExpression="PredictionDisabled" />
          <asp:CheckBoxField DataField="AlwaysSendChkUp" HeaderText="AlwaysSendChkUp" 
            SortExpression="AlwaysSendChkUp" />
          <asp:CheckBoxField DataField="NormallyResponds" HeaderText="NormallyResponds" 
            SortExpression="NormallyResponds" />
          <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
          <asp:CommandField ShowEditButton="True" ShowInsertButton="True" ButtonType="Button" />
        </Fields>
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <PagerSettings FirstPageImageUrl="~/images/imgButtons/FirstPage.gif" 
          LastPageImageUrl="~/images/imgButtons/LastPage.gif" Mode="NumericFirstLast" 
          NextPageImageUrl="~/images/imgButtons/NextPage.gif" 
          PreviousPageImageUrl="~/images/imgButtons/PrevPage.gif" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="#F7F7DE" />
      </asp:DetailsView>
    </ContentTemplate>
  </asp:UpdatePanel>
  <asp:ObjectDataSource ID="odsCustomerDetails" runat="server" OldValuesParameterFormatString="original_{0}"
    SelectMethod="GetByCustId" TypeName="QOnT.DataSets.CustomersDataSetTableAdapters.CustomerDetailsTableAdapter"
    InsertMethod="InsertQuery" UpdateMethod="UpdateQuery" OnUpdating="odsCustomerDetails_OnUpdating" >
    <InsertParameters>
      <asp:Parameter Name="CompanyName" Type="String" />
      <asp:Parameter Name="ContactTitle" Type="String" />
      <asp:Parameter Name="ContactFirstName" Type="String" />
      <asp:Parameter Name="ContactLastName" Type="String" />
      <asp:Parameter Name="ContactAltFirstName" Type="String" />
      <asp:Parameter Name="ContactAltLastName" Type="String" />
      <asp:Parameter Name="Department" Type="String" />
      <asp:Parameter Name="BillingAddress" Type="String" />
      <asp:Parameter Name="CityID" Type="Int32" />
      <asp:Parameter Name="StateOrProvince" Type="String" />
      <asp:Parameter Name="PostalCode" Type="String" />
      <asp:Parameter Name="Country_Region" Type="String" />
      <asp:Parameter Name="PhoneNumber" Type="String" />
      <asp:Parameter Name="Extension" Type="String" />
      <asp:Parameter Name="FaxNumber" Type="String" />
      <asp:Parameter Name="CellNumber" Type="String" />
      <asp:Parameter Name="EmailAddress" Type="String" />
      <asp:Parameter Name="AltEmailAddress" Type="String" />
      <asp:Parameter Name="ContractNo" Type="String" />
      <asp:Parameter Name="CustomerType" Type="String" />
      <asp:Parameter Name="EquipTypeId" Type="Int32" />
      <asp:Parameter Name="PrimaryItemId" Type="Int32" />
      <asp:Parameter Name="PriPrefQty" Type="Decimal" />
      <asp:Parameter Name="SecondaryItemId" Type="Int32" />
      <asp:Parameter Name="SecPrefQty" Type="Decimal" />
      <asp:Parameter Name="TypicallySecToo" Type="Boolean" />
      <asp:Parameter Name="DeliveryById" Type="Int32" />
      <asp:Parameter Name="SalesAgentID" Type="Int32" />
      <asp:Parameter Name="MachineSN" Type="String" />
      <asp:Parameter Name="UsesFilter" Type="Boolean" />
      <asp:Parameter Name="autofulfill" Type="Boolean" />
      <asp:Parameter Name="enabled" Type="Boolean" />
      <asp:Parameter Name="PredictionDisabled" Type="Boolean" />
      <asp:Parameter Name="AlwaysSendChkUp" Type="Boolean" />
      <asp:Parameter Name="NormallyResponds" Type="Boolean" />
      <asp:Parameter Name="Notes" Type="String" />
    </InsertParameters>
    <SelectParameters>
      <asp:ControlParameter ControlID="lblCustomerID" DefaultValue="1" Name="CustomerID"
        PropertyName="Text" Type="Int32" />
    </SelectParameters>
    <UpdateParameters>
      <asp:Parameter Name="CompanyName" Type="String" />
      <asp:Parameter Name="ContactTitle" Type="String" />
      <asp:Parameter Name="ContactFirstName" Type="String" />
      <asp:Parameter Name="ContactLastName" Type="String" />
      <asp:Parameter Name="ContactAltFirstName" Type="String" />
      <asp:Parameter Name="ContactAltLastName" Type="String" />
      <asp:Parameter Name="Department" Type="String" />
      <asp:Parameter Name="BillingAddress" Type="String" />
      <asp:Parameter Name="CityId" Type="Int32" />
      <asp:Parameter Name="StateOrProvince" Type="String" />
      <asp:Parameter Name="PostalCode" Type="String" />
      <asp:Parameter Name="Country_Region" Type="String" />
      <asp:Parameter Name="PhoneNumber" Type="String" />
      <asp:Parameter Name="Extension" Type="String" />
      <asp:Parameter Name="FaxNumber" Type="String" />
      <asp:Parameter Name="CellNumber" Type="String" />
      <asp:Parameter Name="EmailAddress" Type="String" />
      <asp:Parameter Name="AltEmailAddress" Type="String" />
      <asp:Parameter Name="ContractNo" Type="String" />
      <asp:Parameter Name="CustomerType" Type="String" />
      <asp:Parameter Name="EquipTypeId" Type="Int32" />
      <asp:Parameter Name="PrimaryItemId" Type="Int32" />
      <asp:Parameter Name="PriPrefQty" Type="Decimal" />
      <asp:Parameter Name="SecondaryItemId" Type="Int32" />
      <asp:Parameter Name="SecPrefQty" Type="Decimal" />
      <asp:Parameter Name="TypicallySecToo" Type="Boolean" />
      <asp:Parameter Name="DeliveryById" Type="Int32" />
      <asp:Parameter Name="SalesAgentID" Type="Int32" />
      <asp:Parameter Name="MachineSN" Type="String" />
      <asp:Parameter Name="UsesFilter" Type="Boolean" />
      <asp:Parameter Name="autofulfill" Type="Boolean" />
      <asp:Parameter Name="enabled" Type="Boolean" />
      <asp:Parameter Name="PredictionDisabled" Type="Boolean" />
      <asp:Parameter Name="AlwaysSendChkUp" Type="Boolean" />
      <asp:Parameter Name="NormallyResponds" Type="Boolean" />
      <asp:Parameter Name="Notes" Type="String" />
      <asp:Parameter Name="Original_CustomerID" Type="Int32" />
    </UpdateParameters>
  </asp:ObjectDataSource>
  <asp:ObjectDataSource ID="odsPeopleList" runat="server" OldValuesParameterFormatString="original_{0}"
    SelectMethod="GetPeople" 
    TypeName="QOnT.DataSets.LookUpDatSetsTableAdapters.PersonsLkupTableAdapter">
  </asp:ObjectDataSource>
  <asp:ObjectDataSource ID="odsCustomerTypes" runat="server" OldValuesParameterFormatString="original_{0}"
    SelectMethod="GetCustomerTypes" TypeName="QOnT.DataSets.LookUpDatSetsTableAdapters.CustomerTypesTblTableAdapter">
  </asp:ObjectDataSource>
  <asp:ObjectDataSource ID="odsEquipType" runat="server" OldValuesParameterFormatString="original_{0}"
    SelectMethod="GetData" TypeName="QOnT.DataSets.LookUpDatSetsTableAdapters.EquipTypeTblTableAdapter">
  </asp:ObjectDataSource>
  <asp:ObjectDataSource ID="odsCity" runat="server" OldValuesParameterFormatString="original_{0}"
    SelectMethod="GetData" TypeName="QOnT.DataSets.LookUpDatSetsTableAdapters.CityTblTableAdapter">
  </asp:ObjectDataSource>
  <asp:ObjectDataSource ID="odsItemTypes" runat="server" OldValuesParameterFormatString="original_{0}"
    SelectMethod="GetItems" TypeName="QOnT.DataSets.LookUpDatSetsTableAdapters.ItemTypeLkupTableAdapter">
  </asp:ObjectDataSource>
</asp:Content>
<%----%>