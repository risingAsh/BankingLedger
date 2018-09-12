using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using BankingLedger.Models;
using System.Web.UI.WebControls;
using System.Data;

namespace BankingLedger.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        private bool HasPassword(ApplicationUserManager manager)
        {
            return manager.HasPassword(User.Identity.GetUserId());
        }

        public bool HasPhoneNumber { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public bool TwoFactorBrowserRemembered { get; private set; }

        public int LoginsCount { get; set; }

        protected void Page_Load()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            HasPhoneNumber = String.IsNullOrEmpty(manager.GetPhoneNumber(User.Identity.GetUserId()));

            // Enable this after setting up two-factor authentientication
            //PhoneNumber.Text = manager.GetPhoneNumber(User.Identity.GetUserId()) ?? String.Empty;

            TwoFactorEnabled = manager.GetTwoFactorEnabled(User.Identity.GetUserId());

            LoginsCount = manager.GetLogins(User.Identity.GetUserId()).Count;

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            if (Session["bankLedger"] != null)
            {
                GridView1.DataSource = (DataTable)Session["bankLedger"];
                GridView1.DataBind();
            }
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // Remove phonenumber from user
        protected void RemovePhone_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var result = manager.SetPhoneNumber(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return;
            }
            var user = manager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                Response.Redirect("/Account/Manage?m=RemovePhoneNumberSuccess");
            }
        }

        // DisableTwoFactorAuthentication
        protected void TwoFactorDisable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), false);

            Response.Redirect("/Account/Manage");
        }

        //EnableTwoFactorAuthentication 
        protected void TwoFactorEnable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), true);

            Response.Redirect("/Account/Manage");
        }

        protected void DepositButton_Click(object sender, EventArgs e)
        {
            double dep;
            if (double.TryParse(DepositTextbox.Text, out dep)) //If the input is a double
            {
                DataTable table;
                //Create a new row for the banking ledger table
                if (Session["bankLedger"] == null) //if there is no transaction data yet
                {
                    table = new DataTable();
                    table.Columns.Add("Date", typeof(string));
                    table.Columns.Add("Deposit", typeof(double));
                    table.Columns.Add("Withdrawal", typeof(double));
                    table.Columns.Add("Balance", typeof(double));

                    //Add deposit information
                    table.Rows.Add(DepositDate.Text, dep, 0.00, dep);

                    //Save table to local cache
                    Session["bankLedger"] = table;

                    //Save balance for easy access
                    Session["balance"] = dep;
                }
                else
                {
                    table = (DataTable)Session["bankLedger"];

                    //Update balance
                    Session["balance"] = (double)Session["balance"] + dep;

                    //Add deposit information
                    table.Rows.Add(DepositDate.Text, dep, 0.00, (double)Session["balance"]);

                    //Save table to local cache
                    Session["bankLedger"] = table;
                }
                //Update GridView
                GridView1.DataSource = table;
                GridView1.DataBind();
                DepositSuccess.Text = "Deposit was successful!";
            }
            else
            {
                DepositSuccess.Text = "Invalid value. Deposit value must be of the form: $.¢¢";
            }
        }

        protected void WithdrawButton_Click(object sender, EventArgs e)
        {
            double withdraw;
            if (double.TryParse(WithdrawTextbox.Text, out withdraw)) //If the input is a double
            {
                DataTable table;
                //Create a new row for the banking ledger table
                if (Session["bankLedger"] == null) //if there is no transaction data yet
                {
                    table = new DataTable();
                    table.Columns.Add("Date", typeof(string));
                    table.Columns.Add("Deposit", typeof(double));
                    table.Columns.Add("Withdrawal", typeof(double));
                    table.Columns.Add("Balance", typeof(double));

                    //Add deposit information
                    table.Rows.Add(WithdrawDate.Text, 0.00, withdraw, withdraw * -1);

                    //Save table to local cache
                    Session["bankLedger"] = table;

                    //Save balance for easy access
                    Session["balance"] = withdraw * -1;
                }
                else
                {
                    table = (DataTable)Session["bankLedger"];

                    //Update balance
                    Session["balance"] = (double)Session["balance"] - withdraw;

                    //Add deposit information
                    table.Rows.Add(WithdrawDate.Text, 0.00, withdraw, (double)Session["balance"]);

                    //Save table to local cache
                    Session["bankLedger"] = table;
                }
                //Update GridView
                GridView1.DataSource = table;
                GridView1.DataBind();
                WithdrawSuccess.Text = "Withdrawal was successful!";
            }
            else
            {
                WithdrawSuccess.Text = "Invalid value. Withdrawal value must be of the form: $.¢¢";
            }
        }
    }
}