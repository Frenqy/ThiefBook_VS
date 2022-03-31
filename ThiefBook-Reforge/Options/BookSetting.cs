using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ThiefBook_Reforge
{
    internal partial class OptionsProvider
    {
        [ComVisible(true)]
        [Guid("E0CB25CF-5092-4665-BBAD-A97D0F6A0048")]
        public class BookSettingOptions : BaseOptionPage<BookSetting> { }
    }

    public class BookSetting : BaseOptionModel<BookSetting>
    {
        [Category("ThiefBook")]
        [DisplayName("Book Path")]
        [Description("Book Path")]
        [DefaultValue("")]
        public string BookPath { get; set; } = "";

        [Category("ThiefBook")]
        [DisplayName("Current Page")]
        [Description("Current Page")]
        [DefaultValue(1)]
        public int CurrentPage { get; set; } = 1;

        [Category("ThiefBook")]
        [DisplayName("Page Size")]
        [Description("PerPageTextCount")]
        [DefaultValue(50)]
        public int PageSize { get; set; } = 50;
    }
}
