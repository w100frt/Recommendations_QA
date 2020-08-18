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
			int total = 0;
            List<TestStep> steps = new List<TestStep>();
			
			if (step.Name.Equals("Verify Teams by Sport")) {
			    steps.Add(new TestStep(order, "Click Sport Menu Open", "", "click", "xpath", "//a[contains(@class,'entity-list')][div[div[contains(.,'"+ sport +"')]]]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				
				var teams = driver.FindElements("xpath", "//div[contains(@class,'explore-basic-rows')]//a"); 
				for (int i = 0; i < teams.Count; i++) {
					int counter = i + 1;
					activeTeam = driver.FindElement("xpath", "(//div[contains(@class,'explore-basic-rows')]//a)["+ counter +"]").GetAttribute("innerText");
					
					if (activeTeam.Equals("NFL")) {
						activeTeam = "NATIONAL FOOTBALL LEAGUE";
					}

					steps.Add(new TestStep(order, "Click Team Name", "", "click", "xpath", "(//div[contains(@class,'explore-basic-rows')]//a)["+ counter +"]", wait));
					steps.Add(new TestStep(order, "Verify Team Name in Header", activeTeam.ToUpper(), "verify_value", "xpath", "//div[contains(@class,'entity-header')]//div[contains(@class,'entity-title')]//span", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					
					if (i < teams.Count - 1) {
						steps.Add(new TestStep(order, "Click Explore", "", "click", "xpath", "//a[contains(@class,'explore-link')]", wait));
						steps.Add(new TestStep(order, "Click Sport Menu Open", "", "click", "xpath", "//a[contains(@class,'entity-list')][div[div[contains(.,'"+ sport +"')]]]", wait));
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();
					}
				}
			}
			
			else if (step.Name.Equals("Verify MLB Teams")) {
			    total = driver.FindElements(
				"xpath","//div[@id='exploreApp']//div[contains(@class,'explore-basic-rows')]//a[not(contains(@class,'header'))]").Count;
				for(int t = 1; t <= total; t++) {
					DataManager.CaptureMap["MLB_TEAM"] = driver.FindElement("xpath","//div[@id='exploreApp']//div[contains(@class,'explore-basic-rows')]//a[not(contains(@class,'header'))]["+ t +"]").Text;
					steps.Add(new TestStep(order, "Template for Team " + t, "", "run_template", "xpath", "MLB_Team", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();					
				}
			}
			
			else if (step.Name.Equals("Click Entity by Data")) {
				steps.Add(new TestStep(order, "Click " + step.Data, "", "click", "xpath", "//a[contains(@class,'entity-list-row-container')][div[contains(.,'"+ step.Data +"')]]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}