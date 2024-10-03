#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
using System.Text.Json.Serialization;
namespace RestfulClient.SelfModel;
/// <summary>
/// action信息
/// </summary>
public class ApiControllersCommonsActionInfo
{
	/// <summary>
	/// action名称
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 参数列表
	/// </summary>
	[JsonPropertyName("paramList")]
	public List<ApiControllersCommonsParameterInfo>? ParamList { get; set; }
}
/// <summary>
/// 控制器信息
/// </summary>
public class ApiControllersCommonsControllerInfo
{
	/// <summary>
	/// 控制器名称
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// action列表
	/// </summary>
	[JsonPropertyName("actionList")]
	public List<ApiControllersCommonsActionInfo>? ActionList { get; set; }
}
/// <summary>
/// 参数信息
/// </summary>
public class ApiControllersCommonsParameterInfo
{
	/// <summary>
	/// 参数名称
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("type")]
	public SystemType? Type { get; set; }
}
/// <summary>
/// 文件
/// </summary>
public class ApiControllersCommonsSystemFile
{
	/// <summary>
	/// 名称
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 完整路径
	/// </summary>
	[JsonPropertyName("path")]
	public string? Path { get; set; }
}
/// <summary>
/// 文件夹
/// </summary>
public class ApiControllersCommonsSystemFolder
{
	/// <summary>
	/// 名称
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 完整路径
	/// </summary>
	[JsonPropertyName("path")]
	public string? Path { get; set; }
	/// <summary>
	/// 子文件夹
	/// </summary>
	[JsonPropertyName("systemFolders")]
	public List<ApiControllersCommonsSystemFolder>? SystemFolders { get; set; }
	/// <summary>
	/// 文件
	/// </summary>
	[JsonPropertyName("systemFiles")]
	public List<ApiControllersCommonsSystemFile>? SystemFiles { get; set; }
}
/// <summary>
/// 
/// </summary>
public class MicrosoftAspNetCoreMvcProblemDetails
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("type")]
	public string? Type { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("status")]
	public int? Status { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("detail")]
	public string? Detail { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("instance")]
	public string? Instance { get; set; }
}
/// <summary>
/// 属性定义
/// </summary>
public class ModelsCodeMaidAttributeDefinition
{
	/// <summary>
	/// 唯一ID1
	/// </summary>
	[JsonPropertyName("id")]
	public long? Id { get; set; }
	/// <summary>
	/// 创建时间
	/// </summary>
	[JsonPropertyName("createTime")]
	public DateTimeOffset? CreateTime { get; set; }
	/// <summary>
	/// 更新时间
	/// </summary>
	[JsonPropertyName("updateTime")]
	public DateTimeOffset? UpdateTime { get; set; }
	/// <summary>
	/// 是否有效
	/// </summary>
	[JsonPropertyName("isDeleted")]
	public bool? IsDeleted { get; set; }
	/// <summary>
	/// Attribute名称
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// Attribute文本
	/// </summary>
	[JsonPropertyName("text")]
	public string? Text { get; set; }
	/// <summary>
	/// 参数文本
	/// </summary>
	[JsonPropertyName("argumentsText")]
	public string? ArgumentsText { get; set; }
	/// <summary>
	/// 参数
	/// </summary>
	[JsonPropertyName("arguments")]
	public string? Arguments { get; set; }
	/// <summary>
	/// 类定义
	/// </summary>
	[JsonPropertyName("propertyDefinition")]
	public ModelsCodeMaidPropertyDefinition? PropertyDefinition { get; set; }
}
/// <summary>
/// 类定义
/// </summary>
public class ModelsCodeMaidClassDefinition
{
	/// <summary>
	/// 唯一ID1
	/// </summary>
	[JsonPropertyName("id")]
	public long? Id { get; set; }
	/// <summary>
	/// 创建时间
	/// </summary>
	[JsonPropertyName("createTime")]
	public DateTimeOffset? CreateTime { get; set; }
	/// <summary>
	/// 更新时间
	/// </summary>
	[JsonPropertyName("updateTime")]
	public DateTimeOffset? UpdateTime { get; set; }
	/// <summary>
	/// 是否有效
	/// </summary>
	[JsonPropertyName("isDeleted")]
	public bool? IsDeleted { get; set; }
	/// <summary>
	/// 命名空间
	/// </summary>
	[JsonPropertyName("nameSpace")]
	public string? NameSpace { get; set; }
	/// <summary>
	/// 修饰符
	/// </summary>
	[JsonPropertyName("modifiers")]
	public string? Modifiers { get; set; }
	/// <summary>
	/// 是否是抽象类
	/// </summary>
	[JsonPropertyName("isAbstract")]
	public bool? IsAbstract { get; set; }
	/// <summary>
	/// 类名
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 注释
	/// </summary>
	[JsonPropertyName("summary")]
	public string? Summary { get; set; }
	/// <summary>
	/// 基类或者接口名称
	/// </summary>
	[JsonPropertyName("base")]
	public string? Base { get; set; }
	/// <summary>
	/// 类引用的命名空间
	/// </summary>
	[JsonPropertyName("using")]
	public string? Using { get; set; }
	/// <summary>
	/// 前导
	/// </summary>
	[JsonPropertyName("leadingTrivia")]
	public string? LeadingTrivia { get; set; }
	/// <summary>
	/// 成员类型
	/// </summary>
	[JsonPropertyName("memberType")]
	public ModelsCodeMaidMemberType? MemberType { get; set; }
	/// <summary>
	/// 项目定义
	/// </summary>
	[JsonPropertyName("project")]
	public ModelsCodeMaidProject? Project { get; set; }
	/// <summary>
	/// 所属项目
	/// </summary>
	[JsonPropertyName("projectId")]
	public long? ProjectId { get; set; }
	/// <summary>
	/// 属性列表
	/// </summary>
	[JsonPropertyName("properties")]
	public List<ModelsCodeMaidPropertyDefinition>? Properties { get; set; }
}
/// <summary>
/// 枚举定义
/// </summary>
public class ModelsCodeMaidEnumDefinition
{
	/// <summary>
	/// 唯一ID1
	/// </summary>
	[JsonPropertyName("id")]
	public long? Id { get; set; }
	/// <summary>
	/// 创建时间
	/// </summary>
	[JsonPropertyName("createTime")]
	public DateTimeOffset? CreateTime { get; set; }
	/// <summary>
	/// 更新时间
	/// </summary>
	[JsonPropertyName("updateTime")]
	public DateTimeOffset? UpdateTime { get; set; }
	/// <summary>
	/// 是否有效
	/// </summary>
	[JsonPropertyName("isDeleted")]
	public bool? IsDeleted { get; set; }
	/// <summary>
	/// 项目文件
	/// </summary>
	[JsonPropertyName("projectDirectoryFile")]
	public ModelsCodeMaidProjectDirectoryFile? ProjectDirectoryFile { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("projectDirectoryFileId")]
	public long? ProjectDirectoryFileId { get; set; }
	/// <summary>
	/// 命名空间
	/// </summary>
	[JsonPropertyName("nameSpace")]
	public string? NameSpace { get; set; }
	/// <summary>
	/// 枚举名
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 注释
	/// </summary>
	[JsonPropertyName("summary")]
	public string? Summary { get; set; }
	/// <summary>
	/// 备注
	/// </summary>
	[JsonPropertyName("remark")]
	public string? Remark { get; set; }
	/// <summary>
	/// 前导
	/// </summary>
	[JsonPropertyName("leadingTrivia")]
	public string? LeadingTrivia { get; set; }
	/// <summary>
	/// 成员列表
	/// </summary>
	[JsonPropertyName("enumMembers")]
	public List<ModelsCodeMaidEnumMemberDefinition>? EnumMembers { get; set; }
}
/// <summary>
/// 枚举成员定义
/// </summary>
public class ModelsCodeMaidEnumMemberDefinition
{
	/// <summary>
	/// 唯一ID1
	/// </summary>
	[JsonPropertyName("id")]
	public long? Id { get; set; }
	/// <summary>
	/// 创建时间
	/// </summary>
	[JsonPropertyName("createTime")]
	public DateTimeOffset? CreateTime { get; set; }
	/// <summary>
	/// 更新时间
	/// </summary>
	[JsonPropertyName("updateTime")]
	public DateTimeOffset? UpdateTime { get; set; }
	/// <summary>
	/// 是否有效
	/// </summary>
	[JsonPropertyName("isDeleted")]
	public bool? IsDeleted { get; set; }
	/// <summary>
	/// 枚举名称
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 枚举值
	/// </summary>
	[JsonPropertyName("value")]
	public int? Value { get; set; }
	/// <summary>
	/// 注释
	/// </summary>
	[JsonPropertyName("summary")]
	public string? Summary { get; set; }
	/// <summary>
	/// 描述
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; set; }
}
public enum ModelsCodeMaidFileType
{
	Other = 0,
	CSahrp = 1,
}
/// <summary>
/// 功能
/// </summary>
public class ModelsCodeMaidMaid
{
	/// <summary>
	/// 唯一ID1
	/// </summary>
	[JsonPropertyName("id")]
	public long? Id { get; set; }
	/// <summary>
	/// 创建时间
	/// </summary>
	[JsonPropertyName("createTime")]
	public DateTimeOffset? CreateTime { get; set; }
	/// <summary>
	/// 更新时间
	/// </summary>
	[JsonPropertyName("updateTime")]
	public DateTimeOffset? UpdateTime { get; set; }
	/// <summary>
	/// 是否有效
	/// </summary>
	[JsonPropertyName("isDeleted")]
	public bool? IsDeleted { get; set; }
	/// <summary>
	/// 名称
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 项目定义
	/// </summary>
	[JsonPropertyName("project")]
	public ModelsCodeMaidProject? Project { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("projectId")]
	public long? ProjectId { get; set; }
	/// <summary>
	/// 功能
	/// </summary>
	[JsonPropertyName("maidWork")]
	public ModelsCodeMaidMaidWork? MaidWork { get; set; }
	/// <summary>
	/// 原路径
	/// </summary>
	[JsonPropertyName("sourcePath")]
	public string? SourcePath { get; set; }
	/// <summary>
	/// 目标路径
	/// </summary>
	[JsonPropertyName("destinationPath")]
	public string? DestinationPath { get; set; }
	/// <summary>
	/// 是否自动修复
	/// </summary>
	[JsonPropertyName("autonomous")]
	public bool? Autonomous { get; set; }
	/// <summary>
	/// 设置
	/// </summary>
	[JsonPropertyName("setting")]
	public JsonElement? Setting { get; set; }
}
public enum ModelsCodeMaidMaidWork
{
	ConfigurationSync = 0,
	DtoSync = 1,
	HttpClientSync = 2,
	ControllerSync = 3,
	MasstransitConsumerSync = 4,
}
public enum ModelsCodeMaidMemberType
{
	ClassDeclarationSyntax = 0,
	InterfaceDeclarationSyntax = 1,
	RecordDeclarationSyntax = 2,
	StructDeclarationSyntax = 3,
}
/// <summary>
/// 项目定义
/// </summary>
public class ModelsCodeMaidProject
{
	/// <summary>
	/// 唯一ID1
	/// </summary>
	[JsonPropertyName("id")]
	public long? Id { get; set; }
	/// <summary>
	/// 创建时间
	/// </summary>
	[JsonPropertyName("createTime")]
	public DateTimeOffset? CreateTime { get; set; }
	/// <summary>
	/// 更新时间
	/// </summary>
	[JsonPropertyName("updateTime")]
	public DateTimeOffset? UpdateTime { get; set; }
	/// <summary>
	/// 是否有效
	/// </summary>
	[JsonPropertyName("isDeleted")]
	public bool? IsDeleted { get; set; }
	/// <summary>
	/// 项目名
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 项目路径
	/// </summary>
	[JsonPropertyName("path")]
	public string? Path { get; set; }
	/// <summary>
	/// Git分支
	/// </summary>
	[JsonPropertyName("gitBranch")]
	public string? GitBranch { get; set; }
	/// <summary>
	/// 是否添加枚举的remark信息
	/// </summary>
	[JsonPropertyName("addEnumRemark")]
	public bool? AddEnumRemark { get; set; }
	/// <summary>
	/// maid集合
	/// </summary>
	[JsonPropertyName("maids")]
	public List<ModelsCodeMaidMaid>? Maids { get; set; }
	/// <summary>
	/// 项目目录
	/// </summary>
	[JsonPropertyName("projectDirectories")]
	public List<ModelsCodeMaidProjectDirectory>? ProjectDirectories { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("enumDefinitions")]
	public List<ModelsCodeMaidEnumDefinition>? EnumDefinitions { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("classDefinitions")]
	public List<ModelsCodeMaidClassDefinition>? ClassDefinitions { get; set; }
}
/// <summary>
/// 项目目录
/// </summary>
public class ModelsCodeMaidProjectDirectory
{
	/// <summary>
	/// 唯一ID1
	/// </summary>
	[JsonPropertyName("id")]
	public long? Id { get; set; }
	/// <summary>
	/// 创建时间
	/// </summary>
	[JsonPropertyName("createTime")]
	public DateTimeOffset? CreateTime { get; set; }
	/// <summary>
	/// 更新时间
	/// </summary>
	[JsonPropertyName("updateTime")]
	public DateTimeOffset? UpdateTime { get; set; }
	/// <summary>
	/// 是否有效
	/// </summary>
	[JsonPropertyName("isDeleted")]
	public bool? IsDeleted { get; set; }
	/// <summary>
	/// 目录名
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 路径
	/// </summary>
	[JsonPropertyName("path")]
	public string? Path { get; set; }
	/// <summary>
	/// 项目定义
	/// </summary>
	[JsonPropertyName("project")]
	public ModelsCodeMaidProject? Project { get; set; }
	/// <summary>
	/// 项目Id
	/// </summary>
	[JsonPropertyName("projectId")]
	public long? ProjectId { get; set; }
	/// <summary>
	/// 目录
	/// </summary>
	[JsonPropertyName("projectDirectoryFiles")]
	public List<ModelsCodeMaidProjectDirectoryFile>? ProjectDirectoryFiles { get; set; }
}
/// <summary>
/// 项目文件
/// </summary>
public class ModelsCodeMaidProjectDirectoryFile
{
	/// <summary>
	/// 唯一ID1
	/// </summary>
	[JsonPropertyName("id")]
	public long? Id { get; set; }
	/// <summary>
	/// 创建时间
	/// </summary>
	[JsonPropertyName("createTime")]
	public DateTimeOffset? CreateTime { get; set; }
	/// <summary>
	/// 更新时间
	/// </summary>
	[JsonPropertyName("updateTime")]
	public DateTimeOffset? UpdateTime { get; set; }
	/// <summary>
	/// 是否有效
	/// </summary>
	[JsonPropertyName("isDeleted")]
	public bool? IsDeleted { get; set; }
	/// <summary>
	/// 项目定义
	/// </summary>
	[JsonPropertyName("project")]
	public ModelsCodeMaidProject? Project { get; set; }
	/// <summary>
	/// 项目Id
	/// </summary>
	[JsonPropertyName("projectId")]
	public long? ProjectId { get; set; }
	/// <summary>
	/// 文件名
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 文件最后修改时间
	/// </summary>
	[JsonPropertyName("lastWriteTime")]
	public DateTimeOffset? LastWriteTime { get; set; }
	/// <summary>
	/// 路径
	/// </summary>
	[JsonPropertyName("path")]
	public string? Path { get; set; }
	/// <summary>
	/// 项目目录
	/// </summary>
	[JsonPropertyName("projectDirectory")]
	public ModelsCodeMaidProjectDirectory? ProjectDirectory { get; set; }
	/// <summary>
	/// 是否是自动生成的文件
	/// </summary>
	[JsonPropertyName("isAutoGen")]
	public bool? IsAutoGen { get; set; }
	/// <summary>
	/// 总行数
	/// </summary>
	[JsonPropertyName("linesCount")]
	public int? LinesCount { get; set; }
	/// <summary>
	/// 空行数
	/// </summary>
	[JsonPropertyName("spaceCount")]
	public int? SpaceCount { get; set; }
	/// <summary>
	/// 注释行数
	/// </summary>
	[JsonPropertyName("commentCount")]
	public int? CommentCount { get; set; }
	/// <summary>
	/// 文件类型
	/// </summary>
	[JsonPropertyName("fileType")]
	public ModelsCodeMaidFileType? FileType { get; set; }
	/// <summary>
	/// 项目结构
	/// </summary>
	[JsonPropertyName("projectStructures")]
	public List<ModelsCodeMaidProjectStructure>? ProjectStructures { get; set; }
	/// <summary>
	/// 关联枚举
	/// </summary>
	[JsonPropertyName("enumDefinitions")]
	public List<ModelsCodeMaidEnumDefinition>? EnumDefinitions { get; set; }
}
/// <summary>
/// 项目结构
/// </summary>
public class ModelsCodeMaidProjectStructure
{
	/// <summary>
	/// 唯一ID1
	/// </summary>
	[JsonPropertyName("id")]
	public long? Id { get; set; }
	/// <summary>
	/// 创建时间
	/// </summary>
	[JsonPropertyName("createTime")]
	public DateTimeOffset? CreateTime { get; set; }
	/// <summary>
	/// 更新时间
	/// </summary>
	[JsonPropertyName("updateTime")]
	public DateTimeOffset? UpdateTime { get; set; }
	/// <summary>
	/// 是否有效
	/// </summary>
	[JsonPropertyName("isDeleted")]
	public bool? IsDeleted { get; set; }
	/// <summary>
	/// 项目文件
	/// </summary>
	[JsonPropertyName("projectDirectoryFile")]
	public ModelsCodeMaidProjectDirectoryFile? ProjectDirectoryFile { get; set; }
	/// <summary>
	/// 类定义
	/// </summary>
	[JsonPropertyName("classDefinition")]
	public ModelsCodeMaidClassDefinition? ClassDefinition { get; set; }
	/// <summary>
	/// 属性
	/// </summary>
	[JsonPropertyName("propertyDefinitions")]
	public List<ModelsCodeMaidPropertyDefinition>? PropertyDefinitions { get; set; }
}
/// <summary>
/// 类定义
/// </summary>
public class ModelsCodeMaidPropertyDefinition
{
	/// <summary>
	/// 唯一ID1
	/// </summary>
	[JsonPropertyName("id")]
	public long? Id { get; set; }
	/// <summary>
	/// 创建时间
	/// </summary>
	[JsonPropertyName("createTime")]
	public DateTimeOffset? CreateTime { get; set; }
	/// <summary>
	/// 更新时间
	/// </summary>
	[JsonPropertyName("updateTime")]
	public DateTimeOffset? UpdateTime { get; set; }
	/// <summary>
	/// 是否有效
	/// </summary>
	[JsonPropertyName("isDeleted")]
	public bool? IsDeleted { get; set; }
	/// <summary>
	/// 类定义
	/// </summary>
	[JsonPropertyName("classDefinition")]
	public ModelsCodeMaidClassDefinition? ClassDefinition { get; set; }
	/// <summary>
	/// 所属类Id
	/// </summary>
	[JsonPropertyName("classDefinitionId")]
	public long? ClassDefinitionId { get; set; }
	/// <summary>
	/// 前导
	/// </summary>
	[JsonPropertyName("leadingTrivia")]
	public string? LeadingTrivia { get; set; }
	/// <summary>
	/// 注释
	/// </summary>
	[JsonPropertyName("summary")]
	public string? Summary { get; set; }
	/// <summary>
	/// 备注
	/// </summary>
	[JsonPropertyName("remark")]
	public string? Remark { get; set; }
	/// <summary>
	/// 完整文本内容
	/// </summary>
	[JsonPropertyName("fullText")]
	public string? FullText { get; set; }
	/// <summary>
	/// 修饰符
	/// </summary>
	[JsonPropertyName("modifiers")]
	public string? Modifiers { get; set; }
	/// <summary>
	/// 初始化器
	/// </summary>
	[JsonPropertyName("initializer")]
	public string? Initializer { get; set; }
	/// <summary>
	/// 属性名称
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 数据类型
	/// </summary>
	[JsonPropertyName("type")]
	public string? Type { get; set; }
	/// <summary>
	/// 是否是枚举
	/// </summary>
	[JsonPropertyName("isEnum")]
	public bool? IsEnum { get; set; }
	/// <summary>
	/// 是否包含Get
	/// </summary>
	[JsonPropertyName("hasGet")]
	public bool? HasGet { get; set; }
	/// <summary>
	/// Get方法体
	/// </summary>
	[JsonPropertyName("get")]
	public string? Get { get; set; }
	/// <summary>
	/// 是否包含Set
	/// </summary>
	[JsonPropertyName("hasSet")]
	public bool? HasSet { get; set; }
	/// <summary>
	/// Set方法体
	/// </summary>
	[JsonPropertyName("set")]
	public string? Set { get; set; }
	/// <summary>
	/// 枚举定义
	/// </summary>
	[JsonPropertyName("enumDefinition")]
	public ModelsCodeMaidEnumDefinition? EnumDefinition { get; set; }
	/// <summary>
	/// 属性列表
	/// </summary>
	[JsonPropertyName("attributes")]
	public List<ModelsCodeMaidAttributeDefinition>? Attributes { get; set; }
	/// <summary>
	/// 项目文件
	/// </summary>
	[JsonPropertyName("projectDirectoryFile")]
	public ModelsCodeMaidProjectDirectoryFile? ProjectDirectoryFile { get; set; }
}
/// <summary>
/// 业务异常返回的结果模型
/// </summary>
public class ServicesModelsResultsIExceptionResult
{
	/// <summary>
	/// 错误码
	/// </summary>
	[JsonPropertyName("code")]
	public string? Code { get; set; }
	/// <summary>
	/// 异常消息
	/// </summary>
	[JsonPropertyName("msg")]
	public string? Msg { get; set; }
}
/// <summary>
/// 
/// </summary>
public class SystemIntPtr
{
}
/// <summary>
/// 
/// </summary>
public class SystemModuleHandle
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("mdStreamVersion")]
	public int? MdStreamVersion { get; set; }
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionAssembly
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("definedTypes")]
	public List<SystemReflectionTypeInfo>? DefinedTypes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("exportedTypes")]
	public List<SystemType>? ExportedTypes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("codeBase")]
	public string? CodeBase { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("entryPoint")]
	public SystemReflectionMethodInfo? EntryPoint { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("fullName")]
	public string? FullName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("imageRuntimeVersion")]
	public string? ImageRuntimeVersion { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isDynamic")]
	public bool? IsDynamic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("location")]
	public string? Location { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("reflectionOnly")]
	public bool? ReflectionOnly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCollectible")]
	public bool? IsCollectible { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFullyTrusted")]
	public bool? IsFullyTrusted { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("escapedCodeBase")]
	public string? EscapedCodeBase { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("manifestModule")]
	public SystemReflectionModule? ManifestModule { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("modules")]
	public List<SystemReflectionModule>? Modules { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("globalAssemblyCache")]
	public bool? GlobalAssemblyCache { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("hostContext")]
	public long? HostContext { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("securityRuleSet")]
	public SystemSecuritySecurityRuleSet? SecurityRuleSet { get; set; }
}
public enum SystemReflectionCallingConventions
{
	Standard = 1,
	VarArgs = 2,
	Any = 3,
	HasThis = 32,
	ExplicitThis = 64,
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionConstructorInfo
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaringType")]
	public SystemType? DeclaringType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("reflectedType")]
	public SystemType? ReflectedType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("module")]
	public SystemReflectionModule? Module { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCollectible")]
	public bool? IsCollectible { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("metadataToken")]
	public int? MetadataToken { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("attributes")]
	public SystemReflectionMethodAttributes? Attributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("methodImplementationFlags")]
	public SystemReflectionMethodImplAttributes? MethodImplementationFlags { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("callingConvention")]
	public SystemReflectionCallingConventions? CallingConvention { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAbstract")]
	public bool? IsAbstract { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isConstructor")]
	public bool? IsConstructor { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFinal")]
	public bool? IsFinal { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isHideBySig")]
	public bool? IsHideBySig { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSpecialName")]
	public bool? IsSpecialName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isStatic")]
	public bool? IsStatic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isVirtual")]
	public bool? IsVirtual { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAssembly")]
	public bool? IsAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamily")]
	public bool? IsFamily { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamilyAndAssembly")]
	public bool? IsFamilyAndAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamilyOrAssembly")]
	public bool? IsFamilyOrAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPrivate")]
	public bool? IsPrivate { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPublic")]
	public bool? IsPublic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isConstructedGenericMethod")]
	public bool? IsConstructedGenericMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericMethod")]
	public bool? IsGenericMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericMethodDefinition")]
	public bool? IsGenericMethodDefinition { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("containsGenericParameters")]
	public bool? ContainsGenericParameters { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("methodHandle")]
	public SystemRuntimeMethodHandle? MethodHandle { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityCritical")]
	public bool? IsSecurityCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecuritySafeCritical")]
	public bool? IsSecuritySafeCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityTransparent")]
	public bool? IsSecurityTransparent { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("memberType")]
	public SystemReflectionMemberTypes? MemberType { get; set; }
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionCustomAttributeData
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("attributeType")]
	public SystemType? AttributeType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("constructor")]
	public SystemReflectionConstructorInfo? Constructor { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("constructorArguments")]
	public List<SystemReflectionCustomAttributeTypedArgument>? ConstructorArguments { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("namedArguments")]
	public List<SystemReflectionCustomAttributeNamedArgument>? NamedArguments { get; set; }
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionCustomAttributeNamedArgument
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("memberInfo")]
	public SystemReflectionMemberInfo? MemberInfo { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("typedValue")]
	public SystemReflectionCustomAttributeTypedArgument? TypedValue { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("memberName")]
	public string? MemberName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isField")]
	public bool? IsField { get; set; }
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionCustomAttributeTypedArgument
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("argumentType")]
	public SystemType? ArgumentType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("value")]
	public JsonElement? Value { get; set; }
}
public enum SystemReflectionEventAttributes
{
	None = 0,
	SpecialName = 512,
	RTSpecialName = 1024,
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionEventInfo
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaringType")]
	public SystemType? DeclaringType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("reflectedType")]
	public SystemType? ReflectedType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("module")]
	public SystemReflectionModule? Module { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCollectible")]
	public bool? IsCollectible { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("metadataToken")]
	public int? MetadataToken { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("memberType")]
	public SystemReflectionMemberTypes? MemberType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("attributes")]
	public SystemReflectionEventAttributes? Attributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSpecialName")]
	public bool? IsSpecialName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("addMethod")]
	public SystemReflectionMethodInfo? AddMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("removeMethod")]
	public SystemReflectionMethodInfo? RemoveMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("raiseMethod")]
	public SystemReflectionMethodInfo? RaiseMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isMulticast")]
	public bool? IsMulticast { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("eventHandlerType")]
	public SystemType? EventHandlerType { get; set; }
}
public enum SystemReflectionFieldAttributes
{
	PrivateScope = 0,
	Private = 1,
	FamAndAssem = 2,
	Assembly = 3,
	Family = 4,
	FamORAssem = 5,
	Public = 6,
	FieldAccessMask = 7,
	Static = 16,
	InitOnly = 32,
	Literal = 64,
	NotSerialized = 128,
	HasFieldRva = 256,
	SpecialName = 512,
	RTSpecialName = 1024,
	HasFieldMarshal = 4096,
	PinvokeImpl = 8192,
	HasDefault = 32768,
	ReservedMask = 38144,
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionFieldInfo
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaringType")]
	public SystemType? DeclaringType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("reflectedType")]
	public SystemType? ReflectedType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("module")]
	public SystemReflectionModule? Module { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCollectible")]
	public bool? IsCollectible { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("metadataToken")]
	public int? MetadataToken { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("memberType")]
	public SystemReflectionMemberTypes? MemberType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("attributes")]
	public SystemReflectionFieldAttributes? Attributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("fieldType")]
	public SystemType? FieldType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isInitOnly")]
	public bool? IsInitOnly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isLiteral")]
	public bool? IsLiteral { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNotSerialized")]
	public bool? IsNotSerialized { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPinvokeImpl")]
	public bool? IsPinvokeImpl { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSpecialName")]
	public bool? IsSpecialName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isStatic")]
	public bool? IsStatic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAssembly")]
	public bool? IsAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamily")]
	public bool? IsFamily { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamilyAndAssembly")]
	public bool? IsFamilyAndAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamilyOrAssembly")]
	public bool? IsFamilyOrAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPrivate")]
	public bool? IsPrivate { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPublic")]
	public bool? IsPublic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityCritical")]
	public bool? IsSecurityCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecuritySafeCritical")]
	public bool? IsSecuritySafeCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityTransparent")]
	public bool? IsSecurityTransparent { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("fieldHandle")]
	public SystemRuntimeFieldHandle? FieldHandle { get; set; }
}
public enum SystemReflectionGenericParameterAttributes
{
	None = 0,
	Covariant = 1,
	Contravariant = 2,
	VarianceMask = 3,
	ReferenceTypeConstraint = 4,
	NotNullableValueTypeConstraint = 8,
	DefaultConstructorConstraint = 16,
	SpecialConstraintMask = 28,
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionICustomAttributeProvider
{
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionMemberInfo
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("memberType")]
	public SystemReflectionMemberTypes? MemberType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaringType")]
	public SystemType? DeclaringType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("reflectedType")]
	public SystemType? ReflectedType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("module")]
	public SystemReflectionModule? Module { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCollectible")]
	public bool? IsCollectible { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("metadataToken")]
	public int? MetadataToken { get; set; }
}
public enum SystemReflectionMemberTypes
{
	Constructor = 1,
	Event = 2,
	Field = 4,
	Method = 8,
	Property = 16,
	TypeInfo = 32,
	Custom = 64,
	NestedType = 128,
	All = 191,
}
public enum SystemReflectionMethodAttributes
{
	PrivateScope = 0,
	ReuseSlot = 1,
	Private = 2,
	FamAndAssem = 3,
	Assembly = 4,
	Family = 5,
	FamORAssem = 6,
	Public = 7,
	MemberAccessMask = 8,
	UnmanagedExport = 16,
	Static = 32,
	Final = 64,
	Virtual = 128,
	HideBySig = 256,
	NewSlot = 512,
	VtableLayoutMask = 1024,
	CheckAccessOnOverride = 2048,
	Abstract = 4096,
	SpecialName = 8192,
	RTSpecialName = 16384,
	PinvokeImpl = 32768,
	HasSecurity = 53248,
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionMethodBase
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("memberType")]
	public SystemReflectionMemberTypes? MemberType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaringType")]
	public SystemType? DeclaringType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("reflectedType")]
	public SystemType? ReflectedType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("module")]
	public SystemReflectionModule? Module { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCollectible")]
	public bool? IsCollectible { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("metadataToken")]
	public int? MetadataToken { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("attributes")]
	public SystemReflectionMethodAttributes? Attributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("methodImplementationFlags")]
	public SystemReflectionMethodImplAttributes? MethodImplementationFlags { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("callingConvention")]
	public SystemReflectionCallingConventions? CallingConvention { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAbstract")]
	public bool? IsAbstract { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isConstructor")]
	public bool? IsConstructor { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFinal")]
	public bool? IsFinal { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isHideBySig")]
	public bool? IsHideBySig { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSpecialName")]
	public bool? IsSpecialName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isStatic")]
	public bool? IsStatic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isVirtual")]
	public bool? IsVirtual { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAssembly")]
	public bool? IsAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamily")]
	public bool? IsFamily { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamilyAndAssembly")]
	public bool? IsFamilyAndAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamilyOrAssembly")]
	public bool? IsFamilyOrAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPrivate")]
	public bool? IsPrivate { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPublic")]
	public bool? IsPublic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isConstructedGenericMethod")]
	public bool? IsConstructedGenericMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericMethod")]
	public bool? IsGenericMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericMethodDefinition")]
	public bool? IsGenericMethodDefinition { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("containsGenericParameters")]
	public bool? ContainsGenericParameters { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("methodHandle")]
	public SystemRuntimeMethodHandle? MethodHandle { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityCritical")]
	public bool? IsSecurityCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecuritySafeCritical")]
	public bool? IsSecuritySafeCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityTransparent")]
	public bool? IsSecurityTransparent { get; set; }
}
public enum SystemReflectionMethodImplAttributes
{
	IL = 0,
	Managed = 1,
	Native = 2,
	Optil = 3,
	CodeTypeMask = 4,
	Runtime = 8,
	ManagedMask = 16,
	Unmanaged = 32,
	NoInlining = 64,
	ForwardRef = 128,
	Synchronized = 256,
	NoOptimization = 512,
	PreserveSig = 4096,
	AggressiveInlining = 65535,
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionMethodInfo
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaringType")]
	public SystemType? DeclaringType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("reflectedType")]
	public SystemType? ReflectedType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("module")]
	public SystemReflectionModule? Module { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCollectible")]
	public bool? IsCollectible { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("metadataToken")]
	public int? MetadataToken { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("attributes")]
	public SystemReflectionMethodAttributes? Attributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("methodImplementationFlags")]
	public SystemReflectionMethodImplAttributes? MethodImplementationFlags { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("callingConvention")]
	public SystemReflectionCallingConventions? CallingConvention { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAbstract")]
	public bool? IsAbstract { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isConstructor")]
	public bool? IsConstructor { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFinal")]
	public bool? IsFinal { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isHideBySig")]
	public bool? IsHideBySig { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSpecialName")]
	public bool? IsSpecialName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isStatic")]
	public bool? IsStatic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isVirtual")]
	public bool? IsVirtual { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAssembly")]
	public bool? IsAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamily")]
	public bool? IsFamily { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamilyAndAssembly")]
	public bool? IsFamilyAndAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFamilyOrAssembly")]
	public bool? IsFamilyOrAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPrivate")]
	public bool? IsPrivate { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPublic")]
	public bool? IsPublic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isConstructedGenericMethod")]
	public bool? IsConstructedGenericMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericMethod")]
	public bool? IsGenericMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericMethodDefinition")]
	public bool? IsGenericMethodDefinition { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("containsGenericParameters")]
	public bool? ContainsGenericParameters { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("methodHandle")]
	public SystemRuntimeMethodHandle? MethodHandle { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityCritical")]
	public bool? IsSecurityCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecuritySafeCritical")]
	public bool? IsSecuritySafeCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityTransparent")]
	public bool? IsSecurityTransparent { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("memberType")]
	public SystemReflectionMemberTypes? MemberType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("returnParameter")]
	public SystemReflectionParameterInfo? ReturnParameter { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("returnType")]
	public SystemType? ReturnType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("returnTypeCustomAttributes")]
	public SystemReflectionICustomAttributeProvider? ReturnTypeCustomAttributes { get; set; }
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionModule
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("assembly")]
	public SystemReflectionAssembly? Assembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("fullyQualifiedName")]
	public string? FullyQualifiedName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("mdStreamVersion")]
	public int? MdStreamVersion { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("moduleVersionId")]
	public Guid? ModuleVersionId { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("scopeName")]
	public string? ScopeName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("moduleHandle")]
	public SystemModuleHandle? ModuleHandle { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("metadataToken")]
	public int? MetadataToken { get; set; }
}
public enum SystemReflectionParameterAttributes
{
	None = 0,
	In = 1,
	Out = 2,
	Lcid = 4,
	Retval = 8,
	Optional = 16,
	HasDefault = 4096,
	HasFieldMarshal = 8192,
	Reserved3 = 16384,
	Reserved4 = 32768,
	ReservedMask = 61440,
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionParameterInfo
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("attributes")]
	public SystemReflectionParameterAttributes? Attributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("member")]
	public SystemReflectionMemberInfo? Member { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("parameterType")]
	public SystemType? ParameterType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("position")]
	public int? Position { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isIn")]
	public bool? IsIn { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isLcid")]
	public bool? IsLcid { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isOptional")]
	public bool? IsOptional { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isOut")]
	public bool? IsOut { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isRetval")]
	public bool? IsRetval { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("defaultValue")]
	public JsonElement? DefaultValue { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("rawDefaultValue")]
	public JsonElement? RawDefaultValue { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("hasDefaultValue")]
	public bool? HasDefaultValue { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("metadataToken")]
	public int? MetadataToken { get; set; }
}
public enum SystemReflectionPropertyAttributes
{
	None = 0,
	SpecialName = 512,
	RTSpecialName = 1024,
	HasDefault = 4096,
	Reserved2 = 8192,
	Reserved3 = 16384,
	Reserved4 = 32768,
	ReservedMask = 62464,
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionPropertyInfo
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaringType")]
	public SystemType? DeclaringType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("reflectedType")]
	public SystemType? ReflectedType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("module")]
	public SystemReflectionModule? Module { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCollectible")]
	public bool? IsCollectible { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("metadataToken")]
	public int? MetadataToken { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("memberType")]
	public SystemReflectionMemberTypes? MemberType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("propertyType")]
	public SystemType? PropertyType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("attributes")]
	public SystemReflectionPropertyAttributes? Attributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSpecialName")]
	public bool? IsSpecialName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("canRead")]
	public bool? CanRead { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("canWrite")]
	public bool? CanWrite { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("getMethod")]
	public SystemReflectionMethodInfo? GetMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("setMethod")]
	public SystemReflectionMethodInfo? SetMethod { get; set; }
}
public enum SystemReflectionTypeAttributes
{
	NotPublic = 0,
	AutoLayout = 1,
	AnsiClass = 2,
	Class = 3,
	Public = 4,
	NestedPublic = 5,
	NestedPrivate = 6,
	NestedFamily = 7,
	NestedAssembly = 8,
	NestedFamAndAssem = 16,
	VisibilityMask = 24,
	NestedFamORAssem = 32,
	SequentialLayout = 128,
	ExplicitLayout = 256,
	LayoutMask = 1024,
	Interface = 2048,
	ClassSemanticsMask = 4096,
	Abstract = 8192,
	Sealed = 16384,
	SpecialName = 65536,
	RTSpecialName = 131072,
	Import = 196608,
	Serializable = 262144,
	WindowsRuntime = 264192,
	UnicodeClass = 1048576,
	AutoClass = 12582912,
}
/// <summary>
/// 
/// </summary>
public class SystemReflectionTypeInfo
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCollectible")]
	public bool? IsCollectible { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("metadataToken")]
	public int? MetadataToken { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isInterface")]
	public bool? IsInterface { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("memberType")]
	public SystemReflectionMemberTypes? MemberType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("namespace")]
	public string? Namespace { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("assemblyQualifiedName")]
	public string? AssemblyQualifiedName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("fullName")]
	public string? FullName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("assembly")]
	public SystemReflectionAssembly? Assembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("module")]
	public SystemReflectionModule? Module { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNested")]
	public bool? IsNested { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaringType")]
	public SystemType? DeclaringType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaringMethod")]
	public SystemReflectionMethodBase? DeclaringMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("reflectedType")]
	public SystemType? ReflectedType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("underlyingSystemType")]
	public SystemType? UnderlyingSystemType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isTypeDefinition")]
	public bool? IsTypeDefinition { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isArray")]
	public bool? IsArray { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isByRef")]
	public bool? IsByRef { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPointer")]
	public bool? IsPointer { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isConstructedGenericType")]
	public bool? IsConstructedGenericType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericParameter")]
	public bool? IsGenericParameter { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericTypeParameter")]
	public bool? IsGenericTypeParameter { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericMethodParameter")]
	public bool? IsGenericMethodParameter { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericType")]
	public bool? IsGenericType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericTypeDefinition")]
	public bool? IsGenericTypeDefinition { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSZArray")]
	public bool? IsSZArray { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isVariableBoundArray")]
	public bool? IsVariableBoundArray { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isByRefLike")]
	public bool? IsByRefLike { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFunctionPointer")]
	public bool? IsFunctionPointer { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isUnmanagedFunctionPointer")]
	public bool? IsUnmanagedFunctionPointer { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("hasElementType")]
	public bool? HasElementType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("genericTypeArguments")]
	public List<SystemType>? GenericTypeArguments { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("genericParameterPosition")]
	public int? GenericParameterPosition { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("genericParameterAttributes")]
	public SystemReflectionGenericParameterAttributes? GenericParameterAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("attributes")]
	public SystemReflectionTypeAttributes? Attributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAbstract")]
	public bool? IsAbstract { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isImport")]
	public bool? IsImport { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSealed")]
	public bool? IsSealed { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSpecialName")]
	public bool? IsSpecialName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isClass")]
	public bool? IsClass { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedAssembly")]
	public bool? IsNestedAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedFamANDAssem")]
	public bool? IsNestedFamAndAssem { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedFamily")]
	public bool? IsNestedFamily { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedFamORAssem")]
	public bool? IsNestedFamORAssem { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedPrivate")]
	public bool? IsNestedPrivate { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedPublic")]
	public bool? IsNestedPublic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNotPublic")]
	public bool? IsNotPublic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPublic")]
	public bool? IsPublic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAutoLayout")]
	public bool? IsAutoLayout { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isExplicitLayout")]
	public bool? IsExplicitLayout { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isLayoutSequential")]
	public bool? IsLayoutSequential { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAnsiClass")]
	public bool? IsAnsiClass { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAutoClass")]
	public bool? IsAutoClass { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isUnicodeClass")]
	public bool? IsUnicodeClass { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCOMObject")]
	public bool? IsComObject { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isContextful")]
	public bool? IsContextful { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isEnum")]
	public bool? IsEnum { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isMarshalByRef")]
	public bool? IsMarshalByRef { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPrimitive")]
	public bool? IsPrimitive { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isValueType")]
	public bool? IsValueType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSignatureType")]
	public bool? IsSignatureType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityCritical")]
	public bool? IsSecurityCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecuritySafeCritical")]
	public bool? IsSecuritySafeCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityTransparent")]
	public bool? IsSecurityTransparent { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("structLayoutAttribute")]
	public SystemRuntimeInteropServicesStructLayoutAttribute? StructLayoutAttribute { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("typeInitializer")]
	public SystemReflectionConstructorInfo? TypeInitializer { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("typeHandle")]
	public SystemRuntimeTypeHandle? TypeHandle { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("guid")]
	public Guid? Guid { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("baseType")]
	public SystemType? BaseType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSerializable")]
	public bool? IsSerializable { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("containsGenericParameters")]
	public bool? ContainsGenericParameters { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isVisible")]
	public bool? IsVisible { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("genericTypeParameters")]
	public List<SystemType>? GenericTypeParameters { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaredConstructors")]
	public List<SystemReflectionConstructorInfo>? DeclaredConstructors { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaredEvents")]
	public List<SystemReflectionEventInfo>? DeclaredEvents { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaredFields")]
	public List<SystemReflectionFieldInfo>? DeclaredFields { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaredMembers")]
	public List<SystemReflectionMemberInfo>? DeclaredMembers { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaredMethods")]
	public List<SystemReflectionMethodInfo>? DeclaredMethods { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaredNestedTypes")]
	public List<SystemReflectionTypeInfo>? DeclaredNestedTypes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaredProperties")]
	public List<SystemReflectionPropertyInfo>? DeclaredProperties { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("implementedInterfaces")]
	public List<SystemType>? ImplementedInterfaces { get; set; }
}
public enum SystemRuntimeInteropServicesLayoutKind
{
	Sequential = 0,
	Explicit = 2,
	Auto = 3,
}
/// <summary>
/// 
/// </summary>
public class SystemRuntimeInteropServicesStructLayoutAttribute
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("typeId")]
	public JsonElement? TypeId { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("value")]
	public SystemRuntimeInteropServicesLayoutKind? Value { get; set; }
}
/// <summary>
/// 
/// </summary>
public class SystemRuntimeFieldHandle
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("value")]
	public SystemIntPtr? Value { get; set; }
}
/// <summary>
/// 
/// </summary>
public class SystemRuntimeMethodHandle
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("value")]
	public SystemIntPtr? Value { get; set; }
}
/// <summary>
/// 
/// </summary>
public class SystemRuntimeTypeHandle
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("value")]
	public SystemIntPtr? Value { get; set; }
}
public enum SystemSecuritySecurityRuleSet
{
	None = 0,
	Level1 = 1,
	Level2 = 2,
}
/// <summary>
/// 
/// </summary>
public class SystemType
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("customAttributes")]
	public List<SystemReflectionCustomAttributeData>? CustomAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCollectible")]
	public bool? IsCollectible { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("metadataToken")]
	public int? MetadataToken { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isInterface")]
	public bool? IsInterface { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("memberType")]
	public SystemReflectionMemberTypes? MemberType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("namespace")]
	public string? Namespace { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("assemblyQualifiedName")]
	public string? AssemblyQualifiedName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("fullName")]
	public string? FullName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("assembly")]
	public SystemReflectionAssembly? Assembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("module")]
	public SystemReflectionModule? Module { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNested")]
	public bool? IsNested { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaringType")]
	public SystemType? DeclaringType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("declaringMethod")]
	public SystemReflectionMethodBase? DeclaringMethod { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("reflectedType")]
	public SystemType? ReflectedType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("underlyingSystemType")]
	public SystemType? UnderlyingSystemType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isTypeDefinition")]
	public bool? IsTypeDefinition { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isArray")]
	public bool? IsArray { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isByRef")]
	public bool? IsByRef { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPointer")]
	public bool? IsPointer { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isConstructedGenericType")]
	public bool? IsConstructedGenericType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericParameter")]
	public bool? IsGenericParameter { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericTypeParameter")]
	public bool? IsGenericTypeParameter { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericMethodParameter")]
	public bool? IsGenericMethodParameter { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericType")]
	public bool? IsGenericType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isGenericTypeDefinition")]
	public bool? IsGenericTypeDefinition { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSZArray")]
	public bool? IsSZArray { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isVariableBoundArray")]
	public bool? IsVariableBoundArray { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isByRefLike")]
	public bool? IsByRefLike { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isFunctionPointer")]
	public bool? IsFunctionPointer { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isUnmanagedFunctionPointer")]
	public bool? IsUnmanagedFunctionPointer { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("hasElementType")]
	public bool? HasElementType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("genericTypeArguments")]
	public List<SystemType>? GenericTypeArguments { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("genericParameterPosition")]
	public int? GenericParameterPosition { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("genericParameterAttributes")]
	public SystemReflectionGenericParameterAttributes? GenericParameterAttributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("attributes")]
	public SystemReflectionTypeAttributes? Attributes { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAbstract")]
	public bool? IsAbstract { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isImport")]
	public bool? IsImport { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSealed")]
	public bool? IsSealed { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSpecialName")]
	public bool? IsSpecialName { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isClass")]
	public bool? IsClass { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedAssembly")]
	public bool? IsNestedAssembly { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedFamANDAssem")]
	public bool? IsNestedFamAndAssem { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedFamily")]
	public bool? IsNestedFamily { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedFamORAssem")]
	public bool? IsNestedFamORAssem { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedPrivate")]
	public bool? IsNestedPrivate { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNestedPublic")]
	public bool? IsNestedPublic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isNotPublic")]
	public bool? IsNotPublic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPublic")]
	public bool? IsPublic { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAutoLayout")]
	public bool? IsAutoLayout { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isExplicitLayout")]
	public bool? IsExplicitLayout { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isLayoutSequential")]
	public bool? IsLayoutSequential { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAnsiClass")]
	public bool? IsAnsiClass { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isAutoClass")]
	public bool? IsAutoClass { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isUnicodeClass")]
	public bool? IsUnicodeClass { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isCOMObject")]
	public bool? IsComObject { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isContextful")]
	public bool? IsContextful { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isEnum")]
	public bool? IsEnum { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isMarshalByRef")]
	public bool? IsMarshalByRef { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isPrimitive")]
	public bool? IsPrimitive { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isValueType")]
	public bool? IsValueType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSignatureType")]
	public bool? IsSignatureType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityCritical")]
	public bool? IsSecurityCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecuritySafeCritical")]
	public bool? IsSecuritySafeCritical { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSecurityTransparent")]
	public bool? IsSecurityTransparent { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("structLayoutAttribute")]
	public SystemRuntimeInteropServicesStructLayoutAttribute? StructLayoutAttribute { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("typeInitializer")]
	public SystemReflectionConstructorInfo? TypeInitializer { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("typeHandle")]
	public SystemRuntimeTypeHandle? TypeHandle { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("guid")]
	public Guid? Guid { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("baseType")]
	public SystemType? BaseType { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isSerializable")]
	public bool? IsSerializable { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("containsGenericParameters")]
	public bool? ContainsGenericParameters { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("isVisible")]
	public bool? IsVisible { get; set; }
}
/// <summary>
/// 任务信息
/// </summary>
public class TaskServiceControllerCommandControllerCommandInfo
{
	/// <summary>
	/// 程序名称
	/// </summary>
	[JsonPropertyName("fileName")]
	public string? FileName { get; set; }
	/// <summary>
	/// 工作目录
	/// </summary>
	[JsonPropertyName("workingDirectory")]
	public string? WorkingDirectory { get; set; }
	/// <summary>
	/// 命令行
	/// </summary>
	[JsonPropertyName("commands")]
	public List<string>? Commands { get; set; }
}
/// <summary>
/// 命令输出
/// </summary>
public class TaskServiceControllerCommandControllerCommandOutPut
{
	/// <summary>
	/// 输出
	/// </summary>
	[JsonPropertyName("standardOutput")]
	public string? StandardOutput { get; set; }
	/// <summary>
	/// 错误
	/// </summary>
	[JsonPropertyName("standardError")]
	public string? StandardError { get; set; }
}
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
