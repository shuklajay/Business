using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading;

namespace Business
{
	public class HttpBaseClient
	{
		protected readonly string Endpoint;
		HttpClient httpClient;

		public HttpBaseClient (string endpoint)
		{
			Endpoint = endpoint;
			httpClient = new HttpClient () {
				Timeout = TimeSpan.FromSeconds (60)
			};// (new NativeMessageHandler ());
		}

		private void SetHeader (bool isFileUpload = false)
		{
			/*httpClient.DefaultRequestHeaders.Add ("device_token", Session.Instance.DeviceToken);
			httpClient.DefaultRequestHeaders.Add ("device_type", Session.Instance.DeviceType);*/

			if (isFileUpload) {
				httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("multipart/form-data"));
			} else {
				httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
			}
		}

		public async Task<string> Get ()
		{
			string response;
			try {
				this.SetHeader ();
				response = await this.httpClient.GetStringAsync (Endpoint);
			} catch (TaskCanceledException exception) {
				response = BuildErrorMessage (exception);
			} catch (HttpRequestException exception) {
				//ErrorLoghelperClass.ErrorLog(exception);
				response = BuildErrorMessage (exception);
			} catch (Exception exception) {
				//ErrorLoghelperClass.ErrorLog(exception);
				response = BuildErrorMessage (exception);
			} finally {
				httpClient.Dispose ();
			}
			response = response.Trim ('"');
			response = response.Replace (@"\", "");
			return response;
		}



		public async Task<string> Put (string payload)
		{

			HttpResponseMessage responseMessage;
			string result = null;

			try {

				this.SetHeader ();

				responseMessage = await this.httpClient.PutAsync (Endpoint, new StringContent (payload, System.Text.Encoding.UTF8, "application/json"));

				result = await responseMessage.Content.ReadAsStringAsync ();

			} catch (TaskCanceledException exception) {
				result = BuildErrorMessage (exception);
			} catch (HttpRequestException exception) {
				//ErrorLoghelperClass.ErrorLog(exception);
				result = BuildErrorMessage (exception);
			} catch (Exception exception) {
				//ErrorLoghelperClass.ErrorLog(exception);
				result = BuildErrorMessage (exception);
			} finally {
				httpClient.Dispose ();
			}

			return result;
		}

		public async Task<string> Post (string payload)
		{
			HttpResponseMessage responseMessage;
			string result = null;
			try {
				this.SetHeader ();
				responseMessage = await this.httpClient.PostAsync (Endpoint, new StringContent (payload, System.Text.Encoding.UTF8, "application/json"));
				result = await responseMessage.Content.ReadAsStringAsync ();
			} catch (TaskCanceledException exception) {
				result = BuildErrorMessage (exception);
			} catch (HttpRequestException exc) {
				//ErrorLoghelperClass.ErrorLog(exc);
				result = BuildErrorMessage (exc);
			} catch (Exception exc) {
				//ErrorLoghelperClass.ErrorLog(exc);
				result = BuildErrorMessage (exc);
			} finally {
				httpClient.Dispose ();
			}
			if (result == "0")
				result = true.ToString ();
			return result;
		}

		//public async Task<string> UploadFile (object payload)
		//{
		//	string result = null;
		//	var request = new FileUploadRequestHelper ();
		//	var data = payload as UploadJobPictureRequest;
		//	try {
		//		SetHeader (true);
		//		result = request.UploadFilesToServer (Endpoint, payload, "no name", "application/octet-stream", Convert.FromBase64String (data.PictureSource), this);

		//	} catch (HttpRequestException exc) {
		//		result = BuildErrorMessage (exc);
		//	} catch (Exception exc) {
		//		result = BuildErrorMessage (exc);
		//	} finally {
		//		//fileUpload.Dispose();
		//	}
		//	return result;
		//}

		public string BuildErrorMessage (Exception exc)
		{
			ErrorMessage msg;
			if (exc == null) {
				msg = new ErrorMessage () {
					Message = "Constants.ErrorInServiceMessage",
					StackTrace = exc.StackTrace
				};
			} else {
				if (exc.Equals (exc as OperationCanceledException)) {
					msg = new ErrorMessage () {
						Message = exc.Message.Equals ("A task was canceled.") ? "Message" : string.Empty,
						StackTrace = exc.StackTrace
					};
				} else
					msg = new ErrorMessage () {
						Message = exc.Message,
						StackTrace = exc.StackTrace
					};
			}
			return JsonConvert.SerializeObject (msg);
		}
	}
}