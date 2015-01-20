
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
	abstract public class AbstractNavigationFragment : Fragment
	{
		public const string ARG_FRAG_NUMBER = "frag_num";

		public enum NavigationFragmentType 
		{
			NAV_HOME,
			NAV_SCORE_BREAKDOWN,
			NAV_BADGES,
			NAV_HELP,
			NAV_DIAGNOSTICS
		};

		public AbstractNavigationFragment ()
		{
		}

		public static AbstractNavigationFragment NewInstance(NavigationFragmentType type)
		{
			AbstractNavigationFragment fragment;
			switch (type) {
			case NavigationFragmentType.NAV_SCORE_BREAKDOWN: 
				fragment = new ScoreBreakdownNavigationFragment ();
				break;
			case NavigationFragmentType.NAV_DIAGNOSTICS:
				fragment = new DiagnosticNavigationFragment ();
				break;
			case NavigationFragmentType.NAV_BADGES:
				fragment = new BadgeNavigationFragment ();
				break;
			default:
				fragment = new HomeNavigationFragment ();
				break;
			}
			Bundle args = new Bundle();
			args.PutInt(AbstractNavigationFragment.ARG_FRAG_NUMBER, (int) type);
			fragment.Arguments = args;
			return fragment;
		}

		public abstract override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState);

	}
}

