<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="team_list.aspx.vb" Inherits="ParseLisa.team_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="/css/team.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>
            Actueel teamoverzicht
        </h2>
        <p>
            <asp:Label ID="lblteamlistDames" runat="server" Text="Label"></asp:Label>
        </p>
        <p>
            <asp:Label ID="lblteamlistHeren" runat="server" Text="Label"></asp:Label>
        </p>
        <p>
            <asp:Label ID="lblteamlistMeisjes" runat="server" Text="Label"></asp:Label>
        </p>
        <p>
            <asp:Label ID="lblteamlistJongens" runat="server" Text="Label"></asp:Label>
        </p>
        <p>
            <asp:Label ID="lblteamlistOverig" runat="server" Text="Label"></asp:Label>
        </p>
    </div>
    </form>
</body>
</html>
