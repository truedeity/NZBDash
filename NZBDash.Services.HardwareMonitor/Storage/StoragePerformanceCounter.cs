﻿#region Copyright
// /************************************************************************
//   Copyright (c) 2016 NZBDash
//   File: StoragePerformanceCounter.cs
//   Created By: Jamie Rees
//  
//   Permission is hereby granted, free of charge, to any person obtaining
//   a copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//  
//   The above copyright notice and this permission notice shall be
//   included in all copies or substantial portions of the Software.
//  
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//   EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//   NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//   LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//   OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//   WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ************************************************************************/
#endregion
using System.Linq;

using NZBDash.Core.Interfaces;
using NZBDash.Services.Monitor.Interfaces;

namespace NZBDash.Services.Monitor.Storage
{
    public class StoragePerformanceCounter : IPerformanceCounter
    {
        public StoragePerformanceCounter(IHardwareService hardwareService, int driveId)
        {
            Service = hardwareService;
            DriveId = driveId;
        }
        private IHardwareService Service { get; set; }
        private int DriveId { get; set; }
        public double Value
        {
            get
            {
                var drives = Service.GetDrives();
                var drive = drives.FirstOrDefault(x => x.DriveId == DriveId);
                if (drive != null)
                {
                    return drive.PercentageFilled;
                }
                return 0;
            }
        }
    }
}
