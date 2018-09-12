<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="BankingLedger.Account.Manage" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage your ledger</h2>

    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>

   <div>
       <br />
       <asp:Label ID="DepositLabel" runat="server" Text="Record deposit" Font-Size="Larger"></asp:Label>
       <br />
       <br />
       Date of transaction:<br />
       <asp:TextBox ID="DepositDate" runat="server"></asp:TextBox>
       <br />
       Amount:<br />
       <asp:TextBox ID="DepositTextbox" runat="server"></asp:TextBox>
       <br />
       <asp:Button ID="DepositButton" runat="server" Text="Submit" OnClick="DepositButton_Click" />
       <br />
       <asp:Label ID="DepositSuccess" runat="server" Text=""></asp:Label>
       <br />
       <br />
       <asp:Label ID="WithdrawLabel" runat="server" Text="Record withdrawal" Font-Size="Larger"></asp:Label>
       <br />
       <br />
       Date of transaction:<br />
       <asp:TextBox ID="WithdrawDate" runat="server"></asp:TextBox>
       <br />
       Amount:<br />
       <asp:TextBox ID="WithdrawTextbox" runat="server"></asp:TextBox>
       <br />
       <asp:Button ID="WithdrawButton" runat="server" Text="Submit" OnClick="WithdrawButton_Click" />
       <br />
       <asp:Label ID="WithdrawSuccess" runat="server" Text=""></asp:Label>
       <br />
       <br />
       <asp:Label ID="TransactionHistoryLabel" runat="server" Text="Transaction History" Font-Size="Larger"></asp:Label>
       <br />
       <asp:GridView ID="GridView1" runat="server">
       </asp:GridView>
       <br />
       <br />
    </div>

</asp:Content>
