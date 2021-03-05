﻿using System.Activities;
using Indico.RPAActivities.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.RuntimeSimple;
using UiPath.Shared.Activities.Utilities;

namespace Indico.RPAActivities.Activities.Activities
{
    public abstract  class IndicoActivityBase<TInput, TOutput> : TaskActivity<TInput, TOutput>
    {
        protected Application Application { get; private set; }

        protected IndicoActivityBase()
        {
            Constraints.Add(ActivityConstraints.HasParentType<ListDatasets, IndicoScope>(string.Format(Resources.ValidationScope_Error, Resources.IndicoScope_DisplayName)));
        }

        protected override void Init(AsyncCodeActivityContext context)
        {
            base.Init(context);

            var objectContainer = context.GetFromContext<IObjectContainer>(IndicoScope.ParentContainerPropertyTag);
            Application = objectContainer.Get<Application>();
        }
    }
}
