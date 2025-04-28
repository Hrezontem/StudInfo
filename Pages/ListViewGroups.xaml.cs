using System.Linq;
using System.Windows.Controls;


namespace StudentSystem.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListViewGroup.xaml
    /// </summary>
    public partial class ListViewGroup : Page
    {
        public ListViewGroup()
        {
            InitializeComponent();
            DataContext = new GroupsViewModel();
        }
    }
}
