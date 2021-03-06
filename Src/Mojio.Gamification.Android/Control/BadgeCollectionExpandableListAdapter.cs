﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class BadgeCollectionExpandableListAdapter : AbstractExpandableListAdapter<Badge>
	{
		private BadgeNavigationFragment mParentFragment;
		private bool mShareEnabled = false;

		public BadgeCollectionExpandableListAdapter (Context context, List<Badge> data)
			: base (context, data)
		{
		}

		public BadgeCollectionExpandableListAdapter (BadgeNavigationFragment parent, Context context, List<Badge> data)
			: base (context, data)
		{
			mParentFragment = parent;
			mShareEnabled = true;
		}

		public override int GetChildrenCount (int groupPosition)
		{
			return mData [groupPosition].IsUnlocked () ? base.GetChildrenCount (groupPosition) : 0;
		}

		public override View GetChildView (int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			if (convertView == null) {
				LayoutInflater li = (LayoutInflater) mContext.GetSystemService (Context.LayoutInflaterService);
				convertView = li.Inflate (Resource.Layout.badge_collection_item, null);
			}
			Badge badge = mData [groupPosition];
			TextView descriptionView = convertView.FindViewById<TextView> (Resource.Id.badgeRowDescription);
			TextView unlockDateView = convertView.FindViewById<TextView> (Resource.Id.badgeRowUnlockDate);
			Button shareButton = convertView.FindViewById<Button> (Resource.Id.badgeRowShareButton);
			unlockDateView.Text = String.Format ("Unlocked on {0}", badge.GetUnlockDate ().Value.ToString (DateTimeUtility.FORMAT_MMMM_DD_YYYY));
			descriptionView.Text = badge.GetDescription ();
			shareButton.Click += (sender, e) => {
				if (mShareEnabled && mParentFragment != null) {
					mParentFragment.ShareBadge (badge);
				}
			};
			return convertView;
		}

		public override View GetGroupView (int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			if (convertView == null) {
				LayoutInflater li = (LayoutInflater)mContext.GetSystemService (Context.LayoutInflaterService);
				convertView = li.Inflate (Resource.Layout.badge_collection_group, null);
			}
			Badge badge = mData [groupPosition];
			BadgeRowView badgeRowView = convertView.FindViewById<BadgeRowView> (Resource.Id.badgeRowHeader);
			ImageView badgeLockedView = convertView.FindViewById<ImageView> (Resource.Id.badgeRowLock);
			badgeRowView.SetBadge (badge);
			if (badge.IsUnlocked ()) {
				badgeRowView.Visibility = ViewStates.Visible;
				badgeLockedView.Visibility = ViewStates.Invisible;
			} else {
				badgeRowView.Visibility = ViewStates.Invisible;
				badgeLockedView.Visibility = ViewStates.Visible;
			}
			return convertView;
		}
	}
}

