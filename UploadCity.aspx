<%@ Page Title="City Upload" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UploadCity.aspx.cs" Inherits="CityUpload" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="content-wrapper">
        <table>
            <tr><td><asp:Label ID="CityUploadLabel" runat="server" /></td></tr>
            <tr>
                <td>
                    <asp:FileUpload ID="CityFileUpload" runat="server" />
                    <asp:Button runat="server" Text="Upload City" onclick="Page_CityUpload" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

