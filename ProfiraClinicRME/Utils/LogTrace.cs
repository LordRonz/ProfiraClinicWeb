
using Serilog;
using System.Runtime.CompilerServices;

namespace ProfiraClinicRME.Utils
{
    public class LogTrace
    {
        public static void TakeContextSnapshot(string msg,
            object? data = null,
            [CallerFilePath] string path = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            string[] splitString = path.Split(new char[] { '\\' });
            string name = splitString[splitString.Length - 1];
            Log.Debug("{path} {member} {line} {msg} {data}", name, member, line, msg, data is null ? null : ObjectDumper.Dump(data));
        }
        public static void Info(string msg,
        object? data = null,
        [CallerFilePath] string path = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            string[] splitString = path.Split(new char[] { '\\' });
            string name = splitString[splitString.Length - 1];
            Log.Information(" {path} {member} {line} {msg} {data}", name, member, line, msg, data is null ? null : ObjectDumper.Dump(data));

        }


        public static void Error(string msg,
            object? data = null,
            [CallerFilePath] string path = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            string[] splitString = path.Split(new char[] { '\\' });
            string name = splitString[splitString.Length - 1];
            Log.Error("{path} {member} {line} {msg} {data}", name, member, line, msg, data is null ? null : ObjectDumper.Dump(data));
        }

        public static void Debug(string msg,
            object? data = null,
            [CallerFilePath] string path = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            string[] splitString = path.Split(new char[] { '\\' });
            string name = splitString[splitString.Length - 1];
            Log.Debug("{path} {member} {line} {msg} {data}", name, member, line, msg, data is null ? null : ObjectDumper.Dump(data));
        }
    }
}
