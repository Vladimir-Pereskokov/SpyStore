using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace SpyStore.DAL.EF
{
    class MyConnectionStrategy : ExecutionStrategy
    {
        private bool prepareTests = false;

        public MyConnectionStrategy(ExecutionStrategyDependencies context, bool prepareTests) :
            this(context, prepareTests ? 0 : ExecutionStrategy.DefaultMaxRetryCount,
                prepareTests ? new TimeSpan(0,0,15) : ExecutionStrategy.DefaultMaxDelay)
        { this.prepareTests = prepareTests; }
        public MyConnectionStrategy(ExecutionStrategyDependencies context, int retries, TimeSpan delay) :
            base(context, retries, delay)
        { }

        protected override bool ShouldRetryOn(Exception exception) => !prepareTests;
        protected override int MaxRetryCount => prepareTests? 0 : base.MaxRetryCount;
    }
}

