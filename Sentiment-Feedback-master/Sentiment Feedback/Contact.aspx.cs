using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.SessionState;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;

namespace Sentiment_Feedback
{
    public partial class Contact : Page
    {

        private static readonly string key = "09775213041c464d823b7120e31598e4";
        private static readonly string endpoint = "https://westus2.api.cognitive.microsoft.com/";
        //private static readonly string endpoint = "https://westus2.api.cognitive.microsoft.com/text/analytics/v2.1/sentiment";
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        static TextAnalyticsClient authenticateClient()
        {
            ApiKeyServiceClientCredentials credentials = new ApiKeyServiceClientCredentials(key);
            TextAnalyticsClient client = new TextAnalyticsClient(credentials)
            {
                Endpoint = endpoint
            };
            return client;
        }
        public void sentimentAnalysisExample(ITextAnalyticsClient client)
        {
            var result = client.Sentiment(txt_Feedback.Text, "en");
            //var result = client.Sentiment("I hate you", "en");  -- Debugging purposes --
            txt_Score.Text = $"{result.Score:0.00}";
        }

        protected void Btn_Submit_Click(object sender, EventArgs e)
        {
            var client = authenticateClient();
            //Need the following line to address TLS. Otherwise you will get an error with connection focibly closed by remote host.
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            sentimentAnalysisExample(client);
        }

    }
    
}