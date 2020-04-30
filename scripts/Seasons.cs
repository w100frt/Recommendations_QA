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
						start = new DateTime(2019, 11, 05);
						end = new DateTime(2019, 11, 05);
						break;
					case "CFB" : 
						start = new DateTime(2019, 08, 19);
						end = new DateTime(2020, 01, 13);
						break;
					case "Golf" :
					case "GOLF" :
						start = new DateTime(2019, 09, 12);
						end = new DateTime(2020, 11, 15);
						break;
					case "MLB" : 
						start = new DateTime(2020, 02, 21);
						end = new DateTime(2020, 09, 27);
						break;
					case "NASCAR" :
						start = new DateTime(2020, 02, 09);
						end = new DateTime(2020, 11, 08);
						break;
					case "NBA" : 
						start = new DateTime(2019, 09, 30);
						end = new DateTime(2020, 04, 15);
						break;
					case "NHL" : 
						start = new DateTime(2019, 09, 15);
						end = new DateTime(2020, 04, 04);
						break;
					case "NFL" :
						start = new DateTime(2019, 08, 01);
						end = new DateTime(2020, 02, 02);
						break;
					case "Soccer" :
					case "SOCCER" : 
						start = new DateTime(2019, 11, 05);
						end = new DateTime(2020, 11, 05);
						break;
					default: 
						start = new DateTime(2019, 11, 05);
						end = new DateTime(2020, 11, 05);
						break;
				}
				log.Info("Current date: " + now);
				if (now >= start && now < end) {
					log.Info("IN SEASON");
				}
				else {
					log.Info("OUT OF SEASON");
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}