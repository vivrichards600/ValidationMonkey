# Validation Monkey

A simple framework to automate testing various field validations normally carried out manually during exploratory testing.

This is just a proof of concept and very rough and ready!! Lot's of things to work out, refactor etc..

The framework is designed to be used along with Selenium WebDriver to randomly fill in form fields and interact with the web page you give it. The application will log it's actions and screenshots as it goes along to a new .HTML file which will get saved to C:\temp (this will be refactored soon!). 

Validation Monkey will randomly decided whether to fill in all fields, no fields or random fields as is navigates through the web. Currently there are only a few example strings; special characters, numbers, incorrect dates, basic script injection - this will be refactored to include a larger list similar to the options currently offered by Bug Magnet (Chrome Extension). This framework also randomly decides whether to click a button, link or press enter in an input field, and so will often randomly navigate away from the initial given url through various paths of your website (to get it to crawl further give it more runs when Starting the Monkey!

Each time the Validation Monkey is run it will always act in a different way, inputting different values, to a different number of fields, navigating to different areas of your application.

## How to use

As part of your Selenium test just call StartMonkey! Currently you can specify how many times to run or if you provide a 0 value the framework will pick a random number of runs for you. A run is basically each time the page is reloaded it will attempt to 'run' some validation. To get the page to crawl and run more validation on your site, give it a higher number of runs! 

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
