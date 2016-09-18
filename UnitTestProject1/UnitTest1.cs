using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class NewTestClass
    {
        private string baseURL = "https://dou.ua/";
        private static RemoteWebDriver driver;
        private string browser;
        private String user = "12testusertest12";
        private String pass = "qwer12341234qwer";
        public String jobCategory = ".NET";
        public String sarchText = "SQL Saturday";
        public string salaryTable = "//div[@class='salarydec-field']/";
        public String city = "select[@name='city']/option[3]";
        public String jobBranch = "select[@name='title']/optgroup[@label='Development']/option[2]";
        public String formGroup ="//div[@class='form-group']";
        public String Position = "Automation QA Engineer";
        public String cityJob = "Kiev";
        
       
        
        public TestContext TestContext { get; set; }

        public void OpenUrl( String url) {
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            driver.Navigate().GoToUrl(url);
        }

        public void WaitForPageToLoad(double time) {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(time));
        }


        /* This test needs to be automated, as it checks one of the main functionalities.
         * Besides, this can be done for a list of different parameters (login options) and verify, 
         * that all the ways are working correctly. 
         */
         [TestMethod]
         public void Test1_LoggingIn()
         {
             OpenUrl(this.baseURL);           
             driver.FindElementById("login-link").Click();           
             driver.SwitchTo().ActiveElement();
             driver.FindElementByLinkText("Google").Click();
             SwitchTheWindow();
             driver.FindElementById("Email").SendKeys(user);
             driver.FindElementById("next").Click();
             driver.FindElementById("Passwd").SendKeys(pass);
             driver.FindElementById("signIn").Click();
             MyTestCleanup();                      
         }


        /* This test should be automated, as this is the one of the most popular 
         * category on this site. Also it can be tested with a list of parameters (different categories and positions)         
         */
        [TestMethod]
        public void Test2_SearchJob()
        {
            OpenUrl(this.baseURL);

            //finding a path to the 'Jobs' link
            driver.FindElementByXPath("//ul/li[6]/a").Click();
            WaitForPageToLoad(5);            
            driver.FindElementByPartialLinkText(jobCategory).Click();                   
            MyTestCleanup();
        }


        /*
         */
        [TestMethod]
        public void CalendarCheck() {
            OpenUrl(this.baseURL);           
            driver.FindElementByCssSelector("a[href ^= 'https://dou.ua/calendar/']").Click();
            WaitForPageToLoad(5);
            //searching for events in Kiev
            driver.FindElementByXPath("//div['page-head']/h1/select[1]/option[5]").Click();
            WaitForPageToLoad(10);
            //searching for '.NET' events
            driver.FindElementByXPath("//div['page-head']/h1/select[2]/option[2]").Click();
            WaitForPageToLoad(10);
            if (driver.FindElementsByXPath("//div[@class='event']") != null)
            {
                //choosing the first event from the list
                driver.FindElementByXPath("//div[@class='event'][1]/h2[@class='title']/a").Click();
            }           
            MyTestCleanup();
        }


        /* This test can be automated to help to improve perfomance (if needed) because of 
         * a big amount of queries per day
         */

        [TestMethod]
        public void SearchCheck() {
            OpenUrl(this.baseURL);            
            driver.FindElementByClassName("inp").SendKeys(sarchText + Keys.Enter);
            WaitForPageToLoad(10);           
            String searchRes = "//div[@class='gsc-results gsc-webResult']";
            //if search returns results, select the first one, to check if the link works
            if (driver.FindElementsByXPath("searchRes" + "/*") == null) return;
            else
            {
                driver.FindElementByXPath(searchRes + "/div[@class='gsc-webResult gsc-result'][1]//table//a").Click();
                WaitForPageToLoad(10);
            }
            MyTestCleanup();

        }


        /* This test can also be taken for checking performance. Also, as there are some list elements.
         * and they can be extended frequently (new languages, categories (f.e.), it can impact on other functionality)
         * The other reason, it was interesting for me to manipulate mouse actions (sorting expirience)
         */

        [TestMethod]
        public void ChekSalaries() {
            OpenUrl(this.baseURL);
            driver.FindElementByCssSelector("a[href='https://jobs.dou.ua/salaries/']").Click();            
            driver.FindElementByXPath(salaryTable + city).Click();
            driver.FindElementByXPath(salaryTable + jobBranch).Click();            
            Actions builder = new Actions(driver);
            String slider = "//div[@class='salarydec-field']//div[@class='salarydec-slider-wrap']//";
            IWebElement elem = driver.FindElementByXPath(slider + "a[1]");
            IWebElement elem2 = driver.FindElementByXPath(slider + "a[2]");           
            builder.DragAndDrop(elem, elem2).Build().Perform();           
            WaitForPageToLoad(10);
            MyTestCleanup();
        }


        /*This test should be automated as this is one of the main functionality.
         * It can be done with a lot of options (I've provided only 1 set of 2 parameters just for sample)
         */
        [TestMethod]
        public void Jinn() {
            OpenUrl(this.baseURL);
            driver.FindElementByXPath("//li[@class='m-hide t-hide']/a").Click();
            SwitchTheWindow();
            WaitForPageToLoad(10);
            driver.FindElementByXPath("//div[@class='hero-unit-submit-wrapper']/a").Click();
            WaitForPageToLoad(10);
            driver.FindElementByXPath(formGroup + "/label[@for='position']/..//input").Clear();
            driver.FindElementByXPath(formGroup + "/label[@for='position']/..//input").SendKeys(Position);
            driver.FindElementByXPath(formGroup + "/label[@for='location']/..//input").Clear();
            driver.FindElementByXPath(formGroup + "/label[@for='location']/..//input").SendKeys(cityJob);
            driver.FindElementByXPath("//button[@type='submit']").Click();
            //this gets you to your account
            WaitForPageToLoad(15);
            MyTestCleanup();
        }

        public void SwitchTheWindow() {
            if (driver.WindowHandles.Count > 1) {
                driver.SwitchTo().Window(driver.WindowHandles[driver.WindowHandles.Count - 1]);               
            }           
        }
 
        public void MyTestCleanup()
        {           
            driver.Quit();        
            
        }
        
        [TestInitialize()]
        public void MyTestInitialize()
        {  
            browser = this.TestContext.Properties["browser"] != null ? this.TestContext.Properties["browser"].ToString() : "firefox";
            switch (browser)
            {
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                case "chrome":
                    driver = new ChromeDriver();
                    break;               
            }           
        }
    }
}
