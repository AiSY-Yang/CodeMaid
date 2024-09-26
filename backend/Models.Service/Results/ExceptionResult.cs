namespace ServicesModels.Results
{
	/// <summary>
	/// 业务异常返回的结果模型
	/// </summary>
	public interface IExceptionResult
	{
		/// <summary>
		/// 错误码
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 异常消息
		/// </summary>
		public string? Msg { get; set; }
	}

	/// <summary>
	/// 业务异常返回的结果模型
	/// </summary>
	public class ExceptionResult : IExceptionResult
	{
		/// <inheritdoc/>
		public ExceptionResult(string code, string? msg)
		{
			Code = code;
			Msg = msg;
		}
		/// <inheritdoc/>
		public ExceptionResult(ResultException resultException)
		{
			Code = resultException.Code;
			Msg = resultException.Msg;
		}

		/// <inheritdoc/>
		public string Code { get; set; }
		/// <inheritdoc/>
		public string? Msg { get; set; }
	}


	/// <summary>
	/// 业务异常返回的结果模型
	/// </summary>
	public class I18nExceptionResult : IExceptionResult
	{
		/// <inheritdoc/>
		public I18nExceptionResult(I18nResultException resultException)
		{
			Code = resultException.Code;
			foreach (var item in I18n.Language.Value ?? I18n.DefaultLanguage)
			{
				if (item is not null)
					if (I18n.I18nDictionary.TryGetValue(item, out var regionLanguage))
					{
						if (regionLanguage.TryGetValue(Code, out var msg))
							Msg = msg;
						break;
					}
			}
			if (Msg is null)
			{
				foreach (var item in I18n.Language.Value ?? I18n.DefaultLanguage)
				{
					if (item is not null)
						if (I18n.I18nDictionary.TryGetValue(item.Split('-')[0], out var rootLanguage))
						{
							if (rootLanguage.TryGetValue(Code, out var msg))
								Msg = msg;
							break;
						}
				}
			}
			if (Msg is not null && resultException.Args is not null)
			{
				Msg = string.Format(Msg, resultException.Args);
			}
		}

		/// <inheritdoc/>
		public string Code { get; set; }
		/// <inheritdoc/>
		public string? Msg { get; set; }
	}

}