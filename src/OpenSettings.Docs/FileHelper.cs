using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenSettings.Docs
{
    public static class FileHelper
    {
        public static void ReplaceTokensInFile(
        string path,
        IEnumerable<KeyValuePair<string, string>> replacements,
        int bufferSize = 128 * 1024)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found.", path);
            }

            var list = new List<KeyValuePair<string, string>>();

            foreach (var kv in replacements ?? throw new ArgumentNullException(nameof(replacements)))
            {
                if (string.IsNullOrEmpty(kv.Key))
                {
                    throw new ArgumentException("Replacement token cannot be null or empty.", nameof(replacements));
                }

                list.Add(kv);
            }

            if (list.Count == 0)
            {
                return;
            }

            var tmp = path + ".tmp";
            var anyChange = false;

            try
            {
                using var input = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.SequentialScan);
                using var reader = new StreamReader(input, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: bufferSize);
                using var output = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, FileOptions.SequentialScan);
                using var writer = new StreamWriter(output, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), bufferSize);

                while (reader.ReadLine() is { } line)
                {
                    var original = line;

                    foreach (var kv in list)
                    {
                        line = Replace(line, kv.Key, kv.Value, StringComparison.Ordinal);
                    }

                    if (!anyChange && !ReferenceEquals(original, line) &&
                        !string.Equals(original, line, StringComparison.Ordinal))
                    {
                        anyChange = true;
                    }

                    writer.WriteLine(line);
                }

                writer.Flush();
                output.Flush(true);
            }
            catch
            {
                TryDelete(tmp);
                throw;
            }

            if (!anyChange)
            {
                TryDelete(tmp);
                return;
            }

            try
            {
                File.Replace(tmp, path, destinationBackupFileName: null);
            }
            catch
            {
                TryDelete(tmp);
                throw;
            }
        }

        private static string Replace(string input, string oldValue, string newValue, StringComparison stringComparison)
        {
            var idx = input.IndexOf(oldValue, stringComparison);

            if (idx < 0)
            {
                return input;
            }

            var sb = new StringBuilder(input.Length + Math.Max(0, (newValue?.Length ?? 0) - oldValue.Length) * 4);

            var last = 0;

            while (idx >= 0)
            {
                sb.Append(input, last, idx - last);
                sb.Append(newValue);
                last = idx + oldValue.Length;
                idx = input.IndexOf(oldValue, last, stringComparison);
            }

            sb.Append(input, last, input.Length - last);

            return sb.ToString();
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (path is not null && File.Exists(path)) File.Delete(path);
            }
            catch
            {
                 /* swallow cleanup errors */
            }
        }
    }
}
