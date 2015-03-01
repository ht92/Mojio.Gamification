using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace Mojio.Gamification.Android
{
	[Activity (Label = "LoginActivity")]			
	public class LoginActivity : Activity
	{
		private Button mLoginButton;
		private EditText mUsernameField;
		private EditText mPasswordField;
		private ProgressBar mProgressBar;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.login_layout);
			mLoginButton = FindViewById<Button> (Resource.Id.login_button);
			mUsernameField = FindViewById<EditText> (Resource.Id.login_username_field);
			mPasswordField = FindViewById<EditText> (Resource.Id.login_password_field);
			mProgressBar = FindViewById<ProgressBar> (Resource.Id.login_progressBar);
			mLoginButton.Click += loginButton_onClick;
			attachListeners ();
		}

		private void loginButton_onClick (object sender, EventArgs e) 
		{
			InputMethodManager imm = (InputMethodManager) GetSystemService (InputMethodService);
			imm.HideSoftInputFromWindow (CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
			string username = mUsernameField.Text;
			string password = mPasswordField.Text;
			GamificationApp.GetInstance ().MyConnectionService.Login (username, password);
			mProgressBar.Visibility = ViewStates.Visible;
			enableInputControls (false);
		}

		private void attachListeners ()
		{
			GamificationApp.GetInstance ().InitializationCompleteEvent += (sender, e) => {
				Intent i = new Intent (this, typeof(MainActivity));
				i.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);
				StartActivity (i);
				Finish ();
				OverridePendingTransition (Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
			};
			GamificationApp.GetInstance ().MyConnectionService.LoginEvent += (sender, e) => {
				if (!e.IsSuccess) {
					mProgressBar.Visibility = ViewStates.Gone;
					enableInputControls (true);
					Toast.MakeText (ApplicationContext, "Failed to login...", ToastLength.Long).Show ();
				}
			};
		}

		private void enableInputControls (bool enable)
		{
			mLoginButton.Enabled = enable;
			mUsernameField.Enabled = enable;
			mPasswordField.Enabled = enable;
		}
	}
}

