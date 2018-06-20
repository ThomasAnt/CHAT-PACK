using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string _tag;
        private BitmapImage _img;
        //EEEE
        //only one messageContainer
        private string _messageContainer;
        private int _amountSent = 0;
        private int _amountReceive = 0;
        private DateTime _friendsSince;
        private double _currMessageAmount = 0;
        private ObservableValue observeValueMessage;


        public User(string name, string tag)
        {
            observeValueMessage = new ObservableValue(0);
            _name = name;
            _tag = tag;

            string dir = GetDirectory();
            _img = new BitmapImage(new Uri(dir + @"\ProfilePicture\default.png"));
        }


        public User(string name, string tag, string img) : this(name, tag)
        {
            _name = name;
            _tag = tag;

            string dir = GetDirectory();
            _img = new BitmapImage(new Uri(dir + @"\ProfilePicture\" + img));
        }
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
        public string Tag
        {
            get
            {
                return _tag;
            }

            set
            {
                _tag = value;
            }
        }
        public double CurrMessageAmount
        {
            get
            {
                return _currMessageAmount;
            }

            set
            {
                _currMessageAmount = value;
            }
        }
        public int AmountSent
        {
            get
            {
                return _amountSent;
            }
            set
            {
                _amountSent = value;
            }
        }
        public int AmountReceive
        {
            get
            {
                return _amountReceive;
            }
            set
            {
                _amountReceive = value;
            }
        }

        public ObservableValue ObserveValueMessage
        {
            get
            {
                return observeValueMessage;
            }

            set
            {
                observeValueMessage = value;
            }
        }

        public string MessageContainer
        {
            get
            {
                return _messageContainer;
            }

            set
            {
                _messageContainer = value;
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

        public int GetTotalMessages()
        {
            int total;
            total = AmountReceive + AmountSent;

            return total;
        }
        private string GetDirectory()
        {
            string path = Directory.GetCurrentDirectory();
            path = Directory.GetParent(path).ToString();
            path = Directory.GetParent(path).ToString();

            return path;
        }

    }
}
