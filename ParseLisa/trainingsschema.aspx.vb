Public Class trainingsschema
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblTrainingDag1.Text = getTrainingSchemaTable(1)
        lblTrainingDag2.Text = getTrainingSchemaTable(2)
        lblTrainingDag3.Text = getTrainingSchemaTable(3)
        lblTrainingDag4.Text = getTrainingSchemaTable(4)
        lblTrainingDag5.Text = getTrainingSchemaTable(5)
        lblNow.Text = Now.ToLongDateString
    End Sub

    Function getTrainingSchemaTable(ByVal dagnummer As Integer) As String
        'Geeft alle trainingen voor deze week
        'params: url:http://hockey-xml.lisa-is.nl/xml/TrainingsService.asp?clubcode=HH11BZ2

        Dim dag_s As String = ""
        Select Case dagnummer.ToString
            Case "1"
                dag_s = "maandag"
            Case "2"
                dag_s = "dinsdag"
            Case "3"
                dag_s = "woensdag"
            Case "4"
                dag_s = "donderdag"
            Case "5"
                dag_s = "vrijdag"
        End Select

        Dim table As String = "<table class=""trainingsdag"" id=""traingingsdag" & dagnummer & """>"
        table += "<tr class=""header""><td colspan=""5"">&nbsp;" & dag_s & "</td></tr>"
        table += "<tr>"
        table += "<td class=""td_tijd bold"">Tijd</td>"
        table += "<td class=""td_team bold"">Team</td>"
        table += "<td class=""td_trainer bold"">Trainer</td>"
        table += "<td class=""td_veld bold"">Veld</td>"
        table += "<td class=""td_deel bold"">Deel</td>"
        table += "</tr>"

        Dim resultXML As XDocument = XDocument.Load("http://hockey-xml.lisa-is.nl/xml/TrainingsService.asp?clubcode=HH11BZ2")
        Dim trainingsdag = From dag In resultXML.Descendants("dag") _
                     Where (dag.Attribute("number").Value = dagnummer.ToString)
                             Select dag

        Dim trainingen = From training In trainingsdag.Descendants("training") _
                         Order By CDate(training.Attribute("start")) Ascending _
                         Select training

        For Each training In trainingen
            Dim trainer = training.Descendants("trainer").First
            Dim tr As String = "<tr>"
            tr += "<td class=""td_tijd"">" & training.Attribute("start").Value & " - " & training.Attribute("einde").Value & "</td>"
            tr += "<td class=""td_team"">" & training.Attribute("team").Value & "</td>"
            '            tr += "<td class=""td_team""><a href=""/team.aspx?team=" & training.Attribute("team_id").Value & """>" & training.Attribute("team").Value & "</a></td>"


            If Not trainer.Attribute("vnaam") Is Nothing Then
                tr += "<td class=""td_trainer"">" & trainer.Attribute("vnaam").Value
                If Not trainer.Attribute("tv") Is Nothing Then
                    tr += " " & trainer.Attribute("tv").Value
                End If
                tr += " " & trainer.Attribute("anaam").Value
                tr += "</td>"
            Else
                tr += "<td class=""td_trainer"">&nbsp;</td>"
            End If

            tr += "<td class=""td_veld"">" & training.Attribute("locatie_omschrijving").Value & "</td>"

            If Not training.Attribute("deelvanlocatie") Is Nothing Then
                Select Case training.Attribute("deelvanlocatie").Value.ToString
                    Case "1"
                        tr += "<td class=""td_deel"">A</td>"
                    Case "2"
                        tr += "<td class=""td_deel"">B</td>"
                    Case "3"
                        tr += "<td class=""td_deel"">AB</td>"
                End Select
            End If

            tr += "</tr>"
            table += tr
        Next
        Return table
    End Function
End Class