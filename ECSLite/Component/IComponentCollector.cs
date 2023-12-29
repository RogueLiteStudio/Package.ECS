namespace ECSLite
{
    public interface IComponentCollector
    {
        int Count { get; }
        IComponent Add(EntityIdentify entityID);
        IComponent Get(EntityIdentify entityID);
        void Remove(EntityIdentify entityID);
        void RemoveAll();
    }
    public interface IComponentCollectorT<T> : IComponentCollector where T : class, IComponent, new()
    {
        ComponentFindResult<T> Find(int startIndex, System.Func<T, bool> condition = null);
    }

}