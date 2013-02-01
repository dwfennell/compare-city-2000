﻿<%@ Page Title="Rank Cities" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CompareCity.aspx.cs" Inherits="CompareCities" %>

<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <table> 
        <tr>
            <th>City Ranking <asp:Label ID="RankingNameLabel" Text="" runat="server" /></th>
        </tr>
        <tr>
            <td>
                <span class="textbox-preamble">Rank Name:</span>
                <asp:TextBox ID="RankingNameTextBox" runat="server"/>
            </td>
        </tr>
        <tr>
            <td>
                <span class="droplist-preamble">Using Rule Set:</span>
                <asp:DropDownList 
                    ID="ScoringRulesList" 
                    ItemType="CompareCity.Model.RuleSet" 
                    OnSelectedIndexChanged="ScoringRulesList_SelectedIndexChanged" 
                    AppendDataBoundItems="false"
                    runat="server">
                    <asp:listitem>--Select Rule Set--</asp:listitem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView 
                    ID="CityRanksView"
                    ItemType="CompareCity.Model.ComparisonGroupMember"
                    AutoGenerateColumns="false"
                    AllowSorting="true" 
                    AutoGenerateDeleteButton="true"
                    DeleteMethod="CityRanksView_DeleteItem"
                    SelectMethod="CityRanksView_GetData"
                    DataKeyNames="ComparisonGroupMemberId"
                    runat="server" >
                    <Columns>
                        <asp:BoundField DataField="CityUser" SortExpression="User" HeaderText="User" />
                        <asp:BoundField DataField="CityInfo" SortExpression="CityInfo" HeaderText="City" />
                        <asp:BoundField DataField="TotalScore" SortExpression="TotalScore" HeaderText="Score" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="CalcRankingButton" Text="Rank Cities!" OnClick="CalcRankingButton_Click" runat="server"/>
                <asp:Button ID="AddCitiesButton" Text="Add Cities" OnClick="AddCitiesButton_Click" runat="server"/>
                
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="SaveButton" Text="Save" OnClick="SaveButton_Click" runat="server"/>
                <asp:Label ID="SaveStatusLabel" Text="" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="RankingNameList" AppendDataBoundItems="false" runat="server">
                    <asp:ListItem>--Select Other Ranking--</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="LoadRankButton" Text="Load Ranking" OnClick="LoadRankButton_Click" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>