using DialogueBuilderWpf.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueBuilderWpf.Test
{

    [TestClass]
    public class Node_Test
    {
        [TestMethod]
        public void CreatingNodesucceeded() 
        {
            string nodeName = "root";
            Node node = new Node(nodeName);
            Assert.AreEqual(nodeName, node.UiID);
        }

        [TestMethod]
        public void NonRootNodeCanBeUpdated()
        {
            string nodeName = "a1";
            string npcTxt = "npc text";
            string tooltipTxt = "tooltip text";
            string effect = "1";
            string skillId = "69";

            Node node = new Node(nodeName);
            node.Update("b1", npcTxt, tooltipTxt, effect, skillId, true);
            Assert.AreEqual("b1", node.UiID);
            Assert.AreEqual(npcTxt, node.NpcText);
            Assert.AreEqual(tooltipTxt, node.TooltipText);
            Assert.AreEqual(effect, node.Effect.ToString());
            Assert.AreEqual(skillId, node.SkillID.ToString());
            Assert.IsTrue(node.InvokeActivity);
        }

        [TestMethod]
        public void RootNameCannotBeUpdated()
        {
            string root = "root";
            string nodeName = "a1";
            string npcTxt = "npc text";
            string tooltipTxt = "tooltip text";
            string effect = "1";
            string skillId = "69";

            Node node = new Node(root);
            node.Update(nodeName, npcTxt, tooltipTxt, effect, skillId, false);
            Assert.AreEqual(root, node.UiID);
            Assert.AreEqual(npcTxt, node.NpcText);
            Assert.AreEqual(tooltipTxt, node.TooltipText);
            Assert.AreEqual(effect, node.Effect.ToString());
            Assert.AreEqual(skillId, node.SkillID.ToString());
            Assert.IsFalse(node.InvokeActivity);
        }

        [TestMethod]
        public void ValueUpdateFailsWithInvalidSkillID()
        {
            string nodeName = "a1";
            string npcTxt = "npc text";
            string tooltipTxt = "tooltip text";
            string effect = "1";
            string skillId = "abc";

            Node node = new Node(nodeName);
            Assert.IsFalse(node.Update("b1", npcTxt, tooltipTxt, effect, skillId, true));
            Assert.AreEqual(nodeName, node.UiID);
        }

        [TestMethod]
        public void ValueUpdateFailsWithInvalidEffect()
        {
            string nodeName = "a1";
            string npcTxt = "npc text";
            string tooltipTxt = "tooltip text";
            string effect = "-200";
            string skillId = "69";

            Node node = new Node(nodeName);
            Assert.IsFalse(node.Update("b1", npcTxt, tooltipTxt, effect, skillId, true));
            Assert.AreEqual(nodeName, node.UiID);
        }

        [TestMethod]
        public void ChildrenCanBeAdded()
        {
            string root = "root";
            string child = "a";
            Node rootNode = new Node(root);
            rootNode.NextOptions.Add(new Node(child));
            Assert.AreEqual(1, rootNode.NextOptions.Count);
        }

        [TestMethod]
        public void ChildOrderStays()
        {
            string root = "root";
            string childA = "a";
            string childB = "b";
            string childC = "c";
            Node rootNode = new Node(root);
            rootNode.NextOptions.Add(new Node(childA));
            rootNode.NextOptions.Add(new Node(childB));
            rootNode.NextOptions.Add(new Node(childC));

            Assert.AreEqual(3, rootNode.NextOptions.Count);
            Assert.AreEqual(rootNode.NextOptions[0].UiID, childA);
            Assert.AreEqual(rootNode.NextOptions[1].UiID, childB);
            Assert.AreEqual(rootNode.NextOptions[2].UiID, childC);
        }

        [TestMethod]
        public void ChildrenUiIdNameStringIsCorrect()
        {
            StringBuilder builder = new StringBuilder();
            string root = "root";
            string childA = "a";
            string childB = "b";
            string childC = "c";
            builder.AppendLine($"- {root}");
            builder.AppendLine($"- {childA}");
            builder.AppendLine($"- {childB}");
            builder.AppendLine($"- {childC}");
            string expected = builder.ToString();

            Node rootNode = new Node(root);
            rootNode.NextOptions.Add(new Node(childA));
            rootNode.NextOptions.Add(new Node(childB));
            rootNode.NextOptions.Add(new Node(childC));
            string result = rootNode.ChildrenListAsString();
            Assert.AreEqual(expected, result);
        }
    }
}
