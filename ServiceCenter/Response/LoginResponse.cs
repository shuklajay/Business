using Newtonsoft.Json;

namespace Business
{
	public class LoginResponse : BaseResponse
	{
		[JsonProperty ("userinfo")]
		public UserInfo UserInfo { get; set; }

		[JsonProperty ("termsandcondition")]
		public string TermsAndCondition { get; set; }
	}
}

