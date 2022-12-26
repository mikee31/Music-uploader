using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicUploaderGUI
{
    public abstract class YoutubeValidator : IStringValidator
    {
        // TODO : add validation api stuff and regex

        private const string VALIDATOR_URL = "https://www.youtube.com/oembed?format=json&url=";


        private Regex regex;

        protected YoutubeValidator(Regex regex)
        {
            this.regex = regex;
        }

        public bool Validate(string value)
        {
            Match match = regex.Match(value);

            if (!match.Success)
                return false;
            if (!ValidateWithYoutube(value))
                return false;
            return true;
        }

        private bool ValidateWithYoutube(string url)
        {
            string fullUrl = VALIDATOR_URL + url;
            Uri uri = new Uri(fullUrl, UriKind.Absolute);
            HttpClient client = new HttpClient();
            Task<HttpResponseMessage> task = Task.Run(() => { return client.GetAsync(uri); });

            if (task.Result.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public abstract bool IsIndexValid(int i, string url);   
    }
}
