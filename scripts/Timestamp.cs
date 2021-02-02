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
			string utcDate = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Mock Training Data Timestamp Input")) {
				
				ele = driver.FindElement("xpath", "/html/body/div/main/form/div/div[1]/div[2]");
				data = ele.GetAttribute("value");

				DateTime dDate;

				if (DateTime.TryParse(data, out dDate))
				{
					String.Format("MM-DD-YYYY hh:mm:ss+ss:ss", dDate);
					log.Info("Verification Passed." + dDate + "is in the correct format"); 
				}
				else
				{
					log.Error("***Verification Failed." + dDate + "is NOT in the correct format");
					err.CreateVerificationError(step, step.Data);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
				}
			
				
			}
		}
	}
}