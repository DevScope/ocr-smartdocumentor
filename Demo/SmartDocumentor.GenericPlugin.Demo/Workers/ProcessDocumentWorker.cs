using SmartDocumentor.Common.Serialization;
using SmartDocumentor.Core.Schemas.Task;
using SmartDocumentor.Core.Workers.Builtin;
using SmartDocumentor.GenericPlugin.Demo.Base;
using SmartDocumentor.GenericPlugin.Utils;
using SmartDocumentor.GenericPlugin.Workers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartDocumentor.GenericPlugin.Demo.Workers
{
    public class ProcessDocumentWorker : BaseProcessDocumentWorker
    {
        private int MinConfidence;
        private string ConfidencePropertyName;
        private bool ConfidenceEnabled;

        protected override void InitializeWorkerMain()
        {
            base.InitializeWorkerMain();

            // Custom
            if (WorkerSettings.TryGetValue("MinConfidence", out string minConfidenceString))
            {
                if (!int.TryParse(minConfidenceString, out MinConfidence))
                {
                    throw new ArgumentException($"Invalid argument 'MinConfidence'. Value: '{minConfidenceString}'.");
                }
            }

            if (WorkerSettings.TryGetValue("ConfidencePropertyName", out ConfidencePropertyName))
            {
            }

            if (WorkerSettings.TryGetValue("ConfidenceEnabled", out string confidenceEnabledString))
            {
                if (!bool.TryParse(confidenceEnabledString, out ConfidenceEnabled))
                {
                    throw new ArgumentException($"Invalid argument 'ConfidenceEnabled'. Value: '{confidenceEnabledString}'.");
                }
            }
        }

        public override void ProcessItem(SDTask item)
        {
            base.ProcessItem(item);

            this.ExtractCustomTableData(item);

            this.CheckConfidence(item);

            base.SetTaskData(item);
        }

        private void ExtractCustomTableData(SDTask item)
        {
            List<string> lines = new List<string>();

            foreach (var ocrJobResult in base.OcrJobResultList)
            {
                for (int i = 0; i < ocrJobResult.TextLines.Count; i++)
                {
                    var textLine = ocrJobResult.TextLines[i];

                    var regexResult = Constants.RegexLine.Match(textLine.Text);
                    if (regexResult.Success)
                    {
                        var linha = regexResult.Groups["Linha"].ToString();
                        var codigo = regexResult.Groups["Codigo"].ToString();

                        lines.Add(codigo);
                    }

                }
            }

            item.SetPropertyValue(Constants.Campos.CustomTable, SerializationHelper.SerializeCompress(lines));
        }

        private void CheckConfidence(SDTask item)
        {
            if (!ConfidenceEnabled)
            {
                return;
            }

            var confidenceAverage = this.ExtractedFieldList.Select(c => c.Entity?.Confidence ?? 0).DefaultIfEmpty(0).Average();
            if (confidenceAverage > MinConfidence)
            {
                item.SetPropertyValue(ConfidencePropertyName, bool.TrueString);

            }
            else
            {
                item.SetPropertyValue(ConfidencePropertyName, bool.FalseString);
            }
        }
    }
}
