using MaidContexts;

using MaidReponsitory.Base;

using Models.DbContext;

namespace MaidReponsitory
{
    public class NameSpaceDefinitionReponsitory : DatabaseEntityReponsitory<NameSpaceDefinition>
    {
        public NameSpaceDefinitionReponsitory(MaidContext context) : base(context)
        {
        }
    }
}