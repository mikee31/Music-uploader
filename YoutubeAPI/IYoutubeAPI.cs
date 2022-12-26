using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicUploaderGUI
{
    public interface IYoutubeAPI
    {
        /// <summary>
        /// Gets the names and urls of every video in a given playlist.
        /// </summary>
        /// <param name="url">The URL of the playlist to gather info about</param>
        /// <returns>A <see cref="List{}"/> containing <see cref="KeyValuePair{string, string}"/> in which the key represents
        /// the name of the video and the value represents its URL</returns>
        List<KeyValuePair<string, string>> GetVideoNamesAndUrls(string url);
        int GetNumberOfVideosInPLaylist(string url);
    }
}
