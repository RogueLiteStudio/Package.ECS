using System;

namespace ECSLite
{
    public class ContextT<TEntity, IContext> : Context where TEntity : struct
    {
        private readonly Func<EntityIdentify, TEntity> CreateEntityFunc;
        public ContextT(int componentCount, int staticComponentCount, Func<EntityIdentify, TEntity> createEntityFunc) : base(componentCount, staticComponentCount)
        {
            CreateEntityFunc = createEntityFunc;
        }

        public TEntity Create()
        {
            var id = CreateEntity();
            return CreateEntityFunc(id);
        }

        public T AddComponent<T>(EntityIdentify id) where T : class, IContext, IComponent, new()
        {
            var entity = entities[id.Index];
            if (entity.ID.Version != id.Version)
            {
                return null;
            }
            int componentId = ComponentIdentity<T>.Id;
            var collector = collectors[componentId] as IComponentCollectorT<T>;
            var component = collector.Add(id) as T;
            if (!ComponentIdentity<T>.Unique)
                entity.AddComponent(componentId);
            return component;
        }
        public T AddStaticComponent<T>() where T : class, IContext, IStaticComponent, new()
        {
            int id = StaticComponentIdentity<T>.Id;
            var component = staticComponents[id] as T;
            if (component == null)
            {
                component = new T();
                staticComponents[id] = component;
            }
            return component;
        }

        public bool HasComponent<T>(EntityIdentify id) where T : class, IContext, IComponent, new()
        {
            var entity = entities[id.Index];
            if (entity.ID.Version != id.Version)
            {
                return false;
            }
            if (!ComponentIdentity<T>.Unique)
                return entity.HasComponent(ComponentIdentity<T>.Id);
            return collectors[ComponentIdentity<T>.Id].Get(id) != null;
        }

        public T GetComponent<T>(EntityIdentify id) where T : class, IContext, IComponent, new()
        {
            var entity = entities[id.Index];
            if (entity.ID.Version != id.Version)
            {
                return null;
            }
            int componentId = ComponentIdentity<T>.Id;
            if (!ComponentIdentity<T>.Unique && !entity.HasComponent(componentId))
                return null;
            return collectors[componentId].Get(id) as T;
        }
        public T GetUniqueComponent<T>(out TEntity entity) where T : class, IContext, IUniqueComponent, new()
        {
            if (!ComponentIdentity<T>.Unique)
            {
                throw new Exception($"{typeof(T).Name} not a UniqueComponent");
            }
            int id = ComponentIdentity<T>.Id;
            var collector = collectors[id] as UniqueComponentCollector<T>;
            
            var component = collector.TryGet(out EntityIdentify entityId);
            entity = CreateEntityFunc(entityId);
            return component;
        }

        public T GetStaticComponent<T>() where T : class, IContext, IStaticComponent, new()
        {
            int id = StaticComponentIdentity<T>.Id;
            return staticComponents[id] as T;
        }

        public void RemoveComponent<T>(EntityIdentify id) where T : class, IContext, IComponent, new()
        {
            var entity = entities[id.Index];
            if (entity.ID.Version != id.Version)
            {
                throw new Exception($"Entity已经被删除 => {id}");
            }
            int componentId = ComponentIdentity<T>.Id;
            if (!entity.HasComponent(componentId))
                return;
            collectors[ComponentIdentity<T>.Id].Remove(id);
            if (!ComponentIdentity<T>.Unique)
                entity.RemoveComponent(componentId);
        }

        public void RemoveAll<T>() where T : class, IContext, IComponent, new()
        {
            collectors[ComponentIdentity<T>.Id].RemoveAll();
        }

        public void AddToAllEntity<T>() where T : class, IContext, IComponent, new()
        {
            AddToAll<T>();
        }

        public EntityFindResult<TEntity, T> Find<T>(int startIndex, Func<T, bool> condition = null) where T: class, IContext, IComponent, new ()
        {
            int id = ComponentIdentity<T>.Id;
            var collector = collectors[id] as IComponentCollectorT<T>;
            var result = collector.Find(startIndex, condition);
            return new EntityFindResult<TEntity, T>
            {
                Component = result.Component,
                Entity = CreateEntityFunc(result.EntityID),
                Index = result.Index,
            };
        }

        public Group<TEntity, IContext, T> CreateGroup<T>(Func<T, bool> condition = null) where T : class, IContext, IComponent, new()
        {
            return new Group<TEntity, IContext, T>(this, condition);
        }
    }
}
