using PipelineFramework.LightInject.Logging;
using System;

namespace PipelineFramework.LightInject.Examples.Customizations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CustomLoggingCompositionRoot : DefaultLoggingCompositionRoot
    {
        protected override Type GetAsyncPipelineComponentInterceptorType() => typeof(CustomAsyncPipelineComponentInterceptor);
    }
}
