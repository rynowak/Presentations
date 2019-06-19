using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RazorComponents.MaterialDesign
{
    public abstract class MdcComponentBase : ComponentBase
    {
        bool isFirstRender = true;

        protected override Task OnAfterRenderAsync()
        {
            if (isFirstRender)
            {
                isFirstRender = false;
                return OnAfterFirstRenderAsync();
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        protected virtual Task OnAfterFirstRenderAsync()
            => Task.CompletedTask;
    }
}
