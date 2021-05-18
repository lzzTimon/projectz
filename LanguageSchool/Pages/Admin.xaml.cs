using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LanguageSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        List<Service> ServiceList = Classes.Base.Ent.Service.ToList();
        public Admin()
        {
            InitializeComponent();
            DGServices.ItemsSource = ServiceList;
        }

        int i = -1;

        private void MediaElement_Initialized(object sender, EventArgs e)
        {
            if (i < ServiceList.Count)
            {
                i++;
                MediaElement ME = (MediaElement)sender;
                Service S = ServiceList[i];
                Uri U = new Uri(S.MainImagePath, UriKind.RelativeOrAbsolute);
                ME.Source = U;
            }
        }

        private void TextBlock_Initialized(object sender, EventArgs e)
        {
            if (i < ServiceList.Count)
            {
                TextBlock TB = (TextBlock)sender;
                Service S = ServiceList[i];
                TB.Text = S.Title;
            }
        }

        private void BEdit_Initialized(object sender, EventArgs e)
        {
            Button BtnEdit = (Button)sender;
            if (BtnEdit != null)
            {
                BtnEdit.Uid = Convert.ToString(i);
            }
        }

        private void BEdit_Click(object sender, RoutedEventArgs e)
        {
            Button BtnEdit = (Button)sender;
            int ind = Convert.ToInt32(BtnEdit.Uid);
            Service S = ServiceList[ind];
            MessageBox.Show(S.Title);

        }

        private void StackPanel_Initialized(object sender, EventArgs e)
        {
            if (i < ServiceList.Count)
            {
                StackPanel SP = (StackPanel)sender;
                Service S = ServiceList[i];
                if (S.Discount != 0)
                {
                    SP.Background = new SolidColorBrush(Color.FromRgb(231, 250, 191));
                }
            }

        }

        private void TextBlock_Initialized_1(object sender, EventArgs e)
        {
            if (i < ServiceList.Count)
            {
                TextBlock TB = (TextBlock)sender;
                Service S = ServiceList[i];
                double el = Convert.ToDouble(S.Discount);
                if (S.Discount > 0)
                {
                    TB.Text = Convert.ToString(Convert.ToInt32(S.Cost) - (Convert.ToInt32(S.Cost) * Convert.ToDouble(S.Discount))+ " рублей за " + S.DurationInSeconds/60 + " мин");
                }
                if (S.Discount == 0)
                {
                    TB.Text = Convert.ToString(Convert.ToInt32(S.Cost) + " рублей за " + S.DurationInSeconds / 60 + " мин");
                }
            }
        }
        private void TextBlock_Initialized_4(object sender, EventArgs e)
        {
            TextBlock TB = (TextBlock)sender;
            Service S = ServiceList[i];
            double el = Convert.ToDouble(S.Discount);
            if (S.Discount > 0)
            {
                TB.Text = "* скидка " + el * 100 + "%";
            }
        }

        private void TextBlock_Initialized_2(object sender, EventArgs e)
        {
            TextBlock TB = (TextBlock)sender;
            Service S = ServiceList[i];

            if (S.Discount > 0)
            {
                TB.Text = Convert.ToInt32(S.Cost) + " ";
            }
        }
    }
}
