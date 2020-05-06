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
				title = "//ul[li[contains(.,'REGULAR SEASON')]]//li[not(contains(@class,'label'))]";
				total = driver.FindElements("xpath", title).Count;
				week = random.Next(1, total+1);

				steps.Add(new TestStep(order, "Capture Week", "WEEK", "capture", "xpath", "(" + title + ")["+ week +"]//div//div[1]", wait));
				steps.Add(new TestStep(order, "Capture Dates", "WEEK_DATES", "capture", "xpath", "(" + title + ")["+ week +"]//div//div[2]", wait));
				steps.Add(new TestStep(order, "Select Week", "", "click", "xpath", "(" + title + ")["+ week +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();	
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}