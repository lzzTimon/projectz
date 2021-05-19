using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
using System.Windows.Forms;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;


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

        /// <summary>
        /// Вывод данных из БД в textbox, при нажатии кнопки изменить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BEdit_Click(object sender, RoutedEventArgs e)
        {
            SVGrid.Visibility = Visibility.Collapsed;
            StackChange.Visibility = Visibility.Visible;
            StackAdd.Visibility = Visibility.Collapsed;

            Button BtnEdit = (Button)sender;
            int ind = Convert.ToInt32(BtnEdit.Uid);
            Service S = ServiceList[ind];

            TextID.Text = Convert.ToString(S.ID);
            BoxTitle.Text = Convert.ToString(S.Title);
            BoxCost.Text = Convert.ToInt32(S.Cost)+"";
            BoxTime.Text = Convert.ToInt32(S.DurationInSeconds/60)+"";
            BoxDescription.Text = Convert.ToString(S.Description);
            BoxDiscount.Text = Convert.ToDouble(S.Discount*100)+"";
            ForPath.Text = Convert.ToString(S.MainImagePath);

        }

        /// <summary>
        /// Выделение ячеек со скидкой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Вывод данных в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Удаление записей 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BDelete_Initialized(object sender, EventArgs e)
        {
            Button BtnDel = (Button)sender;
            if (BtnDel != null)
            {
                BtnDel.Uid = Convert.ToString(i);
            }
        }

        private void BDelete_Click(object sender, RoutedEventArgs e)
        {
            Button BtnDel = (Button)sender;
            int ind = Convert.ToInt32(BtnDel.Uid);
            Service S = ServiceList[ind];

            DialogResult DR = (DialogResult)MessageBox.Show("Внимание!", "Следующая запись будет удалена. Удалить запись?", (MessageBoxButton)MessageBoxButtons.YesNo);
            if (DR == DialogResult.Yes)
            {
                Classes.Base.Ent.Service.Remove(S);
                MessageBox.Show("Запись удалена");
                Classes.Base.Ent.SaveChanges();
                Classes.Global.GlobalFrame.Navigate(new Admin());
            }
            else if (DR == DialogResult.No)
            {
                MessageBox.Show("Изменения не внесены");
            }
            
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Classes.Global.GlobalFrame.Navigate(new Admin());
        }

        private void BtnImg_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog OFD = new System.Windows.Forms.OpenFileDialog();
            OFD.ShowDialog();
            string Path = OFD.FileName;
            if (Path != null)
            {
                int c = Path.IndexOf('У');
                string New = Path.Substring(c);
                ForPath.Text = New.ToString();
            }
        }

        private void BtnWrite_Click(object sender, RoutedEventArgs e)
        {
            double discount = Convert.ToDouble(BoxDiscount.Text)/100;
            int time = Convert.ToInt32(BoxTime.Text)*60;

            DialogResult DR = (DialogResult)MessageBox.Show("Следующая запись будет изменена. Изменить запись?", "Внимание",  (MessageBoxButton)MessageBoxButtons.YesNo);
            if (DR == DialogResult.Yes)
            {
                Service obj = new Service() { ID = Convert.ToInt32(TextID.Text), Title = BoxTitle.Text, Cost = Convert.ToInt32(BoxCost.Text), DurationInSeconds = time, Description = BoxDescription.Text, Discount = discount, MainImagePath = ForPath.Text };
                Classes.Base.Ent.Service.AddOrUpdate(obj);
                Classes.Base.Ent.SaveChanges();
                MessageBox.Show("Изменения сохранены");
            }
            else if (DR == DialogResult.No)
            {
                MessageBox.Show("Изменения не были сохранены");
            }
        }

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            StackAdd.Visibility = Visibility.Visible;
            StackChange.Visibility = Visibility.Collapsed;
            SVGrid.Visibility = Visibility.Collapsed;
        }

        private void BtnAddImg_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog OFD = new System.Windows.Forms.OpenFileDialog();
            OFD.ShowDialog();
            string Path = OFD.FileName;
            if (Path != null)
            {
                int c = Path.IndexOf('У');
                string New = Path.Substring(c);
                ForPath.Text = New.ToString();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Service S = ServiceList[i];
            if (S.Title == BoxNewHeader.Text)
            {
                MessageBox.Show("Данное имя уже существует в системе");
            }
            if (Convert.ToInt32(BoxNewTime.Text) > 14400 || Convert.ToInt32(BoxNewTime.Text) < 0)
            {
                MessageBox.Show("Время не может привышать 4 часов или быть отрицательным");
            }
            else if (Convert.ToInt32(BoxNewTime.Text) < 14400 || Convert.ToInt32(BoxNewTime.Text) > 0)
            {
                double discount = Convert.ToDouble(BoxNewDiscount.Text) / 100;
                int time = Convert.ToInt32(BoxNewTime.Text) * 60;

                DialogResult DR = (DialogResult)MessageBox.Show("Следующая запись будет добавлена. Добавить запись?", "Внимание", (MessageBoxButton)MessageBoxButtons.YesNo);
                if (DR == DialogResult.Yes)
                {
                    Service obj = new Service() { Title = BoxNewHeader.Text, Cost = Convert.ToInt32(BoxNewCost.Text), DurationInSeconds = time, Description = BoxNewDescription.Text, Discount = discount, MainImagePath = AddPath.Text };
                    Classes.Base.Ent.Service.Add(obj);
                    Classes.Base.Ent.SaveChanges();
                    MessageBox.Show("Запись добавлена");
                }
                else if (DR == DialogResult.No)
                {
                    MessageBox.Show("Запись не была добавлена");
                }

            }
        }
    }
}
