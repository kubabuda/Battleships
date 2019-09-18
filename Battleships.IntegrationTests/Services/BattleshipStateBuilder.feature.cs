// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.0.0.0
//      SpecFlow Generator Version:3.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Battleships.IntegrationTests.Services
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("BattleshipStateBuilder")]
    public partial class BattleshipStateBuilderFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "BattleshipStateBuilder.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "BattleshipStateBuilder", "\tIn order to get Battleship game state\r\n\tAs a someone playing Battleships\r\n\tI wan" +
                    "t to get proper game states", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Place all ship on new game board")]
        [NUnit.Framework.TestCaseAttribute("2,3,4", null)]
        [NUnit.Framework.TestCaseAttribute("4,2,3", null)]
        public virtual void PlaceAllShipOnNewGameBoard(string ships, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Place all ship on new game board", null, exampleTags);
#line 7
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 8
 testRunner.Given("New state builder instance", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.And(string.Format("I have entered \'{0}\' into configuration", ships), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.When("I generate new game state", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 11
 testRunner.Then(string.Format("All \'{0}\' are placed on board", ships), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Next state with invalid guess")]
        [NUnit.Framework.TestCaseAttribute("A11", null)]
        [NUnit.Framework.TestCaseAttribute("AA1", null)]
        [NUnit.Framework.TestCaseAttribute("M1", null)]
        [NUnit.Framework.TestCaseAttribute("MAAAA1", null)]
        [NUnit.Framework.TestCaseAttribute("FooBar", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        public virtual void NextStateWithInvalidGuess(string invalidGuess, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Next state with invalid guess", null, exampleTags);
#line 19
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 20
 testRunner.Given("New state builder instance", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 21
 testRunner.When(string.Format("I generate next state with \'{0}\'", invalidGuess), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 22
 testRunner.Then("InvalidInputException is thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Shooting new spot")]
        [NUnit.Framework.TestCaseAttribute("Empty", "1", "1", "A1", "Miss", null)]
        [NUnit.Framework.TestCaseAttribute("Ship", "1", "1", "A1", "Hit", null)]
        public virtual void ShootingNewSpot(string state, string line, string column, string guess, string nextState, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Shooting new spot", null, exampleTags);
#line 34
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 35
 testRunner.Given("New state builder instance", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 36
 testRunner.And("I have previous game state", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
 testRunner.And(string.Format("\'{0}\' in prev state \'{1}\' \'{2}\'", state, line, column), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
 testRunner.When(string.Format("I generate next state with \'{0}\'", guess), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 39
 testRunner.Then(string.Format("\'{0}\' \'{1}\' is in \'{2}\'", line, column, nextState), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Shooting the same spot twice")]
        [NUnit.Framework.TestCaseAttribute("Hit", "1", "1", "A1", null)]
        [NUnit.Framework.TestCaseAttribute("Miss", "1", "1", "A1", null)]
        public virtual void ShootingTheSameSpotTwice(string state, string line, string column, string invalidGuess, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Shooting the same spot twice", null, exampleTags);
#line 47
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 48
 testRunner.Given("New state builder instance", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 49
 testRunner.And("I have previous game state", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
 testRunner.And(string.Format("\'{0}\' in prev state \'{1}\' \'{2}\'", state, line, column), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
 testRunner.When(string.Format("I generate next state with \'{0}\'", invalidGuess), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 52
 testRunner.Then("CellRepetitionException is thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion