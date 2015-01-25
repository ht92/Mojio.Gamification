﻿using System;
using System.Collections.Generic;
using Android;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class TripHistoryExpandableListAdapter : BaseExpandableListAdapter
	{
		private Context mContext;
		private List<TripDataModel> mListData;
	
		public TripHistoryExpandableListAdapter (Context context, List<TripDataModel> listData)
		{
			mContext = context;
			mListData = listData;
		}

		public override Java.Lang.Object GetChild (int groupPosition, int childPosition)
		{
			return null;
		}

		public override long GetChildId (int groupPosition, int childPosition)
		{
			return childPosition;
		}

		public override View GetChildView (int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			if (convertView == null) {
				LayoutInflater li = (LayoutInflater) mContext.GetSystemService (Context.LayoutInflaterService);
				convertView = li.Inflate (Resource.Layout.trip_history_list_item, null);
			}
			ScoreRowView tripRecordSafetyScore = (ScoreRowView) convertView.FindViewById<ScoreRowView> (Resource.Id.tripRecordListItem_safetyScore);
			ScoreRowView tripRecordEfficiencyScore = (ScoreRowView) convertView.FindViewById<ScoreRowView> (Resource.Id.tripRecordListItem_efficiencyScore);
			TripDataModel dataModel = mListData [groupPosition];
			ScoreWrapper safetyScore = ScoreWrapper.WrapScore (dataModel.TripSafetyScore);
			ScoreWrapper efficiencyScore = ScoreWrapper.WrapScore (dataModel.TripEfficiencyScore);
			tripRecordSafetyScore.SetScoreLabel ("SAFETY");
			tripRecordSafetyScore.SetScore (safetyScore.Score);
			tripRecordSafetyScore.SetRankLabel (String.Format ("RANK {0}", safetyScore.Rank));
			tripRecordEfficiencyScore.SetScoreLabel ("EFFICIENCY");
			tripRecordEfficiencyScore.SetScore (efficiencyScore.Score);
			tripRecordEfficiencyScore.SetRankLabel (String.Format ("RANK {0}", efficiencyScore.Rank));
			return convertView;
		}

		public override int GetChildrenCount (int groupPosition)
		{
			return 1;
		}

		public override Java.Lang.Object GetGroup (int groupPosition)
		{
			return null;
		}

		public override int GroupCount {
			get {
				return mListData.Count;
			}
		}

		public override long GetGroupId (int groupPosition)
		{
			return groupPosition;
		}

		public override View GetGroupView (int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			if (convertView == null) {
				LayoutInflater li = (LayoutInflater)mContext.GetSystemService (Context.LayoutInflaterService);
				convertView = li.Inflate (Resource.Layout.trip_history_list_group, null);
			}
			ScoreRowView tripRecordHeader = (ScoreRowView) convertView.FindViewById<ScoreRowView> (Resource.Id.tripRecordHeader);
			TripDataModel dataModel = mListData [groupPosition];
			ScoreWrapper overallScore = ScoreWrapper.WrapScore (ScoreCalculator.CalculateOverallScore (dataModel.TripSafetyScore, dataModel.TripEfficiencyScore));
			tripRecordHeader.SetScoreLabel (dataModel.MyTrip.StartTime.ToString ("MMMM dd, yyyy h:mm tt"));
			tripRecordHeader.SetScore (overallScore.Score);
			tripRecordHeader.SetRankLabel (String.Format ("RANK {0}", overallScore.Rank));
			return convertView;
		}

		public override bool HasStableIds {
			get {
				return false;
			}
		}

		public override bool IsChildSelectable (int groupPosition, int childPosition)
		{
			return false;
		}
	}
}
