using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Mojio.Gamification.Core;
using Xamarin.Facebook;
using Xamarin.Facebook.Model;
using Xamarin.Facebook.Widget;

namespace Mojio.Gamification.Android
{
	public class HomeNavigationFragment : AbstractNavigationFragment, View.IOnTouchListener, GestureDetector.IOnGestureListener
	{
		private const int SWIPE_MIN_DISTANCE = 200;
		private const int SWIPE_THRESHOLD_VELOCITY = 200;
		private GestureDetector mGestureDetector;
		private CircularIndicatorView mDriverScoreIndicator;
		private ImageView mNotificationIndicator;

		private UiLifecycleHelper mUiHelper;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			mUiHelper = new UiLifecycleHelper (this.Activity, null);
			mUiHelper.OnCreate (savedInstanceState);
		}

		public override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			mUiHelper.OnActivityResult (requestCode, (int)resultCode, data, new ShareDialogCallback ());
		}

		public override void OnResume ()
		{
			base.OnResume ();
			mUiHelper.OnResume ();
		}

		public override void OnPause ()
		{
			base.OnPause ();
			mUiHelper.OnPause ();
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			mUiHelper.OnDestroy ();
		}

		public override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);
			mUiHelper.OnSaveInstanceState (outState);
		}
			
		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.home_frag_layout, container, false);
			rootView.SetOnTouchListener (this);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];
		
			mGestureDetector = new GestureDetector (this);
			mDriverScoreIndicator = rootView.FindViewById<CircularIndicatorView> (Resource.Id.home_circularIndicator);
			mDriverScoreIndicator.SetIndicatorWidth (60);
			mDriverScoreIndicator.SetIndicatorValue (GamificationApp.GetInstance ().MyStatisticsManager.OverallScore);
			//mDriverScoreIndicator.SetOnTouchListener (this);
			mDriverScoreIndicator.Click += mScoreButton_onClick;
			//mDriverScoreIndicator.Click += mShareButton_onClick;

			mNotificationIndicator = rootView.FindViewById<ImageView> (Resource.Id.home_notificationIndicator);
			mNotificationIndicator.Click += mScoreButton_onClick;
			if (GamificationApp.GetInstance ().MyNotificationService.HasNotifications ()) {
				mNotificationIndicator.Visibility = ViewStates.Visible;
				AlphaAnimation animation = new AlphaAnimation (0, 1);
				animation.Duration = 1000;
				animation.Interpolator = new LinearInterpolator ();
				animation.RepeatCount = Animation.Infinite;
				animation.RepeatMode = RepeatMode.Reverse;
				mNotificationIndicator.StartAnimation (animation);
			}
			return rootView;
		}

		public bool OnTouch (View v, MotionEvent e)
		{
			return mGestureDetector.OnTouchEvent (e);
		}
			
		public bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
			double diffY = e1.GetY() - e2.GetY();
			double diffX = e1.GetX() - e2.GetX();

			if (Math.Abs (diffY) < SWIPE_MIN_DISTANCE) {
				if (diffX > SWIPE_MIN_DISTANCE && Math.Abs (velocityX) > SWIPE_THRESHOLD_VELOCITY) {
					((MainActivity)Activity).SelectFragment ((int) AbstractNavigationFragment.NavigationFragmentType.NAV_SCORE_BREAKDOWN);
				} else if (-diffX > SWIPE_MIN_DISTANCE && Math.Abs (velocityX) > SWIPE_THRESHOLD_VELOCITY) {
					((MainActivity)Activity).SelectFragment ((int) AbstractNavigationFragment.NavigationFragmentType.NAV_TRIP_HISTORY);
				}
			} else if (Math.Abs (diffX) < SWIPE_MIN_DISTANCE) {
				if (diffY > SWIPE_MIN_DISTANCE && Math.Abs (velocityY) > SWIPE_THRESHOLD_VELOCITY) {
					((MainActivity)Activity).SelectFragment ((int) AbstractNavigationFragment.NavigationFragmentType.NAV_BADGES);
				} else if (-diffY > SWIPE_MIN_DISTANCE && Math.Abs (velocityY) > SWIPE_THRESHOLD_VELOCITY) {
					((MainActivity)Activity).SelectFragment ((int) AbstractNavigationFragment.NavigationFragmentType.NAV_DIAGNOSTICS);
				}
			}
			return true;
		}
			
		public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) { return true; }
		public bool OnSingleTapUp(MotionEvent e) { return true; }
		public bool OnDown (MotionEvent e) { return true; }
		public void OnLongPress(MotionEvent e) {}
		public void OnShowPress(MotionEvent e) {}

		private void mScoreButton_onClick (object sender, EventArgs e)
		{
			if (GamificationApp.GetInstance ().MyNotificationService.HasNotifications ()) {
				mNotificationIndicator.ClearAnimation ();
				mNotificationIndicator.Visibility = ViewStates.Gone;
				DialogFragment notificationDialog = new NotificationDialogFragment ();
				notificationDialog.Show (FragmentManager, "notification");
			}
		}

		private void mShareButton_onClick (object sender, EventArgs e)
		{
			showShareDialog ();
		}

		private async Task showShareDialog ()
		{
			Badge b = GamificationApp.GetInstance ().MyAchievementManager.GetBadge (AchievementManager.BADGE_FIRST_TRIP_NAME);

			IOpenGraphObject badge = OpenGraphObjectFactory.CreateForPost ("mojiogamification:badge");
			badge.SetProperty ("title", b.GetDisplayName ());
			badge.SetProperty ("description", b.GetDescription ());

			IOpenGraphAction action = OpenGraphActionFactory.CreateForPost ("mojiogamification:unlock");
			action.SetProperty ("badge", (Java.Lang.Object) badge);

			Bitmap badgeIcon = ((BitmapDrawable) Resources.GetDrawable (b.GetDrawableResource ())).Bitmap;

			FacebookDialog.OpenGraphActionDialogBuilder builder = new FacebookDialog.OpenGraphActionDialogBuilder (this.Activity, action, "mojiogamification:unlock", "badge");
			builder.SetImageAttachmentsForAction (new List<Bitmap> { badgeIcon });
			FacebookDialog shareDialog = builder.Build ();
			mUiHelper.TrackPendingDialogCall (shareDialog.Present ());
		}
	}

	/*
	 * Callback handler for when share dialog closes and control returns to the calling app
	 */
	public class ShareDialogCallback : FacebookDialog.ICallback 
	{
		public void OnComplete (FacebookDialog.PendingCall pendingCall, Bundle data)
		{
			Logger.GetInstance ().Info ("ShareDialog successfully completed.");
		}

		public void OnError (FacebookDialog.PendingCall pendingCall, Java.Lang.Exception error, Bundle data)
		{
			Logger.GetInstance ().Error (String.Format("ShareDialog error - {0}", error));
		}

		public void Dispose ()
		{
			throw new NotImplementedException ();
		}
			
		public IntPtr Handle {
			get {
				throw new NotImplementedException ();
			}
		}
	}
}

