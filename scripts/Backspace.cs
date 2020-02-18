using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{
		public void Execute(DriverManager driver, TestStep step)
		{
			var element = driver.FindElement("xpath", "//input[@placeholder='Search a provider']").SendKeys(Keys.Control, "a");
			log.Info("Selecting all text");
			element.SendKeys(Keys.Delete);
			log.Info("Removing all text");
		}
	}
}