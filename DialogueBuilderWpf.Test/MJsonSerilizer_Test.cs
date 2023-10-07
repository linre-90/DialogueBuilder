using DialogueBuilderWpf.src;
using DialogueBuilderWpf.src.serializer;

namespace DialogueBuilderWpf.Test
{
    [TestClass]
    public class MJsonSerilizer_Test
    {
        readonly string projectName = "mjsonTest";
        readonly string projectDir = Directory.GetCurrentDirectory();

        [TestMethod]
        public void WtitesFileWithCorrectData()
        {
            DialogueTree tree = MTestUtils.SetupTestTree();
            MJsonSerializer mJsonSerializer = new MJsonSerializer();
            mJsonSerializer.Serialize(projectDir, projectName, tree.Root);

            Assert.IsTrue(File.Exists(Path.Join(MFileWriter.BuildJsonFilePath(projectDir, projectName))));
            string content = File.ReadAllText(MFileWriter.BuildJsonFilePath(projectDir, projectName));

            Assert.IsTrue(content.Contains("{\"UiID\":\"root\","));
            Assert.IsTrue(content.Contains("{\"UiID\":\"d2\","));
            Assert.IsTrue(content.Contains("{\"UiID\":\"a1\","));
            Assert.IsTrue(content.Contains("{\"UiID\":\"f2\","));
            Assert.IsTrue(content.Contains("\"InvokeActivity\":false"));
            Assert.IsTrue(content.Contains("\"NextOptions\":[]"));


            File.Delete(MFileWriter.BuildJsonFilePath(projectDir, projectName));
        }

        [TestMethod]
        public void ReadsFileWithCorrectData()
        {
            DialogueTree tree = MTestUtils.SetupTestTree();
            MJsonSerializer mJsonSerializer = new MJsonSerializer();
            mJsonSerializer.Serialize(projectDir, projectName, tree.Root);

            DialogueTree treeFromFile = mJsonSerializer.DeserializeProject(MFileWriter.BuildJsonFilePath(projectDir, projectName));

            Assert.AreEqual(tree.Root.ChildrenListAsString().Length, treeFromFile.Root.ChildrenListAsString().Length);
            Assert.IsNotNull(treeFromFile.FindNodeById("f2"));
            Assert.IsNotNull(treeFromFile.FindNodeById("b2"));
            Assert.IsNotNull(treeFromFile.FindNodeById("b1"));
            Assert.IsNotNull(treeFromFile.FindNodeById("a1"));

            File.Delete(MFileWriter.BuildJsonFilePath(projectDir, projectName));
        }
    }
}
