# MSSQL Fileless Rootkit - WarSQLKit

Bildiğiniz üzere uzun zamandır MSSQL üzerine çalışmalar yapmaktayım. Bu yazımda uzun zamandır uğraştığım bir konuyu ele alacağım, MSSQL Rootkit. Bildiğiniz üzere şimdiye kadar MS-SQL için anlatılan post-exploitation işlemlerinin büyük çoğunluğu “xp_cmdshell” ve “sp_OACreate” stored procedure’lerini kullanarak anlatılır. Peki xp_cmdshell ve sp_OACreate stored procedure’lerinin olmadığı bir MSSQL sunucusunun “sa” hesabını ele geçirmişsek, o sisteme girmekten vaz mı geçeceğiz?
Tabii ki vazgeçmememiz gerekiyor. Bu makale “sa” hesabının yakalandığı ve “xp_cmdshell”, “sp_OACreate”, “sp_OAMethod” vb. prosedürlerin hiç birinin çalışmadığı bir senaryo düşünülerek kaleme alınmıştır.
