using System;

namespace Business
{
	public class ServiceHelper
	{
		private static string _serviceUrl = Constants.BASE_URL;

		public static string ServiceUrl {
			get {
				return _serviceUrl;
			}
			set {
				_serviceUrl = value;
			}
		}

		public static T GetResponse<T> (string response)
		{
			T data = default (T);

			try {
				data = Newtonsoft.Json.JsonConvert.DeserializeObject<T> (response);
			} catch (Exception ex) {
				//TODO: Add error log in xam insights
			}
			return data;
		}
	}
}

