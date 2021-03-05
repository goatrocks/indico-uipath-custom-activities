using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Indico.RPAActivities.Activities.Activities;
using Indico.RPAActivities.Activities.Properties;
using UiPath.Shared.Activities.Localization;

namespace Indico.RPAActivities.Activities
{
    [LocalizedDisplayName(nameof(Resources.WorkflowSubmission_DisplayName))]
    [LocalizedDescription(nameof(Resources.WorkflowSubmission_Description))]
    public class WorkflowSubmission : IndicoActivityBase<(int WorkflowId, List<string> FilePaths, List<string> Urls), List<int>>
    {
        [LocalizedDisplayName(nameof(Resources.WorkflowSubmission_WorkflowID_DisplayName))]
        [LocalizedDescription(nameof(Resources.WorkflowSubmission_WorkflowID_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<int> WorkflowID { get; set; }

        [LocalizedDisplayName(nameof(Resources.WorkflowSubmission_FilePaths_DisplayName))]
        [LocalizedDescription(nameof(Resources.WorkflowSubmission_FilePaths_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<List<string>> FilePaths { get; set; }

        [LocalizedDisplayName(nameof(Resources.WorkflowSubmission_Urls_DisplayName))]
        [LocalizedDescription(nameof(Resources.WorkflowSubmission_Urls_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<List<string>> Urls { get; set; }

        [LocalizedDisplayName(nameof(Resources.WorkflowSubmission_SubmissionIDs_DisplayName))]
        [LocalizedDescription(nameof(Resources.WorkflowSubmission_SubmissionIDs_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<List<int>> SubmissionIDs { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (WorkflowID == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(WorkflowID)));

            base.CacheMetadata(metadata);
        }

        protected override (int WorkflowId, List<string> FilePaths, List<string> Urls) GetInputs(AsyncCodeActivityContext ctx)
            => (WorkflowID.Get(ctx), FilePaths.Get(ctx), Urls.Get(ctx));

        protected override async Task<List<int>> ExecuteAsync((int WorkflowId, List<string> FilePaths, List<string> Urls) input, CancellationToken cancellationToken)
            => await Application.WorkflowSubmission(input.WorkflowId, input.FilePaths, input.Urls, cancellationToken);

        protected override void SetOutputs(AsyncCodeActivityContext ctx, List<int> output) => SubmissionIDs.Set(ctx, output);
    }
}

