Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports System.Diagnostics
Imports System.Windows.Forms

Public Class Form1
    Inherits Form

    Private btnCreate As Button
    Private btnView As Button

    Public Sub New()
        Me.Text = "Nawaz Ali, Slidely Task 2 - Slidely Form App"
        Me.Size = New Size(400, 300)

        btnCreate = New Button()
        btnCreate.Text = "CREATE NEW SUBMISSION (CTRL + N)"
        btnCreate.Location = New Point(50, 150)
        btnCreate.Size = New Size(300, 50)
        AddHandler btnCreate.Click, AddressOf BtnCreate_Click
        Me.Controls.Add(btnCreate)

        btnView = New Button()
        btnView.Text = "VIEW SUBMISSIONS (CTRL + V)"
        btnView.Location = New Point(50, 50)
        btnView.Size = New Size(300, 50)
        AddHandler btnView.Click, AddressOf BtnView_Click
        Me.Controls.Add(btnView)

        ' Set up keyboard shortcuts
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf Form1_KeyDown
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.N Then
            BtnCreate_Click(Nothing, Nothing)
        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            BtnView_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub BtnCreate_Click(sender As Object, e As EventArgs)
        Dim createForm As New CreateSubmissionForm()
        createForm.ShowDialog()
    End Sub

    Private Sub BtnView_Click(sender As Object, e As EventArgs)
        Dim viewForm As New ViewSubmissionsForm()
        viewForm.ShowDialog()
    End Sub
End Class

Public Class CreateSubmissionForm
    Inherits Form

    Private txtName As TextBox
    Private txtEmail As TextBox
    Private txtPhone As TextBox
    Private txtGitHub As TextBox
    Private lblStopwatch As Label
    Private stopwatch As Stopwatch
    Private WithEvents btnStopwatch As Button
    Private btnSubmit As Button

    Public Sub New()
        Me.Text = "Nawaz Ali, Slidely Task 2 - Create Submission"
        Me.Size = New Size(400, 400)

        Dim lblName As New Label()
        lblName.Text = "Name:"
        lblName.Location = New Point(10, 10)
        Me.Controls.Add(lblName)

        txtName = New TextBox()
        txtName.Location = New Point(150, 10)
        Me.Controls.Add(txtName)

        Dim lblEmail As New Label()
        lblEmail.Text = "Email:"
        lblEmail.Location = New Point(10, 50)
        Me.Controls.Add(lblEmail)

        txtEmail = New TextBox()
        txtEmail.Location = New Point(150, 50)
        Me.Controls.Add(txtEmail)

        Dim lblPhone As New Label()
        lblPhone.Text = "Phone Num:"
        lblPhone.Location = New Point(10, 90)
        Me.Controls.Add(lblPhone)

        txtPhone = New TextBox()
        txtPhone.Location = New Point(150, 90)
        Me.Controls.Add(txtPhone)

        Dim lblGitHub As New Label()
        lblGitHub.Text = "Github Link For Task 2:"
        lblGitHub.Location = New Point(10, 130)
        Me.Controls.Add(lblGitHub)

        txtGitHub = New TextBox()
        txtGitHub.Location = New Point(150, 130)
        Me.Controls.Add(txtGitHub)

        lblStopwatch = New Label()
        lblStopwatch.Text = "00:00:00"
        lblStopwatch.Location = New Point(150, 170)
        lblStopwatch.Size = New Size(100, 30)
        Me.Controls.Add(lblStopwatch)

        btnStopwatch = New Button()
        btnStopwatch.Text = "TOGGLE STOPWATCH (CTRL + T)"
        btnStopwatch.Location = New Point(10, 170)
        AddHandler btnStopwatch.Click, AddressOf BtnStopwatch_Click
        Me.Controls.Add(btnStopwatch)

        btnSubmit = New Button()
        btnSubmit.Text = "SUBMIT (CTRL + S)"
        btnSubmit.Location = New Point(150, 210)
        AddHandler btnSubmit.Click, AddressOf BtnSubmit_Click
        Me.Controls.Add(btnSubmit)

        stopwatch = New Stopwatch()

        ' Set up keyboard shortcuts
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf CreateSubmissionForm_KeyDown
    End Sub

    Private Sub CreateSubmissionForm_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.T Then
            BtnStopwatch_Click(Nothing, Nothing)
        ElseIf e.Control AndAlso e.KeyCode = Keys.S Then
            BtnSubmit_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub BtnStopwatch_Click(sender As Object, e As EventArgs)
        If stopwatch.IsRunning Then
            stopwatch.Stop()
            btnStopwatch.Text = "Start Stopwatch"
        Else
            stopwatch.Start()
            btnStopwatch.Text = "Stop Stopwatch"
        End If
        UpdateStopwatchLabel()
    End Sub

    Private Sub UpdateStopwatchLabel()
        lblStopwatch.Text = stopwatch.Elapsed.ToString("hh\:mm\:ss")
    End Sub

    Private Async Sub BtnSubmit_Click(sender As Object, e As EventArgs)
        Dim client As New HttpClient()
        Dim data = New With {
            Key .name = txtName.Text,
            Key .email = txtEmail.Text,
            Key .phone = txtPhone.Text,
            Key .github_link = txtGitHub.Text,
            Key .stopwatch_time = stopwatch.Elapsed.ToString("hh\:mm\:ss")
        }
        Dim content As StringContent = New StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
        Dim response As HttpResponseMessage = Await client.PostAsync("http://localhost:3000/submit", content)

        If response.IsSuccessStatusCode Then
            MessageBox.Show("Submission successful!")
            Me.Close()
        Else
            MessageBox.Show("Submission failed.")
        End If
    End Sub
