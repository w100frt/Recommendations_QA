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
			
			if (step.Name.Equals("Verify Odds Row Subtitles by Sport")) {
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
						stoppage.Add("TOTAL");
						stoppage.Add("TEAM TO WIN");
						stoppage.Add("BOTH TEAMS TO SCORE");
						break;
					case "NBA":
					case "NFL":
					case "NCAABasketball":
					case "NCAAFootball":
						stoppage.Add("SPREAD");
						stoppage.Add("TOTAL");
						stoppage.Add("TEAM TO WIN");
						break;
					case "NHL":
						stoppage.Add("PUCK LINE");
						stoppage.Add("TOTAL");
						stoppage.Add("TEAM TO WIN");
						break;
					case "MLB":
						stoppage.Add("RUN LINE");
						stoppage.Add("TOTAL");
						stoppage.Add("TEAM TO WIN");
						break;
					default :
						stoppage.Add("");
						stoppage.Add("");
						stoppage.Add("");
						break;
				}
				
				foreach (string stop in stoppage) {
					steps.Add(new TestStep(order, "Verify Odds Subtitles for " + data, stop, "verify_value", "xpath", "//div[contains(@class,'odds-rows')]//div[contains(@class,'odds-row')]["+size+"]//div[contains(@class,'subtitle')]", wait));
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