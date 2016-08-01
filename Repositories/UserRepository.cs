using System;
using System.Threading.Tasks;

namespace Business
{
	public class UserRepository : IUserRepository
	{
		readonly HttpRequestHelper client;

		#region Constructor

		public UserRepository ()
		{
			client = new HttpRequestHelper ();
		}

		#endregion

		#region IUserRepository implementation

		public async Task<LoginResponse> Login (LoginRequest request)
		{
			var response = await client.CreatePostRequest<LoginResponse> ("login", request);
			return response;
		}

		public async Task<BaseResponse> Logout (int userId)
		{
			string payload = "{\"userid\":" + userId + "}";
			return await this.client.CreatePostRequest<BaseResponse> ("logout", payload);

		}

		public async Task<BaseResponse> ForgotPassword (string email)
		{
			string payload = "{\"emailid\":" + "\"" + email + "\"" + "}";
			return await this.client.CreatePostRequest<BaseResponse> ("forgotpassword", payload);
		}

		public async Task<BaseResponse> ChangePassword (int userId, string oldPassword, string newPassword)
		{
			string payload = "{\"userid\":" + userId + ", \"oldpassword\":\"" + oldPassword + "\",\"newpassword\":\"" + newPassword + "\" }";
			return await this.client.CreatePostRequest<BaseResponse> ("changepassword", payload);
		}

		#endregion
	}
}