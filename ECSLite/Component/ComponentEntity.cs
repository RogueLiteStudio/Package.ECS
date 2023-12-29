namespace ECSLite
{
    internal class ComponentEntity<T> where T : class, IComponent, new()
    {
        public T Component = new T();
        public EntityIdentify Owner;
        public int Index;
        public ulong Version;

        public void Reset()
        {
            Owner = default;
            Version = 0;
            ComponentReset<T>.OnReset(Component);
        }
    }
}
