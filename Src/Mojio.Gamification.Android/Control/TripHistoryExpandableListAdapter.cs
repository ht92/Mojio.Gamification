using System;
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class TripHistoryExpandableListAdapter : AbstractExpandableListAdapter<TripDataModel>
	{
		public TripHistoryExpandableListAdapter (Context context, List<TripDataModel> listData)
			: base (context, listData)
		{
		}
			
		public override View GetGroupView (int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			if (convertView == null) {
				LayoutInflater li = (LayoutInflater)mContext.GetSystemService (Context.LayoutInflaterService);
				convertView = li.Inflate (Resource.Layout.trip_history_list_group, null);
			}
			ScoreRowView tripRecordHeader = convertView.FindViewById<ScoreRowView> (Resource.Id.tripRecordHeader);
			TripDataModel dataModel = mData [groupPosition];
			ScoreWrapper overallScore = ScoreWrapper.WrapScore (ScoreCalculator.CalculateOverallScore (dataModel.TripSafetyScore, dataModel.TripEfficiencyScore));
			tripRecordHeader.SetScoreLabel (dataModel.MyTrip.StartTime.ToLocalTime ().ToString (DateTimeUtility.FORMAT_MMM_DD_YYYY_H_MM_TT));
			tripRecordHeader.SetScore (overallScore);
			return convertView;
		}

		public override View GetChildView (int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			return createChildView (mData [groupPosition]);
		}

		private LinearLayout createChildView (TripDataModel dataModel)
		{
			LinearLayout tripRecordChildView = createChildViewLayout ();
			populateChildView (tripRecordChildView, dataModel);
			return tripRecordChildView;
		}

		private LinearLayout createChildViewLayout ()
		{
			LinearLayout layout = new LinearLayout (mContext);
			layout.Orientation = Orientation.Vertical;
			int padding = (int) TypedValue.ApplyDimension (ComplexUnitType.Dip, 16, mContext.Resources.DisplayMetrics);
			layout.SetPadding (padding, 0, padding, 0);
			return layout;
		}

		private void populateChildView (LinearLayout layout, TripDataModel dataModel)
		{
			ScoreRowView tripRecordSafetyScore = new ScoreRowView (mContext);
			ScoreRowView tripRecordEfficiencyScore = new ScoreRowView (mContext);
			ScoreWrapper safetyScore = ScoreWrapper.WrapScore (dataModel.TripSafetyScore);
			ScoreWrapper efficiencyScore = ScoreWrapper.WrapScore (dataModel.TripEfficiencyScore);
			tripRecordSafetyScore.SetScoreLabel ("SAFETY");
			tripRecordSafetyScore.SetScore (safetyScore);
			tripRecordEfficiencyScore.SetScoreLabel ("EFFICIENCY");
			tripRecordEfficiencyScore.SetScore (efficiencyScore);
			layout.AddView (tripRecordSafetyScore);
			layout.AddView (tripRecordEfficiencyScore);

			UserStatsModel stats = dataModel.GetTripStats ();
			layout.AddView (StatRowFactory.CreateRow (mContext, Resource.String.stat_totalDistance, stats.totalDistance, NumberDisplayer.NumberType.DISTANCE));
			layout.AddView (StatRowFactory.CreateRow (mContext, Resource.String.stat_totalTime, stats.totalDuration, NumberDisplayer.NumberType.TIME));
			layout.AddView (StatRowFactory.CreateRow (mContext, Resource.String.stat_totalHardAccelerations, stats.totalHardAccelerations, NumberDisplayer.NumberType.INTEGER));
			layout.AddView (StatRowFactory.CreateRow (mContext, Resource.String.stat_totalHardBrakes, stats.totalHardBrakes, NumberDisplayer.NumberType.INTEGER));
			layout.AddView (StatRowFactory.CreateRow (mContext, Resource.String.stat_totalHardLefts, stats.totalHardLefts, NumberDisplayer.NumberType.INTEGER));
			layout.AddView (StatRowFactory.CreateRow (mContext, Resource.String.stat_totalHardRights, stats.totalHardRights, NumberDisplayer.NumberType.INTEGER));
			layout.AddView (StatRowFactory.CreateRow (mContext, Resource.String.stat_totalAccidents, stats.totalAccidents, NumberDisplayer.NumberType.INTEGER));
			layout.AddView (StatRowFactory.CreateRow (mContext, Resource.String.stat_accidentFrequency, new NumberDisplayer (stats.totalAccidents / stats.totalDistance, NumberDisplayer.DEFAULT_DECIMAL_PLACES, ValueUnit.Per (ValueUnit.NULL_UNIT, ValueUnit.DISTANCE_KM)))); 
			layout.AddView (StatRowFactory.CreateRow (mContext, Resource.String.stat_fuelEfficiency, new NumberDisplayer (stats.totalFuelConsumption * 100 / stats.totalDistance, NumberDisplayer.DEFAULT_DECIMAL_PLACES, ValueUnit.Per (ValueUnit.LITRE, ValueUnit.DISTANCE_100KM))));
			layout.AddView (StatRowFactory.CreateRow (mContext, Resource.String.stat_fuelConsumption, stats.totalFuelConsumption, NumberDisplayer.NumberType.FUEL_CONSUMPTION));
		}
	}
}

