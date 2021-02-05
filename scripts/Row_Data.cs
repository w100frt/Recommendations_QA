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
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[3]/div/button";
				}
				else if (step.Name.Equals("Training Key Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[4]/div/button";
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
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[2]";
				}
				else if (step.Name.Equals("Hyperparameter Key Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[5]";
				}
				else if (step.Name.Equals("Status Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[6]";
				}
				
				ele = driver.FindElement("xpath", xpath);
				data = ele.GetAttribute("outerText");
				string  text = data.ToString();
				
				if(data is string) {
					log.Info("Verification Passed." + text + "is text");
				} 
				else {
					log.Error("***Verification Failed." + text + "is NOT text");
					err.CreateVerificationError(step, xpath, text);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);

				}
			}

			
			else if (step.Name.Equals("ID Row") || step.Name.Equals("ID Row 2")) {
				if (step.Name.Equals("ID Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[1]/a";
				}
				
				
				ele = driver.FindElement("xpath", xpath);
				data = ele.GetAttribute("href");
				if(data is string) {
					log.Info("Verification Passed." + data + "is a url");
				} 
				else {
					log.Error("***Verification Failed." + data + "is not a url");
					err.CreateVerificationError(step, data, xpath);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);

				}
			}

			else if (step.Name.Equals("Test ID") || step.Name.Equals("Test Name") || step.Name.Equals("Test Description") || 
				step.Name.Equals("Test Status") || step.Name.Equals("Test Training Engine Factory Type") 
				|| step.Name.Equals("Training Key ID") || step.Name.Equals("Training Key Name") || step.Name.Equals("Training Key Description") ||
				step.Name.Equals("Training Key Status") || step.Name.Equals("Training Key Class Name")) {
				if (step.Name.Equals("Test ID")) {
					xpath = "/html/body/div[1]/main/div/div[1]/table/tbody/tr[1]/td[3]/div/div/div/div/div[2]/form/div[1]/span";
					//*[@id="mcmodal-bc8e4227-3b40-48fc-9566-9cd67699005b"]/div/div/div[2]/form/div[1]/span
				}
				else if (step.Name.Equals("Test Name")) {
					xpath = "/html/body/div[1]/main/div/div[1]/table/tbody/tr[1]/td[3]/div/div/div/div/div[2]/form/div[2]/span";
				}
				else if (step.Name.Equals("Test Description")) {
					xpath = "/html/body/div[1]/main/div/div[1]/table/tbody/tr[1]/td[3]/div/div/div/div/div[2]/form/div[3]/span";
				}
				else if (step.Name.Equals("Test Status")) {
					xpath = "/html/body/div[1]/main/div/div[1]/table/tbody/tr[1]/td[3]/div/div/div/div/div[2]/form/div[4]/span";
				}
				else if (step.Name.Equals("Test Training Engine Factory Type")) {
					xpath = "/html/body/div[1]/main/div/div[1]/table/tbody/tr[1]/td[3]/div/div/div/div/div[2]/form/div[5]/span";
				}
				else if (step.Name.Equals("Training Key ID")) {
					xpath = "/html/body/div[1]/main/div/div[1]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[1]/span";
				}
				else if (step.Name.Equals("Training Key Name")) {
					xpath = "/html/body/div[1]/main/div/div[1]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[2]/span";
				}
				else if (step.Name.Equals("Training Key Description")) {
					xpath = "/html/body/div[1]/main/div/div[1]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[3]/span";
				}
				else if (step.Name.Equals("Training Key Status")) {
					xpath = "/html/body/div[1]/main/div/div[1]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[4]/span";
				}
				else if (step.Name.Equals("Training Key Class Name")) {
					xpath = "/html/body/div[1]/main/div/div[1]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[5]/span";
				}

				ele = driver.FindElement("xpath", xpath);
				data = ele.GetAttribute("textContent");
				string  text = data.ToString();
				
				if(data is string) {
					log.Info("Verification Passed." + text + "is text");
				} 
				else {
					log.Error("***Verification Failed." + text + "is NOT text");
					err.CreateVerificationError(step, xpath, text);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);

				}
			}

			else {
				throw new Exception("Test Step not found in script");
			}

			
		}
	}
}