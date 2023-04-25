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

            textBoxMsg.TextChanged += TextBoxMsg_TextChanged;
            BoxTypeCombo.SelectionChanged += BoxTypeCombo_SelectionChanged;

            DockTextBoxOptions.Height = 95;
            textBoxMsgDock.Margin = new Thickness(0, 118, 0, 10);

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

            if (view.MajoraMaskMode)
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

        private void Majora_RenderPreview()
        {
            ViewModel view = (ViewModel)DataContext;

            Message mes = view.SelectedMessage;
            byte[] outD = mes.ConvertTextData(view.Version, view.CreditsMode, false).ToArray();

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

            msgPreview.Dispatcher.Invoke(() =>
            {
                msgPreview.Source = BitmapToImageSource(bmp);
            });
        }

        private void Ocarina_RenderPreview()
        {
            ViewModel view = (ViewModel)DataContext;

            Message mes = new Message(textBoxMsg.Text, (TextboxType)BoxTypeCombo.SelectedIndex);
            byte[] outD = mes.ConvertTextData(view.Version, view.CreditsMode, false).ToArray();

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

            msgPreview.Dispatcher.Invoke(() => 
            {
                msgPreview.Source = BitmapToImageSource(bmp);
            });
        }

        private void TextBoxMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                msgPreview.Opacity = 1;
                ViewModel view = (ViewModel)DataContext;

                if (view.SelectedMessage == null)
                    return;

                if (view.MajoraMaskMode)
                    Majora_RenderPreview();
                else
                    Ocarina_RenderPreview();
            }
            catch (Exception ex)
            {
                msgPreview.Opacity = 0.5;
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

            if (view.SelectedMessage != null)
            {
                if (view.MajoraMaskMode)
                {
                    if (!IsMajoraMode)
                    {
                        IsMajoraMode = true;
                        BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(MajoraTextboxType)).Cast<MajoraTextboxType>();
                        BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>();
                        DockTextBoxOptions.Height = 215;
                        textBoxMsgDock.Margin = new Thickness(0, 241, 0, 10);
                    }

                    BoxTypeCombo.SelectedItem = view.SelectedMessage.MajoraBoxType;
                    MajoraIconCombo.SelectedItem = (MajoraIcons)view.SelectedMessage.MajoraIcon;

                    MajoraJumpToTextBox.TextChanged -= MajoraJumpToTextBox_TextChanged;
                    MajoraFirstPriceTextBox.TextChanged -= MajoraFirstPriceTextBox_TextChanged;
                    MajoraSecondPriceTextBox.TextChanged -= MajoraSecondPriceTextBox_TextChanged;

                    MajoraJumpToTextBox.Background = System.Windows.Media.Brushes.White;
                    MajoraFirstPriceTextBox.Background = System.Windows.Media.Brushes.White;
                    MajoraSecondPriceTextBox.Background = System.Windows.Media.Brushes.White;

                    MajoraJumpToTextBox.Text = "0x" + Convert.ToString((ushort)view.SelectedMessage.MajoraNextMessage, 16).ToUpper();
                    MajoraFirstPriceTextBox.Text = Convert.ToString(view.SelectedMessage.MajoraFirstItemPrice);
                    MajoraSecondPriceTextBox.Text = Convert.ToString(view.SelectedMessage.MajoraSecondItemPrice);

                    MajoraJumpToTextBox.TextChanged += MajoraJumpToTextBox_TextChanged;
                    MajoraFirstPriceTextBox.TextChanged += MajoraFirstPriceTextBox_TextChanged;
                    MajoraSecondPriceTextBox.TextChanged += MajoraSecondPriceTextBox_TextChanged;
                }
                else if (!view.MajoraMaskMode)
                {
                    if (IsMajoraMode)
                    {
                        IsMajoraMode = false;
                        BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(TextboxType)).Cast<TextboxType>();
                        BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>().Where(x => x <= TextboxPosition.Bottom);
                        DockTextBoxOptions.Height = 95;
                        textBoxMsgDock.Margin = new Thickness(0, 118, 0, 10);
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

        private void MajoraJumpToTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;

            try
            {
                view.SelectedMessage.MajoraNextMessage = Convert.ToInt16(MajoraJumpToTextBox.Text.TrimStart(new char[] { '0', 'x' }), 16);
                MajoraJumpToTextBox.Background = System.Windows.Media.Brushes.White;
            }
            catch (Exception)
            {
                view.SelectedMessage.MajoraNextMessage = -1;
                MajoraJumpToTextBox.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void MajoraFirstPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;

            try
            {
                view.SelectedMessage.MajoraFirstItemPrice = Convert.ToInt16(MajoraFirstPriceTextBox.Text);
                MajoraFirstPriceTextBox.Background = System.Windows.Media.Brushes.White;
            }
            catch (Exception)
            {
                view.SelectedMessage.MajoraFirstItemPrice = -1;
                MajoraFirstPriceTextBox.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void MajoraSecondPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;

            try
            {
                view.SelectedMessage.MajoraSecondItemPrice = Convert.ToInt16(MajoraSecondPriceTextBox.Text);
                MajoraSecondPriceTextBox.Background = System.Windows.Media.Brushes.White;
            }
            catch (Exception)
            {
                view.SelectedMessage.MajoraFirstItemPrice = -1;
                MajoraSecondPriceTextBox.Background = System.Windows.Media.Brushes.Red;
            }
        }
    }
}
