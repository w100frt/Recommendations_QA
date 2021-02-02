using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;
using log4net;
using System.Threading;
using System.Collections.ObjectModel;

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
			ReadOnlyCollection<IWebElement> elements;
			string data = "";
			string xpath = "";
			string url = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Mock Training Data Timestamp Input")) {
				
				var MockTimestampA = driver.FindElements("xpath", "/html/body/div/main/form/div/div[1]/div[2]/input");
			
				if (MockTimestampA = "MM-DD-YYYY hh:mm:ss+ss:ss"){
					log.Info("Verification Passed. Date Format Correct");
				}
				else {
					log.Error("Date format incorrect");
					err.CreateVerificationError(step, "date format incorrect");
					}
			}
		}
	}
}
