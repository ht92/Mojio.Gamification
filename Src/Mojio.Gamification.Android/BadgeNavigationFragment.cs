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
		private LinearLayout mBadgeCollectionLayout;

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
			View rootView = inflater.Inflate(Resource.Layout.badge_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mBadgeCollectionLayout = (LinearLayout) rootView.FindViewById<LinearLayout> (Resource.Id.badge_collectionLayout);
			initializeBadgeList (rootView.Context);
			return rootView;
		}

		private void initializeBadgeList (Context context)
		{
			List<Badge> unlockedBadges = ((GamificationApp)Activity.Application).MyAchievementManager.GetUnlockedBadgeCollection ();
			foreach (Badge badge in unlockedBadges) {
				BadgeRowView badgeRowView = new BadgeRowView (context);
				badgeRowView.SetBadge (badge);
				mBadgeCollectionLayout.AddView (badgeRowView);
			}
		}
	}
}

