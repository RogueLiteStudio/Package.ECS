namespace VECS
{
    //Context���ͣ���Ҫ���������������ɴ������ECSLite
    public interface IView { }

    public interface IViewComponent : IView
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