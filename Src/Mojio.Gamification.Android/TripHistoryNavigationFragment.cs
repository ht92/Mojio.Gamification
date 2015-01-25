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
using Android.Graphics;

using Newtonsoft.Json;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class TripHistoryNavigationFragment : AbstractNavigationFragment
	{
		private TripHistoryExpandableListAdapter mTripHistoryListAdapter;
		private ExpandableListView mTripHistoryList;
		private List<TripDataModel> mListData;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.trip_history_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mTripHistoryList = (ExpandableListView) rootView.FindViewById<ExpandableListView> (Resource.Id.tripHistory_expandableListView);
			prepareListData ();
			mTripHistoryListAdapter = new TripHistoryExpandableListAdapter (rootView.Context, mListData);
			mTripHistoryList.SetAdapter (mTripHistoryListAdapter);
			return rootView;
		}

		private void prepareListData ()
		{
			mListData = new List<TripDataModel> ();
			List<TripRecord> tripRecords = GamificationApp.GetInstance ().MyTripRecordRepository.GetRecords ();
			foreach (TripRecord record in tripRecords) {
				TripDataModel tripDataModel = TripDataModel.Deserialize (record.tripData);
				mListData.Add (tripDataModel);
			}
		}
	}
}

