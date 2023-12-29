namespace ECSLite
{
    internal class FlagComponentEntity<T> where T : class, IComponent, new()
    {
        public EntityIdentify Owner;
        public int Index;

        public void Reset()
        {
            Owner = default;
        }
    }
}
