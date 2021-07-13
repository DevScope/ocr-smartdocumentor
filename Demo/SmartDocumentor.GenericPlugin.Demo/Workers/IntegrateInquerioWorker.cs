using SmartDocumentor.Core.Schemas.Task;
using SmartDocumentor.Core.Workers.Builtin;

namespace SmartDocumentor.GenericPlugin.Demo.Workers
{
    public class IntegrateInquerioWorker : BaseWorker
    {
        protected override void InitializeWorkerMain()
        {
            base.InitializeWorkerMain();

            // Custom
        }

        public override void ProcessItem(SDTask item)
        {
        }
    }
}