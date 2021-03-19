using System;
using System.Collections.Generic;
using System.Text;

namespace IQB.EntityLayer.ReportModels
{
    public class ReportResponse
    {
        public int StatusCode { get; set; }
        public List<PlotData> Response { get; set; }
        public string xType { get; set; }
        public string yType { get; set; }
        public string y1Type { get; set; }
        public string StatusMessage { get; set; }
    }

    public class PlotData
    {
        public string x { get; set; }

        public string xN
        {
            get
            {
                if (x != null)
                {
                    String xyz = x.Split(' ')[0].ToString();
                    return xyz;
                }
                else
                    return
                        "";
               
            }
            set { }

        }

        public string y { get; set; }
        public string y1 { get; set; }
    }

}
