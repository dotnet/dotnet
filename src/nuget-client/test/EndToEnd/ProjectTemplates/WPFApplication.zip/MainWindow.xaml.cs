using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace $safeprojectname$
{
    /// <summary>
    /// Interaction logic for $safeitemrootname$.xaml
    /// </summary>
    public partial class $safeitemrootname$ : Window
    {
        public $safeitemrootname$()
        {
            InitializeComponent();
        }
    }
}
