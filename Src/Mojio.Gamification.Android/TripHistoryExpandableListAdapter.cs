using System;
using System.Collections.Generic;
using Android.Content;
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
			
		public override View GetChildView (int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			if (convertView == null) {
				LayoutInflater li = (LayoutInflater) mContext.GetSystemService (Context.LayoutInflaterService);
				convertView = li.Inflate (Resource.Layout.trip_history_list_item, null);
			}
			ScoreRowView tripRecordSafetyScore = convertView.FindViewById<ScoreRowView> (Resource.Id.tripRecordListItem_safetyScore);
			ScoreRowView tripRecordEfficiencyScore = convertView.FindViewById<ScoreRowView> (Resource.Id.tripRecordListItem_efficiencyScore);
			TripDataModel dataModel = mData [groupPosition];
			ScoreWrapper safetyScore = ScoreWrapper.WrapScore (dataModel.TripSafetyScore);
			ScoreWrapper efficiencyScore = ScoreWrapper.WrapScore (dataModel.TripEfficiencyScore);
			tripRecordSafetyScore.SetScoreLabel ("SAFETY");
			tripRecordSafetyScore.SetScore (safetyScore);
			tripRecordEfficiencyScore.SetScoreLabel ("EFFICIENCY");
			tripRecordEfficiencyScore.SetScore (efficiencyScore);
			return convertView;
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
			tripRecordHeader.SetScoreLabel (dataModel.MyTrip.StartTime.ToString ("MMM dd, yyyy h:mm tt"));
			tripRecordHeader.SetScore (overallScore);
			return convertView;
		}
	}
}

