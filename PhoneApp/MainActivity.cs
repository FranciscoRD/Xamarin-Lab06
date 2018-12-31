using Android.App;
using Android.Widget;
using Android.OS;

namespace PhoneApp
{
    [Activity(Label = "Phone App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static readonly System.Collections.Generic.List<string> PhoneNumbers = new System.Collections.Generic.List<string>();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var PhoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            var TranslateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            var CallButton = FindViewById<Button>(Resource.Id.CallButton);
            var CallHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);

            CallButton.Enabled = false;
            var TranslatedNumber = string.Empty;

            TranslateButton.Click += (object sender, System.EventArgs e) =>
              {
                  var Translator = new PhoneTranslator();
                  TranslatedNumber = Translator.ToNumber(PhoneNumberText.Text);
                  if (string.IsNullOrWhiteSpace(TranslatedNumber))
                  {
                      CallButton.Text = "Lllamar";
                      CallButton.Enabled = false;
                  }
                  else
                  {
                      CallButton.Text = $"Llamar al {TranslatedNumber}";
                      CallButton.Enabled = true;
                  }
              };
            CallButton.Click += (object sender, System.EventArgs e) =>
              {
                  var CallDialog = new AlertDialog.Builder(this);
                  CallDialog.SetMessage($"Llamar al numero {TranslatedNumber}?");
                  CallDialog.SetNeutralButton("Llamar",delegate
                  {
                      PhoneNumbers.Add(TranslatedNumber);
                      CallHistoryButton.Enabled = true;
                      var CallIntent = new Android.Content.Intent(Android.Content.Intent.ActionCall);
                      CallIntent.SetData(Android.Net.Uri.Parse($"tel:{TranslatedNumber}"));
                      StartActivity(CallIntent);
                  });
                  CallDialog.SetNegativeButton("Cancelar",delegate { });
                  CallDialog.Show();
              };
            CallHistoryButton.Click += (object sender, System.EventArgs e) =>
            {
                var Intent = new Android.Content.Intent(this,typeof(CallHistoryActivity));
                Intent.PutStringArrayListExtra("phone_numbers",PhoneNumbers);
                StartActivity(Intent);
            };
            Validate();
        }
        public async void Validate()
        {
            var MessageText = FindViewById<TextView>(Resource.Id.textView2);
            //var ServiceClient = new SALLab05.ServiceClient();
            var ServiceClient = new SALLab06.ServiceClient();

            string StudentEmail = "francisco_renan-dt@hotmail.com";
            string Password = "zxxzpa6413";
            string myDevice = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            var Result = await ServiceClient.ValidateAsync(StudentEmail, Password, myDevice);

            MessageText.Text = $"{Result.Status}\n{Result.Fullname})\n{Result.Token}";


        }
    }
}

