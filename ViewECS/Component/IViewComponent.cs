namespace VECS
{
    public interface IViewComponent
    {
    }
    public class ViewComponentIdentity<T> where T : IViewComponent
    {
        public static int Id { get; set; } = -1;
        public static bool Unique { get; set; }
    }

    public class ViewComponentClear<T> where T : IViewComponent
    {
        public static System.Action<ViewEntity, T> OnRemove = (ViewEntity, T) =>{};
        public static System.Action<T> OnReset = (T) => { };
    }
}