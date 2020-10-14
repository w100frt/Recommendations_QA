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
			string data;
			string url = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify Main Nav Link Values")) {
				string[] dataSet = {"HOME", "SCORES", "LIVE TV", "STORIES", "SEARCH", "SIGN IN", "Account"};
				elements = driver.FindElements("xpath", "//ul[@class='nav']//li[contains(@class,'desktop-show')]//span[contains(@class,'nav-item-text')]");
				
				if(dataSet.Length != elements.Count) {
					log.Error("Unexpected element count. Expected: [" + dataSet.Length + "] does not match Actual: [" + elements.Count + "]");
					err.CreateVerificationError(step, dataSet.Length.ToString(), elements.Count.ToString());
				}
				else {
					for (int i=0; i < elements.Count; i++) {
						if(dataSet[i].Equals(elements[i].GetAttribute("innerText").Trim())) {
							log.Info("Verification Passed. Expected [" + dataSet[i] + "] matches Actual [" + elements[i].GetAttribute("innerText").Trim() +"]");
						}
						else {
							log.Error("Verification FAILED. Expected: [" + dataSet[i] + "] does not match Actual: [" + elements[i].GetAttribute("innerText").Trim() + "]");
							err.CreateVerificationError(step, dataSet[i], elements[i].GetAttribute("innerText").Trim());
						}
					}
				}
			}
			
			else if (step.Name.Equals("Verify URL Contains String")) {
				url = driver.GetDriver().Url.ToString();
				if (url.Contains(step.Data)) {
					log.Info("Verification Passed. Expected [" + step.Data + "]" + " can be found in Actual URL [" + url + "]");
				}
				else {
					log.Error("Verification FAILED.*** Expected: [" + step.Data + "] is not within Actual URL [" + url + "]");
					err.CreateVerificationError(step, step.Data, url);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
				}
			}
			
			else if (step.Name.Equals("Store Sport by Data")) {
				DataManager.CaptureMap["SPORT"] = step.Data;
				log.Info("Storing " + step.Data + "to capture map as SPORT...");
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}