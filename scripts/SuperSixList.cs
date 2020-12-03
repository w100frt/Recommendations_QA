using System;
using System.Globalization;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;
using System.Collections.ObjectModel;
using System.IO;

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
            var path = Path.Combine(Directory.GetCurrentDirectory());
            path = Path.Combine(path, "..");
            log.Info("Current Directory: " + path);
        }
    }
}