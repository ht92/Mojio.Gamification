using System;
using Android.App;
using Android.Runtime;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	[Application]
	public class GamificationApp : Application
	{
		public UserStatsRepository MyUserStatsRepository { get; set; }

		public GamificationApp(IntPtr javaReference, JniHandleOwnership transfer)
			: base (javaReference, transfer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate ();
			MyUserStatsRepository = new UserStatsRepository (this);
			//MojioConnectUtility.Connect ();
		}
	}
}

