using System;

namespace ECSLite
{
    public class ContextT<IContext> : Context
    {
        protected IStaticComponent[] staticComponents;
        public ContextT(int componentCount, int uniqueCount, int staticComponentCount) : base(componentCount, uniqueCount)
        {
            staticComponents = new IStaticComponent[staticComponentCount];
        }

        public Entity<IContext> Create()
        {
            var e = CreateEntity();
            return new Entity<IContext> { entity = e, ID = e.ID  };
        }

        internal Entity<IContext> IndexToEntity(int index)
        {
            var e = Get(index);
            if (e == null)
                return default;
            return new Entity<IContext> { entity = e, ID = e.ID };
        }

        public T AddStaticComponent<T>() where T : class, IContext, IStaticComponent, new()
        {
            int id = StaticComponentIdentity<T>.Id;
            T component = staticComponents[id] as T;
            if (component == null)
            {
                component = new T();
                staticComponents[id] = component;
            }
            return component;
        }

        public T GetStaticComponent<T>() where T : class, IContext, IStaticComponent, new()
        {
            int id = StaticComponentIdentity<T>.Id;
            return staticComponents[id] as T;
        }

        public T GetUniqueComponent<T>(out Entity<IContext> entity) where T : class, IContext, IUniqueComponent, new()
        {
            var component = GetUniqueComponent<T>(out EntityInternal e);
            if (component == null)
            {
                entity = default;
                return null;
            }
            entity = new Entity<IContext> { entity = e, ID = e.ID };
            return component;
        }

        public void RemoveComponentAll<T>() where T : class, IContext, IComponent, new()
        {
            RemoveAll<T>();
        }

        public void AddToAllEntity<T>() where T : class, IContext, IComponent, new()
        {
            AddToAll<T>();
        }

        public EntityFindResult<IContext, T> Find<T>(int startIndex, Func<T, bool> condition = null) where T: class, IContext, IComponent, new ()
        {
            var result =FindComponet(startIndex, condition);
            return new EntityFindResult<IContext, T>
            {
                Component = result.Component,
                Entity = IndexToEntity(result.EntityIndex),
                Index = result.Index,
            };
        }

        public Group<IContext, T> CreateGroup<T>(Func<T, bool> condition = null) where T : class, IContext, IComponent, new()
        {
            return new Group<IContext, T>(this, condition);
        }
    }
}
