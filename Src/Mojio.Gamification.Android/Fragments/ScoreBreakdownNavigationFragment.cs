using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class ScoreBreakdownNavigationFragment : AbstractNavigationFragment
	{
		private LinearLayout mScoreBreakdownLayout;
		private LinearLayout mUserStatsLayout;

		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.scoring_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mScoreBreakdownLayout = rootView.FindViewById<LinearLayout> (Resource.Id.sbd_breakdownLayout);
			mUserStatsLayout = rootView.FindViewById<LinearLayout> (Resource.Id.sbd_statsLinearLayout);
		
			initializeScoreBreakdownPanel (rootView.Context);
			initializeStatsPanel (rootView.Context);
			return rootView;
		}

		private void initializeScoreBreakdownPanel(Context context) 
		{
			StatisticsManager statsManager = ((GamificationApp)(Activity.Application)).MyStatisticsManager;

			ScoreRowView overallScoreRow = new ScoreRowView (context);
			ScoreWrapper overallScore = ScoreWrapper.WrapScore (statsManager.OverallScore);
			overallScoreRow.SetScoreLabel ("OVERALL");
			overallScoreRow.SetScore (overallScore);

			ScoreRowView safetyScoreRow = new ScoreRowView (context);
			ScoreWrapper safetyScore = ScoreWrapper.WrapScore (statsManager.MyStats.safetyScore);
			safetyScoreRow.SetScoreLabel ("SAFETY");
			safetyScoreRow.SetScore (safetyScore);

			ScoreRowView efficiencyScoreRow = new ScoreRowView (context);
			ScoreWrapper efficiencyScore = ScoreWrapper.WrapScore (statsManager.MyStats.efficiencyScore);
			efficiencyScoreRow.SetScoreLabel ("EFFICIENCY");
			efficiencyScoreRow.SetScore (efficiencyScore);

			mScoreBreakdownLayout.AddView (overallScoreRow);
			mScoreBreakdownLayout.AddView (safetyScoreRow);
			mScoreBreakdownLayout.AddView (efficiencyScoreRow);
		}

		private void initializeStatsPanel(Context context)
		{
			UserStatsModel stats = ((GamificationApp)(Activity.Application)).MyStatisticsManager.MyStats;
			var totalHardEvents = stats.totalHardAccelerations + stats.totalHardBrakes + stats.totalHardLefts + stats.totalHardRights;

			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_totalTrips, stats.totalTrips, NumberDisplayer.NumberType.INTEGER));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_totalDistance, stats.totalDistance, NumberDisplayer.NumberType.DISTANCE));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_totalTime, stats.totalDuration, NumberDisplayer.NumberType.TIME));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_averageDistance, stats.totalDistance / stats.totalTrips, NumberDisplayer.NumberType.DISTANCE));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_averageDuration, stats.totalDuration / stats.totalTrips, NumberDisplayer.NumberType.TIME));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_totalHardAccelerations, stats.totalHardAccelerations, NumberDisplayer.NumberType.INTEGER));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_totalHardBrakes, stats.totalHardBrakes, NumberDisplayer.NumberType.INTEGER));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_totalHardLefts, stats.totalHardLefts, NumberDisplayer.NumberType.INTEGER));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_totalHardRights, stats.totalHardRights, NumberDisplayer.NumberType.INTEGER));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_hardEventFrequency, new NumberDisplayer (totalHardEvents / stats.totalDistance, NumberDisplayer.DEFAULT_DECIMAL_PLACES, ValueUnit.Per (ValueUnit.NULL_UNIT, ValueUnit.DISTANCE_KM))));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_totalAccidents, stats.totalAccidents, NumberDisplayer.NumberType.INTEGER));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_accidentFrequency, new NumberDisplayer (stats.totalAccidents / stats.totalDistance, NumberDisplayer.DEFAULT_DECIMAL_PLACES, ValueUnit.Per (ValueUnit.NULL_UNIT, ValueUnit.DISTANCE_KM)))); 
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_fuelEfficiency, new NumberDisplayer (stats.totalFuelConsumption * 100 / stats.totalDistance, NumberDisplayer.DEFAULT_DECIMAL_PLACES, ValueUnit.Per (ValueUnit.LITRE, ValueUnit.DISTANCE_100KM))));
			mUserStatsLayout.AddView (StatRowFactory.CreateRow (context, Resource.String.stat_fuelConsumption, stats.totalFuelConsumption, NumberDisplayer.NumberType.FUEL_CONSUMPTION));
		}
	}
}

