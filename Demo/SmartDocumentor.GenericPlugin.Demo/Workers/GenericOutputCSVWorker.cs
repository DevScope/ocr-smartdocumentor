using SmartDocumentor.Core.Schemas.Task;
using SmartDocumentor.Core.Storage;
using SmartDocumentor.Core.Workers.Builtin;
using SmartDocumentor.GenericPlugin.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocumentor.GenericPlugin.Demo.Workers
{
    public class GenericOutputCSVWorker : BaseWorker
    {
        public string OutputFolder;
        private string OutputFileName;
        public List<Field> Fields;
        IDocumentsStorage GlobalStorage;
        private bool GeneratePdf;

        protected override void InitializeWorkerMain()
        {
            if (!WorkerSettings.TryGetValue("OutputFolder", out OutputFolder))
            {
                throw new ArgumentException("Missing argument 'OutputFolder'");
            }

            if (!WorkerSettings.TryGetValue("OutputFileName", out OutputFileName))
            {
                throw new ArgumentException("Missing argument 'OutputFileName'");
            }

            if (!WorkerSettings.TryGetValue("GeneratePdf", out string GeneratePdf))
            {
                throw new ArgumentException("Missing argument 'GeneratePdf'");
            }

            this.GeneratePdf = bool.TrueString.Equals(GeneratePdf, StringComparison.InvariantCultureIgnoreCase);

            if (!WorkerSettings.TryGetValue("ConfigFileName", out string configFileName))
            {
                throw new ArgumentException("Missing argument 'ConfigFileName'");
            }

            if (!WorkerSettings.TryGetValue("PluginId", out string pluginId))
            {
                throw new ArgumentException("Missing argument 'PluginId'");
            }

            this.Fields = FieldUtils.GetFieldsFromXml(configFileName, pluginId).ToList();

            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }

            this.GlobalStorage = this.Runtime.GetWorkspaceDocsStorage(this.Runtime.CurrentWorkspace.ID);
        }

        public override void ProcessItem(SDTask item)
        {
            var documentPath = Path.Combine(this.GlobalStorage.Connection.BasePath, item.Document.ID);
            var pdfFileName = Path.GetFileNameWithoutExtension(item.ID) + ".pdf";

            var docData = new StringBuilder();
            var docHeader = new StringBuilder();

            foreach (var field in this.Fields.OrderBy(c => c.OrderId))
            {
                docHeader.Append($"{field.Label};");
                docData.Append($"{item.GetPropertyValue(field.Name)};");
            }

            if (this.GeneratePdf)
            {
                docHeader.Append("File");
                docData.Append(pdfFileName);

                var pdfOutputFile = Path.Combine(this.OutputFolder, pdfFileName);

                if (File.Exists(pdfOutputFile))
                    File.Delete(pdfOutputFile);

                Utils.Helper.GeneratePDFToFile(documentPath, pdfOutputFile);
            }

            var outputFile = Path.Combine(this.OutputFolder, this.OutputFileName);

            if (!File.Exists(outputFile))
                File.WriteAllText(outputFile, docHeader.ToString() + Environment.NewLine, Encoding.UTF8);

            File.AppendAllText(outputFile, docData.ToString() + Environment.NewLine, Encoding.UTF8);
        }

        private string GetValidName(string fileName)
        {
            foreach (var item in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(item.ToString(), string.Empty);
            }

            fileName = fileName.Replace(" ", string.Empty);

            return fileName;
        }
    }
}
