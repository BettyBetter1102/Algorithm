using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace LogMailDemo
{
    public class DataLineChart
    {
        public DataLineChart()
        {
            Data = new List<List<DataPoint>>();
            ImageStream = new MemoryStream();
        }
        public List<List<DataPoint>> Data { get; set; }

        public string Title { get; set; }

        public string AppID { get; set; }

        public MemoryStream ImageStream { get; set; }

        ~DataLineChart()
        {
            ImageStream.Close();
        }
    }
}
