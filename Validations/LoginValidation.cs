using System;

namespace Business
{
	public class LoginValidation : BaseValidation
	{
		public LoginValidation (LoginRequest request)
		{
			ValidationMessage = new System.Text.StringBuilder ();
			ValidationMessage.Validate (request.Username.IsEmpty () ? ValidationConstants.UsernameRequired : string.Empty);
			ValidationMessage.Validate (request.Password.IsEmpty () ? ValidationConstants.PasswordRequired : string.Empty);
		}
	}
}