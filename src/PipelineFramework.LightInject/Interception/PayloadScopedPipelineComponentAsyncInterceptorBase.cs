using LightInject.Interception;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PipelineFramework.LightInject.Interception
{
    public abstract class PayloadScopedPipelineComponentAsyncInterceptorBase<TPayload> : AsyncInterceptor where TPayload : class
    {
        protected PayloadScopedPipelineComponentAsyncInterceptorBase() : base(null)
        {
        }

        protected override Task<T> InvokeAsync<T>(IInvocationInfo invocationInfo)
        {
            if (!(invocationInfo.Arguments.FirstOrDefault() is TPayload payload))
            {
                throw new InvalidOperationException(
                    $"Expected first parameter of intercepted method {invocationInfo.Method.Name} to have parameter of type {nameof(TPayload)}");
            }
            BeforeExecute(payload);
            var task = base.InvokeAsync<T>(invocationInfo);
            AfterExecute(payload);
            return task;
        }

        protected  abstract void BeforeExecute(TPayload payload);

        protected abstract void AfterExecute(TPayload payload);
    }
}