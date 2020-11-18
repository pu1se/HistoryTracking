using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HistoryTracking.DAL.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserType
    {
        Customer,
        Reseller,
        Distributor,
        SystemUser
    }
}
