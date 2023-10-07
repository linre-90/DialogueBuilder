using DialogueBuilderWpf.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueBuilderWpf.Test
{
    public class MTestUtils
    {
        internal static DialogueTree SetupTestTree()
        {
            // Build tree to test against
            // root
            DialogueTree tree = new DialogueTree(new Node("root"));
            // add two children to root
            tree.AddNode(tree.Root);
            tree.AddNode(tree.Root);
            tree.Root.NextOptions[0].Update("a1", "", "", "-1", "1", false);
            tree.Root.NextOptions[1].Update("b1", "", "", "-1", "1", false);
            // add 3 children to a1
            tree.AddNode(tree.Root.NextOptions[0]);
            tree.AddNode(tree.Root.NextOptions[0]);
            tree.AddNode(tree.Root.NextOptions[0]);
            tree.Root.NextOptions[0].NextOptions[0].Update("a2", "", "", "-1", "1", false);
            tree.Root.NextOptions[0].NextOptions[1].Update("b2", "", "", "-1", "1", false);
            tree.Root.NextOptions[0].NextOptions[2].Update("c2", "", "", "-1", "1", false);
            // add 3 children to b1
            tree.AddNode(tree.Root.NextOptions[1]);
            tree.AddNode(tree.Root.NextOptions[1]);
            tree.AddNode(tree.Root.NextOptions[1]);
            tree.Root.NextOptions[1].NextOptions[0].Update("d2", "", "", "-1", "1", false);
            tree.Root.NextOptions[1].NextOptions[1].Update("e2", "", "", "-1", "1", false);
            tree.Root.NextOptions[1].NextOptions[2].Update("f2", "", "", "-1", "1", false);
            return tree;
        }

    }
}
