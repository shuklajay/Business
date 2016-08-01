/*using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Business
{
	public class FileUploadRequestHelper
	{
		private readonly Encoding encoding = Encoding.UTF8;
		public string formDataBoundary = String.Format ("----------{0:N}", Guid.NewGuid ());

		public FileUploadRequestHelper ()
		{
		}

		public string UploadFilesToServer (string endPoint, object data, string fileName, string fileContentType, byte [] fileData, HttpBaseClient baseClient)
		{
			string respons = string.Empty;
			string boundary = "----------" + DateTime.Now.Ticks.ToString ("x");
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create (new Uri (endPoint));
			httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
			httpWebRequest.Method = "POST";
			//httpWebRequest.Headers = baseClient.GetWebRequestHeaders ();
			httpWebRequest.BeginGetRequestStream ((result) => {
				try {
					HttpWebRequest request = (HttpWebRequest)result.AsyncState;
					using (Stream requestStream = request.EndGetRequestStream (result)) {
						WriteMultipartForm (requestStream, boundary, data, fileName, fileContentType, fileData);
					}
					request.BeginGetResponse (a => {
						try {
							var response = request.EndGetResponse (a);
							var responseStream = response.GetResponseStream ();
							using (var sr = new StreamReader (responseStream)) {
								using (StreamReader streamReader = new StreamReader (response.GetResponseStream ())) {
									string responseString = streamReader.ReadToEnd ();
									//responseString is depend upon your web service.
									respons = responseString;
									if (responseString == "Success") {
										//  MessageBox.Show("Backup stored successfully on server.");
									} else {
										//  MessageBox.Show("Error occurred while uploading backup on server.");
									}
								}
							}
						} catch (WebException e) {
							using (WebResponse response = e.Response) {
								HttpWebResponse httpResponse = (HttpWebResponse)response;
								//Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
								using (Stream data2 = response.GetResponseStream ())
								using (var reader = new StreamReader (data2)) {
									string text = reader.ReadToEnd ();
									respons = text;
									Debug.WriteLine (text);
								}
							}
						}
					}, null);
				} catch (Exception ex) {
					ex.Source.ToString ();
				}
			}, httpWebRequest);
			return respons;
		}

		/// <summary>
		/// Writes multi part HTTP POST request. Author : Farhan Ghumra
		/// </summary>
		private void WriteMultipartForm (Stream s, string boundary, object data, string fileName, string fileContentType, byte [] fileData)
		{
			// The first boundary
			byte [] boundarybytes = Encoding.UTF8.GetBytes ("--" + boundary + "\r\n");
			// the last boundary.
			byte [] trailer = Encoding.UTF8.GetBytes ("\r\n--" + boundary + "–-\r\n");
			// the form data, properly formatted
			string formdataTemplate = "Content-Dis-data; name=\"{0}\"\r\n\r\n{1}";
			// the form-data file upload, properly formatted
			string fileheaderTemplate = "Content-Dis-data; name=\"{0}\"; filename=\"{1}\";\r\nContent-Type: {2}\r\n\r\n";

			// Added to track if we need a CRLF or not.
			bool bNeedsCRLF = false;
			Dictionary<string, object> dataDict = ConvertToDictionory (data);
			if (dataDict != null) {
				foreach (string key in dataDict.Keys) {
					// if we need to drop a CRLF, do that.
					if (bNeedsCRLF)
						WriteToStream (s, "\r\n");

					// Write the boundary.
					WriteToStream (s, boundarybytes);

					// Write the key.
					WriteToStream (s, string.Format (formdataTemplate, key, dataDict [key]));
					bNeedsCRLF = true;
				}
			}

			// If we don't have keys, we don't need a crlf.
			if (bNeedsCRLF)
				WriteToStream (s, "\r\n");

			WriteToStream (s, boundarybytes);
			WriteToStream (s, string.Format (fileheaderTemplate, "file", fileName, fileContentType));
			// Write the file data to the stream.
			WriteToStream (s, fileData);
			WriteToStream (s, trailer);
		}

		/// <summary>
		/// Writes string to stream. Author : Farhan Ghumra
		/// </summary>
		private void WriteToStream (Stream s, string txt)
		{
			byte [] bytes = Encoding.UTF8.GetBytes (txt);
			s.Write (bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Writes byte array to stream. Author : Farhan Ghumra
		/// </summary>
		private void WriteToStream (Stream s, byte [] bytes)
		{
			s.Write (bytes, 0, bytes.Length);
		}



		public Dictionary<string, object> ConvertToDictionory (object postParameters)
		{
			var request = (UploadJobPictureRequest)postParameters;

			Dictionary<string, object> _postParameters = new Dictionary<string, object> ();
			_postParameters.Add ("vehiclejob_id", request.VehicleJobId);
			_postParameters.Add ("picture_type", request.PictureType);
			_postParameters.Add ("lat_lang", request.LatLong);
			_postParameters.Add ("weather_information", request.WeatherInformation);
			_postParameters.Add ("locationinformation", request.LocationInformation);
			_postParameters.Add ("uploaded_by", request.UploadedBy);
			_postParameters.Add ("picturesource", request.PictureSource);
			_postParameters.Add ("filename", request.filename);
			//_postParameters.Add ("picture_type", new FileParameter (request.DocumentImage, request.DocumentTypeId.ToString (), ".png"));
			return _postParameters;
		}

		public class FileParameter
		{
			public byte [] File { get; set; }

			public string DocumentTypeId { get; set; }

			public string ContentType { get; set; }

			public FileParameter (byte [] file) : this (file, null)
			{
			}

			public FileParameter (byte [] file, string documentTypeId) : this (file, documentTypeId, null)
			{
			}

			public FileParameter (byte [] file, string documentTypeId, string contenttype)
			{
				File = file;
				DocumentTypeId = documentTypeId;
				ContentType = contenttype;
			}
		}

		public byte [] GetMultipartFormData (Dictionary<string, object> postParameters, string boundary)
		{
			Stream formDataStream = new System.IO.MemoryStream ();
			bool needsCLRF = false;
			foreach (var param in postParameters) {
				// Skip it on the first parameter, add it to subsequent parameters.
				if (needsCLRF)
					formDataStream.Write (encoding.GetBytes ("\r\n"), 0, encoding.GetByteCount ("\r\n"));

				needsCLRF = true;

				if (param.Value is FileParameter) {
					FileParameter fileToUpload = (FileParameter)param.Value;

					// Add just the first part of this param, since we will write the file data directly to the Stream
					string header = string.Format ("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
										boundary,
										param.Key,
										fileToUpload.DocumentTypeId ?? param.Key,
										fileToUpload.ContentType ?? "application/octet-stream");

					formDataStream.Write (encoding.GetBytes (header), 0, encoding.GetByteCount (header));

					// Write the file data directly to the Stream, rather than serializing it to a string.
					formDataStream.Write (fileToUpload.File, 0, fileToUpload.File.Length);
				} else {
					string postData = string.Format ("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
										  boundary,
										  param.Key,
										  param.Value);

					formDataStream.Write (encoding.GetBytes (postData), 0, encoding.GetByteCount (postData));
				}
			}
			// Add the end of the request.  Start with a newline
			string footer = "\r\n--" + boundary + "--\r\n";
			formDataStream.Write (encoding.GetBytes (footer), 0, encoding.GetByteCount (footer));

			// Dump the Stream into a byte[]
			formDataStream.Position = 0;
			byte [] formData = new byte [formDataStream.Length];
			formDataStream.Read (formData, 0, formData.Length);
			formDataStream.Flush ();

			return formData;
		}
	}
}*/