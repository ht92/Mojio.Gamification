
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
	abstract public class NavigationFragment : Fragment
	{
		public const string ARG_FRAG_NUMBER = "frag_num";
		public NavigationFragment ()
		{
		}

		public static Fragment NewInstance(int position)
		{
			Fragment fragment;
			switch (position) {
			case 1: 
				fragment = new ScoreBreakdownNavigationFragment ();
				break;
			default:
				fragment = new HomeNavigationFragment ();
				break;
			}
			Bundle args = new Bundle();
			args.PutInt(NavigationFragment.ARG_FRAG_NUMBER, position);
			fragment.Arguments = args;
			return fragment;
		}

		public abstract override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState);
	}
}

