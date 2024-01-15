using System;
using System.Collections.Generic;

namespace DialogueBuilderWpf.src.serializer
{
    /// <summary>
    /// Class provides methods for formatting tree nodes and relationships to csv row.
    /// </summary>
    internal class CsvRowFormatter
    {
        protected void ConvertTreeToCsvRows(Node node, List<string> dataCsv, List<string> relationShipCsv)
        {
            if (node == null) return;
            dataCsv.Add(FormatNodeDataRow(node));

            for (int i = 0; i < node.NextOptions.Count; i++)
            {
                relationShipCsv.Add(FormatNodeRelationshipRow(node, node.NextOptions[i]));
                ConvertTreeToCsvRows(node.NextOptions[i], dataCsv, relationShipCsv);
            }
        }

        /// <summary>
        /// Format node to csv row. Replaces ',' with ';' when csv is used.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected string FormatNodeDataRow(Node node)
        {
            // encodes "," to ";" is expected.
            return $"{node.UiID.Replace(",", ";")}" +
                $",{node.NpcText.Replace(",", ";")}" +
                $",{node.TooltipText.Replace(",", ";")}" +
                $",{node.Effect}" +
                $",{node.SkillID}" +
                $",{node.InvokeActivity}";
        }
        /// <summary>
        /// Format node children to csv row. Replaces ',' with ';' when csv is used.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        protected string FormatNodeRelationshipRow(Node parent, Node child)
        {
            // encodes "," to ";" is expected.
            return $"{Guid.NewGuid().ToString()},{parent.UiID.Replace(",", ";")},{child.UiID.Replace(",", ";")}";
        }
    }
}
