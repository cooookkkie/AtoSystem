using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdoNetWindow.SEAOVER.TwoLine
{
    /// <summary>
    /// Loading.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Loading : UserControl
    {
        
        public Loading()
        {
            InitializeComponent();
            DoubleAnimation doubleAnimation = new DoubleAnimation();

            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            doubleAnimation.From = 0;
            doubleAnimation.To = 360;

            RotateTransform rotateTransform = new RotateTransform();

            rotateTransform.CenterX += this.imgLoading.Width / 2;
            rotateTransform.CenterY += this.imgLoading.Height / 2;

            this.imgLoading.RenderTransform = rotateTransform;

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
        }
    }
}
