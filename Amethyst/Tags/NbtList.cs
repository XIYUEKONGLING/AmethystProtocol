using Amethyst.Types;

namespace Amethyst.Tags;

public class NbtList : NbtTag, IList<NbtTag>
{
    private readonly List<NbtTag> _tags = [];
    
    public override NbtTagType Type => NbtTagType.List;
    
    /// <summary>
    /// The type of tags contained in this list. 
    /// If the list is empty, this can be NbtTagType.End.
    /// </summary>
    public NbtTagType ListType { get; private set; } = NbtTagType.End;

    public override void WritePayload(Stream stream)
    {
        // If list is not empty, ensure all items match the ListType
        if (_tags.Count > 0)
        {
            ListType = _tags[0].Type;
            if (_tags.Any(t => t.Type != ListType))
            {
                throw new InvalidDataException("All tags in an NBT List must be of the same type.");
            }
        }
        else
        {
            // Empty list usually writes TAG_End (0) as type, but can be anything.
            // We keep existing ListType if set, or default to End.
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
        
        ListType = (NbtTagType)typeId;
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
    public IEnumerator<NbtTag> GetEnumerator() => _tags.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _tags.GetEnumerator();
    public void Add(NbtTag item) => _tags.Add(item);
    public void Clear() => _tags.Clear();
    public bool Contains(NbtTag item) => _tags.Contains(item);
    public void CopyTo(NbtTag[] array, int arrayIndex) => _tags.CopyTo(array, arrayIndex);
    public bool Remove(NbtTag item) => _tags.Remove(item);
    public int Count => _tags.Count;
    public bool IsReadOnly => false;
    public int IndexOf(NbtTag item) => _tags.IndexOf(item);
    public void Insert(int index, NbtTag item) => _tags.Insert(index, item);
    public void RemoveAt(int index) => _tags.RemoveAt(index);
    public NbtTag this[int index] { get => _tags[index]; set => _tags[index] = value; }
}
