using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MusicUploaderGUI
{
    public class FormValidator : IFormValidator
    {
        private bool isUrlValid;
        private bool isIpValid;
        private bool isIndexValid;
        private YoutubeValidator ytValidator;
        private IStringValidator ipValidator;
        public FormValidator()
        {
            isUrlValid = false;
            isIpValid = false;
            isIndexValid = false;
            ipValidator = new IpAddressValidator();
        }

        public bool IsUrlValid()
        {
            return isUrlValid;
        }

        public bool IsIpValid()
        {
            return isIpValid;
        }

        public bool IsIndexValid()
        {
            return isIndexValid;
        }

        public void NewSubmit(string url)
        {
            this.isIpValid = true;
            this.isIndexValid = true;
            if (!(this.ytValidator is VideoUrlValidator))
                this.ytValidator = new VideoUrlValidator();
            this.isUrlValid = this.ytValidator.Validate(url);
        }

        public void NewSubmit(string url, string ipAddress)
        {
            this.isIpValid = this.ipValidator.Validate(ipAddress);
            this.isIndexValid = true;
            if (!(this.ytValidator is VideoUrlValidator))
                this.ytValidator = new VideoUrlValidator();
            this.isUrlValid = this.ytValidator.Validate(url);
        }

        public void NewSubmit(string url, string ipAddress, int firstIndex)
        {
            this.isIpValid = this.ipValidator.Validate(ipAddress);
            if (!(this.ytValidator is PlaylistUrlValidator))
                this.ytValidator = new PlaylistUrlValidator();
            this.isIndexValid = this.ytValidator.IsIndexValid(firstIndex - 1, url);
            this.isUrlValid = this.ytValidator.Validate(url);
        }

        public void NewSubmit(string url, int firstIndex)
        {
            this.isIpValid = true;
            if (!(this.ytValidator is PlaylistUrlValidator))
                this.ytValidator = new PlaylistUrlValidator();
            this.isIndexValid = this.ytValidator.IsIndexValid(firstIndex - 1, url);
            this.isUrlValid = this.ytValidator.Validate(url);
        }
    }
}
