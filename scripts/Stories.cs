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
			List<TestStep> steps = new List<TestStep>();

			if (step.Name.Equals("Click Arrow Forward to End of Carousel")) {
				eleCount = driver.FindElements("xpath", "//div[contains(@class,'carousel-container card-carousel') and contains(@class,'can-scroll-right')]").Count; 
				while (eleCount > 0) {
					steps.Add(new TestStep(order, "Click Arrow Forward", "", "click", "xpath", "//button[@class='carousel-button-next image-button']", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();	
					eleCount = driver.FindElements("xpath", "//div[contains(@class,'carousel-container card-carousel') and contains(@class,'can-scroll-right')]").Count;			
				}
			}
			
			else {
				log.Warn("Test Step not found in script...");
			}
			
		}
	}
}