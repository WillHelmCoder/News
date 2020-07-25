using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dal.Classes
{
    public static class Static
    {
       
        public static MvcHtmlString SpanishMonthName(this HtmlHelper helper, Int32 monthNumber)
        {
            var monthName = String.Empty;

            switch (monthNumber)
            {
                case 1: monthName = "Enero"; break;
                case 2: monthName = "Febrero"; break;
                case 3: monthName = "Marzo"; break;
                case 4: monthName = "Abril"; break;
                case 5: monthName = "Mayo"; break;
                case 6: monthName = "Junio"; break;
                case 7: monthName = "Julio"; break;
                case 8: monthName = "Agosto"; break;
                case 9: monthName = "Septiembre"; break;
                case 10: monthName = "Octubre"; break;
                case 11: monthName = "Noviembre"; break;
                case 12: monthName = "Diciembre"; break;
                default: monthName = String.Empty; break;
            }

            return new MvcHtmlString(monthName);
        }

        public static MvcHtmlString Truncate(this HtmlHelper helper, String text)
        {
            var charactersToTruncate = 33;

            if (text.Length <= charactersToTruncate) return new MvcHtmlString(text);

            var truncatedText = text.Substring(0, charactersToTruncate);

            truncatedText += "...";

            return new MvcHtmlString(truncatedText);
        }

        public static String GetName()
        {
            return "OH HAI!";
        }
    }
}