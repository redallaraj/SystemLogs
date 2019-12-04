Imports System.Runtime.InteropServices
Imports System.Net.NetworkInformation
Imports System.Net
Public Class Form1
    Dim srv As New srv.Service1Client

    <StructLayout(LayoutKind.Sequential)>
    Structure LASTINPUTINFO
        <MarshalAs(UnmanagedType.U4)>
        Public cbSize As Integer
        <MarshalAs(UnmanagedType.U4)>
        Public dwTime As Integer
    End Structure
    <DllImport("user32.dll")>
    Shared Function GetLastInputInfo(ByRef plii As LASTINPUTINFO) As Boolean
    End Function

    Dim idletime As Integer
    Dim lastInputInf As New LASTINPUTINFO()

    Public Function GetLastInputTime() As Integer
        idletime = 0
        lastInputInf.cbSize = Marshal.SizeOf(lastInputInf)
        lastInputInf.dwTime = 0
        If GetLastInputInfo(lastInputInf) Then
            idletime = Environment.TickCount - lastInputInf.dwTime
        End If
        If idletime > 0 Then
            Return idletime / 1000
        Else : Return 0
        End If
    End Function

    Private LastLastIdletime As Integer = 0
    Public countMouseMove As Integer = 0
    Dim ip As String
    Dim machineId As String
    Dim pcName As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tm.Enabled = True
        tm.Start()
        tm2.Enabled = True
        tm2.Start()
        Data()
        Me.Hide()
        Me.ShowInTaskbar = False
        NotifyIcon1.ShowBalloonTip(500000, "Click", "Programi po punon ne background !", ToolTipIcon.Info)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        ShowInTaskbar = False
    End Sub

    Private Sub tm_Tick(sender As Object, e As EventArgs) Handles tm.Tick
        Dim it As Integer = GetLastInputTime()
        If LastLastIdletime > it Then
            Label1.Text = "Statusi ndryshoi !"
            countMouseMove += 1
            'sumofidletime = sumofidletime.Add(TimeSpan.FromSeconds(LastLastIdletime))
            'Label2.Text = "Totali i kohes inaktive : " & sumofidletime.ToString
            Label3.Text = "Heret qe mouse ka levizur : " & countMouseMove.ToString
        Else
            Label1.Text = GetLastInputTime()
        End If
        LastLastIdletime = it
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Hide()
        Me.ShowInTaskbar = False
    End Sub

    Public Sub Data()
        Dim mac As String
        Dim nics() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces
        mac = nics(0).GetPhysicalAddress.ToString
        txt_mac.Text = mac
        machineId = mac
        pcName = Dns.GetHostName
        txt_pc.Text = pcName
        Dim h As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName)
        For Each add As IPAddress In h.AddressList
            If add.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                ip = h.AddressList(1).ToString
                txt_ip.Text = ip
            End If
        Next
    End Sub

    Private Sub tm2_Tick(sender As Object, e As EventArgs) Handles tm2.Tick
        Try
            Dim c As Boolean
            c = srv.insertPc(ip, machineId, countMouseMove)
            countMouseMove = 0
        Catch ex As Exception
            countMouseMove = 0
            Try
                srv.insErr(ip, ex.Message)
            Catch ex1 As Exception
                MessageBox.Show("ERROR !")
            End Try
        End Try
    End Sub
End Class
