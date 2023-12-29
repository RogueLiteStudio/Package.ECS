using System.Collections.Generic;
namespace ECSLite
{
    public class Context
    {
        protected IStaticComponent[] staticComponents;
        protected IComponentCollector[] collectors;
        protected List<EntityInternal> entities = new List<EntityInternal>();
        private int componentCount;
        private int unUsedEntityCount = 0;

        public Context(int componentCount, int staticComponentCount)
        {
            this.componentCount = componentCount;
            collectors = new IComponentCollector[componentCount];
            staticComponents = new IStaticComponent[staticComponentCount];
        }

        public void InitComponentCollector<T>() where T : class, IComponent, new()
        {
            if (ComponentIdentity<T>.Id == -1)
            {
                throw new System.Exception($"Component类型未初始化 => {typeof(T).FullName}");
            }
            if (collectors[ComponentIdentity<T>.Id] != null)
            {
                throw new System.Exception($"ComponentId 重复或重复注册 => {typeof(T).FullName}");
            }
            collectors[ComponentIdentity<T>.Id] = new ComponentCollector<T>();
        }

        public void InitFlagComponentCollector<T>() where T : class, IComponent, new()
        {
            if (ComponentIdentity<T>.Id == -1)
            {
                throw new System.Exception($"Component类型未初始化 => {typeof(T).FullName}");
            }
            if (collectors[ComponentIdentity<T>.Id] != null)
            {
                throw new System.Exception($"ComponentId 重复或重复注册 => {typeof(T).FullName}");
            }
            collectors[ComponentIdentity<T>.Id] = new FlagComponentCollector<T>();
        }

        public void InitUniqueComponentCollector<T>() where T : class, IUniqueComponent, new()
        {
            if (ComponentIdentity<T>.Id == -1)
            {
                throw new System.Exception($"Component类型未初始化 => {typeof(T).FullName}");
            }
            if (collectors[ComponentIdentity<T>.Id] != null)
            {
                throw new System.Exception($"ComponentId 重复或重复注册 => {typeof(T).FullName}");
            }
            collectors[ComponentIdentity<T>.Id] = new UniqueComponentCollector<T>();
        }

        protected EntityIdentify CreateEntity()
        {
            if (unUsedEntityCount > 0)
            {
                for (int i=0; i<entities.Count; ++i)
                {
                    var entity = entities[i];
                    if (!entity.Used)
                    {
                        entity.Used = true;
                        unUsedEntityCount--;
                        return entity.ID;
                    }
                }
            }
            var newEntity = new EntityInternal 
            {
                ID = new EntityIdentify { Index = entities.Count, Version = 1 },
                Used = true,
                ComponentFlag = new System.Collections.BitArray(componentCount),
            };
            entities.Add(newEntity);
            return newEntity.ID;
        }

        public void DestroyEntity(EntityIdentify entityID)
        {
            var entity = entities[entityID.Index];
            if (entity.ID.Version == entityID.Version)
            {
                for (int i = 0; i < collectors.Length; ++i)
                {
                    collectors[i].Remove(entityID);
                }
                entity.Clear();
                unUsedEntityCount++;
            }
        }

        protected void AddToAll<T>() where T : class, IComponent, new()
        {
            int componentId = ComponentIdentity<T>.Id;
            var collector = collectors[componentId];
            for (int i=0; i<entities.Count; ++i)
            {
                var entity = entities[i];
                var component = collector.Add(entity.ID) as T;
                if (component != null)
                {
                    entity.AddComponent(componentId);
                }
            }
        }
    }
}
