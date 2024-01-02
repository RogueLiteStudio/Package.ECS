using System.IO;

namespace ECSEditor
{
    public static class ECSGenerator
    {

        public static void ECSLiteGen<TContext>(string nameSpace, string path, System.Func<System.Type, string, string> customReset)
        {
            ComponentCollector collector = new ComponentCollector();
            collector.Collector(nameSpace, typeof(TContext));
            ECSLiteContextGenerator.Gen(Path.Combine(path, $"{collector.ContextName}Context.cs"), collector);
            ECSLiteInitGenerator.Gen(Path.Combine(path, $"{collector.ContextName}ECSInit.cs"), collector);
            ECSLiteResetGenerator.Gen(Path.Combine(path, $"{collector.ContextName}ComponentReset.cs"), collector, customReset);
            ECSLiteClearupGenerator.Gen(Path.Combine(path, $"{collector.ContextName}ComponentReset_Init.cs"), collector);
        }

        public static void ViewECSGen(string nameSpace, string path, System.Func<System.Type, string, string> customReset)
        {
            ComponentCollector collector = new ComponentCollector();
            collector.Collector(nameSpace, typeof(VECS.IView), typeof(VECS.IViewComponent), typeof(VECS.IViewUniqueComponent), typeof(VECS.IViewStaticComponent));
            ViewECSInitGenerator.Gen(Path.Combine(path, $"{collector.ContextName}ECSInit.cs"), collector);
            ViewECSResetGenerator.Gen(Path.Combine(path, $"{collector.ContextName}ComponentClearup_Reset.cs"), collector, customReset);
            ViewECSClearupGenerator.Gen(Path.Combine(path, $"{collector.ContextName}ComponentClearup_Init.cs"), collector);
        }
    }

}