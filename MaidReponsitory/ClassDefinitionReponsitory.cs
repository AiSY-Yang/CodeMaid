using MaidContexts;

using MaidReponsitory.Base;

using Microsoft.EntityFrameworkCore;

using Models.Database;
using Models.DbContext;

using Reponsitory;


namespace MaidReponsitory
{
	public class ClassDefinitionReponsitory : DatabaseEntityReponsitory<ClassDefinition>
    {
        public ClassDefinitionReponsitory(MaidContext context) : base(context)
        {
        }
    }
}