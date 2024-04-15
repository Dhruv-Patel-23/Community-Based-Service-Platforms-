﻿Imports System.Diagnostics.Eventing
Imports System.IO

Public Class Homepage_Admin


    Public Queries As DataTable = New DataTable()
    Public NotVerifiedSPs As DataTable = New DataTable()
    Public VerifiedSPs As DataTable = New DataTable()
    Private Sub RemovePreviousForm()
        ' Check if any form is already in Panel5
        If SessionManager.Panel3.Controls.Count > 0 Then
            ' Remove the first control (form) from Panel5
            SessionManager.Panel3.Controls.Clear()
        End If
    End Sub

    Private Sub RetrieveVerifiedSP()
        Dim connection As New MySqlConnection(SessionManager.connectionString)

        Try
            connection.Open()
            ' Connection established successfully

            Dim query As String = $"SELECT sp.userID,sp.serviceProviderName as name,sp.serviceProviderPhotos as photos,sp.serviceProviderdescription,sp.experienceYears,cd.location,cd.mobileNumber FROM serviceproviders as sp JOIN contactDetails as cd ON sp.userID = cd.userID WHERE registrationStatus = 'Approved'"
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

    Private Sub RetrieveNotVerifiedSP()
        Dim connection As New MySqlConnection(SessionManager.connectionString)

        Try
            connection.Open()
            ' Connection established successfully

            Dim query As String = $"SELECT sp.userID,sp.serviceProviderName as name, sp.serviceProviderPhotos as photos, sp.serviceProviderdescription,sp.experienceYears,cd.location,cd.mobileNumber FROM serviceproviders as sp JOIN contactDetails as cd ON sp.userID = cd.userID WHERE registrationStatus = 'Pending'"
            Dim command As New MySqlCommand(query, connection)

            Dim reader As MySqlDataReader = command.ExecuteReader()
            NotVerifiedSPs.Load(reader)
            reader.Close()

        Catch ex As Exception
            ' Handle connection errors
            MessageBox.Show("Error connecting to MySQL: " & ex.Message)
        End Try

    End Sub

    Private Sub RetrieveQueries()
        Dim connection As New MySqlConnection(SessionManager.connectionString)

        Try
            connection.Open()
            ' Connection established successfully

            Dim query As String = $"SELECT type AS QueryType, userID AS QueryBy, appointmentID as AppointmentID, queryDate AS QueryDate, description AS Description, status AS Status FROM AddressQueries as aq ORDER BY queryDate DESC;"
            Dim command As New MySqlCommand(query, connection)

            Dim reader As MySqlDataReader = command.ExecuteReader()
            Queries.Load(reader)
            reader.Close()

        Catch ex As Exception
            ' Handle connection errors
            MessageBox.Show("Error connecting to MySQL: " & ex.Message)
        Finally
            connection.Close()
        End Try

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        RemovePreviousForm()
        All_Queries.Margin = New Padding(10, 10, 10, 10)
        With All_Queries
            .TopLevel = False
            SessionManager.Panel3.Controls.Add(All_Queries)
            .BringToFront()
            .Show()
        End With
    End Sub
    Private Sub DataGridView1_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 AndAlso e.ColumnIndex <= 5 Then
            ' Draw cell border
            e.Paint(e.CellBounds, DataGridViewPaintParts.All)

            ' Draw right border
            Dim rectRight As New Rectangle(e.CellBounds.Right - 1, e.CellBounds.Top, 1, e.CellBounds.Height)
            e.Graphics.FillRectangle(Brushes.Black, rectRight)

            ' Draw bottom border
            Dim rectBottom As New Rectangle(e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Width, 1)
            e.Graphics.FillRectangle(Brushes.Black, rectBottom)

            e.Handled = True
        End If
    End Sub

    Private Sub Homepage_Admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Font = New Font(SessionManager.font_family, 9, FontStyle.Regular)
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font(SessionManager.font_family, 9, FontStyle.Regular)
        DataGridView1.DefaultCellStyle.Font = New Font(SessionManager.font_family, 9, FontStyle.Regular)
        Queries.Clear()
        VerifiedSPs.Clear()
        NotVerifiedSPs.Clear()
        RetrieveQueries()
        RetrieveNotVerifiedSP()
        RetrieveVerifiedSP()
        For i As Integer = 1 To Math.Min(6, Queries.Rows.Count)
            Dim ithRow As DataRow = Queries.Rows(i - 1)
            Dim queryType As String = ithRow("QueryType").ToString()
            Dim queryBy As String = ithRow("QueryBy").ToString()
            Dim appointmentID As String = ithRow("AppointmentID").ToString()
            Dim queryDate As String = ithRow("QueryDate").ToString()
            Dim description As String = ithRow("Description").ToString()
            Dim status As String = ithRow("Status").ToString()
            DataGridView1.Rows.Add(queryType, queryBy, appointmentID, queryDate, description, status)
        Next
        If VerifiedSPs.Rows.Count >= 1 Then
            Dim row As DataRow = VerifiedSPs.Rows(0)
            Label6.Text = row("name")
            If Not row.IsNull("photos") Then
                ' Retrieve the byte array from the "userPhoto" column
                Dim userPhoto As Byte() = DirectCast(row("photos"), Byte())

                ' Check if user photo is not null
                If userPhoto IsNot Nothing AndAlso userPhoto.Length > 0 Then
                    ' Convert byte array to image and display it
                    Using ms As New MemoryStream(userPhoto)
                        ' Set the PictureBox properties and display the image
                        PictureBox4.SizeMode = PictureBoxSizeMode.StretchImage
                        PictureBox4.Image = Image.FromStream(ms)
                    End Using
                Else
                    ' If user photo is null, set a default image or display a placeholder
                    PictureBox4.Image = My.Resources.Resource1.displayPicture
                End If
            End If
        End If

        If VerifiedSPs.Rows.Count >= 2 Then
            Dim row As DataRow = VerifiedSPs.Rows(1)
            Label7.Text = row("name")
            If Not row.IsNull("photos") Then
                ' Retrieve the byte array from the "userPhoto" column
                Dim userPhoto As Byte() = DirectCast(row("photos"), Byte())

                ' Check if user photo is not null
                If userPhoto IsNot Nothing AndAlso userPhoto.Length > 0 Then
                    ' Convert byte array to image and display it
                    Using ms As New MemoryStream(userPhoto)
                        ' Set the PictureBox properties and display the image
                        PictureBox3.SizeMode = PictureBoxSizeMode.StretchImage
                        PictureBox3.Image = Image.FromStream(ms)
                    End Using
                Else
                    ' If user photo is null, set a default image or display a placeholder
                    PictureBox3.Image = My.Resources.Resource1.displayPicture
                End If
            End If
        End If

        If NotVerifiedSPs.Rows.Count >= 1 Then
            Dim row As DataRow = NotVerifiedSPs.Rows(0)
            Label5.Text = row("name")
            If Not row.IsNull("photos") Then
                ' Retrieve the byte array from the "userPhoto" column
                Dim userPhoto As Byte() = DirectCast(row("photos"), Byte())

                ' Check if user photo is not null
                If userPhoto IsNot Nothing AndAlso userPhoto.Length > 0 Then
                    ' Convert byte array to image and display it
                    Using ms As New MemoryStream(userPhoto)
                        ' Set the PictureBox properties and display the image
                        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                        PictureBox1.Image = Image.FromStream(ms)
                    End Using
                Else
                    ' If user photo is null, set a default image or display a placeholder
                    PictureBox1.Image = My.Resources.Resource1.displayPicture
                End If
            End If
        End If

        If NotVerifiedSPs.Rows.Count >= 2 Then
            Dim row As DataRow = NotVerifiedSPs.Rows(1)
            Label4.Text = row("name")
            If Not row.IsNull("photos") Then
                ' Retrieve the byte array from the "userPhoto" column
                Dim userPhoto As Byte() = DirectCast(row("photos"), Byte())

                ' Check if user photo is not null
                If userPhoto IsNot Nothing AndAlso userPhoto.Length > 0 Then
                    ' Convert byte array to image and display it
                    Using ms As New MemoryStream(userPhoto)
                        ' Set the PictureBox properties and display the image
                        PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
                        PictureBox2.Image = Image.FromStream(ms)
                    End Using
                Else
                    ' If user photo is null, set a default image or display a placeholder
                    PictureBox2.Image = My.Resources.Resource1.displayPicture
                End If
            End If
        End If





        LinkLabel1.LinkBehavior = LinkBehavior.NeverUnderline
        LinkLabel2.LinkBehavior = LinkBehavior.NeverUnderline
        LinkLabel3.LinkBehavior = LinkBehavior.NeverUnderline
        PictureBox1.Size = New Size(140, 140)
        PictureBox2.Size = New Size(140, 140)
        PictureBox3.Size = New Size(140, 140)
        PictureBox4.Size = New Size(140, 140)
        PictureBox1.Image = My.Resources.Resource1.sample_SP
        PictureBox2.Image = My.Resources.Resource1.sample_SP
        PictureBox3.Image = My.Resources.Resource1.sample_SP
        PictureBox4.Image = My.Resources.Resource1.sample_SP
        PictureBox1.BackgroundImageLayout = ImageLayout.Stretch
        PictureBox2.BackgroundImageLayout = ImageLayout.Stretch
        PictureBox3.BackgroundImageLayout = ImageLayout.Stretch
        PictureBox4.BackgroundImageLayout = ImageLayout.Stretch

        ' Subscribe to the CellContentClick event of the DataGridView
        AddHandler DataGridView1.CellContentClick, AddressOf DataGridView1_CellContentClick
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        RemovePreviousForm()
        Registration_Request.Margin = New Padding(0, 0, 0, 0)
        With Registration_Request
            .TopLevel = False
            .Dock = DockStyle.Fill
            SessionManager.Panel3.Controls.Add(Registration_Request)
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        RemovePreviousForm()
        All_Service_Providers.Margin = New Padding(0, 0, 0, 0)
        With All_Service_Providers
            .TopLevel = False
            .Dock = DockStyle.Fill
            SessionManager.Panel3.Controls.Add(All_Service_Providers)
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        ' Check if the clicked cell is the "View" button column
        If e.ColumnIndex = DataGridView1.Columns("View").Index AndAlso e.RowIndex >= 0 Then
            ' Handle the "View" button click for the specific row
            Dim appointmentId As String = DataGridView1.Rows(e.RowIndex).Cells("AppointmentID").Value.ToString()
            ' You can perform actions based on the appointment ID, such as opening a new form or displaying details
            ' Check if a Reply_Query form is already open
            For Each form As Form In Application.OpenForms
                If TypeOf form Is Reply_Query Then
                    form.Close()
                    Exit For ' Exit loop once a form is closed
                End If
            Next

            Dim Reply As New Reply_Query()
            Reply.description = Queries.Rows(e.RowIndex)("Description")
            Reply.status = Queries.Rows(e.RowIndex)("Status")
            Reply.title = Queries.Rows(e.RowIndex)("QueryType")
            Reply.Show()
        End If
    End Sub


End Class