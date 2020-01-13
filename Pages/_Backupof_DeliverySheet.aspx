<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="_Backupof_DeliverySheet.aspx.cs" Inherits="QOnT.Pages.DeliverySheet" %>
<asp:Content ID="cntDeliveryHdr" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="cntDeliveryBdy" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlDeliveryDate" runat="server">
    <h1>Delivery Sheet</h1>
    <div class="simpleLightBrownForm">
      &nbsp;Which Delivery Date:&nbsp;
      <asp:DropDownList ID="ddlActiveRoastDates" runat="server" 
        DataSourceID="odsActiveRoastDates" DataTextField="RequiredByDate"  DataTextFormatString="{0:dd-MMM-yyyy (ddd)}"
        DataValueField="RequiredByDate" AppendDataBoundItems="true" 
        onselectedindexchanged="ddlActiveRoastDates_SelectedIndexChanged">
        <asp:ListItem Text="--- Select Date ---"  />
      </asp:DropDownList>&nbsp;&nbsp;<asp:Button ID="btnCreateSheet" 
        Text="Create Sheet" runat="server" onclick="btnCreateSheet_Click" />&nbsp;&nbsp;
       <asp:Button ID="btnRefresh" Text="Refresh" 
        runat="server" ToolTip="Referesh the active delivery days" 
        onclick="btnRefresh_Click" />&nbsp;&nbsp;
      <asp:Label runat="server" ID="lblDeliveryBy" Text="Delivery by: " Visible="false"  />
      <asp:DropDownList ID="ddlDeliveryBy" runat="server" AutoPostBack="true" Visible="false"
          onselectedindexchanged="ddlDeliveryBy_SelectedIndexChanged" />
      <span style="float:right">
        <asp:Button ID="btnPrint" runat="server" Text="Print"  onclick="btnPrint_Click" CssClass="hideWhenPrinting" />
      </span>
      <asp:ObjectDataSource ID="odsActiveRoastDates" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GetActiveDeliveryDates" 
        TypeName="QOnT.DataSets.ActiveDeliveriesDataSetTableAdapters.OrdersTblTableAdapter">
      </asp:ObjectDataSource>   </div >
   <br />
   </asp:Panel>
    <asp:Table ID="tblDeliveries" runat="server" CssClass="TblZebra" Width="100%"  >  
      <asp:TableHeaderRow TableSection="TableHeader" >
        <asp:TableHeaderCell>By</asp:TableHeaderCell>
        <asp:TableHeaderCell>To</asp:TableHeaderCell>
        <asp:TableHeaderCell Width="90px">Received By</asp:TableHeaderCell>
        <asp:TableHeaderCell Width="100px">Signature</asp:TableHeaderCell>
        <asp:TableHeaderCell>Items</asp:TableHeaderCell>
        <asp:TableHeaderCell>In Stock</asp:TableHeaderCell>
      </asp:TableHeaderRow>
    </asp:Table>
  <br />
  <asp:Table ID="tblTotals" runat="server" CssClass="TblCoffee" Width="100%">
  </asp:Table>
  <br />
  <asp:Label ID="ltrlWhichDate" Text="" runat="server" CssClass="hideWhenPrinting" />

</asp:Content>