using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;

using Mojio.Events;

namespace Mojio.Gamification.Android
{
	public class AboutNavigationFragment : AbstractNavigationFragment
	{
		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.about_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];
			return rootView;
		}
	}
}

