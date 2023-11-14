namespace BlazorMD.SourceGenerator;

/// <summary>
/// Information about a Markdown component that should be generated.
/// </summary>
public readonly record struct MarkdownComponentToGenerate
{
    /// <summary>
    /// The class name of the Markdown component.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// The namespace of the Markdown component.
    /// </summary>
    public readonly string Namespace;

    /// <summary>
    /// The fully qualified name (Namespace + class name) of the Markdown component.
    /// </summary>
    public readonly string FullyQualifiedName;

    /// <summary>
    /// The name of the Markdown file used to generate the Markdown component.
    /// </summary>
    public readonly string MarkdownFileName;

    /// <summary>
    /// The file path to the source code file.
    /// </summary>
    public readonly string FilePath;

    public MarkdownComponentToGenerate(string name, string ns, string fullyQualifiedName, string filePath)
    {
        Name = name;
        Namespace = ns;
        FullyQualifiedName = fullyQualifiedName;
        MarkdownFileName = $"{name}.md";
        FilePath = Path.GetFullPath(filePath).Replace(Path.GetFileName(filePath), string.Empty);
    }
}
