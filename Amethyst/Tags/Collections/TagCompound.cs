using System.Text;
using Amethyst.Tags.Primitives;
using Amethyst.Types;

namespace Amethyst.Tags.Collections;

public class TagCompound : Tag, IDictionary<string, Tag>
{
    private readonly Dictionary<string, Tag> _tags = [];

    public override TagType Type => TagType.Compound;

    public override void WritePayload(Stream stream)
    {
        foreach (var tag in _tags.Values)
        {
            // Inside a compound, tags write their ID, Name, and Payload
            tag.Write(stream, writeId: true, writeName: true);
        }
        
        // Close the compound with TAG_End
        stream.WriteByte((byte)TagType.End);
    }

    public override void ReadPayload(Stream stream)
    {
        _tags.Clear();

        while (true)
        {
            var typeIdByte = stream.ReadByte();
            if (typeIdByte == -1) throw new EndOfStreamException();

            var type = (TagType)typeIdByte;
            if (type == TagType.End)
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
    public int GetInt(string name) => _tags.TryGetValue(name, out var tag) && tag is TagInt i ? i.Value : 0;
    public string GetString(string name) => _tags.TryGetValue(name, out var tag) && tag is TagString s ? s.Value : string.Empty;
    public void Set(string name, Tag tag) { tag.Name = name; _tags[name] = tag; }

    // IDictionary Implementation
    public void Add(string key, Tag value) { value.Name = key; _tags.Add(key, value); }
    public bool ContainsKey(string key) => _tags.ContainsKey(key);
    public bool Remove(string key) => _tags.Remove(key);
    public bool TryGetValue(string key, out Tag value) => _tags.TryGetValue(key, out value!);
    public Tag this[string key] { get => _tags[key]; set { value.Name = key; _tags[key] = value; } }
    public ICollection<string> Keys => _tags.Keys;
    public ICollection<Tag> Values => _tags.Values;
    public void Add(KeyValuePair<string, Tag> item) { item.Value.Name = item.Key; _tags.Add(item.Key, item.Value); }
    public void Clear() => _tags.Clear();
    public bool Contains(KeyValuePair<string, Tag> item) => _tags.Contains(item);
    public void CopyTo(KeyValuePair<string, Tag>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, Tag>>)_tags).CopyTo(array, arrayIndex);
    public bool Remove(KeyValuePair<string, Tag> item) => _tags.Remove(item.Key);
    public int Count => _tags.Count;
    public bool IsReadOnly => false;
    public IEnumerator<KeyValuePair<string, Tag>> GetEnumerator() => _tags.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _tags.GetEnumerator();
}
