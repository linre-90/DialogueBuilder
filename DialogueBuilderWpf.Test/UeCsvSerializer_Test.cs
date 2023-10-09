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
    public class UeCsvSerializer_Test
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
            UeCsvSerilizer csvSerilizer = new UeCsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);
            Assert.IsTrue(File.Exists(dataFilePath));
            Assert.IsTrue(File.Exists(relationFilePath));
        }

        [TestMethod]
        public void SavedDataFileContainsCorrectData()
        {
            UeCsvSerilizer csvSerilizer = new UeCsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);

            string[] dataRows = File.ReadAllLines(dataFilePath);
            Assert.IsTrue(dataRows[3].Equals("a2,,,-1,1,False"));
            Assert.IsTrue(dataRows[7].Equals("d2,,,-1,1,False"));
        }

        [TestMethod]
        public void SavedDataFileContainsCorrectHeader()
        {
            UeCsvSerilizer csvSerilizer = new UeCsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);

            string[] dataRows = File.ReadAllLines(dataFilePath);
            Assert.IsTrue(dataRows[0].Equals(",NpcText,TooltipText,Effect,SkillID,InvokeActivity"));
        }

        [TestMethod]
        public void SavedRelationFileContainsCorrectData()
        {
            UeCsvSerilizer csvSerilizer = new UeCsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);

            string[] dataRows = File.ReadAllLines(relationFilePath);
            Assert.IsTrue(dataRows[1].Contains("root,a1"));
            Assert.IsTrue(dataRows[3].Contains("a1,b2"));
        }

        [TestMethod]
        public void SavedRelationFileContainsCorrectHeader()
        {
            UeCsvSerilizer csvSerilizer = new UeCsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);

            string[] dataRows = File.ReadAllLines(relationFilePath);
            Assert.AreEqual(dataRows[0], ",Parent,Child");
        }
    }
}
