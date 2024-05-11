using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace BlazorMD.SourceGenerator;

/// <summary>
/// Source generator for Markdown components.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class MarkdownComponentSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext initContext)
    {
        IncrementalValuesProvider<MarkdownComponentToGenerate?> valuesProvider = initContext.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: "BlazorMD.SourceGenerator.Attributes.MarkdownComponentAttribute",
                predicate: static (node, cancellationToken) => node is ClassDeclarationSyntax,
                transform: GetTypeToGenerate
            );

        var markdownFilesProvider = initContext.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(".razor.md"))
            .Select((text, cancellationToken) => new MarkdownFile(Path.GetFileName(text.Path), text.GetText(cancellationToken)!.ToString()))
            .Collect();

        var valuesAndMarkdownFiles = valuesProvider.Combine(markdownFilesProvider);

        initContext.RegisterSourceOutput(
            source: valuesAndMarkdownFiles,
            action: static (spc, source) => Execute(spc, in source.Left, source.Right)
        );
    }

    private static void Execute(SourceProductionContext context, in MarkdownComponentToGenerate? markdownComponentToGenerate, ImmutableArray<MarkdownFile> markdownFiles)
    {
        if (markdownComponentToGenerate is { } markdownComponent)
        {
            string filename = $"{markdownComponent.Name}.g.razor.cs";

            string markdownFilePath = Path.Combine(markdownComponent.FilePath, markdownComponent.MarkdownFileName);

            var markdownFile = markdownFiles.Where(file => file.Name == markdownComponent.MarkdownFileName).FirstOrDefault();

            var source = MarkdownSourceGenerationHelper.GenerateSource(markdownComponent, markdownFile.Content);
            context.AddSource(filename, SourceText.From(source, Encoding.UTF8));
        }
    }

    private static MarkdownComponentToGenerate? GetTypeToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var loc = typeSymbol.Locations.Where(location => location.IsInSource).FirstOrDefault();

        var filePath = loc.SourceTree is not null ? loc.SourceTree.FilePath : string.Empty;

        return new(
            name: typeSymbol.Name,
            ns: typeSymbol.ContainingNamespace.IsGlobalNamespace ? string.Empty : typeSymbol.ContainingNamespace.ToDisplayString(),
            fullyQualifiedName: typeSymbol.ToDisplayString(),
            filePath: filePath
        );
    }
}
