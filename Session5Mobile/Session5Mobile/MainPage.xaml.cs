using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Session5Mobile
{
    using Models;

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            List<Wells> wells = AppData.GetWells();
            ComboWells.ItemsSource = wells;
            ComboWells.SelectedIndex = 0;

            UpdateWell();
        }

        /// <summary>
        /// При выборе well из комбобокса
        /// </summary>
        private void ComboWells_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWell();
        }

        /// <summary>
        /// Отображение списка слоев 
        /// </summary>
        private void UpdateWell()
        {
            Wells well = ComboWells.SelectedItem as Wells;

            if (well == null)
                well = AppData.GetWells().FirstOrDefault();

            List<WellLayers> layers = AppData.GetWellLayers().Where(p => p.WellID == well.ID).ToList();

            int point = layers.LastOrDefault().EndPoint;

            WellLayers oilLayer = new WellLayers()
            {
                StartPoint = point,
                EndPoint = point + well.GasOilDepth
            };

            layers.Add(oilLayer);

            ListLayers.ItemsSource = layers.OrderBy(p => p.StartPoint).ToList();

            TextCapacity.Text = well.Capacity + " m3";
        }

        /// <summary>
        /// Добавление well
        /// </summary>
        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditWellPage(null));
        }

        /// <summary>
        /// Редактирование well
        /// </summary>
        private async void BtnEdit_Clicked(object sender, EventArgs e)
        {
            Wells selectedwell = ComboWells.SelectedItem as Wells;
            await Navigation.PushAsync(new AddEditWellPage(selectedwell));
        }
    }
}
