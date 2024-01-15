using System.Collections.Generic;

namespace DialogueBuilderWpf.src
{
    /// <summary>
    /// Dialogue tree is wrapper to use, query and alternate managed nodes.
    /// </summary>
    internal class DialogueTree
    {
        public Node Root { get; private set; }

        public DialogueTree(Node root)
        {
            this.Root = root;
        }

        /* Helper for collecting recursion function results */
        private class RecursionOutParentChild
        {
            public Node? parent;
            public Node? child;
        }

        /* Helper for collecting recursion function results */
        private class RecursionOutChildUiIDS
        {
            public List<string> childrenUiIDs = new List<string>();
        }

        /* Helper for collecting recursion function results */
        private class RecursionOutSingleNode
        {
            public Node? node;
        }

        /// <summary>
        /// Add new node to the tree.
        /// </summary>
        /// <param name="target"></param>
        public void AddNode(Node target) => target.NextOptions.Add(new Node("unnamed"));

        /// <summary>
        /// Remove node and all children nodes.
        /// </summary>
        /// <param name="target"></param>
        public void RemoveNodeBranch(Node target)
        {
            RecursionOutParentChild result = new RecursionOutParentChild();
            RemoveNodeBranchRec(target, Root, Root, result);
            if (result.parent != null && result.child != null)
            {
                result.parent.NextOptions.Remove(result.child);
            }
        }

        /// <summary>
        /// Find delete target parent in tree. Recursive.
        /// </summary>
        /// <param name="deleteTarget"></param>
        /// <param name="current"></param>
        /// <param name="currentParent"></param>
        /// <param name="nodeDelete"></param>
        private void RemoveNodeBranchRec(Node deleteTarget, Node current, Node currentParent, RecursionOutParentChild nodeDelete)
        {
            if (current == null) return;

            if (deleteTarget.UiID.Equals(current.UiID))
            {
                nodeDelete.parent = currentParent;
                nodeDelete.child = current;
                return;
            }

            currentParent = current;

            for (int i = 0; i < current.NextOptions.Count; i++)
            {
                RemoveNodeBranchRec(deleteTarget, current.NextOptions[i], currentParent, nodeDelete);
            }
        }

        /// <summary>
        /// Get all children ids as list.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<string> GetNodeChildrenUIiDS(string parentId)
        {
            RecursionOutChildUiIDS result = new RecursionOutChildUiIDS();
            GetNodeChildrenUIiDSRec(parentId, Root, result);
            return result.childrenUiIDs;
        }

        /// <summary>
        /// Recursive method to find node children ids.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="rootNode"></param>
        /// <param name="childUiIDs"></param>
        private void GetNodeChildrenUIiDSRec(string parentId, Node rootNode, RecursionOutChildUiIDS childUiIDs)
        {
            if (rootNode == null) return;

            if (rootNode.UiID.Equals(parentId))
            {
                foreach (Node item in rootNode.NextOptions)
                {
                    childUiIDs.childrenUiIDs.Add(item.UiID);
                }
                return;
            }

            for (int i = 0; i < rootNode.NextOptions.Count; i++)
            {
                GetNodeChildrenUIiDSRec(parentId, rootNode.NextOptions[i], childUiIDs);
            }
        }

        /// <summary>
        /// Find node from the tree based on id.
        /// </summary>
        /// <param name="UiID"></param>
        /// <returns></returns>
        public Node? FindNodeById(string UiID)
        {
            RecursionOutSingleNode result = new RecursionOutSingleNode();
            FindNodeByIdRec(UiID, Root, result);
            return result.node;
        }

        /// <summary>
        /// Recursively find node from the tree by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="node"></param>
        /// <param name="outNode"></param>
        private void FindNodeByIdRec(string id, Node node, RecursionOutSingleNode outNode)
        {
            if (node == null) return;
            if (node.UiID.Equals(id))
            {
                outNode.node = node;
                return;
            }

            for (int i = 0; i < node.NextOptions.Count; i++)
            {
                FindNodeByIdRec(id, node.NextOptions[i], outNode);
            }
        }
    }
}
