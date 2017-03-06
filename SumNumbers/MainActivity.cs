using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Lang;
using System.ServiceModel;

namespace SumNumbers
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/sum")]
    public class MainActivity : Activity
    {
        public static readonly EndpointAddress endPoint = new EndpointAddress("http://192.168.1.54:9608/PlusNumbersWCFService.svc");
        //public static readonly EndpointAddress endPoint = new EndpointAddress("http://192.168.1.9:9608/PlusNumbersWCFService.svc");

        private PlusNumbersWCFServiceClient plusNumbersWCFServiceClient;
        private TextView A;
        private TextView B;
        private TextView res;
        private Button sumBtn;
        private Button delBtn;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            InitializeSumNumbersServiceClient();

            A = FindViewById<TextView>(Resource.Id.numberA);
            B = FindViewById<TextView>(Resource.Id.numberB);
            res = FindViewById<TextView>(Resource.Id.sumNumbers);
            sumBtn = FindViewById<Button>(Resource.Id.SumNumbersBtn);
            delBtn = FindViewById<Button>(Resource.Id.DeleteBtn);
                     
                      
            sumBtn.Click += delegate { SumNumbers(A, B, res); };

            delBtn.Click += delegate { DeleteNumbers(A, B, res); };
            
        }

        private void InitializeSumNumbersServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();
            plusNumbersWCFServiceClient = new PlusNumbersWCFServiceClient(binding, endPoint);
        }

        private static BasicHttpBinding CreateBasicHttp()
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                Name = "basicHttpBinding",
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647
            };
            TimeSpan timeout = new TimeSpan(0, 0, 30);
            binding.SendTimeout = timeout;
            binding.OpenTimeout = timeout;
            binding.ReceiveTimeout = timeout;
            return binding;
        }

        private void DeleteNumbers(TextView A, TextView B, TextView res)
        {
            A.Text = "";
            B.Text = "";
            res.Text = "";

        }

        public string SumNumbers(TextView A, TextView B, TextView res)
        {
            int numberA = A.Text != "" ? Integer.ParseInt(A.Text.ToString()) : 0;

            int numberB = B.Text != "" ? Integer.ParseInt(B.Text.ToString()) : 0;

            int result = plusNumbersWCFServiceClient.PlusNumbers(numberA, numberB);

            plusNumbersWCFServiceClient.Close();
            
            res.Text = result.ToString();

            return result.ToString();



        }
    }
}

