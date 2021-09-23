// ***********************************************************************
// Copyright (c) 2017 Yowko Tsai
// ***********************************************************************

using System;
using Microsoft.VisualStudio.TestPlatform.TestGeneration.Model;
using EnvDTE;

namespace xUnit.net.TestGenerator
{
    /// <summary>
    /// A unit test project for xUnit unit tests.
    /// </summary>
    public class xUnitUnitTestProjectManager : UnitTestProjectManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="xUnitUnitTestProjectManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to use to get the interfaces required.</param>
        /// <param name="naming">The naming object used to decide how projects, classes and methods are named and created.</param>
        public xUnitUnitTestProjectManager(IServiceProvider serviceProvider, INaming naming)
            : base(serviceProvider, naming)
        {
        }
        /// <summary>
        /// Returns the full namespace that contains the test framework code elements for a given source project.
        /// </summary>
        /// <param name="sourceProject">The source project.</param>
        /// <returns>The full namespace that contains the test framework code elements.</returns>
        public override string FrameworkNamespace(Project sourceProject)
        {
            return "Xunit";
        }
    }
}