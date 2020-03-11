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
        protected override void InitializeWorkerMain()
        {
            base.InitializeWorkerMain();

            // Custom
        }

        public override void ProcessItem(SDTask item)
        {
            base.ProcessItem(item);

            this.ExtractTableData(item);

            base.SetTaskData(item);
        }

        private void ExtractTableData(SDTask item)
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
                        var descricao = ocrJobResult.TextLines[i + 1].Text; // O OCR está a partir a desc para a segunda linhas

                        lines.Add(linha);
                    }

                }
            }

            item.SetPropertyValue(Constants.Campos.CustomTable, SerializationHelper.SerializeCompress(lines));
        }
    }
}
