namespace DialogueBuilderWpf.src.serializer
{
    /// <summary>
    /// Interface for all serializers.
    /// </summary>
    internal interface ISerializer
    {
        public void Serialize(string projectDir, string projectName, Node? data);
    }
}
