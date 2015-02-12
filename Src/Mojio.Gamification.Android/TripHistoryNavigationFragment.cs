using System.Collections.Generic;

using Android.OS;
using Android.Views;
using Android.Widget;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class TripHistoryNavigationFragment : AbstractNavigationFragment
	{
		private TripHistoryExpandableListAdapter mTripHistoryListAdapter;
		private ExpandableListView mTripHistoryList;
		private List<TripDataModel> mListData;

		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.trip_history_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mTripHistoryList = rootView.FindViewById<ExpandableListView> (Resource.Id.tripHistory_expandableListView);
			prepareListData ();
			mTripHistoryListAdapter = new TripHistoryExpandableListAdapter (rootView.Context, mListData);
			mTripHistoryList.SetAdapter (mTripHistoryListAdapter);
			return rootView;
		}

		private void prepareListData ()
		{
			mListData = new List<TripDataModel> ();
			List<TripDataModel> tripRecords = GamificationApp.GetInstance ().MyTripHistoryManager.GetRecords ();
			foreach (TripDataModel record in tripRecords) {
				mListData.Add (record);
			}
			mListData.Reverse ();
		}
	}
}

