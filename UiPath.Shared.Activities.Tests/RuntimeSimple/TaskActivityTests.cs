﻿using System;
using System.Activities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UiPath.Shared.Activities.RuntimeSimple;
using FluentAssertions;

namespace UiPath.Shared.Activities.Tests.RuntimeSimple
{
    public class TaskActivityTests1 : TaskActivityTests<TaskActivityTests1.ExceptionThrowingActivity<InvalidOperationException>, InvalidOperationException>
    {
        public class ExceptionThrowingActivity<TException> : ExceptionThrowingActivityBase
            where TException : Exception, new()
        {
            // Exception thrown by a task
            protected override async Task<int> ExecuteAsync(int p, CancellationToken cancellationToken) =>
                throw new TException();
        }
    }

    public class TaskActivityTests2 : TaskActivityTests<TaskActivityTests2.ExceptionThrowingActivity<ArgumentException>, ArgumentException>
    {
        public class ExceptionThrowingActivity<TException> : ExceptionThrowingActivityBase
            where TException : Exception, new()
        {
            // Exception thrown when starting a task
            protected override Task<int> ExecuteAsync(int p, CancellationToken cancellationToken) =>
                throw new TException();
        }
    }

    public abstract class ExceptionThrowingActivityBase : TaskActivity<int, int>
    {
        protected override int GetInputs(AsyncCodeActivityContext ctx) => 1;

        protected override void SetOutputs(AsyncCodeActivityContext ctx, int output)
        {
        }
    }

    public abstract class TaskActivityTests<TActivity, TExpectedException>
        where TActivity : Activity, new()
        where TExpectedException : Exception
    {
        private WorkflowInvoker _invoker;

        [SetUp]
        public void SetUp() => _invoker = new WorkflowInvoker(new TActivity());

        [Test]
        public void Invoke_ShouldThrow_WhenContinueOnErrorIsFalse() => _invoker.Invoking(i =>
                           i.Invoke(new Dictionary<string, object> { { nameof(TaskActivity<int, int>.ContinueOnError), false } }))
           .Should()
           .Throw<TExpectedException>();

        [Test]
        public void Invoke_ShouldReturnNoError_WhenContinueOnErrorIsTrue() =>
            _invoker.Invoking(i =>
                i.Invoke(new Dictionary<string, object> { { nameof(TaskActivity<int, int>.ContinueOnError), true } }))
            .Should()
            .NotThrow();
    }
}
