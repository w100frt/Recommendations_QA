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
			IWebElement ele;
			string data = "";
			string xpath = "";
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("ID") || step.Name.Equals("Training Job ID") || step.Name.Equals("Status") || step.Name.Equals("Training Data Timestamp") 
			|| step.Name.Equals("ID Row Data") || step.Name.Equals("Training Job ID Row") || step.Name.Equals("Status Row") 
				|| step.Name.Equals("Training Data Timestamp Row")) {
				if (step.Name.Equals("ID")) {
					xpath = "/html/body/div/main/div[10]/table/thead/tr/th[1]";
				}
				else if (step.Name.Equals("Training Job ID")) {
					xpath = "/html/body/div/main/div[10]/table/thead/tr/th[2]";
				}
				else if (step.Name.Equals("Status")) {
					xpath = "/html/body/div/main/div[10]/table/thead/tr/th[3]";
				}
				else if (step.Name.Equals("Training Data Timestamp")) {
					xpath = "/html/body/div/main/div[10]/table/thead/tr/th[4]";
				}
				else if (step.Name.Equals("ID Row Data")) {
					xpath = "/html/body/div/main/div[10]/table/tbody/tr/td[1]";
				}
				else if (step.Name.Equals("Training Job ID Row")) {
					xpath = "/html/body/div/main/div[10]/table/tbody/tr/td[2]";
				}
				else if (step.Name.Equals("Status Row")) {
					xpath = "/html/body/div/main/div[10]/table/tbody/tr/td[3]";
				}
				else if (step.Name.Equals("Training Data Timestamp Row")) {
					xpath = "/html/body/div/main/div[10]/table/tbody/tr/td[4]";
				}

				ele = driver.FindElement("xpath", xpath);
				data = ele.GetAttribute("textContent");
				string  text = data.ToString();
				int textLength = text.Length;
				log.Info(textLength);

				if(textLength == 0) {
					log.Error("***Verification Failed." + text + "is NOT text");
					err.CreateVerificationError(step, xpath, text);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
				} 
				else {
					log.Info("Verification Passed." + text + "is text");

				}
			}

			else {
				throw new Exception("Test Step not found in script");
			}

			
		}
	}
}