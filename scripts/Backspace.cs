using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		public void Execute(DriverManager driver, TestStep step)
		{
			var element = driver.FindElement("xpath", "//input[@placeholder='Search a provider']").SendKeys("^(a)");
			log.Info("Selecting all text");
			element.SendKeys(Keys.Delete);
			log.Info("Removing all text");
		}
	}
}