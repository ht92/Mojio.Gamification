using System;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class NotificationDialogFragment : DialogFragment
	{
		public override Dialog OnCreateDialog (Bundle savedInstanceState)
		{			
			AlertDialog.Builder builder = new AlertDialog.Builder (Activity);
			LayoutInflater layoutInflater = Activity.LayoutInflater;
			View rootView = layoutInflater.Inflate (Resource.Layout.notification_view_layout, null);
			initializedDialog (rootView);
			builder.SetView (rootView);
			return builder.Create ();
		}

		private void initializedDialog (View view)
		{
			initializeCloseButton (view);
			if (AppNotificationService.GetInstance ().HasTripNotifications ()) {
				populateTripsLayout (view);
			}
			if (AppNotificationService.GetInstance ().HasBadgeNotifications ()) {
				populateBadgesLayout (view);
			}
		}
			
		private void populateTripsLayout (View view)
		{
			TextView notifiedTripsTitle = view.FindViewById<TextView> (Resource.Id.notificationView_trips_title);
			LinearLayout notifiedTripsLayout = view.FindViewById<LinearLayout> (Resource.Id.notificationView_trips_layout);
			foreach (TripDataModel tripDataModel in AppNotificationService.GetInstance ().NotifiedTrips) {
				ScoreRowView overallScoreRow = new ScoreRowView (Activity);
				ScoreWrapper overallScore = ScoreWrapper.WrapScore (ScoreCalculator.CalculateOverallScore (tripDataModel.TripSafetyScore, tripDataModel.TripEfficiencyScore));
				overallScoreRow.SetScoreLabel (tripDataModel.MyTrip.StartTime.ToString ("MMMM dd, yyyy h:mm tt"));
				overallScoreRow.SetScore (overallScore.Score);
				overallScoreRow.SetRankLabel (String.Format ("RANK {0}", overallScore.Rank));
				notifiedTripsLayout.AddView (overallScoreRow);
			}
			notifiedTripsTitle.Visibility = ViewStates.Visible;
			notifiedTripsLayout.Visibility = ViewStates.Visible;
		}

		private void populateBadgesLayout (View view)
		{
			TextView notifiedBadgesTitle = view.FindViewById<TextView> (Resource.Id.notificationView_badges_title);
			LinearLayout notifiedBadgesLayout = view.FindViewById<LinearLayout> (Resource.Id.notificationView_badges_layout);
			foreach (Badge badge in AppNotificationService.GetInstance ().NotifiedBadges) {
				BadgeRowView badgeRow = new BadgeRowView (Activity);
				badgeRow.SetBadge (badge);
				notifiedBadgesLayout.AddView (badgeRow);
			}
			notifiedBadgesTitle.Visibility = ViewStates.Visible;
			notifiedBadgesLayout.Visibility = ViewStates.Visible;
		}

		private void initializeCloseButton (View view)
		{
			ImageView closeButton = view.FindViewById<ImageView> (Resource.Id.notificationView_close_button);
			closeButton.Click += dialog_onClose;
		}

		public override void OnDismiss (IDialogInterface dialog)
		{
			AppNotificationService.GetInstance ().ClearNotifications ();
			base.OnDismiss (dialog);
		}

		private void dialog_onClose (object sender, EventArgs e) {
			this.Dialog.Dismiss ();
		}
	}
}

