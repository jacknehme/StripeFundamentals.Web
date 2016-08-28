using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;


namespace StripeFundamentals.Web.Services
{
    public class EmailService
    {
        public void SendRefundEmail(StripeCharge stripeCharge)
        {
            var message = new MailMessage() { IsBodyHtml = true };
            message.To.Add(new MailAddress("customerservice@mailinator.com"));
            message.Subject = string.Format("refund request on charge: {0}", stripeCharge.Id);
            message.Body = string.Format("<p>{0}</p>", string.Format("A customer at this email addres {0} was issued a refund on their purchase: '{1}'. Please follow up to determine a reason.", stripeCharge.ReceiptEmail, stripeCharge.Description));


            using(var smtp = new SmtpClient())
            {
                // will error if no email server. Check Web.Config <system.net> is correct. 
                // Look into "Paper cut" for email server
                smtp.Send(message);
            }
        }
    }
}