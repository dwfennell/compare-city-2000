<%@ Page Title="Rank Cities" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CompareCity.aspx.cs" Inherits="CompareCities" %>

<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" ViewStateMode="Enabled" Runat="Server">
    <table> 
        <tr>
            <th>City Ranking <asp:Label ID="RankingNameLabel" Text="" runat="server" /></th>
        </tr>
        <tr>
            <td colspan="3">
                <asp:TextBox ID="RankingNameTextBox" runat="server"/>
            </td>
        </tr>
        <tr>
            <td colspan="3"><span class="gridview-subheading">Cities</span></td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:GridView ID="CityRankingGridView" AllowSorting="true" runat="server">
                    <EmptyDataTemplate>
                        <span class="gridview-emptydata">--search below for cities to rank--</span>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <span class="label-preamble">Rule Set: </span>
                <asp:Label ID="RuleSetLabel" Text="-no rule set loaded-" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <span class="label-preamble">Formula: </span>
                <asp:Label ID="RuleFormulaLabel" Text="-no rule set loaded-" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ScoringRulesList" runat="server" />
            </td>
            <td>
                <asp:Button ID="LoadRuleSetButton" Text="Load Rule Set" OnClick="LoadRuleSetButton_Click" CssClass="ranking-button"  runat="server" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="CalcRankingButton" Text="Rank Cities!" OnClick="CalcRankingButton_Click" CssClass="ranking-button"  runat="server"/>
                <asp:Label ID="CalcRankingStatusLabel" Text="" ForeColor="Red" runat="server" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="SaveButton" Text="Save Ranking" OnClick="SaveButton_Click" CssClass="ranking-button"  runat="server"/>
                <asp:Label ID="SaveStatusLabel" Text="" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="RankingNameList" runat="server"/>
            </td>
            <td>
                <asp:Button ID="LoadRankButton" Text="Load Ranking" OnClick="LoadRankButton_Click" runat="server" CssClass="ranking-button" />
                <asp:Label ID="LoadStatusLabel" Text="" runat="server" />
            </td>
        </tr>
    </table>

    <table class="FindCitiesTable">
        <tr><th>Find Cities</th></tr>
        <tr>
            <td>
                <span class="textbox-preamble" >User:</span>
            </td>
            <td>
                <asp:TextBox ID="CitySearchUserTextBox" runat="server" Enabled="false"/>
            </td>
            <td>
                <asp:CheckBox ID="CitySearchUserCheckBox" 
                    Checked="true" 
                    Text="All Users" 
                    AutoPostBack="true" 
                    OnCheckedChanged="CitySearchUserCheckBox_CheckedChanged" 
                    runat="server"/>
            </td>
        </tr>
        <tr>
            <td>
                <span class="textbox-preamble">City Name:</span>
            </td>
            <td>
                <asp:TextBox ID="CitySeachCityNameTextBox" Enabled="false" runat="server" />
            </td>
            <td>
                <asp:CheckBox ID="CitySearchCityNameCheckBox" 
                    Text="All Cities" 
                    Checked="true" 
                    AutoPostBack="true" 
                    OnCheckedChanged="CitySearchCityNameCheckBox_CheckedChanged" 
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:GridView 
                    ID="CitySearchGridView" 
                    AllowPaging="true" 
                    runat="server" 
                    OnRowCommand="CitySearchGridView_RowCommand">
                    <Columns>
                        <asp:ButtonField ButtonType="Link" Text="Add City" CommandName="AddCity"/>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="FindCitiesButton" Text="Find Cities" OnClick="FindCitiesButton_Click" runat="server" CssClass="ranking-button"/>
            </td>
        </tr>
    </table>
</asp:Content>