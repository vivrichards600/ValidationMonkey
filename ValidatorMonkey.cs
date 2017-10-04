using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;

public static class ValidatorMonkey
{
    //### INPUT TYPES
    // Read from json file various input values to monkey about with 
    //Bug Magnet:
    //Lorems
    //Text size
    //Cities
    //Exception-mail addresses
    //URLs
    //Numbers
    //Whitespace
    //Format Exploits

    //TODO: inputs nead to be read from json file
    private static List<string> inputs = new List<string>(new string[] { "John O'Grady", "Chloë Rømer", "email@domain.com", "@domain.com", "2147483647", "-1", "", " ", "NULL", "<b><i>HELLO</b>", "<script>alert('Executing JS')</script>", "<img src'https://blog.codinghorror.com/content/images/uploads/2011/04/6a0120a85dcdae970b014e880f778e970d-800wi.png'/>", "31/04/2025", "01/05/2017" });
    private static Random randomNumber = new Random();
    private static string reportFilename = string.Format("{0:yyyy-MM-ddTHH-mm-ss}-VMR.html", DateTime.Now);
    private static IWebDriver driver;

    /// <summary>
    /// Start validator monkey populating fields an clicking buttons & links
    /// </summary>
    /// <param name="numberOfTimes">Number of times to run</param>
    /// <param name="Driver">Selenium WebDriver being used</param>
    public static void StartMonkey(int numberOfTimes, IWebDriver Driver)
    {
        driver = Driver; // set the WebDriver we are using
        CreateReport(); // created html file with valid html template
        WriteToReport(string.Format("<p class='lead'>Monkey Business started {0}, on URL {1}</p>", DateTime.Now.ToString(), driver.Url));

        //if user doesn't specify number of runs then select a number at random up to 20 for now..
        int numberOfRuns = numberOfTimes == 0 ? randomNumber.Next(0, 20) : numberOfTimes;
        WriteToReport(string.Format("<p class='lead'>Validator Monkey will attempt to run '{0}' times</p>", numberOfRuns));

        WriteToReport(" <div class='row'><div class='col-12'>");
        // loop to run the following code for the random number of runs picked
        for (int currentRun = 0; currentRun < numberOfRuns; currentRun++)
        {
            // Randomly pick whether to fill no fields, random fields or all fields of a page
            int inputMode = randomNumber.Next(0, 3);
            switch (inputMode)
            {
                case 1:
                    InputRandomFields(driver);
                    SubmitForm(driver);
                    break;
                case 2:
                    InputAllFields(driver);
                    SubmitForm(driver);
                    break;
                default://input no fields
                    WriteToReport("- Not inputting into any fields");
                    SubmitForm(driver);
                    break;
            }
        }
        WriteToReport("</div></div>");
        WriteToReport(string.Format("<h3>Validator Monkey ended {0}</h3>", DateTime.Now.ToString()));
        FinishReport(); // add closing tags for html report file 
    }

    /// <summary>
    /// Input string in to random input fields
    /// </summary>
    /// <param name="driver">Selenium WebDriver being used</param>
    private static void InputRandomFields(IWebDriver driver)
    {
        IList<IWebElement> textInputs = driver.FindElements(By.CssSelector("input"));   // get all input fields on page
        int randomNumberOfInputFields = randomNumber.Next(0, textInputs.Count); // select a random number of fields to input
        int randomNumberOfTextAreaInputFields = randomNumber.Next(0, textInputs.Count); // select a random number of fields to input
        WriteToReport(string.Format("- Attempting to randomly complete {0} of {1} fields", randomNumberOfInputFields, textInputs.Count));

        for (int i = 0; i < randomNumberOfInputFields; i++)
        {
            int randomInputField = randomNumber.Next(0, textInputs.Count); // grab a input field at random
            String randomInput = inputs[randomNumber.Next(0, inputs.Count)]; // grab an input string at random
            try
            {
                textInputs[randomInputField].Clear(); //clear input out before input
                textInputs[randomInputField].SendKeys(randomInput);  //input the random stuff 
                WriteToReport(string.Format("- Inputting '{0}'", randomInput));
            }
            catch
            {
                WriteToReport(string.Format("** Error Inputting '{0}'", randomInput));
            }
        }
    }

    /// <summary>
    /// Input strings in to all input fields
    /// </summary>
    /// <param name="driver">Selenium WebDriver being used</param>
    private static void InputAllFields(IWebDriver driver)
    {
        IList<IWebElement> textInputs = driver.FindElements(By.CssSelector("input")); // get all text input fields on page
        WriteToReport(string.Format("- Attempting to complete all {0} fields", textInputs.Count));
        for (int i = 0; i < textInputs.Count; i++)
        {
            String randomInput = inputs[randomNumber.Next(0, inputs.Count)]; // grab an input string at random
            try
            {
                try
                {
                    // assume we are dealing with text box
                    textInputs[i].Clear(); //clear input out before input
                    textInputs[i].SendKeys(randomInput);  //input the random stuff 
                    WriteToReport(string.Format("- Inputting '{0}'", randomInput));
                }
                catch
                {
                    //   must be as drop down or checkbox etc as isn't input textbox
                    textInputs[i].SendKeys(Keys.Space);
                    WriteToReport(string.Format("- Selecting '{0}'", randomInput));
                }
            }
            catch
            {
                WriteToReport(string.Format("** Error Inputting '{0}'", randomInput));
            }
        }
    }

