// ***********************************************************************
// File Name        : BusinessException.cs
// Author           : chipenggang
// Create Date      : 2019-03-16
//
// Description      ：throw some business exception, and the caller will processing by different situation
// 
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Trains.Domain
{
    /// <summary>
    /// business exception
    /// </summary>
    public class BusinessException : Exception
    {
        /// <summary>
        /// exception code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public BusinessException()
            : base()
        {
            /* nothing to do */
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message">error message</param>
        public BusinessException(string message) 
            : base(message)
        {
            /* nothing to do */
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="code">error code</param>
        public BusinessException(string message, int code)
            : base(message)
        {
            this.Code = code;
        }

        /// <summary>
        /// invoke function with try
        /// </summary>
        /// <param name="func">delegate functon</param>
        /// <returns>function result</returns>
        public static string TryRun(Func<int> func)
        {
            try
            {
                return func.Invoke().ToString();
            }
            catch (BusinessException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return "System Error";
            }
        }
    }
}