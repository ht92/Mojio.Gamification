using System;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Content.PM;
using Android.Locations;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;

using Mojio;
using Mojio.Client;
using Mojio.Gamification.Core;

using Fragment = Android.App.Fragment;

namespace Mojio.Gamification.Android
{
	[Activity (Label = "Mojio.Gamification.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private DrawerLayout mDrawerLayout;
		private ListView mDrawerList;
		private String mDrawerTitle;
		private String[] mPageTitles;

		private ActionBarDrawerToggle mDrawerToggle;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RequestedOrientation = ScreenOrientation.Portrait;
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			Window.DecorView.SetBackgroundResource (getActivityBackground ());
				
			mDrawerTitle = this.Resources.GetString (Resource.String.navigation_drawer_title);
			mPageTitles = this.Resources.GetStringArray (Resource.Array.pages_array);
			mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mDrawerList = FindViewById<ListView> (Resource.Id.drawer_list);

			mDrawerLayout.SetDrawerShadow (Resource.Drawable.drawer_shadow, GravityCompat.Start);
			ArrayAdapter listAdapter = new ArrayAdapter (this, Android.Resource.Layout.drawer_list_item, mPageTitles);
			mDrawerList.Adapter = listAdapter;
			mDrawerList.ItemClick += navDrawer_onClick;
				
			this.ActionBar.SetDisplayHomeAsUpEnabled (true);
			this.ActionBar.SetHomeButtonEnabled (true);

			mDrawerToggle = new NavigationDrawerToggle (this, mDrawerLayout, Resource.Drawable.ic_drawer, Resource.String.drawer_open, Resource.String.drawer_close);
			mDrawerLayout.SetDrawerListener (mDrawerToggle);

			if (savedInstanceState == null) {
				SelectFragment ((int)AbstractNavigationFragment.NavigationFragmentType.NAV_HOME);
			}
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
				base.OnBackPressed ();
			} else {
				SelectFragment ((int)AbstractNavigationFragment.NavigationFragmentType.NAV_HOME);
			}
		}

		private void navDrawer_onClick (object sender, AdapterView.ItemClickEventArgs e) 
		{
			int position = e.Position;
			mDrawerList.SetItemChecked (position, true);
			Title = mPageTitles [position];
			mDrawerLayout.CloseDrawer (mDrawerList);
			SelectFragment (position);
		}

		public void SelectFragment (int position)
		{
			//programmatically add fragment to activity layout
			var fragmentManager = this.FragmentManager;
			AbstractNavigationFragment currentFragment = fragmentManager.FindFragmentByTag<AbstractNavigationFragment> (position.ToString ());
			if (currentFragment != null && currentFragment.IsVisible) { return; }
			var fragment = AbstractNavigationFragment.NewInstance ((AbstractNavigationFragment.NavigationFragmentType)position);
			var transaction = fragmentManager.BeginTransaction ();
			transaction.SetCustomAnimations (Resource.Animator.fade_in, Resource.Animator.fade_out, Resource.Animator.slide_in_left, Resource.Animator.fade_out);
			transaction.Replace (Resource.Id.content_frame, fragment, position.ToString ());
			transaction.Commit ();
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


