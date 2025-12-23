using System.Text.RegularExpressions;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly partial struct Identifier : IType<Identifier>
{
    public string Namespace { get; }
    public string Value { get; }

    public Identifier(string fullIdentifier)
    {
        var parts = fullIdentifier.Split(':');
        
        if (parts.Length == 1)
        {
            Namespace = "minecraft";
            Value = parts[0];
        }
        else if (parts.Length == 2)
        {
            Namespace = parts[0];
            Value = parts[1];
        }
        else
        {
            throw new ArgumentException($"Invalid identifier format: {fullIdentifier}", nameof(fullIdentifier));
        }

        Validate();
    }

    public Identifier(string @namespace, string value)
    {
        Namespace = @namespace;
        Value = value;
        Validate();
    }

    public override string ToString() => $"{Namespace}:{Value}";

    public void Write(Stream stream)
    {
        new String(ToString()).Write(stream);
    }

    public static Identifier Read(Stream stream)
    {
        var content = String.Read(stream);
        return new Identifier(content.Value);
    }

    private void Validate()
    {
        if (!NamespaceRegex().IsMatch(Namespace))
        {
            throw new ArgumentException($"Invalid characters in namespace: {Namespace}", nameof(Namespace));
        }

        if (!ValueRegex().IsMatch(Value))
        {
            throw new ArgumentException($"Invalid characters in value: {Value}", nameof(Value));
        }
    }

    [GeneratedRegex("^[a-z0-9.-_]+$")]
    private static partial Regex NamespaceRegex();

    [GeneratedRegex("^[a-z0-9.-_/]+$")]
    private static partial Regex ValueRegex();

    public static implicit operator string(Identifier i) => i.ToString();
    public static implicit operator Identifier(string s) => new(s);
}