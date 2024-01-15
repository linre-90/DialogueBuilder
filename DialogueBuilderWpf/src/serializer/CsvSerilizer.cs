using System;
using System.Collections.Generic;

namespace DialogueBuilderWpf.src.serializer
{
    /// <summary>
    /// Serializer that saves data in standart csv format to disk.
    /// </summary>
    internal class CsvSerilizer: CsvRowFormatter, ISerializer
    {
        public void Serialize(string projectDir, string projectName , Node? data)
        {
            if (data == null) throw new Exception("Data is missing from csv serializer.");

            List<string> dataCsvRows = new () { "UiID,NpcText,TooltipText,Effect,SkillID,InvokeActivity" };
            List<string>  relationShipCsvRows = new () { "ID,Parent,Child" };

            base.ConvertTreeToCsvRows(data, dataCsvRows, relationShipCsvRows);

            MFileWriter.WriteCsvFiles(dataCsvRows, relationShipCsvRows, projectDir, projectName, "CSV");
        }
    }
}
