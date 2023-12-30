namespace VECS
{
    public interface IViewStaticComponent
    {
    }
    public class ViewStaticComponentIdentity<T> where T : IViewStaticComponent
    {
        public static int Id { get; set; } = -1;
    }
}