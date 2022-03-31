global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ThiefBook_Reforge
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.ThiefBook_ReforgeString)]
    [ProvideOptionPage(typeof(OptionsProvider.BookSettingOptions), "ThiefBook_Reforge", "BookSetting", 0, 0, true, SupportsProfiles = true)]
    public sealed class ThiefBook_ReforgePackage : ToolkitPackage
    {
        public int TotalPage { get; private set; } = 0;
        private readonly StringBuilder sb = new();
        private const string PREFIX = "                                                          ";
        private string cacheString = string.Empty;
        private string cachePath = string.Empty;

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.RegisterCommandsAsync();
            var setting = await BookSetting.GetLiveInstanceAsync();
            TryInit(setting.BookPath, setting.PageSize);
        }

        public void TryInit(string currentPath, int pageSize)
        {
            if (cachePath != currentPath)
            {
                cachePath = currentPath;
                cacheString = File.ReadAllText(cachePath);
                TotalPage = (int)Math.Ceiling(cacheString.Length / (float)pageSize);
            }
        }

        public string GetCurrentString(string currentPath, int currentPage, int pageSize)
        {
            TryInit(currentPath, pageSize);

            var startIndex = (currentPage - 1) * pageSize;
            var size = Math.Min(pageSize, cacheString.Length - startIndex);
            sb.Clear();
            sb.Append(PREFIX);
            sb.Append(cacheString.Substring(startIndex, size));
            sb.Append("\t//");
            sb.Append(currentPage);
            sb.Append('/');
            sb.Append(TotalPage);
            return sb.ToString();
        }

    }
}