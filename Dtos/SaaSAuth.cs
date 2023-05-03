using System;
namespace tasklistDotNetReact
{
	public class SaaSAuth
	{
		public SaaSAuth()
		{
		}

		public string scope { get; set; }
		public int expires_in { get; set; }
		public string access_token { get; set; }
	}
}

