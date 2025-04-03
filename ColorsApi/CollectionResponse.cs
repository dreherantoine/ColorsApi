namespace ColorsApi;

public class CollectionResponse<T>
{
    public IReadOnlyCollection<T> Items { get; set; }
}
