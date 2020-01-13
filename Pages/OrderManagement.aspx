<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderManagement.aspx.cs" Inherits="QOnT.Pages.OrderManagement" %>
<asp:Content ID="cntORderManagementHdr" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="cntORderManagementBdy" ContentPlaceHolderID="MainContent" runat="server">

  <asp:FormView ID="fvOrdersMain" runat="server" Height="50px" CssClass="TblWhite" Width="100%">
    <EditItemTemplate>
      Company Name: &nbsp;
      <asp:TextBox runat="server" ID="CustomerNameTextBox" Text='<%# Bind("CompanyName") %>' />  
      &nbsp;
      <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
        CommandName="Update" Text="Update" />
      &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" 
        CausesValidation="False" CommandName="Cancel" Text="Cancel" /><br />
      RoastDate:&nbsp;
      <asp:TextBox ID="RoastDateTextBox" runat="server" 
        Text='<%# Bind("RoastDate") %>' />
      &nbsp;
      <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
        CommandName="Update" Text="Update" />
      &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
        CausesValidation="False" CommandName="Cancel" Text="Cancel" /><br />
    </EditItemTemplate>
    <InsertItemTemplate>

      Company Name: &nbsp;
      <asp:TextBox runat="server" ID="CustomerNameTextBox" Text='<%# Bind("CompanyName") %>' />  
      &nbsp;
      <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
        CommandName="Update" Text="Update" />
        &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
          CausesValidation="False" CommandName="Cancel" Text="Cancel" /><br />
      RoastDate:&nbsp;
      <asp:TextBox ID="RoastDateTextBox" runat="server" 
        Text='<%# Bind("RoastDate") %>' />&nbsp;
      <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" 
        CommandName="Insert" Text="Insert" />
      &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" 
        CausesValidation="False" CommandName="Cancel" Text="Cancel" />
    </InsertItemTemplate>
    <ItemTemplate>
      <asp:Label ID="CustomerIDLabel" runat="server" Visible="false" Text='<%# Bind("CustomerID") %>' />
      <table class="TblWhite" >
        <tr>
          <td>Company Name:</td>
          <td><asp:Label ID="CompanyNameLabel" runat="server" Text='<%# Bind("CompanyName") %>' /></td>
        </tr>
        <tr>
          <td>Order Date:</td>
          <td><asp:Label ID="OrderDateLabel" runat="server" Text='<%# Bind("{0:d}","OrderDate") %>' /></td>
        </tr>
        <tr>
          <td>Roast Date:</td>
          <td><asp:Label ID="RoastDateLabel" runat="server" Text='<%# Bind("RoastDate") %>' /></td>
        </tr>
        <tr>
          <td>Required By Date:</td>
          <td><asp:Label ID="RequiredByDateLabel" runat="server" Text='<%# Bind("RequiredByDate") %>' /></td>
        </tr>
        <tr>
          <td>Delivery By:</td>
          <td><asp:Label ID="AbreviationLabel" runat="server" Text='<%# Bind("Abreviation") %>' /></td>
        </tr>
      </table>
    </ItemTemplate>
    <PagerTemplate>
    </PagerTemplate>
    
  </asp:FormView>
  <asp:GridView ID="gvOrdersDetail" runat="server" AutoGenerateColumns="true" class="TblSimple">
  </asp:GridView>

</asp:Content>
