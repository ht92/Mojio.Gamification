using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public static class StatRowFactory
	{
		public static LinearLayout CreateRow (Context context, int labelId, double value, NumberDisplayer.NumberType numberType)
		{
			string label = context.Resources.GetString (labelId);
			return CreateRow (context, label, value, numberType);
		}

		public static LinearLayout CreateRow (Context context, string label, double value, NumberDisplayer.NumberType numberType)
		{
			NumberDisplayer numberDisplayer = NumberDisplayer.CreateNumberDisplayer (value, numberType);
			return CreateRow (context, label, numberDisplayer);
		}

		public static LinearLayout CreateRow (Context context, int labelId, NumberDisplayer numberDisplayer)
		{
			string label = context.Resources.GetString (labelId);
			return CreateRow (context, label, numberDisplayer);
		}

		public static LinearLayout CreateRow (Context context, string label, NumberDisplayer numberDisplayer)
		{
			LinearLayout rowLayout = new LinearLayout(context);
			rowLayout.Orientation = Orientation.Horizontal;
			rowLayout.AddView (createStatLabelView (context, label));
			rowLayout.AddView (createStatValueView (context, numberDisplayer)); 
			return rowLayout;
		}


		private static TextView createStatLabelView (Context context, string label)
		{
			TextView statLabelTextView = new TextView (context);
			statLabelTextView.Text = label + ":";
			statLabelTextView.SetAllCaps (true);
			statLabelTextView.SetTypeface (null, TypefaceStyle.Bold);
			statLabelTextView.Gravity = GravityFlags.Left | GravityFlags.CenterVertical;
			statLabelTextView.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent, 3);
			return statLabelTextView;
		}

		private static TextView createStatValueView (Context context, NumberDisplayer numberDisplayer)
		{
			TextView statValueTextView = new TextView (context);
			statValueTextView.Text = numberDisplayer.GetString ();
			statValueTextView.SetTypeface (null, TypefaceStyle.Bold);
			statValueTextView.Gravity = GravityFlags.Right | GravityFlags.CenterVertical;
			return statValueTextView;
		}
	}
}

