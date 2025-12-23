using Amethyst.Tags;

namespace AmethystTests.Helpers;

public static class TagTestHelper
{
    public static T AssertRoundTrip<T>(T sourceTag, Func<T> factory) where T : Tag
    {
        using var stream = new MemoryStream();
        
        // 1. Write
        sourceTag.WritePayload(stream);
        
        // 2. Reset
        stream.Position = 0;
        
        // 3. Create instance using the factory
        var newTag = factory();
        
        // 4. Read
        newTag.ReadPayload(stream);
        
        return newTag;
    }
}