    /// <summary>
    /// Submit form
    /// </summary>
    /// <param name="driver">Selenium WebDriver being used</param>
    private static void SubmitForm(IWebDriver driver)
    {
        WriteToReport("- Screen before form submission");
        TakeScreenshot(); // screenshot before submit form
        IList<IWebElement> textInputs = driver.FindElements(By.CssSelector("input")); // get all text input fields on page
        IList<IWebElement> buttons = driver.FindElements(By.CssSelector("input[type='submit']")); // get page buttons
        IList<IWebElement> links = driver.FindElements(By.TagName("a")); // get page links

        //before we just attempt to submit on enter keypress - if we have both buttons and links, pick one at random
        if (buttons.Count > 0 && links.Count > 0)
        {
            int submitType = randomNumber.Next(0, 2);
            if (submitType == 0) // 0 = button
            {
                // click a random button on the form
                int randomButton = randomNumber.Next(0, buttons.Count);
                string buttonText = buttons[randomButton].GetAttribute("value");
                try
                {
                    buttons[randomButton].Click();
                    WriteToReport(string.Format("-- Clicking '{0}' button", buttonText));
                }
                catch
                {
                    WriteToReport(string.Format("** Error Clicking '{0}' button", buttonText));
                }
            }
            else // 1 = link
            {
                int randomLink = randomNumber.Next(0, links.Count);
                string linkText = links[randomLink].Text;
                try
                {
                    links[randomLink].Click();
                    WriteToReport(string.Format("-- Clicking '{0}' link", linkText));
                }
                catch
                {
                    WriteToReport(string.Format("** Error Clicking '{0}' link", linkText));
                }
            }
        }
        else
        {
            if (buttons.Count > 0)   // if we have buttons
            {
                // click a random button on the form
                int randomButton = randomNumber.Next(0, buttons.Count);
                buttons[randomButton].Click();
                WriteToReport(string.Format("-- Clicking '{0}' Button", buttons[randomButton].Text));
            }
            else // we did not find buttons, is the form using a link as a button instead ?
            {
                if (links.Count > 0) // we found links, click a random one
                {
                    int randomLink = randomNumber.Next(0, links.Count);
                    string linkText = links[randomLink].Text;
                    try
                    {
                        links[randomLink].Click();
                        WriteToReport(string.Format("-- Clicking '{0}' link", linkText));
                    }
                    catch
                    {
                        WriteToReport(string.Format("** Error Clicking '{0}' link", linkText));
                    }
                }
                else // we found no buttons or links so press enter in an input field instead to attempt to submit form
                {
                    int randomInput = randomNumber.Next(0, textInputs.Count);
                    string inputText = textInputs[randomInput].Text;
                    try
                    {
                        textInputs[randomInput].SendKeys(Keys.Enter);
                        WriteToReport(string.Format("-- Pressing Keys.Enter in '{0}' input", inputText));
                    }
                    catch
                    {
                        WriteToReport(string.Format("** Error Pressing Keys.Enter in '{0}' input", inputText));
                    }
                }
            }
        }
        TakeScreenshot(); //screenshot after form submission
        WriteToReport(string.Format("<p>Monkey navigated to URL: '{0}'", driver.Url));
    }

    /// <summary>
    /// Take a screenshot of the current WebDriver window
    /// </summary>
    private static void TakeScreenshot()
    {
        var screenshot = ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;
        WriteToReport(string.Format("<img src='data:image/jpeg;base64,{0}' width='650px'>", screenshot));
    }

    /// <summary>
    /// Create a new HTML Report
    /// </summary>
    private static void CreateReport()
    {
        File.AppendAllLines(@"C:\temp\" + reportFilename, new[] {
        "<html><head><title>Validator Monkey</title><link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css'/></head><body><div class='container'><h1>Validator Monkey</h1>"
        });
    }

    /// <summary>
    /// Finish writing the valid HTML markup for the report
    /// </summary>
    private static void FinishReport()
    {
        File.AppendAllLines(@"C:\temp\" + reportFilename, new[] {
        "</div></body></html>"
        });
    }

    /// <summary>
    /// Append line to HTML report
    /// </summary>
    /// <param name="logText"></param>
    private static void WriteToReport(string logText)
    {
        File.AppendAllLines(@"C:\temp\" + reportFilename, new[] { "<br>" + logText }); //add log text and go to new line
    }
}
