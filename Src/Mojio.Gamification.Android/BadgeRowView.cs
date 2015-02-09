using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

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
			SetBadgeLabel (badge.GetDisplayName ());
		}

		private void SetBadgeImage (int resid)
		{
			if (mBadgeView == null) {
				mBadgeView = FindViewById<BadgeView> (Resource.Id.br_badgeView);
			}
			mBadgeView.SetImageResource (resid);
		}

		private void SetBadgeLabel (string label)
		{
			if (mBadgeLabel == null) {
				mBadgeLabel = FindViewById<TextView> (Resource.Id.br_badgeLabel);
			}
			mBadgeLabel.Text = label; 
		}
	}
}

