using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace LogMailDemo
{
    public class LineChart
    {
        Chart ch;
        public LineChart(DataLineChart DataLineChart)
        {
            ch = new Chart();
            #region ChartArea
            ChartArea ca = new ChartArea();

            ca.Area3DStyle.Inclination = 40;
            ca.Area3DStyle.IsClustered = true;
            ca.Area3DStyle.IsRightAngleAxes = false;
            ca.Area3DStyle.LightStyle = LightStyle.Realistic;
            ca.Area3DStyle.Perspective = 9;
            ca.Area3DStyle.Rotation = 25;
            ca.Area3DStyle.WallWidth = 3;
            //ca.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            ca.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            ca.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            // ca.AxisY.LabelStyle.Font = new Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            ca.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            ca.BackColor = Color.OldLace;
            ca.BackGradientStyle = GradientStyle.TopBottom;
            ca.BackSecondaryColor = Color.White;
            ca.BorderColor = Color.FromArgb(64, 64, 64, 64);
            ca.BorderDashStyle = ChartDashStyle.Solid;
            ca.ShadowColor = Color.Transparent;


            //custom
            ca.AxisX.LabelStyle.Format = "yyyyMMdd";
            ca.AxisX.Title = "Date-Timeline";
            ca.AxisX.TitleAlignment = StringAlignment.Center;
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.Interval = 1.0;

            ca.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;
            ca.AxisX.LabelStyle.Font = new Font("Calibri", 15, FontStyle.Regular);
            ca.AxisX.IsMarginVisible = true;
            ca.AxisX.TitleFont = new Font("Calibri", 25, FontStyle.Bold);

            ca.AxisY.Title = "Data-In (records)";
            ca.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            ca.AxisY.LabelStyle.Font = new Font("Calibri", 20, FontStyle.Regular);
            ca.AxisY.TitleFont = new Font("Calibri", 25, FontStyle.Bold);

            ch.BorderlineWidth = 14;
            ch.Height = 768;
            ch.Width = 1366;

            ch.ChartAreas.Add(ca);
            #endregion

            #region Serises
            foreach (var seriseData in DataLineChart.Data)
            {
                Series s = new Series() { ChartType = SeriesChartType.Line, IsValueShownAsLabel = true, Font = new Font("Calibri", 20, FontStyle.Regular), BorderWidth = 3, MarkerSize = 8, MarkerStyle = MarkerStyle.Circle, XValueType = ChartValueType.Date };
                foreach (var dataPoint in seriseData)
                {
                    s.Points.Add(dataPoint);
                }
                ch.Series.Add(s);
            }
            #endregion
            ch.Titles.Add(new Title(DataLineChart.Title, Docking.Top, new Font("Calibri", 35, FontStyle.Bold), Color.Black));
        }

        public void SaveChart(MemoryStream imageStream)
        {

            ch.SaveImage(imageStream, ChartImageFormat.Jpeg);
            ch.SaveImage(DateTime.Now.Millisecond.ToString() + ".jpg", ChartImageFormat.Jpeg);
        }

        public MemoryStream SaveChart()
        {
            MemoryStream imageStream = new MemoryStream();
            ch.SaveImage(imageStream, ChartImageFormat.Jpeg);
            imageStream.Position = 0;
            return imageStream;
        }
    }
}
