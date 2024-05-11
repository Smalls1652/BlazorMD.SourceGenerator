namespace BlazorMD.SourceGenerator;

/// <summary>
/// Information about a Markdown file.
/// </summary>
public readonly record struct MarkdownFile
{
    /// <summary>
    /// The name of the Markdown file.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// The content of the Markdown file.
    /// </summary>
    public readonly string Content;

    public MarkdownFile(string name, string content)
    {
        Name = name;
        Content = content;
    }
}
