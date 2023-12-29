namespace ECSLite
{
    public struct ComponentFindResult<TComponent> where TComponent : IComponent
    {
        public EntityIdentify EntityID;
        public int Index;
        public TComponent Component;
    }

    public struct EntityFindResult<TEntity, TComponent> where TComponent : IComponent
    {
        public TEntity Entity;
        public int Index;
        public TComponent Component;
    }

}
