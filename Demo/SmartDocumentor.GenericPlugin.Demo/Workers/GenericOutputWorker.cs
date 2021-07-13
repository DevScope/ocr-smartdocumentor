using SmartDocumentor.Core.Schemas.Task;
using SmartDocumentor.Core.Storage;
using SmartDocumentor.Core.Workers.Builtin;
using SmartDocumentor.GenericPlugin.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartDocumentor.GenericPlugin.Demo.Workers
{
    public class GenericOutputWorker : BaseWorker
    {
        public string OutputFolder;
        public List<Field> Fields;
        private IDocumentsStorage GlobalStorage;

        protected override void InitializeWorkerMain()
        {
            if (!WorkerSettings.TryGetValue("OutputFolder", out OutputFolder))
            {
                throw new ArgumentException("Missing argument 'OutputFolder'");
            }

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
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            var documentPath = Path.Combine(this.GlobalStorage.Connection.BasePath, item.Document.ID);
            var pdfFileName = Path.GetFileNameWithoutExtension(item.ID) + ".pdf";

            var docData = new StringBuilder();
            docData.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            docData.AppendLine("<Document>");
            docData.AppendLine($"   <Filename>{pdfFileName}</Filename>");

            foreach (var field in this.Fields)
            {
                docData.AppendLine($"   <{GetValidName(field.Name)}>{item.GetPropertyValue(field.Name)}</{GetValidName(field.Name)}>");
            }

            docData.AppendLine("</Document>");

            var outputFile = Path.Combine(this.OutputFolder, item.ID);
            File.WriteAllText(outputFile, docData.ToString(), Encoding.UTF8);

            var pdfOutputFile = Path.Combine(this.OutputFolder, pdfFileName);

            GenericPlugin.Utils.Helper.GeneratePDFToFile(documentPath, pdfOutputFile);
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