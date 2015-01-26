using System;
using System.Collections.Generic;
using Android;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class BadgeCollectionExpandableListAdapter : BaseExpandableListAdapter
	{
		private Context mContext;
		private List<Badge> mListData;

		public BadgeCollectionExpandableListAdapter (Context context, List<Badge> listData)
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
				convertView = li.Inflate (Resource.Layout.badge_collection_item, null);
			}
			TextView descriptionView = (TextView)convertView.FindViewById<TextView> (Resource.Id.badgeRowDescription);
			TextView unlockDateView = (TextView)convertView.FindViewById<TextView> (Resource.Id.badgeRowUnlockDate);
			Badge badge = mListData [groupPosition];
			descriptionView.Text = badge.GetDescription ();
			unlockDateView.Text = String.Format ("Unlocked on {0}", badge.GetUnlockDate ().Value.ToString (DateTimeUtility.FORMAT_MMMM_DD_YYYY_H_MM_TT));
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
				convertView = li.Inflate (Resource.Layout.badge_collection_group, null);
			}
			Badge badge = mListData [groupPosition];
			BadgeRowView badgeRowView = (BadgeRowView)convertView.FindViewById<BadgeRowView> (Resource.Id.badgeRowHeader);
			badgeRowView.SetBadge (badge);
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

