<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wedstrijdschema.aspx.vb" Inherits="ParseLisa.wedstrijdschema" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link type="text/css" rel="Stylesheet" href="/css/team.css" />
</head>
<body>
    <form id="form1" runat="server">
    <h2>Wedstrijdschema</h2>
    <div id="game-selector">
    <label for="ddlUpcoming">Aankomende wedstrijden</label><br />
    <asp:DropDownList ID="ddlUpcoming" runat="server" AutoPostBack="True">
    </asp:DropDownList>
    </div>
    <p>
    <asp:Label ID="lblWedstrijden" runat="server" Text=""></asp:Label>
    </p>
    </form>
</body>
</html>
