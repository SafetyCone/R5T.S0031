using System;
using System.Threading.Tasks;


namespace R5T.S0031.Deploy
{
    class Program
    {
        static async Task Main()
        {
            await DeployScripts.Instance.DeployToLocalCloudBinariesDirectory();
            // await DeployScripts.Instance.DeployToRemoteBinariesDirectory();
        }
    }
}
