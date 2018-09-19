﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFxBook
{
    public interface IMyFxBookLogger
    {
        void LogWarning(string message);
        void LogError(string message);
        void LogError(Exception exception, string message);
    }
}
