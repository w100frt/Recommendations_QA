using System;
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
			string data = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			bool withinSeven = false;
			
			if (step.Name.Equals("Verify Countdown Clock Within 7 Days")) {
				if (DataManager.CaptureMap.ContainsKey("CURRENT")) {
					log.Info("Game date: " + DataManager.CaptureMap["CURRENT"]);
					if (DataManager.CaptureMap["CURRENT"].Equals("TODAY") || DataManager.CaptureMap["CURRENT"].Equals("TOMORROW")) {
						withinSeven = true;
					}
					else {
						var week = DateTime.Now.AddDays(+7);
						log.Info("Seven days from now : " + week.ToString("ddd, MMM d").ToUpper());
						if (week >= DateTime.Parse(DataManager.CaptureMap["CURRENT"])) {
							log.Info("within week");
							withinSeven = true;
						}
						else {
							log.Info("not within week");
							withinSeven = false;
						}
					}
				}
				else {
					
					log.Info("No Key for CURRENT");
				}

				if (withinSeven) {
					steps.Add(new TestStep(order, "Verify Countdown is Displayed", "", "verify_displayed", "xpath", "//div[contains(@class,'countdown-timer')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}				
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}