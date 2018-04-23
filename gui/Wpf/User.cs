using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wpf
{
    class User : IComparable<User>
    {
        private string _name;
        private BitmapImage _img;
        private string _message = "";

        public User(string name)
        {
            _name = name;
            _img = new BitmapImage(new Uri(@"C:\Schule\3Klasse\syp\repositories\chatpack-wpf\Wpf\ProfilePicture\default.png"));
        }
        public User(string name, string img):this(name)
        {
            _name = name;
            _img = new BitmapImage(new Uri(@"C:\Schule\3Klasse\syp\repositories\chatpack-wpf\Wpf\ProfilePicture\" + img));
        }//C:\Users\Stephan\Desktop\lsad\Wpf\ProfilePicture

        //public void AddMessage


        #region Prop
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public BitmapImage Img
        {
            get
            {
                return _img;
            }

            set
            {
                _img = value;
            }
        }
        #endregion

        public int CompareTo(User other)
        {
            return this.Name.CompareTo(other.Name);
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
