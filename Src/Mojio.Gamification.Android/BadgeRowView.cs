using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class BadgeRowView : LinearLayout
	{
		private BadgeView mBadgeView;
		private TextView mBadgeLabel;

		public BadgeRowView (Context context) :
		base (context)
		{
			LayoutInflater.From (context).Inflate (Resource.Layout.badge_row_layout, this);
		}

		public BadgeRowView (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			initViews (context, attrs);
		}

		public BadgeRowView (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			initViews (context, attrs);
		}

		private void initViews(Context context, IAttributeSet attrs)
		{
			LayoutInflater.From (context).Inflate (Resource.Layout.badge_row_layout, this);
		}

		public void SetBadge (Badge badge)
		{
			SetBadgeImage (badge.GetDrawableResource ());
			SetBadgeLabel (badge.GetName ());
		}

		private void SetBadgeImage (int resid)
		{
			if (mBadgeView == null) {
				mBadgeView = (BadgeView)FindViewById<BadgeView> (Resource.Id.br_badgeView);
			}
			mBadgeView.SetImageResource (resid);
		}

		private void SetBadgeLabel (string label)
		{
			if (mBadgeLabel == null) {
				mBadgeLabel = (TextView)FindViewById<TextView> (Resource.Id.br_badgeLabel);
			}
			mBadgeLabel.Text = label; 
		}
	}
}

