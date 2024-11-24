using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBEAPI.Models
{
    public class UserLoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class MainPageModel
    {

        public MainPageModel()
        {
            newsModels = new List<NewsModel>();
            managingComiteeDTOs = new List<ManagingComiteeDTO>();

            CorouselImage1 = @"/images/CC1.JPEG";
            CorouselImage2 = @"/images/CC2.JPEG";
            CorouselImage3 = @"/images/c3.jpg";
        }



        public List<NewsModel> newsModels { get; set; }
        public List<ManagingComiteeDTO> managingComiteeDTOs { get; set; }


        public String CorouselImage1 { get; set; }
        public String CorouselImage2 { get; set; }
        public String CorouselImage3 { get; set; }
        public String MainText { get; set; }
        public String Slogan { get; set; }

        public String RulesRegulation { get; set; }
        public String DayQuote { get; set; }

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


    }

    public class NewsModel
    {
        public int NewsID { get; set; }
        public DateTime DateofAction { get; set; }

        public String NewsText { get; set; }

        public String NewsLink { get; set; }
    }
}
