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
			
			if (step.Name.Equals("Algorithm Specification Row") || step.Name.Equals("Training Key Row")) {
				if (step.Name.Equals("Algorithm Specification Row")) {
					xpath = "/html/body/div/main/div/div[1]/table/tbody/tr[1]/td[3]/div/button";
				}
				else if (step.Name.Equals("Training Key Row")) {
					xpath = "/html/body/div/main/div/div[1]/table/tbody/tr[1]/td[4]/div/button";
				}
				
				ele = driver.FindElement("xpath", xpath);
				data = ele.GetAttribute("type");

				if(data == "button") {
					log.Info("Verification Passed." + data + "is a button ");
				} 
				else {
					log.Error("***Verification Failed." + data + "is NOT a button");
					err.CreateVerificationError(step, xpath, data);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);

				}
				
			}
			

			else if (step.Name.Equals("Name Row") || step.Name.Equals("Hyperparameter Key Row") || step.Name.Equals("Status Row")) {
				if (step.Name.Equals("Name Row")) {
					xpath = "/html/body/div/main/div/div[1]/table/tbody/tr[1]/td[2]";
				}
				else if (step.Name.Equals("Hyperparameter Key Row")) {
					xpath = "/html/body/div/main/div/div[1]/table/tbody/tr[1]/td[5]";
				}
				else if (step.Name.Equals("Status Row")) {
					xpath = "/html/body/div/main/div/div[1]/table/tbody/tr[1]/td[6]";
				}
				
				ele = driver.FindElement("xpath", xpath);
				string  text = ele.ToString();
				log.Info(text);
				
				if(data is string) {
					log.Info("Verification Passed." + data + "is text");
				} 
				else {
					log.Error("***Verification Failed." + data + "is NOT text");
					err.CreateVerificationError(step, xpath, data);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);

				}
			}

			else if (step.Name.Equals("ID Row") || step.Name.Equals("ID Row 2")) {
				if (step.Name.Equals("ID Row")) {
					xpath = "/html/body/div/main/div/div[1]/table/tbody/tr[1]/td[1]/a[1]";
				}
				
				
				ele = driver.FindElement("xpath", xpath);
				string  idUrl = ele.ToString();
				log.Info(idUrl);


				if(idUrl.Contains("href")) {
					log.Info("Verification Passed." + data + "is a link");
				} 
				else {
					log.Error("***Verification Failed." + data + "is NOT a link");
					err.CreateVerificationError(step, xpath, data);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);

				}
			}


			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}