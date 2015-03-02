using System;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
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
			DisplayMetrics metric = new DisplayMetrics ();
			Activity.WindowManager.DefaultDisplay.GetMetrics (metric);
			int dialogWidth = (int) (metric.WidthPixels * WIDTH_SCALE);
			int dialogHeight = (int) (metric.HeightPixels * HEIGHT_SCALE);
			Dialog.Window.SetLayout (dialogWidth, dialogHeight);
			Dialog.Window.SetBackgroundDrawable (new ColorDrawable (Resources.GetColor (Resource.Color.transparent_black)));
		}

		private Dialog createDialog (View view)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (Activity);
			builder.SetView (view);
			return builder.Create ();
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

		public override void OnDismiss (IDialogInterface dialog)
		{
			AppNotificationService.GetInstance ().ClearNotifications ();
			base.OnDismiss (dialog);
		}

		internal class NotificationDialogTabContentFactory : Java.Lang.Object, TabHost.ITabContentFactory
		{
			private Context _context;

			public NotificationDialogTabContentFactory (Context context)
			{
				_context = context;
			}
			public View CreateTabContent (string tag)
			{
				if (tag.Equals (TAB_NEW_TRIPS_TAG)) return createNewTripsView ();
				else if (tag.Equals (TAB_NEW_BADGES_TAG)) return createNewBadgesView ();
				else throw new ArgumentException (String.Format ("Invalid tag provided: {0}", tag));
			}

			private View createNewTripsView ()
			{
				ScrollView view = new ScrollView (_context);
				LinearLayout layout = new LinearLayout (_context);
				layout.Orientation = Orientation.Vertical;
				foreach (TripDataModel tripDataModel in AppNotificationService.GetInstance ().NotifiedTrips) {
					ScoreRowView overallScoreRow = new ScoreRowView (_context);
					ScoreWrapper overallScore = ScoreWrapper.WrapScore (ScoreCalculator.CalculateOverallScore (tripDataModel.TripSafetyScore, tripDataModel.TripEfficiencyScore));
					overallScoreRow.SetScoreLabel (tripDataModel.MyTrip.StartTime.ToString ("MMM dd, yyyy h:mm tt"));
					overallScoreRow.SetScore (overallScore);
					layout.AddView (overallScoreRow);
				}
				view.AddView (layout);
				return view;
			}

			private View createNewBadgesView ()
			{
				ScrollView view = new ScrollView (_context);
				LinearLayout layout = new LinearLayout (_context);
				layout.Orientation = Orientation.Vertical;
				foreach (Badge badge in AppNotificationService.GetInstance ().NotifiedBadges) {
					BadgeRowView badgeRow = new BadgeRowView (_context);
					badgeRow.SetBadge (badge);
					layout.AddView (badgeRow);
				}
				view.AddView (layout);
				return view;
			}
		}
	}
}

