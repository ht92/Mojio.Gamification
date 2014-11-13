
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
using Android.Graphics;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class ScoreBreakdownNavigationFragment : AbstractNavigationFragment
	{

		private UserStatsRepository mUserStatsRepository;
		private UserStats mUserStats;
		private LinearLayout mScoreBreakdownLayout;
		private LinearLayout mUserStatsLayout;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			mUserStatsRepository = ((GamificationApp)this.Activity.Application).MyUserStatsRepository;
			mUserStats = mUserStatsRepository.GetUserStats ();
		}

		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.scoring_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mScoreBreakdownLayout = (LinearLayout) rootView.FindViewById<LinearLayout> (Resource.Id.sbd_breakdownLayout);
			mUserStatsLayout = (LinearLayout)rootView.FindViewById<LinearLayout> (Resource.Id.sbd_statsLinearLayout);
		
			initializeScoreBreakdownPanel (rootView.Context);
			initializeStatsPanel (rootView.Context);
			return rootView;
		}

		private void initializeScoreBreakdownPanel(Context context) 
		{
			ScoreWrapper score = ScoreWrapper.wrapScore (mUserStats.overallScore);

			ScoreRowView overallScoreRow = new ScoreRowView (context);
			overallScoreRow.SetScoreLabel ("OVERALL");
			overallScoreRow.SetScore (score.Score);
			overallScoreRow.SetRankLabel ("RANK " + score.Rank.ToString ());

			ScoreRowView safetyScoreRow = new ScoreRowView (context);
			safetyScoreRow.SetScoreLabel ("SAFETY");
			safetyScoreRow.SetScore (score.Score);
			safetyScoreRow.SetRankLabel ("RANK " + score.Rank.ToString ());

			ScoreRowView efficiencyScoreRow = new ScoreRowView (context);
			efficiencyScoreRow.SetScoreLabel ("EFFICIENCY");
			efficiencyScoreRow.SetScore (score.Score);
			efficiencyScoreRow.SetRankLabel ("RANK " + score.Rank.ToString ());

			mScoreBreakdownLayout.AddView (overallScoreRow);
			mScoreBreakdownLayout.AddView (safetyScoreRow);
			mScoreBreakdownLayout.AddView (efficiencyScoreRow);
		}

		private void initializeStatsPanel(Context context)
		{
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Distance Travelled", mUserStats.totalDistance.ToString ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Time Driven", mUserStats.totalDuration.ToString ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Speeding Events", mUserStats.numSpeeding.ToString ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Speeding Distance", mUserStats.distanceSpeeding.ToString ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Harsh Events", mUserStats.numHarshEvents.ToString ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Idle Time", mUserStats.totalIdleTime.ToString ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Fuel Consumption", mUserStats.totalFuelConsumption.ToString ()));

		}

		private LinearLayout createUserStatRow(Context context, string label, string value)
		{
			LinearLayout rowLayout = new LinearLayout(context);
			rowLayout.Orientation = Orientation.Horizontal;

			TextView statLabelTextView = new TextView (context);
			statLabelTextView.Text = label + ":";
			statLabelTextView.SetAllCaps (true);
			statLabelTextView.SetTypeface (null, TypefaceStyle.Bold);
			statLabelTextView.Gravity = GravityFlags.Left | GravityFlags.CenterVertical;
			statLabelTextView.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent, 3);

			TextView statValueTextView = new TextView (context);
			statValueTextView.Text = value;
			statValueTextView.SetTypeface (null, TypefaceStyle.Bold);
			statValueTextView.Gravity = GravityFlags.Right | GravityFlags.CenterVertical;

			rowLayout.AddView (statLabelTextView);
			rowLayout.AddView (statValueTextView); 

			return rowLayout;
		}
	}
}

