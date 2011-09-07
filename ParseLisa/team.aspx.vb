Imports System.Xml
Imports System.Xml.Linq

Public Class team
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ddTeamList.DataSource = getTeamList()
            ddTeamList.DataBind()

            Dim selected_team As String = Request.QueryString.Get("team")
            If Not selected_team Is Nothing Then
                ddTeamList.SelectedValue = selected_team

                rptSpelers.DataSource = getTeamDatatable(selected_team)
                rptSpelers.DataBind()

                rptOndersteuning.DataSource = getTeamOndersteuningDatatable(selected_team)
                rptOndersteuning.DataBind()

                rptPoule.DataSource = getTeamPouleStanden(selected_team)
                rptPoule.DataBind()

                lblTeamUpcoming.Text = getTeamUPcoming(selected_team)

                lblTeamResults.Text = getTeamResultsTable(selected_team)

                lblTeamNaam.Text = ddTeamList.SelectedItem.Text
            Else
                'hoop uitzetten
            End If


        End If
    End Sub

    Function getTeamResults(ByVal team_id As Integer) As DataTable
        'Geeft alle resultaten terug van een team
        'params: url:http://hockey-xml.lisa-is.nl/xml/ResultsService.asp?clubcode=HH11BZ2
        '        team_id

        Dim dtResults As New DataTable

        Dim play_date As New DataColumn
        play_date.DataType = Type.GetType("System.String")
        play_date.ColumnName = "play_date"

        Dim home_team As New DataColumn
        home_team.DataType = Type.GetType("System.String")
        home_team.ColumnName = "home_team"

        Dim away_team As New DataColumn
        away_team.DataType = Type.GetType("System.String")
        away_team.ColumnName = "away_team"

        Dim goals_for As New DataColumn
        goals_for.DataType = Type.GetType("System.String")
        goals_for.ColumnName = "goals_for"

        Dim goals_against As New DataColumn
        goals_against.DataType = Type.GetType("System.String")
        goals_against.ColumnName = "goals_against"

        Dim closed As New DataColumn
        closed.DataType = Type.GetType("System.String")
        closed.ColumnName = "closed"

        dtResults.Columns.Add(play_date)
        dtResults.Columns.Add(home_team)
        dtResults.Columns.Add(away_team)
        dtResults.Columns.Add(goals_for)
        dtResults.Columns.Add(goals_against)
        dtResults.Columns.Add(closed)

        Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/ResultsService.asp?clubcode=HH11BZ2")
        Dim team_results = From team In resultXML.Descendants("team") _
                Where (team.Attribute("id").Value = team_id.ToString)


        Dim wedstrijddagen = From dag In team_results.Descendants("dag")
                      Select dag

        For Each dag In wedstrijddagen
            Dim datum As String = dag.Attribute("datum").Value

            Dim uitslagen = From uitslag In dag.Descendants("uitslag")
                            Select uitslag

            For Each uitslag In uitslagen
                Dim row = dtResults.NewRow

                If Not datum Is Nothing Then
                    row("play_date") = datum
                End If

                If Not uitslag.Attribute("hometeam") Is Nothing Then
                    row("home_team") = uitslag.Attribute("hometeam").Value
                End If

                If Not uitslag.Attribute("awayteam") Is Nothing Then
                    row("away_team") = uitslag.Attribute("awayteam").Value
                End If

                If Not uitslag.Attribute("goalsfor") Is Nothing Then
                    row("goals_for") = uitslag.Attribute("goalsfor").Value
                End If

                If Not uitslag.Attribute("goalsagainst") Is Nothing Then
                    row("goals_against") = uitslag.Attribute("goalsagainst").Value
                End If

                If Not uitslag.Attribute("definitief") Is Nothing Then
                    row("closed") = uitslag.Attribute("definitief").Value
                End If

                dtResults.Rows.Add(row)
            Next
        Next
        Return dtResults
    End Function
    Function getTeamUPcoming(ByVal team_id As Integer) As String
        'Geeft alle aankomende wedstrijden weer voor een team
        'params: url: http://hockey-xml.lisa-is.nl/xml/FutureMatchesService.asp?clubcode=HH11BZ2

        Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/FutureMatchesService.asp?clubcode=HH11BZ2")
        Dim table As String = "<table cellspacing=""0"" class=""wedstrijdschema"">"
        table += "<tr class=""header""><td>Wedstrijd</td><td>Team</td><td>Tegen</td><td>Nr</td><td>Tijd</td><td>Locatie</td></tr>"

        Dim wedstrijden = From wedstrijd In resultXML.Descendants("wedstrijd") _
                           Where (wedstrijd.Attribute("team_id").Value = team_id.ToString)
                           Select wedstrijd

        If wedstrijden.Count > 0 Then

            For Each game In wedstrijden
                Dim tr As String = "<tr class=""game_row"">"

                If Not game.Attribute("wedstrijd_datum") Is Nothing Then
                    Dim datum As Date = game.Attribute("wedstrijd_datum").Value
                    tr += "<td>" & datum.ToShortDateString & "</td>"
                Else
                    tr += "<td>&nbsp;</td>"
                End If

                If Not game.Attribute("club_team") Is Nothing Then
                    tr += "<td>" & game.Attribute("club_team").Value & "</td>"
                Else
                    tr += "<td>&nbsp;</td>"
                End If

                If Not game.Attribute("opponent_club_naam") Is Nothing And Not game.Attribute("opponent_club_naam") Is Nothing Then
                    tr += "<td>" & game.Attribute("opponent_club_naam").Value & " " & game.Attribute("opponent_team").Value & "</td>"
                Else
                    tr += "<td>&nbsp;</td>"
                End If

                If Not game.Attribute("WedstrijdNummer") Is Nothing Then
                    tr += "<td>" & game.Attribute("WedstrijdNummer").Value & "</td>"
                Else
                    tr += "<td>&nbsp;</td>"
                End If

                If Not game.Attribute("tijd") Is Nothing Then
                    tr += "<td>" & game.Attribute("tijd").Value & "</td>"
                Else
                    tr += "<td>&nbsp;</td>"
                End If

                If Not game.Attribute("wedstrijd_thuis") Is Nothing Then
                    If game.Attribute("wedstrijd_thuis").Value = "nee" Then
                        If Not game.Attribute("verzamel") Is Nothing Then
                            tr += "<td>Uit (verz.: " & game.Attribute("verzamel").Value & ")</td>"
                        Else
                            tr += "<td>Uit</td>"
                        End If

                    Else
                        tr += "<td>Thuis</td>"
                    End If
                Else
                    tr += "<td>&nbsp;</td>"
                End If

                tr += "</tr>"

                Dim scheidsrechters = From scheidsrechter In game.Descendants("scheidsrechter") _
                                    Select scheidsrechter

                For Each scheids In scheidsrechters
                    tr += "<tr>"

                    tr += "<td>&nbsp;</td><td>Arbitrage</td>"
                    tr += "<td colspan""3"">" & scheids.Attribute("vnaam").Value & " " & scheids.Attribute("tv").Value & " " & scheids.Attribute("anaam").Value & "</td>"

                    tr += "</tr>"
                Next

                'If scheidsrechters.Count > 0 Then
                tr += "<tr><td colspan=""5"">&nbsp;</td></tr>"
                'End If

                table += tr
            Next

        Else
            'er is geen game
            table += "<tr><td colspan=""6"">Geen wedstrijden gevonden</td></tr>"
        End If

        table += "<table>"
        Return table

    End Function
    Function getTeamResultsTable(ByVal team_id As Integer) As String
        'Geeft alle resultaten terug van een team
        'params: url:http://hockey-xml.lisa-is.nl/xml/ResultsService.asp?clubcode=HH11BZ2
        '        team_id

        Dim table As String = "<table class=""team_uitslagen"">"
        table += "<tr class=""header""><td colspan=""4"">Uitslagen</td></tr>"

        Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/ResultsService.asp?clubcode=HH11BZ2")
        Dim team_results = From team In resultXML.Descendants("team") _
                Where (team.Attribute("id").Value = team_id.ToString)


        Dim wedstrijddagen = From dag In team_results.Descendants("dag")
                      Select dag

        For Each dag In wedstrijddagen
            Dim datum As Date = dag.Attribute("datum").Value
            Dim tr As String = "<tr>"
            tr += "<td colspan=""4"" class=""bold"">" & datum.ToLongDateString & "</td>"
            tr += "</tr>"


            Dim uitslagen = From uitslag In dag.Descendants("uitslag")
                            Select uitslag

            For Each uitslag In uitslagen

                tr += "<tr>"
                tr += "<td>" & "&nbsp;" & "</td>"
                tr += "<td>" & uitslag.Attribute("hometeam").Value & "</td>"
                tr += "<td>" & uitslag.Attribute("awayteam").Value & "</td>"
                tr += "<td>" & uitslag.Attribute("goalsfor").Value & "-" & uitslag.Attribute("goalsagainst").Value & "</td>"
                tr += "</tr>"

            Next
            tr += "<tr id=""spacing_row""><td colspan=""4"">&nbsp;</td></tr>"
            table += tr

        Next
        table += "</table>"

        Return table
    End Function
    Function getTeamDatatable(ByVal team_id As Integer) As DataTable
        'Geeft een teamdatatable terug op basis van Lisa XML
        'params: url:http://hockey-xml.lisa-is.nl/xml/PlayerInfoService.asp?clubcode=HH11BZ2
        '        team_id

        Dim dtTeam As New DataTable

        Dim cteam_type As New DataColumn
        cteam_type.DataType = Type.GetType("System.String")
        cteam_type.ColumnName = "type"

        Dim cteam_id As New DataColumn
        cteam_id.DataType = Type.GetType("System.String")
        cteam_id.ColumnName = "team_id"

        Dim cteam_naam As New DataColumn
        cteam_naam.DataType = Type.GetType("System.String")
        cteam_naam.ColumnName = "team_naam"

        Dim cvnaam As New DataColumn
        cvnaam.DataType = Type.GetType("System.String")
        cvnaam.ColumnName = "voornaam"

        Dim ctussenv As New DataColumn
        ctussenv.DataType = Type.GetType("System.String")
        ctussenv.ColumnName = "tussenv"

        Dim canaam As New DataColumn
        canaam.DataType = Type.GetType("System.String")
        canaam.ColumnName = "achternaam"

        Dim clidnummer As New DataColumn
        clidnummer.DataType = Type.GetType("System.String")
        clidnummer.ColumnName = "lidnummer"

        Dim cfoto As New DataColumn
        cfoto.DataType = Type.GetType("System.String")
        cfoto.ColumnName = "foto"

        Dim crol As New DataColumn
        crol.DataType = Type.GetType("System.String")
        crol.ColumnName = "rol"

        dtTeam.Columns.Add(cteam_type)
        dtTeam.Columns.Add(cteam_id)
        dtTeam.Columns.Add(cteam_naam)
        dtTeam.Columns.Add(cvnaam)
        dtTeam.Columns.Add(ctussenv)
        dtTeam.Columns.Add(canaam)
        dtTeam.Columns.Add(clidnummer)
        dtTeam.Columns.Add(cfoto)
        dtTeam.Columns.Add(crol)

        'Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/PlayerInfoService.asp?clubcode=HH11BZ2")
        Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/TeamsService.asp?clubcode=HH11BZ2")

        Dim teams = From team In resultXML.Descendants("team") _
                    Where (team.Attribute("id").Value = team_id.ToString)

        Dim spelers = From speler In teams.Descendants("sp")
                      Select speler

        'add new row for each speler in xml

        Dim teller As Integer = 1
        For Each speler In spelers
            Dim row = dtTeam.NewRow

            row("type") = teller
            row("team_id") = team_id.ToString

            If Not speler.Attribute("vnaam") Is Nothing Then
                row("voornaam") = speler.Attribute("vnaam").Value
            End If

            If Not speler.Attribute("tv") Is Nothing Then
                row("tussenv") = speler.Attribute("tv").Value
            End If

            If Not speler.Attribute("anaam") Is Nothing Then
                row("achternaam") = speler.Attribute("anaam").Value
            End If

            row("rol") = "speler"
            teller = teller + 1
            dtTeam.Rows.Add(row)
        Next

        Return dtTeam
    End Function

    Function getTeamOndersteuningDatatable(ByVal team_id As Integer) As DataTable
        'Geeft een teamdatatable terug op basis van Lisa XML
        'params: url:http://hockey-xml.lisa-is.nl/xml/PlayerInfoService.asp?clubcode=HH11BZ2
        '        team_id

        Dim dtTeam As New DataTable

        Dim cteam_type As New DataColumn
        cteam_type.DataType = Type.GetType("System.String")
        cteam_type.ColumnName = "type"

        Dim cteam_id As New DataColumn
        cteam_id.DataType = Type.GetType("System.String")
        cteam_id.ColumnName = "team_id"

        Dim cteam_naam As New DataColumn
        cteam_naam.DataType = Type.GetType("System.String")
        cteam_naam.ColumnName = "team_naam"

        Dim cvnaam As New DataColumn
        cvnaam.DataType = Type.GetType("System.String")
        cvnaam.ColumnName = "voornaam"

        Dim ctussenv As New DataColumn
        ctussenv.DataType = Type.GetType("System.String")
        ctussenv.ColumnName = "tussenv"

        Dim canaam As New DataColumn
        canaam.DataType = Type.GetType("System.String")
        canaam.ColumnName = "achternaam"

        Dim clidnummer As New DataColumn
        clidnummer.DataType = Type.GetType("System.String")
        clidnummer.ColumnName = "lidnummer"

        Dim cfoto As New DataColumn
        cfoto.DataType = Type.GetType("System.String")
        cfoto.ColumnName = "foto"

        Dim crol As New DataColumn
        crol.DataType = Type.GetType("System.String")
        crol.ColumnName = "rol"

        dtTeam.Columns.Add(cteam_type)
        dtTeam.Columns.Add(cteam_id)
        dtTeam.Columns.Add(cteam_naam)
        dtTeam.Columns.Add(cvnaam)
        dtTeam.Columns.Add(ctussenv)
        dtTeam.Columns.Add(canaam)
        dtTeam.Columns.Add(clidnummer)
        dtTeam.Columns.Add(cfoto)
        dtTeam.Columns.Add(crol)

        'Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/PlayerInfoService.asp?clubcode=HH11BZ2")
        Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/TeamsService.asp?clubcode=HH11BZ2")

        Dim teams = From team In resultXML.Descendants("team") _
                    Where (team.Attribute("id").Value = team_id.ToString)

        Dim spelers = From speler In teams.Descendants("as")
                      Select speler

        'add new row for each speler in xml

        For Each speler In spelers
            Dim row = dtTeam.NewRow

            row("team_id") = team_id.ToString

            If Not speler.Attribute("function") Is Nothing Then
                row("type") = speler.Attribute("function").Value
            End If

            If Not speler.Attribute("vnaam") Is Nothing Then
                row("voornaam") = speler.Attribute("vnaam").Value
            End If

            If Not speler.Attribute("tv") Is Nothing Then
                row("tussenv") = speler.Attribute("tv").Value
            End If

            If Not speler.Attribute("anaam") Is Nothing Then
                row("achternaam") = speler.Attribute("anaam").Value
            End If

            row("rol") = "ondersteuning"

            dtTeam.Rows.Add(row)
        Next

        Return dtTeam
    End Function
    Function getTeamList() As DataTable
        'Geeft een datatable terug met een lijst van Teams binnen de Club
        'params: url:http://hockey-xml.lisa-is.nl/xml/Teams2Service.asp?clubcode=HH11BZ2

        Dim dtList As New DataTable

        Dim cteam_id As New DataColumn
        cteam_id.DataType = Type.GetType("System.String")
        cteam_id.ColumnName = "id"

        Dim cteam_naam As New DataColumn
        cteam_naam.DataType = Type.GetType("System.String")
        cteam_naam.ColumnName = "naam"

        Dim cafkorting As New DataColumn
        cafkorting.DataType = Type.GetType("System.String")
        cafkorting.ColumnName = "afkorting"

        dtList.Columns.Add(cteam_id)
        dtList.Columns.Add(cteam_naam)
        dtList.Columns.Add(cafkorting)

        Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/Teams2Service.asp?clubcode=HH11BZ2")
        Dim team_list = From team In resultXML.Descendants("team")

        'add new row for each team in xml
        For Each team In team_list
            Dim row = dtList.NewRow

            If Not team.Attribute("id") Is Nothing Then
                row("id") = team.Attribute("id").Value
            End If

            If Not team.Attribute("naam") Is Nothing Then
                row("naam") = team.Attribute("naam").Value
            End If

            If Not team.Attribute("afkorting") Is Nothing Then
                row("afkorting") = team.Attribute("afkorting").Value
            End If

            dtList.Rows.Add(row)
        Next

        Return dtList
    End Function
    Function getTeamPouleStanden(ByVal team_id As Integer) As DataTable
        'Geeft een datatable terug met standen binnen een team poule
        'params: url:http://hockey-xml.lisa-is.nl/xml/StandenService.asp?clubcode=HH11BZ2

        Dim dtStanden As New DataTable

        Dim nummer As New DataColumn
        nummer.DataType = Type.GetType("System.String")
        nummer.ColumnName = "nummer"

        Dim iteam As New DataColumn
        iteam.DataType = Type.GetType("System.String")
        iteam.ColumnName = "team"

        Dim totalmatches As New DataColumn
        totalmatches.DataType = Type.GetType("System.String")
        totalmatches.ColumnName = "totalmatches"

        Dim totalpoints As New DataColumn
        totalpoints.DataType = Type.GetType("System.String")
        totalpoints.ColumnName = "totalpoints"

        Dim penaltypoints As New DataColumn
        penaltypoints.DataType = Type.GetType("System.String")
        penaltypoints.ColumnName = "penaltypoints"

        Dim won As New DataColumn
        won.DataType = Type.GetType("System.String")
        won.ColumnName = "won"

        Dim lost As New DataColumn
        lost.DataType = Type.GetType("System.String")
        lost.ColumnName = "lost"

        Dim draw As New DataColumn
        draw.DataType = Type.GetType("System.String")
        draw.ColumnName = "draw"

        Dim goalsfor As New DataColumn
        goalsfor.DataType = Type.GetType("System.String")
        goalsfor.ColumnName = "goalsfor"

        Dim goalsagainst As New DataColumn
        goalsagainst.DataType = Type.GetType("System.String")
        goalsagainst.ColumnName = "goalsagainst"

        Dim ownteam As New DataColumn
        ownteam.DataType = Type.GetType("System.String")
        ownteam.ColumnName = "ownteam"

        dtStanden.Columns.Add(nummer)
        dtStanden.Columns.Add(iteam)
        dtStanden.Columns.Add(totalmatches)
        dtStanden.Columns.Add(totalpoints)
        dtStanden.Columns.Add(penaltypoints)
        dtStanden.Columns.Add(won)
        dtStanden.Columns.Add(lost)
        dtStanden.Columns.Add(draw)
        dtStanden.Columns.Add(goalsfor)
        dtStanden.Columns.Add(goalsagainst)
        dtStanden.Columns.Add(ownteam)

        Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/StandenService.asp?clubcode=HH11BZ2")
        Dim poule = From team In resultXML.Descendants("team") _
                 Where (team.Attribute("id").Value = team_id.ToString)

        Dim standen = From stand In poule.Descendants("stand")
                      Select stand

        For Each stand In standen
            Dim row = dtStanden.NewRow

            If Not stand.Attribute("nummer") Is Nothing Then
                row("nummer") = stand.Attribute("nummer").Value
            End If

            If Not stand.Attribute("team") Is Nothing Then
                row("team") = stand.Attribute("team").Value
            End If

            If Not stand.Attribute("totalmatches") Is Nothing Then
                row("totalmatches") = stand.Attribute("totalmatches").Value
            End If

            If Not stand.Attribute("totalpoints") Is Nothing Then
                row("totalpoints") = stand.Attribute("totalpoints").Value
            End If

            If Not stand.Attribute("penaltypoints") Is Nothing Then
                row("penaltypoints") = stand.Attribute("penaltypoints").Value
            End If

            If Not stand.Attribute("won") Is Nothing Then
                row("won") = stand.Attribute("won").Value
            End If

            If Not stand.Attribute("lost") Is Nothing Then
                row("lost") = stand.Attribute("lost").Value
            End If

            If Not stand.Attribute("draw") Is Nothing Then
                row("draw") = stand.Attribute("draw").Value
            End If

            If Not stand.Attribute("goalsfor") Is Nothing Then
                row("goalsfor") = stand.Attribute("goalsfor").Value
            End If

            If Not stand.Attribute("goalsagainst") Is Nothing Then
                row("goalsagainst") = stand.Attribute("goalsagainst").Value
            End If

            If Not stand.Attribute("ownteam") Is Nothing Then
                row("ownteam") = stand.Attribute("ownteam").Value
            End If

            dtStanden.Rows.Add(row)
        Next
        Return dtStanden
    End Function

    Protected Sub btnSelect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSelect.Click
        'linken naar deze pagina met het id van het geselecteerde team
        Response.Redirect(String.Format("team.aspx?team={0}", ddTeamList.SelectedValue))
    End Sub
End Class