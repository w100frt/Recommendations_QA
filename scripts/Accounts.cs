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
			bool stop = false;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Get or Compare Device ID")) {
				try {
					js.ExecuteScript("wisRegistration.getDeviceID();", data);
					log.Info("device id");
				}
				catch (Exception e) {
					log.Info("failed");
					log.Info("ERROR ERROR ERROR: " + e);
				}
				//.ToString();
				log.Info(data);
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}