namespace IQB.EntityLayer.Common
{
    using HomeModels;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Xamarin.Forms;

    public class UtilityEL
    {
        public static bool IsValidEmailAddress(string email)
        {
            string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
            return Regex.IsMatch(email, emailRegex);
        }

        public static DateTime GetExactData(string datetime, string format)
        {
            DateTime tempDate = DateTime.Now;
            //
           
            if (Device.RuntimePlatform == Device.iOS)
            {
                //When run on IOS device the uncomment the following line and commen other
                 DateTime.TryParse(datetime,out tempDate);
                //When run on IOS Simulator the uncomment the following line and commen other
                //DateTime.TryParseExact(datetime, format, System.Globalization.CultureInfo.InstalledUICulture, System.Globalization.DateTimeStyles.None, out tempDate);
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                //tempDate = DateTime.ParseExact(datetime, format, System.Globalization.CultureInfo.InvariantCulture);
                // DateTime.TryParseExact(datetime, format, System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.None, out tempDate);
                DateTime.TryParseExact(datetime, format, System.Globalization.CultureInfo.InstalledUICulture, System.Globalization.DateTimeStyles.None, out tempDate);
            }

            return tempDate;
        }

        public static string GetFormattedDateFormat(DateTime date)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                switch (Convert.ToString((date.Day).ToString()[(date.Day).ToString().Length - 1]))
                {
                    case "1":
                        sb.Append($"{date.Day.ToString()}st ");
                       // sb.Append("1st ");
                        break;

                    case "2":
                        sb.Append($"{date.Day.ToString()}nd ");
                     //   sb.Append("2nd ");
                        break;

                    case "3":
                        sb.Append($"{date.Day.ToString()}rd ");
                     //   sb.Append("3rd ");
                        break;

                    default:
                        sb.Append($"{date.Day.ToString()}th ");
                        break;

                }

                sb.Append($"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month)} {date.Year}");

            }
            catch { }

            return sb.ToString();
        }
        public static string GetRandomString(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static ApiResult ToApiModel(string result)
        {
            ApiResult obj = new ApiResult();

            try
            {
                if (result != null)
                {
                    obj = JsonConvert.DeserializeObject<ApiResult>(result);
                }
            }
            catch
            {
                obj = new ApiResult();
            }

            return obj;
        }

        public static HomeEL ToModelHome(object result)
        {
            HomeEL obj = new HomeEL();

            try
            {
                if (result!=null)
                {
                    obj = JsonConvert.DeserializeObject<HomeEL>(Convert.ToString(result));
                }
            }
            catch(Exception ex)
            {
                obj = new HomeEL();
            }

            return obj;
        }

        public static T ToModel<T>(object result)
        {
            List<T> objList = new List<T>();
            T obj = Activator.CreateInstance<T>();

            //T obj1 = Activator.CreateInstance<T>();
            //obj1 = (T)JsonConvert.DeserializeObject(Convert.ToString(result), typeof(T));

            try
            {
                if (result != null)
                {
                    objList = (List<T>)JsonConvert.DeserializeObject(Convert.ToString(result), typeof(List<T>));

                    if (objList != null && objList.Count > 0)
                    {
                        obj = objList.FirstOrDefault();
                    }
                }
            }
            catch(Exception ex)
            {
                obj = Activator.CreateInstance<T>();
            }

            return obj;
        }

        public static T ToModelNew<T>(object result)
        {            
            List<T> objList = new List<T>();
            T obj = Activator.CreateInstance<T>();
            try
            {
                if (result != null)
                {
                    var obj1 = JsonConvert.DeserializeObject<T>(result.ToString());
                    obj = obj1;

                }
            }
            catch (Exception es)
            {
                obj = Activator.CreateInstance<T>();
            }
            return obj;
        }

        public static List<T> ToModelList<T>(object result)
        {
            List<T> objList = new List<T>();
          
            try
            {
                if (result != null)
                {
                    objList = (List<T>)JsonConvert.DeserializeObject(Convert.ToString(result), typeof(List<T>));
                    
                }
            }
            catch (Exception es)
            {
                objList = new List<T>();
            }
            return objList;
        }

        public static List<T> ToModelListNew<T>(object result)
        {
            List<T> objList = new List<T>();           
            try
            {
                if (result != null)
                {
                    var obj = JsonConvert.DeserializeObject<T>(result.ToString());
                    objList.Add(obj);
                }
            }
            catch (Exception es)
            {
                objList = new List<T>();
            }
            return objList;
        }

     
    }
}