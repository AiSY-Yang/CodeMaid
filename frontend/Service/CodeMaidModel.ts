/** action信息 */
interface ApiControllersCommonsActionInfo {
	/** action名称 */
	name: string | undefined,
	/** 参数列表 */
	paramList: ApiControllersCommonsParameterInfo[],
}
/** 控制器信息 */
interface ApiControllersCommonsControllerInfo {
	/** 控制器名称 */
	name: string | undefined,
	/** action列表 */
	actionList: ApiControllersCommonsActionInfo[],
}
/** 参数信息 */
interface ApiControllersCommonsParameterInfo {
	/** 参数名称 */
	name: string | undefined,
	/**  */
	'type': SystemType,
}
/** 文件 */
interface ApiControllersCommonsSystemFile {
	/** 名称 */
	name: string | undefined,
	/** 完整路径 */
	path: string | undefined,
}
/** 文件夹 */
interface ApiControllersCommonsSystemFolder {
	/** 名称 */
	name: string | undefined,
	/** 完整路径 */
	path: string | undefined,
	/** 子文件夹 */
	systemFolders: ApiControllersCommonsSystemFolder[],
	/** 文件 */
	systemFiles: ApiControllersCommonsSystemFile[],
}
/**  */
interface MicrosoftAspNetCoreMvcProblemDetails {
	/**  */
	'type': string| null | undefined,
	/**  */
	title: string| null | undefined,
	/**  */
	status: number| null | undefined,
	/**  */
	detail: string| null | undefined,
	/**  */
	instance: string| null | undefined,
}
/** 属性定义 */
interface ModelsCodeMaidAttributeDefinition {
	/** 唯一ID1 */
	id: number | undefined,
	/** 创建时间 */
	createTime: string | undefined,
	/** 更新时间 */
	updateTime: string| null | undefined,
	/** 是否有效 */
	isDeleted: boolean | undefined,
	/** Attribute名称 */
	name: string | undefined,
	/** Attribute文本 */
	text: string | undefined,
	/** 参数文本 */
	argumentsText: string| null | undefined,
	/** 参数 */
	arguments: string| null | undefined,
	/** 类定义 */
	propertyDefinition: ModelsCodeMaidPropertyDefinition,
}
/** 类定义 */
interface ModelsCodeMaidClassDefinition {
	/** 唯一ID1 */
	id: number | undefined,
	/** 创建时间 */
	createTime: string | undefined,
	/** 更新时间 */
	updateTime: string| null | undefined,
	/** 是否有效 */
	isDeleted: boolean | undefined,
	/** 命名空间 */
	nameSpace: string| null | undefined,
	/** 修饰符 */
	modifiers: string| null | undefined,
	/** 是否是抽象类 */
	isAbstract: boolean | undefined,
	/** 类名 */
	name: string | undefined,
	/** 注释 */
	summary: string| null | undefined,
	/** 基类或者接口名称 */
	base: string| null | undefined,
	/** 类引用的命名空间 */
	using: string| null | undefined,
	/** 前导 */
	leadingTrivia: string| null | undefined,
	/** 成员类型 */
	memberType: ModelsCodeMaidMemberType,
	/** 项目定义 */
	project: ModelsCodeMaidProject,
	/** 所属项目 */
	projectId: number| null | undefined,
	/** 属性列表 */
	properties: ModelsCodeMaidPropertyDefinition[],
}
/** 枚举定义 */
interface ModelsCodeMaidEnumDefinition {
	/** 唯一ID1 */
	id: number | undefined,
	/** 创建时间 */
	createTime: string | undefined,
	/** 更新时间 */
	updateTime: string| null | undefined,
	/** 是否有效 */
	isDeleted: boolean | undefined,
	/** 项目文件 */
	projectDirectoryFile: ModelsCodeMaidProjectDirectoryFile,
	/**  */
	projectDirectoryFileId: number | undefined,
	/** 命名空间 */
	nameSpace: string| null | undefined,
	/** 枚举名 */
	name: string | undefined,
	/** 注释 */
	summary: string| null | undefined,
	/** 备注 */
	remark: string | undefined,
	/** 前导 */
	leadingTrivia: string| null | undefined,
	/** 成员列表 */
	enumMembers: ModelsCodeMaidEnumMemberDefinition[],
}
/** 枚举成员定义 */
interface ModelsCodeMaidEnumMemberDefinition {
	/** 唯一ID1 */
	id: number | undefined,
	/** 创建时间 */
	createTime: string | undefined,
	/** 更新时间 */
	updateTime: string| null | undefined,
	/** 是否有效 */
	isDeleted: boolean | undefined,
	/** 枚举名称 */
	name: string | undefined,
	/** 枚举值 */
	value: number | undefined,
	/** 注释 */
	summary: string| null | undefined,
	/** 描述 */
	description: string| null | undefined,
}
/** 文件类型 */
enum ModelsCodeMaidFileType {
	other= 0,
	cSahrp= 1,
}
/** 功能 */
interface ModelsCodeMaidMaid {
	/** 唯一ID1 */
	id: number | undefined,
	/** 创建时间 */
	createTime: string | undefined,
	/** 更新时间 */
	updateTime: string| null | undefined,
	/** 是否有效 */
	isDeleted: boolean | undefined,
	/** 名称 */
	name: string | undefined,
	/** 项目定义 */
	project: ModelsCodeMaidProject,
	/**  */
	projectId: number | undefined,
	/** 功能 */
	maidWork: ModelsCodeMaidMaidWork,
	/** 原路径 */
	sourcePath: string | undefined,
	/** 目标路径 */
	destinationPath: string | undefined,
	/** 是否自动修复 */
	autonomous: boolean | undefined,
	/** 设置 */
	setting: Blob | undefined,
}
/** 功能 */
enum ModelsCodeMaidMaidWork {
	configurationSync= 0,
	dtoSync= 1,
	httpClientSync= 2,
	controllerSync= 3,
	masstransitConsumerSync= 4,
}
/** 成员类型 */
enum ModelsCodeMaidMemberType {
	classDeclarationSyntax= 0,
	interfaceDeclarationSyntax= 1,
	recordDeclarationSyntax= 2,
	structDeclarationSyntax= 3,
}
/** 项目定义 */
interface ModelsCodeMaidProject {
	/** 唯一ID1 */
	id: number | undefined,
	/** 创建时间 */
	createTime: string | undefined,
	/** 更新时间 */
	updateTime: string| null | undefined,
	/** 是否有效 */
	isDeleted: boolean | undefined,
	/** 项目名 */
	name: string | undefined,
	/** 项目路径 */
	path: string | undefined,
	/** Git分支 */
	gitBranch: string | undefined,
	/** 是否添加枚举的remark信息 */
	addEnumRemark: boolean | undefined,
	/** maid集合 */
	maids: ModelsCodeMaidMaid[],
	/** 项目目录 */
	projectDirectories: ModelsCodeMaidProjectDirectory[],
	/**  */
	enumDefinitions: ModelsCodeMaidEnumDefinition[],
	/**  */
	classDefinitions: ModelsCodeMaidClassDefinition[],
}
/** 项目目录 */
interface ModelsCodeMaidProjectDirectory {
	/** 唯一ID1 */
	id: number | undefined,
	/** 创建时间 */
	createTime: string | undefined,
	/** 更新时间 */
	updateTime: string| null | undefined,
	/** 是否有效 */
	isDeleted: boolean | undefined,
	/** 目录名 */
	name: string | undefined,
	/** 路径 */
	path: string | undefined,
	/** 项目定义 */
	project: ModelsCodeMaidProject,
	/** 项目Id */
	projectId: number | undefined,
	/** 目录 */
	projectDirectoryFiles: ModelsCodeMaidProjectDirectoryFile[],
}
/** 项目文件 */
interface ModelsCodeMaidProjectDirectoryFile {
	/** 唯一ID1 */
	id: number | undefined,
	/** 创建时间 */
	createTime: string | undefined,
	/** 更新时间 */
	updateTime: string| null | undefined,
	/** 是否有效 */
	isDeleted: boolean | undefined,
	/** 项目定义 */
	project: ModelsCodeMaidProject,
	/** 项目Id */
	projectId: number | undefined,
	/** 文件名 */
	name: string | undefined,
	/** 文件最后修改时间 */
	lastWriteTime: string | undefined,
	/** 路径 */
	path: string | undefined,
	/** 项目目录 */
	projectDirectory: ModelsCodeMaidProjectDirectory,
	/** 是否是自动生成的文件 */
	isAutoGen: boolean | undefined,
	/** 总行数 */
	linesCount: number | undefined,
	/** 空行数 */
	spaceCount: number | undefined,
	/** 注释行数 */
	commentCount: number | undefined,
	/** 文件类型 */
	fileType: ModelsCodeMaidFileType,
	/** 项目结构 */
	projectStructures: ModelsCodeMaidProjectStructure[],
	/** 关联枚举 */
	enumDefinitions: ModelsCodeMaidEnumDefinition[],
}
/** 项目结构 */
interface ModelsCodeMaidProjectStructure {
	/** 唯一ID1 */
	id: number | undefined,
	/** 创建时间 */
	createTime: string | undefined,
	/** 更新时间 */
	updateTime: string| null | undefined,
	/** 是否有效 */
	isDeleted: boolean | undefined,
	/** 项目文件 */
	projectDirectoryFile: ModelsCodeMaidProjectDirectoryFile,
	/** 类定义 */
	classDefinition: ModelsCodeMaidClassDefinition,
	/** 属性 */
	propertyDefinitions: ModelsCodeMaidPropertyDefinition[],
}
/** 类定义 */
interface ModelsCodeMaidPropertyDefinition {
	/** 唯一ID1 */
	id: number | undefined,
	/** 创建时间 */
	createTime: string | undefined,
	/** 更新时间 */
	updateTime: string| null | undefined,
	/** 是否有效 */
	isDeleted: boolean | undefined,
	/** 类定义 */
	classDefinition: ModelsCodeMaidClassDefinition,
	/** 所属类Id */
	classDefinitionId: number | undefined,
	/** 前导 */
	leadingTrivia: string| null | undefined,
	/** 注释 */
	summary: string| null | undefined,
	/** 备注 */
	remark: string| null | undefined,
	/** 完整文本内容 */
	fullText: string | undefined,
	/** 修饰符 */
	modifiers: string | undefined,
	/** 初始化器 */
	initializer: string| null | undefined,
	/** 属性名称 */
	name: string | undefined,
	/** 数据类型 */
	'type': string | undefined,
	/** 是否是枚举 */
	isEnum: boolean | undefined,
	/** 是否包含Get */
	hasGet: boolean | undefined,
	/** Get方法体 */
	get: string| null | undefined,
	/** 是否包含Set */
	hasSet: boolean | undefined,
	/** Set方法体 */
	set: string| null | undefined,
	/** 枚举定义 */
	enumDefinition: ModelsCodeMaidEnumDefinition,
	/** 属性列表 */
	attributes: ModelsCodeMaidAttributeDefinition[],
	/** 项目文件 */
	projectDirectoryFile: ModelsCodeMaidProjectDirectoryFile,
}
/** 业务异常返回的结果模型 */
interface ServicesModelsResultsIExceptionResult {
	/** 错误码 */
	code: string | undefined,
	/** 异常消息 */
	msg: string| null | undefined,
}
/**  */
interface SystemIntPtr {
}
/**  */
interface SystemModuleHandle {
	/**  */
	mdStreamVersion: number | undefined,
}
/**  */
interface SystemReflectionAssembly {
	/**  */
	definedTypes: SystemReflectionTypeInfo[],
	/**  */
	exportedTypes: SystemType[],
	/**  */
	codeBase: string| null | undefined,
	/**  */
	entryPoint: SystemReflectionMethodInfo,
	/**  */
	fullName: string| null | undefined,
	/**  */
	imageRuntimeVersion: string | undefined,
	/**  */
	isDynamic: boolean | undefined,
	/**  */
	location: string | undefined,
	/**  */
	reflectionOnly: boolean | undefined,
	/**  */
	isCollectible: boolean | undefined,
	/**  */
	isFullyTrusted: boolean | undefined,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	escapedCodeBase: string | undefined,
	/**  */
	manifestModule: SystemReflectionModule,
	/**  */
	modules: SystemReflectionModule[],
	/**  */
	globalAssemblyCache: boolean | undefined,
	/**  */
	hostContext: number | undefined,
	/**  */
	securityRuleSet: SystemSecuritySecurityRuleSet,
}
/**  */
enum SystemReflectionCallingConventions {
	standard= 1,
	varArgs= 2,
	any= 3,
	hasThis= 32,
	explicitThis= 64,
}
/**  */
interface SystemReflectionConstructorInfo {
	/**  */
	name: string | undefined,
	/**  */
	declaringType: SystemType,
	/**  */
	reflectedType: SystemType,
	/**  */
	'module': SystemReflectionModule,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	isCollectible: boolean | undefined,
	/**  */
	metadataToken: number | undefined,
	/**  */
	attributes: SystemReflectionMethodAttributes,
	/**  */
	methodImplementationFlags: SystemReflectionMethodImplAttributes,
	/**  */
	callingConvention: SystemReflectionCallingConventions,
	/**  */
	isAbstract: boolean | undefined,
	/**  */
	isConstructor: boolean | undefined,
	/**  */
	isFinal: boolean | undefined,
	/**  */
	isHideBySig: boolean | undefined,
	/**  */
	isSpecialName: boolean | undefined,
	/**  */
	isStatic: boolean | undefined,
	/**  */
	isVirtual: boolean | undefined,
	/**  */
	isAssembly: boolean | undefined,
	/**  */
	isFamily: boolean | undefined,
	/**  */
	isFamilyAndAssembly: boolean | undefined,
	/**  */
	isFamilyOrAssembly: boolean | undefined,
	/**  */
	isPrivate: boolean | undefined,
	/**  */
	isPublic: boolean | undefined,
	/**  */
	isConstructedGenericMethod: boolean | undefined,
	/**  */
	isGenericMethod: boolean | undefined,
	/**  */
	isGenericMethodDefinition: boolean | undefined,
	/**  */
	containsGenericParameters: boolean | undefined,
	/**  */
	methodHandle: SystemRuntimeMethodHandle,
	/**  */
	isSecurityCritical: boolean | undefined,
	/**  */
	isSecuritySafeCritical: boolean | undefined,
	/**  */
	isSecurityTransparent: boolean | undefined,
	/**  */
	memberType: SystemReflectionMemberTypes,
}
/**  */
interface SystemReflectionCustomAttributeData {
	/**  */
	attributeType: SystemType,
	/**  */
	'constructor': SystemReflectionConstructorInfo,
	/**  */
	constructorArguments: SystemReflectionCustomAttributeTypedArgument[],
	/**  */
	namedArguments: SystemReflectionCustomAttributeNamedArgument[],
}
/**  */
interface SystemReflectionCustomAttributeNamedArgument {
	/**  */
	memberInfo: SystemReflectionMemberInfo,
	/**  */
	typedValue: SystemReflectionCustomAttributeTypedArgument,
	/**  */
	memberName: string | undefined,
	/**  */
	isField: boolean | undefined,
}
/**  */
interface SystemReflectionCustomAttributeTypedArgument {
	/**  */
	argumentType: SystemType,
	/**  */
	value: Blob| null | undefined,
}
/**  */
enum SystemReflectionEventAttributes {
	none= 0,
	specialName= 512,
	rtSpecialName= 1024,
}
/**  */
interface SystemReflectionEventInfo {
	/**  */
	name: string | undefined,
	/**  */
	declaringType: SystemType,
	/**  */
	reflectedType: SystemType,
	/**  */
	'module': SystemReflectionModule,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	isCollectible: boolean | undefined,
	/**  */
	metadataToken: number | undefined,
	/**  */
	memberType: SystemReflectionMemberTypes,
	/**  */
	attributes: SystemReflectionEventAttributes,
	/**  */
	isSpecialName: boolean | undefined,
	/**  */
	addMethod: SystemReflectionMethodInfo,
	/**  */
	removeMethod: SystemReflectionMethodInfo,
	/**  */
	raiseMethod: SystemReflectionMethodInfo,
	/**  */
	isMulticast: boolean | undefined,
	/**  */
	eventHandlerType: SystemType,
}
/**  */
enum SystemReflectionFieldAttributes {
	privateScope= 0,
	private= 1,
	famAndAssem= 2,
	assembly= 3,
	family= 4,
	famOrAssem= 5,
	public= 6,
	fieldAccessMask= 7,
	static= 16,
	initOnly= 32,
	literal= 64,
	notSerialized= 128,
	hasFieldRva= 256,
	specialName= 512,
	rtSpecialName= 1024,
	hasFieldMarshal= 4096,
	pinvokeImpl= 8192,
	hasDefault= 32768,
	reservedMask= 38144,
}
/**  */
interface SystemReflectionFieldInfo {
	/**  */
	name: string | undefined,
	/**  */
	declaringType: SystemType,
	/**  */
	reflectedType: SystemType,
	/**  */
	'module': SystemReflectionModule,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	isCollectible: boolean | undefined,
	/**  */
	metadataToken: number | undefined,
	/**  */
	memberType: SystemReflectionMemberTypes,
	/**  */
	attributes: SystemReflectionFieldAttributes,
	/**  */
	fieldType: SystemType,
	/**  */
	isInitOnly: boolean | undefined,
	/**  */
	isLiteral: boolean | undefined,
	/**  */
	isNotSerialized: boolean | undefined,
	/**  */
	isPinvokeImpl: boolean | undefined,
	/**  */
	isSpecialName: boolean | undefined,
	/**  */
	isStatic: boolean | undefined,
	/**  */
	isAssembly: boolean | undefined,
	/**  */
	isFamily: boolean | undefined,
	/**  */
	isFamilyAndAssembly: boolean | undefined,
	/**  */
	isFamilyOrAssembly: boolean | undefined,
	/**  */
	isPrivate: boolean | undefined,
	/**  */
	isPublic: boolean | undefined,
	/**  */
	isSecurityCritical: boolean | undefined,
	/**  */
	isSecuritySafeCritical: boolean | undefined,
	/**  */
	isSecurityTransparent: boolean | undefined,
	/**  */
	fieldHandle: SystemRuntimeFieldHandle,
}
/**  */
enum SystemReflectionGenericParameterAttributes {
	none= 0,
	covariant= 1,
	contravariant= 2,
	varianceMask= 3,
	referenceTypeConstraint= 4,
	notNullableValueTypeConstraint= 8,
	defaultConstructorConstraint= 16,
	specialConstraintMask= 28,
}
/**  */
interface SystemReflectionICustomAttributeProvider {
}
/**  */
interface SystemReflectionMemberInfo {
	/**  */
	memberType: SystemReflectionMemberTypes,
	/**  */
	name: string | undefined,
	/**  */
	declaringType: SystemType,
	/**  */
	reflectedType: SystemType,
	/**  */
	'module': SystemReflectionModule,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	isCollectible: boolean | undefined,
	/**  */
	metadataToken: number | undefined,
}
/**  */
enum SystemReflectionMemberTypes {
	constructor= 1,
	event= 2,
	field= 4,
	method= 8,
	property= 16,
	typeInfo= 32,
	custom= 64,
	nestedType= 128,
	all= 191,
}
/**  */
enum SystemReflectionMethodAttributes {
	privateScope= 0,
	reuseSlot= 1,
	private= 2,
	famAndAssem= 3,
	assembly= 4,
	family= 5,
	famOrAssem= 6,
	public= 7,
	memberAccessMask= 8,
	unmanagedExport= 16,
	static= 32,
	final= 64,
	virtual= 128,
	hideBySig= 256,
	newSlot= 512,
	vtableLayoutMask= 1024,
	checkAccessOnOverride= 2048,
	abstract= 4096,
	specialName= 8192,
	rtSpecialName= 16384,
	pinvokeImpl= 32768,
	hasSecurity= 53248,
}
/**  */
interface SystemReflectionMethodBase {
	/**  */
	memberType: SystemReflectionMemberTypes,
	/**  */
	name: string | undefined,
	/**  */
	declaringType: SystemType,
	/**  */
	reflectedType: SystemType,
	/**  */
	'module': SystemReflectionModule,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	isCollectible: boolean | undefined,
	/**  */
	metadataToken: number | undefined,
	/**  */
	attributes: SystemReflectionMethodAttributes,
	/**  */
	methodImplementationFlags: SystemReflectionMethodImplAttributes,
	/**  */
	callingConvention: SystemReflectionCallingConventions,
	/**  */
	isAbstract: boolean | undefined,
	/**  */
	isConstructor: boolean | undefined,
	/**  */
	isFinal: boolean | undefined,
	/**  */
	isHideBySig: boolean | undefined,
	/**  */
	isSpecialName: boolean | undefined,
	/**  */
	isStatic: boolean | undefined,
	/**  */
	isVirtual: boolean | undefined,
	/**  */
	isAssembly: boolean | undefined,
	/**  */
	isFamily: boolean | undefined,
	/**  */
	isFamilyAndAssembly: boolean | undefined,
	/**  */
	isFamilyOrAssembly: boolean | undefined,
	/**  */
	isPrivate: boolean | undefined,
	/**  */
	isPublic: boolean | undefined,
	/**  */
	isConstructedGenericMethod: boolean | undefined,
	/**  */
	isGenericMethod: boolean | undefined,
	/**  */
	isGenericMethodDefinition: boolean | undefined,
	/**  */
	containsGenericParameters: boolean | undefined,
	/**  */
	methodHandle: SystemRuntimeMethodHandle,
	/**  */
	isSecurityCritical: boolean | undefined,
	/**  */
	isSecuritySafeCritical: boolean | undefined,
	/**  */
	isSecurityTransparent: boolean | undefined,
}
/**  */
enum SystemReflectionMethodImplAttributes {
	il= 0,
	managed= 1,
	native= 2,
	optil= 3,
	codeTypeMask= 4,
	runtime= 8,
	managedMask= 16,
	unmanaged= 32,
	noInlining= 64,
	forwardRef= 128,
	synchronized= 256,
	noOptimization= 512,
	preserveSig= 4096,
	aggressiveInlining= 65535,
}
/**  */
interface SystemReflectionMethodInfo {
	/**  */
	name: string | undefined,
	/**  */
	declaringType: SystemType,
	/**  */
	reflectedType: SystemType,
	/**  */
	'module': SystemReflectionModule,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	isCollectible: boolean | undefined,
	/**  */
	metadataToken: number | undefined,
	/**  */
	attributes: SystemReflectionMethodAttributes,
	/**  */
	methodImplementationFlags: SystemReflectionMethodImplAttributes,
	/**  */
	callingConvention: SystemReflectionCallingConventions,
	/**  */
	isAbstract: boolean | undefined,
	/**  */
	isConstructor: boolean | undefined,
	/**  */
	isFinal: boolean | undefined,
	/**  */
	isHideBySig: boolean | undefined,
	/**  */
	isSpecialName: boolean | undefined,
	/**  */
	isStatic: boolean | undefined,
	/**  */
	isVirtual: boolean | undefined,
	/**  */
	isAssembly: boolean | undefined,
	/**  */
	isFamily: boolean | undefined,
	/**  */
	isFamilyAndAssembly: boolean | undefined,
	/**  */
	isFamilyOrAssembly: boolean | undefined,
	/**  */
	isPrivate: boolean | undefined,
	/**  */
	isPublic: boolean | undefined,
	/**  */
	isConstructedGenericMethod: boolean | undefined,
	/**  */
	isGenericMethod: boolean | undefined,
	/**  */
	isGenericMethodDefinition: boolean | undefined,
	/**  */
	containsGenericParameters: boolean | undefined,
	/**  */
	methodHandle: SystemRuntimeMethodHandle,
	/**  */
	isSecurityCritical: boolean | undefined,
	/**  */
	isSecuritySafeCritical: boolean | undefined,
	/**  */
	isSecurityTransparent: boolean | undefined,
	/**  */
	memberType: SystemReflectionMemberTypes,
	/**  */
	returnParameter: SystemReflectionParameterInfo,
	/**  */
	returnType: SystemType,
	/**  */
	returnTypeCustomAttributes: SystemReflectionICustomAttributeProvider,
}
/**  */
interface SystemReflectionModule {
	/**  */
	assembly: SystemReflectionAssembly,
	/**  */
	fullyQualifiedName: string | undefined,
	/**  */
	name: string | undefined,
	/**  */
	mdStreamVersion: number | undefined,
	/**  */
	moduleVersionId: string | undefined,
	/**  */
	scopeName: string | undefined,
	/**  */
	moduleHandle: SystemModuleHandle,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	metadataToken: number | undefined,
}
/**  */
enum SystemReflectionParameterAttributes {
	none= 0,
	in= 1,
	out= 2,
	lcid= 4,
	retval= 8,
	optional= 16,
	hasDefault= 4096,
	hasFieldMarshal= 8192,
	reserved3= 16384,
	reserved4= 32768,
	reservedMask= 61440,
}
/**  */
interface SystemReflectionParameterInfo {
	/**  */
	attributes: SystemReflectionParameterAttributes,
	/**  */
	member: SystemReflectionMemberInfo,
	/**  */
	name: string| null | undefined,
	/**  */
	parameterType: SystemType,
	/**  */
	position: number | undefined,
	/**  */
	isIn: boolean | undefined,
	/**  */
	isLcid: boolean | undefined,
	/**  */
	isOptional: boolean | undefined,
	/**  */
	isOut: boolean | undefined,
	/**  */
	isRetval: boolean | undefined,
	/**  */
	defaultValue: Blob| null | undefined,
	/**  */
	rawDefaultValue: Blob| null | undefined,
	/**  */
	hasDefaultValue: boolean | undefined,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	metadataToken: number | undefined,
}
/**  */
enum SystemReflectionPropertyAttributes {
	none= 0,
	specialName= 512,
	rtSpecialName= 1024,
	hasDefault= 4096,
	reserved2= 8192,
	reserved3= 16384,
	reserved4= 32768,
	reservedMask= 62464,
}
/**  */
interface SystemReflectionPropertyInfo {
	/**  */
	name: string | undefined,
	/**  */
	declaringType: SystemType,
	/**  */
	reflectedType: SystemType,
	/**  */
	'module': SystemReflectionModule,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	isCollectible: boolean | undefined,
	/**  */
	metadataToken: number | undefined,
	/**  */
	memberType: SystemReflectionMemberTypes,
	/**  */
	propertyType: SystemType,
	/**  */
	attributes: SystemReflectionPropertyAttributes,
	/**  */
	isSpecialName: boolean | undefined,
	/**  */
	canRead: boolean | undefined,
	/**  */
	canWrite: boolean | undefined,
	/**  */
	getMethod: SystemReflectionMethodInfo,
	/**  */
	setMethod: SystemReflectionMethodInfo,
}
/**  */
enum SystemReflectionTypeAttributes {
	notPublic= 0,
	autoLayout= 1,
	ansiClass= 2,
	class= 3,
	public= 4,
	nestedPublic= 5,
	nestedPrivate= 6,
	nestedFamily= 7,
	nestedAssembly= 8,
	nestedFamAndAssem= 16,
	visibilityMask= 24,
	nestedFamOrAssem= 32,
	sequentialLayout= 128,
	explicitLayout= 256,
	layoutMask= 1024,
	interface= 2048,
	classSemanticsMask= 4096,
	abstract= 8192,
	sealed= 16384,
	specialName= 65536,
	rtSpecialName= 131072,
	import= 196608,
	serializable= 262144,
	windowsRuntime= 264192,
	unicodeClass= 1048576,
	autoClass= 12582912,
}
/**  */
interface SystemReflectionTypeInfo {
	/**  */
	name: string | undefined,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	isCollectible: boolean | undefined,
	/**  */
	metadataToken: number | undefined,
	/**  */
	isInterface: boolean | undefined,
	/**  */
	memberType: SystemReflectionMemberTypes,
	/**  */
	'namespace': string| null | undefined,
	/**  */
	assemblyQualifiedName: string| null | undefined,
	/**  */
	fullName: string| null | undefined,
	/**  */
	assembly: SystemReflectionAssembly,
	/**  */
	'module': SystemReflectionModule,
	/**  */
	isNested: boolean | undefined,
	/**  */
	declaringType: SystemType,
	/**  */
	declaringMethod: SystemReflectionMethodBase,
	/**  */
	reflectedType: SystemType,
	/**  */
	underlyingSystemType: SystemType,
	/**  */
	isTypeDefinition: boolean | undefined,
	/**  */
	isArray: boolean | undefined,
	/**  */
	isByRef: boolean | undefined,
	/**  */
	isPointer: boolean | undefined,
	/**  */
	isConstructedGenericType: boolean | undefined,
	/**  */
	isGenericParameter: boolean | undefined,
	/**  */
	isGenericTypeParameter: boolean | undefined,
	/**  */
	isGenericMethodParameter: boolean | undefined,
	/**  */
	isGenericType: boolean | undefined,
	/**  */
	isGenericTypeDefinition: boolean | undefined,
	/**  */
	isSzArray: boolean | undefined,
	/**  */
	isVariableBoundArray: boolean | undefined,
	/**  */
	isByRefLike: boolean | undefined,
	/**  */
	isFunctionPointer: boolean | undefined,
	/**  */
	isUnmanagedFunctionPointer: boolean | undefined,
	/**  */
	hasElementType: boolean | undefined,
	/**  */
	genericTypeArguments: SystemType[],
	/**  */
	genericParameterPosition: number | undefined,
	/**  */
	genericParameterAttributes: SystemReflectionGenericParameterAttributes,
	/**  */
	attributes: SystemReflectionTypeAttributes,
	/**  */
	isAbstract: boolean | undefined,
	/**  */
	isImport: boolean | undefined,
	/**  */
	isSealed: boolean | undefined,
	/**  */
	isSpecialName: boolean | undefined,
	/**  */
	isClass: boolean | undefined,
	/**  */
	isNestedAssembly: boolean | undefined,
	/**  */
	isNestedFamAndAssem: boolean | undefined,
	/**  */
	isNestedFamily: boolean | undefined,
	/**  */
	isNestedFamOrAssem: boolean | undefined,
	/**  */
	isNestedPrivate: boolean | undefined,
	/**  */
	isNestedPublic: boolean | undefined,
	/**  */
	isNotPublic: boolean | undefined,
	/**  */
	isPublic: boolean | undefined,
	/**  */
	isAutoLayout: boolean | undefined,
	/**  */
	isExplicitLayout: boolean | undefined,
	/**  */
	isLayoutSequential: boolean | undefined,
	/**  */
	isAnsiClass: boolean | undefined,
	/**  */
	isAutoClass: boolean | undefined,
	/**  */
	isUnicodeClass: boolean | undefined,
	/**  */
	isComObject: boolean | undefined,
	/**  */
	isContextful: boolean | undefined,
	/**  */
	isEnum: boolean | undefined,
	/**  */
	isMarshalByRef: boolean | undefined,
	/**  */
	isPrimitive: boolean | undefined,
	/**  */
	isValueType: boolean | undefined,
	/**  */
	isSignatureType: boolean | undefined,
	/**  */
	isSecurityCritical: boolean | undefined,
	/**  */
	isSecuritySafeCritical: boolean | undefined,
	/**  */
	isSecurityTransparent: boolean | undefined,
	/**  */
	structLayoutAttribute: SystemRuntimeInteropServicesStructLayoutAttribute,
	/**  */
	typeInitializer: SystemReflectionConstructorInfo,
	/**  */
	typeHandle: SystemRuntimeTypeHandle,
	/**  */
	guid: string | undefined,
	/**  */
	baseType: SystemType,
	/**  */
	isSerializable: boolean | undefined,
	/**  */
	containsGenericParameters: boolean | undefined,
	/**  */
	isVisible: boolean | undefined,
	/**  */
	genericTypeParameters: SystemType[],
	/**  */
	declaredConstructors: SystemReflectionConstructorInfo[],
	/**  */
	declaredEvents: SystemReflectionEventInfo[],
	/**  */
	declaredFields: SystemReflectionFieldInfo[],
	/**  */
	declaredMembers: SystemReflectionMemberInfo[],
	/**  */
	declaredMethods: SystemReflectionMethodInfo[],
	/**  */
	declaredNestedTypes: SystemReflectionTypeInfo[],
	/**  */
	declaredProperties: SystemReflectionPropertyInfo[],
	/**  */
	implementedInterfaces: SystemType[],
}
/**  */
enum SystemRuntimeInteropServicesLayoutKind {
	sequential= 0,
	explicit= 2,
	auto= 3,
}
/**  */
interface SystemRuntimeInteropServicesStructLayoutAttribute {
	/**  */
	typeId: Blob | undefined,
	/**  */
	value: SystemRuntimeInteropServicesLayoutKind,
}
/**  */
interface SystemRuntimeFieldHandle {
	/**  */
	value: SystemIntPtr,
}
/**  */
interface SystemRuntimeMethodHandle {
	/**  */
	value: SystemIntPtr,
}
/**  */
interface SystemRuntimeTypeHandle {
	/**  */
	value: SystemIntPtr,
}
/**  */
enum SystemSecuritySecurityRuleSet {
	none= 0,
	level1= 1,
	level2= 2,
}
/**  */
interface SystemType {
	/**  */
	name: string | undefined,
	/**  */
	customAttributes: SystemReflectionCustomAttributeData[],
	/**  */
	isCollectible: boolean | undefined,
	/**  */
	metadataToken: number | undefined,
	/**  */
	isInterface: boolean | undefined,
	/**  */
	memberType: SystemReflectionMemberTypes,
	/**  */
	'namespace': string| null | undefined,
	/**  */
	assemblyQualifiedName: string| null | undefined,
	/**  */
	fullName: string| null | undefined,
	/**  */
	assembly: SystemReflectionAssembly,
	/**  */
	'module': SystemReflectionModule,
	/**  */
	isNested: boolean | undefined,
	/**  */
	declaringType: SystemType,
	/**  */
	declaringMethod: SystemReflectionMethodBase,
	/**  */
	reflectedType: SystemType,
	/**  */
	underlyingSystemType: SystemType,
	/**  */
	isTypeDefinition: boolean | undefined,
	/**  */
	isArray: boolean | undefined,
	/**  */
	isByRef: boolean | undefined,
	/**  */
	isPointer: boolean | undefined,
	/**  */
	isConstructedGenericType: boolean | undefined,
	/**  */
	isGenericParameter: boolean | undefined,
	/**  */
	isGenericTypeParameter: boolean | undefined,
	/**  */
	isGenericMethodParameter: boolean | undefined,
	/**  */
	isGenericType: boolean | undefined,
	/**  */
	isGenericTypeDefinition: boolean | undefined,
	/**  */
	isSzArray: boolean | undefined,
	/**  */
	isVariableBoundArray: boolean | undefined,
	/**  */
	isByRefLike: boolean | undefined,
	/**  */
	isFunctionPointer: boolean | undefined,
	/**  */
	isUnmanagedFunctionPointer: boolean | undefined,
	/**  */
	hasElementType: boolean | undefined,
	/**  */
	genericTypeArguments: SystemType[],
	/**  */
	genericParameterPosition: number | undefined,
	/**  */
	genericParameterAttributes: SystemReflectionGenericParameterAttributes,
	/**  */
	attributes: SystemReflectionTypeAttributes,
	/**  */
	isAbstract: boolean | undefined,
	/**  */
	isImport: boolean | undefined,
	/**  */
	isSealed: boolean | undefined,
	/**  */
	isSpecialName: boolean | undefined,
	/**  */
	isClass: boolean | undefined,
	/**  */
	isNestedAssembly: boolean | undefined,
	/**  */
	isNestedFamAndAssem: boolean | undefined,
	/**  */
	isNestedFamily: boolean | undefined,
	/**  */
	isNestedFamOrAssem: boolean | undefined,
	/**  */
	isNestedPrivate: boolean | undefined,
	/**  */
	isNestedPublic: boolean | undefined,
	/**  */
	isNotPublic: boolean | undefined,
	/**  */
	isPublic: boolean | undefined,
	/**  */
	isAutoLayout: boolean | undefined,
	/**  */
	isExplicitLayout: boolean | undefined,
	/**  */
	isLayoutSequential: boolean | undefined,
	/**  */
	isAnsiClass: boolean | undefined,
	/**  */
	isAutoClass: boolean | undefined,
	/**  */
	isUnicodeClass: boolean | undefined,
	/**  */
	isComObject: boolean | undefined,
	/**  */
	isContextful: boolean | undefined,
	/**  */
	isEnum: boolean | undefined,
	/**  */
	isMarshalByRef: boolean | undefined,
	/**  */
	isPrimitive: boolean | undefined,
	/**  */
	isValueType: boolean | undefined,
	/**  */
	isSignatureType: boolean | undefined,
	/**  */
	isSecurityCritical: boolean | undefined,
	/**  */
	isSecuritySafeCritical: boolean | undefined,
	/**  */
	isSecurityTransparent: boolean | undefined,
	/**  */
	structLayoutAttribute: SystemRuntimeInteropServicesStructLayoutAttribute,
	/**  */
	typeInitializer: SystemReflectionConstructorInfo,
	/**  */
	typeHandle: SystemRuntimeTypeHandle,
	/**  */
	guid: string | undefined,
	/**  */
	baseType: SystemType,
	/**  */
	isSerializable: boolean | undefined,
	/**  */
	containsGenericParameters: boolean | undefined,
	/**  */
	isVisible: boolean | undefined,
}
/** 任务信息 */
interface TaskServiceControllerCommandControllerCommandInfo {
	/** 程序名称 */
	fileName: string | undefined,
	/** 工作目录 */
	workingDirectory: string| null | undefined,
	/** 命令行 */
	commands: string[],
}
/** 命令输出 */
interface TaskServiceControllerCommandControllerCommandOutPut {
	/** 输出 */
	standardOutput: string | undefined,
	/** 错误 */
	standardError: string | undefined,
}
