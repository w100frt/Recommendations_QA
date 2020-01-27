using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;

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
			string sport = step.Data;
			string activeTeam;
            List<TestStep> steps = new List<TestStep>();
            steps.Add(new TestStep(order, "Click Sport Menu Open", "", "click", "xpath", "//a[span[contains(.,'"+ sport +"')]]", wait));
            TestRunner.RunTestSteps(driver, null, steps);
			steps.Clear();
			
			var teams = driver.FindElements("xpath", "//div[contains(@class,'explore-basic-rows')]//a//span"); 
			for (int i = 0; i < teams.Count; i++) {
				int counter = i + 1;
				activeTeam = driver.FindElement("xpath", "(//div[contains(@class,'explore-basic-rows')]//a//span)["+ counter +"]").Text;
				DataManager.CaptureMap.Add("ACTIVE_TEAM", activeTeam);
				
				if (activeTeam.Equals("NFL")) {
					activeTeam = "NATIONAL FOOTBALL LEAGUE";
				}

				steps.Add(new TestStep(order, "Click Team Name", "", "click", "xpath", "(//div[contains(@class,'explore-basic-rows')]//a//span)["+ counter +"]", wait));
				steps.Add(new TestStep(order, "Verify Team Name in Header", activeTeam, "verify_value", "xpath", "//div[contains(@class,'entity-header')]//div[contains(@class,'entity-title')]//span", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				
				if (i < teams.Count - 1) {
					steps.Add(new TestStep(order, "Click Explore", "", "click", "xpath", "//a[contains(@class,'explore-link')]", wait));
					steps.Add(new TestStep(order, "Click Sport Menu Open", "", "click", "xpath", "//a[span[contains(.,'"+ sport +"')]]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
			}
		}
	}
}