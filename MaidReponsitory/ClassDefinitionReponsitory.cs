using MaidContexts;

using MaidReponsitory.Base;

using Models.CodeMaid;


namespace MaidReponsitory
{
    public class ClassDefinitionReponsitory : DatabaseEntityReponsitory<ClassDefinition>
    {
        public ClassDefinitionReponsitory(MaidContext context) : base(context)
        {
        }
    }
}