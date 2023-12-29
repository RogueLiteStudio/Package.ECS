namespace ECSLite
{
    public struct Group<IContext, TComponent> where TComponent : class, IContext, IComponent, new()
    {
        private ContextT<IContext> context;
        private int index;
        private System.Func<TComponent, bool> condition;
        public Entity<IContext> Entity;
        public TComponent Component;

        public Group(ContextT<IContext> context, System.Func<TComponent, bool> condition)
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
