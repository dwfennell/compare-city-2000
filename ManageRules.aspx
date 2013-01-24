<%@ Page Title="Rules" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageRules.aspx.cs" Inherits="ManageRules" %>


<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" Runat="Server">

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <table>
        <tr>
            <th colspan="2">Create Scoring Rules</th>
        </tr>
        <tr>
            <td></td><td><asp:Label ID="FormulaStatus" runat="server" Text="" /></td>
        </tr>
        <tr>
            <td>Name:</td>
            <td><asp:TextBox ID="FormulaNameTextBox" runat="server" /></td>
            <td></td>
        </tr>
        <tr>
            <td>Formula:</td>
            <td><asp:TextBox ID="FormulaTextBox" runat="server"/></td>
            <td><asp:Button ID="CheckFormulaButton" Text="Check Formula" runat="server" OnClick="CheckFormula_Page"/></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td><asp:Button ID="SaveFormulaButton" text="Save Formula" runat="server" OnClick="SaveFormula_Page" /></td>
        </tr>
    </table>
</asp:Content>

