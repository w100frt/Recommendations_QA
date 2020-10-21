using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;
using System.Threading;
using System.Text;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		public void Execute(DriverManager driver, TestStep step)
		{
			VerifyError err = new VerifyError();
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
			List<string> standings = new List<string>();
            List<TestStep> steps = new List<TestStep>();
			Random random = new Random();
			int total = 0;
			int size;
			string name = "";
			IWebElement element;
			StringBuilder sb = new StringBuilder();
			
			if (step.Name.Equals("Click and Capture Random Team By Division")) {
				name = "//div[contains(@class,'table-standings')][table//th[contains(.,'"+ step.Data +"')]]//tbody/tr";
				total = driver.FindElements("xpath", name).Count; 
				total = random.Next(1, total+1);
				steps.Add(new TestStep(order, "Capture Team from " + step.Data, "DIV_TEAM", "capture", "xpath", "(//div[contains(@class,'table-standings')][table//th[contains(.,'"+ step.Data +"')]]//a[contains(@class,'entity-name')])["+ total +"]", wait));
				steps.Add(new TestStep(order, "Click Team from " + step.Data, "", "click", "xpath", "(//div[contains(@class,'table-standings')][table//th[contains(.,'"+ step.Data +"')]]//a[contains(@class,'entity-name')])["+ total +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Capture Player Stat Leader by Number")) {
				steps.Add(new TestStep(order, "Capture Leader " + step.Data, "STAT_LEADER", "capture", "xpath", "(//div[contains(@class,'stat-leader-info')]/div[1])["+ step.Data +"]", wait));
				steps.Add(new TestStep(order, "Capture Leader Team " + step.Data, "STAT_LEADER_TEAM", "capture", "xpath", "(//div[contains(@class,'stat-leader-info')]/div[@class='uc'])["+ step.Data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				name = DataManager.CaptureMap["STAT_LEADER"];
				name = name.Substring(0,1) + "." + name.Substring(name.IndexOf(" "));
				sb.AppendLine(name);
				sb.Append(DataManager.CaptureMap["STAT_LEADER_TEAM"]);
				DataManager.CaptureMap["STAT_LEADER"] = sb.ToString();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}