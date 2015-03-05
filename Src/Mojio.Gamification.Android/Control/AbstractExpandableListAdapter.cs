using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Mojio.Gamification.Android
{
	public abstract class AbstractExpandableListAdapter<T> : BaseExpandableListAdapter
	{
		protected Context mContext;
		protected List<T> mData;

		protected AbstractExpandableListAdapter (Context context, List<T> data) 
		{
			mContext = context;
			mData = data;
		}

		public List<T> GetData ()
		{
			return mData;
		}
			
		public override Java.Lang.Object GetChild (int groupPosition, int childPosition)
		{
			return null;
		}

		public override long GetChildId (int groupPosition, int childPosition)
		{
			return childPosition;
		}

		public override int GetChildrenCount (int groupPosition)
		{
			return 1;
		}

		public override Java.Lang.Object GetGroup (int groupPosition)
		{
			return null;
		}

		public override int GroupCount {
			get {
				return mData.Count;
			}
		}

		public override long GetGroupId (int groupPosition)
		{
			return groupPosition;
		}


		public override bool HasStableIds {
			get {
				return false;
			}
		}

		public override bool IsChildSelectable (int groupPosition, int childPosition)
		{
			return false;
		}

		public abstract override View GetChildView (int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent);
		public abstract override View GetGroupView (int groupPosition, bool isExpanded, View convertView, ViewGroup parent);
	}
}

