using Microsoft.AspNetCore.Components;

namespace BlazorBolt
{
    public interface ITab
    {
        RenderFragment ChildContent { get; }
    }
}