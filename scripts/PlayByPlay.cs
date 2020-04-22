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
			List<string> stoppage = new List<string>();
			IWebElement ele;
			int size;
			int start;
			int end;
			string data = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify PBP Headers By Sport")) {
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
					case "NHL" :
						stoppage.Add("1ST PERIOD");
						stoppage.Add("2ND PERIOD");
						stoppage.Add("3RD PERIOD");
						break;
					case "Soccer" :
					case "NCAABasketball" :
						stoppage.Add("1ST HALF");
						stoppage.Add("2ND HALF");
						break;
					case "MLB":
						stoppage.Add("TOP 1ST");
						stoppage.Add("BOTTOM 1ST");
						stoppage.Add("TOP 2ND");
						stoppage.Add("BOTTOM 2ND");
						stoppage.Add("TOP 3RD");
						stoppage.Add("BOTTOM 3RD");
						stoppage.Add("TOP 4TH");
						stoppage.Add("BOTTOM 4TH");
						stoppage.Add("TOP 5TH");
						stoppage.Add("BOTTOM 5TH");
						stoppage.Add("TOP 6TH");
						stoppage.Add("BOTTOM 6TH");
						stoppage.Add("TOP 7TH");
						stoppage.Add("BOTTOM 7TH");
						stoppage.Add("TOP 8TH");
						stoppage.Add("BOTTOM 8TH");
						stoppage.Add("TOP 9TH");
						stoppage.Add("BOTTOM 9TH");
						break;
					default :
						stoppage.Add("1ST QUARTER");
						stoppage.Add("2ND QUARTER");
						stoppage.Add("3RD QUARTER");
						stoppage.Add("4TH QUARTER");
						break;
				}
				
				foreach (string stop in stoppage) {
					steps.Add(new TestStep(order, "Verify PBP Header for " +step.Data, stop, "verify_value", "xpath", "(//div[contains(@class,'pbp-header')])["+size+"]", wait));
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