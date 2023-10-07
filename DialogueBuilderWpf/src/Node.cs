using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DialogueBuilderWpf.src
{
    /// <summary>
    /// Class represents single node inside the tree. 
    /// Dialogue node provides n number of child nodes.
    /// </summary>
    internal class Node
    {
        [JsonInclude]
        /// <summary>
        /// Ui Name to identify node in graphics view.
        /// </summary>
        public string UiID { get; private set; }

        [JsonInclude]
        /// <summary>
        /// Text showed to user after node is entered inside dialogue screen.
        /// </summary>
        public string NpcText { get; private set; }

        [JsonInclude]
        /// <summary>
        /// Text shown to user when node is presented as future option in dialogue.
        /// </summary>
        public string TooltipText { get; private set; }

        [JsonInclude]
        /// <summary>
        /// Node affection direction -1, 0, 1
        /// </summary>
        public int Effect { get; private set; }

        [JsonInclude]
        /// <summary>
        /// Skill ID that this node affects.
        /// </summary>
        public int SkillID { get; private set; }

        [JsonInclude]
        /// <summary>
        /// Does this trigger some action in game?
        /// </summary>
        public bool InvokeActivity { get; private set; }

        [JsonInclude]
        /// <summary>
        /// Child nodes.
        /// </summary>
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
        /// Returns true if properties can be updated otherwise false.
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
        /// Get all child nodes as bullet list string.
        /// </summary>
        /// <returns></returns>
        public string ChildrenListAsString()
        {
            StringBuilder outStrBuilder = new StringBuilder();
            ChildrenListAsStringRec(this, outStrBuilder);
            return outStrBuilder.ToString();
        }

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
