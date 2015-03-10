using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.App;
using Mojio.Events;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class AppNotificationService
	{
		public event EventHandler NotificationEvent;

		private const int NOTIFICATION_LIGHT_ON = 1000;
		private const int NOTIFICATION_LIGHT_OFF = 3000;
		private const int NOTIFICATION_DEFAULT_FLAGS = (int) (NotificationDefaults.Sound | NotificationDefaults.Vibrate);

		private static string NOTIFICATION_TITLE_TEXT = GamificationApp.GetInstance ().Resources.GetString (Resource.String.app_name);
		private static string NOTIFICATION_NEW_TRIP_TEXT = GamificationApp.GetInstance ().Resources.GetString (Resource.String.notification_new_trips);
		private static string NOTIFICATION_NEW_BADGE_TEXT = GamificationApp.GetInstance ().Resources.GetString (Resource.String.notification_new_badges);
		private static int NOTIFICATION_ICON_RES = Resource.Drawable.logo_white;

		public List<TripDataModel> NotifiedTrips { get; private set; }
		public List<Badge> NotifiedBadges { get; private set; }

		public enum NotificationType
		{
			NOTIFICATION_TRIP,
			NOTIFICATION_BADGE,
		}

		private static AppNotificationService mInstance;
		private readonly NotificationManager mNotificationManager;

		public static AppNotificationService GetInstance ()
		{
			if (mInstance == null) {
				mInstance = new AppNotificationService ();
			}
			return mInstance;
		}

		private AppNotificationService ()
		{
			mNotificationManager = (NotificationManager) GamificationApp.GetInstance ().GetSystemService (Context.NotificationService);
			NotifiedTrips = new List<TripDataModel> ();
			NotifiedBadges = new List<Badge> ();
		}

		public void IssueTripNotification (TripDataModel tripDataModel)
		{
			NotifiedTrips.Add (tripDataModel);
			issueNotification (NotificationType.NOTIFICATION_TRIP, NOTIFICATION_TITLE_TEXT, NOTIFICATION_NEW_TRIP_TEXT);
		}

		public void IssueBadgeNotification (Badge badge)
		{
			NotifiedBadges.Add (badge);
			issueNotification (NotificationType.NOTIFICATION_BADGE, NOTIFICATION_TITLE_TEXT, NOTIFICATION_NEW_BADGE_TEXT);
		}

		public bool HasTripNotifications ()
		{
			return NotifiedTrips.Count > 0;
		}

		public bool HasBadgeNotifications ()
		{
			return NotifiedBadges.Count > 0;
		}

		public bool HasNotifications ()
		{
			return HasTripNotifications () || HasBadgeNotifications ();
		}

		public void ClearNotifications ()
		{
			NotifiedTrips.Clear ();
			NotifiedBadges.Clear ();
			mNotificationManager.CancelAll ();
		}

		private void issueNotification (NotificationType type, string title, string text)
		{
			NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder (GamificationApp.GetInstance ())
				.SetSmallIcon (NOTIFICATION_ICON_RES)
				.SetContentTitle (title)
				.SetContentText (text)
				.SetAutoCancel (true)
				.SetLights (Color.White, NOTIFICATION_LIGHT_ON, NOTIFICATION_LIGHT_OFF)
				.SetDefaults (NOTIFICATION_DEFAULT_FLAGS);

			Intent resultIntent = new Intent (GamificationApp.GetInstance (), typeof(MainActivity));
			resultIntent.SetAction (Intent.ActionMain);
			resultIntent.AddCategory (Intent.CategoryLauncher);
			PendingIntent resultPendingIntent = PendingIntent.GetActivity (GamificationApp.GetInstance (), 0, resultIntent, PendingIntentFlags.UpdateCurrent);
			notificationBuilder.SetContentIntent (resultPendingIntent);
			mNotificationManager.Notify ((int)type, notificationBuilder.Build ());
			onNotificationEvent ();
		}

		private void onNotificationEvent ()
		{
			NotificationEvent (this, EventArgs.Empty);
		}
	}
}

