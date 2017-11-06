using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;

using HackAtHome.Entities;

namespace HackAtHomeClient
{
    public class Complex : Fragment
    {
        public List<Evidence> MyList { get; set; }

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			RetainInstance = true;
		}
    }
}
