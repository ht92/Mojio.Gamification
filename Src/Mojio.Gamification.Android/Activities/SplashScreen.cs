using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Views.Animations;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Mojio.Gamification.Android
{
	[Activity (Label = "LoadingActivity")]			
	public class SplashScreen : Activity
	{
		private static int SPLASH_TIME_OUT = 1000;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.splash_screen);
		
			System.Threading.Timer timer = new System.Threading.Timer (obj => {
				load ();
			}, null, SPLASH_TIME_OUT, System.Threading.Timeout.Infinite);
		}
	
		private void load ()
		{
			Intent i = new Intent (this, typeof(MainActivity));
			StartActivity (i);
			OverridePendingTransition (Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
			Finish ();
		}
	}
}

