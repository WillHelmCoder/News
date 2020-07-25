using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Dal.AuthControllers;
using Dal.Models;

namespace Dal.Controllers
{
    public class PayPalController : BasicController
    {
        // GET: PayPal
        public ActionResult PaymentNotification()
        {
            var formVals = new Dictionary<string, string>();
            formVals.Add("cmd", "_notify-validate");
            string response = PayPalModel.GetPayPalResponse(formVals, false);


            if (response == "VERIFIED")
            {

                string transactionID = Request["txn_id"];
                string sAmountPaid = Request["mc_gross"];
                string orderID = Request["item_number"];


                //int pagoId = 0;
                //Int32.TryParse(Request["item_number"].ToString(), out pagoId);

                //var order = StoreService.GetShoppingCartById(pagoId);

                //if (order != null)
                //{
                //    if (!StoreService.UpdatePaypalTokenOrder(transactionID, order.Token))
                //    {
                //        SetMessage(StoreService.ServiceTempData);
                //        return new EmptyResult();
                //    }

                //}

            }
            return new EmptyResult();
        }

        public ActionResult Complete(string orderId, string responseCode, string authorizeId, string receiptNo)
        {
            return Redirect("/");
        }

        public ActionResult Cancel(String orderId)
        {
            return Redirect("/");
        }
    }
}