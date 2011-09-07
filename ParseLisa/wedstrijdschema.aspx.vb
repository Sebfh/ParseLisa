Public Class wedstrijdschema
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            binddata()
        End If
    End Sub
    Private Sub binddata()
        'dropdownlist vullen met speeldagen
        For Each item In getWedstrijden()
            Dim ddlItem As New ListItem
            Dim datum() As String = item.ToString.Split("-")
            Dim longdate As New Date(datum(0), datum(1), datum(2))
            ddlItem.Value = item.ToString
            ddlItem.Text = longdate.ToLongDateString

            ddlUpcoming.Items.Add(ddlItem)
        Next

        If Not Request.QueryString.Get("dag") Is Nothing Then
            ddlUpcoming.SelectedValue = Request.QueryString.Get("dag")

            'table met wedsctrijden maken voor deze datum
            Dim selected() As String = Request.QueryString.Get("dag").Split("-")
            Dim geselecteerde_dag As New Date(selected(0), selected(1), selected(2))

            lblWedstrijden.Text = getWedstrijdenForDate(geselecteerde_dag)
        Else
            If ddlUpcoming.Items.Count > 1 Then
                'er zijn dagen geplant
                Dim str() As String = ddlUpcoming.SelectedValue().Split("-")
                Dim default_dag As New Date(str(0), str(1), str(2))
                lblWedstrijden.Text = getWedstrijdenForDate(default_dag)
            Else
                'er zijn geen aankomende dagen
                Dim table As String = "<table cellspacing=""0"" class=""wedstrijdschema"">"
                table += "<tr><td>Geen wedstrijden gevonden</td></tr>"
                table += "</table>"
                lblWedstrijden.Text = table
            End If
           
        End If
    End Sub
    Function getWedstrijdenForDate(ByVal datum As Date) As String
        Dim table As String = "<table cellspacing=""0"" class=""wedstrijdschema"">"
        table += "<tr class=""header""><td>Team</td><td>Tegen</td><td>Nr</td><td>Tijd</td><td>Locatie</td></tr>"
        Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/FutureMatchesService.asp?clubcode=HH11BZ2")

        Dim wedstrijden = From wedstrijd In resultXML.Descendants("wedstrijd") _
                            Where (wedstrijd.Attribute("wedstrijd_datum").Value = String.Format("{0}-{1}-{2}", datum.Year, Right("0" & datum.Month, 2), Right("0" & datum.Day, 2)))
                            Select wedstrijd

        For Each game In wedstrijden
            Dim tr As String = "<tr class=""game_row"">"

            If Not game.Attribute("club_team") Is Nothing Then
                tr += "<td>" & game.Attribute("club_team").Value & "</td>"
            Else
                tr += "<td>&nbsp;</td>"
            End If

            If Not game.Attribute("opponent_club_naam") Is Nothing And Not game.Attribute("opponent_team") Is Nothing Then
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

        table += "</table>"

        Return table
    End Function
    Function getWedstrijden() As ArrayList
        'genereerd een arraylist met aankomende speeldagen
        Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/FutureMatchesService.asp?clubcode=HH11BZ2")

        Dim wedstrijddagen = From wedstrijd In resultXML.Descendants("wedstrijd")
                            Select wedstrijd

        Dim dagen As New ArrayList

        For Each result In wedstrijddagen
            If Not dagen.Contains(result.Attribute("wedstrijd_datum").Value) Then
                dagen.Add(result.Attribute("wedstrijd_datum").Value)
            End If
        Next

        Return dagen

    End Function

    Protected Sub ddlUpcoming_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlUpcoming.SelectedIndexChanged
        'als er een dage gekozen wordt dan wedstrijden updaten voor nieuwe datum
        Response.Redirect(String.Format("/wedstrijdschema.aspx?dag={0}", ddlUpcoming.SelectedValue))
    End Sub
End Class