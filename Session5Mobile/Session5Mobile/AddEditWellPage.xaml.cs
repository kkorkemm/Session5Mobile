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
    using Newtonsoft.Json;
    using System.Net;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditWellPage : ContentPage
    {
        private WebClient client = new WebClient();
        private Wells currentWell = new Wells();
        private List<WellLayers> layers = new List<WellLayers>();
        private List<WellLayers> layersForRemove = new List<WellLayers>();
        private string address = AppData.CheckDevice();

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
            StringBuilder errors = new StringBuilder();

            #region проверка на заполненность полей
            if (ComboRocks.SelectedItem == null)
                errors.AppendLine("Выберите rock");
            if (string.IsNullOrWhiteSpace(TextStart.Text))
                errors.AppendLine("Укажите начальную точку");
            if (string.IsNullOrWhiteSpace(TextEnd.Text))
                errors.AppendLine("Укажите конечную точку");

            if (errors.Length > 0)
            {
                DisplayAlert("Warning!", errors.ToString(), "Ok");
                return;
            }
            #endregion

            #region Проверка введенных чисел
            try
            {
                int positiveNumber = Convert.ToInt32(TextStart.Text);
                if (positiveNumber < 0)
                    errors.AppendLine("Значения глубин должны быть положительными");
            }
            catch
            {
                errors.AppendLine("Значения глубин должны быть числами");
            }

            try
            {
                int positiveNumber = Convert.ToInt32(TextEnd.Text);
                if (positiveNumber < 0)
                    errors.AppendLine("Значения глубин должны быть положительными");
            }
            catch
            {
                errors.AppendLine("Значения глубин должны быть числами");
            }

            if (errors.Length > 0)
            {
                DisplayAlert("Warning!", errors.ToString(), "Ok");
                return;
            }
            #endregion

            // проверка на минимальную глубину
            if (int.Parse(TextEnd.Text) - int.Parse(TextStart.Text) < 100)
                errors.AppendLine("Глубина слоя не может быть меньше 100");

            // проверка на наличие одинаковых слоев
            RockTypes rock = ComboRocks.SelectedItem as RockTypes;
            var sameRock = layers.FirstOrDefault(p => p.RockTypeID == rock.ID);
            if (sameRock != null)
                errors.AppendLine("Слой такого типа уже добавлен в список");

            #region проверка на перекрытие слоев
            foreach (var item in layers)
            {
                if (item.StartPoint < int.Parse(TextStart.Text) && item.EndPoint > int.Parse(TextEnd.Text))
                {
                    errors.AppendLine("Слой помещен внутри другого слоя");
                    break;
                }
                if (item.EndPoint > int.Parse(TextStart.Text) && item.EndPoint < int.Parse(TextEnd.Text))
                {
                    errors.AppendLine("Слои перекрываются");
                    break;
                }
            }

            if (errors.Length > 0)
            {
                DisplayAlert("Warning!", errors.ToString(), "Ok");
                return;
            }
            #endregion

            WellLayers newLayer = new WellLayers()
            {
                WellID = currentWell.ID,
                RockTypeID = (ComboRocks.SelectedItem as RockTypes).ID,
                StartPoint = int.Parse(TextStart.Text),
                EndPoint = int.Parse(TextEnd.Text)
            };

            layers.Add(newLayer);
            ListLayers.ItemsSource = layers.OrderBy(p => p.StartPoint).ToList();

            TextStart.Text = "";
            TextEnd.Text = "";
            ComboRocks.SelectedItem = null;
        }

        /// <summary>
        /// Удаление из листа
        /// </summary>
        private void BtnRemove_Clicked(object sender, EventArgs e)
        {
            WellLayers layer = (sender as Button).BindingContext as WellLayers;
            layersForRemove.Add(layer);
            layers.Remove(layer);
            ListLayers.ItemsSource = layers.ToList();
        }

        
        private async void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(currentWell.WellName))
                errors.AppendLine("Введите название well");

            #region проверка положительности чисел
            try
            {
                int positive = currentWell.Capacity;
                if (positive < 1)
                    errors.AppendLine("Мощность скважины - положительное число выше нуля");
            }
            catch
            {
                errors.AppendLine("Мощность скважины - положительное число");
            }

            try
            {
                int positive = currentWell.GasOilDepth;
                if (positive < 1)
                    errors.AppendLine("Глубина добычи газа и нефти - положительное число выше нуля");
            }
            catch
            {
                errors.AppendLine("Глубина добычи газа и нефти - положительное число");
            }
            #endregion

            if (errors.Length > 0)
            {
                await DisplayAlert("Error!", errors.ToString(), "Ok");
                return;
            }

            // проверка на превышение глубины добычи нефти и газа
            foreach (var item in layers)
            {
                if (item.EndPoint > currentWell.GasOilDepth)
                {
                    errors.AppendLine("Не должно быть слоев, превышающих глубину добычи нефти и газа");
                    break;
                }
            }

            if (layers.FirstOrDefault().StartPoint != 0)
                errors.AppendLine("Начальная точка первого слоя должна равняться нуля");

            layers = layers.OrderBy(p => p.StartPoint).ToList();
            for (int i = 0; i < layers.Count() - 2; i++)
            {
                if (layers[i + 1].StartPoint - layers[i].EndPoint != 0)
                {
                    errors.AppendLine("Обнаружено пустое место между слоями!");
                    break;
                }
            }

            if (errors.Length > 0)
            {
                await DisplayAlert("Error!", errors.ToString(), "Ok");
                return;
            }

            Wells lastWell;
            try
            {
                if (currentWell.ID == 0)
                {
                    currentWell.WellTypeID = 1;
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var result = client.UploadString($"{address}Wells", JsonConvert.SerializeObject(currentWell));

                    lastWell = AppData.GetWells().OrderBy(p => p.ID).LastOrDefault();
                }
                else
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var result = client.UploadString($"{address}Wells/{currentWell.ID}", "PUT", JsonConvert.SerializeObject(currentWell));
                    lastWell = currentWell;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "Ok");
                return;
            }

            try
            {
                foreach (var item in layers)
                {
                    if (item.ID == 0)
                    {
                        item.WellID = lastWell.ID;
                        client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                        var result = client.UploadString($"{address}WellLayers", JsonConvert.SerializeObject(item));
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "Ok");
                return;
            }

            try
            {
                foreach (var item in layersForRemove)
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var result = client.UploadString($"{address}WellLayers/{item.ID}", "DELETE", JsonConvert.SerializeObject(item));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert ("Error!", ex.Message, "Ok");
                return;
            }
            
            

            await DisplayAlert("Success!", "Well успешно сохранен!", "Ok");
            await Navigation.PopAsync();
        }
    }
}