End Class

Public Class ViewSubmissionsForm
    Inherits Form

    Private lblSubmission As Label
    Private currentIndex As Integer
    Private submissions As List(Of Object)
    Private btnPrevious As Button
    Private btnNext As Button

    Public Sub New()
        Me.Text = "Nawaz Ali, Slidely Task 2 - View Submissions"
        Me.Size = New Size(400, 300)

        lblSubmission = New Label()
        lblSubmission.Location = New Point(10, 10)
        lblSubmission.Size = New Size(360, 200)
        Me.Controls.Add(lblSubmission)

        btnPrevious = New Button()
        btnPrevious.Text = "PREVIOUS (CTRL + P)"
        btnPrevious.Location = New Point(10, 220)
        AddHandler btnPrevious.Click, AddressOf BtnPrevious_Click
        Me.Controls.Add(btnPrevious)

        btnNext = New Button()
        btnNext.Text = "NEXT (CTRL + N)"
        btnNext.Location = New Point(200, 220)
        AddHandler btnNext.Click, AddressOf BtnNext_Click
        Me.Controls.Add(btnNext)

        currentIndex = 0
        LoadSubmissions()

        ' Set up keyboard shortcuts
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf ViewSubmissionsForm_KeyDown
    End Sub

    Private Sub ViewSubmissionsForm_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.P Then
            BtnPrevious_Click(Nothing, Nothing)
        ElseIf e.Control AndAlso e.KeyCode = Keys.N Then
            BtnNext_Click(Nothing, Nothing)
        End If
    End Sub

    Private Async Sub LoadSubmissions()
        Dim client As New HttpClient()
        Dim response As HttpResponseMessage = Await client.GetAsync("http://localhost:3000/read?index=" & currentIndex)

        If response.IsSuccessStatusCode Then
            Dim submission As Object = JsonConvert.DeserializeObject(Of Object)(Await response.Content.ReadAsStringAsync())
            submissions = New List(Of Object)({submission})
            DisplaySubmission()
        Else
            MessageBox.Show("Failed to load submissions.")
        End If
    End Sub

    Private Sub DisplaySubmission()
        If submissions IsNot Nothing AndAlso submissions.Count > 0 Then
            Dim submission = submissions(currentIndex)
            lblSubmission.Text = $"Name: {submission("name")}" & vbCrLf &
                                 $"Email: {submission("email")}" & vbCrLf &
                                 $"Phone: {submission("phone")}" & vbCrLf &
                                 $"GitHub: {submission("github_link")}" & vbCrLf &
                                 $"Time: {submission("stopwatch_time")}"
        Else
            lblSubmission.Text = "No submissions found."
        End If
    End Sub

    Private Sub BtnPrevious_Click(sender As Object, e As EventArgs)
        If submissions IsNot Nothing AndAlso currentIndex > 0 Then
            currentIndex -= 1
            DisplaySubmission()
        End If
    End Sub

    Private Sub BtnNext_Click(sender As Object, e As EventArgs)
        If submissions IsNot Nothing AndAlso currentIndex < submissions.Count - 1 Then
            currentIndex += 1
            DisplaySubmission()
        End If
    End Sub
End Class

