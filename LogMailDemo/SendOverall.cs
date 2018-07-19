using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMailDemo
{
    public class SendOverall : MailHelper
    {
        public void Send(List<DataLineChart> ldlc, DateTime RecordBeforeTime)
        {
            string contentStr = string.Format("<html><body><p>Partners' data collected from 00:00 to {0} everyday in past 10 days.</p>", RecordBeforeTime.ToString("HH:mm"));
            string titleStr = string.Format("Partners' Data Statistics: Until {0}", RecordBeforeTime);
            base.Send(ldlc, RecordBeforeTime, titleStr, contentStr);
        }
    }
}
