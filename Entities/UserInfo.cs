using Newtonsoft.Json;

namespace Business
{
	public class UserInfo
	{
		[JsonProperty ("id")]
		public int Id { get; set; }

		[JsonProperty ("firstname")]
		public string Firstname { get; set; }

		[JsonProperty ("lastname")]
		public string Lastname { get; set; }

		[JsonProperty ("emailid")]
		public string Emailid { get; set; }

		[JsonIgnore]
		private string _fullName;

		[JsonIgnore]
		public string FullName { get { return Firstname + " " + Lastname; } set { _fullName = value; } }
	}
}