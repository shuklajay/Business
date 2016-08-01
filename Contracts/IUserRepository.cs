using System.Threading.Tasks;

namespace Business
{
	public interface IUserRepository
	{
		Task<LoginResponse> Login (LoginRequest request);

		Task<BaseResponse> Logout (int userId);

	}
}