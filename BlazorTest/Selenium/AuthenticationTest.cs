using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BlazorTest.Selenium;


public class AuthenticationTest : IDisposable
{

    private readonly IWebDriver driver;

    public AuthenticationTest()
    {
        driver = new Driver().GetDriver();
        driver.Navigate().GoToUrl(new Uri("http://localhost:5198"));
    }


    [Fact]
    public void TestKingFunction()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        var div = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("createGame")));

        driver.FindElement(By.Id("login")).Click();
    }


    public void Dispose()
    {
        driver.Quit();
    }
}

