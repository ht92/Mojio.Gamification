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
	public class BadgeNavigationFragment : AbstractNavigationFragment
	{
		private BadgeCollectionExpandableListAdapter mBadgeCollectionExpandableListAdapter;
		private ExpandableListView mBadgeCollectionExpandableListView;
		private List<Badge> mListData;

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
			View rootView = inflater.Inflate(Resource.Layout.badge_collection_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mBadgeCollectionExpandableListView = (ExpandableListView) rootView.FindViewById<ExpandableListView> (Resource.Id.badgeCollection_expandableListView);
			prepareListData ();
			mBadgeCollectionExpandableListAdapter = new BadgeCollectionExpandableListAdapter (rootView.Context, mListData);
			mBadgeCollectionExpandableListView.SetAdapter (mBadgeCollectionExpandableListAdapter);
			return rootView;
		}

		private void prepareListData ()
		{
			mListData = new List<Badge> ();
			List<Badge> unlockedBadges = ((GamificationApp)Activity.Application).MyAchievementManager.GetUnlockedBadgeCollection ();
			foreach (Badge badge in unlockedBadges) {
				mListData.Add (badge);
			}
		}
	}
}

