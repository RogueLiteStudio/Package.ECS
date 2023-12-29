﻿namespace ECSLite
{
    public abstract class GroupUpdateSystemT<ContextType, TEntity, IContext, TComponent> :ISystem 
        where ContextType : ContextT<TEntity, IContext>, new()
        where TEntity : struct
        where TComponent : class, IContext, IComponent, new()
    {
        public ContextType Context { get; private set; }
        public GroupUpdateSystemT(ContextType context)
        {
            Context = context;
        }

        public void OnUpdate()
        {
            OnStartUpdate();
            var group = Context.CreateGroup<TComponent>(null);
            while (group.MoveNext())
            {
                var entity = group.Entity;
                var component = group.Component;
                OnExecuteEntity(entity, component);
            }
            OnFinishUpdate();
        }
        protected virtual void OnStartUpdate() { }
        protected virtual void OnFinishUpdate() { }
        protected abstract void OnExecuteEntity(TEntity entity, TComponent component);
    }
}