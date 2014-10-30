using System;
using Android.App;
using Android.Runtime;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	[Application]
	public class GamificationApp : Application
	{
		public UserStatsRepository UserStatsRepository { get; set; }

		public GamificationApp(IntPtr javaReference, JniHandleOwnership transfer)
			: base (javaReference, transfer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate ();
			UserStatsRepository = new UserStatsRepository (this);
		}
	}
}

