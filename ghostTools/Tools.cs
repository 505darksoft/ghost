using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ghostDataAccess;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace ghostTools
{
    public class Tools
    {
        #region Convert to GitHub folder
        public static string ConvertToGitHubFolder(string str)
        {
            //if (s != null)
            //{
            //    foreach (char c in s.ToCharArray())
            //    {
            //        if (!(c >= 65 && c <= 90) && !(c >= 97 && c <= 122))
            //        {
            //            s = s.Replace(c, '-');
            //        }
            //    }
            //    for (int i = 0; i < 5; i++)
            //    {
            //        s = s.Replace("--", "-");
            //    }
            //    for (int i = 0; i < 5; i++)
            //    {
            //        s = s.TrimEnd('-');
            //    }
            //}
            string bs = " ";
            for (int i = 0; i < 5; i++)
            {
                bs += " ";
                str = str.Replace(bs, " ");
            }
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            str = rgx.Replace(str, "");
            return str;
        }
        #endregion

        #region Convert to GitHub file
        public static string ConvertToGitHubFile(string s, List<TextReplace> TextReplaceList)
        {
            if (s != null)
            {
                string bs = " ";
                for (int i = 0; i < 5; i++)
                {
                    bs += " ";
                    s = s.Replace(bs, " ");
                }
                foreach (TextReplace t in TextReplaceList)
                {
                    s = s.Replace(t.OldText, t.NewText);
                }
            }
            return s;
        }
        #endregion

        #region Resize image
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        #endregion

        #region Download image
        public static Image DownloadImage(string url)
        {
            byte[] data;
            using (WebClient wc = new WebClient())
            {
                data = wc.DownloadData(url);
            }
            Image img;
            using (var ms = new MemoryStream(data))
            {
                img = Image.FromStream(ms);
            }
            return img;
        }
        #endregion

        #region Encode/decode
        public static string EncodeStringToBase64(string stringToEncode)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(stringToEncode));
        }

        public static string DecodeStringFromBase64(string stringToDecode)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(stringToDecode));
        }
        #endregion
    }
}
