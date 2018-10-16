using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GamesInfo.API.Services
{
    public class FetcherKnownAPIAlgorithm : IFetcherAlgorithm
    {
        public string Fetch()
        {
            WebClient webClient = new WebClient();
            return webClient.DownloadString("http://livescore-api.com/api-client/scores/live.json?key=irKJl0UTILlox8xR&secret=e7RFHDKYAskskOPDgzmBAgvj2RIAZfsd");
        }
    }
}
