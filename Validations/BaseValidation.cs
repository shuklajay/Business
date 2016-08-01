using System.Text;

namespace Business
{
	public class BaseValidation
	{
		internal StringBuilder ValidationMessage { get; set; }

		public BaseValidation ()
		{
			ValidationMessage = new StringBuilder ();
		}

		public bool IsValid {
			get {
				return ValidationMessage.ToString ().Trim ().IsEmpty () ? true : false;
			}
		}

		public string ErrorMessage {
			get {
				return ValidationMessage.ToString ();
			}
		}

		public string ValidateLength (string field, string fieldName, int maxLength)
		{
			string message = string.Format ("message", fieldName, maxLength);

			return !field.IsEmpty () && field.Length > maxLength ? message : string.Empty;
		}
	}

	public static class ExtentionClass
	{
		public static StringBuilder Validate (this StringBuilder stringBuilder, string value)
		{
			if (!value.IsEmpty ()) {
				return stringBuilder.AppendLine (value);
			}
			return stringBuilder;
		}
	}
}

