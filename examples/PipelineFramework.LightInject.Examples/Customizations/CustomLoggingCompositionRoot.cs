using PipelineFramework.LightInject.Logging;
using System;

namespace PipelineFramework.LightInject.Examples.Customizations
{
    public class CustomLoggingCompositionRoot : DefaultLoggingCompositionRoot
    {
        protected override Type GetAsyncPipelineComponentInterceptorType() => typeof(CustomAsyncPipelineComponentInterceptor);
    }
}
