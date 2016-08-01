using System;
namespace Business
{
	/// <summary>
	/// Singleton 
	/// </summary>
	public class Session
	{
		private static Session _Instance;
		public static Session Instance {
			get {
				if (_Instance == null) {
					_Instance = new Session ();
				}
				return _Instance;
			}
		}
	}
}
