Imports System.IO
Imports System.Net
Imports System.Reflection

Partial Public Class Form1
    Public Sub CommandLineSupport()
        Dim FileName As String
        Dim OverWorld As String
        Dim UnderWorld As String
        If My.Application.CommandLineArgs.Count > 1 Then
            Try
                FileName = My.Application.CommandLineArgs(1) & "\" & TextBox9.Text
                OverWorld = FileName & "-0.PNG"
                UnderWorld = FileName & "-1.PNG"
                LoadLevel(My.Application.CommandLineArgs(0))
                Form2?.P?.Image?.Save(OverWorld, Imaging.ImageFormat.Png)
                Form3?.P?.Image?.Save(UnderWorld, Imaging.ImageFormat.Png)
            Catch ex As Exception
            End Try
            Environment.Exit(0)
        End If
    End Sub
    Public Sub DeMap2(P1 As String, P2 As String)
        Dim info As ProcessStartInfo
        info = New ProcessStartInfo With {
            .FileName = PT & "\MAP\D.EXE",
            .Arguments = P1 & " " & P2,
            .UseShellExecute = False
        }
        Dim Proc As Process
        Try
            Proc = Process.Start(info)
            Proc.WaitForExit(60000)
            If Proc.HasExited = False Then
                Proc.Kill()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub LoadLevel(id As String)
        Dim EncryptFile As String
        Dim DatFile As String
        Label2.Text = "SMM2Viewer (modified by bilago) v010"
        TextBox9.Text = id
        EncryptFile = PT & "\MAP\" & id
        TextBox1.Text = EncryptFile
        DatFile = EncryptFile & ".DAT"
        SetStatus(Label2, "Starting Level loading")
        If id.Length = 11 Then
            If Not File.Exists(EncryptFile) Then
                If Not File.Exists(DatFile) Then
                    SetStatus(Label2, "Downloading the file")
                    DownloadFile(id, DatFile)
                Else
                    SetStatus(Label2, "File already downloaded")
                End If
                Label2.Text += vbCrLf & Date.Now.ToString & " Decoding file"
                DeMap2(DatFile, EncryptFile)
                If Not File.Exists(DatFile) Then
                    SetStatus(Label2, "Could not download the file")
                    Return
                End If
                If Not File.Exists(EncryptFile) Then
                    SetStatus(Label2, "Could not decode the file")
                    Return
                End If
            Else
                SetStatus(Label2, "Decoded level found")
            End If
            SetStatus(Label2, "Reading File")
            isMapIO = True
            RefPic()
        Else
            SetStatus(Label2, "level code isn't valid (LEV-ELC-0DE)")
            Return
        End If
    End Sub
    Private Sub SetStatus(label As Label, status As String)
        label.Text += vbCrLf & Date.Now.ToString & " " & status
        label.Invalidate()
        label.Update()
        label.Refresh()
        Application.DoEvents()
    End Sub
    Public Sub DownloadFile(level As String, filename As String)
        Dim url As String
        url = "https://tgrcode.com/mm2/level_data/" & level
        If File.Exists(filename) Then
            File.Delete(filename)
        End If
        Using client As New WebClient()
            client.DownloadFile(url, filename)
        End Using
    End Sub

    Public Sub LoadEFileEnglish(IO As Boolean)

        LoadLvlData(TextBox1.Text, IO)
        If IO Then
            Label2.Text += vbCrLf & vbCrLf
            Label2.Text += "Level Name：" & LH.Name & vbCrLf
            Label2.Text += "Description：" & LH.Desc & vbCrLf
            Label2.Text += "Timer Length：" & LH.Timer & vbCrLf
            Label2.Text += "Style：" & LH.GameStyle & vbCrLf
            Label2.Text += "Version：" & LH.GameVer & vbCrLf
            Label2.Text += "Start：" & LH.StartY & vbCrLf
            Label2.Text += "Finish：" & LH.GoalX & "," & LH.GoalY & vbCrLf
        End If

        Label2.Text += "======World Info======" & vbCrLf
        Label2.Text += "Theme：" & MapHdr.Theme & vbCrLf
        Label2.Text += "Width：" & MapHdr.BorR & vbCrLf
        Label2.Text += "Height：" & MapHdr.BorT & vbCrLf
        Label2.Text += "Bricks：" & MapHdr.GroundCount & vbCrLf
        Label2.Text += "Entities：" & MapHdr.ObjCount & vbCrLf
        Label2.Text += "Tracks：" & MapHdr.TrackCount & vbCrLf
        Label2.Text += "Autoscroll：" & MapHdr.AutoscrollType & vbCrLf
        Label2.Text += "Water Height：" & MapHdr.LiqSHeight & "-" & MapHdr.LiqEHeight & vbCrLf

        Dim LInfo() As FieldInfo
        Dim I As FieldInfo
        If IO Then
            LInfo = LH.GetType.GetFields()
            TextBox2.Text = ""
            For Each I In LInfo
                TextBox2.Text += I.Name & ":" & I.GetValue(LH) & vbCrLf
            Next
            TextBox3.Text = "===M0===" & vbCrLf
            '表世界H200长度2DEE0
            LInfo = MapHdr.GetType.GetFields()
            For Each I In LInfo
                TextBox3.Text += I.Name & ":" & I.GetValue(MapHdr).ToString & vbCrLf
            Next
        Else
            TextBox4.Text = "===M1===" & vbCrLf
            '表世界H200长度2DEE0
            LInfo = MapHdr.GetType.GetFields()
            For Each I In LInfo
                TextBox4.Text += I.Name & ":" & I.GetValue(MapHdr).ToString & vbCrLf
            Next
        End If


    End Sub
End Class
