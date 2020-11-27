﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.Enums
{
    public enum OrderStatusType
    {
        Draft,
        WaitingForApproval,
        InUse,
        Suspended,
        Deleted
    }
}
