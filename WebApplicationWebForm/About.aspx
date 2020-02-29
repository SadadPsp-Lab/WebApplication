<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebApplicationWebForm.About" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h2>Your app description page.</h2>
    </hgroup>

    <article>
        <p>        
            Use this area to provide additional information.
        </p>

        <p>        
            Use this area to provide additional information.
        </p>

        <p>        
            Use this area to provide additional information.
        </p>
    </article>

    <aside>
        <h3>Aside Title</h3>
        <p>        
            Use this area to provide additional information.
        </p>
        <ul>
            <li><a runat="server" href="~/">Home</a></li>
            <li><a runat="server" href="~/About">About</a></li>
            <li><a runat="server" href="~/Contact">Contact</a></li>
        </ul>
    </aside>


    <asp:Label ID="Message" runat="server"></asp:Label>
    <asp:Label ID="ResCode" runat="server"></asp:Label>
    <asp:Label ID="RRN" runat="server"></asp:Label>
    <asp:Label ID="Trace" runat="server"></asp:Label>
</asp:Content>