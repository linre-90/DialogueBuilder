using System;
using System.Collections.Generic;

namespace DialogueBuilderWpf.src.serializer
{
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

        protected string FormatNodeRelationshipRow(Node parent, Node child)
        {
            // encodes "," to ";" is expected.
            return $"{Guid.NewGuid().ToString()},{parent.UiID.Replace(",", ";")},{child.UiID.Replace(",", ";")}";
        }
    }
}
