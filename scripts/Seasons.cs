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
			int size;
			DateTime start;
			DateTime end;
			DateTime now;
			string data = "";
			bool season = false;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Is Sport In-Season?")) {
				now = DateTime.Today;
				switch(step.Data) {
					case "CBK" : 
						start = new DateTime(2020, 11, 24);
						end = new DateTime(2021, 04, 05);
						break;
					case "CFB" : 
						start = new DateTime(2020, 08, 19);
						end = new DateTime(2021, 01, 11);
						break;
					case "Golf" :
					case "GOLF" :
						start = new DateTime(2021, 01, 07);
						end = new DateTime(2021, 09, 05);
						break;
					case "MLB" : 
						start = new DateTime(2021, 04, 01);
						end = new DateTime(2021, 10, 28);
						break;
					case "NASCAR" :
						start = new DateTime(2021, 02, 09);
						end = new DateTime(2021, 11, 07);
						break;
					case "NBA" : 
						start = new DateTime(2020, 12, 11);
						end = new DateTime(2021, 08, 01);
						break;
					case "NHL" : 
						start = new DateTime(2021, 01, 12);
						end = new DateTime(2021, 05, 30);
						break;
					case "NFL" :
						start = new DateTime(2020, 08, 01);
						end = new DateTime(2021, 02, 02);
						break;
					case "Soccer" :
					case "SOCCER" : 
						start = new DateTime(2020, 11, 05);
						end = new DateTime(2021, 11, 05);
						break;
					default: 
						start = new DateTime(2020, 11, 05);
						end = new DateTime(2021, 11, 05);
						break;
				}
				log.Info("Current date: " + now);
				if (now >= start && now < end) {
					DataManager.CaptureMap.Add("IN_SEASON", "True");
					log.Info("Today is in-season. Storing IN_SEASON to Capture Map as True.");
				}
				else {
					DataManager.CaptureMap.Add("IN_SEASON", "False");
					log.Info("Today is not in-season. Storing IN_SEASON to Capture Map as False.");
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}