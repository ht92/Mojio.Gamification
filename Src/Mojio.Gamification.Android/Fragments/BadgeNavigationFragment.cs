using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Xamarin.Facebook;
using Xamarin.Facebook.Model;
using Xamarin.Facebook.Widget;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class BadgeNavigationFragment : AbstractNavigationFragment
	{
		private BadgeCollectionExpandableListAdapter mBadgeCollectionExpandableListAdapter;
		private ExpandableListView mBadgeCollectionExpandableListView;
		private List<Badge> mListData;
		private UiLifecycleHelper mUiHelper;

		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.badge_collection_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mBadgeCollectionExpandableListView = rootView.FindViewById<ExpandableListView> (Resource.Id.badgeCollection_expandableListView);
			prepareListData ();
			mBadgeCollectionExpandableListAdapter = new BadgeCollectionExpandableListAdapter (this, rootView.Context, mListData);
			mBadgeCollectionExpandableListView.SetAdapter (mBadgeCollectionExpandableListAdapter);
			mBadgeCollectionExpandableListView.SetOnGroupClickListener (new BadgeCollectionGroupClickListener ());
			return rootView;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			mUiHelper = new UiLifecycleHelper (this.Activity, null);
			mUiHelper.OnCreate (savedInstanceState);
		}

		public override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			mUiHelper.OnActivityResult (requestCode, (int)resultCode, data, new ShareDialogCallback ());
		}
			
		public override void OnResume ()
		{
			base.OnResume ();
			mUiHelper.OnResume ();
		}

		public override void OnPause ()
		{
			base.OnPause ();
			mUiHelper.OnPause ();
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			mUiHelper.OnDestroy ();
		}

		public override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);
			mUiHelper.OnSaveInstanceState (outState);
		}

		private void prepareListData ()
		{
			mListData = new List<Badge> ();
			List<Badge> unlockedBadges = GamificationApp.GetInstance ().MyAchievementManager.GetUnlockedBadgeCollection ();
			List<Badge> lockedBadges = GamificationApp.GetInstance ().MyAchievementManager.GetLockedBadgeCollection ();
			mListData.AddRange (unlockedBadges);
			mListData.AddRange (lockedBadges);
		}

		public void ShareBadge (Badge b)
		{
			if (!FacebookDialog.CanPresentShareDialog (GamificationApp.GetInstance ().ApplicationContext, FacebookDialog.ShareDialogFeature.ShareDialog)) {
				Toast.MakeText (GamificationApp.GetInstance ().ApplicationContext, Resource.String.error_facebook_app_not_installed, ToastLength.Long).Show ();
			} else {
				IOpenGraphObject badge = OpenGraphObjectFactory.CreateForPost ("mojiogamification:badge");
				badge.SetProperty ("title", b.GetDisplayName ());
				badge.SetProperty ("description", b.GetDescription ());

				IOpenGraphAction action = OpenGraphActionFactory.CreateForPost ("mojiogamification:unlock");
				action.SetProperty ("badge", (Java.Lang.Object)badge);

				Bitmap badgeIcon = ((BitmapDrawable)Resources.GetDrawable (b.GetDrawableResource ())).Bitmap;
				FacebookDialog.OpenGraphActionDialogBuilder builder = new FacebookDialog.OpenGraphActionDialogBuilder (this.Activity, action, "mojiogamification:unlock", "badge");
				builder.SetImageAttachmentsForObject ("badge", new List<Bitmap> { badgeIcon });
				FacebookDialog shareDialog = builder.Build ();
				mUiHelper.TrackPendingDialogCall (shareDialog.Present ());
			}
		}

		internal class BadgeCollectionGroupClickListener : Java.Lang.Object, ExpandableListView.IOnGroupClickListener
		{
			bool ExpandableListView.IOnGroupClickListener.OnGroupClick (ExpandableListView parent, View clickedView, int groupPosition, long id)
			{
				BadgeCollectionExpandableListAdapter adapter = (BadgeCollectionExpandableListAdapter)parent.ExpandableListAdapter;
				List<Badge> data = adapter.GetData ();
				return !(data [groupPosition].IsUnlocked ());
			}
		}
			
		/*
		 * Callback handler for when share dialog closes and control returns to the calling app
		 */
		internal class ShareDialogCallback : Java.Lang.Object, FacebookDialog.ICallback 
		{
			public void OnComplete (FacebookDialog.PendingCall pendingCall, Bundle data)
			{
				Logger.GetInstance ().Info ("ShareDialog successfully completed.");
			}

			public void OnError (FacebookDialog.PendingCall pendingCall, Java.Lang.Exception error, Bundle data)
			{
				Logger.GetInstance ().Error (String.Format("ShareDialog error - {0}", error));
			}
		}
	}
}

