using System;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace BlazorBolt
{
    internal static class Formatter
    {
        public static string FormatSyntaxTree(RazorCodeDocument generated)
        {
            var tree = generated.GetSyntaxTree();
            var property = tree.GetType().GetProperty("Root", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property == null)
            {
                throw new InvalidOperationException("Could not find Root property.");
            }

            var root = property.GetValue(tree);

            var type = typeof(RazorCodeDocument).Assembly.GetType("Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxSerializer", throwOnError: true);
            var method = type.GetMethod("Serialize", BindingFlags.NonPublic | BindingFlags.Static);
            if (method == null)
            {
                throw new InvalidOperationException("Could not find method Serialize.");
            }

            return (string)method.Invoke(null, new[]{ root, });
        }

        public static string FormatDocumentNode(RazorCodeDocument generated)
        {
            var document = generated.GetDocumentIntermediateNode();

            var type = typeof(RazorCodeDocument).Assembly.GetType("Microsoft.AspNetCore.Razor.Language.Intermediate.DebuggerDisplayFormatter", throwOnError: true);
            var formatter = Activator.CreateInstance(type);
            var method = formatter.GetType().GetMethod("FormatTree", BindingFlags.Public | BindingFlags.Instance);
            if (method == null)
            {
                throw new InvalidOperationException("Could not find method FormatTree.");
            }

            method.Invoke(formatter, new object[]{ document, });

            return formatter.ToString();
        }
    }
}