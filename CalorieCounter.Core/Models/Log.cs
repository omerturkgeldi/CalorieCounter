﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalorieCounter.Data.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Application { get; set; }
        public string MachineName { get; set; }
        public string Logged { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }

    }
}
