using DialogueBuilderWpf.src;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace DialogueBuilderWpf.ui
{
    internal class NodeEditorViewModel
    {
        /// <summary>
        /// Data about node editor "boxes" UIiD of the node is key and value is position in canvas(nodeEditor).
        /// </summary>
        public Dictionary<string, Point> NodeTreeNodes { get; private set; }

        public NodeEditorViewModel()
        {
            NodeTreeNodes = new Dictionary<string, Point>();
        }

        /// <summary>
        /// Re creates NodeEditorViewModel state.
        /// </summary>
        /// <param name="root"></param>
        public void UpdateUiState(Node root)
        {
            Dictionary<string, Point> outNodeTreeNodesState = new Dictionary<string, Point>();
            UpdateUiStateRec(root, NodeTreeNodes, outNodeTreeNodesState);
            this.NodeTreeNodes = outNodeTreeNodesState;
        }

        private void UpdateUiStateRec(Node node, Dictionary<string, Point> nodeTreeNodes, Dictionary<string, Point> outNodeTreeNodes)
        {
            if (node == null) return;

            if (nodeTreeNodes.ContainsKey(node.UiID))
            {
                Point value;
                nodeTreeNodes.TryGetValue(node.UiID, out value);
                outNodeTreeNodes.Add(node.UiID, value);
            }
            else
            {
                // Node is new and it has not been drawn once put it in top left corner
                outNodeTreeNodes.Add(node.UiID, new Point(0, 0));
            }

            for (int i = 0; i < node.NextOptions.Count; i++)
            {
                UpdateUiStateRec(node.NextOptions[i], nodeTreeNodes, outNodeTreeNodes);
            }
        }

        /// <summary>
        /// Renames key in <seealso cref="NodeTreeNodes"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="newKey"></param>
        public void RenameKey(string target, string newKey)
        {
            Point position = new Point() { 
                X = NodeTreeNodes[target].X, 
                Y = NodeTreeNodes[target].Y };

            NodeTreeNodes.Remove(target);
            NodeTreeNodes.Add(newKey, position);
        }

        /// <summary>
        /// Updates [key] elements position[point].
        /// </summary>
        /// <param name="key"></param>
        /// <param name="point"></param>
        public void UpdateEntryPosition(string key, Point point) => NodeTreeNodes[key] = point;


        /// <summary>
        /// Save node editor layout to disk.
        /// </summary>
        /// <param name="dataService"></param>
        public void Save(DataService dataService)
        {
            string json = JsonSerializer.Serialize(NodeTreeNodes);
            File.WriteAllText(Path.Combine(dataService.ProjectDir!, $"ui_{dataService.ProjectName!}.json"), json);
        }

        /// <summary>
        /// Load node layout from disk.
        /// </summary>
        /// <param name="dataService"></param>
        public void Load(DataService dataService)
        {
            string json = File.ReadAllText(Path.Combine(dataService.ProjectDir!, $"ui_{dataService.ProjectName!}.json"));
            Dictionary<string, Point>? temp = JsonSerializer.Deserialize<Dictionary<string, Point>>(json);
            if(temp != null)
            {
                NodeTreeNodes = temp;
            }
        }
    }
}
