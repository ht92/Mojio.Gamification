using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class HomeNavigationFragment : AbstractNavigationFragment, View.IOnTouchListener, GestureDetector.IOnGestureListener
	{
		private const int SWIPE_MIN_DISTANCE = 200;
		private const int SWIPE_THRESHOLD_VELOCITY = 200;
		private GestureDetector mGestureDetector;
		private CircularIndicatorView mDriverScoreIndicator;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
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
			mDriverScoreIndicator = (CircularIndicatorView) rootView.FindViewById<CircularIndicatorView> (Resource.Id.home_circularIndicator);
			mDriverScoreIndicator.SetIndicatorWidth (60);
			mDriverScoreIndicator.SetIndicatorValue (((GamificationApp) (Activity.Application)).MyStatisticsManager.OverallScore);
			mDriverScoreIndicator.SetOnTouchListener (this);

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
	}
}

