﻿<%@ Page Title="Rules" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageRules.aspx.cs" Inherits="ManageRules" %>


<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" Runat="Server">

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <table>
        <tr>
            <th colspan="2">Create Scoring Rules</th>
        </tr>
        <tr>
            <td></td>
            <td><asp:Label ID="FormulaStatus" runat="server" Text="" /></td>
        </tr>
        <tr>
            <td>Name:</td>
            <td><asp:TextBox ID="FormulaNameTextBox" runat="server" /></td>
            <td></td>
        </tr>
        <tr>
            <td>Formula:</td>
            <td><asp:TextBox ID="FormulaTextBox" runat="server"/></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="CheckFormulaButton" Text="Check Formula" runat="server" OnClick="CheckFormulaButton_Click" />
                <asp:Button ID="SaveFormulaButton" text="Save Formula" runat="server" OnClick="SaveFormulaButton_Click" />
            </td>
            <td></td>
        </tr>
        <tr>
            <th colspan="3">Your Rule Sets</th>
        </tr>
        <tr>
            <td colspan="3">
                <asp:GridView 
                    ItemType="CompareCity.Model.RuleSet" 
                    ID="RuleSetsView" 
                    runat="server" 
                    SelectMethod="GetRules"
                    AllowSorting="true" 
                    AutoGenerateColumns="false" 
                    AutoGenerateDeleteButton="true" 
                    DeleteMethod="RuleSetsView_DeleteItem" 
                    DataKeyNames="RuleSetId" 
                    OnRowCommand="RuleSetsView_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="RuleSetName" HeaderText="Name" SortExpression="RuleSetName" />
                        <asp:BoundField DataField="Formula" HeaderText="Formula" SortExpression="Formula" />
                        <asp:BoundField DataField="Valid" HeaderText="Valid?" SortExpression="Valid" />
                        <asp:ButtonField ButtonType="Link" Text="Copy to edit area" CommandName="CopyFormula"/>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>

