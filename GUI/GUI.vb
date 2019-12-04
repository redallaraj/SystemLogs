Imports System.Net.NetworkInformation
Imports System.Net
Imports System.IO
Public Class GUI
    Private Sub GUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim nics() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces
        Dim mac As String
        mac = nics(0).GetPhysicalAddress.ToString
        txt_machineid.Text = mac
        Dim h As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName)
        Dim ip As String
        For Each add As IPAddress In h.AddressList
            If add.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                ip = h.AddressList(1).ToString
                txt_ip.Text = ip
            End If
        Next
    End Sub

    Public Sub fnCreateKey(ByVal sTreeKey As String, ByVal key As String)
        Dim regVersion As Microsoft.Win32.RegistryKey
        regVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(sTreeKey, True)
        If regVersion Is Nothing Then
            regVersion = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(sTreeKey)
        End If
        regVersion.SetValue("System Log", key)
        regVersion.Close()
    End Sub
    Public Function getKeyReg(ByVal sTreeKey As String, ByVal key As String) As String
        Dim noSuch As String = ""
        Dim regVersion As Microsoft.Win32.RegistryKey
        regVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(sTreeKey, False)
        Dim intVersion As Integer = 0
        If (Not regVersion Is Nothing) Then
            noSuch = regVersion.GetValue(key, 0)
            regVersion.Close()
        End If
        Return noSuch
    End Function


    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        Dim srv As New srv.Service1Client
        Dim username As String = txt_username.Text
        Dim fullname As String = txt_fullname.Text
        Dim ip As String = txt_ip.Text
        Dim machineId As String = txt_machineid.Text
        Dim test As Boolean
        Try

            srv.Open()
            test = srv.registerUser(username, fullname, ip, machineId)
            If test = False Then
                MessageBox.Show("Gabim !", "Njoftim !")

            Else
                MessageBox.Show("Te dhenat u kaluan me sukses !", "Njoftim !")
            End If

            fnCreateKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "C:\logsystem\LOG.exe")
            getKeyReg("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "System Log")

            Dim appPath As String = My.Application.Info.DirectoryPath
            Dim myDir As String = Path.GetDirectoryName(appPath)
            ' Dim pathOfExe2 As String = Path.Combine(myDir, "LOG.exe")
            Dim pathofExe2 As String = myDir & "\" & "LOG.exe"

            If (Not Directory.Exists("C:\logsystem")) Then
                Directory.CreateDirectory("C:\logsystem")
                My.Computer.FileSystem.CopyFile(appPath & "\" & "LOG.exe",
           "C:\logsystem\LOG.exe")
                My.Computer.FileSystem.CopyFile(appPath & "\" & "LOG.exe.config",
                     "C:\logsystem\LOG.exe.config")
            Else
                File.Copy(appPath & "\" & "LOG.exe", "C:\logsystem\LOG.exe", True)
                File.Copy(appPath & "\" & "LOG.exe.config", "C:\logsystem\LOG.exe.config", True)
            End If
            Process.Start("C:\logsystem\LOG.exe")
            srv.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub


End Class