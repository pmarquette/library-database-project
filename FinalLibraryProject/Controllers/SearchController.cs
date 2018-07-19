using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalLibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalLibraryProject.Controllers
{
    public class SearchController : Controller
    {
        private readonly FinalLibraryDatabaseContext _context;


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SearchController(FinalLibraryDatabaseContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;

            _userManager = userManager;
            _signInManager = signInManager;
        }

        SearchResultModel model = new SearchResultModel();

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            var item = _context.LibraryItem.FirstOrDefault(itemTemp => itemTemp.ItemId == id);

            var numCopies = _context.LibraryItem.Where(itemTemp => itemTemp.ItemId == id).ToList();
            int copiesNumber = numCopies.Count<LibraryItem>();

            var waitListCopies = _context.WaitList.Where(itemTemp => itemTemp.ItemId == id).ToList();
            int waitListNumber = waitListCopies.Count<WaitList>() + 1;

            var checkoutListCopies = _context.Checkouts.Where(itemTemp => ((itemTemp.ItemId == id) && (itemTemp.TimeReturned == null))).ToList();
            int checkoutNumber = checkoutListCopies.Count<Checkouts>();

            int copiesAvailable = copiesNumber - checkoutNumber;

            System.Diagnostics.Debug.WriteLine("Test");
            System.Diagnostics.Debug.WriteLine("Test");
            System.Diagnostics.Debug.WriteLine("Test");
            System.Diagnostics.Debug.WriteLine(copiesAvailable);
            System.Diagnostics.Debug.WriteLine(checkoutNumber);
            System.Diagnostics.Debug.WriteLine(copiesNumber);

            var model = new ItemDetailModel
            {
                ItemID = id,
                Title = item.Title,
                AuthorFirstName = item.AuthorFirstName,
                AuthorLastName = item.AuthorLastName,
                DetailDescription = item.DetailDescription,
                NumberAvailable = copiesAvailable, 
                NumberOfCopies = copiesNumber, 
                WaitListSize = waitListNumber,
                Publisher = item.Publisher,
                YearPublished = (int)item.YearPublished,
                LibrarySection = item.LibrarySection,
                Genre = EnumeratedTypes.GetGenreString((int)item.Genre)
            };

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            
            var editModel = _context.LibraryItem.FirstOrDefault(itemTemp => itemTemp.ItemId == id);

            return View(editModel);
        }

        public IActionResult CommitEdit([Bind("ItemId,Title,Isbn,AuthorFirstName,AuthorLastName,Publisher,YearPublished,DetailDescription,Cost,LibrarySection,ItemType,Genre,DateAddedToSystem")] LibraryItem currentItem)
        {

            _context.Update(currentItem);
            _context.SaveChanges();

            return RedirectToAction("Detail", "Search", new { id = currentItem.ItemId });
        }

        public IActionResult Delete(int id)
        {

            var itemToDelete = _context.LibraryItem.FirstOrDefault(itemTemp => itemTemp.ItemId == id);
            _context.LibraryItem.Remove(itemToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "Search");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Checkout(string ItemID)
        {

            int id = Int32.Parse(ItemID);

            Guid ownerIdGuid = Guid.Empty;
            String userId = _userManager.GetUserId(User);
            ownerIdGuid = new Guid(userId);
            ApplicationUser currentUser = _context.ApplicationUser.Include("Library").FirstOrDefault(x => x.Id == ownerIdGuid);


            var newCheckout = new Checkouts
            {
                ItemId = id,
                LibraryId = currentUser.Library.LibraryId,
                CheckoutTime = DateTime.Now,
                DueDate = (DateTime.Now).AddDays(14),
                LateFee = 0
            };


            var oldCheckout = _context.Checkouts.FirstOrDefault(x => ((x.ItemId == id) && (x.LibraryId == currentUser.Library.LibraryId) && (x.TimeReturned == null)));

            // This ensures they cannot checkout same book twice, can't checkout if suspended = 2
            if(oldCheckout == null && currentUser.Library.CurrentlySuspended == 1)
            {
                _context.Add(newCheckout);
                _context.SaveChanges();

            }


            return RedirectToAction("Index", "Account");
        }


        [HttpPost]
        [Authorize]
        public IActionResult WaitList(string ItemID)
        {

            int id = Int32.Parse(ItemID);

            Guid ownerIdGuid = Guid.Empty;
            String userId = _userManager.GetUserId(User);
            ownerIdGuid = new Guid(userId);
            ApplicationUser currentUser = _context.ApplicationUser.Include("Library").FirstOrDefault(x => x.Id == ownerIdGuid);

            

            var newWaitList = new WaitList
            {
                ItemId = id,
                LibraryId = currentUser.Library.LibraryId,
                TimeWaitListed = DateTime.Now
            };

            var currentlyOnWaitList = _context.WaitList.FirstOrDefault(x => ((x.ItemId == id) && (x.LibraryId == currentUser.Library.LibraryId)));
            var currentCheckout = _context.Checkouts.FirstOrDefault(x => ((x.ItemId == id) && (x.LibraryId == currentUser.Library.LibraryId) && (x.TimeReturned == null)));

            if(currentlyOnWaitList == null && currentCheckout == null)
            {
                _context.Add(newWaitList);
                _context.SaveChanges();
            }
            

            return RedirectToAction("Index", "Account");
        }



        public IActionResult SearchResult(string TitleString, string AuthorString, string YearPublished, string Publisher, string Genre, string ItemType)
        {
   
            if (TitleString == "test")
            {
                model.Items = _context.LibraryItem;

            }
            else
            {
                int YearPublishedNum = 0;

                if(YearPublished != null)
                {
                    YearPublishedNum = Int32.Parse(YearPublished);
                }

                
                
                if(Genre == "Genre" && ItemType == "Item Type")
                {
                    if (AuthorString == null)
                    {
                        model.Items = _context.LibraryItem.Where(itemTemp => ((itemTemp.Title == TitleString)
                                                                  || (itemTemp.AuthorFirstName == AuthorString)
                                                                  || (itemTemp.AuthorLastName == AuthorString)
                                                                  || (itemTemp.Publisher == Publisher)
                                                                  || (itemTemp.YearPublished == YearPublishedNum)))
                                                                  .ToList();
                    }

                    else
                    {
                        // Split up Author String into first and last names
                        string[] namesSplit = AuthorString.Split(null);

                        if (namesSplit.Count() == 1)
                        {
                            model.Items = _context.LibraryItem.Where(itemTemp => ((itemTemp.Title == TitleString)
                                                                      || (itemTemp.AuthorFirstName == namesSplit[0])
                                                                      || (itemTemp.AuthorLastName == namesSplit[0])
                                                                      || (itemTemp.Publisher == Publisher)
                                                                      || (itemTemp.YearPublished == YearPublishedNum)))
                                                                      .ToList();
                        }

                        if (namesSplit.Count() == 2)
                        {
                            model.Items = _context.LibraryItem.Where(itemTemp => ((itemTemp.Title == TitleString)
                                                                      || (itemTemp.AuthorFirstName == namesSplit[0])
                                                                      || (itemTemp.AuthorLastName == namesSplit[0])
                                                                      || (itemTemp.AuthorLastName == namesSplit[1])
                                                                      || (itemTemp.AuthorLastName == namesSplit[1])
                                                                      || (itemTemp.Publisher == Publisher)
                                                                      || (itemTemp.YearPublished == YearPublishedNum)))
                                                                      .ToList();
                        }

                        if (namesSplit.Count() > 2)
                        {

                            System.Diagnostics.Debug.WriteLine("Test");
                            System.Diagnostics.Debug.WriteLine("Test");
                            System.Diagnostics.Debug.WriteLine("Test");
                            System.Diagnostics.Debug.WriteLine(namesSplit[0]);
                            System.Diagnostics.Debug.WriteLine(namesSplit[1]);

                            model.Items = _context.LibraryItem.Where(itemTemp => ((itemTemp.Title == TitleString)
                                                                      || (itemTemp.AuthorFirstName == AuthorString)
                                                                      || (itemTemp.AuthorLastName == AuthorString)
                                                                      || (itemTemp.Publisher == Publisher)
                                                                      || (itemTemp.YearPublished == YearPublishedNum)))
                                                                      .ToList();
                        }
                    }
                }

                if (Genre != "Genre" && ItemType == "Item Type")
                {
                    model.Items = _context.LibraryItem.Where(itemTemp => ((itemTemp.Title == TitleString)
                                                                  || (itemTemp.AuthorFirstName == AuthorString)
                                                                  || (itemTemp.AuthorLastName == AuthorString)
                                                                  || (itemTemp.Publisher == Publisher)
                                                                  || (itemTemp.YearPublished == YearPublishedNum)
                                                                  || (itemTemp.Genre == EnumeratedTypes.GetGenreNumber(Genre))))
                                                                  .ToList();
                }

                if (Genre == "Genre" && ItemType != "Item Type")
                {
                    model.Items = _context.LibraryItem.Where(itemTemp => ((itemTemp.Title == TitleString)
                                                                  || (itemTemp.AuthorFirstName == AuthorString)
                                                                  || (itemTemp.AuthorLastName == AuthorString)
                                                                  || (itemTemp.Publisher == Publisher)
                                                                  || (itemTemp.YearPublished == YearPublishedNum)
                                                                  || (itemTemp.ItemType == EnumeratedTypes.GetItemTypeNumber(ItemType))))
                                                                  .ToList();
                }

                if (Genre != "Genre" && ItemType != "Item Type")
                {
                    model.Items = _context.LibraryItem.Where(itemTemp => ((itemTemp.Title == TitleString)
                                                                  || (itemTemp.AuthorFirstName == AuthorString)
                                                                  || (itemTemp.AuthorLastName == AuthorString)
                                                                  || (itemTemp.Publisher == Publisher)
                                                                  || (itemTemp.YearPublished == YearPublishedNum)
                                                                  || (itemTemp.Genre == EnumeratedTypes.GetGenreNumber(Genre))
                                                                  || (itemTemp.ItemType == EnumeratedTypes.GetItemTypeNumber(ItemType))))
                                                                  .ToList();
                }


            }


            // Get number of copies available

            ICollection<LibraryItem> itemList = _context.LibraryItem.ToList();
            int[] itemIDList = new int[itemList.Count<LibraryItem>()];

            for (int i = 0; i < itemList.Count<LibraryItem>(); i++)
            {
                itemIDList[i] = itemList.ElementAt<LibraryItem>(i).ItemId;

            }


            Dictionary<int, int> checkoutCountList = new Dictionary<int, int>();

            for (int i = 0; i < itemIDList.Length; i++)
            {
                int itemID = itemIDList[i];

                var numCopies = _context.LibraryItem.Where(itemTemp => itemTemp.ItemId == itemIDList[i]).ToList();
                int copiesNumber = numCopies.Count<LibraryItem>();

                var checkoutListCopies = _context.Checkouts.Where(itemTemp => ((itemTemp.ItemId == itemIDList[i]) && (itemTemp.TimeReturned == null))).ToList();
                int checkoutNumber = checkoutListCopies.Count<Checkouts>();

                int copiesAvailable = copiesNumber - checkoutNumber;

                checkoutCountList.Add(itemIDList[i], copiesAvailable);
            }

            model.copiesCountList = checkoutCountList;

            return View(model);
        }
    }
}
