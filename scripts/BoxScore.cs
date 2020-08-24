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
			List<string> stoppage = new List<string>();
			IWebElement ele;
			int size;
			int start;
			int end;
			int game = 0;
			string data = "";
			string status = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify Box Score Tabs By Sport")) {
				ele = driver.FindElement("xpath","//img[@class='location-image']");
				data = ele.GetAttribute("src");
				start = data.LastIndexOf('/')+1;
				end = data.IndexOf(".vresize") - start;
				data = data.Substring(start, end);
				if (ele.GetAttribute("src").Contains("soccer")) {
					data = "Soccer";
				}
				size = 1;
				switch(data) {
					case "Soccer" :
						stoppage.Add("MATCHUP");
						stoppage.Add(DataManager.CaptureMap["AWAY_TEAM"]);
						stoppage.Add(DataManager.CaptureMap["HOME_TEAM"]);
						break;
					case "NBA":
					case "NHL":
						stoppage.Add(DataManager.CaptureMap["AWAY_TEAM"]);
						stoppage.Add(DataManager.CaptureMap["HOME_TEAM"]);
						stoppage.Add("MATCHUP");
						break;
					default :
						stoppage.Add(DataManager.CaptureMap["AWAY_TEAM"]);
						stoppage.Add(DataManager.CaptureMap["HOME_TEAM"]);
						break;
				}
				
				foreach (string stop in stoppage) {
					steps.Add(new TestStep(order, "Verify Box Score Links for " + data, stop, "verify_value", "xpath", "(//div[contains(@class,'boxscore')]//a)["+size+"]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					size++;
				}

			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}