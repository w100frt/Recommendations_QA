using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;
using log4net;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		public void Execute(DriverManager driver, TestStep step)
		{
			IWebElement element = driver.FindElement("xpath", "//input[@placeholder='Search a provider']");
			element.Click();
			element.Clear();
			element.SendKeys(" ");
			element.SendKeys(Keys.Backspace);
			log.Info("Removing all text");
		} 
	}
}