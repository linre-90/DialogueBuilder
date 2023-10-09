using DialogueBuilderWpf.src;
using DialogueBuilderWpf.src.serializer;
using System.Text.Json;

namespace DialogueBuilderWpf.Test
{
    [TestClass]
    public class MFileWriterTest
    {
        readonly string projectName = "MfileWriterTest";

        [TestMethod]
        public void CreatesJsonFile()
        {
            
            MFileWriter.WriteJsonFile(JsonSerializer.Serialize("asd"), Directory.GetCurrentDirectory(), projectName);
            Assert.IsTrue(File.Exists(DataService.GetProjectJsonFilePath(Directory.GetCurrentDirectory(), projectName)));
            File.Delete(DataService.GetProjectJsonFilePath(Directory.GetCurrentDirectory(), projectName));
        }

        [TestMethod]
        public void CreatesCsvFiles()
        {
            MFileWriter.WriteCsvFiles(
                new List<string>() { "asdasd"}, 
                new List<string>() { "asdasd" }, 
                Directory.GetCurrentDirectory(), 
                projectName, "UE");

            Assert.IsTrue(File.Exists(Path.Join(Directory.GetCurrentDirectory(), "export", $"UE_{projectName}_relation.csv")));
            Assert.IsTrue(File.Exists(Path.Join(Directory.GetCurrentDirectory(), "export", $"UE_{projectName}_data.csv")));

            File.Delete(Path.Join(Directory.GetCurrentDirectory(), "export", $"UE_{projectName}_relation.csv"));
            File.Delete(Path.Join(Directory.GetCurrentDirectory(), "export", $"UE_{projectName}_data.csv"));
        }
    }
}
