// ***********************************************************************
// Copyright (c) 2017 Yowko Tsai
// ***********************************************************************

using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TestPlatform.TestGeneration;
using Microsoft.VisualStudio.TestPlatform.TestGeneration.Data;
using Microsoft.VisualStudio.TestPlatform.TestGeneration.Logging;
using Microsoft.VisualStudio.TestPlatform.TestGeneration.Model;
using VSLangProj80;

namespace xUnit.net.TestGenerator
{
    public class xUnitSolutionManager : SolutionManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="xUnitSolutionManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to use to get the interfaces required.</param>
        /// <param name="naming">The naming object used to decide how projects, classes and methods are named and created.</param>
        /// <param name="directory">The directory object to use for directory operations.</param>
        public xUnitSolutionManager(IServiceProvider serviceProvider, INaming naming, IDirectory directory)
            : base(serviceProvider, naming, directory)
        {
        }

        /// <summary>
        /// Performs any preparatory tasks that have to be done after a new unit test project has been created.
        /// </summary>
        /// <param name="unitTestProject">The <see cref="Project"/> of the unit test project that has just been created.</param>
        /// <param name="sourceMethod">The <see cref="CodeFunction2"/> of the source method that is to be unit tested.</param>
        protected override void OnUnitTestProjectCreated(Project unitTestProject, CodeFunction2 sourceMethod)
        {
            if (unitTestProject == null)
                throw new ArgumentNullException(nameof(unitTestProject));
            if (sourceMethod == null)
                throw new ArgumentNullException(nameof(sourceMethod));

            TraceLogger.LogInfo("xUnitSolutionManager.OnUnitTestProjectCreated: Adding reference to NUnit assemblies through nuget.");

            base.OnUnitTestProjectCreated(unitTestProject, sourceMethod);

            this.EnsureNuGetReference(unitTestProject, "xunit", "2.4.2");
            this.EnsureNuGetReference(unitTestProject, "xunit.runner.visualstudio", "2.4.3");


            var vsp = unitTestProject.Object as VSProject2;
            var reference = vsp?.References.Find(GlobalConstants.MSTestAssemblyName);
            if (reference != null)
            {
                TraceLogger.LogInfo("xUnitSolutionManager.OnUnitTestProjectCreated: Removing reference to {0}", reference.Name);
                reference.Remove();
            }
        }
    }
}