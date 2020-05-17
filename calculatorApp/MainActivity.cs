using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using System;
using Java.Lang;
using Android.Speech;

namespace calculatorApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        private TextView _calculatorText;
        private string[] _numbers = new string[2];
        private string _operator;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _calculatorText = FindViewById<TextView>(Resource.Id.txtViewCalculator);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        [Java.Interop.Export("ButtonClick")]
        public void buttonClick(View v)
        {
            Button button = (Button)v;
            if ("0123456789.".Contains(button.Text))
            {
                AddDigit(button.Text);
            }
            else if("÷×+-".Contains(button.Text))
            {
                AddOperator(button.Text);
            }
            else if ("=" == button.Text)
            {
                Calculate();
            }
            else
            {
                Erase();
            }
        }

        private void Erase()
        {
            _numbers[0] = _numbers[1] = null;
            _operator = null;
            UpdateCalculatorText();
        }

        private void Calculate(string newOperator=null)
        {
            double? result = null;
            double? first = _numbers[0] == null ? null : (double?)double.Parse(_numbers[0]);
            double? second = _numbers[1] == null ? null : (double?)double.Parse(_numbers[1]);

            switch(_operator)
            {
                case "÷":
                    result = first / second;
                    break;
                case "×":
                    result = first * second;
                    break;
                case "-":
                    result = first - second;
                    break;
                case "+":
                    result = first + second;
                    break;
            }
            if (result != null)
            {
                _numbers[0] = result.ToString();
                _operator = newOperator;
                _numbers[1] = null;
                UpdateCalculatorText();
            }
        }

        private void AddOperator(string value)
        {
            if (_numbers[1] != null)
            {
                Calculate(value);
                return;
            }
            _operator = value;
            UpdateCalculatorText();
        }

        private void UpdateCalculatorText() => _calculatorText.Text = $"{ _numbers[0]}{_operator}{_numbers[1]}";

        private void AddDigit(string value)
        {
            int index = _operator == null ? 0 : 1;

            if (value == "." && _numbers[index].Contains("."))
            {
                return;
            }

            _numbers[index] += value;

            UpdateCalculatorText();
        }
    }
}