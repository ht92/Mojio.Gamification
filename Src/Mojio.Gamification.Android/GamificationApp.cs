using System;
using System.Collections.Generic;
using Android.App;
using Android.Runtime;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	[Application]
	public class GamificationApp : Application
	{
		public event EventHandler InitializationCompleteEvent;

		public ConnectionService MyConnectionService { get; set; }
		public AppNotificationService MyNotificationService { get; set; }

		public DataManagerHelper MyDataManagerHelper { get; set; }
		public UserStatsRepository MyUserStatsRepository { get; set; }
		public UserBadgesRepository MyUserBadgesRepository { get; set; }
		public UserTripRecordsRepository MyUserTripRecordsRepository { get; set; }

		public StatisticsManager MyStatisticsManager { get; set; }
		public AchievementManager MyAchievementManager { get; set; }
		public TripHistoryManager MyTripHistoryManager { get; set; }

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
			initializeServices ();
			attachListeners ();
		}

		private void attachListeners ()
		{
			MyConnectionService.LoginEvent += (sender, e) => {
				if (e.IsSuccess) {
					initializeDatabase ();
					if (!MyUserStatsRepository.DoesUserExist (MyConnectionService.CurrentUserName)) {
						initializeNewUser ();
					}
					initializeManagers ();
					OnInitializationCompleteEvent ();
				}
			};
		}

		private void initializeServices ()
		{
			MyConnectionService = ConnectionService.GetInstance ();
			MyNotificationService = AppNotificationService.GetInstance ();
		}

		private void initializeDatabase ()
		{
			MyDataManagerHelper = DataManagerHelper.GetInstance ();
			MyUserStatsRepository = UserStatsRepository.GetInstance ();
			MyUserBadgesRepository = UserBadgesRepository.GetInstance ();
			MyUserTripRecordsRepository = UserTripRecordsRepository.GetInstance ();
		}

		private void initializeNewUser ()
		{
			MyUserStatsRepository.AddUserStats (UserStats.CreateStats (MyConnectionService.CurrentUserName));
			MyUserBadgesRepository.UpdateBadges (JsonSerializationUtility.Serialize (AchievementManager.DEFAULT_BADGE_COLLECTION));
			MyUserTripRecordsRepository.UpdateTripRecords (JsonSerializationUtility.Serialize (TripHistoryManager.CreateNewHistory ()));
		}

		private void initializeManagers () 
		{
			MyStatisticsManager = StatisticsManager.GetInstance ();
			MyAchievementManager = AchievementManager.GetInstance ();
			MyTripHistoryManager = TripHistoryManager.GetInstance ();
		}

		private void OnInitializationCompleteEvent ()
		{
			InitializationCompleteEvent (this, EventArgs.Empty);
		}
	}
}

