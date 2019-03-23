// ***********************************************************************
// File Name        : Station.cs
// Author           : chipenggang
// Create Date      : 2019-03-16
//
// Description      ：operate file helper
// 
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Trains.Utils
{
    /// <summary>
    /// operate file helper
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// read from file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFromFile(string path)
        {
            StringBuilder stringBuilder = new StringBuilder();
            StreamReader streamReader = new StreamReader(path, Encoding.Default);
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                stringBuilder.Append(line.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}
