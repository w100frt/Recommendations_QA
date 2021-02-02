using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;
using log4net;
using System.Threading;
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
			List<TestStep> steps = new List<TestStep>();
			IWebElement ele;
			ReadOnlyCollection<IWebElement> elements;
			string data = "";
			string xpath = "";
			string url = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Mock Training Data Timestamp Input")) {
				var DateTime? Parse(string s)
				{
					// you may want to add a few more formats here
					var formats = new[] { "MM-DD-YYYY hh:mm:ss+ss:ss"};
					DateTime dt;
					if (DateTime.TryParseExact(s, formats,
											CultureInfo.InvariantCulture, // ISO is invariant
											DateTimeStyles.RoundtripKind, // this is important
											out dt))
						return dt;

					return null;
				}
				string[] timeStamp = {dt};
				elements = driver.FindElements("xpath", "/html/body/div/main/form/div/div[1]/div[2]/input");
				
				if(timeStamp.Length != elements.Count) {
					log.Error("Unexpected element count. Expected: [" + timeStamp.Length + "] does not match Actual: [" + elements.Count + "]");
					err.CreateVerificationError(step, timeStamp.Length.ToString(), elements.Count.ToString());
				}
				else {
					for (int i=0; i < elements.Count; i++) {
						if(dataSet[i].Equals(elements[i].GetAttribute("innerText").Trim())) {
							log.Info("Verification Passed. Expected [" + timeStamp[i] + "] matches Actual [" + elements[i].GetAttribute("innerText").Trim() +"]");
						}
						else {
							log.Error("Verification FAILED. Expected: [" + timeStamp[i] + "] does not match Actual: [" + elements[i].GetAttribute("innerText").Trim() + "]");
							err.CreateVerificationError(step, timeStamp[i], elements[i].GetAttribute("innerText").Trim());
						}
					}
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}