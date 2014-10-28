using System;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;

using Mojio;
using Mojio.Client;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	[Activity (Label = "Mojio.Gamification.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		public MojioClient client;
		private Button connectButton;
		private Button logoutButton;
		private EditText debugText;

		private DrawerLayout mDrawerLayout;
		private ListView mDrawerList;
		//private String mDrawerTitle;
		private String[] mPageTitles;

		//private ActionBarDrawerToggle mDrawerToggle;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			//mDrawerTitle = "Navigation";
			mPageTitles = this.Resources.GetStringArray (Resource.Array.pages_array);
			mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mDrawerList = FindViewById<ListView> (Resource.Id.drawer_list);

			//mDrawerLayout.SetDrawerShadow (Resource.Drawable.drawer_shadow, GravityCompat.Start);
			ArrayAdapter listAdapter = new ArrayAdapter (this, Android.Resource.Layout.drawer_list_item, mPageTitles);
			mDrawerList.Adapter = listAdapter;
			//mDrawerList.ItemClick += null;

			//this.ActionBar.SetDisplayHomeAsUpEnabled (true);
			//this.ActionBar.SetHomeButtonEnabled (true);

			connectButton = FindViewById<Button> (Resource.Id.ConnectButton);
			logoutButton = FindViewById<Button> (Resource.Id.LogoutButton);
			debugText = FindViewById<EditText> (Resource.Id.debugText);
			connectButton.Click += connectButton_handler;
			logoutButton.Click += logoutButton_handler;

		}

		private async void connectButton_handler(object sender, EventArgs e) 
		{
			bool isLive = false;
			string endPoint = isLive ? MojioClient.Live : MojioClient.Sandbox;
			Guid appId = new Guid ("e143d232-8b8c-45c2-b2ac-dc6757d25d9f");
			Guid secretKey = new Guid ("115b0974-579c-4920-afa1-67e2bd942bf5");
			MojioClient client = new MojioClient (endPoint);
			bool success = await client.BeginAsync (appId, secretKey);
			debugText.Append ("\nConnecting to Mojio Client..." + success.ToString () + "\n");
			//Authenticate User
			//MojioResponse<Token> token = await client.SetUserAsync ("htang92", "Eece419");
			//debugText.Append ("\n Logging in...token...\n" + token.Data.ToString ());
			//Results<Trip> trips = MojioConnectUtility.GetTrips(client);
		}

		private void logoutButton_handler(object sender, EventArgs e)
		{
			client.ClearUserAsync ();
		}
	}
}


