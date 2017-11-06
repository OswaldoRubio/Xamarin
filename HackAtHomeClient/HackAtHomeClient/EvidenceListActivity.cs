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

using HackAtHome.Entities;
using HackAtHome.CustomAdapters;
using HackAtHome.SAL;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/MyImage")]
    public class EvidenceListActivity : Activity
    {
        private string FullName;
        private string Token;
        private ListView listView;
        private Complex EvidenceList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.EvidenceListLayout);

            var FullNameView = FindViewById<TextView>(Resource.Id.textViewFullName);
            listView = FindViewById<ListView>(Resource.Id.listViewEvidence);

            listView.ItemClick += (sender, e) =>
            {
                var ActivityIntent =
                        new Android.Content.Intent(
                            this, typeof(EvidenceDetailActivity)
                        );
                Evidence ev = EvidenceList.MyList.ElementAt(e.Position);

                ActivityIntent.PutExtra("EvidenceId", ev.EvidenceID);
                ActivityIntent.PutExtra("EvidenceTitle", ev.Title);
                ActivityIntent.PutExtra("EvidenceStatus", ev.Status);
                ActivityIntent.PutExtra("FullName", FullName);
                ActivityIntent.PutExtra("Token", Token);

                StartActivity(ActivityIntent);
            };

			EvidenceList = (Complex)this.FragmentManager.FindFragmentByTag("EvidenceList");
			if (EvidenceList == null)
			{
				EvidenceList = new Complex();
				var FragmentTransaction = this.FragmentManager.BeginTransaction();
				FragmentTransaction.Add(EvidenceList, "EvidenceList");
				FragmentTransaction.Commit();
			}

            if (savedInstanceState != null)
            {
                FullName = savedInstanceState.GetString("FullName");
                FullNameView.Text = FullName;
                Token = savedInstanceState.GetString("Token");
                LoadItems();
            }

            if(string.IsNullOrEmpty(Token))
            {
                string FullNameValue = Intent.GetStringExtra("FullName");
                FullNameView.Text = FullNameValue;
                FullName = FullNameValue;

                string TokenValue = Intent.GetStringExtra("Token");
                Token = TokenValue;  

                GetListViewItems();
            }
        }

        private async void GetListViewItems()
        {
            var Services = new Services();
            EvidenceList.MyList 
                        = await Services.GetEvidencesAsync(Token);
            LoadItems();
        }

        private void LoadItems()
        {
            listView.Adapter = new EvidencesAdapter(
                this,
                EvidenceList.MyList,
                Resource.Layout.EvidenceItem,
                Resource.Id.textViewTitle,
                Resource.Id.textViewStatus);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutString("FullName", FullName);
            outState.PutString("Token", Token);
        }
    }
}
