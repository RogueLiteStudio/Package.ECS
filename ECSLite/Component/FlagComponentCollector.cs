using System;
using System.Collections.Generic;

namespace ECSLite
{
    internal class FlagComponentCollector<T> : IComponentCollectorT<T> where T : class, IComponent, new()
    {
        private readonly List<FlagComponentEntity<T>> mUnits = new List<FlagComponentEntity<T>>();
        private readonly Queue<int> mUnUsedIdxs = new Queue<int>();
        private readonly Dictionary<int, int> mIdIdxMap = new Dictionary<int, int>();//EntityId => 数组索引
        private T Component = new T();
        public int Count => mIdIdxMap.Count;

        private FlagComponentEntity<T> Create()
        {
            if (mUnUsedIdxs.Count > 0)
            {
                var index = mUnUsedIdxs.Dequeue();
                return mUnits[index];
            }
            var unit = new FlagComponentEntity<T>();
            unit.Index = mUnits.Count;
            mUnits.Add(unit);
            return unit;
        }

        public IComponent Add(EntityIdentify entityID)
        {
            if (mIdIdxMap.ContainsKey(entityID.Index))
            {
                return Component;
            }
            var unit = Create();
            mIdIdxMap.Add(entityID.Index, unit.Index);
            unit.Owner = entityID;
            return Component;
        }

        public ComponentFindResult<T> Find(int startIndex, Func<T, bool> condition = null)
        {
            for (int i = startIndex; i < mUnits.Count; ++i)
            {
                var unit = mUnits[i];
                if (!unit.Owner.Valid)
                    continue;
                return new ComponentFindResult<T>()
                {
                    EntityID = unit.Owner,
                    Index = i + 1,
                    Component = Component
                };
            }
            return default;
        }

        public IComponent Get(EntityIdentify entityID)
        {
            if (mIdIdxMap.ContainsKey(entityID.Index))
            {
                return Component;
            }
            return default;
        }

        public void Remove(EntityIdentify entityID)
        {
            if (mIdIdxMap.TryGetValue(entityID.Index, out int idx))
            {
                var unit = mUnits[idx];
                unit.Reset();
                mUnUsedIdxs.Enqueue(idx);
                mIdIdxMap.Remove(entityID.Index);
            }
        }

        public void RemoveAll()
        {
            foreach (var kv in mIdIdxMap)
            {
                var unit = mUnits[kv.Value];
                mUnUsedIdxs.Enqueue(kv.Value);
                unit.Reset();
            }
            mIdIdxMap.Clear();
        }
    }
}
