/*
 * Copyright 2018 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.KinesisTap.DiagnosticTool
{
    public class WindowsEventLogSimulator : LogSimulator, IDisposable
    {
        const string EVENT_SOURCE = "KTDiag.exe";
        private EventLog _log;

        public WindowsEventLogSimulator(string[] args) : base(1000, 1000, 1)
        {
            ParseOptionValues(args);

            string logName = args[1];
            string source = logName + "_" + EVENT_SOURCE;

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, logName);
            }
            _log = new EventLog(logName, ".", source);
        }

        protected override void WriteLog(string v)
        {
            int eventId = (int)(DateTime.Now.Ticks % ushort.MaxValue);
            _log.WriteEntry(v, EventLogEntryType.Information, eventId);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                base.Dispose(disposing);
                if (disposing)
                {
                    _log.Dispose();
                }
                disposedValue = true;
            }
        }
        #endregion
    }
}
