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

            List<Wells> wells = AppData.GetWells();
            ComboWells.ItemsSource = wells;
            ComboWells.SelectedIndex = 0;

            UpdateWell();
        }

        private void BtnEdit_Clicked(object sender, EventArgs e)
        {

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

            List<WellLayers> layers = AppData.GetWellLayers().Where(p => p.WellID == well.ID).ToList();

            ListLayers.ItemsSource = layers.OrderBy(p => p.StartPoint).ToList();

            TextCapacity.Text = well.Capacity + " m3";
        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {

        }
    } 
}
