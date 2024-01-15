using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DialogueBuilderWpf.src
{
    /// <summary>
    /// Node is json serializable data storage entity that stores single dialogue in program.<br></br>
    /// Connecting single nodes to other nodes creates tree structure that forms whole conversation.
    /// </summary>
    internal class Node
    {
        [JsonInclude]
        public string UiID { get; private set; }

        [JsonInclude]
        public string NpcText { get; private set; }

        [JsonInclude]
        public string TooltipText { get; private set; }

        [JsonInclude]
        public int Effect { get; private set; }

        [JsonInclude]
        public int SkillID { get; private set; }

        [JsonInclude]
        public bool InvokeActivity { get; private set; }

        [JsonInclude]
        public List<Node> NextOptions { get; private set; }


        public Node(string uiID)
        {
            this.UiID = uiID;
            this.NpcText = "";
            this.TooltipText = "";
            this.Effect = 0;
            this.InvokeActivity = false;
            this.SkillID = -1;
            NextOptions = new List<Node>();
        }

        /// <summary>
        /// Update node values to new parameter values.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="npcText"></param>
        /// <param name="tooltipText"></param>
        /// <param name="effect"></param>
        /// <param name="skillId"></param>
        /// <param name="launchesPersuation"></param>
        /// <returns></returns>
        public bool Update(string id, string npcText, string tooltipText, string effect, string skillId, bool launchesPersuation)
        {
            bool validEffectDirection = (effect.Equals("-1") || effect.Equals("0") || effect.Equals("1"));

            if(int.TryParse(skillId, out _) && validEffectDirection)
            {
                // Cannot update root node id
                if (!id.Equals("root") && !UiID.Equals("root"))
                {
                    UiID = id;
                }

                NpcText = npcText;
                TooltipText = tooltipText;
                Effect = int.Parse(effect);
                SkillID = int.Parse(skillId);
                InvokeActivity = launchesPersuation;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get all children that this node has in string format.
        /// </summary>
        /// <returns></returns>
        public string ChildrenListAsString()
        {
            StringBuilder outStrBuilder = new StringBuilder();
            ChildrenListAsStringRec(this, outStrBuilder);
            return outStrBuilder.ToString();
        }

        /// <summary>
        /// Travel all children recursively to form string representation of children.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="outStrBuilder"></param>
        private void ChildrenListAsStringRec(Node node, StringBuilder outStrBuilder)
        {
            if (node == null) return;
            outStrBuilder.AppendLine($"- {node.UiID}");

            if (node.NextOptions.Count <= 0) return;
            for (int i = 0; i < node.NextOptions.Count; i++)
            {
                ChildrenListAsStringRec(node.NextOptions[i], outStrBuilder);
            }
        }
    }
}
