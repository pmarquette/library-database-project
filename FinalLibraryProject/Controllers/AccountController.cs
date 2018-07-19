using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalLibraryProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalLibraryProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly FinalLibraryDatabaseContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(FinalLibraryDatabaseContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;

            _userManager = userManager;
            _signInManager = signInManager;
        }

        LibraryMember model = new LibraryMember();

        // GET: /<controller>/
        [Authorize]
        public IActionResult Index()
        {
            Guid ownerIdGuid = Guid.Empty;
            String userId = _userManager.GetUserId(User);
            ownerIdGuid = new Guid(userId);
            ApplicationUser currentUser = _context.ApplicationUser.Include("Library").FirstOrDefault(x => x.Id == ownerIdGuid);


            var member = currentUser.Library;

            var checkout = _context.Checkouts.Include("Item").Where(checkoutTemp => ((checkoutTemp.LibraryId == member.LibraryId) && (checkoutTemp.TimeReturned == null))).ToList();
            var pastCheckout = _context.Checkouts.Include("Item").Where(checkoutTemp => ((checkoutTemp.LibraryId == member.LibraryId) && (checkoutTemp.TimeReturned != null))).ToList();
            var waitList = _context.WaitList.Include("Item").Where(waitListTemp => waitListTemp.LibraryId == member.LibraryId).ToList();

            //need to sort and get latest one
            var transactions = _context.Transactions.Where(transactionTemp => transactionTemp.LibraryId == member.LibraryId);

            transactions.OrderBy(transaction => transaction.TransactionTime);

            var lastTransaction = new Transactions
            {
                CurrentAccountBalance = 0
            };

            if(transactions.Count() > 0)
            {
                lastTransaction = transactions.Last<Transactions>();
            }


            // for waitlist position
            for (int i = 0; i < waitList.Count(); i++)
            {
                // Gets list of all wait list requests for that specific item that the user has wait listed
                var itemWaitList = _context.WaitList.Where(waitListTemp => waitListTemp.ItemId == waitList.ElementAt(i).ItemId).ToList();
                // Sort list
                List<WaitList> SortedList = itemWaitList.OrderBy(o => o.TimeWaitListed).ToList();
                // Get position in sorted list of current user
                int itemIndex = SortedList.IndexOf(itemWaitList.Where(p => p.LibraryId == member.LibraryId).FirstOrDefault());

            }

            var model = new AccountPageModel
            {
                LibraryId = member.LibraryId,
                FirstName = member.FirstName,
                LastName = member.LastName,
                CheckoutsList = checkout,
                WaitListList = waitList,
                PastCheckoutsList = pastCheckout,
                CurrentAccountBalance = ((transactions == null) ? 0 : lastTransaction.CurrentAccountBalance)
            };

            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            await _signInManager.PasswordSignInAsync(Email, Password, true, lockoutOnFailure: false);


            return RedirectToAction("Index", "Search");
        }

        public IActionResult LoginPage()
        {
            return View();
        }


        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Search");
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string FirstName, 
                                                  string LastName, 
                                                  string PhoneNumber,
                                                  string MemberType,
                                                  int Age,
                                                  string StreetAddress,
                                                  string ZipCode,
                                                  string City,
                                                  string State,
                                                  string Email, 
                                                  string Password
                                                  )
        {


            var member = new LibraryMember {
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
                Age = Age,
                MemberType = EnumeratedTypes.GetMemberTypeNumber(MemberType),
                StreetAddress = StreetAddress,
                ZipCode = ZipCode,
                CityAddress = City,
                StateAddress = State,
                Email = Email,
                CurrentlySuspended = 1,
                PinNumber = 1000,
                MaxCheckoutAmount = 50,
                TimeAccountCreated = DateTime.Now
            };

            _context.Add(member);

            var user = new ApplicationUser { UserName = Email, Email = Email, Library = member };
            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Account");
            }

            return View(model);
        }

        [Authorize]
        public IActionResult Payment(int PaymentAmount)
        {
            Guid ownerIdGuid = Guid.Empty;
            String userId = _userManager.GetUserId(User);
            ownerIdGuid = new Guid(userId);
            ApplicationUser currentUser = _context.ApplicationUser.Include("Library").FirstOrDefault(x => x.Id == ownerIdGuid);

            var member = currentUser.Library;

            //sort for latest transaction
            var transactions = _context.Transactions.Where(transactionTemp => transactionTemp.LibraryId == member.LibraryId);

            transactions.OrderBy(transaction => transaction.TransactionTime);

            var lastTransaction = transactions.Last<Transactions>();


            var currentTransaction = new Transactions {
                LibraryId = member.LibraryId,
                PreviousAccountBalance = (lastTransaction == null) ? 0 : lastTransaction.CurrentAccountBalance,
                CurrentFine = 0,
                CurrentPayment = (double)PaymentAmount,
                TransactionTime = DateTime.Now
            };

            if(currentTransaction.PreviousAccountBalance - PaymentAmount >= 0)
            {
                _context.Add(currentTransaction);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Account");
        }


        [Authorize]
        public IActionResult ReturnItem(int ItemID)
        {
            Guid ownerIdGuid = Guid.Empty;
            String userId = _userManager.GetUserId(User);
            ownerIdGuid = new Guid(userId);
            ApplicationUser currentUser = _context.ApplicationUser.Include("Library").FirstOrDefault(x => x.Id == ownerIdGuid);

            // Find item in checkouts and update return date
            var returnItem = _context.Checkouts.FirstOrDefault(checkoutTemp => ((checkoutTemp.LibraryId == currentUser.Library.LibraryId) && (checkoutTemp.ItemId == ItemID) && (checkoutTemp.TimeReturned == null)));

            returnItem.TimeReturned = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction("Index", "Account");
        }


    }
}
