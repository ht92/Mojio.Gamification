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

namespace Mojio.Gamification.Android
{
	public class ScoreRowView : LinearLayout
	{
		private TextView mScoreLabelTextView;
		private CircularIndicatorView mScoreIndicatorView;
		private TextView mRankLabelTextView;

		public ScoreRowView (Context context) :
			base (context)
		{
			LayoutInflater.From (context).Inflate (Resource.Layout.score_row_layout, this);
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
			TypedArray arr = context.Theme.ObtainStyledAttributes (attrs, Resource.Styleable.ScoreRowView, 0, 0);
			string scoreLabelText = arr.GetString (Resource.Styleable.ScoreRowView_scoreLabel);
			string rankLabelText = arr.GetString (Resource.Styleable.ScoreRowView_rankLabel);
			arr.Recycle ();

			LayoutInflater.From (context).Inflate (Resource.Layout.score_row_layout, this);

			SetScoreLabel (scoreLabelText);
			SetRankLabel (rankLabelText);
		}

		public void SetScoreLabel(string label)
		{
			if (mScoreLabelTextView == null) {
				mScoreLabelTextView = (TextView)FindViewById<TextView> (Resource.Id.sr_scoreLabelTextView);
			}
			mScoreLabelTextView.Text = label;
		}

		public void SetRankLabel(string label)
		{
			if (mRankLabelTextView == null) {
				mRankLabelTextView = (TextView)FindViewById<TextView> (Resource.Id.sr_rankLabelTextView);
			}
			mRankLabelTextView.Text = label;
		}

		public void SetScore(double score)
		{
			if (mScoreIndicatorView == null) {
				mScoreIndicatorView = (CircularIndicatorView)FindViewById<CircularIndicatorView> (Resource.Id.sr_scoreIndicator);
			}
			mScoreIndicatorView.SetIndicatorValue (Math.Round(score, MidpointRounding.AwayFromZero));
		}
	}
}

