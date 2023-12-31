﻿using DialogueBuilderWpf.src;
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

        readonly string dataFilePath = Path.Join(Directory.GetCurrentDirectory(), "export", "CSV_csvTest_data.csv");
        readonly string relationFilePath = Path.Join(Directory.GetCurrentDirectory(), "export", "CSV_csvTest_relation.csv");
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
            catch (Exception) { }
        }

        [TestMethod]
        public void CreatesBothFiles()
        {
            CsvSerilizer csvSerilizer = new CsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);
            Assert.IsTrue(File.Exists(dataFilePath));
            Assert.IsTrue(File.Exists(relationFilePath));
        }

        [TestMethod]
        public void WritesCorrectHeaderLineInDataFile()
        {
            CsvSerilizer csvSerilizer = new CsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);

            string[] dataRows = File.ReadAllLines(dataFilePath);
            Assert.IsTrue(dataRows[0].Equals("UiID,NpcText,TooltipText,Effect,SkillID,InvokeActivity"));
        }

        [TestMethod]
        public void WritesCorrectDataInDataFile()
        {
            CsvSerilizer csvSerilizer = new CsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);

            string[] dataRows = File.ReadAllLines(dataFilePath);
            Assert.IsTrue(dataRows[3].Equals("a2,,,-1,1,False"));
            Assert.IsTrue(dataRows[7].Equals("d2,,,-1,1,False"));

        }

        [TestMethod]
        public void WritesCorrectHeaderLineInRelationFile()
        {
            CsvSerilizer csvSerilizer = new CsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);
            string[] dataRows = File.ReadAllLines(relationFilePath);
            Assert.AreEqual(dataRows[0], "ID,Parent,Child");
        }

        [TestMethod]
        public void WritesCorrectDataInRelationFile()
        {
            CsvSerilizer csvSerilizer = new CsvSerilizer();
            csvSerilizer.Serialize(Directory.GetCurrentDirectory(), "csvTest", MTestUtils.SetupTestTree().Root);
            string[] dataRows = File.ReadAllLines(relationFilePath);
            Assert.IsTrue(dataRows[1].Contains("root,a1"));
            Assert.IsTrue(dataRows[3].Contains("a1,b2"));
        }
    }
}
