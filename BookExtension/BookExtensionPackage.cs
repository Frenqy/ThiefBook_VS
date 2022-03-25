using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace BookExtension
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(BookExtensionPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(OptionPageGrid),
        "Frenqy Category", "BooksOpts", 0, 0, true)]

    public sealed class BookExtensionPackage : AsyncPackage
    {
        /// <summary>
        /// BookExtensionPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "e59631f9-1ae0-46be-98aa-f0afdbb5c7ea";

        #region Package Members

        public OptionPageGrid Dialog => GetDialogPage(typeof(OptionPageGrid)) as OptionPageGrid;

        public const string Prefix = "                                                          ";

        public int BookPage
        {
            get
            {
                return Dialog.OptBookPage;
            }
            set
            {
                Dialog.OptBookPage = value;
            }
        }

        public string CacheString
        {
            get
            {
                if (cachePath != Dialog.OptBookPath)
                {
                    cachePath = Dialog.OptBookPath;
                    cacheString = File.ReadAllText(cachePath);
                    TotalPage = (int)Math.Round(cacheString.Length / 50f, MidpointRounding.AwayFromZero);
                }
                return cacheString;
            }
        }

        public int PerPageCount => Dialog.OptPerPageCount;

        public int TotalPage { get; private set; }
        private string cacheString = string.Empty;
        private string cachePath = string.Empty;

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await HidePage.InitializeAsync(this);
            await LastPage.InitializeAsync(this);
            await NextPage.InitializeAsync(this);
        }

        #endregion
    }

    public class OptionPageGrid : DialogPage
    {
        private int optPage = 0;
        private string bookPath = "";
        private int perPageCount = 50;

        [Category("Frenqy Category")]
        [DisplayName("Current Page")]
        [Description("Current Page")]
        public int OptBookPage
        {
            get { return optPage; }
            set { optPage = value; }
        }

        [Category("Frenqy Category")]
        [DisplayName("Book Path")]
        [Description("Book Path")]
        public string OptBookPath
        {
            get { return bookPath; }
            set { bookPath = value; }
        }

        [Category("Frenqy Category")]
        [DisplayName("PerPageCount")]
        [Description("PerPageCount")]
        public int OptPerPageCount
        {
            get { return perPageCount; }
            set { perPageCount = value; }
        }
    }

}
