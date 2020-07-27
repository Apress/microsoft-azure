using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.SessionState;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;

namespace Sentiment_Feedback
{
    public partial class SiteMaster : MasterPage
    {
        //private static readonly string key = "09775213041c464d823b7120e31598e4";
        //private static readonly string endpoint = "https://westus2.api.cognitive.microsoft.com/";
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}