using Newtonsoft.Json;

namespace Business
{
	public class BaseResponse
	{
		[JsonProperty ("status")]
		public int Status { get; set; }

		[JsonProperty ("message")]
		public string Message { get; set; }

		[JsonIgnore]
		public bool IsValid { get; set; }
	}
}