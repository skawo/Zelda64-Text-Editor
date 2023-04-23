using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using OcarinaTextEditor.Enums;

namespace OcarinaTextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsMajoraMode = false;

        public MainWindow()
        {
            InitializeComponent();

            BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(TextboxType)).Cast<TextboxType>();
            BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>().Where(x => x <= TextboxPosition.Bottom);
            MajoraIconCombo.ItemsSource = Enum.GetValues(typeof(MajoraIcons)).Cast<MajoraIcons>();
            MajoraIconCombo.Visibility = Visibility.Hidden;
            MajoraIconLbl.Visibility = Visibility.Hidden;

            textBoxMsg.TextChanged += TextBoxMsg_TextChanged;
            BoxTypeCombo.SelectionChanged += BoxTypeCombo_SelectionChanged;

            foreach (var Icon in Enum.GetValues(typeof(Enums.MsgIcon)))
            {
                var Item = new MenuItem();
                Item.Header = Icon.ToString();
                Item.SetBinding(Button.CommandProperty, new Binding("OnRequestAddControl"));
                Item.CommandParameter = $"ICON:{Icon.ToString()}";

                IconHeader.Items.Add(Item);
            }

        }


        BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();


                return bitmapimage;
            }
        }

        private void SetMsgBackground(int Type)
        {
            if (Type == 4)
                dockMsgPreview.Background = System.Windows.Media.Brushes.Black;
            else
                dockMsgPreview.Background = System.Windows.Media.Brushes.White;
        }

        private void BoxTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BoxTypeCombo.SelectedItem == null)
                return;

            ViewModel view = (ViewModel)DataContext;

            if (IsMajoraMode)
                view.SelectedMessage.MajoraBoxType = (MajoraTextboxType)BoxTypeCombo.SelectedItem;
            else
                view.SelectedMessage.BoxType = (TextboxType)BoxTypeCombo.SelectedItem;

            textBoxMsg.TextChanged -= TextBoxMsg_TextChanged;
            BoxTypeCombo.SelectionChanged -= BoxTypeCombo_SelectionChanged;

            SetMsgBackground(BoxTypeCombo.SelectedIndex);
            TextBoxMsg_TextChanged(null, null);

            textBoxMsg.TextChanged += TextBoxMsg_TextChanged;
            BoxTypeCombo.SelectionChanged += BoxTypeCombo_SelectionChanged;
        }

        private void TextBoxMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ViewModel view = (ViewModel)DataContext;

                if (textBoxMsg.Text == "")
                    return;

                if (ROMInfo.IsMajoraMask(view.Version))
                {
                    if (!IsMajoraMode)
                    {
                        IsMajoraMode = true;
                        BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(MajoraTextboxType)).Cast<MajoraTextboxType>();
                    }

                    Message mes = view.SelectedMessage;
                    byte[] outD = mes.ConvertTextData(view.Version, false).ToArray();

                    ZeldaMessage.MessagePreviewMajora mp = new ZeldaMessage.MessagePreviewMajora(outD);
                    Bitmap bmpTemp = mp.GetPreview(0, true, 1.5f);

                    Bitmap bmp = new Bitmap(bmpTemp.Width, mp.MessageCount * bmpTemp.Height);
                    bmp.MakeTransparent();

                    using (Graphics grfx = Graphics.FromImage(bmp))
                    {
                        grfx.DrawImage(bmpTemp, 0, 0);

                        for (int i = 1; i < mp.MessageCount; i++)
                        {
                            bmpTemp = mp.GetPreview(i, true, 1.5f);
                            grfx.DrawImage(bmpTemp, 0, bmpTemp.Height * i);
                        }
                    }

                    msgPreview.Source = BitmapToImageSource(bmp);
                }
                else
                {
                    if (IsMajoraMode)
                    {
                        IsMajoraMode = false;
                        BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(TextboxType)).Cast<TextboxType>();
                    }

                    Message mes = new Message(textBoxMsg.Text, (TextboxType)BoxTypeCombo.SelectedIndex);
                    byte[] outD = mes.ConvertTextData(view.Version, false).ToArray();


                    ZeldaMessage.MessagePreview mp = new ZeldaMessage.MessagePreview((ZeldaMessage.Data.BoxType)BoxTypeCombo.SelectedIndex, outD);
                    Bitmap bmpTemp = mp.GetPreview(0, true, 1.5f);

                    Bitmap bmp = new Bitmap(bmpTemp.Width, mp.MessageCount * bmpTemp.Height);
                    bmp.MakeTransparent();

                    using (Graphics grfx = Graphics.FromImage(bmp))
                    {
                        grfx.DrawImage(bmpTemp, 0, 0);

                        for (int i = 1; i < mp.MessageCount; i++)
                        {
                            bmpTemp = mp.GetPreview(i, true, 1.5f);
                            grfx.DrawImage(bmpTemp, 0, bmpTemp.Height * i);
                        }
                    }

                    msgPreview.Source = BitmapToImageSource(bmp);
                }   
            }
            catch (Exception)
            {

            }

        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;
            TextBox box = sender as TextBox;
            view.TextboxPosition = box.SelectionStart;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;

            bool MajoraMode = ROMInfo.IsMajoraMask(view.Version);

            if (view.SelectedMessage != null)
            {
                if (MajoraMode)
                {
                    if (!IsMajoraMode)
                    {
                        IsMajoraMode = true;
                        BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(MajoraTextboxType)).Cast<MajoraTextboxType>();
                        BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>();
                        MajoraIconCombo.Visibility = Visibility.Visible;
                        MajoraIconLbl.Visibility = Visibility.Visible;
                    }

                    BoxTypeCombo.SelectedItem = view.SelectedMessage.MajoraBoxType;
                    MajoraIconCombo.SelectedItem = (MajoraIcons)view.SelectedMessage.MajoraIcon;


                }
                else if (!MajoraMode)
                {
                    if (IsMajoraMode)
                    {
                        IsMajoraMode = false;
                        BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(TextboxType)).Cast<TextboxType>();
                        BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>().Where(x => x <= TextboxPosition.Top);
                        MajoraIconCombo.Visibility = Visibility.Hidden;
                        MajoraIconLbl.Visibility = Visibility.Hidden;
                    }

                    BoxTypeCombo.SelectedItem = view.SelectedMessage.BoxType;
                }
            }
        }

        private void MajoraIconCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;

            view.SelectedMessage.MajoraIcon = (byte)(MajoraIcons)MajoraIconCombo.SelectedItem;
            TextBoxMsg_TextChanged(null, null);
        }
    }
}
