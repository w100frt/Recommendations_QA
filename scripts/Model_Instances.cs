using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;
using log4net;
using System.Threading;
using System.Collections.ObjectModel;
using System.Linq;

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
			string data = "";
			string xpath = "";
			int count = 0;
			VerifyError err = new VerifyError();
			string noInstancesTable = "";
			bool modelInstancesTable = false;
			
			if (step.Name.Equals("Check for Model Instances Table")) {
				count = driver.FindElements("xpath","/html/body/div/main/div[10]").Count;
				
				if (count > 0){
					log.Info("Model Instances Table EXISTS. Running Table Verification Template.");
					steps.Add(new TestStep(order, "Run Template for Table Verification", "ModelInstancesTable", "run_template", "xpath", "", wait));
				}
				else {
					log.Info("Model Instances Table DOES NOT EXIST. Verifying No Model Instances message.");
					steps.Add(new TestStep(order, "Verify No Table Text", "No Model instances are available for this configuration", "verify_value", "xpath", "//main[div[h2[.='Model Instances']]]//div[contains(@class,'text-center')]/span", wait));
				}
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("ID Row") || step.Name.Equals("Training Job ID Row") || step.Name.Equals("Status Row") || step.Name.Equals("Training Data Timestamp Row")) {
				switch (step.Name) {
					case "ID Row": 
						xpath = "/html/body/div/main/div[10]/table/tbody/tr/td[1]";
						break;
					case "Training Job ID Row":
						xpath = "/html/body/div/main/div[10]/table/tbody/tr/td[2]";
						break;
					case "Status Row": 
						xpath = "/html/body/div/main/div[10]/table/tbody/tr/td[3]";
						break;
					case "Training Data Timestamp Row":
						xpath = "/html/body/div/main/div[10]/table/tbody/tr/td[4]";
						break;
					default: 
						xpath = "/html/body/div/main/div[10]/table/tbody/tr/td[4]";
						break;
				}
				
				ele = driver.FindElement("xpath", xpath);
				data = ele.GetAttribute("textContent");
				int textLength1 = data.Length;
				log.Info(textLength1);
				if(textLength1 == 0) {
					log.Error("***Verification Failed. " + data + " is NOT text");
					err.CreateVerificationError(step, step.Name, data);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
				} 
				else {
					log.Info("Verification Passed. " + data + " is text");
				}
			}
			
			/*
			xpath = "/html/body/div/main/div[10]";
			ele = driver.FindElement("xpath", xpath);
			data = ele.GetAttribute("outerHTML");
			string text1 = data.ToString();
			int textLength = text1.Length;

			if (textLength > 1500) {
				modelInstancesTable = true;
			}
			else{ 
				modelInstancesTable = false;
			}
			log.Info(modelInstancesTable);
							
							
			if(modelInstancesTable == true){

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
					int textLength1 = text.Length;
					log.Info(textLength1);
					if(textLength1 == 0) {
						log.Error("***Verification Failed." + text + "is NOT text");
						err.CreateVerificationError(step, xpath, text);
						driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
					} 
					else {
						log.Info("Verification Passed." + text + "is text");

					}
				}
				else {
					xpath = "/html/body/div/main/div[10]/span";
					ele = driver.FindElement("xpath", xpath);
					data = ele.GetAttribute("textContent");
					string  text = data.ToString();
					log.Info(text);


					if(text != "No Model instances are available for this configuration") {
						log.Error("***Verification Failed. Table not present and incorrect message text.");
						err.CreateVerificationError(step, xpath, text);
						driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
					} 
					else {
						log.Info("Verification Passed. Table is not present but text is present: No Model instances are available for this configuration");

					}


				}
			}*/

			else {
				throw new Exception("Test Step not found in script");
			}

			
		}
	}
}