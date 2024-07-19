using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Models.CodeMaid;

namespace ContextBases
{
	/// <summary>
	/// a context for backup database, no foreign key,you must add code in config
	/// <code>builder.Property&lt;long&gt;(&quot;BackupId&quot;).HasComment(&quot;a auto increment id for backup database&quot;);</code>
	/// <code>builder.Property&lt;DateTimeOffset&gt;(&quot;BackupTime&quot;).HasDefaultValueSql(&quot;current_timestamp&quot;).HasComment(&quot;backup time&quot;)</code>
	/// <code>builder.HasKey(&quot;BackupId&quot;)</code>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class BackupContextBase : ContextBase
	{
		public BackupContextBase(DbContextOptions options) : base(options)
		{
		}
		protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
		{
			configurationBuilder.IgnoreAny<DatabaseEntityBase>();
			base.ConfigureConventions(configurationBuilder);
		}
	}
}