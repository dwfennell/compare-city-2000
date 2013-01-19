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

    <asp:ListView ID="CitiesView" ItemType="CompareCity.Models.City" runat="server" SelectMethod="GetCities">
        <LayoutTemplate>
            <table runat="server" ID="TableCities">
                <tr runat="server">
                    <th runat="server"></th>
                    <th runat="server">City Name</th>
                    <th runat="server">Mayor Name</th>
                    <th runat="server">Pop</th>
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
                <asp:Label ID="PopulationLabel" runat="server" Text='<%# Item.Population %>' />
            </td>
        </tr>
        </ItemTemplate>
    </asp:ListView>

</asp:Content>