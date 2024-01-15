using System.Collections.Generic;
using System.IO;

namespace DialogueBuilderWpf.src.serializer
{
    /// <summary>
    /// File writer provides static methods to actually write data on disk.
    /// </summary>
    internal class MFileWriter
    {
        /// <summary>
        /// Write json data to file.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="projectDir"></param>
        /// <param name="projectName"></param>
        public static void WriteJsonFile(string data, string projectDir, string projectName)
        {
            File.WriteAllText(DataService.GetProjectJsonFilePath(projectDir, projectName), data);
        }

        /// <summary>
        /// Write csv data to file.
        /// </summary>
        /// <param name="dataCsv"></param>
        /// <param name="relationshipCsv"></param>
        /// <param name="projectDir"></param>
        /// <param name="projectName"></param>
        /// <param name="prefix"></param>
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
