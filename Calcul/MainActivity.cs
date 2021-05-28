using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace Calcul
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private TextView _calculatorTextView;

        private readonly string[] _numbers = new string[2];
        private string _operator;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
           
            
            SetContentView(Resource.Layout.activity_main);
            _calculatorTextView = FindViewById<TextView>(Resource.Id.calculator_text_view);
        }

        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button) v;
            if ("0123456789.".Contains(button.Text ?? string.Empty))
                AddDigitOrDecimalPoint(button.Text);
            else if ("÷×+-".Contains(button.Text ?? string.Empty))
                AddOperator(button.Text);
            else if ("=" == button.Text)
                Calculate();
            else
                Erase();
        }

        private void AddDigitOrDecimalPoint(string value)
        {
            int index = _operator == null ? 0 : 1;

            if (value == "." && _numbers[index].Contains("."))
                return;

            _numbers[index] += value;

            UpdateCalculatorText();
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

        private void Calculate(string newOperator = null)
        {
            double? result = null;
            double? first = _numbers[0] == null ? null : (double?) double.Parse(_numbers[0]);
            double? second = _numbers[1] == null ? null : (double?) double.Parse(_numbers[1]);

            switch (_operator)
            {
                case "÷":
                    result = first / second;
                    break;
                case "×":
                    result = first * second;
                    break;
                case "+":
                    result = first + second;
                    break;
                case "-":
                    result = first - second;
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

        private void Erase()
        {
            _numbers[0] = _numbers[1] = null;
            _operator = null;
            UpdateCalculatorText();
        }

        private void UpdateCalculatorText() => _calculatorTextView.Text = $"{_numbers[0]} {_operator} {_numbers[1]}";


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}