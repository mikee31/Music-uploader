using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MusicUploaderGUI
{
    public class PlaylistUrlValidator : YoutubeValidator
    {
        private static readonly Regex REGEX = new Regex(@"\/playlist\?list=");
        private IYoutubeAPI youtubeAPI;

        public PlaylistUrlValidator() : base(REGEX)
        {
            this.youtubeAPI = new YoutubeAPI();
        }

        public override bool IsIndexValid(int i, string url)
        {
            int nbVids = youtubeAPI.GetNumberOfVideosInPLaylist(url);
            if (i < 0 || i >= nbVids)
                return false;
            return true;
        }
    }
}
