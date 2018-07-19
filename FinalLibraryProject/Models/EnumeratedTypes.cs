using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLibraryProject.Models
{
    public class EnumeratedTypes
    {
        public static string GetGenreString(int genreNumber)
        {
            switch(genreNumber)
            {
                case 1:
                    return "Literary fiction";
                case 2:
                    return "Non-fiction";
                case 3:
                    return "Horror";
                case 4:
                    return "Mystery";
                case 5:
                    return "Romance";
                case 6:
                    return "Sci-fi";
                case 7:
                    return "Young Adult";
                case 8:
                    return "Thriller";
                case 9:
                    return "Children's";

            }

            return "Not found";
        }

        public static int GetGenreNumber(string genreString)
        {
            switch (genreString)
            {
                case "Literary fiction":
                    return 1;
                case "Non-fiction":
                    return 2;
                case "Horror":
                    return 3;
                case "Mystery":
                    return 4;
                case "Romance":
                    return 5;
                case "Sci-fi":
                    return 6;
                case "Young Adult":
                    return 7;
                case "Thriller":
                    return 8;
                case "Children's":
                    return 9;

            }
            return 0;
        }

        public static string GetMemberTypeString(int memberTypeNumber)
        {
            switch (memberTypeNumber)
            {
                case 1:
                    return "Student";
                case 2:
                    return "Faculty";
                case 3:
                    return "Staff";
            }

            return "Unknown";
        }



        public static int GetMemberTypeNumber(string memberTypeString)
        {
            switch (memberTypeString)
            {
                case "Student":
                    return 1;
                case "Faculty":
                    return 2;
                case "Staff":
                    return 3;
            }

            return 0;
        }


        public static string GetItemTypeString(int itemTypeNumber)
        {
            switch (itemTypeNumber)
            {
                case 1:
                    return "Book";
                case 2:
                    return "Audiobook";
                case 3:
                    return "Video";
                case 4:
                    return "Magazine";
            }

            return "Unknown";
        }

        public static int GetItemTypeNumber(string itemTypeString)
        {
            switch (itemTypeString)
            {
                case "Book":
                    return 1;
                case "Audiobook":
                    return 2;
                case "Video":
                    return 3;
                case "Magazine":
                    return 4;
            }

            return 0;
        }
    }
}
