using System;
using System.Collections.Generic;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class NotificationDialogFragment : DialogFragment
	{
		private static float WIDTH_SCALE = 0.9f;
		private static float HEIGHT_SCALE = 0.6f;
		private static string TAB_NEW_TRIPS_TAG = "tab0";
		private static string TAB_NEW_BADGES_TAG = "tab1";
		private TabHost mTabHost;
		private TabHost.ITabContentFactory mTabContentFactory;

		public override Dialog OnCreateDialog (Bundle savedInstanceState)
		{			
			LayoutInflater layoutInflater = Activity.LayoutInflater;
			View rootView = layoutInflater.Inflate (Resource.Layout.notification_view_layout, null);
			mTabHost = rootView.FindViewById<TabHost> (Resource.Id.notification_view_tabhost);
			mTabHost.Setup ();
			mTabContentFactory = new NotificationDialogTabContentFactory (rootView.Context);
			initializeTabHostView ();
			return createDialog (rootView);
		}

		public override void OnStart ()
		{
			base.OnStart ();
			if (Dialog == null)
				return;
			resizeDialog ();
		}

		private Dialog createDialog (View view)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (Activity);
			builder.SetView (view);
			return builder.Create ();
		}

		private void resizeDialog () 
		{
			DisplayMetrics metric = new DisplayMetrics ();
			Activity.WindowManager.DefaultDisplay.GetMetrics (metric);
			int dialogWidth = (int) (metric.WidthPixels * WIDTH_SCALE);
			int dialogHeight = (int) (metric.HeightPixels * HEIGHT_SCALE);
			Dialog.Window.SetLayout (dialogWidth, dialogHeight);
			Dialog.Window.SetBackgroundDrawableResource (Resource.Color.transparent_black);
		}

		private void initializeTabHostView ()
		{
			if (AppNotificationService.GetInstance ().HasTripNotifications ()) {
				populateTripsLayout ();
			}
			if (AppNotificationService.GetInstance ().HasBadgeNotifications ()) {
				populateBadgesLayout ();
			}
		}
			
		private void populateTripsLayout ()
		{
			TabHost.TabSpec tabSpec = mTabHost.NewTabSpec (TAB_NEW_TRIPS_TAG);
			tabSpec.SetIndicator (Resources.GetString (Resource.String.notification_view_new_trips));
			tabSpec.SetContent (mTabContentFactory);
			mTabHost.AddTab (tabSpec);
		}

		private void populateBadgesLayout ()
		{
			TabHost.TabSpec tabSpec = mTabHost.NewTabSpec (TAB_NEW_BADGES_TAG);
			tabSpec.SetIndicator (Resources.GetString (Resource.String.notification_view_new_badges));
			tabSpec.SetContent (mTabContentFactory);
			mTabHost.AddTab (tabSpec);
		}

		internal class NotificationDialogTabContentFactory : Java.Lang.Object, TabHost.ITabContentFactory
		{
			private Context _context;
			private View _tripsTab;
			private View _badgesTab;

			public NotificationDialogTabContentFactory (Context context)
			{
				_context = context;
				_tripsTab = createNewTripsView ();
				_badgesTab = createNewBadgesView ();
			}

			public View CreateTabContent (string tag)
			{
				if (tag.Equals (TAB_NEW_TRIPS_TAG)) return _tripsTab;
				else if (tag.Equals (TAB_NEW_BADGES_TAG)) return _badgesTab;
				else throw new ArgumentException (String.Format ("Invalid tag provided: {0}", tag));
			}

			private View createNewTripsView ()
			{
				List<TripDataModel> data = new List<TripDataModel> (AppNotificationService.GetInstance ().NotifiedTrips);
				TripHistoryExpandableListAdapter listAdapter = new TripHistoryExpandableListAdapter (_context, data);
				return createListView (listAdapter);
			}

			private View createNewBadgesView ()
			{
				List<Badge> data = new List<Badge> (AppNotificationService.GetInstance ().NotifiedBadges);
				BadgeCollectionExpandableListAdapter listAdapter = new BadgeCollectionExpandableListAdapter (_context, data);
				return createListView (listAdapter);
			}

			private ExpandableListView createListView (IExpandableListAdapter adapter)
			{
				ExpandableListView listView = new ExpandableListView (_context);
				listView.SetGroupIndicator (null);
				listView.ChoiceMode = ChoiceMode.Single;
				listView.SetOnGroupClickListener (new NotificationDialogListOnGroupClickListener ());
				listView.SetAdapter (adapter);
				return listView;
			}

			internal class NotificationDialogListOnGroupClickListener : Java.Lang.Object, ExpandableListView.IOnGroupClickListener
			{
				bool ExpandableListView.IOnGroupClickListener.OnGroupClick (ExpandableListView parent, View clickedView, int groupPosition, long id)
				{
					return true;
				}
			}
		}
	}
}

