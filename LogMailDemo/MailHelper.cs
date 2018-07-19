﻿using LogMailDemo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Linq;
public class MailHelper
{
    private MailMessage mMailMessage;
    private SmtpClient mSmtpClient;
    private string mSenderServerHost = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("MailServerHost")) ? "smtphost.redmond.corp.microsoft.com" : ConfigurationManager.AppSettings.Get("MailServerHost");
    private List<string> mReceiverList = ConfigurationManager.AppSettings.Get("RecieveEmailAccountList").Split(';').ToList();
    private List<string> mCCList = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("CCList")) ? null : ConfigurationManager.AppSettings.Get("CCList").Split(';').ToList();
    private string mSender = ConfigurationManager.AppSettings.Get("SendEmailAccount");
    private string htmlBody = string.Empty;


    public void Send(List<DataLineChart> ldlc, DateTime RecordBeforeTime, string Title, string HtmlBodyStr)
    {
        mMailMessage = new MailMessage();
        mMailMessage.From = new MailAddress(mSender);
        foreach (var receiver in mReceiverList)
        {
            mMailMessage.To.Add(receiver);
        }
        if (mCCList != null)
        {
            foreach (var cc in mCCList)
                mMailMessage.CC.Add(cc);
        }
        
        mMailMessage.Subject = Title;
        mMailMessage.IsBodyHtml = true;
        mMailMessage.Body = htmlBody;
        var atv = AddLinkedResource(ldlc, RecordBeforeTime, HtmlBodyStr);
        foreach (var dlc in ldlc)
        {
            mMailMessage.Attachments.Add(AddAttachment(dlc));
        }
        mMailMessage.AlternateViews.Add(atv);

        mSmtpClient = new SmtpClient(mSenderServerHost);
        mSmtpClient.UseDefaultCredentials = true;
        mSmtpClient.Send(mMailMessage);
    }

    protected AlternateView AddLinkedResource(List<DataLineChart> ldlc, DateTime Time, string htmlBodyStr)
    {
        List<LinkedResource> llr = new List<LinkedResource>();
        LinkedResource img;
        htmlBody = htmlBodyStr;
        //htmlBody = string.Empty;
        //htmlBody = string.Format("<html><body><p>Partners' data collected from 00:00 to {0} everyday in past 10 days.</p>",Time.ToString("HH:mm"));
        StringBuilder htmlContent = new StringBuilder();
        foreach (var dlc in ldlc)
        {
            img = new LinkedResource(dlc.ImageStream, MediaTypeNames.Image.Jpeg);
            img.ContentId = Guid.NewGuid().ToString();
            htmlContent.Append(string.Format("<img src=\"cid:{0}\">", img.ContentId));
            llr.Add(img);
        }
        htmlBody += htmlContent.ToString() + "</body></html>";
        AlternateView atv = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
        foreach (var lr in llr)
        {
            atv.LinkedResources.Add(lr);
        }
        return atv;
    }

    private Attachment AddAttachment(DataLineChart dlc)
    {
        MemoryStream s2 = new MemoryStream();
        dlc.ImageStream.Position = 0;
        dlc.ImageStream.CopyTo(s2);
        dlc.ImageStream.Position = 0;
        s2.Position = 0;
        Attachment att = new Attachment(s2, dlc.Title + ".jpg");
        return att;
    }
}