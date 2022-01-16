using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Xentab.Views
{
    /// <summary>
    /// Page to login with user name and password
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetPage" /> class.
        /// </summary>
        public SetPage()
        {
            this.InitializeComponent();
        }

        async public void ShowTable(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TablePage(), true);
        }
    }
}