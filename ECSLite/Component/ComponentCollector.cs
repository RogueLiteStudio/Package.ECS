using System;
using System.Collections.Generic;

namespace ECSLite
{
    internal class ComponentCollector<T> : IComponentCollectorT<T> where T : class, IComponent, new()
    {
        private readonly List<ComponentEntity<T>> mUnits = new List<ComponentEntity<T>>();
        private readonly Queue<int> mUnUsedIdxs = new Queue<int>();
        private readonly Dictionary<int, int> mIdIdxMap = new Dictionary<int, int>();//index => 数组索引
        public int Count => mIdIdxMap.Count;
        private ComponentEntity<T> Create()
        {
            if (mUnUsedIdxs.Count > 0)
            {
                var index = mUnUsedIdxs.Dequeue();
                return mUnits[index];
            }
            var unit = new ComponentEntity<T>();
            unit.Index = mUnits.Count;
            mUnits.Add(unit);
            return unit;
        }

        public IComponent Add(int entityIdx)
        {
            if (mIdIdxMap.TryGetValue(entityIdx, out int idx))
            {
                var exist = mUnits[idx];
                return exist.Component;
            }
            var unit = Create();
            mIdIdxMap.Add(entityIdx, unit.Index);
            unit.EntityIdx = entityIdx;
            return unit.Component;
        }

        public ComponentFindResult<T> Find(int startIndex, Func<T, bool> condition = null)
        {
            for (int i = startIndex; i < mUnits.Count; ++i)
            {
                var unit = mUnits[i];
                if (unit.EntityIdx < 0)
                    continue;
                if (condition == null || condition(unit.Component))
                {
                    return new ComponentFindResult<T>()
                    {
                        EntityIndex = unit.EntityIdx,
                        Index = i + 1,
                        Component = unit.Component
                    };
                }
            }
            return default;
        }

        public IComponent Get(int entityIdx)
        {
            if (mIdIdxMap.TryGetValue(entityIdx, out int idx))
            {
                return mUnits[idx].Component;
            }
            return null;
        }

        public void Remove(int entityIdx)
        {
            if (mIdIdxMap.TryGetValue(entityIdx, out int idx))
            {
                var unit = mUnits[idx];
                unit.Reset();
                mUnUsedIdxs.Enqueue(idx);
                mIdIdxMap.Remove(entityIdx);
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
