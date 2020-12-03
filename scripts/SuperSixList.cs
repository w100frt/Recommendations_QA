using System;
using System.Globalization;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json.Linq;

namespace SeleniumProject.Function
{
    public class Script : ScriptingInterface.IScript
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(DriverManager driver, TestStep step)
        {
            long order = step.Order;
            string wait = step.Wait != null ? step.Wait : "";
            IWebElement ele;
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
            var path = Path.Combine(@"C:\Users\truon\source\repos\New_Selenium.hao\SeleniumProject\Postman_Collection\report.json");
            log.Info("Current Directory: " + path);

            var list = JObject.Parse(File.ReadAllText(path));
            log.info(list);
            //foreach (JToken x in list["list"])
            //{
            //    if (x["league"].ToString() == DataManager.CaptureMap["SPORT"])
            //    {
            //        foreach (String id in x["ids"])
            //        {
            //            log.Info(DataManager.CaptureMap["SPORT"] + " game id:" + id);
            //        }
            //    }
            //}

        }
    }
}