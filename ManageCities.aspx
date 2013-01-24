<%@ Page Title="Cities" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageCities.aspx.cs" Inherits="ManageCities" %>



<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" Runat="Server">
    <div class="content-wrapper">
        <h3>Upload City</h3>
        <table>
            <tr><td><asp:Label ID="CityUploadLabel" runat="server" /></td></tr>
            <tr>
                <td>
                    <asp:FileUpload ID="CityFileUpload" runat="server" />
                    <asp:Button ID="UploadButton" runat="server" Text="Upload City" onclick="Page_CityUpload" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <h3>Your Cities</h3>

    <asp:GridView ID="CitiesView" 
        ItemType="CompareCity.Models.CityInfo" 
        runat="server" 
        SelectMethod="GetCities" 
        AllowSorting="true" 
        AutoGenerateColumns="false"
        HeaderStyle-CssClass="CityListHeader"
        RowStyle-CssClass="CityListRow">
        <Columns>
            <asp:BoundField DataField="CityName" HeaderText="City Name" SortExpression="CityName" />
            <asp:BoundField DataField="MayorName" HeaderText="Mayor" SortExpression="MayorName" />
            <asp:BoundField DataField="CitySize" HeaderText="Pop" SortExpression="CitySize" />
            <asp:BoundField DataField="YearOfFounding" HeaderText="Founded" SortExpression="YearOfFounding" />
            <asp:BoundField DataField="DaysSinceFounding" HeaderText="Days Since Founding" SortExpression="DaysSinceFounding" />
            <asp:BoundField DataField="AvailableFunds" HeaderText="Cashflow" SortExpression="AvailableFunds" />
            <asp:BoundField DataField="LifeExpectancy" HeaderText="Life Expectancy" SortExpression="LifeExpectancy" />
            <asp:BoundField DataField="EducationQuotent" HeaderText="Education Quotent (EQ)" SortExpression="EducationQuotent" />
            <asp:BoundField DataField="Uploaded" HeaderText="Uploaded" SortExpression="Uploaded" />
        </Columns>
    </asp:GridView>

</asp:Content>