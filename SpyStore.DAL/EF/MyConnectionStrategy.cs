using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace SpyStore.DAL.EF
{
    public class MyExecutionStrategy : ExecutionStrategy
    {
        private bool prepareTests = false;

        public MyExecutionStrategy(ExecutionStrategyDependencies context, bool prepareTests) :
            this(context, prepareTests ? 0 : ExecutionStrategy.DefaultMaxRetryCount,
                prepareTests ? new TimeSpan(0,0,15) : ExecutionStrategy.DefaultMaxDelay)
        { this.prepareTests = prepareTests; }
        public MyExecutionStrategy(ExecutionStrategyDependencies context, int retries, TimeSpan delay) :
            base(context, retries, delay)
        { }

        protected override bool ShouldRetryOn(Exception exception) => !prepareTests;
        protected override int MaxRetryCount => prepareTests? 0 : base.MaxRetryCount;
    }
}

