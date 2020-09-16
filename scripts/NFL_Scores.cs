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
			int total;
			int week;
			int months;
			int year;
			string title;
			string date;
			string data = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			Random random = new Random();

			string[] preSeason = {"August"};
			string[] preSeasonWeeks = {"HALL OF FAME GAME", "PRE WEEK 1", "PRE WEEK 2", "PRE WEEK 3", "PRE WEEK 4"};
			string[] regSeason = {"September", "October", "November", "December", "January", "February"};
			string[] regSeasonWeek = {"WEEK 1", "WEEK 2", "WEEK 3", "WEEK 4", "WEEK 5", "WEEK 6", "WEEK 7", "WEEK 8", "WEEK 9", "WEEK 10", "WEEK 11", "WEEK 12", "WEEK 13", "WEEK 14", "WEEK 15", "WEEK 16", "WEEK 17"};
			string[] postSeason = {"January", "February"};
			string[] postSeasonWeeks = {"WILD CARD", "DIVISIONAL CHAMPIONSHIP", "CONFERENCE CHAMPIONSHIP", "PRO BOWL", "SUPER BOWL"};
			
			if (step.Name.Equals("Select Regular Season NFL Date")) {
				title = "//div[contains(@class,'scores') and not(@style='display: none;')][div[contains(@class,'dropdown')]]//ul[li[contains(.,'REGULAR SEASON')]]//li[not(contains(@class,'label'))]";
				total = driver.FindElements("xpath", title).Count;
				week = random.Next(1, total+1);

				steps.Add(new TestStep(order, "Capture Week", "WEEK", "capture", "xpath", "(" + title + ")["+ week +"]//div//div[1]", wait));
				steps.Add(new TestStep(order, "Capture Dates", "WEEK_DATES", "capture", "xpath", "(" + title + ")["+ week +"]//div//div[2]", wait));
				steps.Add(new TestStep(order, "Select Week", "", "click", "xpath", "(" + title + ")["+ week +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();	
			}
			
			else if (step.Name.Equals("Verify NFL Week")) {
				if (String.IsNullOrEmpty(step.Data)) {
					DateTime today = DateTime.Now;
					
					// determine week of season by today's date and time
					if (today >= DateTime.Parse("09/01/2020") && today < DateTime.Parse("09/15/2020 11:00:00")) {
						step.Data = "WEEK 1";
					}
					else if (today >= DateTime.Parse("09/15/2020 11:00:01") && today < DateTime.Parse("09/22/2020 11:00:00")) {
						step.Data = "WEEK 2";
					}
					/*else if (today >= DateTime(2020, 9, 22, 11, 0, 0) && today < DateTime(2020, 9, 29, 11, 0, 0)) {
						step.Data = "WEEK 3";
					}
					else if (today >= DateTime(2020, 9, 29, 11, 0, 0) && today < DateTime(2020, 10, 6, 11, 0, 0)) {
						step.Data = "WEEK 4";
					}
					else if (today >= DateTime(2020, 10, 6, 11, 0, 0) && today < DateTime(2020, 10, 13, 11, 0, 0)) {
						step.Data = "WEEK 5";
					}
					else if (today >= DateTime(2020, 10, 13, 11, 0, 0) && today < DateTime(2020, 10, 20, 11, 0, 0)) {
						step.Data = "WEEK 6";
					}
					else if (today >= DateTime(2020, 10, 20, 11, 0, 0) && today < DateTime(2020, 10, 27, 11, 0, 0)) {
						step.Data = "WEEK 7";
					}*/
					
					/*
					int now = time.Hours;
					int et = now - 4;
					if (et >= 0 && et < 11){
						log.Info("Current Eastern Time hour is " + et + ". Default to Yesterday.");
						step.Data = "YESTERDAY";
					}
					else {
						log.Info("Current Eastern Time hour is " + et + ". Default to Today.");
						step.Data = "TODAY";
					}*/				
				}
				else {
					step.Data = "SUPER BOWL";
				}

				steps.Add(new TestStep(order, "Verify Displayed Week on NFL", step.Data, "verify_value", "xpath", "//h2[contains(@class,'section-title fs-30 desktop-show') and not(@style='display: none;')]", wait));
				DataManager.CaptureMap["CURRENT"] = driver.FindElement("xpath", "//h2[contains(@class,'section-title fs-30 desktop-show') and not(@style='display: none;')]").Text;
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}