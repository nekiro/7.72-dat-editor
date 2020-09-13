using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
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

namespace _7._72_dat_editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateNewItem(object sender, RoutedEventArgs e)
        {
            DatManager.AddNewObject();
            UpdateUI();
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("This software was made by Nekiro. Thanks for using!", "About");
        }

        private void LoadDat(object sender, RoutedEventArgs e)
        {
            DatManager.Items.Clear();

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Dat File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "dat",
                Filter = "dat files (*.dat)|*.dat",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == true)
            {
                if (DatManager.ParseDat(openFileDialog1.FileName))
                {
                    UpdateUI();
                }
                else
                {
                    MessageBox.Show("Dat failed to load.", "Fail");
                }
            }
        }

        private void SaveDat(object sender, RoutedEventArgs e)
        {
            if (DatManager.Items.Count < 1)
            {
                MessageBox.Show("Nothing to save");
                return;
            }

            var saveFileDialog = new VistaSaveFileDialog
            {
                Title = "Save dat file",
                DefaultExt = ".dat",
                CheckFileExists = false,
                RestoreDirectory = true,
                Filter = "dat files (*.dat)|*.dat",
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                if (DatManager.SaveDat(saveFileDialog.FileName))
                {
                    MessageBox.Show("Dat saved.", "Success");
                }
                else
                {
                    MessageBox.Show("Dat failed to save.", "Fail");
                }
            }
        }

        private void UpdateUI()
        {
            listBox.Items.Clear();
            Flags.Visibility = Visibility.Hidden;
            listBox.SelectedIndex = -1;
            foreach (var item in DatManager.Items)
            {
                listBox.Items.Add(item.id);
            }
        }

        private void SelectItem(object sender, SelectionChangedEventArgs e)
        {
            // update flags
            if (listBox.SelectedIndex == -1)
            {
                return;
            }

            var item = DatManager.Items.ElementAt(listBox.SelectedIndex);

            Ground.IsChecked = item.ground;
            GroundSpeed.Text = item.groundSpeed.ToString();
            Clip.IsChecked = item.clip;
            Topp.IsChecked = item.top;
            Container.IsChecked = item.container;
            Stackable.IsChecked = item.stackable;
            ForceUse.IsChecked = item.corpse;
            MultiUse.IsChecked = item.usable;
            Writable.IsChecked = item.writable;
            WritableLen.Text = item.writableLenght.ToString();
            Readable.IsChecked = item.readable;
            ReadableLen.Text = item.readableLenght.ToString();
            FluidContainer.IsChecked = item.fluid;
            Splash.IsChecked = item.splash;
            Blocking.IsChecked = item.blocking;
            Movable.IsChecked = item.immovable;
            BlocksMissile.IsChecked = item.blocksMissile;
            BlocksPath.IsChecked = item.blocksPath;
            Pickupable.IsChecked = item.pickupable;
            Hangable.IsChecked = item.hangable;
            Horizontal.IsChecked = item.horizontal;
            Vertical.IsChecked = item.vertical;
            Rotable.IsChecked = item.rotatable;
            HasLight.IsChecked = item.hasLight;
            LightLevel.Text = item.lightLevel.ToString();
            LightColor.Text = item.lightColor.ToString();
            DontHide.IsChecked = item.dontHide;
            FloorChange.IsChecked = item.floorchange;
            Offset.IsChecked = item.hasOffset;
            OffsetX.Text = item.offsetX.ToString();
            OffsetY.Text = item.offsetY.ToString();
            Elevation.IsChecked = item.hasHeight;
            Height.Text = item.height.ToString();
            Layer.IsChecked = item.layer;
            IdleAnimated.IsChecked = item.idleAnimated;
            Minimap.IsChecked = item.minimap;
            MinimapColor.Text = item.minimapColor.ToString();
            Actions.IsChecked = item.actioned;
            Action.Text = item.actions.ToString();
            FullGround.IsChecked = item.groundItem;

            Flags.Visibility = Visibility.Visible;
        }

        private void SpellCheck(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            if (txtBox == null)
            {
                return;
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(txtBox.Text, "[^0-9]"))
            {
                txtBox.Text = txtBox.Text.Remove(txtBox.Text.Length - 1);
            }
            else
            {
                if (!ushort.TryParse(txtBox.Text, out ushort val))
                {
                    txtBox.Text = "";
                }
            }

            if (listBox.SelectedIndex == -1)
            {
                return;
            }

            ushort temp;
            var item = DatManager.Items.ElementAt(listBox.SelectedIndex);
            if (txtBox.Name == "GroundSpeed")
            {
                if (ushort.TryParse(txtBox.Text, out temp))
                {
                    item.groundSpeed = temp;
                }
            }
            else if (txtBox.Name == "ReadableLen")
            {
                if (ushort.TryParse(txtBox.Text, out temp))
                {
                    item.readableLenght = temp;
                }
            }
            else if (txtBox.Name == "WritableLen")
            {
                if (ushort.TryParse(txtBox.Text, out temp))
                {
                    item.writableLenght = temp;
                }
            }
            else if (txtBox.Name == "LightLevel")
            {
                if (ushort.TryParse(txtBox.Text, out temp))
                {
                    item.lightLevel = temp;
                }
            }
            else if (txtBox.Name == "LightColor")
            {
                if (ushort.TryParse(txtBox.Text, out temp))
                {
                    item.lightColor = temp;
                }
            }
            else if (txtBox.Name == "OffsetX")
            {
                if (ushort.TryParse(txtBox.Text, out temp))
                {
                    item.offsetX = temp;
                }
            }
            else if (txtBox.Name == "OffsetY")
            {
                if (ushort.TryParse(txtBox.Text, out temp))
                {
                    item.offsetY = temp;
                }
            }
            else if (txtBox.Name == "Height")
            {
                if (ushort.TryParse(txtBox.Text, out temp))
                {
                    item.height = temp;
                }
            }
            else if (txtBox.Name == "MinimapColor")
            {
                if (ushort.TryParse(txtBox.Text, out temp))
                {
                    item.minimapColor = temp;
                }
            }
            else if (txtBox.Name == "Action")
            {
                if (ushort.TryParse(txtBox.Text, out temp))
                {
                    item.actions = temp;
                }
            }
            else
            {
                MessageBox.Show(txtBox.Name);
            }
        }

        private void ToggleFlag(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null)
            {
                return;
            }

            if (listBox.SelectedIndex == -1)
            {
                return;
            }

            DatManager.Item item = DatManager.Items.ElementAt(listBox.SelectedIndex);
            if (checkBox.Name == "Ground")
            {
                item.ground = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Clipp")
            {
                item.clip = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Topp")
            {
                item.top = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Container")
            {
                item.container = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Stackable")
            {
                item.stackable = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "ForceUse")
            {
                item.corpse = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "MultiUse")
            {
                item.usable = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Writable")
            {
                item.writable = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Readable")
            {
                item.readable = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "FluidContainer")
            {
                item.fluid = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Splash")
            {
                item.splash = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Blocking")
            {
                item.blocking = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Movable")
            {
                item.immovable = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "BlocksMissile")
            {
                item.blocksMissile = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "BlocksPath")
            {
                item.blocksPath = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Pickupable")
            {
                item.pickupable = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Hangable")
            {
                item.hangable = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Horizontal")
            {
                item.horizontal = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Vertical")
            {
                item.vertical = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Rotatable")
            {
                item.rotatable = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "HasLight")
            {
                item.hasLight = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "DontHide")
            {
                item.dontHide = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "FloorChange")
            {
                item.floorchange = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Offset")
            {
                item.hasOffset = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Elevation")
            {
                item.hasHeight = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Layer")
            {
                item.layer = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "IdleAnimated")
            {
                item.idleAnimated = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Minimap")
            {
                item.minimap = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "Actions")
            {
                item.actioned = checkBox.IsChecked.Value;
            }
            else if (checkBox.Name == "FullGround")
            {
                item.groundItem = checkBox.IsChecked.Value;
            }
            else
            {
                MessageBox.Show(checkBox.Name);
            }
        }
    }
}
