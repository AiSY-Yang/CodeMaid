namespace Microsoft.AspNetCore.Mvc.ModelBinding.Validation
{
	/// <summary>
	/// 验证枚举是否是有效的值
	/// </summary>
	public class EnumValueValidatorProvider : IModelValidatorProvider
	{
		/// <inheritdoc/>
		public void CreateValidators(ModelValidatorProviderContext context)
		{
			if (context.ModelMetadata.IsEnum)
			{
				context.Results.Add(new ValidatorItem() { Validator = validator });
			}
		}
		static readonly EnumValueModelValidator validator = new EnumValueModelValidator();
	}
	/// <summary>
	/// 验证枚举是否是有效的值
	/// </summary>
	class EnumValueModelValidator : IModelValidator
	{
		/// <inheritdoc/>
		public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
		{
			if (context.Model != null && context.ModelMetadata.EnumNamesAndValues != null && !context.ModelMetadata.EnumNamesAndValues.ContainsKey(context.Model!.ToString()!))
			{
				yield return new ModelValidationResult(context.ModelMetadata.Name, $"字段{context.ModelMetadata.Name}是个枚举,传入的值不在有效范围内");
			}
		}
	}
}