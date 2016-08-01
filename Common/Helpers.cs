using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Business
{
	public static class Helpers
	{
		public static bool IsEmpty (this string str)
		{
			return String.IsNullOrEmpty (str) || String.IsNullOrWhiteSpace (str);
		}

		public static bool ValidateEmail (this string email)
		{
			const string MatchEmailPattern = "[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}";
			var res = Regex.IsMatch (email, MatchEmailPattern);
			return res;
		}

		public static bool ValidatePassword (this string password)
		{
			const string MatchPasswordPattern = "^.*(?=.{6,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$";
			return Regex.IsMatch (password, MatchPasswordPattern);
		}


		/// <summary>
		/// Converts the given object into serialized string format
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>string</returns>
		public static string ObjectToString<T> (T dataObject)
		{
			try {
				return JsonConvert.SerializeObject (dataObject);
			} catch {
				return string.Empty;
			}
		}

		/// <summary>
		/// Converts the given string into type of given object
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="str"></param>
		/// <returns></returns>
		public static T StringToObject<T> (string value)
		{
			T data = default (T);
			try {
				data = JsonConvert.DeserializeObject<T> (value);
			} catch {

			}
			return data;
		}

		public static string ConvertToOfflineFinalJson<T> (T dataObject)
		{
			try {
				string value = JsonConvert.SerializeObject (dataObject, Formatting.Indented);
				return value;
			} catch {
				return string.Empty;
			}
		}

		public static T ConvertToServiceFinalJson<T> (string dataObject)
		{
			T data = default (T);
			try {
				data = JsonConvert.DeserializeObject<T> (dataObject);
				return data;
			} catch (Exception ex) {
				return data;
			}
		}
	}
}