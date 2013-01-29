<%@ Page Title="Rank Cities" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CompareCity.aspx.cs" Inherits="CompareCities" %>

<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <table> 
        <tr>
            <th>City Ranking</th>
        </tr>
        <tr>
            <td>
                <span class="textbox-preamble">Rank Name:</span>
                <asp:TextBox runat="server"/>
            </td>
        </tr>
        <tr>
            <td>
                <span class="droplist-preamble">Use Scoring Rules:</span>
                <asp:DropDownList ID="ScoringRulesList" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView 
                    ID="CityRanksView" 
                    
                    runat="server" >

                    <Columns>

                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="CalcRankingButton" Text="Rank Cities!" runat="server"/>
                <asp:Button ID="AddCitiesButton" Text="Add Cities" runat="server"/>
                <asp:Button ID="SaveButton" Text="Save" runat="server"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="RankingNameList" runat="server" />
                <asp:Button ID="LoadRankButton" Text="Load Ranking" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>