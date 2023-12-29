namespace ECSLite
{
    internal interface IComponentCollector
    {
        int Count { get; }
        IComponent Add(int entityID);
        IComponent Get(int entityID);
        void Remove(int entityID);
        void RemoveAll();
    }
    internal interface IComponentCollectorT<T> : IComponentCollector where T : class, IComponent, new()
    {
        ComponentFindResult<T> Find(int startIndex, System.Func<T, bool> condition = null);
    }

}