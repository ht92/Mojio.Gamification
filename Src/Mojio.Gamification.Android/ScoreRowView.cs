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
	public class ScoreRowView : LinearLayout
	{
		private static string RANK_LABEL = "RANK";
		private TextView mScoreLabelTextView;
		private CircularIndicatorView mScoreIndicatorView;
		private TextView mRankLabelTextView;

		public ScoreRowView (Context context) :
			base (context)
		{
			initViews (context, null);
		}

		public ScoreRowView (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			initViews (context, attrs);
		}

		public ScoreRowView (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			initViews (context, attrs);
		}

		private void initViews(Context context, IAttributeSet attrs)
		{
			LayoutInflater.From (context).Inflate (Resource.Layout.score_row_layout, this);
		}

		public void SetScoreLabel(string label)
		{
			if (mScoreLabelTextView == null) {
				mScoreLabelTextView = FindViewById<TextView> (Resource.Id.sr_scoreLabelTextView);
			}
			mScoreLabelTextView.Text = label;
		}

		public void SetScore(ScoreWrapper score)
		{
			if (mScoreIndicatorView == null) {
				mScoreIndicatorView = FindViewById<CircularIndicatorView> (Resource.Id.sr_scoreIndicator);
			}
			if (mRankLabelTextView == null) {
				mRankLabelTextView = FindViewById<TextView> (Resource.Id.sr_rankLabelTextView);
			}
			mScoreIndicatorView.SetIndicatorValue (Math.Round(score.Score, MidpointRounding.AwayFromZero));
			mRankLabelTextView.Text = String.Format ("{0} {1}", RANK_LABEL, score.Rank);
		}
	}
}

