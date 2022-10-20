using MaidContexts;

using MaidReponsitory.Base;

using Models.CodeMaid;

namespace MaidReponsitory
{
    public class PropertyDefinitionReponsitory : DatabaseEntityReponsitory<PropertyDefinition>
    {
        public PropertyDefinitionReponsitory(MaidContext context) : base(context)
        {
        }
    }
}