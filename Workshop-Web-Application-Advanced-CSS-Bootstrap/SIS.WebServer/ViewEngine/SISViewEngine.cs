using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Reflection;

namespace SIS.MvcFramework.ViewEngine
{
    public class SISViewEngine : IViewEngine
    {
        public string GetHtml<T>(string viewContent, T model)
        {
            string csharpHtmlCode = GetCSharpCode(viewContent);
            string code = $@"
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework.ViewEngine;
namespace AppViewContentNamespace
{{
    public class AppViewCode() : IView
    {{
        public string GetHtml(Model)
        {{
            var html = new StringBuilder();
            
            {csharpHtmlCode};
            
            return html.ToString();
        }}
    }}
}}";
            var view = CompileAndInstance(code);
            var htmlResult = view?.GetHtml();

            return htmlResult;
        }

        private IView CompileAndInstance(string code)
        {
            var compilation = CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location));

            var netStandartAssembly = Assembly.Load(new AssemblyName("netstandart")).GetReferencedAssemblies();

            foreach (var assembly in netStandartAssembly)
            {
                compilation.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(assembly).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

            using (var memoryStream = new MemoryStream())
            {
                compilation.Emit(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var assemblyBytes = memoryStream.ToArray();
            }

            return null;
        }

        private string GetCSharpCode(string viewContent)
        {
            return string.Empty;
        }
    }
}
