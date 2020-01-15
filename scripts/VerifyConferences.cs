using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using OpenQA.Selenium.IWebElement;
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
			string[] expectedConf = {"BOWLS", "TOP 25", "AAC", "ACC", "BIG 12", "BIG SKY", "BIG SOUTH", "BIG TEN", "C-USA", "CAA", "IND-FCS", "INDEPENDENTS", "IVY", "MAC", "MEAC", "MVC", "MW", "NEC", "OVC", "PAC-12", "PATRIOT LEAGUE", "PIONEER", "SEC", "SOUTHERN", "SOUTHLAND", "SUN BELT", "SWAC"};
            List<TestStep> steps = new List<TestStep>();
            steps.Add(new TestStep(order, "Open Conference Dropdown", "", "click", "xpath", "//a[@class='dropdown-menu-title']", wait));
			steps.Add(new TestStep(order, "Verify Dropdown is Displayed", "", "verify_displayed", "xpath", "//div[@class='scores-home-container']//div[contains(@class,'dropdown')]//ul", wait));
            TestRunner.RunTestSteps(driver, null, steps);
			steps.Clear();
			
			var conferences = driver.FindElements("xpath", "//div[@class='scores-home-container']//div[contains(@class,'dropdown')]//ul//li"); 
			for (int i = 0; i < conferences.Count; i++) {
				if (expectedConf[i].Equals(conferences[i].GetAttribute("innerText"))) {
					log.Info("Success. " + expectedConf[i] + " matches " + conferences[i].GetAttribute("innerText"));
				}
				else {
					log.Error("***Verification FAILED. Expected data [" + expectedConf[i] + "] does not match actual data [" + conferences[i].GetAttribute("innerText") + "] ***");
					err.CreateVerificationError(step, expectedConf[i], conferences[i].GetAttribute("innerText"));
				}
			}
		}
	}
}