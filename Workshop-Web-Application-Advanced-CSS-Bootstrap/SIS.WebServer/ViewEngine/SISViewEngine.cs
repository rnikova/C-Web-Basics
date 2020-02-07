using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text.RegularExpressions;
using SIS.MvcFramework.Identity;
using System.Net;
using SIS.MvcFramework.Validation;

namespace SIS.MvcFramework.ViewEngine
{
    public class SISViewEngine : IViewEngine
    {
        public object WebUtilitycode { get; private set; }

        private string GetModelType<T>(T model)
        {
            if (model is IEnumerable)
            {
                return $"IEnumerable<{model.GetType().GetGenericArguments()[0].FullName}>";
            }

            return model.GetType().FullName;
        }

        public string GetHtml<T>(string viewContent, T model, ModelStateDictionary modelState, Principal user = null)
        {
            string csharpHtmlCode = string.Empty;
            csharpHtmlCode = this.CheckForWidgets(viewContent);
            csharpHtmlCode = this.GetCSharpCode(csharpHtmlCode);
            string code = $@"
using System;
using System.Net;
using System.Linq;
using System.Text;
using SIS.MvcFramework.Identity;
using System.Collections.Generic;
using SIS.MvcFramework.ViewEngine;
using SIS.MvcFramework.Validation;
namespace AppViewCodeNamespace
{{
    public class AppViewCode : IView
    {{
        public string GetHtml(object model, ModelStateDictionary modelState, Principal user)
        {{
            var Model = {(model == null ? "new {}" : "model as " + GetModelType(model))};
            var User = user;           
            var ModelState= modelState;
	        var html = new StringBuilder();
            {csharpHtmlCode}
            
	        return html.ToString();
        }}
    }}
}}";
            var view = this.CompileAndInstance(code, model?.GetType().Assembly);
            var htmlResult = view?.GetHtml(model, modelState, user);
            return htmlResult;
        }

        private string CheckForWidgets(string viewContent)
        {
            var widgets = Assembly
                .GetEntryAssembly()?
                .GetTypes()
                .Where(type => typeof(IViewWidget).IsAssignableFrom(type))
                .Select(x => (IViewWidget)Activator.CreateInstance(x))
                .ToList();

            if (widgets == null || widgets.Count == 0)
            {
                return viewContent;
            }

            string widgetPrefix = "@Widgets.";

            foreach (var viewWidget in widgets)
            {
                viewContent = viewContent.Replace($"{widgetPrefix}{viewWidget.GetType().Name}", viewWidget.Render());
            }

            return viewContent;
        }

        private IView CompileAndInstance(string code, Assembly modelAssembly)
        {
            modelAssembly = modelAssembly ?? Assembly.GetEntryAssembly();

            var compilation = CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(Object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(Assembly.GetEntryAssembly().Location))
                .AddReferences(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("netstandard")).Location))
                .AddReferences(MetadataReference.CreateFromFile(modelAssembly.Location));

            var netStandartAssembly = Assembly.Load(new AssemblyName("netstandart")).GetReferencedAssemblies();

            foreach (var assembly in netStandartAssembly)
            {
                compilation.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(assembly).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

            using (var memoryStream = new MemoryStream())
            {
                var compilationResult = compilation.Emit(memoryStream);

                if (!compilationResult.Success)
                {
                    var errors = compilationResult.Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error);
                    var errorsHtml = new StringBuilder();
                    errorsHtml.AppendLine($"<h1>{errors.Count()} errors:</h1>");

                    foreach (var error in errors)
                    {
                        errorsHtml.AppendLine($"<div>{error.Location} => {error.GetMessage()}</div>");
                    }

                    errorsHtml.AppendLine($"<pre>{WebUtility.HtmlEncode(code)}</pre>");
                    return new ErrorView(errorsHtml.ToString());
                }

                memoryStream.Seek(0, SeekOrigin.Begin);

                var assemblyBytes = memoryStream.ToArray();
                var assembly = Assembly.Load(assemblyBytes);
                var type = assembly.GetType("AppViewContentNamespace.AppViewCode");

                if (type == null)
                {
                    Console.WriteLine("AppViewCode not found.");

                    return null;
                }

                var instance = Activator.CreateInstance(type);

                if (instance == null)
                {
                    Console.WriteLine("AppViewCode cannot be instanciated.");

                    return null;
                }

                return instance as IView;
            }
        }

        private string GetCSharpCode(string viewContent)
        {
            var lines = viewContent.Split(new string[] { "\r\n", "\n\r", "\n" }, StringSplitOptions.None);
            var csharpCode = new StringBuilder();
            var supportedOperators = new[] { "for", "if", "else" };
            var csharpCodeRegex = new Regex(@"[^\s<""\&]+", RegexOptions.Compiled);
            var csharpCodeDepth = 0;

            foreach (var line in lines)
            {
                string currentLine = line;

                if (currentLine.TrimStart().StartsWith("@{"))
                {
                    csharpCodeDepth++;
                }
                else if (currentLine.TrimStart().StartsWith("{") || currentLine.TrimStart().StartsWith("}"))
                {
                    if (csharpCodeDepth > 0)
                    {
                        if (currentLine.TrimStart().StartsWith("{"))
                        {
                            csharpCodeDepth++;
                        }
                        else if (currentLine.TrimStart().StartsWith("}"))
                        {
                            if ((--csharpCodeDepth) == 0)
                            {
                                continue;
                            }
                        }
                    }

                    csharpCode.AppendLine(currentLine);
                }
                else if (csharpCodeDepth > 0)
                {
                    csharpCode.AppendLine(currentLine);
                    continue;
                }
                else if (supportedOperators.Any(x => currentLine.TrimStart().StartsWith("@" + x)))
                {
                    var atSignLocation = currentLine.IndexOf("@");
                    var csharpLine = currentLine.Remove(atSignLocation, 1);

                    csharpCode.AppendLine(csharpLine);
                }
                else
                {
                    if (currentLine.Contains("@RenderBody()"))
                    {
                        var csharpLine = $"html.AppendLine(@\"{currentLine}\");";
                        csharpCode.AppendLine(csharpLine);
                    }
                    else
                    {
                        var csharpStringToAppend = "html.AppendLine(@\"";
                        var restOfLine = currentLine;

                        while (restOfLine.Contains("@"))
                        {
                            var atSignLocation = restOfLine.IndexOf("@");
                            var plainText = restOfLine.Substring(0, atSignLocation).Replace("\"", "\"\"");
                            var csharpExpression = csharpCodeRegex.Match(restOfLine.Substring(atSignLocation + 1))?.Value;

                            if (csharpExpression.Contains("{") && csharpExpression.Contains("}"))
                            {
                                var csharpInlineExpression =
                                    csharpExpression.Substring(1, csharpExpression.IndexOf("}") - 1);

                                csharpStringToAppend += plainText + "\" + " + csharpInlineExpression + " + @\"";
                                csharpExpression = csharpExpression.Substring(0, csharpExpression.IndexOf("}") + 1);
                            }
                            else
                            {
                                csharpStringToAppend += plainText + "\" + " + csharpExpression + " + @\"";
                            }

                            if (restOfLine.Length <= atSignLocation + csharpExpression.Length + 1)
                            {
                                restOfLine = string.Empty;
                            }
                            else
                            {
                                restOfLine = restOfLine.Substring(atSignLocation + csharpExpression.Length + 1);
                            }
                        }

                        csharpStringToAppend += $"{restOfLine.Replace("\"", "\"\"")}\");";
                        csharpCode.AppendLine(csharpStringToAppend);
                    }
                }
            }

            return csharpCode.ToString();
        }
    }
}
