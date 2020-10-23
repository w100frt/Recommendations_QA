using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			string data = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			int number = 1;
			ReadOnlyCollection<IWebElement> elements;
			int count = 0;
			int bars = 0;
			
			if (step.Name.Equals("Verify Event Odds Details by Number")) {
				bool numeric = int.TryParse(step.Data, out number);
				
				steps.Add(new TestStep(order, "Verify Number of Header Items", "3", "verify_count", "xpath", "(//div[contains(@class,'event')]//a)["+ number +"]//div[contains(@class,'event-card-header')]//div[contains(@class,'flex-col')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				
				for (int i = 1; i <= 3; i++) {
					switch(i) {
						case 1: 
							if (DataManager.CaptureMap["SPORT"].Equals("MLB")) {
								data = "RUN LINE";
							}
							else {
								data = "SPREAD";	
							}
							break;
						case 2: 
							data = "TEAM TO WIN";
							break;
						default:
							data = "TOTAL";
							break;
					}
					
					if (numeric) {
						number = Int32.Parse(step.Data);
						steps.Add(new TestStep(order, "Verify First Slide Odds Header", data, "verify_value", "xpath", "(//div[contains(@class,'event-container')])["+ number +"]//div[contains(@class,'feed-component')]//li[" + i + "]//div[contains(@class,'chart-container-header')]//div[contains(@class,'text fs')]", wait));
						steps.Add(new TestStep(order, "Verify Sub Header Text Exists", "", "verify_displayed", "xpath", "(//div[contains(@class,'event-container')])[" + number +"]//div[contains(@class,'feed-component')]//li[" + i + "]//div[contains(@class,'chart-container-header')]//div[contains(@class,'sub-header')]", wait));
						steps.Add(new TestStep(order, "Verify Odds Numbers Exist", "", "verify_displayed", "xpath", "((//div[contains(@class,'event-container')])[" + number + "]//div[contains(@class,'feed-component')]//li[" + i + "]//div[contains(@class,'chart-container-header')]//div[contains(@class,'number')])[1]", wait));
						steps.Add(new TestStep(order, "Verify Odds Numbers Exist", "", "verify_displayed", "xpath", "((//div[contains(@class,'event-container')])["+ number +"]//div[contains(@class,'feed-component')]//li[" + i + "]//div[contains(@class,'chart-container-header')]//div[contains(@class,'number')])[2]", wait));
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();
					}
					if (i != 3) {
						steps.Add(new TestStep(order, "Click Arrow Right", "", "click", "xpath", "(//button[contains(@class,'next')])["+ number +"]", wait));
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();
					}
				}
			}
			
			else if (step.Name.Equals("Click Prop Header By Name")) {
				DataManager.CaptureMap["PROP"] = step.Data;
				steps.Add(new TestStep(order, "Click " + step.Data, "", "click", "xpath", "//div[contains(@class,'prop-bets-name') and contains(.,'"+ step.Data.ToUpper()+"')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				
				ele = driver.FindElement("xpath","//div[contains(@class,'prop-bets-component')][div[contains(.,'"+ DataManager.CaptureMap["PROP"].ToUpper() +"')]]");
                js.ExecuteScript("arguments[0].scrollIntoView(true);", ele);
			}
			
			else if (step.Name.Equals("Verify Number of Current Prop Displayed")) {
				steps.Add(new TestStep(order, "Verify Number of " + DataManager.CaptureMap["PROP"], step.Data, "verify_count", "xpath", "//div[contains(@class,'prop-bets-component')][div[contains(.,'"+ DataManager.CaptureMap["PROP"].ToUpper() +"')]]//tbody/tr", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Click See All Futures for Current Prop")) {
				steps.Add(new TestStep(order, "Click See All " + DataManager.CaptureMap["PROP"], "", "click", "xpath", "//div[contains(@class,'prop-bets-component')][div[contains(.,'"+ DataManager.CaptureMap["PROP"].ToUpper() +"')]]//a[contains(.,'SEE ALL')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify Team Text Exists for Current Prop")) {
				elements = driver.FindElements("xpath","//div[contains(@class,'prop-bets-component')][div[contains(.,'"+ DataManager.CaptureMap["PROP"].ToUpper() +"')]]//div[contains(@class,'flex')]");
				
				foreach(IWebElement e in elements) {
					if (!String.IsNullOrEmpty(e.GetAttribute("innerText"))) {
						log.Info("Verification PASSED. Element text is " + e.GetAttribute("innerText") + ".");
					}
					else {
						log.Error("***Verification FAILED. Element returned no text. ***");
						err.CreateVerificationError(step, "Team Text Expected", e.GetAttribute("innerText"));
						driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
					}
				}
			}
			
			else if (step.Name.Equals("Verify Odds Text Exists for Current Prop")) {
				elements = driver.FindElements("xpath","//div[contains(@class,'prop-bets-component')][div[contains(.,'"+ DataManager.CaptureMap["PROP"].ToUpper() +"')]]//span[contains(@class,'ff')]");
				
				foreach(IWebElement e in elements) {
					if (!String.IsNullOrEmpty(e.GetAttribute("innerText"))) {
						log.Info("Verification PASSED. Element text is " + e.GetAttribute("innerText") + ".");
					}
					else {
						log.Error("***Verification FAILED. Element returned no text. ***");
						err.CreateVerificationError(step, "Team Text Expected", e.GetAttribute("innerText"));
						driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
					}
				}
			}	
			
			else if (step.Name.Equals("Verify Number of Event Props")) {
				count = driver.FindElements("xpath","//div[contains(@class,'prop-bets-component')]/div[contains(@class,'prop-bets-event-title') or contains(.,'PROPS')]").Count;
				
				if (count >= 1 && count <= 5) {
					log.Info ("Verication PASSED. " + count + " is between 1 and 5.");
				}
				else {
					log.Error("***Verification FAILED. Props expected to be between 1 and 5. Actual total is " + count +  ". ***");
					err.CreateVerificationError(step, "Between 1 and 5", count.ToString());
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}