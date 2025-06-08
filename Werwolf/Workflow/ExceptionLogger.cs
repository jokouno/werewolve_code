using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Werwolf.Workflow
{
    public static partial class ExceptionLogger
    {
        private static readonly List<string> _callStack = new();

        public static void Log(string methodName, params object[] parameters)
        {
            string paramString = parameters != null && parameters.Length > 0
                ? string.Join(", ", parameters)
                : string.Empty;
            _callStack.Add($"{DateTime.Now:O} - {methodName}({paramString})");
        }

        public static void LogException(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine("------ Exception ------");
            sb.AppendLine(ex.ToString());
            sb.AppendLine("------ Callstack ------");
            foreach (var entry in _callStack)
                sb.AppendLine(entry);

            string directory = GetDirectoryPath();

            Directory.CreateDirectory(directory);

            string filePath = Path.Combine(directory,
                $"Exception_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            File.WriteAllText(filePath, sb.ToString());

            _callStack.Clear();
        }

        private static string GetDirectoryPath()
        {
#if ANDROID
    var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(
                  Android.OS.Environment.DirectoryDownloads);
    return dir.AbsolutePath;
#else
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "ExceptionLogs");
#endif
        }
    }
}