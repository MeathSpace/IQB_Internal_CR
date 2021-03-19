using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IQB.Utility
{
    public class ErrorLogger
    {
        /// <summary>
        /// Data to be uploaded on Google spreadsheets.
        /// </summary>
        private List<ExceptionEntry> lstExceptionEntry;

        /// <summary>
        /// Gets or set Google Form ID.
        /// </summary>
        public string formId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FormID">The form ID of Google Form, which is to be extracted from live form URL</param>
        public ErrorLogger(string FormID)
        {
            formId = FormID;
            lstExceptionEntry = new List<ExceptionEntry>();
        }

        /// <summary>
        /// Adds an exception entry to the exception list, which is to be uploaded.
        /// </summary>
        /// <param name="id">Entry id of text box, which is avaible by viewing source (Ctrl + U) of Google Form.</param>
        /// <param name="data">Data for the column.</param>
        public void AddEntry(string id, string data)
        {
            lstExceptionEntry.Add(new ExceptionEntry(id, data));
        }

        /// <summary>
        /// Uploads data to Google form. The results can be seen in Google Drive as well a Google Spreadsheets.
        /// </summary>
        /// <returns>The result of HTTP POST request to Google Drive. It is the object of class HttpResponseMessage.</returns>
        public async Task<HttpResponseMessage> UploadAsync()
        {
            try
            {
                var objHttpClient = new HttpClient();

                var FormParameters = Uri.EscapeUriString(GetFormData());

                var objHttpRequestMessage = new HttpRequestMessage(HttpMethod.Post, GetFormUrl());
                objHttpRequestMessage.Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(FormParameters)));
                objHttpRequestMessage.Content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                return await objHttpClient.SendAsync(objHttpRequestMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// It returns URL to which the data is posted.
        /// </summary>
        /// <returns>The Google Form URL</returns>
        private string GetFormUrl()
        {
            var FormUrl = new StringBuilder("https://docs.google.com/forms/d/e/");
            FormUrl.Append(formId);
            FormUrl.Append("/formResponse");
            return FormUrl.ToString();
        }

        /// <summary>
        /// It returns HTTP POST data, which consists the textbox ID of Google Form &amp; its respective values.
        /// </summary>
        /// <returns>HTTP POST reuqest data</returns>
        private string GetFormData()
        {
            var FormData = new StringBuilder();

            foreach (ExceptionEntry entry in lstExceptionEntry)
            {
                FormData.Append("entry.");
                FormData.Append(entry.entryId);
                FormData.Append("=");
                FormData.Append(entry.data);
                FormData.Append("&");
            }

            FormData.Remove(FormData.Length - 1, 1);
            return FormData.ToString();
        }
    }

}
