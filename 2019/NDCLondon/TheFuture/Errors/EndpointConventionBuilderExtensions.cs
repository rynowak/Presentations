using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Builder
{
    public static class EndpointConventionBuilderExtensions
    {
        public static T AddMetadata<T>(this T builder, object metadata) where T : IEndpointConventionBuilder
        {
            builder.Add(b => b.Metadata.Add(metadata));
            return builder;
        }
    }
}
