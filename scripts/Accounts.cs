using System;
using System.Globalization;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;
using log4net;
using System.Threading;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{		
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public void Execute(DriverManager driver, TestStep step)
		{
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
			List<TestStep> steps = new List<TestStep>();
			IWebElement ele;
			int size;
			string data = "";
			string test = "";
			bool stop = true;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Get or Compare Device ID")) {
				try {
					test = (string) js.ExecuteScript("return document.readyState;");
					data = (string) js.ExecuteScript("return window.wisRegistration.getDeviceID();");
					stop = (bool) js.ExecuteScript("return window.wisRegistration.isUserLoggedIn();");
					
					log.Info("readyState: " + test);
					log.Info("isuserloggedin: " + stop);
					log.Info("device id: " + data);
				}
				catch (Exception e) {
					log.Info("ERROR: " + e);
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}