using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRINederland.Framework.Xml;
using System.IO;
using MannusBackup.Configuration;
using ESRINederland.Framework.Logging;

namespace MannusBackup.Mail
{
    internal class XmlDataToHtmlCreator
    {
        private ILogger _logger;

        public XmlDataToHtmlCreator()
        {
            _logger = Logger.GetLogger();
        }

        public void ExportResultsToHtmlForEmail(string htmlFileName, string xmlFileName)
        {
            string xsltPath = MannusBackupConfiguration.GetConfig().XsltMailTemplate;
            string htmlText = TransformXmlToHtml(xmlFileName, xsltPath);
            File.WriteAllText(htmlFileName, htmlText);
        }

        private string TransformXmlToHtml(string xmlFilePath, string xsltPath)
        {
            if (!File.Exists(xsltPath))
            {
                _logger.LogError("Xlst voor samenstellen mailbericht met resultaat niet gevonden '{0}'", xsltPath);
                return string.Empty;
            }
            XsltTransformer xsltTransfomer = new XsltTransformer(xmlFilePath, xsltPath);
            return xsltTransfomer.TransformXml();
        }
    }
}
