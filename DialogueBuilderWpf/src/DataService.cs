using DialogueBuilderWpf.src.serializer;
using System.Collections.Generic;
using System.IO;

namespace DialogueBuilderWpf.src
{
    /// <summary>
    /// Class acts as fasade for ui to modify state and to subscribe events about state changes (DataChangeEvent).
    /// Exceptions will bubble up and service user is responsible to handle them. <br></br>
    /// This should be used to interact with program "backend" located in src folder.
    /// </summary>
    internal class DataService
    {
        /// <summary>
        /// Data and project state.
        /// </summary>
        public DialogueTree? Data { get; private set; }

        /// <summary>
        /// Directory where project is stored on disk.
        /// </summary>
        public string? ProjectDir { get; private set; }

        /// <summary>
        /// ProjectName = opened directory, value is extracted from DirPath in initialize methods.
        /// </summary>
        public string? ProjectName { get; private set; }


        public delegate void DataChangedEventHandler();
        public event DataChangedEventHandler? DataChangeEvent;


        public static string GetProjectJsonFilePath(string projectDir, string projectName)
        {
            return Path.Combine(projectDir, $"{projectName}.json");
        }

        /// <summary>
        /// Initializes new empty project.
        /// </summary>
        /// <param name="folderPath"></param>
        public void InitializeNewProject(string folderPath)
        {
            this.Data = new DialogueTree(new Node("root"));
            this.ProjectDir = folderPath;
            this.ProjectName = new DirectoryInfo(folderPath).Name;
            DataChangeEvent?.Invoke();
        }

        /// <summary>
        /// Load saved project from file.
        /// </summary>
        /// <param name="folderPath"></param>
        public void InitializeFromFile(string folderPath)
        {
            this.ProjectDir = folderPath;
            this.ProjectName = new DirectoryInfo(folderPath).Name;
            MJsonSerializer jsonSerializer = new();
            this.Data = jsonSerializer.DeserializeProject(GetProjectJsonFilePath(ProjectDir, ProjectName));
            DataChangeEvent?.Invoke();
        }

        /// <summary>
        /// Get all children uiids as list.
        /// </summary>
        /// <param name="parentUiID"></param>
        /// <returns></returns>
        public List<string>? GetChildrenUiIds(string parentUiID)  => Data!.GetNodeChildrenUIiDS(parentUiID);

        /// <summary>
        /// Search <see cref="Data"/> for node with id.
        /// </summary>
        /// <param name="nodeUiID"></param>
        /// <returns></returns>
        public Node? FindNodeById(string nodeUiID) => Data!.FindNodeById(nodeUiID);

        /// <summary>
        /// Save project to disk.
        /// </summary>
        /// <param name="serializer"></param>
        public void Save(ISerializer serializer) => serializer.Serialize(ProjectDir!, ProjectName!, Data!.Root);

        /// <summary>
        /// Add new node to <see cref="Data"/> tree.
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
        /// Delete node and children from <see cref="Data"/>
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
        /// Update single node values.
        /// </summary>
        /// <param name="targetNodeUiID"></param>
        /// <param name="id"></param>
        /// <param name="npcText"></param>
        /// <param name="tooltipText"></param>
        /// <param name="effect"></param>
        /// <param name="skillId"></param>
        /// <param name="launchesPersuation"></param>
        /// <returns></returns>
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
