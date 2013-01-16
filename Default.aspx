<%@ Page Title="Compare City" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2></h2>
            </hgroup>
            <p>
                Compare Sim City 2000 cities!
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>...</h3>
    <ol class="round">
        <li class="one">
            <h5>..</h5>
            ..
        </li>
        <li class="two">
            <h5>...</h5>
            ...
        </li>
        <li class="three">
            <h5>...</h5>
            ...
        </li>
    </ol>
</asp:Content>