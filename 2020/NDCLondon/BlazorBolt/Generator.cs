using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor;

namespace BlazorBolt
{
    internal class Generator
    {
        public Generator()
        {
            Declarations = new Dictionary<string, string>();
            References = new List<MetadataReference>();

            GC.KeepAlive(typeof(EditForm));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic && assembly.Location != null)
                {
                    References.Add(MetadataReference.CreateFromFile(assembly.Location));
                }
            }

            BaseCompilation = CSharpCompilation.Create(
                assemblyName: "__Test",
                Array.Empty<SyntaxTree>(),
                References.ToArray(),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            References.Add(BaseCompilation.ToMetadataReference());

            FileSystem = new TestRazorProjectFileSystem();
            Engine = RazorProjectEngine.Create(RazorConfiguration.Default, FileSystem, builder =>
            {
                builder.Features.Add(new CompilationTagHelperFeature());
                builder.Features.Add(new DefaultMetadataReferenceFeature() { References = References, });
                CompilerFeatures.Register(builder);
            });
        }

        private RazorProjectEngine Engine { get; }
        private TestRazorProjectFileSystem FileSystem { get; }
        private Dictionary<string, string> Declarations { get; }
        private CSharpCompilation BaseCompilation { get; }
        private List<MetadataReference> References { get; }

        public void Add(string filePath, string content)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var item = new TestRazorProjectItem(filePath, fileKind: FileKinds.Component)
            {
                Content = content ?? string.Empty,
            };

            FileSystem.Add(item);
        }

        public RazorCodeDocument Update(string filePath, string content)
        {
            var obj = FileSystem.GetItem(filePath, fileKind: FileKinds.Component);
            if (obj.Exists && obj is TestRazorProjectItem item)
            {
                Declarations.TryGetValue(filePath, out var existing);

                var declaration = Engine.ProcessDeclarationOnly(item);
                var declarationText = declaration.GetCSharpDocument().GeneratedCode;

                // Updating a declaration, create a new compilation
                if (!string.Equals(existing, declarationText, StringComparison.Ordinal))
                {
                    Declarations[filePath] = declarationText;

                    // Yeet the old one.
                    References.RemoveAt(References.Count - 1);

                    var compilation = BaseCompilation.AddSyntaxTrees(Declarations.Select(kvp =>
                        {
                            return CSharpSyntaxTree.ParseText(kvp.Value, path: kvp.Key);
                        }));
                    References.Add(compilation.ToMetadataReference());
                }

                item.Content = content ?? string.Empty;
                var generated = Engine.Process(item);
                return generated;
            }

            throw new InvalidOperationException($"Cannot find item '{filePath}'.");
        }
    }
}