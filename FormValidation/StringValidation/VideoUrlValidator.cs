using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicUploaderGUI
{
    public class VideoUrlValidator : YoutubeValidator
    {
        private static readonly Regex REGEX = new Regex(@"\/watch\?v=");
        public VideoUrlValidator() : base(REGEX)
        {

        }

        public override bool IsIndexValid(int i, string url)
        {
            if (i == 0)
                return true;
            else
                return false;
        }
    }
}
