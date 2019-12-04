' NOTE: You can use the "Rename" command on the context menu to change the class name "Service1" in code, svc and config file together.
' NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.vb at the Solution Explorer and start debugging.
Imports Oracle.DataAccess.Client
Public Class Service1
    Implements IService1

    Public Sub New()
    End Sub

    Dim cs As String = ConfigurationManager.ConnectionStrings("CS").ConnectionString

    Public Function GetData(ByVal value As Integer) As String Implements IService1.GetData
        Return String.Format("You entered: {0}", value)
    End Function

    Public Function GetDataUsingDataContract(ByVal composite As CompositeType) As CompositeType Implements IService1.GetDataUsingDataContract
        If composite Is Nothing Then
            Throw New ArgumentNullException("composite")
        End If
        If composite.BoolValue Then
            composite.StringValue &= "Suffix"
        End If
        Return composite
    End Function


    Public Function insertLogPc(ip As String, machineId As String, pcName As String, mouseMove As Integer) As Boolean Implements IService1.insertLogPc
        Dim success As Boolean = False
        Dim con As OracleConnection = New OracleConnection(cs)
        Dim cmd As OracleCommand = New OracleCommand("insertLogPc", con)
        Try

            con.Open()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New OracleParameter("ips", ip))
            cmd.Parameters.Add(New OracleParameter("pcName", pcName))
            cmd.Parameters.Add(New OracleParameter("machineId", machineId))
            cmd.Parameters.Add(New OracleParameter("mouseMove", mouseMove))
            cmd.Parameters.Add(New OracleParameter("sERRMSG", OracleDbType.Varchar2, 200, "sERRMSG", ParameterDirection.Output))

            cmd.ExecuteNonQuery()
            Dim msg As String = cmd.Parameters("sERRMSG").Value.ToString()
            If msg = "OK" Then
                success = True
            End If
        Catch ex As Exception
            Console.WriteLine("Error : " + ex.Message)
        Finally
            con.Close()
            con.Dispose()
            cmd.Dispose()

        End Try
        Return success
    End Function

    Public Function registerUser(userName As String, fullName As String, ip As String, machineId As String) As Boolean Implements IService1.registerUser
        Dim success As Boolean = False
        Dim sQuery As String = "INSERT INTO PC(usern,full_name,ip,machine_id) VALUES (:userN,:fullName,:ip,:machine_id) "
        Dim con As OracleConnection = New OracleConnection(cs)
        Dim cmd As OracleCommand = New OracleCommand(sQuery, con)
        Try
            con.Open()
            cmd.Parameters.Add(New OracleParameter(":userN", userName))
            cmd.Parameters.Add(New OracleParameter(":fullName", fullName))
            cmd.Parameters.Add(New OracleParameter(":ip", ip))
            cmd.Parameters.Add(New OracleParameter(":machine_id", machineId))
            Dim nr As Integer = cmd.ExecuteNonQuery()
            If nr > 0 Then
                success = True
            End If
        Catch ex As Exception
            Throw
        Finally
            con.Close()
            con.Dispose()
            cmd.Dispose()
        End Try
        Return success
    End Function



    Public Function insertPc(ip As String, pcName As String, mouseMove As Integer) As Boolean Implements IService1.insertPc
        Dim success As Boolean = False
        Dim query As String = "insert into log_pc(ip,machine_id,mouse_move) values (:ip,:machineId,:mouseMove)"
        Dim con As OracleConnection = New OracleConnection(cs)
        Dim cmd As OracleCommand = New OracleCommand(query, con)
        Try
            con.Open()
            cmd.Parameters.Add(New OracleParameter(":ip", ip))
            cmd.Parameters.Add(New OracleParameter(":machineId", pcName))
            cmd.Parameters.Add(New OracleParameter(":mouseMove", mouseMove))
            Dim check As Integer = cmd.ExecuteNonQuery()
            If check > 0 Then
                success = True
            End If
        Catch ex As Exception
            Console.WriteLine("Error : " + ex.Message)
        Finally
            con.Close()
            con.Dispose()
            cmd.Dispose()
        End Try
        Return success
    End Function

    Public Function insErr(ip As String, error_msg As String) As Boolean Implements IService1.insErr
        Dim success As Boolean = False
        Dim q As String = "insert into log_erroret(ip,erroret) values (:ip,:errMessage)"
        Dim con As OracleConnection = New OracleConnection(cs)
        Dim cmd As OracleCommand = New OracleCommand(q, con)
        Try
            con.Open()
            cmd.Parameters.Add(New OracleParameter(":ip", ip))
            cmd.Parameters.Add(New OracleParameter(":errMessage", error_msg))
            Dim check As Integer = cmd.ExecuteNonQuery()
            If check > 0 Then
                success = True
            End If
        Catch ex As Exception
            Console.WriteLine("Error !")
        Finally
            con.Close()
            con.Dispose()
            cmd.Dispose()
        End Try
        Return success
    End Function

End Class
