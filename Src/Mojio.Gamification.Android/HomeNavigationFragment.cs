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

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class HomeNavigationFragment : AbstractNavigationFragment
	{

		private Button mScoreButton;
		private UserStatsRepository mUserStatsRepository;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			mUserStatsRepository = ((GamificationApp)this.Activity.Application).MyUserStatsRepository;
		}

		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.home_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mScoreButton = (Button) rootView.FindViewById<Button> (Resource.Id.ScoreButton);
			UserStats stats = mUserStatsRepository.GetUserStats ();
			ScoreWrapper safetyScore = ScoreWrapper.WrapScore (stats.safetyScore);
			ScoreWrapper efficiencyScore = ScoreWrapper.WrapScore (stats.efficiencyScore);
			ScoreWrapper overallScore = ScoreCalculator.CalculateOverallScore (new List<ScoreWrapper> {safetyScore, efficiencyScore});
			mScoreButton.Text = overallScore.Score.ToString ();
			mScoreButton.Click += mScoreButton_onClick;

			return rootView;
		}

		private void mScoreButton_onClick (object sender, EventArgs e)
		{
			((MainActivity)Activity).OnClick ((View)sender, (int)NavigationFragmentType.NAV_SCORE_BREAKDOWN);
		}

	}
}

