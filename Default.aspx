<%@ Page Title="Compare City 2000" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>Getting Started:</h3>
    <ol class="round">
        <li class="one">
            <h5><a href="ManageCities.aspx">Upload Cities</a></h5>
            Upload your city so our servers can run through it with a fine-toothed comb.
        </li>
        <li class="two">
            <h5><a href="ManageRules.aspx">Write a Scoring Formula</a></h5>
            Tell our mainframe what makes your city the best, by writing a precise mathematical formulas!
        </li>
        <li class="three">
            <h5><a href="CompareCity.aspx">Group Cities and Calculate Scores!</a></h5>
            Group your cities together with other users' and claim your inevitable victory.
        </li>
    </ol>
</asp:Content>