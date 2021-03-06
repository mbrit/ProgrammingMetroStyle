﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    public interface IReportPageViewModel : IViewModelSingleton<ReportViewItem>
    {
        ICommand EditCommand { get; }
        ICommand OpenMapCommand { get; }
    }
}
