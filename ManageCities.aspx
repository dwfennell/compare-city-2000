<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageCities.aspx.cs" Inherits="ManageCities" %>



<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" Runat="Server">
    <div class="content-wrapper">
        <h3>Upload City</h3>
        <table>
            <tr><td><asp:Label ID="CityUploadLabel" runat="server" /></td></tr>
            <tr>
                <td>
                    <asp:FileUpload ID="CityFileUpload" runat="server" />
                    <asp:Button ID="Button1" runat="server" Text="Upload City" onclick="Page_CityUpload" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <h3>Your Cities</h3>

    <asp:ListView ID="CitiesView" ItemType="CompareCity.Models.CityInfo" runat="server" SelectMethod="GetCities" >
        <LayoutTemplate>
            <table runat="server" ID="TableCities">
                <tr runat="server">
                    <th runat="server"></th>
                    <th runat="server">City Name</th>
                    <th runat="server">Mayor</th>
                    <th runat="server">City Size</th>
                    <th runat="server">Founded</th>
                    <th runat="server">Days Since Founding</th>
                    <th runat="server">Cashflow</th>
                    <th runat="server">Life Expectancy</th>
                    <th runat="server">Eduation Quotent</th>
                </tr>
                <tr runat="server" id="itemPlaceholder" />
            </table>
            <%-- TODO: Data pager here? --%>
        </LayoutTemplate>

        <ItemTemplate>
            <tr>
                <td runat="server"></td>
                <td runat="server">
                    <asp:Label ID="CityNameLabel" runat="server" Text='<%#: Item.CityName %>' />
                </td>
                <td runat="server">
                    <asp:Label ID="MayorNameLabel" runat="server" Text='<%#: Item.MayorName %>' />
                </td>
                <td runat="server">
                    <asp:Label ID="CitySizeLabel" runat="server" Text='<%# Item.CitySize %>' />
                </td>
                <td runat="server">
                    <asp:Label ID="FoundedLabel" runat="server" Text='<%#: Item.YearOfFounding %>' />
                </td>
                <td runat="server">
                    <asp:Label ID="DaysSinceFoundingLabel" runat="server" Text='<%#: Item.DaysSinceFounding %>' />
                </td>
                <td runat="server">
                    <asp:Label ID="AvailableFundsLabel" runat="server" Text='<%#: Item.AvailableFunds %>' />
                </td>
                <td runat="server">
                    <asp:Label ID="LifeExpectancyLabel" runat="server" Text='<%#: Item.LifeExpectancy %>' />
                </td>
                <td runat="server">
                    <asp:Label ID="EducationQuotentLabel" runat="server" Text='<%#: Item.EducationQuotent %>' />
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>

</asp:Content>