using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExamindMui5Import
{
    class Program
    {
        const string RootFolder = @"C:\Users\Johnny\projects\examind-web-2";

        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(RootFolder, "*.tsx*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var newLines = new List<string>();
                var materialImports = new List<string>();
                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    if (line.IsMuiRootImport())
                        materialImports.Add(line.ExtractComponent());
                    else
                        newLines.Add(line);
                }

                if (!materialImports.Any())
                    continue;

                newLines.Insert(
                    lines.ToList().FirstMaterialImportIndex() ?? 0,
                    $"import {{{string.Join(", ", materialImports.OrderBy(i => i))}}} from '@mui/material';");

                File.WriteAllText(file, string.Join("\n", newLines) + "\n");
            }
        }

    }

    static class Helpers
    {
        static Regex RootImportPattern = new Regex(@"import[^\{]+'@mui\/material");
        public static bool IsMuiRootImport(this string s) =>
            RootImportPattern.IsMatch(s);

        public static string ExtractComponent(this string s)  {
            var match = Regex.Match(s, @"import\s+(\w+?)\s+from");
            return match.Groups[1].Value;
        }

        public static int? FirstMaterialImportIndex(this List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
                if (RootImportPattern.IsMatch(lines[i]))
                    return i;

            return null;
        }
    }
}
