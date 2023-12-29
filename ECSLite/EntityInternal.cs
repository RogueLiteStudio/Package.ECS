using System.Collections;

namespace ECSLite
{
    public class EntityInternal
    {
        public EntityIdentify ID;
        public BitArray ComponentFlag;
        public bool Used;

        public bool HasComponent(int index)
        {
            return ComponentFlag[index];
        }

        public void AddComponent(int index)
        {
            ComponentFlag[index] = true;
        }

        public void RemoveComponent(int index)
        {
            ComponentFlag[index] = false;
        }

        public void Clear()
        {
            Used = false;
            ID.Version++;
            ComponentFlag.SetAll(false);
        }
    }
}
