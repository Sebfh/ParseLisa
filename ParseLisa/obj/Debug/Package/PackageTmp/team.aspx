<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="team.aspx.vb" Inherits="ParseLisa.team" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="/css/team.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="teamselecter">
        <asp:DropDownList ID="ddTeamList" DataValueField="id" DataTextField="naam" runat="server">
        </asp:DropDownList>
        <asp:Button ID="btnSelect" runat="server" Text="naar team" />
        <h2>
            <asp:Label ID="lblTeamNaam" runat="server" Text=""></asp:Label>
        </h2>
    </div>
    <div class="team_players">
        <asp:Repeater ID="rptSpelers" runat="server">
        <HeaderTemplate>
            <table class="team_players">
            <tr class="header">
                <td colspan="2">Spelers</td>
            </tr>
        </HeaderTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
            <ItemTemplate>
                <tr>
                    <td style="width: 30px;">
                    <%# Container.ItemIndex + 1 %>
                    </td>
                    <td>
                    <%# DataBinder.Eval(Container.DataItem, "voornaam")%> <%# DataBinder.Eval(Container.DataItem, "tussenv")%> <%# DataBinder.Eval(Container.DataItem, "achternaam")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="team_ondersteuning">
                <asp:Repeater ID="rptOndersteuning" runat="server">
        <HeaderTemplate>
            <table class="team_ondersteuning">
            <tr class="header">
                <td colspan="2">Teamondersteuning</td>
            </tr>
        </HeaderTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
            <ItemTemplate>
                <tr>
                    <td style="width: 50px;">
                    <%# DataBinder.Eval(Container.DataItem, "type")%>
                    </td>
                    <td>
                    <%# DataBinder.Eval(Container.DataItem, "voornaam")%> <%# DataBinder.Eval(Container.DataItem, "tussenv")%> <%# DataBinder.Eval(Container.DataItem, "achternaam")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </div>
       <div class="team_upcoming_container">
        <asp:Label ID="lblTeamUpcoming" runat="server" Text=""></asp:Label>
    </div>
    <div class="team_poule">
        
        <asp:Repeater ID="rptPoule" runat="server">
        <HeaderTemplate>
            <table class="team_poule">
                <tr class="header">
                    <td colspan="10">Stand in de poule</td>
                </tr>
                <tr>
                    <td class="bold"> </td>
                    <td class="bold">Team</td>
                    <td class="bold">#</td>
                    <td class="bold">Pntn</td>
                    <td class="bold">W</td>
                    <td class="bold">G</td>
                    <td class="bold">V</td>
                    <td class="bold">++</td>
                    <td class="bold">--</td>
                    <td class="bold">Straf</td>
                </tr>
        </HeaderTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
         <ItemTemplate>
                <tr>
                    <td><%# DataBinder.Eval(Container.DataItem, "nummer")%></td>
                
                    <td><%# DataBinder.Eval(Container.DataItem, "team")%></td>
               
                    <td><%# DataBinder.Eval(Container.DataItem, "totalmatches")%></td>
                
                    <td class="bold"><%# DataBinder.Eval(Container.DataItem, "totalpoints")%></td>
               
                    <td class="green"><%# DataBinder.Eval(Container.DataItem, "won")%></td>
                
                    <td><%# DataBinder.Eval(Container.DataItem, "draw")%></td>
                    
                    <td class="red"><%# DataBinder.Eval(Container.DataItem, "lost")%></td>
              
                    <td><%# DataBinder.Eval(Container.DataItem, "goalsfor")%></td>
              
                    <td><%# DataBinder.Eval(Container.DataItem, "goalsagainst")%></td>

                    <td><%# DataBinder.Eval(Container.DataItem, "penaltypoints")%></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        
    </div>
 
    <div class="team_uitslagen_container">
        <asp:Label ID="lblTeamResults" runat="server" Text=""></asp:Label>
    </div>
    
    </form>
</body>
</html>
