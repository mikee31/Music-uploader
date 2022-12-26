using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicUploaderGUI
{
    public class IpAddressValidator : IStringValidator
    {
        // TODO : could be better. The address 999.999.999.999 works with this regex.
        //private static readonly Regex IP_REGEX = new Regex(@"^([0-9]{1,3}\.){3}[0-9]{1,3}$");
        private static readonly Regex IP_REGEX = new Regex(@"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$");

        public bool Validate(string value)
        {
            return IP_REGEX.IsMatch(value);
        }
    }
}
