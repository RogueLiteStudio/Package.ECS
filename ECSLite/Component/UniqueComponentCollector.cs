using System;

namespace ECSLite
{
    internal class UniqueComponentCollector<T> : IComponentCollectorT<T> where T : class, IUniqueComponent, new()
    {
        private readonly ComponentEntity<T> Component = new ComponentEntity<T>();
        public int Count => Component.Owner.Valid ? 1 : 0;

        public IComponent Add(EntityIdentify entityID)
        {
            if (Component.Owner == entityID)
            {
                return Component.Component;
            }
            else
            {
                Remove(entityID);
            }
            Component.Owner = entityID;
            return Component.Component;
        }
        public T TryGet(out EntityIdentify id)
        {
            id = Component.Owner;
            if (id.Valid)
                return null;
            return Component.Component;
        }

        public ComponentFindResult<T> Find(int startIndex, Func<T, bool> condition = null)
        {
            var result = new ComponentFindResult<T>();
            if (startIndex == 0
                && Component.Owner.Valid && (condition == null || condition(Component.Component)))
            {
                result.Component = Component.Component;
                result.Index = 1;
                result.EntityID = Component.Owner;
            }
            return result;
        }

        public IComponent Get(EntityIdentify entityID)
        {
            if (entityID == Component.Owner)
                return Component.Component;
            return null;
        }

        public void Remove(EntityIdentify entityID)
        {
            if (entityID == Component.Owner)
            {
                Component.Reset();
            }
        }

        public void RemoveAll()
        {
            if (Component.Owner.Valid)
            {
                Component.Reset();
            }
        }
    }
}
