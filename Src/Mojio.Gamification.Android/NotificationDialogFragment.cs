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
			populateDialog (rootView);
			builder.SetView (rootView);
			builder.SetPositiveButton (Resource.String.close_label, dialog_onClose);
			return builder.Create ();;
		}

		private void populateDialog (View view)
		{
			if (AppNotificationService.GetInstance ().HasTripNotifications ()) {
				populateTripsLayout (view);
			}
			if (AppNotificationService.GetInstance ().HasBadgeNotifications ()) {
				populateBadgesLayout (view);
			}
		}
			
		private void populateTripsLayout (View view)
		{
			LinearLayout notifiedTripsLayout = view.FindViewById<LinearLayout> (Resource.Id.notificationView_trips_layout);
			foreach (TripDataModel tripDataModel in AppNotificationService.GetInstance ().NotifiedTrips) {
				ScoreRowView overallScoreRow = new ScoreRowView (Activity);
				ScoreWrapper overallScore = ScoreWrapper.WrapScore (ScoreCalculator.CalculateOverallScore (tripDataModel.TripSafetyScore, tripDataModel.TripEfficiencyScore));
				overallScoreRow.SetScoreLabel (tripDataModel.MyTrip.StartTime.ToString ("MMMM dd, yyyy h:mm tt"));
				overallScoreRow.SetScore (overallScore.Score);
				overallScoreRow.SetRankLabel (String.Format ("RANK {0}", overallScore.Rank));
				notifiedTripsLayout.AddView (overallScoreRow);
			}
			notifiedTripsLayout.Visibility = ViewStates.Visible;
		}

		private void populateBadgesLayout (View view)
		{
			LinearLayout notifiedBadgesLayout = view.FindViewById<LinearLayout> (Resource.Id.notificationView_badges_layout);
			foreach (Badge badge in AppNotificationService.GetInstance ().NotifiedBadges) {
				TextView badgeText = new TextView (Activity);
				badgeText.Text = badge.GetDisplayName ();
				notifiedBadgesLayout.AddView (badgeText);
			}
			notifiedBadgesLayout.Visibility = ViewStates.Visible;
		}

		public override void OnDismiss (IDialogInterface dialog)
		{
			AppNotificationService.GetInstance ().ClearNotifications ();
			base.OnDismiss (dialog);
		}

		private void dialog_onClose (object sender, EventArgs e) {}
	}
}

