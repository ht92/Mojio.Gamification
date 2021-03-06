﻿using Android.App;
using Android.Content;
using Android.OS;

namespace Mojio.Gamification.Android
{
	[Activity (Label = "LoadingActivity")]			
	public class SplashScreen : Activity
	{
		private const int SPLASH_TIME_OUT = 2000;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.splash_screen);
		
			System.Threading.Timer timer = new System.Threading.Timer (obj => {
				load ();
			}, null, SPLASH_TIME_OUT, System.Threading.Timeout.Infinite);
		}
	
		private void load ()
		{
			Intent i = new Intent (this, typeof(LoginActivity));
			StartActivity (i);
			Finish ();
			OverridePendingTransition (Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
		}
	}
}

