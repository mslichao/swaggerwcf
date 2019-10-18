using Ionic.Zip;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace SwaggerWcf.Support
{
    public class StaticContent
    {
        private const string ZipFileName = "SwaggerWcf.www.swagger-ui.zip";
        private static readonly ZipFile Archive;
        private static ZipFile _archiveCustom;

        internal static GetFileCustomDelegate GetFileCustom;

        static StaticContent()
        {
            try
            {
                Assembly assembly = Assembly.GetAssembly(typeof(StaticContent));
                Stream zipStream = assembly.GetManifestResourceStream(ZipFileName);

                if (zipStream == null)
                    return;

                Archive = ZipFile.Read(zipStream);
            }
            catch { }
        }

        internal static void SetArchiveCustom(Stream zipCustomStream)
        {
            try
            {
                if (zipCustomStream == null)
                    return;

                _archiveCustom = ZipFile.Read(zipCustomStream);
            }
            catch { }
        }

        public static Stream GetFile(string filename, out string contentType, out long contentLength)
        {
            if (GetFileCustom != null)
            {
                Stream output = GetFileCustom(filename, out contentType, out contentLength);
                if (output != null)
                    return output;
            }

            contentType = GetContentType(filename);

            if (_archiveCustom != null)
            {
                ZipEntry file = _archiveCustom.Entries.FirstOrDefault(entry => entry.FileName == filename);
                if (file != null && contentType != null)
                {
                    contentLength = file.UncompressedSize;
                    return file.OpenReader();
                }
            }

            if (Archive != null)
            {
                ZipEntry file = Archive.Entries.FirstOrDefault(entry => entry.FileName == filename);
                if (file != null && contentType != null)
                {
                    contentLength = file.UncompressedSize;
                    return file.OpenReader();
                }
            }

            return ReturnError(out contentType, out contentLength);
        }

        private static Stream ReturnError(out string contentType, out long contentLength)
        {
            contentType = "";
            contentLength = 0;
            return Stream.Null;
        }

        private static string GetContentType(string filename)
        {
            int lastIndexOfDot = filename.LastIndexOf('.');
            if (lastIndexOfDot < 0)
                return null;

            string extension = filename.Substring(lastIndexOfDot + 1);
            switch (extension.ToLower())
            {
                case "html":
                    return "text/html";

                case "js":
                    return "application/javascript";

                case "css":
                    return "text/css";

                case "svg":
                    return "image/svg+xml";

                case "ttf":
                    return "application/x-font-ttf";

                case "eot":
                    return "application/vnd.ms-fontobject";

                case "woff":
                    return "application/font-woff";

                case "woff2":
                    return "application/font-woff2";

                case "gif":
                    return "image/gif";

                case "ico":
                    return "image/x-icon";

                case "png":
                    return "image/png";

                default:
                    return null;
            }
        }
    }
}
