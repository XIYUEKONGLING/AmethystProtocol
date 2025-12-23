using System.Text;
using Amethyst.Types;

namespace Amethyst.Tags;

public class NbtCompound : NbtTag, IDictionary<string, NbtTag>
{
    private readonly Dictionary<string, NbtTag> _tags = [];

    public override NbtTagType Type => NbtTagType.Compound;

    public override void WritePayload(Stream stream)
    {
        foreach (var tag in _tags.Values)
        {
            // Inside a compound, tags write their ID, Name, and Payload
            tag.Write(stream, writeId: true, writeName: true);
        }
        
        // Close the compound with TAG_End
        stream.WriteByte((byte)NbtTagType.End);
    }

    public override void ReadPayload(Stream stream)
    {
        _tags.Clear();

        while (true)
        {
            var typeIdByte = stream.ReadByte();
            if (typeIdByte == -1) throw new EndOfStreamException();

            var type = (NbtTagType)typeIdByte;
            if (type == NbtTagType.End)
            {
                break;
            }

            // Read Name
            var nameLen = TUnsignedShort.Read(stream).Value;
            Span<byte> nameBuffer = nameLen <= 1024 ? stackalloc byte[nameLen] : new byte[nameLen];
            stream.ReadExactly(nameBuffer);
            var name = Encoding.UTF8.GetString(nameBuffer);

            // Read Payload
            var tag = Create(type);
            tag.Name = name;
            tag.ReadPayload(stream);

            _tags[name] = tag;
        }
    }

    // Helper to get primitives easily
    public int GetInt(string name) => _tags.TryGetValue(name, out var tag) && tag is NbtInt i ? i.Value : 0;
    public string GetString(string name) => _tags.TryGetValue(name, out var tag) && tag is NbtString s ? s.Value : string.Empty;
    public void Set(string name, NbtTag tag) { tag.Name = name; _tags[name] = tag; }

    // IDictionary Implementation
    public void Add(string key, NbtTag value) { value.Name = key; _tags.Add(key, value); }
    public bool ContainsKey(string key) => _tags.ContainsKey(key);
    public bool Remove(string key) => _tags.Remove(key);
    public bool TryGetValue(string key, out NbtTag value) => _tags.TryGetValue(key, out value!);
    public NbtTag this[string key] { get => _tags[key]; set { value.Name = key; _tags[key] = value; } }
    public ICollection<string> Keys => _tags.Keys;
    public ICollection<NbtTag> Values => _tags.Values;
    public void Add(KeyValuePair<string, NbtTag> item) { item.Value.Name = item.Key; _tags.Add(item.Key, item.Value); }
    public void Clear() => _tags.Clear();
    public bool Contains(KeyValuePair<string, NbtTag> item) => _tags.Contains(item);
    public void CopyTo(KeyValuePair<string, NbtTag>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, NbtTag>>)_tags).CopyTo(array, arrayIndex);
    public bool Remove(KeyValuePair<string, NbtTag> item) => _tags.Remove(item.Key);
    public int Count => _tags.Count;
    public bool IsReadOnly => false;
    public IEnumerator<KeyValuePair<string, NbtTag>> GetEnumerator() => _tags.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _tags.GetEnumerator();
}
