using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.Entities;
using HackAtHome.SAL;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/MyImage", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Services ServicesClient;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            if(savedInstanceState != null) {
                base.OnRestoreInstanceState(savedInstanceState);
            }

            if(ServicesClient == null)
            {
                ServicesClient = new Services();
            }

            var validar = FindViewById<Button>(Resource.Id.buttonValidate);
			var email = FindViewById<EditText>(Resource.Id.editTextEmail);
			var password = FindViewById<EditText>(Resource.Id.editTextPassword);

            email.Text = "oswaldo.rubio@outlook.com";
            password.Text = "";

            validar.Click += (sender, e) => {
                if(validData(email, password))
                    AutenticateUser(email.Text.Trim(), password.Text.Trim());
            };
        }

        private async void AutenticateUser(string email, string pass)
        {
            ResultInfo info = await ServicesClient.AutenticateAsync(email, pass);
            switch (info.Status)
            {
                case Status.Success:
                    SendEvidence(email);
                    var ActivityIntent =
                        new Android.Content.Intent(
                            this, typeof(EvidenceListActivity)
                        );
                    ActivityIntent.PutExtra("FullName", info.FullName);
                    ActivityIntent.PutExtra("Token", info.Token);
                    StartActivity(ActivityIntent);
                    break;
                case Status.InvalidUserOrNotInEvent:
                    showErrorMessage(Resources.GetString(
                        Resource.String.AutenticationError));
                    break;
                default:
                    showErrorMessage(Resources.GetString(
                        Resource.String.UnknownError));
                    break;
            }
        }

        private bool validData(EditText email, EditText pass)
        {
            if (string.IsNullOrEmpty(email.Text.Trim()))
            {
                string msg = Resources.GetString(Resource.String.EmptyEmail);
                showErrorMessage(msg);
                return false;
            }
            else
            if (string.IsNullOrEmpty(pass.Text.Trim()))
            {
                string msg = Resources.GetString(Resource.String.EmpyPassword);
                showErrorMessage(msg);
                return false;
            }
            return true;
        }

        private async void SendEvidence(string email)
        {
			var MicrosoftEvidence = new LabItem
			{
				Email = email,
				Lab = "Hack@Home",
				DeviceId = Android.Provider.Settings.Secure.GetString(
					ContentResolver, Android.Provider.Settings.Secure.AndroidId)

			};

			var MicrosoftClient = new MicrosoftServiceClient();
			await MicrosoftClient.SendEvidence(MicrosoftEvidence);
        }

        private void showErrorMessage(string msg)
        {
			Android.App.AlertDialog.Builder Builder =
					 new AlertDialog.Builder(this);
			AlertDialog Alert = Builder.Create();
			Alert.SetTitle(Resources.GetString(Resource.String.ErrorTitle));
			Alert.SetIcon(Resource.Drawable.MyImage);
			Alert.SetMessage(msg);
			Alert.SetButton("Ok", (s, ev) => { });
			Alert.Show();
        }
    }
}
