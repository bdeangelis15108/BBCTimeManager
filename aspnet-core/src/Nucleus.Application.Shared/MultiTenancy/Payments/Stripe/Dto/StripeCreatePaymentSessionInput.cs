﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nucleus.MultiTenancy.Payments.Stripe.Dto
{
    public class StripeCreatePaymentSessionInput
    {
        public long PaymentId { get; set; }

        public string SuccessUrl { get; set; }

        public string CancelUrl { get; set; }
    }
}
