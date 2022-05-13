using System;
using System.IO;

namespace DIN4000ImportPlugin
{
    class Program
    {

        static void Main(string[] args)
        {

            StartArgs startArgs = new StartArgs();
            startArgs.Parse(args);
            startArgs.UnZipCsvFiles();
            if (startArgs.CsvFiles.Count > 0)
            {
                var recs = new List<ToolRecord>();
                foreach (string csvFileName in startArgs.CsvFiles)
                    DIN4000CsvReader.ReadCsvFile(csvFileName, recs);

                using (ToolFromCsvMaker toolMaker = new ToolFromCsvMaker(startArgs.SCInstallFolder))
                {
                    toolMaker.ImportRecsToDB(recs, startArgs.ResultDBFile);
                    Console.WriteLine("Imported tools count: " + toolMaker.ImportedToolsCount);
                }
            }
            startArgs.ClearUnzippedCsvFiles();
        }
    }
}
