﻿// ***********************************************************************
// Copyright (c) 2017-2021 Yowko Tsai
// ***********************************************************************

using System;
using Microsoft.VisualStudio.TestPlatform.TestGeneration.Data;
using Microsoft.VisualStudio.TestPlatform.TestGeneration.Model;
using System.ComponentModel.Composition;
using EnvDTE;
//using static VSLangProj.PrjKind;
using System.Linq;
using VSLangProj80;
using VSLangProj;

namespace xUnit.net.TestGenerator
{
    /// <summary>
    /// The provider for the NUnit 3 unit test framework.
    /// </summary>
    [Export(typeof(IFrameworkProvider))]
    public class xUnitFrameworkProvider : FrameworkProviderBase
    {
        /// <summary>
        /// Unsupported testable project type guids.
        /// </summary>
        private readonly Guid[] unsupportedProjects =
        {
            Guid.Parse("BC8A1FFA-BEE3-4634-8014-F334798102B3"), // Windows Store
            Guid.Parse("C089C8C0-30E0-4E22-80C0-CE093F111A43"), // Phone Silverlight
            Guid.Parse("76F1466A-8B6D-4E39-A767-685A06062A39"), // Phone Appx
            Guid.Parse("786C830F-07A1-408B-BD7F-6EE04809D6DB"), // Portable app
            Guid.Parse("A1591282-1198-4647-A2B1-27E5FF5F6F3B"), // Silverlight
        };

        /// <summary>
        /// Service provider reference.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="xUnitFrameworkProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to use to get the interfaces required.</param>
        /// <param name="configurationSettings">The configuration settings object to be used to determine how the test method is generated.</param>
        /// <param name="naming">The naming object used to decide how projects, classes and methods are named and created.</param>
        /// <param name="directory">The directory object to use for directory operations.</param>
        [ImportingConstructor]
        public xUnitFrameworkProvider(IServiceProvider serviceProvider, IConfigurationSettings configurationSettings, INaming naming, IDirectory directory)
            : base(new xUnitSolutionManager(serviceProvider, naming, directory), new xUnitUnitTestProjectManager(serviceProvider, naming), new xUnitUnitTestClassManager(configurationSettings, naming))
        {
            this.serviceProvider = serviceProvider;
        }
        

        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        public override string Name => "xUnit.net 2.0";

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public override string AssemblyName => "xunit.core";

        /// <summary>
        /// Returns a value indicating whether a <see cref="Project"/> is a test project for the unit test framework.
        /// </summary>
        /// <remarks>
        /// In addition to the standard implementation, this checks for a reference to the MSTestv2Framework assembly used to test Windows Store Apps.
        /// </remarks>
        /// <param name="project">The <see cref="Project"/> to check.</param>
        /// <returns><c>True</c> if <paramref name="project"/> is a unit test project, <c>false</c> otherwise.</returns>
        public override bool IsTestProject(Project project)
        {
            bool result = false;

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            result = base.IsTestProject(project);
            if (!result)
            {
                if (project.Kind == PrjKind.prjKindCSharpProject || project.Kind == PrjKind.prjKindVBProject)
                    result = ProjectHasReference(project, "xunit.core");
            }

            return result;
        }

        /// <summary>
        /// Check if the current project is a testable project; i.e. tests can be generated for this project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>True if project if tests can be generated for this project.</returns>
        public override bool IsTestableProject(Project project)
        {
            // Base IsTestableProject filters C# and VB projects.
            //if (!base.IsTestableProject(project))
            if (!(project.Kind == PrjKind.prjKindCSharpProject || project.Kind == PrjKind.prjKindVBProject))
            {
                return false;
            }

            // Further restrict projects to C# desktop
            foreach (var projectGuid in project.ProjectTypeGuids(this.serviceProvider))
            {
                if (this.unsupportedProjects.Contains(projectGuid))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
