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
    public class DataService_Test
    {
        readonly string projectName = "dataServiceTest";
        readonly string projectDir = Directory.GetCurrentDirectory();


        [TestCleanup]
        public void RemoveFilesAndDicts()
        {
            try
            {
                File.Delete(DataService.GetProjectJsonFilePath(Path.Combine(projectDir, projectName), projectName));
                Directory.Delete(Path.Combine(projectDir, projectName));
            }
            catch (Exception){}
        }

        [TestInitialize]
        public void TestInit()
        {
            Directory.CreateDirectory(Path.Combine(projectDir, projectName));
            MJsonSerializer mJsonSerializer = new MJsonSerializer();
            mJsonSerializer.Serialize(Path.Combine(projectDir, projectName), projectName, MTestUtils.SetupTestTree().Root);
        }

        [TestMethod]
        public void DataServiceInitializesEmpty()
        {
            DataService dataService = new DataService();
            dataService.InitializeNewProject(Path.Combine(projectDir, projectName));

            Assert.IsTrue(dataService.Data!.Root.UiID.Equals("root"));
            Assert.IsTrue(dataService.ProjectDir!.Equals(Path.Combine(projectDir, projectName)));
            Assert.IsTrue(dataService.ProjectName!.Equals(projectName));
        }

        [TestMethod]
        public void DataServiceInitializesFromFile()
        {
            DataService dataService = new DataService();
            dataService.InitializeFromFile(Path.Combine(projectDir, projectName));

            Assert.IsTrue(dataService.Data!.Root.UiID.Equals("root"));
            Assert.IsTrue(dataService.ProjectDir!.Equals(Path.Combine(projectDir, projectName)));
            Assert.IsTrue(dataService.ProjectName!.Equals(projectName));
        }

        [TestMethod]
        public void GetChildUiIds()
        {
            DataService dataService = new DataService();
            dataService.InitializeFromFile(Path.Combine(projectDir, projectName));
            List<string> children = dataService!.GetChildrenUiIds("a1")!;
            Assert.AreEqual(3, children.Count);
            Assert.IsTrue(children.Contains("a2"));
            Assert.IsTrue(children.Contains("b2"));
            Assert.IsTrue(children.Contains("c2"));
        }

        [TestMethod]
        public void FindNodeByIdFindNode()
        {
            DataService dataService = new DataService();
            dataService.InitializeFromFile(Path.Combine(projectDir, projectName));
            Node? found = dataService!.FindNodeById("a1")!;
            Assert.IsNotNull(found);
        }

        [TestMethod]
        public void AddnewNodeSucceesds()
        {
            DataService dataService = new DataService();
            dataService.InitializeFromFile(Path.Combine(projectDir, projectName));
            dataService!.AddNewNodeToParent("e2");
            Node? found = dataService.FindNodeById("unnamed");
            Assert.IsNotNull(found);
        }

        [TestMethod]
        public void NodeBranchDeletes()
        {
            DataService dataService = new DataService();
            dataService.InitializeFromFile(Path.Combine(projectDir, projectName));
            dataService!.DeleteNodeBranch("b1");
            Node? found = dataService.FindNodeById("d2");
            Assert.IsNull(found);
            Node? found1 = dataService.FindNodeById("e2");
            Assert.IsNull(found1);
            Node? found2 = dataService.FindNodeById("f2");
            Assert.IsNull(found2);
            Node? found3 = dataService.FindNodeById("b1");
            Assert.IsNull(found3);
        }

        [TestMethod]
        public void NodeValuesCanBeUpdated()
        {
            DataService dataService = new DataService();
            dataService.InitializeFromFile(Path.Combine(projectDir, projectName));
            dataService.UpdateNodeValues("a1", "aa1", "asd", "dsa", "-1", "69", true);
            Node? updatedNode = dataService.FindNodeById("aa1");
            Assert.IsNotNull(updatedNode);

            Assert.AreEqual("aa1", updatedNode.UiID);
            Assert.AreEqual("asd", updatedNode.NpcText);
            Assert.AreEqual("dsa", updatedNode.TooltipText);
            Assert.AreEqual(-1, updatedNode.Effect);
            Assert.AreEqual(69, updatedNode.SkillID);
            Assert.AreEqual(true, updatedNode.InvokeActivity);
        }

        [TestMethod]
        public void DataServiceSavesJson()
        {
            try
            {
                DataService dataService = new DataService();
                dataService.InitializeFromFile(Path.Combine(projectDir, projectName));
                dataService.Save(new MJsonSerializer());
            }
            catch (Exception e)
            {
                Assert.Fail("Data service json save failed: " + e.Message);
            }
        }


        [TestMethod]
        public void DataServiceSavesCsv()
        {
            try
            {
                DataService dataService = new DataService();
                dataService.InitializeFromFile(Path.Combine(projectDir, projectName));
                dataService.Save(new UeCsvSerilizer());
            }
            catch (Exception e)
            {
                Assert.Fail("Data service csv save failed: " + e.Message);
            }
        }
    }
}
