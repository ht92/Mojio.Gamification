using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Mojio.Gamification.Android
{
	[Activity (Label = "LoginActivity")]			
	public class LoginActivity : Activity
	{
		private Button mLoginButton;
		private EditText mUsernameField;
		private EditText mPasswordField;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.login_layout);
			mLoginButton = FindViewById<Button> (Resource.Id.login_button);
			mUsernameField = FindViewById<EditText> (Resource.Id.login_username_field);
			mPasswordField = FindViewById<EditText> (Resource.Id.login_password_field);
			mLoginButton.Click += loginButton_onClick;
		}

		private void loginButton_onClick (object sender, EventArgs e) 
		{
			string username = mUsernameField.Text;
			string password = mPasswordField.Text;
			if (!GamificationApp.GetInstance ().MyConnectionService.Login (username, password)) {
				Toast.MakeText (ApplicationContext, "Failed to login...", ToastLength.Long).Show ();
				return;
			}
			Intent i = new Intent (this, typeof(MainActivity));
			StartActivity (i);
			OverridePendingTransition (Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
			Finish ();
		}
	}
}

