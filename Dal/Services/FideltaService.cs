using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using Dal.Classes;
using Dal.Interfaces;
using Dal.Models;
using Dal.Repositories;

namespace Dal.Services
{
    public class FideltaService : BaseService
    {
        public FideltaService(Repository repository, ILog logger)
            : base(repository, logger)
        {
        }

        public FideltaAPIResponseModel CreatePromotion(String businessCode, String title, String description, String restriction, DateTime starts, DateTime expires, Decimal bonus, Int32 promotionType, String urlImage)
        {
            var postData =
              "{\"BusinessCode\":" + "\"" + businessCode + "\"," +
              "\"Title\":" + "\"" + title + "\"," +
              "\"Description\":" + "\"" + description + "\"," +
              "\"Restriction\":" + "\"" + restriction + "\"," +
              "\"Starts\":" + "\"" + starts.ToString("MM-dd-yyyy") + "\"," +
              "\"Expires\":" + "\"" + expires.ToString("MM-dd-yyyy") + "\"," +
              "\"Bonus\":" + bonus + "," +
               "\"romotionType\":" + promotionType + "," +
               "\"Image\":" + "\"" + urlImage + "\"}";

            String jsonResponse = String.Empty;

            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers["SomeHeader"] = "Hello Fidelta";
                    webClient.Headers[HttpRequestHeader.UserAgent] = Configuration.CannonicalSiteUrl;
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    jsonResponse = webClient.UploadString(Configuration.FideltaCreatePromotion, postData);
                }

            }
            catch
            {
                ServiceModelState.AddModelError("", "Sistema de lealtad fuera de servicio.");
                return null;
            }

            var serializer = new JavaScriptSerializer();
            var responseModel = serializer.Deserialize<FideltaAPIResponseModel>(jsonResponse);

            if (responseModel == null)
            {
                ServiceModelState.AddModelError("", "Sistema de lealtad fuera de servicio.");
                return null;
            }

            return responseModel;

        }
    }
}