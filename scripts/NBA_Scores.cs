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
			IWebElement chip;
			int loc;
			int months;
			string title;
			string date;
			string data;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			Random random = new Random();
			
			if (step.Name.Equals("Select Regular Season NBA Date")) {
				string[] regularSeason = new string[] {"September", "October", "November", "December", "January", "February", "March", "April"};
				DateTime now = DateTime.Now;
				date = now.ToString("MMMM");
				
				// check if current month is in the regular season
				if (Array.Exists(regularSeason, element => element == data)) {
					loc = Array.IndexOf(regularSeason, data);
					if (loc == 0 || loc == regularSeason.Length-1) {
						// current month is start or end of regular season. can only click one way on arrows.
						if(loc == 0) 
							//div[@class='qs-arrow qs-right']
							log.Info(loc);
						else 
							//div[@class='qs-arrow qs-left']
							log.Info(loc);
					}
					else {
						// current month is inside limits of regular season. can click both arrows.
						log.Info(loc);
					}
				}
				else {
					// month is not in regular season. 
					// navigate to most recent month of season. assume end of last season.
					//div[@class='qs-arrow qs-left']
					// check if current month is in regular season
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}