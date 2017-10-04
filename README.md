# Validation Monkey

A simple framework to automate testing various field validations normally carried out manually during exploratory testing.

This is just a proof of concept and very rough and ready!! Lot's of things to work out, refactor etc..

The framework is used along with Selenium to randomly fill in form fields and interact with the web page you give it. The application will log it's actions and screenshots as it goes along to a new .HTML file saved in C:\temp (this will be refactored soon!). 

Validation Monkey will randomly decided whether to fill in all fields, no fields or random fields as is navigates through the given url. Currently there are only a few example string, special characters, numbers, incorrect dates, basic script injection - this will be refactored to include a larger list similar to the options currently offered by Bug Magnet (Chrome Extension). This framework also randomly decides whether to click a button, link or press enter in an input field, randomly navigating through the website. Each time the Validation Monkey is run it will always act in a different way.

## How to use

As part of your Selenium test just Start the Monkey! Currently you can specify how many times to run or by providing a 0 value the framework will pick a random number of runs for you. 

``` c#
 // Let the Monkey loose and run 5 times!
 ValidatorMonkey.StartMonkey(5, Driver);
```

Below is a very simple example of setting up a new Chrome WebDriver, Navigating to a page, Starting the Monkey and then finally closing down the WebDriver.

``` c#
 // Setup test to go to page to test
IWebDriver Driver = new ChromeDriver();
Driver.Navigate().GoToUrl("http://computer-database.herokuapp.com/");

// Let the Monkey loose picking a random number of runs!
ValidatorMonkey.StartMonkey(0, Driver);

// Close down driver
Driver.Quit();

```
