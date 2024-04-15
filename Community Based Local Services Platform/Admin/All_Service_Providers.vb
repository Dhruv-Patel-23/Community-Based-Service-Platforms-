﻿
Imports System.IO

Public Class All_Service_Providers
    Public VerifiedSPs As DataTable = New DataTable()
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configure the form
        Me.CenterToParent()
        Me.WindowState = FormWindowState.Normal
        Me.Size = New Size(1200, 700)
        Dim Header As New Label()
        Header.Text = "Service Providers"
        Header.Location = New Point(97, 101)
        Header.AutoSize = True
        Header.Font = New Font(SessionManager.font_family, 10, FontStyle.Regular)
        Me.Controls.Add(Header)
        Me.BackColor = ColorTranslator.FromHtml("#FFFFFF")
        ' Create and configure the FlowLayoutPanel
        Dim Main_Panel As New Panel()
        Main_Panel.Size = New Size(800, 430)
        Main_Panel.Location = New Point(98, 138)
        Main_Panel.AutoScroll = True
        Me.Controls.Add(Main_Panel)
        RetrieveVerifiedSP()
        ' Create some sample cards
        For i As Integer = 1 To VerifiedSPs.Rows.Count ' Adjust the number of cards for demonstration
            ' Dim card As New Card("pic.png", "Service Provider Name", "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.", "Location", "Contact", "Experience", "Services from 09:00 AM to 05:00 PM  ")
            Dim ithRow As DataRow = VerifiedSPs.Rows(i - 1)
            Dim card As New Panel()
            card.Size = New Size(734, 198)
            card.BorderStyle = BorderStyle.FixedSingle
            Dim pictureBox As New PictureBox()
            pictureBox.Size = New Size(169, 157)
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom
            pictureBox.Location = New Point(21, 21)
            pictureBox.Image = My.Resources.Resource1.sample_SP
            Dim label1 As New Label()
            label1.Text = ithRow("serviceProviderName").ToString()
            label1.AutoSize = False
            label1.Size = New Size(280, 28)
            label1.Location = New Point(208, 22)
            label1.Font = New Font(SessionManager.font_family, 13, FontStyle.Regular)
            label1.ForeColor = Color.FromArgb(0, 0, 0)

            Dim label2 As New Label()
            label2.Text = ithRow("serviceProviderdescription").ToString()
            label2.AutoSize = False
            label2.Size = New Size(490, 47)
            label2.Location = New Point(208, 99)
            label2.Font = New Font(SessionManager.font_family, 7, FontStyle.Regular)
            label2.ForeColor = Color.FromArgb(0, 0, 0)

            Dim label3 As New Label()
            label3.Text = ithRow("location").ToString()
            label3.AutoSize = False
            label3.Size = New Size(73, 28)
            label3.Location = New Point(208, 50)
            label3.Font = New Font(SessionManager.font_family, 10, FontStyle.Regular)
            label3.ForeColor = Color.FromArgb(0, 0, 0)

            Dim ellipsePanel3 As New Panel()
            ellipsePanel3.Size = New Size(5, 5)
            ellipsePanel3.Location = New Point(295, 59)
            ellipsePanel3.BackColor = Color.Black

            Dim label4 As New Label()
            label4.Text = ithRow("mobileNumber").ToString()
            label4.AutoSize = False
            label4.Size = New Size(94, 28)
            label4.Location = New Point(314, 50)
            label4.Font = New Font(SessionManager.font_family, 10, FontStyle.Regular)
            label4.ForeColor = Color.FromArgb(0, 0, 0)

            Dim ellipsePanel4 As New Panel()
            ellipsePanel4.Size = New Size(5, 5)
            ellipsePanel4.Location = New Point(405, 59)
            ellipsePanel4.BackColor = Color.Black

            Dim label5 As New Label()
            label5.Text = ithRow("experienceYears").ToString() & " years"
            label5.AutoSize = False
            label5.Size = New Size(112, 28)
            label5.Location = New Point(424, 50)
            label5.Font = New Font(SessionManager.font_family, 10, FontStyle.Regular)
            label5.ForeColor = Color.FromArgb(0, 0, 0)

            Dim label6 As New Label()
            label6.Text = "Services from 09:00 AM to 05:00 PM"
            label6.AutoSize = False
            label6.Size = New Size(268, 14)
            label6.Location = New Point(208, 76)
            label6.Font = New Font(SessionManager.font_family, 7, FontStyle.Regular)
            label6.ForeColor = Color.FromArgb(0, 0, 0)





            card.Controls.Add(pictureBox)
            card.Controls.Add(label1)
            card.Controls.Add(label2)
            card.Controls.Add(label3)
            'card.Controls.Add(ellipsePanel3)
            card.Controls.Add(label4)
            'card.Controls.Add(ellipsePanel4)
            card.Controls.Add(label5)
            card.Controls.Add(label6)

            card.Location = New Point(0, (i - 1) * 224)
            Main_Panel.Controls.Add(card)
        Next
    End Sub
    Private Sub RetrieveVerifiedSP()
        Dim connection As New MySqlConnection(SessionManager.connectionString)

        Try
            connection.Open()
            ' Connection established successfully

            Dim query As String = $"SELECT sp.userID,sp.serviceProviderName,sp.serviceProviderdescription,sp.experienceYears,cd.location,cd.mobileNumber FROM serviceproviders as sp JOIN contactDetails as cd ON sp.userID = cd.userID WHERE registrationStatus = 'Approved'"
            Dim command As New MySqlCommand(query, connection)

            Dim reader As MySqlDataReader = command.ExecuteReader()
            VerifiedSPs.Load(reader)
            reader.Close()

        Catch ex As Exception
            ' Handle connection errors
            MessageBox.Show("Error connecting to MySQL: " & ex.Message)
        Finally
            connection.Close()
        End Try

    End Sub

End Class