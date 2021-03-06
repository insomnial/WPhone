﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using CineQuest.XMLclasses;


namespace CineQuest
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }
        

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {

            // Sample data; replace with real data
            /*this.Items.Add(new ItemViewModel() { LineOne = "Chittagong", LineTwo = "12:00 PM - 1:30 PM", LineThree = "C12", LineFour = "Set in the turbulence of the 1930s British Bangladesh, Chittagong is a true story of a 14 year old boy, Jhunku, and of his journey to find where he belongs. For the first time in Indian history, the British army is defeated by a ragtag army of schoolboys and their teacher, Masterda. Called a traitor by his peers, and let down by a man he trusts, Jhunku impulsively joins the movement. As his world is turned upside down, Jhunku is forced to confront his self-doubts. As the leaders of the movement are progressively caught or killed, Jhunku battles against seemingly insurmountable odds to win a victory of his own. The film is an exciting action-drama, made more so by the fact that it is true." });
            this.Items.Add(new ItemViewModel() { LineOne = "Everybody in our Family", LineTwo = "1:30 PM - 3:00 PM", LineThree = "C12", LineFour = "Marius is a divorced man in his late thirties. His five year-old daughter Sofia lives with her mother, which causes Marius a deep frustration. On the day Marius arrives to take his daughter on their annual holiday, he is told that she is ill but he doesn't believe it and insists to take her with him. The situation soon gets out of control with all the family taking part in a web of humor, violence, childish songs, police interventions and love statements." });
            this.Items.Add(new ItemViewModel() { LineOne = "Out Cold", LineTwo = "3:00 PM - 4:30 PM", LineThree = "C12", LineFour = "A snowboarder's plans for his own snowboard park go awry when an ex-girlfriend returns to town." });
            */

            WebClient webclient = new WebClient();
            webclient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webclient_DownloadStringCompleted);
            webclient.DownloadStringAsync(new Uri("http://mobile.cinequest.org/mobileCQ.php?type=festival"));
        }

        private void webclient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs data)
        {
            if (data.Error != null)
            {
                MessageBox.Show("error");
            }
            Festival festival = null;
            XmlReader reader = null;
            FestivalParser parser = new FestivalParser();

            try
            {
                /*XmlSerializer serializer = new XmlSerializer(typeof(Festival));
                reader = XmlReader.Create(new StringReader(data.Result));
                MessageBox.Show(reader.ToString());
                
                object deserialization = serializer.Deserialize(reader);
                
                MessageBox.Show(deserialization.ToString());
                festival = (Festival)deserialization;*/

                parser = new FestivalParser();
                festival = parser.Parse(data.Result);

                FilmItemList list = new FilmItemList(festival);
                list.populateList();

                //FilmItemList listTest = new FilmItemList();     /* test data */
                foreach (FilmItem item in list.Itemlist)
                {
                    /* connect films in ItemList to UI elements */
                    this.Items.Add(new ItemViewModel() {
                        IVMid = item.id,
                        IVMtitle = item.title,
                        IVMdescription = item.description,
                        IVMtagline = item.tagline,
                        IVMgenre = item.genre,
                        IVMimageURL = item.imageURL,
                        IVMdirector = item.director,
                        IVMproducer = item.producer,
                        IVMcinematographer = item.cinematographer,
                        IVMeditor = item.editor,
                        IVMcast = item.cast,
                        IVMcountry = item.country,
                        IVMlanguage = item.language,
                        IVMfilminfo = item.filminfo,
                        IVMshowtimes = item.showtimes
                    });
                }
            }
            catch (Exception ex)
            {
                //if (ex.GetType == )
                {
                    MessageBox.Show(ex.GetType().ToString(), "XML Error", MessageBoxButton.OK);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

            }
            /* house keeping */
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    this.IsDataLoaded = true;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}