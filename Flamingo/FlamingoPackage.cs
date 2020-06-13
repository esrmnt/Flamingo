using System;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using Task = System.Threading.Tasks.Task;

namespace Flamingo
{    
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(FlamingoPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(FlamingoProperties))]
    public sealed class FlamingoPackage : AsyncPackage
    {
        public const string PackageGuidString = "f25074c2-500e-468c-90ae-d7cf15a3d3c0";

        #region Package Members
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {            
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await XSLTRunCommand.InitializeAsync(this);
            await XSLTDebugCommand.InitializeAsync(this);
            await FlamingoPropertiesCommand.InitializeAsync(this);
        }

        #endregion
    }
}
