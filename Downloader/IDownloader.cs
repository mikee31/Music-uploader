using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicUploaderGUI
{
    public interface IDownloader
    {
        void Download(String url, int index);
        
    }
}
