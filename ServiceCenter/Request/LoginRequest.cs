using System;
using Newtonsoft.Json;

namespace Business
{
	public class LoginRequest
	{
		[JsonProperty ("emailid")]
		public string Username { get; set; }

		[JsonProperty ("password")]
		public string Password { get; set; }
	}
}