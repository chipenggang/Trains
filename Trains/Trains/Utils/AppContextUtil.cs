// ***********************************************************************
// File Name        : AppDomainUtil.cs
// Author           : chipenggang
// Create Date      : 2019-03-16
//
// Description      ：operate file helper
// 
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Trains.Utils
{
    /// <summary>
    /// get or operate AppDomain
    /// </summary>
    public class AppContextUtil
    {
        /// <summary>
        /// get current base directory
        /// </summary>
        /// <returns>base directory path</returns>
        public static string GetCurrentBaseDirectory()
        {
            return AppContext.BaseDirectory;
        }
    }
}
