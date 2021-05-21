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
using System.Text.RegularExpressions;

namespace LanguageSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        List<Service> ServiceList1 = Classes.Base.Ent.Service.ToList();
        List<Service> ServiceList = new List<Service>(); //отрисовка
        List<ClientService> ClentServiceList = Classes.Base.Ent.ClientService.ToList();

        string Path;
        public Admin()
        {
            InitializeComponent();

            BitmapImage BMI = new BitmapImage();
            BMI.BeginInit();
            Path = @"\Resources\school_logo.png";
            BMI.UriSource = new Uri(Path, UriKind.RelativeOrAbsolute);
            BMI.EndInit();
            Logo.Source = BMI;
            Logo.Stretch = Stretch.UniformToFill;

            ServiceList = ServiceList1;

            DGServices.ItemsSource = ServiceList;
            ComboBoxHuman.ItemsSource = Classes.Base.Ent.Client.ToList();
            ComboBoxHuman.SelectedValuePath = "ID";
            ComboBoxHuman.DisplayMemberPath = "Human";
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
        /// Вывод данных из БД в textbox, при нажатии кнопки Изменить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BEdit_Click(object sender, RoutedEventArgs e)
        {
            SVGrid.Visibility = Visibility.Collapsed;
            StackChange.Visibility = Visibility.Visible;
            StackAdd.Visibility = Visibility.Collapsed;
            StackNewNote.Visibility = Visibility.Collapsed;
            ForButtons.Visibility = Visibility.Collapsed;

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
        /// Удаление записей из приложения
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

            DialogResult DR = (DialogResult)MessageBox.Show("Следующая запись будет удалена. Удалить запись?", "Внимание", (MessageBoxButton)MessageBoxButtons.YesNo);
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


        /// <summary>
        /// Метод для загрузки изображений 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Изменение записей 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWrite_Click(object sender, RoutedEventArgs e)
        {
            

            try
            {
                double discount = Convert.ToDouble(BoxDiscount.Text) / 100;
                int time = Convert.ToInt32(BoxTime.Text) * 60;

                DialogResult DR = (DialogResult)MessageBox.Show("Следующая запись будет изменена. Изменить запись?", "Внимание", (MessageBoxButton)MessageBoxButtons.YesNo);
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
            catch
            {
                MessageBox.Show("Ошибка ввода");
            }
        }

        /// <summary>
        /// Добавление записей 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            StackAdd.Visibility = Visibility.Visible;
            StackChange.Visibility = Visibility.Collapsed;
            SVGrid.Visibility = Visibility.Collapsed;
            StackNewNote.Visibility = Visibility.Collapsed;
            ForButtons.Visibility = Visibility.Collapsed;
        }

        public string AddPathPhoto;
        private void BtnAddImg_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog OFD = new System.Windows.Forms.OpenFileDialog();
            OFD.ShowDialog();
            string Path = OFD.FileName;
            if (Path != null)
            {
                int c = Path.IndexOf('У');
                string New = Path.Substring(c);
                AddPathPhoto = New.ToString();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Service S = ServiceList[i];
            try
            {
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
                        Service obj = new Service() { Title = BoxNewHeader.Text, Cost = Convert.ToInt32(BoxNewCost.Text), DurationInSeconds = time, Description = BoxNewDescription.Text, Discount = discount, MainImagePath = AddPathPhoto };
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
            catch
            {
                MessageBox.Show("Ошибка ввода");
            }

        }


        /// <summary>
        /// Добавление новой записи на программу обучения 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BNew_Initialized(object sender, EventArgs e)
        {
            Button BtnNote = (Button)sender;
            if (BtnNote != null)
            {
                BtnNote.Uid = Convert.ToString(i);
            }
        }

        private void BNew_Click(object sender, RoutedEventArgs e)
        {
            StackAdd.Visibility = Visibility.Collapsed;
            StackChange.Visibility = Visibility.Collapsed;
            SVGrid.Visibility = Visibility.Collapsed;
            ForButtons.Visibility = Visibility.Collapsed;
            StackNewNote.Visibility = Visibility.Visible;

            Button BtnEdit = (Button)sender;
            int ind = Convert.ToInt32(BtnEdit.Uid);
            Service S = ServiceList[ind];

            BlockServiceName.Text = "Название курса: " + Convert.ToString(S.Title);
            BlockServiceTime.Text = "Время занятий: " + Convert.ToString((S.DurationInSeconds) / 60) + "мин";
        }

        DateTime DT;
        private void TimeOfNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Regex r1 = new Regex("[0-1][0-9]:[0-5][0-9]");
                Regex r2 = new Regex("2[0-3]:[0-5][0-9]");
                string s = "";
                if ((r1.IsMatch(TimeOfNote.Text) || r2.IsMatch(TimeOfNote.Text)) && TimeOfNote.Text.Length == 5)
                {
                    //MessageBox.Show(TimeOfNote.Text);
                    TimeSpan TS = TimeSpan.Parse(TimeOfNote.Text);
                    DT = Convert.ToDateTime(DateOfNote.SelectedDate);
                    DT = DT.Add(TS);
                    if (DT > DateTime.Now)
                    {
                        MessageBox.Show(DT + "");
                    }
                    else
                    {
                        MessageBox.Show("Запись не может быть в прошедешм времени");
                        BtnNote.IsEnabled = false;
                    }
                }
                else
                {
                    if (TimeOfNote.Text.Length >= 5)
                    {
                        MessageBox.Show("Время указано неверно");
                        BtnNote.IsEnabled = false;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка ввода");
            }
        }

        private void BtnNote_Click(object sender, RoutedEventArgs e)
        {
            Service S = ServiceList[i];
            int indexCombo = (int)ComboBoxHuman.SelectedValue;

            try
            {
                DialogResult DR = (DialogResult)MessageBox.Show("Следующая запись будет добавлена. Добавить запись?", "Внимание", (MessageBoxButton)MessageBoxButtons.YesNo);
                if (DR == DialogResult.Yes)
                {
                    ClientService obj = new ClientService() { ClientID = indexCombo, ServiceID = S.ID, StartTime = DT };
                    Classes.Base.Ent.ClientService.Add(obj);
                    Classes.Base.Ent.SaveChanges();
                    MessageBox.Show("Изменения сохранены");
                }
                else if (DR == DialogResult.No)
                {
                    MessageBox.Show("Изменения не были сохранены");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка ввода");
            }
        }


        /// <summary>
        /// Сортировака
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void BtnSortUp_Click(object sender, RoutedEventArgs e)
        {
            i = -1;
            ServiceList.Sort((x,y) => x.Cost.CompareTo(y.Cost));
            DGServices.Items.Refresh();
        }

        private void BtnSortDown_Click(object sender, RoutedEventArgs e)
        {
            i = -1;
            ServiceList.Sort((x, y) => x.Cost.CompareTo(y.Cost));
            ServiceList.Reverse();
            DGServices.Items.Refresh();
        }


       

        List<Service> ServiceListFilter = new List<Service>();

        /// <summary>
        /// фильтрация по скидке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboDiscout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            i = -1;
            switch (ComboDiscout.SelectedIndex)
            {
                case 0:
                    ServiceListFilter = ServiceList1.Where(x => x.Discount <= .05).ToList();
                    ServiceList = ServiceListFilter;
                    DGServices.ItemsSource = ServiceList;
                    break;
                case 1:
                    ServiceListFilter = ServiceList1.Where(x => (x.Discount >= .05) && (x.Discount < .15)).ToList();
                    ServiceList = ServiceListFilter;
                    DGServices.ItemsSource = ServiceList;
                    break;
                case 2:
                    ServiceListFilter = ServiceList1.Where(x => (x.Discount >= .15) && (x.Discount < .3)).ToList();
                    ServiceList = ServiceListFilter;
                    DGServices.ItemsSource = ServiceList;
                    break;
                case 3:
                    ServiceListFilter = ServiceList1.Where(x => (x.Discount >= .3) && (x.Discount < .7)).ToList();
                    ServiceList = ServiceListFilter;
                    DGServices.ItemsSource = ServiceList;
                    break;
                case 4:
                    ServiceListFilter = ServiceList1.Where(x => (x.Discount >= .7) && (x.Discount < 1)).ToList();
                    ServiceList = ServiceListFilter;
                    DGServices.ItemsSource = ServiceList;
                    break;
                case 5:
                    ServiceListFilter = ServiceList1.Where(x => x.Discount <= 1).ToList();
                    ServiceList = ServiceListFilter;
                    DGServices.ItemsSource = ServiceList;
                    break;
            }
            AllNotes.Text = Convert.ToString("Общее количество записей: " + ServiceList1.Count);
            FilterNotes.Text = Convert.ToString("Отфильтрованные записи: " + ServiceList.Count);
        }



        /// <summary>
        /// Поиск по названию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoxForSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            i = -1;
            if (BoxForSearch.Text != "")
            {
                List<Service> ServiceListSerach = new List<Service>();
                ServiceListSerach = ServiceList.Where(x => x.Title.Contains(BoxForSearch.Text)).ToList();
                ServiceList = ServiceListSerach;
                DGServices.ItemsSource = ServiceList;

                AllNotes.Text = Convert.ToString("Общее количество записей: " + ServiceList1.Count);
                FilterNotes.Text = Convert.ToString("Отфильтрованные записи: " + ServiceList.Count);
            }
            else
            {
                if (ServiceListFilter.Count == 0)
                {
                    ServiceList = ServiceList1;
                    DGServices.ItemsSource = ServiceList;
                }
                else
                {
                    ServiceList = ServiceListFilter;
                    DGServices.ItemsSource = ServiceList;
                }
            }
        }


        /// <summary>
        /// вторая кнопка назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack2_Click(object sender, RoutedEventArgs e)
        {
            Classes.Global.GlobalFrame.Navigate(new Admin());
        }

        private void BtnBack3_Click(object sender, RoutedEventArgs e)
        {
            Classes.Global.GlobalFrame.Navigate(new Admin());
        }
    }
}
