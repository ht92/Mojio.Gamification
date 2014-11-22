using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Mojio.Gamification.Android
{
	public class CircularIndicatorView : View
	{	
		private static readonly int DEFAULT_INDICATOR_WIDTH = 40;
		private static readonly int DEFAULT_START_VALUE = 0;		//min value is 0
		private static readonly int DEFAULT_MAX_VALUE = 100;		//max value is 100
		private static readonly float START_DEGREES = 90;			//View starts at 6 o'clock
		private static readonly int MAX_DEGREES = 360;
	
		//Default background
		private int mBackgroundCenter;
		private int mBackgroundRadius;

		private float mIndicatorTextSize;
		private float mMetricTextSize;
	
		private float mIndicatorWidth = DEFAULT_INDICATOR_WIDTH;
		private int mIndicatorDegrees;
		private int mIndicatorValue = DEFAULT_START_VALUE;
		private int mIndicatorRange = DEFAULT_MAX_VALUE;
		private Color mIndicatorColor = Color.White;
		private Color mTextColor = Color.White;
		private Color mBackgroundColor = Color.Transparent;

		//paint objects
		private Paint mIndicatorPaint;
		private Paint mIndicatorEmptyPaint;
		private Paint mBackgroundPaint;
		private Paint mTextPaint;
		private Paint mMetricPaint;
	
		//bounds of each flow
		private RectF mIndicatorBounds;
	
		//text position
		private float mTextPosY;
		private float mMetricPosY;
		private float mMetricPaddingY;

		//metrics in use
		private String mMetricText;

		//typeface of text
		private Typeface mTypeface;

		//handler to updateview
		private UpdateHandler mValueUpdateHandler;

		public CircularIndicatorView (Context context)
			: base (context)
		{
		}

		public CircularIndicatorView (Context context, IAttributeSet attrs) 
			: base (context, attrs)
		{
			Initialize (context.ObtainStyledAttributes (attrs, Resource.Styleable.CircularIndicator));
		}
			
		private void Initialize (TypedArray a)
		{
			mIndicatorTextSize = a.GetDimension (Resource.Styleable.CircularIndicator_textSize, 22);
			mTextColor = a.GetColor (Resource.Styleable.CircularIndicator_textColor, Color.White);
			mMetricText = a.GetString (Resource.Styleable.CircularIndicator_metricText);
			mMetricTextSize = a.GetDimension (Resource.Styleable.CircularIndicator_metricTextSize, 14);
			mMetricPaddingY = Resources.GetDimension (Resource.Dimension.circularIndicator_metricPaddingY);
			mIndicatorRange = a.GetInt (Resource.Styleable.CircularIndicator_range, DEFAULT_MAX_VALUE);
			mIndicatorColor = a.GetColor (Resource.Styleable.CircularIndicator_indicatorColor, Color.White);
			String typeFace = a.GetString(Resource.Styleable.CircularIndicator_typeface);
			if (typeFace != null) {
				mTypeface = Typeface.CreateFromAsset (Resources.Assets, typeFace);
			}
		}
			
		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow ();
			mValueUpdateHandler = new UpdateHandler (this);
			setupBounds ();
			setupPaints ();
			setupTextPosition ();
		}

		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow ();
			mValueUpdateHandler = null;
			mIndicatorPaint = null;
			mIndicatorEmptyPaint = null;
			mIndicatorBounds = null;
			mBackgroundPaint = null;
			mTextPaint = null;
			mMetricPaint = null;
		}

		private void setupPaints() {
			mIndicatorPaint = new Paint();
			mIndicatorPaint.Color = new Color(mIndicatorColor);
			mIndicatorPaint.AntiAlias = true;
			mIndicatorPaint.SetStyle(Paint.Style.Stroke);
			mIndicatorPaint.StrokeWidth = mIndicatorWidth;

			mIndicatorEmptyPaint = new Paint();
			mIndicatorEmptyPaint.Color = new Color(mIndicatorColor.R, mIndicatorColor.G, mIndicatorColor.B, mIndicatorColor.A / 2);
			mIndicatorEmptyPaint.AntiAlias = true;
			mIndicatorEmptyPaint.SetStyle(Paint.Style.Stroke);
			mIndicatorEmptyPaint.StrokeWidth = mIndicatorWidth;

			mBackgroundPaint = new Paint();
			mBackgroundPaint.Color = mBackgroundColor;
			mBackgroundPaint.AntiAlias = true;
			mBackgroundPaint.SetStyle(Paint.Style.Fill);

			mTextPaint = new Paint();
			mTextPaint.Color = new Color(mTextColor);
			mTextPaint.SetStyle(Paint.Style.Fill);
			mTextPaint.AntiAlias = true;
			mTextPaint.TextSize = mIndicatorTextSize;
			mTextPaint.SetTypeface(mTypeface);
			mTextPaint.TextAlign = Paint.Align.Center;

			mMetricPaint = new Paint();
			mMetricPaint.Color = new Color(mTextColor);
			mMetricPaint.SetStyle(Paint.Style.Fill);
			mMetricPaint.AntiAlias = true;
			mMetricPaint.TextSize = mMetricTextSize;
			mMetricPaint.SetTypeface(mTypeface);
			mMetricPaint.TextAlign = Paint.Align.Center;
		}

		private void setupBounds() {
			mBackgroundCenter = LayoutParameters.Width / 2;
			mBackgroundRadius = mBackgroundCenter - PaddingTop;

			mIndicatorBounds = new RectF(PaddingTop + mIndicatorWidth / 2,
				PaddingLeft + mIndicatorWidth / 2,
				LayoutParameters.Width - PaddingRight
				- mIndicatorWidth / 2, LayoutParameters.Height
				- PaddingBottom - mIndicatorWidth / 2);
		}

		//set up text position
		private void setupTextPosition() {
			Rect textBounds = new Rect ();
			mTextPaint.GetTextBounds ("1", 0, 1, textBounds);
			mTextPosY = mIndicatorBounds.CenterY () + (textBounds.Height () / 2f);
			mMetricPosY = mTextPosY + mMetricPaddingY;
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw (canvas);
			canvas.DrawCircle (mBackgroundCenter, mBackgroundCenter, mBackgroundRadius, mBackgroundPaint);
			canvas.DrawArc (mIndicatorBounds, START_DEGREES, mIndicatorDegrees , false, mIndicatorPaint);
			canvas.DrawArc (mIndicatorBounds, START_DEGREES, MAX_DEGREES , false, mIndicatorEmptyPaint);
			canvas.DrawText (mIndicatorValue.ToString (), mIndicatorBounds.CenterX (), mTextPosY, mTextPaint);
			canvas.DrawText (mMetricText, mIndicatorBounds.CenterX (), mMetricPosY, mMetricPaint);
		}

		public void SetIndicatorValue(double value) {
			if (value <= mIndicatorRange) {
				mIndicatorDegrees = (int) (value * MAX_DEGREES / 100);
			} else {
				mIndicatorDegrees = MAX_DEGREES;
			}
			mIndicatorValue = (int) Math.Round (value, MidpointRounding.AwayFromZero);
			if (mValueUpdateHandler != null) {
				mValueUpdateHandler.SendEmptyMessage (0);
			}
		}

		public CircularIndicatorView SetRange(int range) {
			mIndicatorRange = range;
			return this;
		}

		public CircularIndicatorView SetIndicatorWidth(float width) {
			mIndicatorWidth = width;
			return this;
		}

		public CircularIndicatorView SetIndicatorTextSize(float size) {
			mIndicatorTextSize = size;
			return this;
		}

		public CircularIndicatorView SetMetricTextSize(float size) {
			mMetricTextSize = size;
			return this;
		}

		public CircularIndicatorView SetIndicatorColor(Color color) {
			mIndicatorColor = color;
			return this;
		}

		public CircularIndicatorView SetTextColor(Color color) {
			mTextColor = color;
			return this;
		}

		public CircularIndicatorView SetMetricText(String text) {
			mMetricText = text;
			return this;
		}
			
		public override void SetBackgroundColor(Color color) {
			mBackgroundColor = color;
		}

		public CircularIndicatorView SetTypeface(Typeface typeface) {
			mTypeface = typeface;
			return this;
		}

		//Handles display invalidates
		private class UpdateHandler : Handler 
		{
			private CircularIndicatorView _circulatorIndicatorView;

			public UpdateHandler(CircularIndicatorView civ) 
				: base ()
			{
				_circulatorIndicatorView = civ;
			}
				
			public override void HandleMessage(Message msg) {
				_circulatorIndicatorView.Invalidate ();
				base.HandleMessage (msg);
			}
		}
	}
}

