Imports MySql.Data.MySqlClient
Module Module1

    Public con As MySqlConnection

    Sub Main()

        Dim sql As String
        Dim foldername As String
        Dim oldfilename As String
        Dim newfilename As String
        Dim oldfilenamewithpath As String
        Dim newfilenamewithpath As String

        db_open()

        sql = "SELECT CM_FILE_GROUP_ID,CF_FILE_SL_NO,CF_FILE_NAME,TRIM(RPAD(CONCAT (DATE_FORMAT(CM_COMM_DATE,'%Y%m%d'),' ',LPAD(CM_COMM_ID,5,' '),'  ',CM_COMM_TYPE,'  ',LPAD(CM_COMM_NO,5,' '),'  ',LPAD(CM_COMM_YEAR,4,' '),'  ',RPAD(CM_DEPARTMENT,5,' '),'  ',CM_SUBJECT,' KW - ',CM_KEYWORD),180,' ')) FILENAME FROM COMM_MASTER JOIN COMM_FILE ON CF_FILE_GROUP_ID = CM_FILE_GROUP_ID WHERE CM_RECORD_STATUS = 51 AND CF_RECORD_STATUS = 51 AND CM_FILE_GROUP_ID BETWEEN 20001 AND 20010 ORDER BY CM_FILE_GROUP_ID"
        Dim cmd As New MySqlCommand(sql, con)
        Dim dr As MySqlDataReader = cmd.ExecuteReader()
        While dr.Read()
            foldername = dr.Item("CM_FILE_GROUP_ID").ToString
            oldfilename = dr.Item("CF_FILE_NAME").ToString
            newfilename = dr.Item("FILENAME").ToString

            oldfilenamewithpath = "D:\2\" & foldername & "\" & oldfilename
            newfilename = ReplaceInvalidChar(newfilename, " ")
            newfilenamewithpath = newfilename & ".pdf"

            Try
                My.Computer.FileSystem.RenameFile(oldfilenamewithpath, newfilenamewithpath)
            Catch ex As Exception
            End Try

        End While
        dr.Close()

    End Sub

    Sub db_open()
        con = New MySqlConnection()
        con.ConnectionString = "server=192.168.1.231;user id=fran1875;password=HMLNKil8;database=letter"
        Try
            con.Open()
        Catch myerror As MySqlException
        End Try
    End Sub
    Sub db_close()
        con.Close()
        con.Dispose()
    End Sub

    Function ReplaceInvalidChar(gText As String, gReplaceChar As String)

        gText = Replace(gText, "/", gReplaceChar)
        gText = Replace(gText, "\", gReplaceChar)
        gText = Replace(gText, ":", gReplaceChar)
        gText = Replace(gText, "?", gReplaceChar)
        gText = Replace(gText, "<", gReplaceChar)
        gText = Replace(gText, ">", gReplaceChar)
        gText = Replace(gText, "|", gReplaceChar)
        gText = Replace(gText, "*", gReplaceChar)

        Dim tempchar As String
        Dim tempstring As String = ""
        For x = 1 To Len(gText)

            tempchar = Mid(gText, x, 1)
            If InStr("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ 0123456789-+%;&!@#$%^&()_=+~`';{}[]?", tempchar) > 0 Then

                tempstring = tempstring & tempchar

            Else

                tempstring = tempstring & " "

            End If

        Next x

        gText = tempstring
        Return gText

    End Function
End Module
