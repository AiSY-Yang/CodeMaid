using Microsoft.AspNetCore.Authorization.Policy;

namespace Microsoft.AspNetCore.Authorization
{
	/// <inheritdoc/>
	public class SkipAuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
	{
		private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

		/// <inheritdoc/>
		public async Task HandleAsync(
			RequestDelegate next,
			HttpContext context,
			AuthorizationPolicy policy,
			PolicyAuthorizationResult authorizeResult)
		{

			authorizeResult = PolicyAuthorizationResult.Success();
			// Fall back to the default implementation.
			await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
		}
	}
}