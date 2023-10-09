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
        public event DataChangedEventHandler? DataChangeEvent;


        public static string GetProjectJsonFilePath(string projectDir, string projectName)
        {
            return Path.Combine(projectDir, $"{projectName}.json");
        }

        public void InitializeNewProject(string folderPath)
        {
            this.Data = new DialogueTree(new Node("root"));
            this.ProjectDir = folderPath;
            this.ProjectName = new DirectoryInfo(folderPath).Name;
            DataChangeEvent?.Invoke();
        }


        public void InitializeFromFile(string folderPath)
        {
            this.ProjectDir = folderPath;
            this.ProjectName = new DirectoryInfo(folderPath).Name;
            MJsonSerializer jsonSerializer = new();
            this.Data = jsonSerializer.DeserializeProject(GetProjectJsonFilePath(ProjectDir, ProjectName));
            DataChangeEvent?.Invoke();
        }

        public List<string>? GetChildrenUiIds(string parentUiID)  => Data!.GetNodeChildrenUIiDS(parentUiID);


        public Node? FindNodeById(string nodeUiID) => Data!.FindNodeById(nodeUiID);

        public void Save(ISerializer serializer) => serializer.Serialize(ProjectDir!, ProjectName!, Data!.Root);

        public void AddNewNodeToParent(string parentUiID)
        {
            Node? node = FindNodeById(parentUiID);
            if(node != null)
            {
                Data!.AddNode(node);
                DataChangeEvent?.Invoke();
            }
        }

        public void DeleteNodeBranch(string targetUiID)
        {
            Node? node = FindNodeById(targetUiID);
            if (node != null)
            {
                Data!.RemoveNodeBranch(node);
                DataChangeEvent?.Invoke();
            }
        }

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
