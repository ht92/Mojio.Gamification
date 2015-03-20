using Android.App;
using Android.OS;
using Android.Views;

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
			NAV_TRIP_HISTORY,
			NAV_ABOUT,
			NAV_DIAGNOSTICS
		};

		protected AbstractNavigationFragment ()
		{
			attachListeners ();
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
			case NavigationFragmentType.NAV_TRIP_HISTORY:
				fragment = new TripHistoryNavigationFragment ();
				break;
			case NavigationFragmentType.NAV_ABOUT:
				fragment = new AboutNavigationFragment ();
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

		protected void attachListeners ()
		{
			GamificationApp.GetInstance ().MyNotificationService.NotificationEvent += (sender, e) => {
				if (this.IsAdded && this.IsVisible) {
					FragmentTransaction fragTransaction = FragmentManager.BeginTransaction ();
					fragTransaction.Detach (this);
					fragTransaction.Attach (this);
					fragTransaction.Commit ();
				}
			};
		}
	}
}

