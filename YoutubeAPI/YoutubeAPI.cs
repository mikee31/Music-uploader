using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Services;

namespace MusicUploaderGUI
{
    public class YoutubeAPI : IYoutubeAPI
    {
        private const string apiKey = "AIzaSyBe2dlOvhPzCJCT1EVUn60aRwxDspRLtk4";
        private YouTubeService yt;
        
        public YoutubeAPI()
        {
            this.yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = apiKey });
        }

        public int GetNumberOfVideosInPLaylist(string url)
        {
            PlaylistItemsResource.ListRequest playlistRequest = yt.PlaylistItems.List("contentDetails");
            playlistRequest.PlaylistId = ExtractIdFromPlaylistUrl(url);
            playlistRequest.Fields = "pageInfo/totalResults";
            PlaylistItemListResponse response = null;
            
            try
            {
                response = playlistRequest.Execute();

            }
            catch (Google.GoogleApiException e)
            {
                return 0;
            }
            if (response.PageInfo.TotalResults != null)
                return (int)response.PageInfo.TotalResults;
            else
                return 0;
        }


        public List<KeyValuePair<string, string>> GetVideoNamesAndUrls(string url)
        {
            string nextPageToken = "";
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>(GetNumberOfVideosInPLaylist(url));
            int resultIndex = 0;

            while (nextPageToken != null)
            {
                // TODO : api call to get items/contentDetails/regionRestriction/blocked in order to know which video
                // is blocked. It's not ideal to download videos the user is not aware are in the playlist, and it 
                // also messes up the index.
                // Only downside is it'll need to do a shitload of api calls.
                // This means that right now, if the index doesn't match, this is likely the cause.

                PlaylistItemsResource.ListRequest playlistRequest = yt.PlaylistItems.List("contentDetails,snippet");
                playlistRequest.PlaylistId = ExtractIdFromPlaylistUrl(url);
                playlistRequest.MaxResults = 50;
                playlistRequest.Fields = "items/contentDetails/videoId,items/snippet/title,nextPageToken";
                playlistRequest.PageToken = nextPageToken;
                PlaylistItemListResponse response = playlistRequest.Execute();

                
                for (int i = 0; i < response.Items.Count; i++, resultIndex++)
                {
                    string id = "https://www.youtube.com/watch?v=" + response.Items.ElementAt(i).ContentDetails.VideoId;
                    string name = response.Items.ElementAt(i).Snippet.Title;
                    result.Insert(resultIndex, new KeyValuePair<string, string>(name, id));
                }
                nextPageToken = response.NextPageToken;
            }

            return result;
        }

        private string ExtractIdFromPlaylistUrl(string url)
        {
            if (url == "")
                return "";
            else if (!url.Contains('='))
                return url;
            return url.Split('=')[1];
        }
    }
}
