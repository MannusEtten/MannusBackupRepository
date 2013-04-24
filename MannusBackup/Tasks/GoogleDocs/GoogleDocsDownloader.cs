namespace MannusBackup.Tasks.GoogleDocs
{
    /*
    internal class GoogleDocsDownloader
    {
        private const string BASEURI = "http://docs.google.com/feeds/default/private/full/";
        private readonly RequestSettings requestSettings;
        private readonly DocumentsService service;
        private GoogleElement configuration;
        private string downloadDirectory = string.Empty;

        public GoogleDocsDownloader(GoogleElement configuration)
        {
            this.configuration = configuration;
            requestSettings = new RequestSettings("MannusBackup", configuration.UserName, configuration.Password);
            service = new DocumentsService("MannusBackup");
            SetupCredentials();
        }

        private void SetupCredentials()
        {
            service.Credentials = requestSettings.Credentials;
            var reqFactory = (GDataGAuthRequestFactory)service.RequestFactory;
            reqFactory.ProtocolMajor = 3;
        }

        private void QueryFolders(string uri, string foldername)
        {
            if (!Directory.Exists(foldername)) Directory.CreateDirectory(foldername);
            var query = new DocumentsListQuery(uri);
            DocumentsFeed doclistFeed = null;
            query.ShowFolders = true;
            query.TitleExact = true;
            try
            {
                doclistFeed = service.Query(query);
                QueryEntries(doclistFeed.Entries, foldername);
            }
            catch (GDataRequestException e)
            {
                Logger.GetLogger().LogError(e.Message);
            }
            catch (ClientFeedException e)
            {
                Logger.GetLogger().LogError(e.Message);
            }
        }

        private void QueryEntries(AtomEntryCollection entries, string folderName)
        {
            foreach (DocumentEntry entry in entries)
            {
                if (entry.IsDocument || entry.IsSpreadsheet || entry.IsPresentation)
                {
                    DownloadDocument(entry, folderName);
                }
                if (entry.IsFolder)
                {
                    QueryFolder(folderName, entry);
                }
            }
        }

        private void QueryFolder(string folderName, DocumentEntry entry)
        {
            string newFolderName = string.Format(@"{0}\{1}", folderName, entry.Title.Text);
            int index = entry.SelfUri.Content.LastIndexOf("folder");
            string uri = string.Format("{0}{1}/contents", BASEURI, entry.SelfUri.Content.Substring(index));
            QueryFolders(uri, newFolderName);
        }

        private Document.DownloadType GetDownloadType(string documentTitle, Document.DocumentType documentType)
        {
            switch (documentType)
            {
                case Document.DocumentType.PDF:
                    return Document.DownloadType.pdf;
                case Document.DocumentType.Presentation:
                    return Document.DownloadType.ppt;
                case Document.DocumentType.Spreadsheet:
                    return Document.DownloadType.xls;
                case Document.DocumentType.Document:
                    return GetDownloadType(documentTitle);
            }
            return Document.DownloadType.html;
        }

        private Document.DownloadType GetDownloadType(string documentTitle)
        {
            string extension = documentTitle.Substring(documentTitle.Length - 3);
            switch (extension)
            {
                case "odt":
                    return Document.DownloadType.odt;
                case "txt":
                    return Document.DownloadType.txt;
                default:
                    return Document.DownloadType.doc;
            }
            return Document.DownloadType.doc;

            throw new NotImplementedException();
        }

        private void DownloadDocument(AtomEntry entry, string folderName)
        {
            var document = new Document();
            document.AtomEntry = entry;
            var request = new DocumentsRequest(requestSettings);
            SaveDocument(entry, document, request, folderName);
        }

        private void SaveDocument(AtomEntry entry, Document document, DocumentsRequest request, string folderName)
        {
            Document.DownloadType documentFormat = GetDownloadType(document.Title, document.Type);
            if (documentFormat != Document.DownloadType.xls)
            {
                try
                {
                    using (Stream stream2 = request.Download(document, documentFormat))
                    {
                        string fileLocation = string.Format(@"{0}\{1}.{2}", folderName, entry.Title.Text, documentFormat);
                        using (var file = new FileStream(fileLocation, FileMode.Create))
                        {
                            DownloadFile(stream2, file);
                        }
                    }
                }
                catch (GDataRequestException e)
                {
                    Logger.GetLogger().LogError(e.Message);
                }
            }
            else
            {
                Logger.GetLogger().LogError("document not downloaded because it is a spreadsheet:" + document.Title);
            }
        }

        private void DownloadFile(Stream stream2, FileStream file)
        {
            if (file != null)
            {
                int nBytes = 2048;
                int count = 0;
                var arr = new Byte[nBytes];
                do
                {
                    count = stream2.Read(arr, 0, nBytes);
                    file.Write(arr, 0, count);
                } while (count > 0);
                file.Flush();
                file.Close();
            }
        }

        internal void DownloadDocuments(string downloadDirectory)
        {
            this.downloadDirectory = downloadDirectory;
            QueryFolders("http://docs.google.com/feeds/default/private/full", downloadDirectory);
        }
    }

    */
}