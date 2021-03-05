using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UiPath.Shared.Activities.Utilities;
using Indico.RPAActivities.Activities.Activities;
using System.Linq;
using Indico.RPAActivities.Activities.Properties;
using Indico.RPAActivities.Entity;
using IndicoV2.DataSets.Models;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace Indico.RPAActivities.Activities
{
    [LocalizedDisplayName(nameof(Resources.ListDatasets_DisplayName))]
    [LocalizedDescription(nameof(Resources.ListDatasets_Description))]
    public class ListDatasets : IndicoActivityBase<bool, List<IDataSetFull>>
    {
        [LocalizedDisplayName(nameof(Resources.ListDatasets_Datasets_DisplayName))]
        [LocalizedDescription(nameof(Resources.ListDatasets_Datasets_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<List<Dataset>> Datasets { get; set; }


        
        
        protected override bool GetInputs(AsyncCodeActivityContext ctx) => true; // no input, using dummy bool

        protected override async Task<List<IDataSetFull>> ExecuteAsync(bool input, CancellationToken cancellationToken) =>
            (await Application.ListDatasets(cancellationToken)).ToList();

        protected override void SetOutputs(AsyncCodeActivityContext ctx, List<IDataSetFull> output) =>
            Datasets.Set(ctx, output);
    }
}

