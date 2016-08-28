using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stripe;
using System.Configuration;
using StripeFundamentals.Web.Services;
using StripeFundamentals.Models.Subscription;

namespace StripeFundamentals.Controllers
{
    public class SubscriptionController : Controller
    {

        private IPlanService planService;

        public  SubscriptionController(IPlanService planService)
        {
            this.planService = planService;
        }
        public SubscriptionController()
        {

        }

        public IPlanService PlanService
        {
            get
            {
                return planService ?? new PlanService();
            }
            private set
            {
                planService = value;
            }
        }


        // GET: Subscription
        public ActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                Plans = PlanService.List()
            };
            return View(viewModel);
        }

        public ActionResult Billing(int planId)
        {

            string stripePublishableKey = ConfigurationManager.AppSettings["stripePublishableKey"];
            var viewModel = new BillingViewModel() { Plan = PlanService.Find(planId), StripePublishableKey = stripePublishableKey };

            return View(viewModel);
        }
    }
}