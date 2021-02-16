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
			
			if (step.Name.Equals("Algorithm Specification Row") || step.Name.Equals("Prediction Key Row") || 
			step.Name.Equals("Prediction Entity Mapping Row") || step.Name.Equals("Training Key Row")) {

				if (step.Name.Equals("Algorithm Specification Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[3]/div/button";
				}
				else if (step.Name.Equals("Training Key Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[4]/div/button";
				}
				else if (step.Name.Equals("Prediction Key Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[3]/div/button";
				}
				else if (step.Name.Equals("Prediction Entity Mapping Row")) {
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
			

			else if (step.Name.Equals("Name Row") || step.Name.Equals("Hyperparameter Key Row") || step.Name.Equals("Status Row") || 
			step.Name.Equals("Pred Status Row")) {
				if (step.Name.Equals("Name Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[2]";
				}
				else if (step.Name.Equals("Hyperparameter Key Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[5]";
				}
				else if (step.Name.Equals("Status Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[6]";
				}
				else if (step.Name.Equals("Pred Status Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[5]";
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

			
			else if (step.Name.Equals("ID Row") || step.Name.Equals("Model Configuration Name Row") ||step.Name.Equals("Pred ID Row")) {
				if (step.Name.Equals("ID Row")) {
					xpath = "//table[@class='entity-table']/tbody/tr[1]/td[1]/a";
				}
				else if (step.Name.Equals("Pred ID Row")) {
					xpath = "/html/body/div/main/div/div[2]/table/tbody/tr[1]/td[1]/a";
				}
				else if (step.Name.Equals("Model Configuration Name Row")) {
					xpath = "/html/body/div/main/div/div[2]/table/tbody/tr[1]/td[2]/a";
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
				|| step.Name.Equals("Training Key ID") || step.Name.Equals("Training Key Name") || step.Name.Equals("Training Key Description") 
				|| step.Name.Equals("Training Key Status") || step.Name.Equals("Model Configuration ID Data") || step.Name.Equals("Model Configuration Name Data") 
				|| step.Name.Equals("Published Data")  || step.Name.Equals("Published Date Data") || step.Name.Equals("Hyperparameter Key Data") 
				|| step.Name.Equals("ID Row Data") || step.Name.Equals("Training Job ID Row") || step.Name.Equals("Status Row") || step.Name.Equals("Key ID") 
				|| step.Name.Equals("Key Class Name") || step.Name.Equals("Key Name") || step.Name.Equals("Key Description") || step.Name.Equals("Key Status") 
				|| step.Name.Equals("Entity ID") || step.Name.Equals("Entity Name") || step.Name.Equals("Entity Deployment Specification") 
				|| step.Name.Equals("Prediction Entity Type") || step.Name.Equals("Target Prediction Entity Type") || step.Name.Equals("Sample Controller Factory Type") 
				|| step.Name.Equals("Entity Status") || step.Name.Equals("Prediction Key ID") || step.Name.Equals("Prediction Key Name") 
				|| step.Name.Equals("Prediction Key Class Name") || step.Name.Equals("Prediction Key Description") || step.Name.Equals("Prediction Key Status")
				|| step.Name.Equals("Prediction Entity Mapping ID") || step.Name.Equals("Prediction Entity Mapping Name") || step.Name.Equals("Prediction Entity Mapping Class Name") 
				|| step.Name.Equals("Prediction Entity Mapping Deployment Specification") || step.Name.Equals("PEM Prediction Entity Type") || step.Name.Equals("PEM Target Prediction Entity Type") 
				|| step.Name.Equals("PEM Sample Controller Factory Type") || step.Name.Equals("Prediction Configuration Name Data") || step.Name.Equals("Prediction Configuration ID Data") 
				|| step.Name.Equals("Prediction Entity Mapping Status")) {
				if (step.Name.Equals("Test ID")) {
					xpath = "/html/body/div[1]/main/div/div[1]/table/tbody/tr[1]/td[3]/div/div/div/div/div[2]/form/div[1]/span";
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
				else if (step.Name.Equals("Model Configuration ID Data")) {
					xpath = "/html/body/div/main/div[2]/div[2]/span";
				}
				else if (step.Name.Equals("Model Configuration Name Data")) {
					xpath = "/html/body/div/main/div[3]/div[2]/span";
				}
				else if (step.Name.Equals("Published Data")) {
					xpath = "/html/body/div/main/div[6]/div[2]/span";
				}
				else if (step.Name.Equals("Published Date Data")) {
					xpath = "/html/body/div/main/div[7]/div[2]/span";
				}
				else if (step.Name.Equals("Hyperparameter Key Data")) {
					xpath = "/html/body/div/main/div[8]/div[2]/span";
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
				else if (step.Name.Equals("Key ID")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[3]/div/div/div/div/div[2]/form/div[1]/span";
				}
				else if (step.Name.Equals("Key Name")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[3]/div/div/div/div/div[2]/form/div[2]/span";
				}
				else if (step.Name.Equals("Key Class Name")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[3]/div/div/div/div/div[2]/form/div[3]/span";
				}
				else if (step.Name.Equals("Key Description")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[3]/div/div/div/div/div[2]/form/div[4]/span";
				}
				else if (step.Name.Equals("Key Status")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[3]/div/div/div/div/div[2]/form/div[5]/span";
				}
				else if (step.Name.Equals("Entity ID")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[1]/span";
				}
				else if (step.Name.Equals("Entity Name")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[2]/span";
				}
				else if (step.Name.Equals("Entity Deployment Specification")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[3]/span";
				}
				else if (step.Name.Equals("Prediction Entity Type")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[4]/span";
				}
				else if (step.Name.Equals("Target Prediction Entity Type")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[5]/span";
				}
				else if (step.Name.Equals("Sample Controller Factory Type")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[6]/span";
				}
				else if (step.Name.Equals("Entity Status")) {
					xpath = "/html/body/div[1]/main/div/div[2]/table/tbody/tr[1]/td[4]/div/div/div/div/div[2]/form/div[7]/span";
				}
				else if (step.Name.Equals("Prediction Key ID")) {
					xpath = "/html/body/div[1]/main/div[6]/div[2]/span/div/div/div/div[2]/form/div[1]/span";
				}
				else if (step.Name.Equals("Prediction Key Name")) {
					xpath = "/html/body/div[1]/main/div[6]/div[2]/span/div/div/div/div[2]/form/div[2]/span";
				}
				else if (step.Name.Equals("Prediction Key Class Name")) {
					xpath = "/html/body/div[1]/main/div[6]/div[2]/span/div/div/div/div[2]/form/div[3]/span";
				}
				else if (step.Name.Equals("Prediction Key Description")) {
					xpath = "/html/body/div[1]/main/div[6]/div[2]/span/div/div/div/div[2]/form/div[4]/span";
				}
				else if (step.Name.Equals("Prediction Key Status")) {
					xpath = "/html/body/div[1]/main/div[6]/div[2]/span/div/div/div/div[2]/form/div[5]/span";
				}
				else if (step.Name.Equals("Prediction Entity Mapping ID")) {
					xpath = "/html/body/div[1]/main/div[7]/div[2]/span/div/div/div/div[2]/form/div[1]/span";
				}
				else if (step.Name.Equals("Prediction Entity Mapping Name")) {
					xpath = "/html/body/div[1]/main/div[7]/div[2]/span/div/div/div/div[2]/form/div[2]/span";
				}
				else if (step.Name.Equals("Prediction Entity Mapping Deployment Specification")) {
					xpath = "/html/body/div[1]/main/div[7]/div[2]/span/div/div/div/div[2]/form/div[3]/span";
				}
				else if (step.Name.Equals("PEM Prediction Entity Type")) {
					xpath = "/html/body/div[1]/main/div[7]/div[2]/span/div/div/div/div[2]/form/div[4]/span";
				}
				else if (step.Name.Equals("PEM Target Prediction Entity Type")) {
					xpath = "/html/body/div[1]/main/div[7]/div[2]/span/div/div/div/div[2]/form/div[5]/span";
				}
				else if (step.Name.Equals("PEM Sample Controller Factory Type")) {
					xpath = "/html/body/div[1]/main/div[7]/div[2]/span/div/div/div/div[2]/form/div[6]/span";
				}
				else if (step.Name.Equals("Prediction Entity Mapping Status")) {
					xpath = "/html/body/div[1]/main/div[7]/div[2]/span/div/div/div/div[2]/form/div[7]/span";
				}
				else if (step.Name.Equals("Prediction Configuration Name Data")) {
					xpath = "/html/body/div/main/div[3]/div[2]/span";
				}
				else if (step.Name.Equals("Prediction Configuration ID Data")) {
					xpath = "/html/body/div/main/div[4]/div[2]/span";
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