<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="trainingsschema.aspx.vb"
    Inherits="ParseLisa.trainingsschema" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="/css/team.css" />
</head>
<body>
    <form id="form1" runat="server">
    <h2>
        Trainingsschema</h2>
    <p style="display:none;">
        <asp:Label ID="lblNow" runat="server" Text=""></asp:Label></p>
    <div id="schema_container">
        <asp:Label ID="lblTrainingDag1" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblTrainingDag2" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblTrainingDag3" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblTrainingDag4" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblTrainingDag5" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
