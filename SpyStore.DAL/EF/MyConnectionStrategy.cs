using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace SpyStore.DAL.EF
{
    class MyConnectionStrategy : ExecutionStrategy
    {
        public MyConnectionStrategy(ExecutionStrategyDependencies context) :
            this(context, ExecutionStrategy.DefaultMaxRetryCount, ExecutionStrategy.DefaultMaxDelay)
        { }
        public MyConnectionStrategy(ExecutionStrategyDependencies context, int retries, TimeSpan delay) :
            base(context, retries, delay)
        { }

        protected override bool ShouldRetryOn(Exception exception) => true;
       
    }
}

