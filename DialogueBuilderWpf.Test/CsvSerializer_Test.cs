using DialogueBuilderWpf.src;
using DialogueBuilderWpf.src.serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueBuilderWpf.Test
{


    [TestClass]
    public class CsvSerializer_Test
    {
        readonly string dataFilePath = Path.Join(Directory.GetCurrentDirectory(), "export", "UE_csvTest_data.csv");
        readonly string relationFilePath = Path.Join(Directory.GetCurrentDirectory(), "export", "UE_csvTest_relation.csv");
        readonly string exportFolderPath = Path.Join(Directory.GetCurrentDirectory(), "export");

        [TestCleanup]
        public void CleanUp()
        {
            try
            {
                File.Delete(dataFilePath);
                File.Delete(relationFilePath);
                Directory.Delete(exportFolderPath);
            }
            catch (Exception){}
        }

        [TestMethod]
        public void SavesFileToDisk()
        {
            CsvSerilizer csvSerilizer = new CsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);
            Assert.IsTrue(File.Exists(dataFilePath));
            Assert.IsTrue(File.Exists(relationFilePath));
        }

        [TestMethod]
        public void SavedDataFileContainsCorrectRows()
        {
            CsvSerilizer csvSerilizer = new CsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);
            Assert.IsTrue(File.Exists(dataFilePath));
            Assert.IsTrue(File.Exists(relationFilePath));

            string[] dataRows = File.ReadAllLines(dataFilePath);
            dataRows.Contains("a2,,,-1,1,false");
            dataRows.Contains("f2,,,-1,1,false");
        }

        [TestMethod]
        public void SavedRelationFileContainsCorrectRows()
        {
            CsvSerilizer csvSerilizer = new CsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);
            Assert.IsTrue(File.Exists(dataFilePath));
            Assert.IsTrue(File.Exists(relationFilePath));

            string[] dataRows = File.ReadAllLines(relationFilePath);
            Assert.AreEqual(dataRows[0], ",Parent,Child");
            Assert.IsTrue(dataRows[1].Contains("root,a1"));
            Assert.IsTrue(dataRows[3].Contains("a1,b2"));
        }
    }
}
