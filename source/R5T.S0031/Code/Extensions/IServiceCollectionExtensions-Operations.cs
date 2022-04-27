using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.D0037;
using R5T.D0084.D001;
using R5T.D0105;
using R5T.L0017.D001;
using R5T.T0063;


namespace R5T.S0031
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="O000_Main"/> operation as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddO000_Main(this IServiceCollection services,
            IServiceAction<IAllRepositoryDirectoryPathsProvider> allRepositoryDirectoryPathsProviderAction,
            IServiceAction<IGitOperator> gitOperatorAction,
            IServiceAction<ILoggerUnbound> loggerUnboundAction,
            IServiceAction<INotepadPlusPlusOperator> notepadPlusPlusOperatorAction)
        {
            services
                .Run(allRepositoryDirectoryPathsProviderAction)
                .Run(gitOperatorAction)
                .Run(loggerUnboundAction)
                .Run(notepadPlusPlusOperatorAction)
                .AddSingleton<O000_Main>();

            return services;
        }
    }
}