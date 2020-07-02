using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;
using System.Collections.ObjectModel;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		public void Execute(DriverManager driver, TestStep step)
		{
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
			int eleCount = 0;
			int total;
			int size = 0;
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();

			if (step.Name.Equals("Click Arrow Forward to End of Carousel")) {
				eleCount = driver.FindElements("xpath", "//div[not(contains(@class,'scorestrip')) and contains(@class,'carousel-wrapper') and contains(@class,'can-scroll-right')]").Count; 
				while (eleCount > 0) {
					steps.Add(new TestStep(order, "Click Arrow Forward", "", "click", "xpath", "//button[@class='carousel-button-next image-button']", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();	
					eleCount = driver.FindElements("xpath", "//div[contains(@class,'carousel') and contains(@class,'can-scroll-right')]").Count;			
				}
			}
			
			else if (step.Name.Equals("Verify Number of Story Cards")) {
				try {
					total = Int32.Parse(step.Data);
				}
				catch (Exception e) {
					total = 50;
					log.Error("Expected data to be a numeral. Setting data to 50.");
				}
			
				size = driver.FindElements("xpath", "//div[contains(@class,'cards-slide')]//a[contains(@class,'card-story')]").Count;
				
				if (size >= total && size <= 100) {
					log.Info("Verification PASSED. Total Stories [" + size + "] is between " + total + " and 100.");
				}
				else {
					log.Error("***Verification FAILED. " + size + " is not between " + total +" and 100***");
					err.CreateVerificationError(step, ">= " + total + " & <= 100", size.ToString());
				}
			}
			
			else {
				log.Warn("Test Step not found in script...");
			}
			
		}
	}
}