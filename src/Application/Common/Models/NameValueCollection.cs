using System.Collections;

namespace CleanArchitecture.Application.Common.Models;

public class NameValueCollection : IEnumerable<NameValue>
{
    private readonly IEnumerable<NameValue> collection;

    public NameValueCollection(IEnumerable<NameValue> collection)
    {
        this.collection = collection.ToList();
    }

    public IEnumerator<NameValue> GetEnumerator() => collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => collection.GetEnumerator();
}
