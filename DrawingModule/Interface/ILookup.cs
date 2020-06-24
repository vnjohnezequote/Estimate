namespace DrawingModule.Interface
{
    public interface ILookup<T>
    {
        T this[string key]
        {
            get;
        }
    }
}