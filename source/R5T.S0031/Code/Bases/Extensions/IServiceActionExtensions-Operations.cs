using System;

using R5T.D0037;
using R5T.D0084.D001;
using R5T.D0105;
using R5T.L0017.D001;
using R5T.T0062;
using R5T.T0063;


namespace R5T.S0031
{
    public static partial class IServiceActionExtensions
    {
        /// <summary>
        /// Adds the <see cref="O000_Main"/> operation as a <see cref="Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<O000_Main> AddO000_MainAction(this IServiceAction _,
            IServiceAction<IAllRepositoryDirectoryPathsProvider> allRepositoryDirectoryPathsProviderAction,
            IServiceAction<IGitOperator> gitOperatorAction,
            IServiceAction<ILoggerUnbound> loggerUnboundAction,
            IServiceAction<INotepadPlusPlusOperator> notepadPlusPlusOperatorAction)
        {
            var serviceAction = _.New<O000_Main>(services => services.AddO000_Main(
                allRepositoryDirectoryPathsProviderAction,
                gitOperatorAction,
                loggerUnboundAction,
                notepadPlusPlusOperatorAction));

            return serviceAction;
        }
    }
}