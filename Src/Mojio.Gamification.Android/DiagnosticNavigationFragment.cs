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
	public class DiagnosticNavigationFragment : AbstractNavigationFragment
	{

		private Button mConnectButton;
		private Button mFetchButton;
		private Button mRandomizeDataButton;
		private Button mResetDataButton;

		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.diagnostic_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mConnectButton = (Button) rootView.FindViewById<Button> (Resource.Id.diag_connectButton);
			mFetchButton = (Button) rootView.FindViewById<Button> (Resource.Id.diag_fetchButton);
			mRandomizeDataButton = (Button) rootView.FindViewById<Button> (Resource.Id.diag_randomDataButton);
			mResetDataButton = (Button) rootView.FindViewById<Button> (Resource.Id.diag_resetDataButton);

			mConnectButton.Click += mConnectButton_onClick;
			mFetchButton.Click += mFetchButton_onClick;
			mRandomizeDataButton.Click += mRandomizeDataButton_onClick;
			mResetDataButton.Click += mResetDataButton_onClick;

			return rootView;
		}

		private void mConnectButton_onClick (object sender, EventArgs e)
		{
			MojioConnectUtility.Connect ();
		}

		private void mFetchButton_onClick (object sender, EventArgs e)
		{

		}

		private void mRandomizeDataButton_onClick (object sender, EventArgs e) 
		{
			UserStats userStats = ((GamificationApp)this.Activity.Application).MyUserStatsRepository.GetUserStats ();
			Random rnd = new Random ();
			Type type = userStats.GetType ();
			System.Reflection.PropertyInfo[] properties = type.GetProperties ();
			foreach (System.Reflection.PropertyInfo property in properties) {
				if (property.Name != "uid") {
					property.SetValue (userStats, rnd.Next (0, 100));
				}
			}
			((GamificationApp)this.Activity.Application).MyUserStatsRepository.UpdateUserStats (userStats);
			userStats = ((GamificationApp)this.Activity.Application).MyUserStatsRepository.GetUserStats ();
			rnd.Next ();
		}

		private void mResetDataButton_onClick (object sender, EventArgs e)
		{
			UserStats userStats = ((GamificationApp)this.Activity.Application).MyUserStatsRepository.GetUserStats ();
			Random rnd = new Random ();
			Type type = userStats.GetType ();
			System.Reflection.PropertyInfo[] properties = type.GetProperties ();
			foreach (System.Reflection.PropertyInfo property in properties) {
				if (property.Name != "uid") {
					property.SetValue (userStats, 0);
				}
			}
			((GamificationApp)this.Activity.Application).MyUserStatsRepository.UpdateUserStats (userStats);
		}
			
	}
}

