using System;
using System.IO;
using System.Text.Json;

namespace DialogueBuilderWpf.src.serializer
{
    internal class MJsonSerializer: ISerializer
    {
        public void Serialize(string projectDir, string projectName, Node? data)
        {
            if (data == null) throw new Exception("Json serializer data is missing.");
            string json = JsonSerializer.Serialize(data);
            MFileWriter.WriteJsonFile(json, projectDir, projectName);
        }

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
