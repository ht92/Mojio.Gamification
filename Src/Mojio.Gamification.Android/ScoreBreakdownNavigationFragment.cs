
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

namespace Mojio.Gamification.Android
{
	public class ScoreBreakdownNavigationFragment : NavigationFragment
	{
		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.scoring_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];
			return rootView;
		}
	}
}

