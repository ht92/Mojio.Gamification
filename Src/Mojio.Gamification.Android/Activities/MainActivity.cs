using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Content.PM;
using Android.Locations;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;

using Fragment = Android.App.Fragment;

namespace Mojio.Gamification.Android
{
	[Activity (Label = "Mojio.Gamification.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private DrawerLayout mDrawerLayout;
		private ListView mDrawerList;
		private String mDrawerTitle;
		private List<String> mNavigationDestinations = new List<string> ();
		private List<string> mNavigationPages;
		private String mNavigationLogout;
		private ProgressDialog mLoadingDialog;
		private ProgressDialog mSyncingProgressDialog;

		private ActionBarDrawerToggle mDrawerToggle;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			attachListeners ();
			initializeComponents ();
			syncLatestData ();
			if (savedInstanceState == null) {
				SelectFragment ((int)AbstractNavigationFragment.NavigationFragmentType.NAV_HOME);
			}
		}

		private void initializeComponents ()
		{
			RequestedOrientation = ScreenOrientation.Portrait;
			SetContentView(Resource.Layout.Main);
			Window.DecorView.SetBackgroundResource (getActivityBackground ());

			mDrawerTitle = this.Resources.GetString (Resource.String.navigation_drawer_title);
			mNavigationPages = new List<string> (this.Resources.GetStringArray (Resource.Array.pages_array));
			mNavigationLogout = Resources.GetString (Resource.String.navigation_logout);
			mNavigationDestinations.AddRange (mNavigationPages);
			mNavigationDestinations.Add (mNavigationLogout);

			mSyncingProgressDialog = new ProgressDialog (this);
			mSyncingProgressDialog.SetMessage (Resources.GetString (Resource.String.sync_data_dialog_progress));
			mSyncingProgressDialog.SetCancelable (false);
			mSyncingProgressDialog.SetCanceledOnTouchOutside (false);

			mLoadingDialog = new ProgressDialog (this);
			mLoadingDialog.SetCancelable (false);
			mLoadingDialog.SetCanceledOnTouchOutside (false);

			mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mDrawerList = FindViewById<ListView> (Resource.Id.drawer_list);

			mDrawerLayout.SetDrawerShadow (Resource.Drawable.drawer_shadow, GravityCompat.Start);

			ArrayAdapter listAdapter = new ArrayAdapter (this, Resource.Layout.drawer_list_item, mNavigationDestinations);
			mDrawerList.Adapter = listAdapter;
			mDrawerList.ItemClick += navDrawer_onClick;

			this.ActionBar.SetDisplayHomeAsUpEnabled (true);
			this.ActionBar.SetHomeButtonEnabled (true);

			mDrawerToggle = new NavigationDrawerToggle (this, mDrawerLayout, Resource.Drawable.ic_drawer, Resource.String.drawer_open, Resource.String.drawer_close);
			mDrawerLayout.SetDrawerListener (mDrawerToggle);
		}

		protected override void OnResume ()
		{
			if (!GamificationApp.GetInstance ().MyConnectionService.IsConnected () 
				|| GamificationApp.GetInstance ().MyConnectionService.HasTokenExpired ()) {
				sessionExpired ();
			}
			base.OnResume ();
		}

		protected override void OnPostCreate(Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			mDrawerToggle.SyncState ();
		}

		protected override void OnTitleChanged (Java.Lang.ICharSequence title, global::Android.Graphics.Color color)
		{
			this.ActionBar.Title = title.ToString ();
		}

		public override void OnConfigurationChanged (Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			mDrawerToggle.OnConfigurationChanged (newConfig);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (mDrawerToggle.OnOptionsItemSelected (item))	return true;
			return base.OnOptionsItemSelected (item);
		}
			
		public override void OnBackPressed() 
		{
			var fragmentManager = this.FragmentManager;
			Fragment currentFragment = fragmentManager.FindFragmentByTag ("0");
			if (currentFragment != null && currentFragment.IsVisible) {
				MoveTaskToBack (true);
			} else {
				SelectFragment ((int)AbstractNavigationFragment.NavigationFragmentType.NAV_HOME);
			}
		}

		private void navDrawer_onClick (object sender, AdapterView.ItemClickEventArgs e) 
		{
			int position = e.Position;
			if (mNavigationDestinations [position].Equals (mNavigationLogout)) {
				logout ();
			} else {
				Title = mNavigationPages [position];
				mDrawerLayout.CloseDrawer (mDrawerList);
				SelectFragment (position);
			}
		}

		public void SelectFragment (int position)
		{
			//programmatically add fragment to activity layout
			mDrawerList.SetItemChecked (position, true);
			var fragmentManager = this.FragmentManager;
			AbstractNavigationFragment currentFragment = fragmentManager.FindFragmentByTag<AbstractNavigationFragment> (position.ToString ());
			if (currentFragment != null && currentFragment.IsVisible) { return; }
			var fragment = AbstractNavigationFragment.NewInstance ((AbstractNavigationFragment.NavigationFragmentType)position);
			var transaction = fragmentManager.BeginTransaction ();
			transaction.SetCustomAnimations (Resource.Animator.fade_in, Resource.Animator.fade_out, Resource.Animator.slide_in_left, Resource.Animator.fade_out);
			transaction.Replace (Resource.Id.content_frame, fragment, position.ToString ());
			transaction.Commit ();
		}

		private void syncLatestData ()
		{
			if (GamificationApp.GetInstance ().MyConnectionService.IsConnected() && GamificationApp.GetInstance ().MyConnectionService.IsLive) {
				GamificationApp.GetInstance ().MyConnectionService.FetchLatestTripsSinceLastReceivedAsync ();
				mSyncingProgressDialog.Show ();
			}
		}

		private void sessionExpired ()
		{
			mLoadingDialog.SetMessage (Resources.GetString (Resource.String.session_expired));
			mLoadingDialog.Show ();
			GamificationApp.GetInstance ().MyConnectionService.Logout ();
		}

		private void logout ()
		{
			mLoadingDialog.SetMessage (Resources.GetString (Resource.String.logging_out));
			mLoadingDialog.Show ();
			GamificationApp.GetInstance ().MyConnectionService.Logout ();
		}

		private int getActivityBackground ()
		{
			LocationManager lm = (LocationManager) GetSystemService (LocationService);
			global::Android.Locations.Location location = lm.GetLastKnownLocation (LocationManager.GpsProvider);
			WeatherManager.WeatherType weatherType = WeatherManager.GetWeather (location);
			switch (weatherType) {
			case WeatherManager.WeatherType.SUNNY:
				return Resource.Drawable.back_sunny;
			case WeatherManager.WeatherType.CLOUDY:
				return Resource.Drawable.back_cloudy;
			case WeatherManager.WeatherType.RAINING:
				return Resource.Drawable.back_rainy;
			case WeatherManager.WeatherType.SNOWING:
				return Resource.Drawable.back_snow;
			default: 
				return Resource.Drawable.back_sunny;
			}
		}

		private void attachListeners ()
		{
			GamificationApp.GetInstance ().MyConnectionService.FetchTripsCompletedEvent += (sender, e) => mSyncingProgressDialog.Dismiss ();
			GamificationApp.GetInstance ().MyConnectionService.LogoutEvent += (sender, e) => 
			{
				if (e.IsSuccess) {
					GamificationApp.GetInstance ().MyNotificationService.ClearNotifications ();
					GamificationApp.GetInstance ().Uninitialize ();
					Intent i = BaseContext.PackageManager.GetLaunchIntentForPackage (BaseContext.PackageName);
					i.AddFlags (ActivityFlags.ClearTop);
					Finish ();
					StartActivity (i);
					OverridePendingTransition (Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
				} else {
					Toast.MakeText (ApplicationContext, Resource.String.error_failed_logout, ToastLength.Long).Show ();
				}
				mLoadingDialog.Dismiss ();
			};
		}
			
		internal class NavigationDrawerToggle : ActionBarDrawerToggle
		{
			private readonly MainActivity owner;
			public NavigationDrawerToggle (MainActivity activity, DrawerLayout layout, int imgRes, int openRes, int closeRes)
				: base (activity, layout, imgRes, openRes, closeRes)
			{
				owner = activity;
			}

			public override void OnDrawerClosed (View drawerView)
			{
				owner.ActionBar.Title = owner.Title;
				owner.InvalidateOptionsMenu ();
			}

			public override void OnDrawerOpened(View drawerView)
			{
				owner.ActionBar.Title = owner.mDrawerTitle;
				owner.InvalidateOptionsMenu ();
			}
		}
	}
}


