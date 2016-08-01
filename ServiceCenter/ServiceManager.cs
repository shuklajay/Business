using System;

namespace Business
{
	public class ServiceManager
	{
		private readonly HttpRequestHelper httpRequestHelper;

		public ServiceManager ()
		{
			httpRequestHelper = new HttpRequestHelper ();
		}
	}
}

