using System;
namespace VECS
{
    public struct EntityFindResult<TComponent> where TComponent : IViewComponent
    {
        public ViewEntity Entity;
        public int Index;
        public ulong Version;
        public TComponent Component;
    }

    public struct Group<TComponent> where TComponent : class, IViewComponent, new()
    {
        private readonly ViewContext Context;
        private readonly Func<TComponent, bool> Condition;
        private readonly bool InCludeDisable;
        private int Index;
        public ViewEntity Entity;
        public TComponent Component;
        public Group(ViewContext context, bool inCludeDisable, Func<TComponent, bool> condition)
        {
            Index = 0;
            InCludeDisable = inCludeDisable;
            Context = context;
            Condition = condition;
            Entity = default;
            Component = default;
        }

        public bool MoveNext()
        {
            var result = Context.Find(Index, 0, InCludeDisable, Condition);
            Component = result.Component;
            Entity = result.Entity;
            Index = result.Index;
            return Entity != null;
        }
    }

}