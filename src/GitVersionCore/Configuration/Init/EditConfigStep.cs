namespace GitVersion.Configuration.Init
{
    using System.Collections.Generic;
    using GitVersion.Configuration.Init.BuildServer;
    using GitVersion.Configuration.Init.SetConfig;
    using GitVersion.Configuration.Init.Wizard;
    using GitVersion.Helpers;

    public class EditConfigStep : ConfigInitWizardStep
    {
        public EditConfigStep(IConsole console, IFileSystem fileSystem) : base(console, fileSystem)
        {
        }

        protected override StepResult HandleResult(string result, Queue<ConfigInitWizardStep> steps, Config config, string workingDirectory)
        {
            switch (result)
            {
                case "0":
                    return StepResult.SaveAndExit();
                case "1":
                    return StepResult.ExitWithoutSaving();

                case "2":
                    steps.Enqueue(new PickBranchingStrategyStep(Console, FileSystem));
                    return StepResult.Ok();

                case "3":
                    steps.Enqueue(new SetNextVersion(Console, FileSystem));
                    return StepResult.Ok();

                case "4":
                    steps.Enqueue(new ConfigureBranches(Console, FileSystem));
                    return StepResult.Ok();
                case "5":
                    steps.Enqueue(new GlobalModeSetting(new EditConfigStep(Console, FileSystem), false, Console, FileSystem));
                    return StepResult.Ok();
                case "6":
                    steps.Enqueue(new AssemblyVersioningSchemeSetting(Console, FileSystem));
                    return StepResult.Ok();
                case "7":
                    steps.Enqueue(new AssemblyInformationalVersioningSchemeSetting(Console, FileSystem));
                    return StepResult.Ok();
                case "8":
                    steps.Enqueue(new SetupBuildScripts(Console, FileSystem));
                    return StepResult.Ok();
            }
            return StepResult.InvalidResponseSelected();
        }

        protected override string GetPrompt(Config config, string workingDirectory)
        {
            return string.Format(@"Which would you like to change?

0) Save changes and exit
1) Exit without saving

2) Run getting started wizard

3) Set next version number
4) Branch specific configuration
5) Branch Increment mode (per commit/after tag) (Current: {0})
6) Assembly versioning scheme (Current: {1})
7) Assembly informational versioning scheme (Current: {2})
8) Setup build scripts", config.VersioningMode, config.AssemblyVersioningScheme, config.AssemblyInformationalVersioningScheme);
        }

        protected override string DefaultResult
        {
            get { return null; }
        }
    }
}