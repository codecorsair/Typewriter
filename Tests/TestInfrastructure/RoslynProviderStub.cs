﻿using System.Linq;
using EnvDTE;
using Microsoft.CodeAnalysis.MSBuild;
using Typewriter.CodeModel;
using Typewriter.CodeModel.Providers;
using Typewriter.CodeModel.Roslyn;

namespace Typewriter.Tests.TestInfrastructure
{
    public class RoslynProviderStub : ICodeModelProvider
    {
        private readonly Microsoft.CodeAnalysis.Workspace workspace;

        public RoslynProviderStub()
        {
            //var componentModel = ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel)) as IComponentModel;
            //this.workspace = componentModel.GetService<VisualStudioWorkspace>();

            var solutionPath = @"C:\Dev\Typewriter\Typewriter\Typewriter.sln";
            var msBuildWorkspace = MSBuildWorkspace.Create();
            var solution = msBuildWorkspace.OpenSolutionAsync(solutionPath).Result;

            this.workspace = msBuildWorkspace;
        }

        public File GetFile(ProjectItem projectItem)
        {
            var project = workspace.CurrentSolution.Projects.FirstOrDefault(p => p.FilePath == projectItem.ContainingProject.FullName);
            var document = project?.Documents.FirstOrDefault(d => d.FilePath == projectItem.FileNames[1]);

            return new RoslynFile(document);
        }
    }
}
