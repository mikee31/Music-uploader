using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicUploaderGUI
{
    public class StringValidatorFactory
    {
        public IStringValidator GetStringValidator(StringValidatorType type)
        {
            switch (type)
            {
                case StringValidatorType.PLAYLIST:
                    return new PlaylistUrlValidator();
                case StringValidatorType.VIDEO:
                    return new VideoUrlValidator();
                case StringValidatorType.IP_ADDRESS:
                    return new IpAddressValidator();
                default:
                    throw new InvalidEnumArgumentException("The value \"" + type + "\" is not valid.");
            }
        }
    }
}
