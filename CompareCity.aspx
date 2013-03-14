<%@ Page Title="Score Cities" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CompareCity.aspx.cs" Inherits="CompareCities" %>

<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" runat="Server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" ViewStateMode="Enabled" runat="Server">

    <%-- Ranking display table --%>

    <table>
        <tr>
            <th class="city-score-title">Score Cities : 
                <asp:Label ID="RankingNameLabel" Text="New Score Group" runat="server"  />
            </th>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="RuleFormulaLabel" Text="" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:GridView
                    ID="CityRankingGridView"
                    CssClass="city-display-gridview"
                    RowStyle-CssClass="cityscore-gridview-row" 
                    HeaderStyle-CssClass="gridview-header-row"
                    runat="server">
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button
                    ID="CalcRankingButton"
                    Text="Score Cities!"
                    OnClick="CalcRankingButton_Click"
                    CssClass="ranking-button"
                    Visible="false"
                    runat="server" />
                <asp:Label ID="CalcRankingStatusLabel" Text="" ForeColor="Red" runat="server" />
            </td>
        </tr>
        <tr></tr>
        <tr>
            <td colspan="2">
                <asp:Button
                    ID="LoadRuleSetButton"
                    Text="Set Formula"
                    OnClick="LoadRuleSetButton_Click"
                    CssClass="ranking-button"
                    runat="server" />
                <asp:DropDownList ID="ScoringRulesList" runat="server" />
            </td>
        </tr>
    </table>

    <%-- City search table --%>
    <hr />
    <div class="city-search-area">
        <table id="FindCities1" class="find-cities" visible="false" runat="server">
            <tr>
                <th class="table-subheading">Add Cities</th>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView
                        ID="CitySearchGridView"
                        AllowPaging="true"
                        RowStyle-CssClass="citysearch-gridview-row"
                        AlternatingRowStyle-CssClass="citysearch-gridview-alt-row"
                        HeaderStyle-CssClass="gridview-header-row"
                        runat="server"
                        OnRowCommand="CitySearchGridView_RowCommand">
                        <Columns>
                            <asp:ButtonField ButtonType="Link" Text="Add City" CommandName="AddCity" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="checkbox-wrapper">
                    <asp:CheckBox
                        ID="CitySearchOnlyMeCheckBox"
                        Checked="false"
                        AutoPostBack="true"
                        OnCheckedChanged="CitySearchOnlyMeCheckBox_CheckedChanged"
                        CssClass="city-search-checkbox"
                        runat="server" />
                    Only My Cities
                </td>
                <td class="checkbox-wrapper">
                    <asp:CheckBox
                        ID="CitySearchUserCheckBox"
                        Checked="true"
                        AutoPostBack="true"
                        OnCheckedChanged="CitySearchUserCheckBox_CheckedChanged"
                        CssClass="city-search-checkbox"
                        runat="server" />
                    Any Users' Cities
                </td>
                <td class="checkbox-wrapper">
                    <asp:CheckBox
                        ID="CitySearchCityNameCheckBox"
                        Checked="true"
                        AutoPostBack="true"
                        OnCheckedChanged="CitySearchCityNameCheckBox_CheckedChanged"
                        CssClass="city-search-checkbox"
                        runat="server" />
                    Any City Name
                </td>
            </tr>
        </table>

        <table id="FindCities2" class="find-cities" visible="false" runat="server">
            <tr>
                <td>
                    <asp:Label ID="CitySearchUserLabel" Text="User: " Visible="false" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="CitySearchUserTextBox" Visible="false" Enabled="false" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="CitySearchCityNameLabel" Text="City Name: " Visible="false" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="CitySeachCityNameTextBox" Enabled="false" Visible="false" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button
                        ID="FindCitiesButton"
                        Text="City Search"
                        OnClick="FindCitiesButton_Click"
                        runat="server"
                        CssClass="ranking-button" />
                </td>
            </tr>
        </table>

    </div>

    <%-- Ranking Save/Load --%>

    <hr ID="Rule2" runat="server" visible="false"/>

    <table>
        <tr>
            <th class="table-subheading" colspan="2">Manage City Scoring Groups</th>
        </tr>

        <tr>
            <td>
                <%--<asp:Button ID="SaveButton" Text="Save Ranking" OnClick="SaveButton_Click" CssClass="ranking-button" runat="server" />
                <asp:Label ID="SaveStatusLabel" Text="" runat="server" />--%>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button 
                    ID="LoadRankButton" 
                    Text="Load CSG" 
                    OnClick="LoadRankButton_Click" 
                    runat="server" 
                    CssClass="ranking-button" />

                <span class="colon">:</span>
                <asp:DropDownList ID="RankingNameList" runat="server" />
                <asp:Label ID="LoadStatusLabel" Text="" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="NewRankButton" Text="New CSG" OnClick="NewRankButton_Click" runat="server" CssClass="ranking-button" />
                <asp:Button ID="DeleteRankButton" Text="Delete CSG" OnClick="DeleteRankButton_Click" runat="server" CssClass="ranking-button" />
            </td>
        </tr>
    </table>

</asp:Content>
