﻿using System;
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
		private CheckBox mIsLiveCheckbox;
		private ProgressBar mProgressBar;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.login_layout);
			mLoginButton = FindViewById<Button> (Resource.Id.login_button);
			mUsernameField = FindViewById<EditText> (Resource.Id.login_username_field);
			mPasswordField = FindViewById<EditText> (Resource.Id.login_password_field);
			mProgressBar = FindViewById<ProgressBar> (Resource.Id.login_progressBar);
			mIsLiveCheckbox = FindViewById<CheckBox> (Resource.Id.isLiveCheckbox);
			mIsLiveCheckbox.CheckedChange += isLive_onCheckedChange;
			mLoginButton.Click += loginButton_onClick;
			attachListeners ();
		}

		private void loginButton_onClick (object sender, EventArgs e) 
		{
			InputMethodManager imm = (InputMethodManager) GetSystemService (InputMethodService);
			imm.HideSoftInputFromWindow (CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
			login ();
		}

		private void isLive_onCheckedChange (object sender, EventArgs e)
		{
			if (mIsLiveCheckbox.Checked) {
				mUsernameField.Text = "alaraozturk@hotmail.com";
				mPasswordField.Text = "Capstone2015";
			} else {
				mUsernameField.Text = "ht92@live.ca";
				mPasswordField.Text = "Eece419";
			}
		}

		private void login ()
		{
			string username = mUsernameField.Text;
			string password = mPasswordField.Text;
			GamificationApp.GetInstance ().MyConnectionService.Login (username, password, mIsLiveCheckbox.Checked);
			mProgressBar.Visibility = ViewStates.Visible;
			enableInputControls (false);
		}

		private void attachListeners ()
		{
			GamificationApp.GetInstance ().InitializationCompleteEvent += (sender, e) => startNextActivity ();
			GamificationApp.GetInstance ().MyConnectionService.LoginEvent += (sender, e) => {
				if (!e.IsSuccess) {
					handleUnsuccessfulLogin ();
				}
			};
		}

		private void startNextActivity ()
		{
			Intent i = new Intent (this, typeof(MainActivity));
			i.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);
			StartActivity (i);
			Finish ();
			OverridePendingTransition (Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
		}

		private void handleUnsuccessfulLogin ()
		{
			mProgressBar.Visibility = ViewStates.Gone;
			enableInputControls (true);
			Toast.MakeText (ApplicationContext, Resource.String.error_failed_login, ToastLength.Long).Show ();
		}

		private void enableInputControls (bool enable)
		{
			mLoginButton.Enabled = enable;
			mUsernameField.Enabled = enable;
			mPasswordField.Enabled = enable;
		}
	}
}

