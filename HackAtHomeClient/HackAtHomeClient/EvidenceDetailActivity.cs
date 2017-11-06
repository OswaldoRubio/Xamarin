
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using HackAtHome.SAL;
using HackAtHome.Entities;
using Android.Webkit;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/MyImage")]
    public class EvidenceDetailActivity : Activity
    {
		private string FullName;
		private string Token;

        private int EvidenceID;
        private string EvidenceTitle;
        private string EvidenceStatus;

        private string url;
        private string description;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
            SetContentView(Resource.Layout.EvidenceDetailsLayout);

            var FullNameView = FindViewById<TextView>(Resource.Id.textViewUserName);
            var TitleView = FindViewById<TextView>(Resource.Id.textViewTitle);
            var StatusView = FindViewById<TextView>(Resource.Id.textViewStatus);

            if (savedInstanceState != null)
            {
				FullName = savedInstanceState.GetString("FullName");
				Token = savedInstanceState.GetString("Token");

				EvidenceID = savedInstanceState.GetInt("EvidenceID", 0);
				EvidenceTitle = savedInstanceState.GetString("EvidenceTitle");
				EvidenceStatus = savedInstanceState.GetString("EvidenceStatus");

				description = savedInstanceState.GetString("description");
				url = savedInstanceState.GetString("url");

				FullNameView.Text = FullName;
				TitleView.Text = EvidenceTitle;
				StatusView.Text = EvidenceStatus;

                LoadData();
            }

            if (string.IsNullOrEmpty(Token))
			{
				string FullNameValue = Intent.GetStringExtra("FullName");
				FullNameView.Text = FullNameValue;
				FullName = FullNameValue;

                EvidenceID = Intent.GetIntExtra("EvidenceId", 0);

                EvidenceTitle = Intent.GetStringExtra("EvidenceTitle");
                TitleView.Text = EvidenceTitle;

                EvidenceStatus = Intent.GetStringExtra("EvidenceStatus");
                StatusView.Text = EvidenceStatus;

				string TokenValue = Intent.GetStringExtra("Token");
				Token = TokenValue;

                GetDetails();
			}
        }


        private async void GetDetails()
        {
            Services Services = new Services();
            EvidenceDetail evidenceDetail = await Services.GetEvidenceByIDAsync(
                Token, EvidenceID);

            url = evidenceDetail.Url;
            description = "<body text=\"white\">" +
                           evidenceDetail.Description + "</body>";
            LoadData();
        }

        private void LoadData()
        {
			var DescriptionView = FindViewById<WebView>(Resource.Id.webViewDescription);
			var ImageView = FindViewById<ImageView>(Resource.Id.imageViewEvidence);

            DescriptionView.SetBackgroundColor(Android.Graphics.Color.Transparent);
			DescriptionView.LoadDataWithBaseURL(
					null, description,
					"text/html",
					"utf-8", null);

			Koush.UrlImageViewHelper.SetUrlDrawable(ImageView, url);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
			outState.PutString("FullName", FullName);
			outState.PutString("Token", Token);

            outState.PutInt("EvidenceID", EvidenceID);
            outState.PutString("EvidenceTitle", EvidenceTitle);
            outState.PutString("EvidenceStatus", EvidenceStatus);

            outState.PutString("description", description);
			outState.PutString("url", url);
        }

		protected override void OnResume()
		{
			base.OnResume();
		}

		protected override void OnPause()
		{
			base.OnPause();
		}

		protected override void OnStop()
		{
			base.OnStop();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		protected override void OnRestart()
		{
			base.OnRestart();
		}
    }
}
