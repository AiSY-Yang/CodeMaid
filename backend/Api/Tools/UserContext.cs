using System.Security.Claims;

namespace Application.Implement;

/// <summary>
/// 用户信息
/// </summary>
public class UserContext
{
	/// <summary>
	/// 用户Id
	/// </summary>
	public Guid UserId { get; init; }
	/// <summary>
	/// 是否是管理员
	/// </summary>
	public bool IsAdmin { get; init; } = false;
	/// <summary>
	/// 权限
	/// </summary>
	public List<string> Roles { get; set; }
	/// <summary>
	/// 用户组Id
	/// </summary>
	public Guid GroupId { get; init; }
	/// <summary>
	/// 解析用户信息
	/// </summary>
	/// <param name="httpContextAccessor"></param>
	public UserContext(IHttpContextAccessor httpContextAccessor)
	{
		var context = httpContextAccessor.HttpContext!;
		if (Guid.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId) && userId != Guid.Empty)
		{
			UserId = userId;
		}
		if (Guid.TryParse(context.User.FindFirst(ClaimTypes.GroupSid)?.Value, out Guid groupSid) && groupSid != Guid.Empty)
		{
			GroupId = groupSid;
		}
		Roles = context.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
		IsAdmin = Roles.Contains("admin", StringComparer.OrdinalIgnoreCase);
	}
}
