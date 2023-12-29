namespace ECSLite
{
    public struct Group<TEntity, IContext, TComponent> where TComponent : class, IContext, IComponent, new() where TEntity : struct
    {
        private ContextT<TEntity, IContext> context;
        private int index;
        private System.Func<TComponent, bool> condition;
        public TEntity Entity;
        public TComponent Component;

        public Group(ContextT<TEntity, IContext> context, System.Func<TComponent, bool> condition)
        {
            index = 0;
            this.context = context;
            this.condition = condition;
            Entity = default;
            Component = default;
        }
        public bool MoveNext()
        {
            var result = context.Find(index, condition);
            Component = result.Component;
            Entity = result.Entity;
            index = result.Index;
            return Component != null;
        }
    }
}
