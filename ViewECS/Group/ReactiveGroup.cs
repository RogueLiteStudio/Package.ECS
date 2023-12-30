namespace VECS
{
    public struct ReactiveGroup<TComponent> where TComponent : class, IViewComponent, new()
    {
        private int Index;
        private readonly int GroupIndex;
        private readonly ulong Version;
        private readonly bool IncludeDisable;
        private readonly ViewContext Context;
        public ViewEntity Entity;
        public TComponent Component;

        public ReactiveGroup(int groupIndex, ulong version, bool includeDisable, ViewContext context)
        {
            IncludeDisable = includeDisable;
            Version = version;
            Context = context;
            Index = 0;
            GroupIndex = groupIndex;
            Entity = default;
            Component = default;
        }

        public bool MoveNext()
        {
            var result = Context.Find<TComponent>(Index, Version, IncludeDisable, null, GroupIndex);
            Component = result.Component;
            Entity = result.Entity;
            Index = result.Index;
            return Entity != null;
        }
    }
}