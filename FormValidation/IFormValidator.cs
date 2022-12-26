using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicUploaderGUI
{
    public interface IFormValidator
    {
        /// <summary>
        /// Validates the URL submitted.
        /// </summary>
        /// <returns>True if valid, false if invalid.</returns>
        ///  /// <exception cref="InvalidOperationException">This exception is thrown if <see cref="newSubmit"/> has
        ///  not ben called first.</exception>  
        bool IsUrlValid();

        /// <summary>
        /// Validates the IP address submitted.
        /// </summary>
        /// <returns>True if valid, false if invalid.</returns>
        ///  /// <exception cref="InvalidOperationException">This exception is thrown if <see cref="newSubmit"/> has
        ///  not ben called first.</exception>
        bool IsIpValid();

        /// <summary>
        /// Validates the index submitted if a playlist is being validated. If not, returns true.
        /// </summary>
        /// <returns>True if valid, false if invalid.</returns>
        ///  /// <exception cref="InvalidOperationException">This exception is thrown if <see cref="newSubmit"/> has
        ///  not ben called first.</exception>
        bool IsIndexValid();

        /// <summary>
        /// Submit new URL to validate. By using this method, it is assumed that the URL should be a video
        /// and that it is not getting uploaded to an IP address.
        /// </summary>
        /// <param name="url">The video URL to validate</param>
        void NewSubmit(string url);

        /// <summary>
        /// Submit new values to validate. By using this method, it is assumed that the URL should be a video
        /// and that it is getting uploaded to an IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address to validate</param>
        /// <param name="url">The video URL to validate</param>
        void NewSubmit(String url, String ipAddress);

        /// <summary>
        /// Submit new values to validate. By using this method, it is assumed that the URL should be a playlist
        /// and that it is getting uploaded to an IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address to validate</param>
        /// <param name="url">The playlist URL to validate</param>
        /// <param name="firstIndex">The index of the first video to download</param>
        void NewSubmit(String url, String ipAddress, int firstIndex);

        /// <summary>
        /// Submit new values to validate. By using this method, it is assumed that the URL should be a playlist
        /// and that it is not getting uploaded to an IP address.
        /// </summary>
        /// <param name="url">The playlist URL to validate</param>
        /// <param name="firstIndex">The index of the first video to download</param>
        void NewSubmit(String url, int firstIndex);
    }
}
