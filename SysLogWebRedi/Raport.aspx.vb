Imports System.Web.Services
Imports Oracle.DataAccess.Client

Public Class Raport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Public Shared cs As String = ConfigurationManager.ConnectionStrings("CS").ConnectionString

    <WebMethod()>
    Public Shared Function afishoDitor() As String
        Dim nr As Integer = 1
        Dim shtml As String = ""
        Try
            Dim con As OracleConnection = New OracleConnection(cs)
            Dim query As String = "Select p.Full_Name, p.IP, TO_CHAR(Min(logp.DT_SYS) - INTERVAL '5' MINUTE, 'DD/MM/YYYY HH24:MI') AS DT_START,
                                    TO_CHAR(Max(logp.DT_SYS), 'DD/MM/YYYY HH24:MI') AS DT_END, NVL(ORET_PREZENTE,0) AS ORET_PREZENTE,
                                    TRUNC(Count(distinct TO_CHAR(logp.dt_sys,'DD-MM-YYYY HH24:MI'))/12 * 3600) As ORET_AKTIVE
                                    From PC p , (
                                                           Select COUNT(DISTINCT TO_CHAR(DT_SYS, 'DD-MM-YYYY HH24')) As ORET_PREZENTE, IP From LOG_PC
                                                           Where  MOUSE_MOVE > 5 AND TO_CHAR(DT_SYS, 'DD-MM-YYYY') = TO_CHAR(sysdate, 'DD-MM-YYYY')
                                                           AND DT_SYS BETWEEN TO_DATE(to_char(sysdate, 'DD-MM-YYYY') || ' 08:30', 'DD-MM-YYYY HH24:MI') AND TO_DATE(to_char(sysdate, 'DD-MM-YYYY')  || ' 22:00', 'DD-MM-YYYY HH24:MI')
                       
                                                           AND NOT EXISTS(select 1 from ref_pushimet where dt_pushim = to_char(sysdate, 'DD-MM-YYYY'))
                                                           AND TO_CHAR(DT_SYS, 'DD-MM-YYYY HH24:MI') BETWEEN TO_CHAR(DT_SYS, 'DD-MM-YYYY HH24') || ':30' AND TO_CHAR(DT_SYS, 'DD-MM-YYYY HH24') || ':59'
                                                           Group By IP
                                                       ) l, 
                                                      ( SELECT * FROM  log_pc WHERE 
                                                        MOUSE_MOVE > 5 And TO_CHAR(DT_SYS, 'DD-MM-YYYY') = TO_CHAR(sysdate, 'DD-MM-YYYY') 
                                                        AND DT_SYS BETWEEN TO_DATE(to_char(sysdate, 'DD-MM-YYYY') || ' 08:30', 'DD-MM-YYYY HH24:MI') AND TO_DATE(to_char(sysdate, 'DD-MM-YYYY')  || ' 22:00', 'DD-MM-YYYY HH24:MI')
                    
                                                        AND NOT EXISTS(select 1 from ref_pushimet where dt_pushim = to_char(sysdate, 'DD-MM-YYYY')) ) logp
                                                        
                                    Where  p.IP = l.IP (+) AND P.ip =  logp.ip (+) 
                                    Group By ORET_PREZENTE, p.IP, p.Full_Name, NVL(ORET_PREZENTE,0) "
            Dim cmd As OracleCommand = New OracleCommand(query, con)
            cmd.Connection.Open()
            Dim da As OracleDataAdapter = New OracleDataAdapter(cmd)
            Dim dt As DataTable = New DataTable()
            da.Fill(dt)
            Dim time As TimeSpan
            shtml = "<table class='table table-bordered' id='tabela'>"
            shtml &= "<thead> <tr>
              <th scope='col'>#</th>
              <th scope='col'>PERDORUES</th>
              <th scope='col'>IP</th>
              <th scope='col'>DATA_FILLIM</th>
              <th scope='col'>DATA_MBARIM</th>
              <th scope='col'>ORET PREZENTE</th>
              <th scope='col'>ORET AKTIVE</th>
            </tr></thead><tbody>"
            For Each dr As DataRow In dt.Rows
                shtml &= "<tr id = '" & dr("IP").ToString() & "'>"
                shtml &= "<td>" & nr & "</td>"
                shtml &= "<td style = 'text-align:left;'>" & dr("FULL_NAME").ToString() & "</td>"
                shtml &= "<td style = 'text-align:left;'>" & dr("IP").ToString() & "</td>"
                shtml &= "<td>" & dr("DT_START").ToString() & "</td>"
                shtml &= "<td>" & dr("DT_END").ToString() & "</td>"
                shtml &= "<td>" & Math.Round(dr("ORET_PREZENTE"), 1) & "</td>"
                time = TimeSpan.FromSeconds(dr("ORET_AKTIVE"))
                shtml &= "<td>" & time.ToString("hh\:mm") & "</td>"
                shtml &= "</tr>"
                nr += 1
            Next
            shtml &= "</tbody>"
            shtml &= "</table>"
            cmd.Connection.Close()
            con.Dispose()
            cmd.Dispose()
        Catch ex As Exception
            shtml = "Error:" & ex.Message
        End Try
        Return shtml
    End Function

    <WebMethod()>
    Public Shared Function afishoMidisDatave(dateStart As String, dateEnd As String) As String
        Dim shtml As String = ""
        Dim nr As Integer = 1
        Try
            Dim con As OracleConnection = New OracleConnection(cs)
            Dim query As String = "SELECT FULL_NAME,IP,TO_CHAR(MIN(DT_START) - INTERVAL '5' MINUTE, 'DD/MM/YYYY HH24:MI') As DT_START ,TO_CHAR(MAX(DT_END), 'DD/MM/YYYY HH24:MI') As DT_END, SUM(ORET_PREZENTE) As ORET_PREZENTE,
                                   TRUNC(ORET_AKTIVE) AS ORET_AKTIVE, SUM(DITE_PUNE) As DITE_PUNE From 
                                   (           Select l.IP As IP, p.Full_Name As FULL_NAME, Min(logp.DT_SYS) AS DT_START, Max(logp.DT_SYS) AS DT_END, NVL(ORET_PREZENTE,0) As ORET_PREZENTE,
                                               TRUNC(Count(distinct TO_CHAR(logp.dt_sys,'DD-MM-YYYY HH24:MI'))/12 * 3600) As ORET_AKTIVE,
                                               count(distinct to_char(logp.dt_sys, 'DD-MM-YYYY')) DITE_PUNE
                                               From PC p , (
                                                               Select COUNT(DISTINCT TO_CHAR(DT_SYS, 'DD-MM-YYYY HH24')) As ORET_PREZENTE, IP From LOG_PC 
                                                               Where MOUSE_MOVE > 5 And TO_DATE(TO_CHAR(DT_SYS, 'DD-MM-YYYY'), 'DD-MM-YYYY') Between TO_DATE(:dateStart, 'DD-MM-YYYY') And TO_DATE(:dateEnd, 'DD-MM-YYYY')
                     
                                                               AND DT_SYS  BETWEEN TO_DATE(to_char(DT_SYS, 'DD-MM-YYYY')  || ' 08:30', 'DD-MM-YYYY HH24:MI') AND TO_DATE(to_char(DT_SYS, 'DD-MM-YYYY')  || ' 22:00', 'DD-MM-YYYY HH24:MI')
                                                               AND NOT EXISTS(select 1 from ref_pushimet where dt_pushim = to_char(DT_SYS, 'DD-MM-YYYY'))
                                                               AND TO_CHAR(DT_SYS, 'DD-MM-YYYY HH24:MI') BETWEEN TO_CHAR(DT_SYS, 'DD-MM-YYYY HH24') || ':30' AND TO_CHAR(DT_SYS, 'DD-MM-YYYY HH24') || ':59'
                            
                                                               Group By IP
                                               ) l, ( SELECT * FROM  log_pc WHERE 
                                                       MOUSE_MOVE > 5 And TO_DATE(TO_CHAR(DT_SYS, 'DD-MM-YYYY'), 'DD-MM-YYYY') Between TO_DATE(:dateStart, 'DD-MM-YYYY') And TO_DATE(:dateEnd, 'DD-MM-YYYY')
                
                                                       AND DT_SYS  BETWEEN TO_DATE(to_char(DT_SYS, 'DD-MM-YYYY')  || ' 08:30', 'DD-MM-YYYY HH24:MI') AND TO_DATE(to_char(DT_SYS, 'DD-MM-YYYY')  || ' 22:00', 'DD-MM-YYYY HH24:MI')
                                                       AND NOT EXISTS(select 1 from ref_pushimet where dt_pushim = to_char(DT_SYS, 'DD-MM-YYYY')) ) logp

                    
                                                       Where  p.IP = l.IP (+) AND P.ip =  logp.ip (+) 
                                                       Group By ORET_PREZENTE, l.IP, p.Full_Name, NVL(ORET_PREZENTE,0)
                                   ) Group By IP, FULL_NAME, TRUNC(ORET_AKTIVE) "
            Dim cmd As OracleCommand = New OracleCommand(query, con)
            cmd.Connection.Open()
            cmd.Parameters.Add(New OracleParameter(":dateStart", dateStart))
            cmd.Parameters.Add(New OracleParameter(":dateEnd", dateEnd))
            Dim da As OracleDataAdapter = New OracleDataAdapter(cmd)
            Dim dt As DataTable = New DataTable()
            da.Fill(dt)
            Dim time As TimeSpan
            shtml = "<table class='table table-bordered' id='tabela'>"
            shtml &= "<thead> <tr>
              <th scope='col'>#</th>
              <th scope='col'>PERDORUES</th>
              <th scope='col'>IP</th>
              <th scope='col'>DATA_FILLIM</th>
              <th scope='col'>DATA_MBARIM</th>
              <th scope='col'>ORET PREZENTE</th>
              <th scope='col'>ORET AKTIVE</th>
              <th scope='col'>DITE_PUNE</th>
            </tr></thead><tbody>"
            For Each dr As DataRow In dt.Rows
                shtml &= "<tr id = '" & dr("IP").ToString() & "'>"
                shtml &= "<td>" & nr & "</td>"
                shtml &= "<td style = 'text-align:left;'>" & dr("FULL_NAME").ToString() & "</td>"
                shtml &= "<td style = 'text-align:left;'>" & dr("IP").ToString() & "</td>"
                shtml &= "<td>" & dr("DT_START").ToString() & "</td>"
                shtml &= "<td>" & dr("DT_END").ToString() & "</td>"
                shtml &= "<td>" & Math.Round(dr("ORET_PREZENTE"), 1) & "</td>"
                time = TimeSpan.FromSeconds(dr("ORET_AKTIVE"))
                shtml &= "<td>" & String.Format("{0:00}:{1:00}", Math.Floor(time.TotalHours()), time.Minutes()) & "</td>"
                shtml &= "<td>" & dr("DITE_PUNE").ToString() & "</td>"
                shtml &= "</tr>"
                nr += 1
            Next
            shtml &= "</tbody>"
            shtml &= "</table>"
            cmd.Connection.Close()
            con.Dispose()
            cmd.Dispose()
        Catch ex As Exception
            shtml = "Error:" & ex.Message
        End Try
        Return shtml
    End Function






End Class