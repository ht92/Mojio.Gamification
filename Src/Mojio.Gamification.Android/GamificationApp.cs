using System;
using Android.App;
using Android.Runtime;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	[Application]
	public class GamificationApp : Application
	{
		public UserStatsRepository MyUserStatsRepository { get; set; }
		public UserBadgeRepository MyUserBadgeRepository { get; set; }
		public TripRecordRepository MyTripRecordRepository { get; set; }

		public StatisticsManager MyStatisticsManager { get; set; }
		public AchievementManager MyAchievementManager { get; set; }

		public AppNotificationService MyNotificationService { get; set; }

		public MojioConnectUtility MyMojioConnectUtility { get; set; }

		private static GamificationApp _instance;

		public GamificationApp(IntPtr javaReference, JniHandleOwnership transfer)
			: base (javaReference, transfer)
		{
		}

		public static GamificationApp GetInstance ()
		{
			return _instance;
		}

		public override void OnCreate()
		{
			base.OnCreate ();
			_instance = this;
			Initialize ();
		}

		public void Initialize ()
		{
			MyUserStatsRepository = UserStatsRepository.GetInstance ();
			MyUserBadgeRepository = UserBadgeRepository.GetInstance ();
			MyTripRecordRepository = TripRecordRepository.GetInstance ();
			MyStatisticsManager = StatisticsManager.GetInstance ();
			MyAchievementManager = AchievementManager.GetInstance ();
			MyMojioConnectUtility = MojioConnectUtility.GetInstance ();
			MyNotificationService = AppNotificationService.GetInstance ();
		}
	}
}

