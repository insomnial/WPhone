﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace CineQuest
{
    //used by FilmItemList to populate the film page
    public class FilmItem
    {
        Films films = new Films();

        public String id { get; set; }
        public String title { get; set; }
        public String description { get; set; }
        public String tagline { get; set; }
        public String genre { get; set; }
        public String imageURL { get; set; }
        public String director { get; set; }
        public String producer { get; set; }
        public String cinematographer { get; set; }
        public String editor { get; set; }
        public String cast { get; set; }
        public String country { get; set; }
        public String language { get; set; }
        public String filminfo { get; set; }
        public List<String> showtimes { get; set; }

        public FilmItem()
        {
            
        }
    }
}
