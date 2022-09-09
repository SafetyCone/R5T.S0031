using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using R5T.D0037;
using R5T.D0084.D001;
using R5T.D0105;
using R5T.T0020;


namespace R5T.S0031
{
    /// <summary>
    /// Git commit and then push to GitHub all changes in all local repositories using a single commit message.
    /// </summary>
    public class O000_Main : IActionOperation
    {
        public IAllRepositoryDirectoryPathsProvider AllRepositoryDirectoryPathsProvider { get; }
        public IGitOperator GitOperator { get; }
        public ILogger Logger { get; }
        private INotepadPlusPlusOperator NotepadPlusPlusOperator { get; }


        public O000_Main(
            IAllRepositoryDirectoryPathsProvider allRepositoryDirectoryPathsProvider,
            IGitOperator gitOperator,
            ILogger<O000_Main> logger,
            INotepadPlusPlusOperator notepadPlusPlusOperator)
        {
            this.AllRepositoryDirectoryPathsProvider = allRepositoryDirectoryPathsProvider;
            this.GitOperator = gitOperator;
            this.Logger = logger;
            this.NotepadPlusPlusOperator = notepadPlusPlusOperator;
        }

        public async Task Run()
        {
            /// Inputs.
            //var commitMessage = "All changes required for Seville functionality (survey service implementations in project file and generate";
            var commitMessage = "Interim changes.";

            /// Run.
            // Get all local repositories.
            var allRepositoryDirectoryPaths = await this.AllRepositoryDirectoryPathsProvider.GetAllRepositoryDirectoryPaths();

            //// For debug.
            //allRepositoryDirectoryPaths = allRepositoryDirectoryPaths.Where(x => x.Contains("Z0000")).ToArray();

            // Get all local repositories with changes.
            var repositoryDirectoryPaths_WithLocalChanges = new List<string>();

            foreach (var repositoryDirectoryPath in allRepositoryDirectoryPaths)
            {
                this.Logger.LogInformation($"Processing repository:\n{repositoryDirectoryPath}...");

                var hasLocalChanges = false;
                try
                {
                    hasLocalChanges = await this.GitOperator.HasUnpushedLocalChanges(
                        T0010.LocalRepositoryDirectoryPath.From(repositoryDirectoryPath));
                }
                catch
                {
                    // The has-local-changes variable is initially false.

                    this.Logger.LogError($"Directory is not a repository:\n{repositoryDirectoryPath}");
                }

                if(hasLocalChanges)
                {
                    this.Logger.LogInformation($"Directory has local changes:\n{repositoryDirectoryPath}.");

                    repositoryDirectoryPaths_WithLocalChanges.Add(repositoryDirectoryPath);
                }
            }

            //// Temp output.
            //var outputFilePath = @"C:\Temp\Output.txt";

            //await FileHelper.WriteAllLines(
            //    outputFilePath,
            //    repositoryDirectoryPaths_WithLocalChanges
            //        .Select(xRepositoryDirectoryPath => Instances.RepositoryNameOperator.GetRepositoryNameFromRepositoryDirectoryPath(xRepositoryDirectoryPath))
            //        .OrderAlphabetically());

            //await this.NotepadPlusPlusOperator.OpenFilePath(outputFilePath);

            // For each repository with local changes.
            foreach (var repositoryDirectoryPath in repositoryDirectoryPaths_WithLocalChanges)
            {
                this.Logger.LogInformation($"Processing repository:\n{repositoryDirectoryPath}...");

                var localRepositoryDirectoryPath = T0010.LocalRepositoryDirectoryPath.From(repositoryDirectoryPath);

                // Stage all changes.
                this.Logger.LogInformation("Staging changes...");

                await this.GitOperator.StageAllUnstagedPaths(localRepositoryDirectoryPath);

                // Commit changes with the comment message.
                this.Logger.LogInformation("Committing changes...");

                await this.GitOperator.Commit(
                    localRepositoryDirectoryPath,
                    commitMessage);

                // Push changes to GitHub.
                this.Logger.LogInformation("Pushing changes...");

                await this.GitOperator.Push(localRepositoryDirectoryPath);
            }
        }
    }
}
