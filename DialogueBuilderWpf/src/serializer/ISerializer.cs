namespace DialogueBuilderWpf.src.serializer
{
    /// <summary>
    /// Interface that all serializers implement.
    /// </summary>
    internal interface ISerializer
    {
        /// <summary>
        /// Serialize data to disk.
        /// </summary>
        /// <param name="projectDir"></param>
        /// <param name="projectName"></param>
        /// <param name="data"></param>
        public void Serialize(string projectDir, string projectName, Node? data);
    }
}
