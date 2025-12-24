# Amethyst Protocol

A minecraft network protocol and NBT utility library.

## Examples

### Network Protocol

> Under development very slowly...

### NBT Parsing

```text
- (Unnamed) [Compound]: { ... 1 Entries }
  - Data [Compound]: { ... 30 Entries }
    - thunderTime [Int]: 33998
    - Difficulty [Byte]: 2
    - allowCommands [Byte]: 1
    - initialized [Byte]: 1
    - hardcore [Byte]: 0
    - version [Int]: 19133
    - ServerBrands [List]: [List<String>, 1 Entries]
    - GameType [Int]: 1
    - LevelName [String]: "Test World"
    - Time [Long]: 4217L
// ......
```

```csharp
public static class Program
{
    public static void Main(string[] args)
    {
        var path = "level.dat";
        using var fs = File.OpenRead(path);
        
        var firstByte = fs.ReadByte();
        fs.Position = 0;
        using Stream nbtStream = firstByte == 0x1F ? new GZipStream(fs, CompressionMode.Decompress) : fs;
        
        var rootTag = TagIO.Read(nbtStream);
        PrintTag(rootTag, depth: 2); 
    }

    private static void PrintTag(Tag tag, int currentLevel = 0, int depth = 1)
    {
        var indent = new string(' ', currentLevel * 2);
        
        string? valueDisplay = tag switch
        {
            TagCompound c   => $"{{ ... {c.Count} Entries }}",
            TagList l       => $"[List<{l.ListType}>, {l.Count} Entries]",
            TagByteArray ba => $"[ByteArray, {ba.Value.Length} Bytes]",
            TagIntArray ia  => $"[IntArray, {ia.Value.Length} Ints]",
            TagLongArray la => $"[LongArray, {la.Value.Length} Longs]",
            TagString s     => $"\"{s.Value}\"",
            TagShort s      => $"{s.Value}s",
            TagInt i        => $"{i.Value}",
            TagLong l       => $"{l.Value}L",
            TagFloat f      => $"{f.Value:0.##}f",
            TagDouble d     => $"{d.Value:0.##}d",
            TagByte b       => $"{b.Value}",
            _               => string.Empty
        };

        var name = string.IsNullOrEmpty(tag.Name) ? "(Unnamed)" : tag.Name;
        Console.WriteLine($"{indent}- {name} [{tag.Type}]: {valueDisplay}");

        if (currentLevel < depth && tag is TagCompound compound)
        {
            foreach (var child in compound.Values)
            {
                PrintTag(child, currentLevel + 1, depth);
            }
        }
    }
}
```
