using System.Collections.Generic;
using System.IO;

namespace DialogueBuilderWpf.src.serializer
{
    internal class MFileWriter
    {

        public static void WriteJsonFile(string data, string projectDir, string projectName)
        {
            File.WriteAllText(DataService.GetProjectJsonFilePath(projectDir, projectName), data);
        }

        public static void WriteCsvFiles(List<string> dataCsv, List<string> relationshipCsv, string projectDir, string projectName, string prefix)
        {
            string exportDirName = "export";
            string dataFilePath = Path.Combine(new string[] {projectDir, exportDirName, $"{prefix}_{projectName}_data.csv"});
            string relationshipFilePath = Path.Combine(new string[] { projectDir, exportDirName, $"{prefix}_{projectName}_relation.csv" });

            Directory.CreateDirectory(Path.Combine(projectDir, exportDirName));
            File.WriteAllLines(dataFilePath, dataCsv);
            File.WriteAllLines(relationshipFilePath, relationshipCsv);
        }
    }
}
