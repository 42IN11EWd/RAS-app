using RunApproachStatistics.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace RunApproachStatistics.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            //close application set pilot laser to null
            MainViewModel viewModel = (MainViewModel)DataContext;
            viewModel.PilotLaserToNullCommand.Execute(0);
            Thread.Sleep(1000);
            while(viewModel.isPilotLaserOn())
            {
                // wait for pilot laser to turn off
                Console.WriteLine("wait for pilot");
            }

            Environment.Exit(-1);
        }
    }
}
