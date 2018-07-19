using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLibraryProject.Models
{
    public class ReportModel
    {
        //list of books, double for fines, int for age

        public string[] BookTitles { get; set; }
        public double FineAverage { get; set; }
        public int AgeAverage { get; set; }

        public int FrequentTime { get; set; }
        public int FineTime { get; set; }

        public DateTime CheckoutDate { get; set; }
        public DateTime FineDate { get; set; }
    }
}
