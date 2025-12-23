using Amethyst.Types;

namespace Amethyst.Tags.Collections;

public class TagList : Tag, IList<Tag>
{
    private readonly List<Tag> _tags = [];
    
    public override TagType Type => TagType.List;
    
    /// <summary>
    /// The type of tags contained in this list. 
    /// If the list is empty, this can be TagType.End.
    /// </summary>
    public TagType ListType { get; private set; } = TagType.End;

    public override void WritePayload(Stream stream)
    {
        // If list is not empty, ensure all items match the ListType
        if (_tags.Count > 0)
        {
            ListType = _tags[0].Type;
            if (_tags.Any(t => t.Type != ListType))
            {
                throw new InvalidDataException("All tags in a TAG_List must be of the same type.");
            }
        }

        stream.WriteByte((byte)ListType);
        new TInt(_tags.Count).Write(stream);

        foreach (var tag in _tags)
        {
            // In a list, tags do not have names and do not write their ID
            tag.WritePayload(stream);
        }
    }

    public override void ReadPayload(Stream stream)
    {
        var typeId = stream.ReadByte();
        if (typeId == -1) throw new EndOfStreamException();
        
        ListType = (TagType)typeId;
        var count = TInt.Read(stream).Value;

        _tags.Clear();
        
        if (count <= 0) return;

        for (var i = 0; i < count; i++)
        {
            var tag = Create(ListType);
            tag.ReadPayload(stream);
            _tags.Add(tag);
        }
    }

    // IList Implementation
    public IEnumerator<Tag> GetEnumerator() => _tags.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _tags.GetEnumerator();
    public void Add(Tag item) => _tags.Add(item);
    public void Clear() => _tags.Clear();
    public bool Contains(Tag item) => _tags.Contains(item);
    public void CopyTo(Tag[] array, int arrayIndex) => _tags.CopyTo(array, arrayIndex);
    public bool Remove(Tag item) => _tags.Remove(item);
    public int Count => _tags.Count;
    public bool IsReadOnly => false;
    public int IndexOf(Tag item) => _tags.IndexOf(item);
    public void Insert(int index, Tag item) => _tags.Insert(index, item);
    public void RemoveAt(int index) => _tags.RemoveAt(index);
    public Tag this[int index] { get => _tags[index]; set => _tags[index] = value; }
}
