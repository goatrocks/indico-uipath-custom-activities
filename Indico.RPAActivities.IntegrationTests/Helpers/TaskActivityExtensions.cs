﻿using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using Indico.RPAActivities.Activities;
using IndicoV2.DataSets.Models;
using UiPath.Shared.Activities.RuntimeSimple;

namespace Indico.RPAActivities.IntegrationTests.Helpers
{
    internal static class TaskActivityExtensions
    {
        private static string BaseUrl => Environment.GetEnvironmentVariable("INDICO_HOST");
        private static string ApiToken => Environment.GetEnvironmentVariable("INDICO_TOKEN");

        public static List<IDataSetFull> Invoke(this ListDatasets listDataSetsActivity) =>
            listDataSetsActivity.Invoke<ListDatasets, bool, List<IDataSetFull>>((lds, output) => lds.Datasets = output);

        public static TOutput Invoke<TActivity, TInput, TOutput>(this TActivity activity, Action<TActivity, OutArgument<TOutput>> setOutput)
            where TActivity : TaskActivity<TInput, TOutput>
        {
            var inToken = new InArgument<string>("inToken");
            var inBaseUrl = new InArgument<string>("inBaseUrl");

            var outVar = new Variable<TOutput>("outVar");
            var outArg = new OutArgument<TOutput>();

            IndicoScope indicoScope = new IndicoScope()
            {
                BaseUrl = new InArgument<string>(ctx => inBaseUrl.Get(ctx)),
                Token = new InArgument<string>(ctx => inToken.Get(ctx)),
            };

            indicoScope.Body.Handler = activity;
            setOutput(activity, new OutArgument<TOutput>(outVar));
            
            var root = new DynamicActivity
            {
                Properties =
                {
                    new DynamicActivityProperty
                    {
                        Name = nameof(IndicoScope.Token),
                        Value = inToken,
                        Type = inToken.GetType(),
                    },
                    new DynamicActivityProperty
                    {
                        Name = nameof(IndicoScope.BaseUrl),
                        Value = inBaseUrl,
                        Type = inBaseUrl.GetType(),
                    },
                    new DynamicActivityProperty
                    {
                        Name = nameof(ListDatasets.Datasets),
                        Type = outArg.GetType(),
                        Value = outArg,
                    }
                },
                Implementation = () => new Sequence
                {
                    Variables = { outVar },
                    Activities =
                    {
                        indicoScope,
                        new Assign<List<IDataSetFull>>()
                        {
                            Value = outVar,
                            To = new ArgumentReference<List<IDataSetFull>>(nameof(ListDatasets.Datasets)),
                        }
                    },
                },
            };



            var resultDictionary = WorkflowInvoker.Invoke(root, GetScopeParams());

            var result = (TOutput)resultDictionary.Single().Value;

            return result;
        }

        public static IDictionary<string, object> GetScopeParams() => new Dictionary<string, object>
            {{nameof(IndicoScope.BaseUrl), BaseUrl}, {nameof(IndicoScope.Token), ApiToken}};
    }
}
