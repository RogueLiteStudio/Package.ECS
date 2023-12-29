namespace ECSLite
{
    public class SystemT<ContextType, TEntity, IContext> : ISystem
        where ContextType : ContextT<TEntity, IContext>, new()
        where TEntity : struct
    {
        protected ContextType Context { get; private set; }

        public SystemT(ContextType context)
        {
            Context = context;
        }

        public Group<TEntity, IContext, T> CreateGroup<T>(System.Func<T, bool> condition) where T : class, IContext, IComponent, new()
        {
            return Context.CreateGroup(condition);
        }
    }
}
