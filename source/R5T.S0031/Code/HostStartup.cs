using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using R5T.A0003;
using R5T.D0037.A002;
using R5T.D0048.Default;
using R5T.D0081.I001;
using R5T.D0082.A001;
using R5T.D0084.D001.I002;
using R5T.D0084.D002.I001;
using R5T.D0088.I0002;
using R5T.D0094.I001;
using R5T.D0095.I001;
using R5T.D0105.I001;
using R5T.Magyar;
using R5T.Ostrogothia.Rivet;
using R5T.T0063;


using IProvidedServiceActionAggregation = R5T.D0088.I0002.IProvidedServiceActionAggregation;
using IRequiredServiceActionAggregation = R5T.D0088.I0002.IRequiredServiceActionAggregation;
using ServicesPlatformRequiredServiceActionAggregation = R5T.A0003.RequiredServiceActionAggregation;


namespace R5T.S0031
{
    public class HostStartup : HostStartupBase
    {
        public override Task ConfigureConfiguration(IConfigurationBuilder configurationBuilder)
        {
            // Do nothing.
        
            return Task.CompletedTask;
        }

        protected override Task ConfigureServices(IServiceCollection services, IProvidedServiceActionAggregation providedServicesAggregation)
        {
            // Inputs.
            var executionSynchronicityProviderAction = Instances.ServiceAction.AddConstructorBasedExecutionSynchronicityProviderAction(Synchronicity.Synchronous);

            var organizationProviderAction = Instances.ServiceAction.AddOrganizationProviderAction(); // Rivet organization.

            var rootOutputDirectoryPathProviderAction = Instances.ServiceAction.AddConstructorBasedRootOutputDirectoryPathProviderAction(@"C:\Temp\Output");

            // Services platform.
            var servicesPlatformRequiredServiceActionAggregation = new ServicesPlatformRequiredServiceActionAggregation
            {
                ConfigurationAction = providedServicesAggregation.ConfigurationAction,
                ExecutionSynchronicityProviderAction = executionSynchronicityProviderAction,
                LoggerAction = providedServicesAggregation.LoggerAction,
                LoggerFactoryAction = providedServicesAggregation.LoggerFactoryAction,
                MachineMessageOutputSinkProviderActions = EnumerableHelper.Empty<IServiceAction<D0099.D002.IMachineMessageOutputSinkProvider>>(),
                MachineMessageTypeJsonSerializationHandlerActions = EnumerableHelper.Empty<IServiceAction<D0098.IMachineMessageTypeJsonSerializationHandler>>(),
                OrganizationProviderAction = organizationProviderAction,
                RootOutputDirectoryPathProviderAction = rootOutputDirectoryPathProviderAction,
            };

            var servicesPlatform = Instances.ServiceAction.AddProvidedServiceActionAggregation(
                servicesPlatformRequiredServiceActionAggregation);

            var loggerUnbound = providedServicesAggregation.LoggerAction;


            // Logging.
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddConsole(
                        servicesPlatform.LoggerSynchronicityProviderAction)
                    .AddFile(
                        servicesPlatform.LogFilePathProviderAction,
                        servicesPlatform.LoggerSynchronicityProviderAction)
                    ;
            });

            // Notepad++
            var notepadPlusPlusExecutableFilePathProviderAction = Instances.ServiceAction.AddHardCodedNotepadPlusPlusExecutableFilePathProviderAction();

            var notepadPlusPlusOperatorAction = Instances.ServiceAction.AddNotepadPlusPlusOperatorAction(
                //commandLineOperatorAction,
                servicesPlatform.BaseCommandLineOperatorAction,
                notepadPlusPlusExecutableFilePathProviderAction);

            // Services.
            // Level 00.

            // Level 01.
            var repositoriesDirectoryPathProviderAction = Instances.ServiceAction.AddConstructorBasedRepositoriesDirectoryPathProviderAction(
            @"C:\Code\DEV\Git\GitHub\SafetyCone");
            //@"C:\Code\DEV\Git\GitHub\davidcoats");
            var gitHubOperatorServiceActions = Instances.ServiceAction.AddGitHubOperatorServiceActions(
                servicesPlatform.SecretsDirectoryFilePathProviderAction);

            // Level 02.
            var allRepositoryDirectoryPathsProviderAction = Instances.ServiceAction.AddAllRepositoryDirectoryPathsProviderAction(
                repositoriesDirectoryPathProviderAction);
            var gitOperatorServices = Instances.ServiceAction.AddGitOperatorServices(
                gitHubOperatorServiceActions.GitHubAuthenticationProviderAction,
                servicesPlatform.SecretsDirectoryFilePathProviderAction);


            // Operations.
            // Level 00.
            var o000_MainAction = Instances.ServiceAction.AddO000_MainAction(
                allRepositoryDirectoryPathsProviderAction,
                gitOperatorServices.GitOperatorAction,
                loggerUnbound,
                notepadPlusPlusOperatorAction);


            // Run.
            services.MarkAsServiceCollectonConfigurationStatement()
                // Services.
                .Run(servicesPlatform.ConfigurationAuditSerializerAction)
                .Run(servicesPlatform.ServiceCollectionAuditSerializerAction)
                // Operations
                .Run(o000_MainAction)
                ;

            return Task.CompletedTask;
        }


        protected override Task FillRequiredServiceActions(IRequiredServiceActionAggregation requiredServiceActions)
        {
            // Do nothing since none are required.
        
            return Task.CompletedTask;
        }
    }
}