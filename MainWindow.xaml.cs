using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FactoryDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private IPolicyFactory factory;
        private enum PolicyType { Vehicle, Household }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void VehicleRadio_Checked(object sender, RoutedEventArgs e)
        {
            PolicyDetailsPanel.Visibility = Visibility.Visible;
            ContentsLabel.Visibility = Visibility.Collapsed;
            ContentsTextBox.Visibility = Visibility.Collapsed;
            factory = new VehiclePolicyFactory();
        }

        private void HouseholdRadio_Checked(object sender, RoutedEventArgs e)
        {
            PolicyDetailsPanel.Visibility = Visibility.Visible;
            ContentsLabel.Visibility = Visibility.Visible;
            ContentsTextBox.Visibility = Visibility.Visible;
            factory = new HouseholdPolicyFactory();
        }

        private void CreatePolicy_Click(object sender, RoutedEventArgs e)
        {
            if (factory != null)
            {
                string model = ModelTextBox.Text;
                string color = ColorTextBox.Text;
                int year = 0;
                decimal contentsValue = 0;

                if (!string.IsNullOrWhiteSpace(YearTextBox.Text) && int.TryParse(YearTextBox.Text, out year))
                {
                    if (factory.GetType() == typeof(VehiclePolicyFactory))
                    {
                        VehiclePolicy policy = factory.CreatePolicy(model, color, year) as VehiclePolicy;
                        ResultLabel.Content = $"Vehicle Insurance Policy Created: {policy.GetPolicyDetails()}";
                    }
                    else if (factory.GetType() == typeof(HouseholdPolicyFactory) && decimal.TryParse(ContentsTextBox.Text, out contentsValue))
                    {
                        HouseholdPolicy policy = factory.CreatePolicy(contentsValue) as HouseholdPolicy;
                        ResultLabel.Content = $"Household Contents Policy Created: {policy.GetPolicyDetails()}";
                    }
                    else
                    {
                        ResultLabel.Content = "Invalid input for policy creation.";
                    }
                }
                else
                {
                    ResultLabel.Content = "Invalid input for year of registration.";
                }
            }
            else
            {
                ResultLabel.Content = "Please select a policy type.";
            }
        }
    }

    // Abstract Factory and Concrete Factories
    public interface IPolicyFactory
    {
        IPolicy CreatePolicy(params object[] args);
    }

    public class VehiclePolicyFactory : IPolicyFactory
    {
        public IPolicy CreatePolicy(params object[] args)
        {
            if (args.Length == 3 && args[0] is string model && args[1] is string color && args[2] is int year)
            {
                return new VehiclePolicy(model, color, year);
            }
            else
            {
                throw new ArgumentException("Invalid arguments for creating Vehicle Policy.");
            }
        }
    }

    public class HouseholdPolicyFactory : IPolicyFactory
    {
        public IPolicy CreatePolicy(params object[] args)
        {
            if (args.Length == 1 && args[0] is decimal contentsValue)
            {
                return new HouseholdPolicy(contentsValue);
            }
            else
            {
                throw new ArgumentException("Invalid arguments for creating Household Policy.");
            }
        }
    }

    // Abstract Product and Concrete Products
    public interface IPolicy
    {
        string GetPolicyDetails();
    }

    public class VehiclePolicy : IPolicy
    {
        private string model;
        private string color;
        private int year;

        public VehiclePolicy(string model, string color, int year)
        {
            this.model = model;
            this.color = color;
            this.year = year;
        }

        public string GetPolicyDetails()
        {
            return $"Vehicle Model: {model}, Color: {color}, Year of Registration: {year}";
        }
    }

    public class HouseholdPolicy : IPolicy
    {
        private decimal contentsValue;

        public HouseholdPolicy(decimal contentsValue)
        {
            this.contentsValue = contentsValue;
        }

        public string GetPolicyDetails()
        {
            return $"Contents Value: {contentsValue:C}";
        }

    }
}
