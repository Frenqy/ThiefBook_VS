using Microsoft.VisualStudio.Threading;
using System.Diagnostics;

namespace ThiefBook_Reforge
{
    [Command(PackageIds.HidePageCommand)]
    internal sealed class HidePageCommand : BaseCommand<HidePageCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();

            ThiefBook_ReforgePackage package = Package as ThiefBook_ReforgePackage;
            var setting = await BookSetting.GetLiveInstanceAsync();
            setting.CurrentPage = Helper.Clamp(--setting.CurrentPage, 0, package.TotalPage - 1);
            await setting.SaveAsync();

            StatusBar statusBar = VS.StatusBar;
            await statusBar.ShowMessageAsync("");
        }
    }

    [Command(PackageIds.LastPageCommand)]
    internal sealed class LastPageCommand : BaseCommand<LastPageCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();

            ThiefBook_ReforgePackage package = Package as ThiefBook_ReforgePackage;
            var setting = await BookSetting.GetLiveInstanceAsync();
            setting.CurrentPage = Helper.Clamp(--setting.CurrentPage, 0, package.TotalPage - 1);
            await setting.SaveAsync();

            var text = package.GetCurrentString(setting.BookPath, setting.CurrentPage, setting.PageSize);
            StatusBar statusBar = VS.StatusBar;
            await statusBar.ShowMessageAsync(text);
        }
    }

    [Command(PackageIds.NextPageCommand)]
    internal sealed class NextPageCommand : BaseCommand<NextPageCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();

            ThiefBook_ReforgePackage package = Package as ThiefBook_ReforgePackage;
            var setting = await BookSetting.GetLiveInstanceAsync();
            setting.CurrentPage = Helper.Clamp(++setting.CurrentPage, 0, package.TotalPage - 1);
            await setting.SaveAsync();

            var text = package.GetCurrentString(setting.BookPath, setting.CurrentPage, setting.PageSize);
            StatusBar statusBar = VS.StatusBar;
            await statusBar.ShowMessageAsync(text);
        }
    }
}
