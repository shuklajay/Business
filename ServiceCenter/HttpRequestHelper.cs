using System;
using System.Threading.Tasks;

namespace Business
{
	public class HttpRequestHelper
	{
		protected readonly string ServiceEndpoint;

		public HttpRequestHelper ()
		{
			ServiceEndpoint = ServiceHelper.ServiceUrl;
		}

		//public async Task<T> CreateGetResponse<T>(string ServiceAPIResourcePath)
		//{
		//    var httpBaseClient = new HttpBaseClient(System.IO.Path.Combine(new string[] {
		//        ServiceEndpoint,
		//        ServiceAPIResourcePath
		//    }));

		//    var responseCRServiceContent = await httpBaseClient.Get();
		//    var responseServiceContent = ServiceHelper.GetResponse<T>(responseCRServiceContent);
		//    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseServiceContent);
		//    return data;
		//}

		public async Task<T> CreateGetWeatherResponse<T> (string ServiceAPIResourcePath)
		{
			try {
				var httpBaseClient = new HttpBaseClient (ServiceAPIResourcePath);

				var responseCRServiceContent = await httpBaseClient.Get ();
				return ServiceHelper.GetResponse<T> (responseCRServiceContent);
			} catch (Exception ex) {
				//ErrorLoghelperClass.ErrorLog (ex);
				throw;
			}
			/*
			var client = new HttpClient ();

			var json = await client.GetStringAsync (url);

			if (string.IsNullOrWhiteSpace (json))
				return null;
			json = json.Trim ('"');
			json = json.Replace (@"\", "");
			return JsonConvert.DeserializeObject<RootObject> (json);*/
		}


		public async Task<T> CreateGetResponse<T> (string ServiceAPIResourcePath)
		{
			try {
				var httpBaseClient = new HttpBaseClient (System.IO.Path.Combine (new string [] {
					ServiceEndpoint,
					ServiceAPIResourcePath
				}));

				var responseCRServiceContent = await httpBaseClient.Get ();
				return ServiceHelper.GetResponse<T> (responseCRServiceContent);
			} catch (Exception ex) {
				//ErrorLoghelperClass.ErrorLog (ex);
				throw;
			}
		}

		public async Task<string> CreateImagePostRequest<T> (string ServiceAPIResourcePath, object payload)
		{
			var httpBaseClient = new HttpBaseClient (System.IO.Path.Combine (new string [] {
				ServiceEndpoint,
				ServiceAPIResourcePath
			}));
			var responseServiceContent = await httpBaseClient.Post (Newtonsoft.Json.JsonConvert.SerializeObject (payload));
			return Newtonsoft.Json.JsonConvert.DeserializeObject<string> (responseServiceContent);
		}

		public async Task<T> CreatePostRequest<T> (string ServiceAPIResourcePath, object payload)
		{
			var httpBaseClient = new HttpBaseClient (System.IO.Path.Combine (new string [] {
				ServiceEndpoint,
				ServiceAPIResourcePath
			}));
			if (payload.GetType ().ToString () != "System.String") {
				payload = Newtonsoft.Json.JsonConvert.SerializeObject (payload);
			}
			var responseServiceContent = await httpBaseClient.Post (payload.ToString ());
			return ServiceHelper.GetResponse<T> (responseServiceContent);
		}

		/*public async Task<T> CreateUploadFileRequest<T> (string ServiceAPIResourcePath, object payload)
		{
			string responseCRServiceContent = "";
			try {
				var httpBaseClient = new HttpBaseClient (System.IO.Path.Combine (new string [] {
				ServiceEndpoint,
				ServiceAPIResourcePath
			}));

				//if (payload.GetType ().ToString () != "System.String") {
				//	payload = Newtonsoft.Json.JsonConvert.SerializeObject (payload);
				//}
				//var responseCRServiceContent = await httpBaseClient.UploadFile(Newtonsoft.Json.JsonConvert.SerializeObject(payload).ToString());
				//var responseCRServiceContent = await httpBaseClient.UploadImage(payload);
				responseCRServiceContent = await httpBaseClient.UploadFile (payload);

				////return ServiceHelper.GetResponse<T> (responseCRServiceContent);
			} catch (Exception ex) {
				ex.StackTrace.ToString ();
			}
			return ServiceHelper.GetResponse<T> (responseCRServiceContent);
		}*/
	}
}