using System;
using System.Collections.Generic;

namespace DialogueBuilderWpf.src.serializer
{

    internal class UeCsvSerilizer: CsvRowFormatter, ISerializer
    {

        public void Serialize(string projectDir, string projectName , Node? data)
        {
            if (data == null) throw new Exception("Data is missing from csv serializer.");
            // Note about unreal csv format. First column name must be empty that is why header row starts with ",".
            // Unreal automatically defines first column as "Name".

            List<string> dataCsvRows = new () { ",NpcText,TooltipText,Effect,SkillID,InvokeActivity" };
            List<string>  relationShipCsvRows = new () { ",Parent,Child" };

            base.ConvertTreeToCsvRows(data, dataCsvRows, relationShipCsvRows);

            MFileWriter.WriteCsvFiles(dataCsvRows, relationShipCsvRows, projectDir, projectName, "UE");
        }
    }
}
