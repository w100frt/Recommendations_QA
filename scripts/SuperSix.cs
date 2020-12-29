using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace SeleniumProject.Function
{
    public class Script : ScriptingInterface.IScript
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(DriverManager driver, TestStep step)
        {

            long order = step.Order;
            string wait = step.Wait != null ? step.Wait : "";
            int super6Count = 0;
            int idInList = 0;
            List<TestStep> steps = new List<TestStep>();
            VerifyError err = new VerifyError();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
            var path = Path.Combine(Directory.GetCurrentDirectory());
            path = Path.Combine(path, "SeleniumProject/Postman_Collection/report.json");
            //var path = Path.Combine(@"C:\Users\truon\source\repos\New_Selenium.hao\SeleniumProject\Postman_Collection\report.json");
            log.Info("Current Directory: " + path);
            var list = JObject.Parse(File.ReadAllText(path));
            DateTime dt = new DateTime();
            foreach (JToken x in list["list"])
            {
                if (x["league"].ToString() == DataManager.CaptureMap["SPORT"])
                { 
                    if (DataManager.CaptureMap["SPORT"] == "CFB")
                    {
                        steps.Add(new TestStep(order, "Open Conference Dropdown", "", "click", "xpath", "//a[@class='dropdown-menu-title']", wait));
                        steps.Add(new TestStep(order, "Verify Dropdown is Displayed", "", "verify_displayed", "xpath", "//div[contains(@class,'scores-home-container')]//div[contains(@class,'dropdown-root active')]//ul", wait));
                        steps.Add(new TestStep(order, "Click on FBS (I - A)", "", "click", "xpath", "//div[@class='sub-container dropdown-items-container']//a[contains(text(), 'FBS (I-A)')]", wait));
                        TestRunner.RunTestSteps(driver, null, steps);
                        steps.Clear();
                    }
                    foreach (string id in x["ids"])
                    {
                        if (step.Name.Equals("Verify Super6 Scorechips")) {
                            if ((int)dt.DayOfWeek > 0 && (int)dt.DayOfWeek < 4)
                            {
                                Thread.Sleep(2000);
                                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                            }
                            idInList++;
                            ReadOnlyCollection<IWebElement> elements;
                            log.Info(DataManager.CaptureMap["SPORT"] + " game id:" + id);
                            log.Info("Checking scorechip");
                            elements = driver.FindElements("xpath", "//a[contains(@href,'id=" + id + "')]//div[@class='super-six-logo']");
                            if (elements.Count > 0)
                            {
                                js.ExecuteScript("arguments[0].scrollIntoView(false)", driver.FindElement("xpath", "//a[contains(@href,'id=" + id + "')]"));
                                log.Info("Found scorechip with super6 logo");
                                super6Count++;
                            }
                            else
                            {
                                elements = driver.FindElements("xpath", "//a[contains(@href,'id=" + id + "')]");
                                if (elements.Count > 0)
                                {
                                    js.ExecuteScript("arguments[0].scrollIntoView(false)", driver.FindElement("xpath", "//a[contains(@href,'id=" + id + "')]"));
                                    log.Error("Error: Cannot find super6 logo on scorechip(id=" + id.ToString() + ")");
                                    err.CreateVerificationError(step, "Super6 logo expected for ID: " + id.ToString(), "Super6 logo not found");
                                    driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
                                }
                                else
                                {
                                    log.Error("Error: Cannot find scorechip(id=" + id.ToString() + ")");
                                    err.CreateVerificationError(step, "Expected scorechip for ID: " + id.ToString(), "No scorechip for ID: " + id.ToString() + " found");
                                    driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
                                }
                            }
                        }
                        else if (step.Name.Equals("Verify Super6 on Event Page"))
                        {
                            ReadOnlyCollection<IWebElement> elements;                     
                            if (DataManager.CaptureMap["SPORT"] == "NFL")
                            {
                                steps.Add(new TestStep(order, "Click Scores", "", "click", "xpath", "//a[@href='/scores']", wait));
                                steps.Add(new TestStep(order, "Click NFL", "", "click", "xpath", "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'NFL')]", wait));
                                
                            }
                            else if (DataManager.CaptureMap["SPORT"] == "CFB")
                            {
                                steps.Add(new TestStep(order, "Click Scores", "", "click", "xpath", "//a[@href='/scores']", wait));
                                steps.Add(new TestStep(order, "Click NCAA FB", "", "click", "xpath", "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'NCAA FB')]", wait));
                                steps.Add(new TestStep(order, "Open Conference Dropdown", "", "click", "xpath", "//a[@class='dropdown-menu-title']", wait));
                                steps.Add(new TestStep(order, "Verify Dropdown is Displayed", "", "verify_displayed", "xpath", "//div[contains(@class,'scores-home-container')]//div[contains(@class,'dropdown-root active')]//ul", wait));
                                steps.Add(new TestStep(order, "Click on FBS (I - A)", "", "click", "xpath", "//div[@class='sub-container dropdown-items-container']//a[contains(text(), 'FBS (I-A)')]", wait));
                            }
                            else if (DataManager.CaptureMap["SPORT"] == "NBA")
                            {
                                steps.Add(new TestStep(order, "Click Scores", "", "click", "xpath", "//a[@href='/scores']", wait));
                                steps.Add(new TestStep(order, "Click NFL", "", "click", "xpath", "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'NBA')]", wait));
                            }
                            TestRunner.RunTestSteps(driver, null, steps);
                            steps.Clear();
                            if ((int)dt.DayOfWeek > 0 && (int)dt.DayOfWeek < 4)
                            {
                                Thread.Sleep(2000);
                                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                            }
                            //Getting abbreviation
                            log.Info("Getting team abbreviation");
                            js.ExecuteScript("arguments[0].scrollIntoView(false)", driver.FindElement("xpath", "//a[contains(@href,'id=" + id + "')]"));
                            steps.Add(new TestStep(order, "Capture Team from " + step.Data, "FIRST_ABB", "capture", "xpath", "(//a[contains(@href,'id=" + id + "')]//div[contains(@class,'abbreviation')]//span)[1]", wait));
                            steps.Add(new TestStep(order, "Capture Team from " + step.Data, "SECOND_ABB", "capture", "xpath", "(//a[contains(@href,'id=" + id + "')]//div[contains(@class,'abbreviation')]//span)[2]", wait));
                            steps.Add(new TestStep(order, "Click Scorechip with ID: "+ id.ToString(), "", "click", "xpath", "//a[contains(@href,'id=" + id + "')]", wait));
                            TestRunner.RunTestSteps(driver, null, steps);
                            steps.Clear();
                            log.Info("Check for Super6 content");
                            js.ExecuteScript("window.scrollBy({top: 100,left: 0,behavior: 'smooth'});");
                            elements = driver.FindElements("xpath", "//div[contains(@class,'feed-component super-six-component')]");
                            if (elements.Count > 0)
                            {
                                log.Info("Found Super6 Component");
                                js.ExecuteScript("arguments[0].scrollIntoView(false)", driver.FindElement("xpath", "//div[contains(@class,'feed-component super-six-component')]"));
                                log.Info("Checking for Super6 logo");
                                elements = driver.FindElements("xpath", "//div[contains(@class,'feed-component super-six-component')]/div[contains(@class,'super-six-logo')]");
                                //Super6 Logo
                                if (elements.Count > 0)
                                {
                                    log.Info("Found Super6 logo");
                                }
                                else
                                {
                                    log.Error("Cannot find Super6 Logo");
                                    err.CreateVerificationError(step, "Expected Super6 logo on event page for ID: " + id.ToString(), "No Super6 logo is found");
                                    driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
                                }
                                log.Info("Checking for Super6 Header");
                                //Super6 Header
                                elements = driver.FindElements("xpath", "//div[contains(@class,'feed-component super-six-component')]/div[contains(@class,'super-six-header')]");
                                if ((elements.Count > 0) && ((elements.ElementAt(0).GetAttribute("innerText")).Equals("THIS IS A SUPER 6 GAME")))
                                {
                                    log.Info("Found Super6 header");
                                }
                                else
                                {
                                    log.Error("Cannot find Super6 Header");
                                    err.CreateVerificationError(step, "Expected Super6 header on event page for ID: " + id.ToString(), "No Super6 header is found");
                                    driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
                                } 
                                log.Info("Checking for Super6 Content");
                                //Super6 Content
                                elements = driver.FindElements("xpath", "//div[contains(@class,'feed-component super-six-component')]/div[contains(@class,'super-six-content')]");
                                if (elements.Count > 0)
                                {
                                    log.Info("Found Super6 Content");
                                }
                                else
                                {
                                    log.Error("Cannot find Super6 Content");
                                    err.CreateVerificationError(step, "Expected Super6 Content on event page for ID: " + id.ToString(), "No Super6 Content is found");
                                    driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
                                }
                                log.Info("Checking for Super6 Chart");
                                //Super6 Chart
                                elements = driver.FindElements("xpath", "//div[contains(@class,'feed-component super-six-component')]/div[contains(@class,'vote-chart-container')]");
                                if (elements.Count > 0)
                                {
                                    log.Info("Found Super6 Chart");
                                    elements = driver.FindElements("xpath", "//div[contains(@class,'feed-component super-six-component')]/div[contains(@class,'vote-chart-container')]//div[contains(@class,'chart-label uc')]");
                                    log.Info("Checking for team abbreviations");
                                    if (DataManager.CaptureMap["FIRST_ABB"] == elements.ElementAt(0).GetAttribute("innerText") && DataManager.CaptureMap["SECOND_ABB"] == elements.ElementAt(1).GetAttribute("innerText"))
                                    {
                                        log.Info("Team Abbreviations Matched");
                                    }
                                    else
                                    {
                                        log.Error("Team Abbrevations Not Matched");
                                        err.CreateVerificationError(step, "Expected Matching Team Abbreviations", "Team Abbrevations are not Matched");
                                        err.CreateVerificationError(step, "Expected first abbrevation " + DataManager.CaptureMap["FIRST_ABB"], "Actual first abbrevation "+ elements.ElementAt(0).GetAttribute("innerText"));
                                        err.CreateVerificationError(step, "Expected second abbrevation " + DataManager.CaptureMap["SECOND_ABB"], "Actual second abbrevation " + elements.ElementAt(1).GetAttribute("innerText"));
                                        driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
                                    }
                                }
                                else
                                {
                                    log.Error("Cannot find Super6 Chart");
                                    err.CreateVerificationError(step, "Expected Super6 Chart on event page for ID: " + id.ToString(), "No Super6 Chart is found");
                                    driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
                                }

                            }
                            else
                            {
                                log.Error("Cannot find Super6 Component");
                                err.CreateVerificationError(step, "Expected Super6 Component on event page for ID: " + id.ToString(), "No Super6 Component is found");
                                driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
                            }

                        }
                    }
                }
            }
            if (step.Name.Equals("Verify Super6 Scorechips"))
            {
                if (idInList == super6Count)
                {
                    if (super6Count == 0)
                    {
                        log.Info("No super6 id coming from SD");
                    }
                    else
                    {
                        log.Info("Found all super6 events for this week");
                    }

                }
                else
                {
                    log.Error("Missing super6 logo on " + (idInList - super6Count).ToString() + " event(s)");
                }
            }
        }
    }
}