using DialogueBuilderWpf.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using System.Xml.Linq;

namespace DialogueBuilderWpf.Test
{
    [TestClass]
    public class DialogueTree_Test
    {



        [TestMethod]
        public void DialogueTreeInitializes()
        {
            Node node = new Node("root");
            DialogueTree tree = new DialogueTree(node);
            Assert.AreEqual(tree.Root, node);
        }

        [TestMethod]
        public void AddsNodeToRoot()
        {
            Node node = new Node("root");
            DialogueTree tree = new DialogueTree(node);
            tree.AddNode(tree.Root);
            Assert.AreEqual(tree.Root.NextOptions[0].UiID, "unnamed");
        }

        [TestMethod]
        public void AddsNodeToChildren()
        {
            Node node = new Node("root");
            DialogueTree tree = new DialogueTree(node);
            tree.AddNode(tree.Root);
            tree.Root.NextOptions[0].Update("a", "", "", "-1", "1", false);
            tree.AddNode(tree.Root.NextOptions[0]);
            Assert.AreEqual(tree.Root.NextOptions[0].NextOptions[0].UiID, "unnamed");
        }

        [TestMethod]
        public void FindsNodeByUiID()
        {
            DialogueTree tree = MTestUtils.SetupTestTree();
            Node? result = tree.FindNodeById("e2");
            Assert.AreEqual("e2", result?.UiID);
            Node? result2 = tree.FindNodeById("a1");
            Assert.AreEqual("a1", result2?.UiID);
            Node? result3 = tree.FindNodeById("xx");
            Assert.IsNull(result3);
        }

        [TestMethod]
        public void GetsCorrectNumberOfChildrenUiIds() 
        {
            DialogueTree tree = MTestUtils.SetupTestTree();
            List<string> result = tree.GetNodeChildrenUIiDS("b1");
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void GetsCorrectChildrenUiIds()
        {
            DialogueTree tree = MTestUtils.SetupTestTree();
            List<string> result = tree.GetNodeChildrenUIiDS("b1");
            Assert.IsTrue(result.Contains("d2"));
            Assert.IsTrue(result.Contains("e2"));
            Assert.IsTrue(result.Contains("f2"));
        }

        [TestMethod]
        public void DeletesBranch()
        {
            DialogueTree tree = MTestUtils.SetupTestTree();
            Node? toRemove = tree.FindNodeById("a1");
            tree.RemoveNodeBranch(toRemove!);

            List<string> result = tree.GetNodeChildrenUIiDS("root");
            Assert.AreEqual(1, result.Count);

            Assert.IsNull(tree.FindNodeById("a1"));
            Assert.IsNull(tree.FindNodeById("a2"));
            Assert.IsNull(tree.FindNodeById("b2"));
            Assert.IsNull(tree.FindNodeById("c2"));

            Assert.IsNotNull(tree.FindNodeById("b1"));
            Assert.IsNotNull(tree.FindNodeById("d2"));
            Assert.IsNotNull(tree.FindNodeById("e2"));
            Assert.IsNotNull(tree.FindNodeById("f2"));
        }
    }
}
