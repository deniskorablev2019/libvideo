using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;
using VideoLibrary.Helpers;

namespace YoutubeExtractor
{
    public static class DownloadUrlResolver
    {
        private static YouTubeService Service = new YouTubeService();

        public static void DecryptDownloadUrl(VideoInfo info)
        {
        }

        public static IEnumerable<VideoInfo> GetDownloadUrls(string videoUrl, bool decryptSignature = true)
        {
            if (videoUrl == null)
                throw new ArgumentNullException(nameof(videoUrl));

            // GetAllVideos normalizes the URL as of libvideo v0.4.1, 
            // don't call TryNormalizeYoutubeUrl here.

            return Service.GetAllVideos(videoUrl).Select(v => new VideoInfo(v));
        }

        public async static Task<IEnumerable<VideoInfo>> GetDownloadUrlsAsync(
            string videoUrl, bool decryptSignature = true) =>
            (await Service.GetAllVideosAsync(videoUrl)).Select(v => new VideoInfo(v));

        public static bool TryNormalizeYoutubeUrl(string url, out string normalizedUrl)
        {
            // If you fix something in here, please be sure to fix in 
            // YouTubeService.TryNormalize as well.

            normalizedUrl = null;

            var builder = new StringBuilder(url);

            url = builder.Replace("youtu.be/", "youtube.com/watch?v=")
                .Replace("youtube.com/embed/", "youtube.com/watch?v=")
                .Replace("/v/", "/watch?v=")
                .Replace("/watch#", "/watch?")
                .ToString();

            string value;

            if (!Query.TryGetParam("v", url, out value))
                return false;

            normalizedUrl = "https://youtube.com/watch?v=" + value;
            return true;
        }
    }
}
