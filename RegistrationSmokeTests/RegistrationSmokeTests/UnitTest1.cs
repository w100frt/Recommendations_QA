using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    [TestFixture(typeof(ChromeDriver))]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]



    public class TestWithMultipleBrowsers<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;

        [SetUp]
        public void CreateDriver()
        {
            this.driver = new TWebDriver();
        }

        [Test]
        public void ExistingUserSignIn()
        {
            string credentials = $"foxsports:foxsportsrocks";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            string url = $"https://{credentials}@" + "www.stg.wp.foxsports.com/accounts/login?fu";
            //string url = $"https://{credentials}@" + "www.foxsports.com/accounts/login?fu";

            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("input.site-email")).Click();
            driver.FindElement(By.CssSelector("input.site-email")).Clear();
            driver.FindElement(By.CssSelector("input.site-email")).SendKeys("forrest.thompson@fox.com");
            driver.FindElement(By.CssSelector("input.site-password")).Clear();
            driver.FindElement(By.CssSelector("input.site-password")).SendKeys("Testing");
            driver.FindElement(By.CssSelector("button.sign-in-button")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(45);
            driver.FindElement(By.Id("wisss_linkTitle"));
            //Thread.Sleep(10000);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(45);
            string credentials2 = $"foxsports:foxsportsrocks";
            string url2 = $"https://{credentials2}@" + "www.stg.wp.foxsports.com/accounts/settings/favorites";
            driver.Navigate().GoToUrl(url2);
            //driver.Navigate().GoToUrl($"https://{credentials2}@" + "www.foxsports.com/accounts/settings/favorites");
            //Thread.Sleep(5000);
            driver.FindElement(By.CssSelector(".wis_logo"));
            driver.Quit();
        }

        [TearDown]

        public void RunForTest1()
        {
            driver.Quit();
        }

        [Test]
        public void NewUserRegistration()
        {
            string credentials = $"foxsports:foxsportsrocks";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            string url = $"https://{credentials}@" + "www.stg.wp.foxsports.com/accounts/login?fu";
            //string url = $"https://{credentials}@" + "www.foxsports.com/accounts/login?fu";

            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("button.sign-up-button")).Click();
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            var emailPrefix = secondsSinceEpoch;
            string email = $"{emailPrefix}" + "@wright.edu";
            driver.FindElement(By.CssSelector("input.site-email")).SendKeys(email);
            driver.FindElement(By.CssSelector("input.site-password")).SendKeys("Testing");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Favorites'])[1]/following::input[3]")).Clear();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Favorites'])[1]/following::input[3]")).SendKeys("Forrest");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Favorites'])[1]/following::input[4]")).Clear();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Favorites'])[1]/following::input[4]")).SendKeys("Thompson");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Favorites'])[1]/following::select[1]")).SendKeys("Male");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Favorites'])[1]/following::input[5]")).SendKeys("09/18/1987");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.FindElement(By.CssSelector("button.sign-up-button")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);
            driver.SwitchTo().Frame(driver.FindElement(By.ClassName("fancybox-iframe")));
            driver.FindElement(By.CssSelector(".wisfav_addButton")).Click();
            driver.FindElement(By.CssSelector(".wisfav_nextAction")).Click();
            driver.FindElement(By.CssSelector(".wisfav_addButton")).Click();
            driver.FindElement(By.CssSelector(".wisfav_nextAction")).Click();
            driver.FindElement(By.CssSelector(".wisfav_addButton")).Click();
            driver.FindElement(By.CssSelector(".wisfav_nextActionAlt")).Click();
            driver.SwitchTo().Frame(driver.FindElement(By.Id("LOCSTORAGE")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);
            string credentials2 = $"foxsports:foxsportsrocks";
            string url2 = $"https://{credentials2}@" + "www.stg.wp.foxsports.com/accounts/settings/favorites";
            //string url2 = $"https://{credentials2}@" + "www.foxsports.com/accounts/settings/favorites";
            driver.Navigate().GoToUrl(url2);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.FindElement(By.CssSelector(".wis_logo"));
            driver.Quit();

        }
        [TearDown]

        public void RunForTest2()
        {
            driver.Quit();
        }
        [Test]
        public void PBCFSRegCheck()
        {
            string credentials1 = $"delta:awesome";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            string url = $"https://{credentials1}@" + "account-qa.foxsports.com/signin?return_to=/signup-account";

            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.Name("email")).SendKeys("dcgliveuser@fox.com");
            driver.FindElement(By.Name("password")).SendKeys("fox123");
            driver.FindElement(By.CssSelector("button.formSubmit")).Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            string credentials2 = $"foxsports:foxsportsrocks";
            string url2 = $"https://{credentials2}@" + "www.stg.wp.foxsports.com/accounts/settings/account-info";

            driver.Navigate().GoToUrl(url2);
            driver.FindElement(By.ClassName("email-input")).Equals("dcgliveuser@fox.com");

            string url3 = $"https://{credentials1}@" + "account-qa.foxsports.com/signup-account";
            driver.Navigate().GoToUrl(url3);
            driver.FindElement(By.CssSelector("button.formSubmit")).Click();

            driver.Quit();
        }
        [TearDown]

        public void RunForTest3()
        {
            driver.Quit();
        }
        [Test]
        public void FSPBCRegCheck()
        {
            string credentials = $"foxsports:foxsportsrocks";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            string url = $"https://{credentials}@" + "www.stg.wp.foxsports.com/accounts/login?fu";

            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("input.site-email")).Click();
            driver.FindElement(By.CssSelector("input.site-email")).Clear();
            driver.FindElement(By.CssSelector("input.site-email")).SendKeys("dcgliveuser@fox.com");
            driver.FindElement(By.CssSelector("input.site-password")).Clear();
            driver.FindElement(By.CssSelector("input.site-password")).SendKeys("fox123");
            driver.FindElement(By.CssSelector("button.sign-in-button")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(45);
            driver.FindElement(By.Id("wisss_linkTitle"));

            string credentials1 = $"delta:awesome";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            string url2 = $"https://{credentials1}@" + "account-qa.foxsports.com/signin?return_to=/signup-account";

            driver.Navigate().GoToUrl(url2);
            driver.FindElement(By.CssSelector("button.formSubmit")).Click();

            driver.Quit();
        }
        [TearDown]

        public void RunForTest4()
        {
            driver.Quit();
        }
    }
}
    
