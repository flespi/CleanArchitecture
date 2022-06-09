using Specialized = System.Collections.Specialized;

namespace CleanArchitecture.Application.Common.Models;

public static class NameValueCollectionExtensions
{
    public static NameValueCollection AsLightweight(this Specialized.NameValueCollection collection)
    {
        var items = collection.AsEnumerable().ToList();
        return new NameValueCollection(items);
    }

    private static IEnumerable<NameValue> AsEnumerable(this Specialized.NameValueCollection collection)
    {
        foreach (var name in collection.AllKeys)
        {
            var value = collection[name];
            yield return new NameValue(name!, value!);
        }
    }
}
