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
            //内容
            string contentStr = string.Format("<html><body><p>Partners' data collected from 00:00 to {0} everyday in past 10 days.</p>", RecordBeforeTime.ToString("HH:mm"));
            //主题
            string titleStr = string.Format("Partners' Data Statistics: Until {0}", RecordBeforeTime);
            base.Send(ldlc, RecordBeforeTime, titleStr, contentStr);
        }
        public void Send(string attchmentPath,string errorMsg,string stackTrace, string patnerName)
        {
            //RecordBeforeTime.ToString("HH:mm")
            //内容
            string contentStr = string.Format("<html><body><p>Error message is: {0}.</p>\r\n<p>StackTrace is: {1}.</p>", errorMsg,stackTrace );
           
            //主题
            string titleStr = string.Format("An error occurred in {0}", patnerName);
            //附件的路径
            base.Send(titleStr, contentStr, attchmentPath);
        }
    }
}
