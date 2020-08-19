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
			int total = 0;
			int size;
			string name = "";
			IWebElement element;
			StringBuilder sb = new StringBuilder();
			
			if (step.Name.Equals("Capture Stat Name by Number")) {
				steps.Add(new TestStep(order, "Capture Name " + step.Data, "STAT_NAME", "capture", "xpath", "(//div[contains(@class,'stat-name')])["+ step.Data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Capture Stat Leader by Number")) {
				steps.Add(new TestStep(order, "Capture Leader " + step.Data, "STAT_LEADER", "capture", "xpath", "(//div[contains(@class,'stat-leader-info')]/div[1])["+ step.Data +"]", wait));
				steps.Add(new TestStep(order, "Capture Leader Team " + step.Data, "STAT_LEADER_TEAM", "capture", "xpath", "(//div[contains(@class,'stat-leader-info')]/div[@class='uc'])["+ step.Data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				name = DataManager.CaptureMap["STAT_LEADER"];
				name = name.Substring(0,1) + ". " + name.Substring(name.IndexOf(" "));
				sb.AppendLine(name);
				sb.AppendLine(DataManager.CaptureMap["STAT_LEADER_TEAM"]);
				DataManager.CaptureMap["STAT_LEADER"] = sb.ToString();
			}
			
			else if (step.Name.Equals("Capture Stat Value by Number")) {
				steps.Add(new TestStep(order, "Capture Value " + step.Data, "STAT_VALUE", "capture", "xpath", "(//div[contains(@class,'stat-data')]/div[1])["+ step.Data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Capture Stat Abbreviation by Number")) {
				steps.Add(new TestStep(order, "Capture Value " + step.Data, "STAT_ABBR", "capture", "xpath", "(//div[@class='stat-abbr'])["+ step.Data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				DataManager.CaptureMap["STAT_ABBR"] = DataManager.CaptureMap["STAT_ABBR"].PadRight(DataManager.CaptureMap["STAT_ABBR"].Length + 1);
			}
			
			else if (step.Name.Equals("Click Stat Category by Number")) {
				steps.Add(new TestStep(order, "Click Category " + step.Data, "", "click", "xpath", "(//a[contains(@class,'stats-overview')])["+ step.Data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Select Stats Category from Dropdown")) {
				steps.Add(new TestStep(order, "Open Stats Dropdown", "", "click", "xpath", "//div[contains(@class,'stats-header')]//a[contains(@class,'dropdown-title')]", wait));
				steps.Add(new TestStep(order, "Click " + step.Data, "", "click", "xpath", "//div[contains(@class,'stats-header')]//a[.='"+ step.Data +"']", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}