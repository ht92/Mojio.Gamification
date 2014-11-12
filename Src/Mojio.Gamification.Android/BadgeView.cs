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
using Android.Graphics.Drawables;

namespace Mojio.Gamification.Android
{
	public class BadgeView : ImageView
	{
		private int mBorderWidth = 10;
		private int mViewWidth;
		private int mViewHeight;
		private Bitmap mImage;
		private Paint mPaint;
		private Paint mPaintBorder;
		private BitmapShader mShader;

		public BadgeView (Context context) :
			base (context)
		{
			setup ();
		}

		public BadgeView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			setup ();
		}

		public BadgeView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			setup ();
		}

		private void setup()
		{
			mPaint = new Paint ();
			mPaint.AntiAlias = true;

			mPaintBorder = new Paint ();
			SetBorderColor (Color.White);
			mPaintBorder.AntiAlias = true;
			SetLayerType (LayerType.Software, mPaintBorder);
			mPaintBorder.SetShadowLayer (4.0f, 0.0f, 2.0f, Color.Black);
		}

		public void SetBorderWidth(int borderWidth)
		{
			mBorderWidth = borderWidth;
			Invalidate ();
		}

		public void SetBorderColor(int borderColor)
		{
			if (mPaintBorder != null) {
				mPaintBorder.Color = new Color (borderColor);
			}
			Invalidate ();
		}

		private void loadBitmap()
		{
			BitmapDrawable bitmapDrawable = (BitmapDrawable)this.Drawable;
			if (bitmapDrawable != null) {
				mImage = bitmapDrawable.Bitmap;
			}
		}
			
		protected override void OnDraw(Canvas canvas) 
		{
			loadBitmap ();
			if (mImage != null) {
				mShader = new BitmapShader (Bitmap.CreateScaledBitmap (mImage, canvas.Width, canvas.Height, false), Shader.TileMode.Clamp, Shader.TileMode.Clamp);
				mPaint.SetShader (mShader);
				int circleCenter = mViewWidth / 2;
				// circleCenter is the x or y of the view's center
				// radius is the radius in pixels of the cirle to be drawn
				// paint contains the shader that will texture the shape
				canvas.DrawCircle (circleCenter + mBorderWidth, circleCenter + mBorderWidth, circleCenter + mBorderWidth - 4.0f, mPaintBorder);
				canvas.DrawCircle (circleCenter + mBorderWidth, circleCenter + mBorderWidth, circleCenter - 4.0f, mPaint);
			}
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			int width = measureWidth (widthMeasureSpec);
			int height = measureHeight (heightMeasureSpec);
			mViewWidth = width - (mBorderWidth * 2);
			mViewHeight = height - (mBorderWidth * 2);
			SetMeasuredDimension (width, height);
		}

		private int measureWidth (int measureSpec)
		{
			int result = 0;

			if (MeasureSpec.GetMode (measureSpec) == MeasureSpecMode.Exactly) {
				result = MeasureSpec.GetSize (measureSpec);;
			} else {
				result = mViewWidth;
			}
			return result;
		}

		private int measureHeight (int measureSpecHeight)
		{
			int result = 0;
			if (MeasureSpec.GetMode (measureSpecHeight) == MeasureSpecMode.Exactly) {
				result = MeasureSpec.GetSize (measureSpecHeight);
			} else {
				result = mViewHeight;
			}
			return (result + 2);
		}
	}
}

