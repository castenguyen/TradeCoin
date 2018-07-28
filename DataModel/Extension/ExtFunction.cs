using DataModel.DataViewModel;
using DataModel.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataModel.DataEntity;

namespace DataModel.Extension
{
    public static class ExtFunction
    {
        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        /// <summary>
        /// Check string is has value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasValue(this string value)
        {
            return !value.HasNoValue();
        }

        public static bool HasNoValue(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Remove Vietnamese sign from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Return Vietnamese string without sign</returns>
        public static string RemoveSign(this string value)
        {
            if (value == null) return value;
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    value = value.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return value;
        }
        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
        /// <summary>
        /// Format thay khoang trang " " thành '-' , Remove dấu tiếng việt
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string ToFriendlyURL(this string title)
        {
            if (title.HasNoValue()) return "";
            // remove entities
            title = Regex.Replace(title.RemoveSign(), @"&\w+;", "");
            // remove anything that is not letters, numbers, dash, or space
            title = Regex.Replace(title, @"[^A-Za-z0-9\-\s]", "");
            // remove any leading or trailing spaces left over
            title = title.Trim();
            // replace spaces with single dash
            title = Regex.Replace(title, @"\s+", "-");
            // if we end up with multiple dashes, collapse to single dash            
            title = Regex.Replace(title, @"\-{2,}", "-");
            // make it all lower case
            title = title.ToLower();
            //Remove special character
            //title = title.RemoveSpecialCharacters();
            // if it's too long, clip it
            //if (title.Length > 80)
            //    title = title.Substring(0, 79);
            // remove trailing dash, if there is one
            if (title.EndsWith("-"))
                title = title.Substring(0, title.Length - 1);
            //string DatetimeNow = DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
            return title;
        }

        /// <summary>
        /// Cắt chỗi title cho bài viết
        /// </summary>
        /// <param name="s">Chuỗi cần cắt</param>
        /// <param name="length">giới hạn</param>
        /// <returns></returns>
        public static string CatChuoi(string s, int length)
        {
            if (String.IsNullOrEmpty(s))
            {
                //throw new ArgumentNullException(s);
                return "";
            }

            var words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words[0].Length > length)
                throw new ArgumentException("Từ đầu tiên dài hơn chuỗi cần cắt");
            var sb = new StringBuilder();
            foreach (var word in words)
            {
                if ((sb + word).Length > length)
                    return string.Format("{0}...", sb.ToString().TrimEnd(' '));
                sb.Append(word + " ");
            }
            return string.Format("{0}", sb.ToString().TrimEnd(' '));
        }

        public static Config Config()
        {
            alluneedbEntities db = new alluneedbEntities();
            Config tmp = db.Configs.FirstOrDefault();
            return tmp;
        }
    }

}
