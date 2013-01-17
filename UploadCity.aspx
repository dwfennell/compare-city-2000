<%@ Page Title="City Upload" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UploadCity.aspx.cs" Inherits="CityUpload" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="content-wrapper">
        <table>
            <tr>
                <td>
                    <span class="file-upload-label">Upload City</span>
                    <asp:FileUpload ID="CityFileUpload" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

