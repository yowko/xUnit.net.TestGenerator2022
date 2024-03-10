// ***********************************************************************
// Copyright (c) 2017 Yowko Tsai
// ***********************************************************************

using Microsoft.VisualStudio.TestPlatform.TestGeneration.Data;
using Microsoft.VisualStudio.TestPlatform.TestGeneration.Model;

namespace xUnit.net.TestGenerator
{
    /// <summary>
    /// A unit test class for xUnit unit tests.
    /// </summary>
    public class xUnitUnitTestClassManager : UnitTestClassManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="xUnitUnitTestClassManager"/> class.
        /// </summary>
        /// <param name="configurationSettings">The configuration settings object to be used to determine how the test method is generated.</param>
        /// <param name="naming">The object to be used to give names to test projects.</param>
        public xUnitUnitTestClassManager(IConfigurationSettings configurationSettings, INaming naming)
            : base(configurationSettings, naming)
        {
            
        }

        /// <summary>
        /// The attribute name for marking a class as a test class.
        /// </summary>
        public override string TestClassAttribute => string.Empty;

        /// <summary>
        /// The attribute name for marking a method as a test.
        /// </summary>
        public override string TestMethodAttribute => "Fact";

        /// <summary>
        /// The code to force a test failure.
        /// </summary>
        public override string AssertionFailure => "Xunit.Assert.Fail(\"This test needs an implementation\")";
    }
}