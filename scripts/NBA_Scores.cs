using System;
using System.Globalization;
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
			IWebElement chip;
			bool in_season = false;
			string path = "";
			int loc;
			int months;
			int year;
			string title;
			string date;
			string data = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			Random random = new Random();
			
			if (step.Name.Equals("Select Regular Season NBA Date")) {
				string[] regularSeason = new string[] {"September", "October", "November", "December", "January", "February", "March", "April"};
				DateTime now = DateTime.Now;
				date = now.ToString("MMMM");
				
				// check if current month is in the regular season
				if (Array.Exists(regularSeason, element => element == date)) {
					loc = Array.IndexOf(regularSeason, date);
					if (loc == 0 || loc == regularSeason.Length-1) {
						// current month is start or end of regular season. can only click one way on arrows.
						months = random.Next(2, regularSeason.Length);
						if(loc == 0) {
							for (int i = 0; i < months; i++) {
								steps.Add(new TestStep(order, "Click Arrow Right", "", "click", "xpath", "//div[@class='qs-arrow qs-right']", wait));
								TestRunner.RunTestSteps(driver, null, steps);
								steps.Clear();									
							}
						}
						else {
							for (int i = 0; i < months; i++) {
								steps.Add(new TestStep(order, "Click Arrow Left", "", "click", "xpath", "//div[@class='qs-arrow qs-left']", wait));
								TestRunner.RunTestSteps(driver, null, steps);
								steps.Clear();									
							}								
						}
					}
					else {
						// current month is inside limits of regular season. can click both arrows.
						log.Info(loc);
					}							
					steps.Add(new TestStep(order, "Capture Month", "MONTH", "capture", "xpath", "//span[contains(@class,'qs-month')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				else {
					// month is not in regular season. 
					// navigate to most recent month of season. assume end of last season.
					//div[@class='qs-arrow qs-left']
					// check if current month is in regular season
				}
				months = driver.FindElements("xpath", "//div[contains(@class,'qs-num')]").Count; 
				months = random.Next(1, months+1);
				steps.Add(new TestStep(order, "Capture Date", "DATE", "capture", "xpath", "(//div[contains(@class,'qs-num')])["+ months +"]", wait));
				steps.Add(new TestStep(order, "Select Date", "", "click", "xpath", "(//div[contains(@class,'qs-num')])["+ months +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();	
			}
			
			else if (step.Name.Equals("Verify NBA Date")) {
				if (String.IsNullOrEmpty(step.Data)) {
					if(DataManager.CaptureMap.ContainsKey("IN_SEASON")) {
						in_season = bool.Parse(DataManager.CaptureMap["IN_SEASON"]);
						if(in_season) {
							TimeSpan time = DateTime.UtcNow.TimeOfDay;
							int now = time.Hours;
							int et = now - 4;
							if (et >= 0 && et < 11){
								log.Info("Current Eastern Time hour is " + et + ". Default to Yesterday.");
								step.Data = "YESTERDAY";
							}
							else {
								log.Info("Current Eastern Time hour is " + et + ". Default to Today.");
								step.Data = "TODAY";
							}				
						}
						else {
							step.Data = "WED, OCT 14";
						}
					}
					else {
						log.Warn("No IN_SEASON variable available.");
					}
				}
				
				path = "//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//div[contains(@class,'week-selector')]//button/span[contains(@class,'title')]";
				steps.Add(new TestStep(order, "Verify Displayed Day on NBA", step.Data, "verify_value", "xpath", path, wait));
				DataManager.CaptureMap["CURRENT"] = driver.FindElement("xpath", path).Text;
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}