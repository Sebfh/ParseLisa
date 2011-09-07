Public Class team_list
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/TeamsService.asp?clubcode=HH11BZ2")

        lblteamlistDames.Text = getTeamsDamesDatatable(resultXML)
        lblteamlistHeren.Text = getTeamsHerenDatatable(resultXML)
        lblteamlistMeisjes.Text = getTeamsMeisjesDatatable(resultXML)
        lblteamlistJongens.Text = getTeamsJongensDatatable(resultXML)
        lblteamlistOverig.Text = getTeamsOverigDatatable(resultXML)
    End Sub
    Function getTeamsDamesDatatable(ByVal resultXML As XDocument) As String
        'Geeft alle teams voor deze club
        'params: url:http://hockey-xml.lisa-is.nl/xml/TeamsService.asp?clubcode=HH11BZ2

        Dim table As String = "<table class=""teams_list"" id=""teamlist_dames"">"
        table += "<tr class=""header""><td colspan=""5"">&nbsp;Teams Dames</td></tr>"


        Dim teams = From team In resultXML.Descendants("team") _
                     Select team

        For Each team In teams

            If team.Attribute("naam").Value.IndexOf("D") = 0 Then
                Dim tr As String = "<tr>"
                tr += "<td><a href=""/team.aspx?team=" & team.Attribute("id").Value & """>" & team.Attribute("naam").Value & "</a></td>"
                tr += "</tr>"

                table += tr
            End If


        Next

        Return table
    End Function
    Function getTeamsHerenDatatable(ByVal resultXML As XDocument) As String
        'Geeft alle teams voor deze club
        'params: url:http://hockey-xml.lisa-is.nl/xml/TeamsService.asp?clubcode=HH11BZ2

        Dim table As String = "<table class=""teams_list"" id=""teamlist_heren"">"
        table += "<tr class=""header""><td colspan=""5"">&nbsp;Teams Heren</td></tr>"


        Dim teams = From team In resultXML.Descendants("team") _
                     Select team

        For Each team In teams

            If team.Attribute("naam").Value.IndexOf("H") = 0 Then
                Dim tr As String = "<tr>"
                tr += "<td><a href=""/team.aspx?team=" & team.Attribute("id").Value & """>" & team.Attribute("naam").Value & "</a></td>"
                tr += "</tr>"

                table += tr
            End If


        Next

        Return table
    End Function
    Function getTeamsMeisjesDatatable(ByVal resultXML As XDocument) As String
        'Geeft alle teams voor deze club
        'params: url:http://hockey-xml.lisa-is.nl/xml/TeamsService.asp?clubcode=HH11BZ2

        Dim table As String = "<table class=""teams_list"" id=""teamlist_heren"">"
        table += "<tr class=""header""><td colspan=""5"">&nbsp;Teams Meisjes</td></tr>"


        Dim teams = From team In resultXML.Descendants("team") _
                     Select team

        For Each team In teams

            If team.Attribute("naam").Value.IndexOf("M") = 0 Then
                Dim tr As String = "<tr>"
                tr += "<td><a href=""/team.aspx?team=" & team.Attribute("id").Value & """>" & team.Attribute("naam").Value & "</a></td>"
                tr += "</tr>"

                table += tr
            End If


        Next

        Return table
    End Function
    Function getTeamsJongensDatatable(ByVal resultXML As XDocument) As String
        'Geeft alle teams voor deze club
        'params: url:http://hockey-xml.lisa-is.nl/xml/TeamsService.asp?clubcode=HH11BZ2

        Dim table As String = "<table class=""teams_list"" id=""teamlist_heren"">"
        table += "<tr class=""header""><td colspan=""5"">&nbsp;Teams Jongens</td></tr>"


        Dim teams = From team In resultXML.Descendants("team") _
                     Select team

        For Each team In teams

            If team.Attribute("naam").Value.IndexOf("J") = 0 Then
                Dim tr As String = "<tr>"
                tr += "<td><a href=""/team.aspx?team=" & team.Attribute("id").Value & """>" & team.Attribute("naam").Value & "</a></td>"
                tr += "</tr>"

                table += tr
            End If


        Next

        Return table
    End Function
    Function getTeamsOverigDatatable(ByVal resultXML As XDocument) As String
        'Geeft alle teams voor deze club
        'params: url:http://hockey-xml.lisa-is.nl/xml/TeamsService.asp?clubcode=HH11BZ2

        Dim table As String = "<table class=""teams_list"" id=""teamlist_heren"">"
        table += "<tr class=""header""><td colspan=""5"">&nbsp;Teams Overig</td></tr>"


        Dim teams = From team In resultXML.Descendants("team") _
                     Select team

        For Each team In teams

            If Not team.Attribute("naam").Value.IndexOf("H") = 0 And Not team.Attribute("naam").Value.IndexOf("D") = 0 And Not team.Attribute("naam").Value.IndexOf("M") = 0 And Not team.Attribute("naam").Value.IndexOf("J") = 0 Then
                Dim tr As String = "<tr>"
                tr += "<td><a href=""/team.aspx?team=" & team.Attribute("id").Value & """>" & team.Attribute("naam").Value & "</a></td>"
                tr += "</tr>"

                table += tr
            End If


        Next

        Return table
    End Function
End Class