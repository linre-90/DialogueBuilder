using DialogueBuilderWpf.src.serializer;
using System.Collections.Generic;
using System.IO;

namespace DialogueBuilderWpf.src
{
    /// <summary>
    /// Class acts as fasade for ui to modify state and to subscribe events about state changes (DataChangeEvent).
    /// Exceptions will bubble up and service user is responsible to handle them.
    /// <seealso cref="DataChangedEventHandler"/>
    /// </summary>
    internal class DataService
    {
        public DialogueTree? Data { get; private set; }

        public string? ProjectDir { get; private set; }

        /* ProjectName = opened directory, value is extracted from DirPath in initialize methods. */
        public string? ProjectName { get; private set; }


        public delegate void DataChangedEventHandler();

        /// <summary>
        /// Event is fired everytime some action is performed that modifies data and old data is no more relevant.
        /// </summary>
        public event DataChangedEventHandler? DataChangeEvent;

        /// <summary>
        /// Creates new project with only root node.
        /// </summary>
        public void InitializeNewProject(string folderPath)
        {
            this.Data = new DialogueTree(new Node("root"));
            this.ProjectDir = folderPath;
            this.ProjectName = new DirectoryInfo(folderPath).Name;
            DataChangeEvent?.Invoke();
        }

        /// <summary>
        /// Loads project from existing json file that is read from disk.<br></br><br></br>
        /// Triggers DataChangeEvent.
        /// </summary>
        /// <param name="folderPath"></param>
        public void InitializeFromFile(string folderPath)
        {
            this.ProjectDir = folderPath;
            this.ProjectName = new DirectoryInfo(folderPath).Name;
            MJsonSerializer jsonSerializer = new();
            this.Data = jsonSerializer.DeserializeProject(MFileWriter.BuildJsonFilePath(ProjectDir, ProjectName));
            DataChangeEvent?.Invoke();
        }

        /// <summary>
        /// Get all children node UiId of parentUIiD named node.
        /// </summary>
        /// <param name="parentUiID"></param>
        /// <returns></returns>
        public List<string>? GetChildrenUiIds(string parentUiID) 
        {
            return Data!.GetNodeChildrenUIiDS(parentUiID);
        }

        /// <summary>
        /// Find node where UiID matches.
        /// </summary>
        /// <param name="nodeUiID"></param>
        /// <returns></returns>
        public Node? FindNodeById(string nodeUiID)
        {
            return Data!.FindNodeById(nodeUiID);
        }

        /// <summary>
        /// Save or export data with given ISerializer.
        /// </summary>
        /// <param name="serializer"></param>
        public void Save(ISerializer serializer)
        {
            serializer.Serialize(ProjectDir!, ProjectName!, Data!.Root);
        }

        /// <summary>
        /// Create new node as the parentId node's child.<br></br><br></br>
        /// Triggers DataChangeEvent.
        /// </summary>
        /// <param name="parentUiID"></param>
        public void AddNewNodeToParent(string parentUiID)
        {
            Node? node = FindNodeById(parentUiID);
            if(node != null)
            {
                Data!.AddNode(node);
                DataChangeEvent?.Invoke();
            }
        }

        /// <summary>
        /// Delete node and all of its children.<br></br><br></br>
        /// Triggers DataChangeEvent.
        /// </summary>
        /// <param name="targetUiID"></param>
        public void DeleteNodeBranch(string targetUiID)
        {
            Node? node = FindNodeById(targetUiID);
            if (node != null)
            {
                Data!.RemoveNodeBranch(node);
                DataChangeEvent?.Invoke();
            }
        }

        /// <summary>
        /// Update single node value.
        /// Returns true on success, false if values cannot be updated. Root node UiID cannot be updated.<br></br><br></br>
        /// Triggers DataChangeEvent.
        /// </summary>
        /// <param name="newValues"></param>
        public bool UpdateNodeValues(string targetNodeUiID, string id, string npcText, string tooltipText, string effect, string skillId, bool launchesPersuation)
        {
            Node? node = FindNodeById(targetNodeUiID);
            if (node != null)
            {
                node.Update(id, npcText, tooltipText, effect, skillId, launchesPersuation);
                DataChangeEvent?.Invoke();
                return true;
            }
            return false;
        }
    }
}
