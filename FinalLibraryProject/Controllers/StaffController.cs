using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalLibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalLibraryProject.Controllers
{
    public class StaffController : Controller
    {
        private readonly FinalLibraryDatabaseContext _context;

        public StaffController(FinalLibraryDatabaseContext context)
        {
            _context = context;

        }

        // GET: /<controller>/
        [Authorize]
        public IActionResult Index()
        {
            

            return View();
        }

        public IActionResult AddItem(string Title,
                                     string ISBN,
                                     string AuthorFirstName,
                                     string AuthorLastName,
                                     string Publisher,
                                     int YearPublished,
                                     string DetailDescription,
                                     double Cost,
                                     string LibrarySection,
                                     string ItemType,
                                     string Genre
                                     )
        {

            var item = new LibraryItem {
                Isbn = ISBN,
                Title = Title,
                AuthorFirstName = AuthorFirstName,
                AuthorLastName = AuthorLastName,
                Publisher = Publisher,
                YearPublished = YearPublished,
                DetailDescription = DetailDescription,
                Cost = 20.00,
                LibrarySection = LibrarySection,
                ItemType = EnumeratedTypes.GetItemTypeNumber(ItemType),
                Genre = EnumeratedTypes.GetGenreNumber(Genre),
                DateAddedToSystem = DateTime.Now,
                CurrentlyPresent = 1,
                CurrentlyReserved = 1
            };

            System.Diagnostics.Debug.WriteLine(item.AuthorFirstName);

            _context.Add(item);
            _context.SaveChanges();

            return RedirectToAction("Detail", "Search", new { id = item.ItemId });
        }

        [Authorize]
        public IActionResult CheckMember(int MemberID)
        {

            var currentMember = _context.LibraryMember.Include("Transactions").FirstOrDefault(memberTemp => memberTemp.LibraryId == MemberID);

            // sort for latest
            var transactions = _context.Transactions.Where(transactionTemp => transactionTemp.LibraryId == MemberID);

            transactions.OrderBy(transaction => transaction.TransactionTime);

            var lastTransaction = transactions.Last<Transactions>();


            string suspensionStatus = "Unknown";

            if (currentMember.CurrentlySuspended == 1)
            {
                suspensionStatus = "Not suspended";
            }

            if (currentMember.CurrentlySuspended == 2)
            {
                suspensionStatus = "Currently suspended";
            }

            var currentCheckout = _context.Checkouts.Include("Item").Where(checkoutTemp => ((checkoutTemp.LibraryId == MemberID) && (checkoutTemp.TimeReturned == null))).ToList();


            var model = new CheckMemberModel {
                LastName = currentMember.LastName,
                FirstName = currentMember.FirstName,
                CurrentlySuspended = suspensionStatus,
                CurrentAccountBalance = lastTransaction.CurrentAccountBalance,
                PastCheckoutsList = currentCheckout
            };


            return View(model);
        }

        [Authorize]
        public IActionResult MemberList()
        {
            var memberList = _context.LibraryMember.ToList();

            var currentModel = new MemberListModel
            {
                MembersList = memberList
            };


            return View(currentModel);
        }

        [Authorize]
        public IActionResult Reports(DateTime checkOutDate, DateTime fineDate, string ageFlag)
        {

            // Most frequent number
            // Generates list of all items in database checked out in last month
            ICollection<Checkouts> itemList = _context.Checkouts.Where(x => x.CheckoutTime >= checkOutDate).ToList();
            int[] itemIDList = new int[itemList.Count<Checkouts>()];

            for (int i = 0; i < itemList.Count<Checkouts>(); i++)
            {
                itemIDList[i] = itemList.ElementAt<Checkouts>(i).ItemId;

            }

            ConcurrentDictionary<int, int> checkoutCountList = new ConcurrentDictionary<int, int>();

            for (int i = 0; i < itemIDList.Length; i++)
            {
                int itemID = itemIDList[i];

                checkoutCountList.AddOrUpdate(itemID, 1, (id, checkoutCount) => checkoutCount + 1);
            }

            // Finding the top 3 most checked out books
            var arrayCountList = checkoutCountList.ToArray();
            var sortedDict = arrayCountList.OrderByDescending(entry => entry.Value)
                .Take(3)
                .ToDictionary(pair => pair.Key, pair => pair.Value);


            string[] BookTitles = new string[3];

            ICollection<LibraryItem> topThreeItems = _context.LibraryItem.Where(x => (x.ItemId == sortedDict.ElementAt(0).Key || x.ItemId == sortedDict.ElementAt(1).Key || x.ItemId == sortedDict.ElementAt(2).Key)).ToList();


            for (int i = 0; i < topThreeItems.Count(); i++)
            {
                BookTitles[i] = topThreeItems.ElementAt(i).Title;
            }



            // average age - all members, sum age, divide by number of members
            var allMembers = _context.LibraryMember;
            int ageSum = 0;

            if(ageFlag == "yes")
            {
                foreach (var currentMember in allMembers)
                {
                    ageSum += currentMember.Age;
                }
            }

            int averageAge = ageSum / allMembers.Count<LibraryMember>();


            // Get average fine for each member, divide by number of members
            List<double> memberFineAverages = new List<double>();

            foreach (var currentMember in allMembers)
            {
                var transactions = _context.Transactions.Where(x => ((x.LibraryId == currentMember.LibraryId) && (x.CurrentFine != 0) && (x.TransactionTime >= fineDate)));
                double totalFines = 0;

                foreach (var currentTransaction in transactions)
                {
                    totalFines += currentTransaction.CurrentFine;
                }

                double averageFineForMember = 0;

                if (transactions.Count<Transactions>() != 0)
                {
                    //averageFineForMember = totalFines / transactions.Count<Transactions>();
                    // Change - now total fines and not divided by count, only want to divide by number of members
                    averageFineForMember = totalFines;
                }



                memberFineAverages.Add(averageFineForMember);

            }



            double totalMemberFineSum = memberFineAverages.Sum();


            double totalMemberFineAverage = totalMemberFineSum / allMembers.Count<LibraryMember>();


            var model = new ReportModel
            {
                AgeAverage = averageAge,
                FineAverage = totalMemberFineAverage,
                BookTitles = BookTitles,
               CheckoutDate = checkOutDate,
               FineDate = fineDate
            };

            return View(model);
        }
    }
}
