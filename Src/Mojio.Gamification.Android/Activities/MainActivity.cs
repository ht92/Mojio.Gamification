using System;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.Res;
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
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			mDrawerTitle = this.Resources.GetString (Resource.String.navigation_drawer_title);
			mPageTitles = this.Resources.GetStringArray (Resource.Array.pages_array);
			mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mDrawerList = FindViewById<ListView> (Resource.Id.drawer_list);

			mDrawerLayout.SetDrawerShadow (Resource.Drawable.drawer_shadow, GravityCompat.Start);
			ArrayAdapter listAdapter = new ArrayAdapter (this, Android.Resource.Layout.drawer_list_item, mPageTitles);
			mDrawerList.Adapter = listAdapter;
			mDrawerList.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => 
			{
				OnClick ((View) sender, e.Position);
			};
				
			this.ActionBar.SetDisplayHomeAsUpEnabled (true);
			this.ActionBar.SetHomeButtonEnabled (true);

			mDrawerToggle = new NavigationDrawerToggle (this, mDrawerLayout, Resource.Drawable.ic_drawer, Resource.String.drawer_open, Resource.String.drawer_close);
			mDrawerLayout.SetDrawerListener (mDrawerToggle);

			if (savedInstanceState == null) {
				selectItem (0);
			}
		}

		public override bool OnCreateOptionsMenu (IMenu menu) 
		{
			this.MenuInflater.Inflate (Resource.Menu.navigation_drawer, menu);
			return true;
		}

		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			bool drawerOpen = mDrawerLayout.IsDrawerOpen (mDrawerList);
			menu.FindItem (Resource.Id.action_websearch).SetVisible (!drawerOpen);
			return base.OnPrepareOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (mDrawerToggle.OnOptionsItemSelected (item))	{
				return true;
			}
			switch (item.ItemId) {
			case Resource.Id.action_websearch:
					Intent intent = new Intent(Intent.ActionWebSearch);
					intent.PutExtra (SearchManager.Query, this.ActionBar.Title);
					if (intent.ResolveActivity (this.PackageManager) != null) {
						StartActivity (intent);
					} else {
						Toast.MakeText (this, Resource.String.app_not_available, ToastLength.Long).Show ();
					}
					return true;
			default:
					return base.OnOptionsItemSelected (item);
			}
		}

		protected override void OnTitleChanged (Java.Lang.ICharSequence title, global::Android.Graphics.Color color)
		{
			this.ActionBar.Title = title.ToString ();
		}

		protected override void OnPostCreate(Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			mDrawerToggle.SyncState ();
		}

		public override void OnConfigurationChanged (Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			mDrawerToggle.OnConfigurationChanged (newConfig);
		}

		public void OnClick(View view, int position) 
		{
			selectItem (position);
		}

		private void selectItem(int position)
		{
			//programmatically add fragment to activity layout
			var fragmentManager = this.FragmentManager;
			Fragment currentFragment = fragmentManager.FindFragmentByTag (position.ToString ());
			if (currentFragment != null && currentFragment.IsVisible) { return; }
			var fragment = NavigationFragment.NewInstance (position);
			var transaction = fragmentManager.BeginTransaction ()
				.SetCustomAnimations (Resource.Animator.slide_in_left, Resource.Animator.slide_out_right)
				.Replace (Resource.Id.content_frame, fragment, position.ToString ())
				.Commit ();

			mDrawerList.SetItemChecked (position, true);
			Title = mPageTitles [position];
			mDrawerLayout.CloseDrawer (mDrawerList);
		}

		internal class NavigationDrawerToggle : ActionBarDrawerToggle
		{
			MainActivity owner;
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


