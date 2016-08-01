using System;
using System.Collections.Generic;

namespace Business
{
	public static class RepositoryInitiator<T>
	{
		#region Property

		// Analysis disable once StaticFieldInGenericType
		///Set this value TRUE or FALSE if you want all calls to REAL or MOCK at one update, this will ignore value set to individually. 
		///If you want some calls are MOCK and some are REAL then set this value to NULL
		internal static bool? IsAllReal = true;

		// Analysis disable once StaticFieldInGenericType
		internal static string TargetNameSpace = Constants.NAMESPACE;

		//Note: add string "Mock" for mock call and "Real" for real call
		//String value "Mock" and "Real" must be a folder name and namespace of the files inside it

		// Analysis disable once StaticFieldInGenericType
		internal static Dictionary<string, string> ObjectFactory = new Dictionary<string, string> () { {
				typeof(IUserRepository).Name,
				string.Format (TargetNameSpace + "." + SetRealOrMock (RepositoryType.Real) + ".UserRepository")
			}
			//	                       ,
			//{
			//	typeof(ISiteListRepository).Name,
			//	string.Format (TargetNameSpace + "." + SetRealOrMock (RepositoryType.Real) + ".SiteListRepository")
			//}, {
			//	typeof(IWeatherRepository).Name,
			//	string.Format (TargetNameSpace + "." + SetRealOrMock (RepositoryType.Real) + ".WeatherRepository")
			//}, {
			//	typeof(IFleetOwnerMyProjectRepository).Name,
			//	string.Format (TargetNameSpace + "." + SetRealOrMock (RepositoryType.Real) + ".FleetOwnerMyProjectRepository")
			//}, {
			//	typeof(IFleetOwnerMySiteRepository).Name,
			//	string.Format (TargetNameSpace + "." + SetRealOrMock (RepositoryType.Real) + ".FleetOwnerMySiteRepository")
			//}, {
			//	typeof(IInstallerMySiteRepository).Name,
			//	string.Format (TargetNameSpace + "." + SetRealOrMock (RepositoryType.Real) + ".InstallerMySiteRepository")
			//},
		};

		#endregion

		#region ReadConfigValue

		internal static string ReadConfigValue (string key)
		{
			return ObjectFactory.ContainsKey (key) ? ObjectFactory [key] : string.Empty;
		}

		#endregion

		#region SetRealOrMock

		internal static string SetRealOrMock (RepositoryType repositoryType)
		{
			try {
				if (IsAllReal != null) {
					return (bool)IsAllReal ? RepositoryType.Real.ToString () : RepositoryType.Mock.ToString ();
				}

			} catch (Exception ex) {
				ex.StackTrace.ToString ();
			}
			return repositoryType.ToString ();
		}

		#endregion

		#region Instance

		public static T Instance {
			get {
				string className = ReadConfigValue (typeof (T).Name);
				var business = Activator.CreateInstance (Type.GetType (className));
				return (T)business;
			}
		}

		#endregion
	}
}