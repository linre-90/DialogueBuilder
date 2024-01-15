using System;
using System.IO;
using System.Text.Json;

namespace DialogueBuilderWpf.src.serializer
{
    /// <summary>
    /// Class handles json serialization to and from disk.
    /// </summary>
    internal class MJsonSerializer: ISerializer
    {
        /// <summary>
        /// Save project tree to json file.
        /// </summary>
        /// <param name="projectDir"></param>
        /// <param name="projectName"></param>
        /// <param name="data"></param>
        /// <exception cref="Exception"></exception>
        public void Serialize(string projectDir, string projectName, Node? data)
        {
            if (data == null) throw new Exception("Json serializer data is missing.");
            string json = JsonSerializer.Serialize(data);
            MFileWriter.WriteJsonFile(json, projectDir, projectName);
        }

        /// <summary>
        /// Load project tree from json file.
        /// </summary>
        /// <param name="projectJsonPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DialogueTree DeserializeProject(string projectJsonPath)
        {
            string data = File.ReadAllText(projectJsonPath);
            
            Node? node = JsonSerializer.Deserialize<Node>(data);

            if (node != null)
            {
                DialogueTree dialogueTree = new DialogueTree(node);
                return dialogueTree;
            }

            throw new Exception("Json serializer data is missing.");
            
        }
    }
}
