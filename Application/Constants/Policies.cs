using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Constants
{
    public static class Policies
    {
        public const string UserOnly = "UserOnly";
        public const string BrokerOnly = "BrokerOnly";
        public const string AdminOnly = "AdminOnly";
        public const string AdminOrBroker = "AdminOrBroker";
        public const string SuperAdminOnly = "SuperAdminOnly";
    }
}
