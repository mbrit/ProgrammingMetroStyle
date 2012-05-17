﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public interface IEnsureTestReportsServiceProxy : IServiceProxy
    {
        Task EnsureTestReports(Action success, FailureHandler failure, Action complete = null);
    }
}
