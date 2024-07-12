using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Controllers.Commons;

/// <summary>
/// disable model binding and skip read form
/// </summary>
/// <remarks>If you want to enter the action without reading the body, you need to have only one [BindNever][ValidateNever] IFormFile parameter, and the rest of the parameters must be FromQuery or FromHeader.</remarks>
[AttributeUsage(AttributeTargets.Method)]
public class DisableFormBindingAttribute : Attribute, IResourceFilter
{
	/// <inheritdoc/>
	public void OnResourceExecuted(ResourceExecutedContext context)
	{
	}
	/// <inheritdoc/>
	public void OnResourceExecuting(ResourceExecutingContext context)
	{
		var formFileValueProviderFactory = context.ValueProviderFactories.OfType<FormFileValueProviderFactory>().FirstOrDefault();
		if (formFileValueProviderFactory != null) context.ValueProviderFactories.Remove(formFileValueProviderFactory);
		var formValueProviderFactory = context.ValueProviderFactories.OfType<FormValueProviderFactory>().FirstOrDefault();
		if (formValueProviderFactory != null) context.ValueProviderFactories.Remove(formValueProviderFactory);
		var jQueryFormValueProviderFactory = context.ValueProviderFactories.OfType<JQueryFormValueProviderFactory>().FirstOrDefault();
		if (jQueryFormValueProviderFactory != null) context.ValueProviderFactories.Remove(jQueryFormValueProviderFactory);
	}
}