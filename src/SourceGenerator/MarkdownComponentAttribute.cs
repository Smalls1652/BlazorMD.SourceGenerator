namespace BlazorMD.SourceGenerator.Attributes;

/// <summary>
/// Attribute for declaring a class to be used for source generating a Markdown component.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class MarkdownComponentAttribute : Attribute
{
}