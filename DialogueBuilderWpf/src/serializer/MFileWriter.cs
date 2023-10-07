using System.Collections.Generic;
using System.IO;

namespace DialogueBuilderWpf.src.serializer
{
    /// <summary>
    /// File writer class supports [json, csv] formats.
    /// </summary>
    internal class MFileWriter
    {

        public static string BuildJsonFilePath(string projectDir, string projectName)
        {
            return Path.Combine(projectDir, $"{projectName}.json");
        }

        public static void WriteJsonFile(string data, string projectDir, string projectName)
        {
            File.WriteAllText(BuildJsonFilePath(projectDir, projectName), data);
        }

        public static void WriteCsvFiles(List<string> dataCsv, List<string> relationshipCsv, string projectDir, string projectName)
        {
            string exportDirName = "export";
            string dataFilePath = Path.Combine(new string[] {projectDir, exportDirName, $"UE_{projectName}_data.csv"});
            string relationshipFilePath = Path.Combine(new string[] { projectDir, exportDirName, $"UE_{projectName}_relation.csv" });

            Directory.CreateDirectory(Path.Combine(projectDir, exportDirName));
            File.WriteAllLines(dataFilePath, dataCsv);
            File.WriteAllLines(relationshipFilePath, relationshipCsv);
        }
    }
}
