using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Session5Mobile
{
    using Models;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditWellPage : ContentPage
    {
        Wells currentWell = new Wells();
        List<WellLayers> layers = new List<WellLayers>();

        public AddEditWellPage(Wells well)
        {
            InitializeComponent();

            if (well != null)
            {
                currentWell = well;
            }

            layers = AppData.GetWellLayers().Where(p => p.WellID == currentWell.ID).OrderBy(p => p.StartPoint).ToList();
            ListLayers.ItemsSource = layers;

            BindingContext = currentWell;

            List<RockTypes> rocks = AppData.GetRockTypes();
            ComboRocks.ItemsSource = rocks;
        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnRemove_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {

        }
    }
}