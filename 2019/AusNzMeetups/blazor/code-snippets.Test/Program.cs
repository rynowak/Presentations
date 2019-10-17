using System;
using code_snippets.Pages;
using code_snippets.Shared;
using Microsoft.AspNetCore.Components.Testing;

namespace code_snippets.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TestHost();
            var component = host.AddComponent<Counter>();

            component.Find("button").Click();
        }
    }
}
