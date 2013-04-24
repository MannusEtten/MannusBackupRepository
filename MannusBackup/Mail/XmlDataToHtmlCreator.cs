using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRINederland.Framework.Xml;
using System.IO;
using MannusBackup.Configuration;

namespace MannusBackup.Mail
{
    internal class XmlDataToHtmlCreator
    {
        public void ExportResultsToHtmlForEmail(string htmlFileName, string xmlFileName)
        {
            string xsltPath = MannusBackupConfiguration.GetConfig().XsltMailTemplate;
            string htmlText = TransformXmlToHtml(xmlFileName, xsltPath);
            File.WriteAllText(htmlFileName, htmlText);
        }

        private string TransformXmlToHtml(string xmlFilePath, string xsltPath)
        {
            XsltTransformer xsltTransfomer = new XsltTransformer(xmlFilePath, xsltPath);
            return xsltTransfomer.TransformXml();
        }
    }
}
