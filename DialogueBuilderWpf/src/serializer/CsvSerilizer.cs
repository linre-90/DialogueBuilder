using System;
using System.Collections.Generic;

namespace DialogueBuilderWpf.src.serializer
{
    /// <summary>
    /// Builds two data tables. <br></br><br></br>
    /// "Data" table contains only node properties it does not contain relationships "primary key" is UiID.
    /// Relationships data table stores information only about node relationships and keys parent UiID to child UiID.
    /// </summary>
    internal class CsvSerilizer: ISerializer
    {

        /// <summary>
        /// Saves data to csv files that are written in {project}/export folder.
        /// </summary>
        /// <param name="projectDir"></param>
        /// <param name="projectName"></param>
        /// <param name="data"></param>
        public void Serialize(string projectDir, string projectName , Node? data)
        {
            if (data == null) throw new Exception("Data is missing from csv serializer.");
            // Note about unreal csv format. First column name must be empty that is why header row starts with ",".
            // Unreal automatically defines first column as "Name".

            List<string> dataCsvRows = new () { ",NpcText,TooltipText,Effect,SkillID,InvokeActivity" };
            List<string>  relationShipCsvRows = new () { ",Parent,Child" };

            ConvertTreeToCsvRows(data, dataCsvRows, relationShipCsvRows);
            MFileWriter.WriteCsvFiles(dataCsvRows, relationShipCsvRows, projectDir, projectName);
        }

        private void ConvertTreeToCsvRows(Node node, List<string> dataCsv, List<string> relationShipCsv)
        {
            if (node == null) return;
            dataCsv.Add(FormatNodeDataRow(node));

            for (int i = 0; i < node.NextOptions.Count; i++)
            {
                relationShipCsv.Add(FormatNodeRelationshipRow(node, node.NextOptions[i]));
                ConvertTreeToCsvRows(node.NextOptions[i], dataCsv, relationShipCsv);
            }
        }

        private string FormatNodeDataRow(Node node)
        {
            // Overriding "," to ";" is expected.
            // Unreal does not allow specifying column seperator when importing file to engine.
            return $"{node.UiID.Replace(",", ";")}" +
                $",{node.NpcText.Replace(",", ";")}" +
                $",{node.TooltipText.Replace(",", ";")}" +
                $",{node.Effect}" +
                $",{node.SkillID}" +
                $",{node.InvokeActivity}";
        }

        private string FormatNodeRelationshipRow(Node parent, Node child)
        {
            // Overriding "," to ";" is expected.
            // Unreal does not allow specifying column seperator when importing file to engine.
            return $"{Guid.NewGuid().ToString()},{parent.UiID.Replace(",", ";")},{child.UiID.Replace(",", ";")}";
        }
    }
}
