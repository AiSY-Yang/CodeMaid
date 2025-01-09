using System.Net;

namespace ServicesModels.Results
{
	/// <summary>
	/// An exception model that follows the RFC 9457 standard
	/// </summary>
	public class RFC9457
	{
		static Uri Blank = new Uri("about:blank");
		/// <summary>
		/// <a herf="https://www.rfc-editor.org/rfc/rfc9457#name-type"/>
		/// </summary>
		public Uri Type { get; set; } = Blank;
		/// <summary>
		/// <a herf="https://www.rfc-editor.org/rfc/rfc9457#name-status"/>
		/// </summary>
		public HttpStatusCode Status { get; set; }=HttpStatusCode.InternalServerError;
		/// <summary>
		/// <a herf="https://www.rfc-editor.org/rfc/rfc9457#name-title"/>
		/// </summary>
		public required string Title { get; set; }
		/// <summary>
		/// <a herf="https://www.rfc-editor.org/rfc/rfc9457#name-detail"/>
		/// </summary>
		public required string Detail { get; set; }
		/// <summary>
		/// <a herf="https://www.rfc-editor.org/rfc/rfc9457#name-instance"/>
		/// </summary>
		public string Instance => System.Diagnostics.Activity.Current?.TraceId.ToString() + System.Diagnostics.Activity.Current?.SpanId.ToString();
	}
}