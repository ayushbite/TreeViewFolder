using System;
using System.Collections.Generic;
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

namespace treeviewlearningangelsix
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
        //when windows first loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var drive in Directory.GetLogicalDrives())//c drive or edrive
            {
                var item = new TreeViewItem();
                item.Header = drive; //setting treeview item header content to drive name and binding it in ui
                item.Tag = drive;//it is used to store the custom information about the now using it for path

                item.Items.Add(null); //adding a dummy item


                //listenting out for item is expanded
                item.Expanded += FolderExpanded;
                treeviewname.Items.Add(item); //adding all the treeviewitems to the treeview

            }

        }

        private void FolderExpanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            //if the item only containing the dummy data

        if(item.Items.Count != 1 && item.Items[0] != null) //cause we added only the one null item initailly
            {
                return;
            }
        item.Items.Clear();
            //getting the full path
            var fullpath = (string)item.Tag;
            var directories = new List<string>();
            try
            {
                    var directorypath = Directory.GetFiles(fullpath);
                    if (directorypath.Length > 0)
                    {
                    directories.AddRange(directorypath);
                    }

                
            }
            catch
            {

            }
            directories.ForEach(d => {
                //for each directory
                //adding all the subdirectories to the item
                var subitem = new TreeViewItem();
                subitem.Header = getfilefoldername(d); //were only getting the path
                //setting tag as full path
                subitem.Tag = d;
                //addign dummy items to get the empty list
                subitem.Items.Add(null);
                //handle expanded and does it recusrively
                subitem.Expanded += FolderExpanded;
                //adding the subitems to the item
                item.Items.Add(subitem);

            });

        }

        private static string getfilefoldername(string d)
        {
            if(string.IsNullOrEmpty(d)) // if we have no path then return empty
            {
                return String.Empty;
            }
            //if theres any forward slash convert it ro backslash
            var normalizedpath = d.Replace('/', '\\');
            //finding the last backslash item
            var lastindex = normalizedpath.LastIndexOf('\\');
            //if we do not find the backslash , return the path itself
            if(lastindex <= 0)
            {
                return d;
            }
            //return name after the last backslash
            return d.Substring(lastindex + 1);
        }
    }
}
