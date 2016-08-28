using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using Stripe;

namespace StripeFundamentals.Web.Controllers
{
    public class StripeWebhookController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(request).ReadToEnd();

            StripeEvent stripeEvent = null;
            try
            {
                stripeEvent = StripeEventUtility.ParseEvent(json);

                // Verify will not work under test conditions
                //stripeEvent = VerifyEventSentFromStripe(stripeEvent);
                if(HasEventBeenProcessedPreviously(stripeEvent))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }

            }
            catch(Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, string.Format("Unable to parse incoming event. The following error occurred: {0}", ex.Message));
            }

            if(stripeEvent == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Incoming event empty");
            }

            var emailService = new Services.EmailService();

            switch(stripeEvent.Type)
            {
                case StripeEvents.ChargeRefunded:
                    var charge = Mapper<StripeCharge>.MapFromJson(stripeEvent.Data.Object.ToString());
                    emailService.SendRefundEmail(charge);
                    break;


                //case :
                //    break;

                default:
                    break;
            }

            //TODO: log Stripe eventid to StripeEvent table in application database
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private bool HasEventBeenProcessedPreviously(StripeEvent stripeEvent)
        {
            // lookup in your database's StripeEventLog by eventId
            // if eventId exists return true

            return false;


        }

        private static StripeEvent VerifyEventSentFromStripe(StripeEvent stripeEvent)
        {
            var eventService = new StripeEventService();
            stripeEvent = eventService.Get(stripeEvent.Id);
            return stripeEvent;
        }
    }
}