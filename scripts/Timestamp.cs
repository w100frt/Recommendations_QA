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
			string utcDate = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Mock Training Data Timestamp Input") || step.Name.Equals("Mock Prediction Data Timestamp Input")) {
				if (step.Name.Equals("Mock Training Data Timestamp Input")) {
					xpath = "//input[@id='TrainingDataTimestamp']";
				}
				else if (step.Name.Equals("Mock Prediction Data Timestamp Input")) {
					xpath = "//input[@id='PredictionDataTimestamp']";
				}
				
				ele = driver.FindElement("xpath", xpath);
				data = ele.GetAttribute("value");
				DateTime dDate;

				if (DateTime.TryParse(data, out dDate))
				{
					String.Format("MM/d/yyyy H:mm:sszzz", dDate);
					string eDate = dDate.ToString();
					if(data == eDate) {
						log.Info("Verification Passed." + data + "is in the correct format");
					} 
					else {
						log.Error("***Verification Failed." + data + "does not equal" + eDate);
						err.CreateVerificationError(step, eDate, data);
						driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);

					}
				}
			}
			
			else if (step.Name.Equals("Verify Training Set To Now") || step.Name.Equals("Verify Prediction Set To Now")) {
				if (step.Name.Equals("Verify Training Set To Now")) {
					xpath = "//input[@id='TrainingDataTimestamp']";
				}
				else if (step.Name.Equals("Verify Prediction Set To Now")) {
					xpath = "//input[@id='PredictionDataTimestamp']";
				}
				
				ele = driver.FindElement("xpath", xpath);
				data = ele.GetAttribute("value");
				DateTime tDate;
				String.Format("mm/dd/yyyy", tDate);
				string toDate = tDate.ToString();
				string dataSplit = data.Substring(0,8);
				string toDateSplit = toDate.Substring(0,8);        
				if(dataSplit == toDateSplit) {
					log.Info("Verification Passed." + dataSplit + "matches" + toDateSplit);
				} 
				else {
					log.Error("***Verification Failed." + dataSplit + "does not equal" + toDateSplit);
					err.CreateVerificationError(step, toDateSplit, dataSplit);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);

				}

				// if (DateTime.TryParse(data, out tDate))
				// {
					
				// }
			}

			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}