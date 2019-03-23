// ***********************************************************************
// File Name        : MapConfigInfo.cs
// Author           : chipenggang
// Create Date      : 2019-03-16
//
// Description      ：the map config info
// ***********************************************************************
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Trains.Utils;

namespace Trains.Config
{
    /// <summary>
    /// map config info
    /// </summary>
    public class MapConfigInfo
    {        
        /// <summary>
        /// file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// path of file
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// get map config info 
        /// </summary>
        /// <returns>map info</returns>
        public string GetMapConfigInfo()
        {
            while (true)
            {
                var builder = new ConfigurationBuilder()
                                            .AddJsonFile("appsettings.json");
                var configuration = builder.Build();

                configuration.GetSection("MapFileInfo").Bind(this);
                if (string.IsNullOrWhiteSpace(FileName))
                {
                    Console.Write("the fileName not config, please check the config info");
                    WhetherContinueOrNot();
                }
                else
                {
                    string filePath = string.IsNullOrWhiteSpace(Path) ? AppContextUtil.GetCurrentBaseDirectory() : Path;
                    var fileName = filePath.TrimEnd('\\') + "\\" + FileName;
                    try
                    {
                        string lineMapStr = FileUtil.ReadFromFile(fileName);
                        return lineMapStr;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("read fail, please check if the file exists.");
                        WhetherContinueOrNot();
                    }
                }
            }
        }

        /// <summary>
        /// Whether Continue Or Not
        /// if y then continue else exit
        /// </summary>
        private void WhetherContinueOrNot()
        {
            Console.Write("input [y] to continue and reload config info(othe key will exit):");
            if (Console.ReadLine().ToLower() != "y")
            {
                Environment.Exit(0);
            }
        }
        
        /// <summary>
        /// to string
        /// </summary>
        /// <returns>object string info</returns>
        public override string ToString()
        {
            return $"MapConfigInfo:{{FileName:{FileName}, Path:{Path}}}";
        }
    }
}
