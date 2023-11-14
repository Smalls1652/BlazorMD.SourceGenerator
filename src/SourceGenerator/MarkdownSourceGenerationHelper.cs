using Markdig;
using System.Text;

namespace BlazorMD.SourceGenerator;

/// <summary>
/// Helper class to generate the source code for a Markdown component.
/// </summary>
public static class MarkdownSourceGenerationHelper
{
    /// <summary>
    /// Generates the source code for a Markdown component.
    /// </summary>
    /// <param name="componentToGenerate"></param>
    /// <param name="markdownContent">The content of the Markdown file.</param>
    /// <returns>Generated C# code for the component.</returns>
    public static string GenerateSource(MarkdownComponentToGenerate componentToGenerate, string markdownContent)
    {
        var markdownPipeline = new MarkdownPipelineBuilder()
                .UseGenericAttributes()
                .UseEmojiAndSmiley()
                .UseBootstrap()
                .UseEmphasisExtras()
                .UsePipeTables()
                .UseAutoLinks()
                .Build();

        string markdownHtml = Markdown.ToHtml(markdownContent, markdownPipeline);

        string[] markdownHtmlByLine = markdownHtml.Split('\n');

        StringBuilder stringBuilder = new($@"// <auto-generated />
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;

namespace {componentToGenerate.Namespace}
{{
    public partial class {componentToGenerate.Name} : global::Microsoft.AspNetCore.Components.ComponentBase
    {{
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {{");
        stringBuilder.AppendLine();
        for (int i = 0; i < markdownHtmlByLine.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(markdownHtmlByLine[i]))
            {
                continue;
            }

            stringBuilder.AppendLine($"\t\t\tbuilder.AddMarkupContent({i}, \"\"\"{markdownHtmlByLine[i]}\"\"\");");
        }

        stringBuilder.AppendLine("\t\t}");
        stringBuilder.AppendLine("\t}");
        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }
}