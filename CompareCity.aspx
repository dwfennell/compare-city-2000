<%@ Page Title="Rank Cities" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CompareCity.aspx.cs" Inherits="CompareCities" %>

<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" runat="Server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" ViewStateMode="Enabled" runat="Server">

    <%-- Ranking display table --%>

    <table>
        <tr>
            <th class="table-heading">City Ranking
                <asp:Label ID="RankingNameLabel" Text="" runat="server" /></th>
        </tr>
        <tr>
            <td colspan="3">
                <asp:TextBox ID="RankingNameTextBox" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:GridView
                    ID="CityRankingGridView"
                    AllowSorting="true"
                    CssClass="city-display-gridview"
                    RowStyle-CssClass="city-display-gridview-row"
                    HeaderStyle-CssClass="city-display-gridview-header"
                    runat="server">
                    <EmptyDataTemplate>
                        <span class="gridview-emptydata">--search below for cities to rank--</span>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="CalcRankingButton" Text="Rank Cities!" OnClick="CalcRankingButton_Click" CssClass="ranking-button" runat="server" />
                <asp:Label ID="CalcRankingStatusLabel" Text="" ForeColor="Red" runat="server" />
            </td>
        </tr>
        <tr></tr>

        <tr>
            <th class="table-subheading">Ranking Formula</th>
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
            <td colspan="2">
                <asp:Button ID="LoadRuleSetButton" Text="Load Rule Set" OnClick="LoadRuleSetButton_Click" CssClass="ranking-button" runat="server" />
                <span class="colon">:</span>
                <asp:DropDownList ID="ScoringRulesList" runat="server" />
            </td>
        </tr>
    </table>

    <%-- City search table --%>

    <table class="FindCitiesTable">
        <tr>
            <th class="table-subheading">Add Cities</th>
        </tr>
        <tr>
            <td>
                <asp:CheckBox
                    ID="CitySearchOnlyMeCheckBox"
                    Checked="false"
                    AutoPostBack="true"
                    Text="Only My Cities"
                    OnCheckedChanged="CitySearchOnlyMeCheckBox_CheckedChanged"
                    CssClass="city-search-checkbox"
                    runat="server" />
            </td>
            <td>
                <asp:CheckBox
                    ID="CitySearchUserCheckBox"
                    Checked="true"
                    AutoPostBack="true"
                    Text="All Users"
                    OnCheckedChanged="CitySearchUserCheckBox_CheckedChanged"
                    CssClass="city-search-checkbox"
                    runat="server" />
            </td>
            <td>
                <asp:CheckBox
                    ID="CitySearchCityNameCheckBox"
                    Checked="true"
                    AutoPostBack="true"
                    Text="All Cities"
                    OnCheckedChanged="CitySearchCityNameCheckBox_CheckedChanged"
                    CssClass="city-search-checkbox"
                    runat="server" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Label ID="CitySearchUserLabel" Text="User:" Visible="false" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="CitySearchUserTextBox" Visible="false" Enabled="false" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="CitySearchCityNameLabel" Text="City Name:" Visible="false" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="CitySeachCityNameTextBox" Enabled="false" Visible="false" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="FindCitiesButton" Text="Find Cities" OnClick="FindCitiesButton_Click" runat="server" CssClass="ranking-button" />
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
                        <asp:ButtonField ButtonType="Link" Text="Add City" CommandName="AddCity" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>

    <%-- Ranking Save/Load --%>

    <table>
        <tr>
            <th class="table-subheading" colspan="2">Save / Load Rankings</th>
        </tr>

        <tr>
            <td>
                <asp:Button ID="SaveButton" Text="Save Ranking" OnClick="SaveButton_Click" CssClass="ranking-button" runat="server" />
                <asp:Label ID="SaveStatusLabel" Text="" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="LoadRankButton" Text="Load Ranking" OnClick="LoadRankButton_Click" runat="server" CssClass="ranking-button" />
                <span class="colon">:</span>
                <asp:DropDownList ID="RankingNameList" runat="server" />
                <asp:Label ID="LoadStatusLabel" Text="" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="NewRankButton" Text="New Ranking" OnClick="NewRankButton_Click" runat="server" CssClass="ranking-button" />
            </td>
        </tr>
    </table>


</asp:Content>
