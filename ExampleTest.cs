using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

[TestClass]
public class ExampleTest
{
    [TestMethod]
    public void MonkeyAroundComputerDatabaseTest()
    {
        // Setup test to go to page to test
        IWebDriver Driver = new ChromeDriver();
        Driver.Navigate().GoToUrl("http://computer-database.herokuapp.com/");

        // Let the Monkey loose!
        ValidatorMonkey.StartMonkey(5, Driver);

        // Close down driver
        Driver.Quit();
    }
}