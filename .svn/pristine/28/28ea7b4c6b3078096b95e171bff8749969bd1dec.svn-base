using System;
using Dal.Models;

namespace Dal.Classes
{
    public static class Configuration
    {
        public static DalEnvironmentTypes DalEnvironment = DalEnvironmentTypes.Production;

        public static String CannonicalSiteUrl = "http://localhost:35785";

        public static Int32 CookieLifeTime = 24 * 24 * 120;

        public static String TempDataCookie = "__TD-Cookie__";
        public static String UserCookie = "__U-Cookie__";
        public static String clickCookie = "__C-Cookie__";

        public static String ExpressionMagicString = "Expression";
        public static String AlertMagicString = "AlertClass";
        public static String MessageMagicString = "Message";

        //public static String FideltaRemoteUrl = "https://www.fidelta.mx/";
        public static String FideltaRemoteUrl = "http://localhost:7777/";
        public static String FideltaCreatePromotion = FideltaRemoteUrl + "api/Promos";


        #region PayPal

        public static String BusinessAccountKey = "inovercy@gmail.com";
        public static Boolean UseSandBox = false;
        public static String CurrencyCode = "MXN";
        public static String ReturnUrl = "/PayPal/Complete";
        public static String CancelUrl = "/PayPal/Cancel";
        public static String NotifyUrl = "/PayPal/PaymentNotification";

        #endregion

    }
}