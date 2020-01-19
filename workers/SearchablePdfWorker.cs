using System;
using System.Collections.Generic;
using System.IO;
using SmartDocumentor.Common.Usings;
using SmartDocumentor.Core.Schemas.Task;
using SmartDocumentor.Core.Storage;
using SmartDocumentor.ImageProcessing.Core;
using SmartDocumentor.ImageProcessing.Pdf;
using SmartDocumentor.Languages;

namespace SmartDocumentor.Core.Workers.Builtin
{
    public class SearchablePdfWorkerSettingsAttribute : WorkerSettingsAttributeBase
    {
        public override IEnumerable<WorkerSettingPropertyField> SettingsFields
        {
            get
            {
                yield return
                    WorkerSettingPropertyField.CreateField("AutoDeskew", "Behavior", true,
                        "Auto deskew",
                        LanguageResourcesManager.GetResourceByName("PROPERTY_WORKER_AutoDeskew"));
                yield return
                    WorkerSettingPropertyField.CreateField("UseOcrServices", "Behavior", false,
                        "Use OCR Services",
                        LanguageResourcesManager.GetResourceByName("PROPERTY_WORKER_UseOcrServices"));
            }
        }
    }

    [SearchablePdfWorkerSettings]
    public class SearchablePdfWorker : BaseWorker
    {
        private bool _autoDeskew;
        private bool _useOcrServices;

        protected override void InitializeWorkerMain()
        {
            if (!bool.TryParse(GetWorkerSettingValue("AutoDeskew"), out _autoDeskew))
                _autoDeskew = true;

            if (!bool.TryParse(GetWorkerSettingValue("UseOcrServices"), out _useOcrServices))
                _useOcrServices = false;
        }

        /// <summary>
        ///     Worker that handles pdf generation.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="worker"></param>
        public override void ProcessItem(SDTask item)
        {
            if (item.Document == null)
                throw new ApplicationException("Document cannot be null");

            var currentDocsStorage = Runtime.GetDocumentsStorageByID(item.Document.StorageID);

            var tempInputFilename = StorageHelper.CloneToTempFile(currentDocsStorage, item.Document.ID);

            if (string.IsNullOrEmpty(tempInputFilename))
                throw new ApplicationException($"SearchablePdfWorker : StorageHelper.CloneToTempFile failed. Workspace: {item.WorkspaceID} TaskId: {item.ID} DocumentId: {item.Document.ID}");

            using (new TempFile(tempInputFilename))
            {
                var tempOutputFilename = Path.ChangeExtension(tempInputFilename, ".pdf");

                using (new TempFile(tempOutputFilename))
                {
                    using (var imageDoc = new ImageDocument(tempInputFilename))
                    {
                        var jobRequest = new SearchablePdfJobRequest
                        {
                            AutoDeskewPages = _autoDeskew,
                            OutputFile = tempOutputFilename,
                            OutputType = SearchablePdfJobOutputType.File,
                            DataSource = new SearchablePdfJobDataSource
                            {
                                InputDocument = imageDoc,
                                SourceType = SearchablePdfJobDataSourceType.ImageDocument
                            }
                        };

                        var pdfJobResult = SearchablePdf.CreatePdf(jobRequest);

                        item.Document.ID = currentDocsStorage.Put(tempOutputFilename);
                    }
                }
            }
        }
    }
}