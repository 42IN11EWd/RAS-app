using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;
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
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        private ToolTip toolTip = new ToolTip();

        public GraphView()
        {
            InitializeComponent();
        }

        private void line_MouseMove(object sender, MouseEventArgs e)
        {
            LineSeries senderLine               = ((LineSeries)sender);
            System.Windows.Point coordinate     = e.GetPosition(senderLine);

            IRangeAxis xAxis                    = (LinearAxis)lineChart.Axes[0];
            var xHit                            = xAxis.GetValueAtPosition(new UnitValue(coordinate.X, Unit.Pixels));
            float xHitOnFloat                   = float.Parse(xHit.ToString());
            float specificXHit                  = (float)(Math.Round((double)xHitOnFloat, 3));            

            IRangeAxis yAxis                    = (LinearAxis)lineChart.Axes[1];            
            var yHit                            = yAxis.GetValueAtPosition(new UnitValue(coordinate.Y, Unit.Pixels));
            float yHitOnFloat                   = float.Parse(yHit.ToString());
            var yMax                            = yAxis.Range.Maximum;
            float yMaxOnFloat                   = float.Parse(yMax.ToString());

            var specHit                         = yMaxOnFloat - yHitOnFloat;
            float specHitY                      = float.Parse(specHit.ToString());
            float specHitY2                     = (float)(Math.Round((double)specHitY, 3));

            if (senderLine.Name.Contains("speed"))
            {
                toolTip.Content = "Time: " + specificXHit.ToString("0.000").Replace(".", ",") + " Speed: " + specHitY2.ToString("0.000").Replace(".", ",");
            }
            else
            {
                toolTip.Content = "Time: " + specificXHit.ToString("0.000").Replace(".", ",") + " Distance: " + specHitY2.ToString("0.000").Replace(".", ",");
            }
            lineChart.ToolTip                   = toolTip;
            toolTip.IsOpen                      = true;           
        }

        private void lineChart_MouseLeave(object sender, MouseEventArgs e)
        {
            if (toolTip.IsOpen)
            {
                toolTip.IsOpen = false;
            }
        }
    }
}
