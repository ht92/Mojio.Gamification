using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.App;

namespace Mojio.Gamification.Android
{
	public class AppNotificationService
	{
		private const int NOTIFICATION_LIGHT_ON = 1000;
		private const int NOTIFICATION_LIGHT_OFF = 3000;
		private const int NOTIFICATION_DEFAULT_FLAGS = (int) (NotificationDefaults.Sound | NotificationDefaults.Vibrate);

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
		}

		public void IssueNotification (Context context, NotificationType type, int drawableRes, string title, string text)
		{
			NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder (context)
				.SetSmallIcon (drawableRes)
				.SetContentTitle (title)
				.SetContentText (text)
				.SetAutoCancel (true)
				.SetLights (Color.White, NOTIFICATION_LIGHT_ON, NOTIFICATION_LIGHT_OFF)
				.SetDefaults (NOTIFICATION_DEFAULT_FLAGS);

			Intent resultIntent = new Intent (context, typeof(SplashScreen));
			resultIntent.AddFlags (ActivityFlags.ClearTop | ActivityFlags.SingleTop | ActivityFlags.NewTask);
			PendingIntent resultPendingIntent = PendingIntent.GetActivity (context, 0, resultIntent, PendingIntentFlags.UpdateCurrent);
			notificationBuilder.SetContentIntent (resultPendingIntent);
			mNotificationManager.Notify ((int)type, notificationBuilder.Build ());
		}
	}
}

