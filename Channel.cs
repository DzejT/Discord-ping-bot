using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace resender
{
    public class Channel
    {
        public ulong from { get; set; }
        public ulong to { get; set; }
    }
}
