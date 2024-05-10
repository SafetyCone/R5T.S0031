using System;


namespace R5T.S0031
{
    public static class Instances
    {
        public static Z0036.ICommitMessages CommitMessages => Z0036.CommitMessages.Instance;
        public static F0082.IFileSystemOperator FileSystemOperator => F0082.FileSystemOperator.Instance;
        public static T0070.IHost Host => T0070.Host.Instance;
        public static Z0022.IRepositoriesDirectoryPathsSets RepositoriesDirectoryPathsSets => Z0022.RepositoriesDirectoryPathsSets.Instance;
        public static T0108.IRepositoryNameOperator RepositoryNameOperator => T0108.RepositoryNameOperator.Instance;
        public static T0062.IServiceAction ServiceAction => T0062.ServiceAction.Instance;
    }
}