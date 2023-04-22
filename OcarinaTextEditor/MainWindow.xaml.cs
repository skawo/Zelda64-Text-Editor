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
        public MainWindow()
        {
            InitializeComponent();

            BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(TextboxType)).Cast<TextboxType>();
            BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>();

            textBoxMsg.TextChanged += TextBoxMsg_TextChanged;
            BoxTypeCombo.SelectionChanged += BoxTypeCombo_SelectionChanged;

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
            textBoxMsg.TextChanged -= TextBoxMsg_TextChanged;
            BoxTypeCombo.SelectionChanged -= BoxTypeCombo_SelectionChanged;

            SetMsgBackground(BoxTypeCombo.SelectedIndex);
            TextBoxMsg_TextChanged(null, null);

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

        private void NumUpNumBoxes_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TextBoxMsg_TextChanged(null, null);
        }

        private void TextBoxMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ViewModel view = (ViewModel)DataContext;

                if (textBoxMsg.Text == "")
                    return;

                Message mes = new Message(textBoxMsg.Text, (TextboxType)BoxTypeCombo.SelectedIndex);
                byte[] outD = mes.ConvertTextData(false).ToArray();


                ZeldaMessage.MessagePreview mp = new ZeldaMessage.MessagePreview((ZeldaMessage.Data.BoxType)BoxTypeCombo.SelectedIndex, outD);

                Bitmap bmp = new Bitmap(384, mp.MessageCount * 108);
                bmp.MakeTransparent();

                using (Graphics grfx = Graphics.FromImage(bmp))
                {
                    for (int i = 0; i < mp.MessageCount; i++)
                    {
                        Bitmap bmpTemp = mp.GetPreview(i, true, 1.5f);
                        grfx.DrawImage(bmpTemp, 0, bmpTemp.Height * i);
                    }
                }

                msgPreview.Source = BitmapToImageSource(bmp);
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

    }
}
