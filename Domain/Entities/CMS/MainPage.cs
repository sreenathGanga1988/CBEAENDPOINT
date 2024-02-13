using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class MainPage :AuditEntity
    {
        public MainPage()
        {
            NewsItems = new List<NewsItem>();

            CorouselImage1 = @"/images/c1.jpg";
            CorouselImage2 = @"/images/c2.jpg";
            CorouselImage3 = @"/images/c3.jpg";
        }


        public int Id { get; set; }
        public List<NewsItem> NewsItems { get; set; }


        public String CorouselImage1 { get; set; }
        public String CorouselImage2 { get; set; }
        public String CorouselImage3 { get; set; }
        public String MainText { get; set; }
        public String Slogan { get; set; }

        public String LogoImage1 { get; set; }

        public String LogoImage2 { get; set; }

        public String ContactDesc1 { get; set; }

        public String ContactDesc2 { get; set; }

        public String ContactLine1 { get; set; }

        public String ContactLine2 { get; set; }

        public String ContactLine3 { get; set; }

        public String Phonenum { get; set; }

        public String Faxnum { get; set; }


        public String Website { get; set; }
        public String Email { get; set; }

        public String RulesRegulation { get; set; }
        public String DayQuote { get; set; }

    }
}
