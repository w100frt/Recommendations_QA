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
			
			else if (step.Name.Equals("Capture Team Stat Leader by Number")) {
				steps.Add(new TestStep(order, "Capture Leader " + step.Data, "STAT_LEADER", "capture", "xpath", "(//div[contains(@class,'stat-leader-info')]/div[1])["+ step.Data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Capture Stat Value by Number")) {
				steps.Add(new TestStep(order, "Capture Value " + step.Data, "STAT_VALUE", "capture", "xpath", "(//div[contains(@class,'stat-data')]/div[contains(@class,'fs')])["+ step.Data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Capture Stat Abbreviation by Number")) {
				steps.Add(new TestStep(order, "Capture Value " + step.Data, "STAT_ABBR", "capture", "xpath", "(//div[@class='stat-abbr'])["+ step.Data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
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
			
			else if (step.Name.Contains("Player Stats Template")) {
				DataManager.CaptureMap["STATS_NUM"] = step.Data;
				if (step.Name.Contains("CFB") || step.Name.Contains("CBK")) {
					steps.Add(new TestStep(order, "CFB Stats " + step.Data, "", "run_template", "xpath", "CFB_PlayerStats", wait));
				}
				if (step.Name.Contains("MLB")) {
					steps.Add(new TestStep(order, "MLB Stats " + step.Data, "", "run_template", "xpath", "MLB_PlayerStats", wait));
				}
				else if (step.Name.Contains("NFL")) {
					steps.Add(new TestStep(order, "NFL Stats " + step.Data, "", "run_template", "xpath", "NFL_PlayerStats", wait));
				}
				else if (step.Name.Contains("NBA")) {
					steps.Add(new TestStep(order, "NBA Stats " + step.Data, "", "run_template", "xpath", "NBA_PlayerStats", wait));
				}
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Contains("Team Stats Template")) {
				DataManager.CaptureMap["STATS_NUM"] = step.Data;
				if (step.Name.Contains("CFB") || step.Name.Contains("CBK")) {
					steps.Add(new TestStep(order, "CFB Stats " + step.Data, "", "run_template", "xpath", "CFB_TeamStats", wait));
				}				
				if (step.Name.Contains("MLB")) {
					steps.Add(new TestStep(order, "MLB Stats " + step.Data, "", "run_template", "xpath", "MLB_TeamStats", wait));
				}
				else if (step.Name.Contains("NBA")) {
					steps.Add(new TestStep(order, "NBA Stats " + step.Data, "", "run_template", "xpath", "NBA_TeamStats", wait));
				}
				else if (step.Name.Contains("NFL")) {
					steps.Add(new TestStep(order, "NFL Stats " + step.Data, "", "run_template", "xpath", "NFL_TeamStats", wait));
				}
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify Bold Category")) {
				name = driver.FindElement("xpath","//th[contains(@class,'cell-number') and contains(@class,'bold')]").GetAttribute("innerText");
				name = name.Trim();
				
				if (name.Equals(step.Data)) {
					log.Info("Verification PASSED. Expected data [" + step.Data + "] matches actual data [" + name + "]");
				}
				else {
					log.Error("***Verification FAILED. Expected data [" + step.Data + "] does not match actual data [" + name + "] ***");
					err.CreateVerificationError(step, step.Data, name);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}