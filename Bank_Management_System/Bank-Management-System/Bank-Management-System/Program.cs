using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Security;
using System.Data.OleDb;
using System.Reflection.PortableExecutable;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace Bank_Management_System
{ 
  
    class Program
    {
        static OleDbConnection con;
        static OleDbCommand cmd;
        static OleDbDataReader reader;
        static String input;
        static decimal balance = 0;
        static string accountId;
        static int amounttodeposit = 0;
        static int amounttowithdraw = 0;
        static string user;
        static string pass = "";
        static string enterText;
        static string NPIN = "";
        static string EnterNewPassword;
        static int number = 0;

        public static void Main()
        {
            Console.Clear();
            Console.WriteLine("WELCOME TO BANK MANAGEMENT SYSTEM\n");
            Console.WriteLine("1. User");
            Console.WriteLine("2. Admin");
            Console.WriteLine("3. Exit");
            Console.Write("\nSelect your option:");
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();
                    User_menu();
                    break;
                case "2":
                    Console.Clear();
                    Login_admin();
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("\n\nThank you for banking with us!");
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Invalid Choice");
                    Main();
                    break;
            }
        }
        public static void User_menu()
        {
            Console.Clear();
            Console.WriteLine("USER ACCOUNT\n");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Log in Account");
            Console.WriteLine("3. Exit");
            Console.Write("\nSelect your option:");
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();
                    Create_Account();
                    break;
                case "2":
                    Console.Clear();
                    Login_Account();
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("\n\nThank you for banking with us!");
                    Console.ReadLine();
                    Main();
                    break;
                default:
                    Console.WriteLine("Invalid Choice");
                    Console.ReadLine();
                    User_menu();
                    break;
            }
        }
        public static void Create_Account()
        {
            string fname;
            string lname;
            var random = new Random();
            

            Console.Write("USER ACCOUNT REGISTRAION\n\n");
            Console.Write("Enter your First Name : ");
            fname = Console.ReadLine();
            Console.Write("Enter your Last Name : ");
            lname = Console.ReadLine();
            enterText = "Account PIN: ";
            CheckPassword(enterText);
            if (pass.ToString().Length != 6)
            {
                Console.WriteLine("Input 6 Digits Only!");
                Console.ReadLine();
                User_menu();
            }
            else if (!int.TryParse(pass, out number))
            {
                Console.WriteLine("Input Digits Only!");
                Console.ReadLine();
                User_menu();
            }
            accountId = new string(Enumerable.Repeat("0123456789", 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
            Console.WriteLine("\n\nPLEASE DO NOT FORGET YOUR ACCOUNT ID");
            Console.WriteLine($"\nAccount ID: {accountId}");
            Console.WriteLine("\n\nPress enter to continue...");
            Console.ReadLine();

            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
            cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO Accounts (AccountID,FirstName,LastName,PIN) VALUES ('" + accountId + "','" + fname + "','" + lname + "','" + pass + "')";
            
            con.Open();
            int sonuc = cmd.ExecuteNonQuery();
            con.Close();

            User_menu();
        }
        public static void Login_Account()
        {
            Console.Clear();
            Console.Write("LOGIN USER ACCOUNT\n\n");
            Console.WriteLine("Account ID:");
            user = Console.ReadLine();

            if(user.ToString().Length != 6)
            {
                Console.WriteLine("Input 6 Digits Only!");
                Console.ReadLine();
                User_menu();
            }
            else if (!int.TryParse(user, out number))
            {
                Console.WriteLine("Input Digits Only!");
                Console.ReadLine();
                User_menu();
            }


            enterText = "Account PIN: \n";
            CheckPassword(enterText);


            if (pass.ToString().Length != 6)
            {
                Console.WriteLine("Input 6 Digits Only!");
                Console.ReadLine();
                User_menu();
            }
            else if (!int.TryParse(pass, out number))
            {
                Console.WriteLine("Input Digits Only!");
                Console.ReadLine();
                User_menu();
            }
            Validation();
          
        }
        public static void Account()
        {
            Console.Clear();

            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
            cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM Accounts WHERE AccountID=" + user + "AND PIN=" + pass + "";
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Hello " + reader[1] + " " + reader[2] + ", welcome to the Bank Management System!\n\n");

            }
            con.Close();

            Console.WriteLine("CHOOSE SERVICES YOU WANT\n\n");
            Console.WriteLine("1. View Balance");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Change Password");
            Console.WriteLine("5. Exit");
            Console.Write("\nSelect your option:");
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();
                    View_balance();
                    Console.WriteLine("\n\nPress enter to continue...");
                    Console.ReadLine();
                    Account();
                    break;

                case "2":
                    Console.Clear();
                    deposit();
                    break;

                case "3":
                    Console.Clear();
                    withdraw();
                    break;
                case "4":
                    Console.Clear();
                    Change_password();
                    break;
                case "5":
                    Console.Clear();
                    Console.WriteLine("\n\nThank you for banking with us!");
                    Console.ReadLine();
                    balance = 0;
                    User_menu();
                    break;
                default:
                    Console.WriteLine("Invalid Choice");
                    Console.ReadLine();
                    Account();
                    break;
            }
        }
        public static void deposit()
        {
            Console.Clear();
            Console.WriteLine("USER ACCOUNT DEPOSIT\n\n");
            Console.WriteLine("Enter amount to deposit:");
            amounttodeposit = Convert.ToInt32(Console.ReadLine());

            if(amounttodeposit % 100 != 0)
            {
                Console.WriteLine("Please enter the amount in multiples of 100");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Account();
            }
            else if(amounttodeposit <= 0)
            {
                Console.WriteLine("Invalid amount!");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Account();
            }
            else
            {
                balance = balance+amounttodeposit;
               
                con = new OleDbConnection();
                con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
                cmd = new OleDbCommand();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE Accounts SET Balance='" + balance + "' WHERE PIN=" + pass;

                con.Open();
                int sonuc = cmd.ExecuteNonQuery();
                con.Close();
                if (sonuc > 0)
                {
                    Console.WriteLine($"\n\nDepositing {amounttodeposit} to your account...");
                    Console.WriteLine("\n\nPress enter to continue...");
                    Console.ReadLine();
                    Dreceipt();

                }
                else
                {
                    Console.WriteLine("There are errors. Depositing Failed!");
                }

            }
        }
        public static void withdraw()
        {
            Console.Clear();
            Console.WriteLine("USER ACCOUNT DEPOSIT\n\n");
            Console.WriteLine("Enter amount to withdraw:");
            amounttowithdraw = Convert.ToInt32(Console.ReadLine());

            if (amounttowithdraw % 100 != 0)
            {
                Console.WriteLine("Please enter the amount in multiples of 100");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Account();
            }
            else if (amounttowithdraw <= 0)
            {
                Console.WriteLine("Invalid amount!");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Account();
            }
            else if (amounttowithdraw > balance)
            {
                Console.WriteLine("Sorry, you don't have insufficient balance");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Account();
            }
            else
            {
                balance = balance - amounttowithdraw;

                con = new OleDbConnection();
                con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
                cmd = new OleDbCommand();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE Accounts SET Balance='" + balance + "' WHERE PIN=" + pass;

                con.Open();
                int sonuc = cmd.ExecuteNonQuery();
                con.Close();
                if (sonuc > 0)
                {
                    Console.WriteLine($"\n\nWithdrawing {amounttowithdraw} to your account...");
                    Console.WriteLine("\n\nPress enter to continue...");
                    Console.ReadLine();
                    Wreceipt();

                }
                else
                {
                    Console.WriteLine("There are errors. Depositing Failed!");
                }

               
            }

        }
        public static void Change_password()
        {

            Console.Clear();
            Console.WriteLine("USER ACCOUNT CHANGE PASSWORD\n\n");
            Console.Write("Enter Current Account ID : ");
            user = Console.ReadLine();

            if (user.ToString().Length != 6)
            {
                Console.WriteLine("Input 6 Digits Only!");
                Console.ReadLine();
                Account();
            }
            else if (!int.TryParse(user, out number))
            {
                Console.WriteLine("Input Digits Only!");
                Console.ReadLine();
                Account();
            }

            enterText = "Enter Current Account PIN: ";
            CheckPassword(enterText);

            if (pass.ToString().Length != 6)
            {
                Console.WriteLine("Input 6 Digits Only!");
                Console.ReadLine();
                Account();
            }
            else if (!int.TryParse(pass, out number))
            {
                Console.WriteLine("Input Digits Only!");
                Console.ReadLine();
                Account();
            }
            CP_Validation();

             EnterNewPassword = "Enter New Account PIN: ";
             CheckNewPassword(EnterNewPassword);

            if (NPIN.ToString().Length != 6)
            {
                Console.WriteLine("Input 6 Digits Only!");
                Console.ReadLine();
                Account();
            }
            else if (!int.TryParse(NPIN, out number))
            {
                Console.WriteLine("Input Digits Only!");
                Console.ReadLine();
                Account();
            }

            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
            cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "UPDATE Accounts SET PIN='" + NPIN + "' WHERE PIN=" + pass;

            con.Open();
            int sonuc = cmd.ExecuteNonQuery();
            con.Close();
            if (sonuc > 0)
            {
                Console.WriteLine("\n\nPIN Changed Successfully");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Account();
            }
            else
            {
                Console.WriteLine("There are errors. The PIN was not changed");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Account();
            }
        }
        public static void ADChange_password()
        {
            Console.Clear();
            Console.WriteLine("ADMIN ACCOUNT CHANGE PASSWORD\n\n");
     
            Console.Write("Enter Current Account ID : ");
            user = Console.ReadLine();

            if (user.ToString().Length != 6)
            {
                Console.WriteLine("Input 6 Digits Only!");
                Console.ReadLine();
                Admin_Account();
            }
            else if (!int.TryParse(user, out number))
            {
                Console.WriteLine("Input Digits Only!");
                Console.ReadLine();
                Admin_Account();
            }

            enterText = "Enter Current Account PIN: ";
            CheckPassword(enterText);

            if (pass.ToString().Length != 6)
            {
                Console.WriteLine("Input 6 Digits Only!");
                Console.ReadLine();
                Admin_Account();
            }
            else if (!int.TryParse(pass, out number))
            {
                Console.WriteLine("Input Digits Only!");
                Console.ReadLine();
                Admin_Account();
            }

            ADCP_Validation();

            EnterNewPassword = "Enter New Account PIN: ";
            CheckNewPassword(EnterNewPassword);

            if (NPIN.ToString().Length != 6)
            {
                Console.WriteLine("Input 6 Digits Only!");
                Console.ReadLine();
                Admin_Account();
            }
            else if (!int.TryParse(NPIN, out number))
            {
                Console.WriteLine("Input Digits Only!");
                Console.ReadLine();
                Admin_Account();
            }

            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
            cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "UPDATE Admins SET PIN='" + NPIN + "' WHERE PIN=" + pass;

            con.Open();
            int sonuc = cmd.ExecuteNonQuery();
            con.Close();
            if (sonuc > 0)
            {
                Console.WriteLine("\n\nPIN Changed Successfully");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Admin_Account();
            }
            else
            {
                Console.WriteLine("There are errors. The PIN was not changed");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Admin_Account();
            }
        }
        public static void Dreceipt()
        {
            Console.Clear();
            Console.WriteLine("DEPOSIT TRANSACTION RECEIPT\n\n");
            DateTime now = DateTime.Now;
            Console.WriteLine(now.ToString("F"));
            Console.WriteLine("Transaction: Deposit");
            Console.WriteLine($"Amount: {amounttodeposit}");
            Console.WriteLine($"Available Balance: {balance}");

            Console.WriteLine("\n\nPress enter to continue...");
            Console.ReadLine();
            Account();
        }
        public static void Wreceipt()
        {
            Console.Clear();
            Console.WriteLine("WITHDRAW TRANSACTION RECEIPT\n\n");
            DateTime now = DateTime.Now;
            Console.WriteLine(now.ToString("F"));
            Console.WriteLine("Transaction: Withdraw");
            Console.WriteLine($"Amount: {amounttowithdraw}");
            Console.WriteLine($"Available Balance: {balance}");

            Console.WriteLine("\n\nPress enter to continue...");
            Console.ReadLine();
            Account();
        }
        public static void Validation()
        {
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
            cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM Accounts WHERE AccountID=" + user + "AND PIN=" + pass+"";
            con.Open();
            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Get_balance();
                Account();
            }
            else
            {
                Console.WriteLine("\n\nAccount ID or Account PIN is incorrect");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                User_menu();
            }
            con.Close();
        }
        public static void Admin_Validation()
        {
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
            cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM Admins WHERE AccountID=" + user + "AND PIN=" + pass + "";
            con.Open();
            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Admin_Account();
            }
            else
            {
                Console.WriteLine("\n\nAccount ID or Account PIN is incorrect");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Main();
            }
            con.Close();
        }
        public static void CP_Validation()
        {
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
            cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM Accounts WHERE AccountID=" + user + "AND PIN=" + pass + "";
            con.Open();
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                
            }
            else
            {
                Console.WriteLine("\n\nAccount ID or Account PIN is incorrect");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Account();
            }
            con.Close();
        }
        public static void ADCP_Validation()
        {
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
            cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM Admins WHERE AccountID=" + user + "AND PIN=" + pass + "";
            con.Open();
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {

            }
            else
            {
                Console.WriteLine("\n\nAccount ID or Account PIN is incorrect");
                Console.WriteLine("\n\nPress enter to continue...");
                Console.ReadLine();
                Admin_Account();
            }
            con.Close();
        }
        public static void View_balance()
        {
            Console.Clear();
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
            cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM Accounts WHERE AccountID=" + user + "AND PIN=" + pass + "";
            con.Open();
            reader = cmd.ExecuteReader();
            Console.WriteLine("USER ACCOUNT BALANCE\n\n");
            while (reader.Read())
            {
                Console.WriteLine("Your Current Balance is:  " + reader[4]);
                
            }
            
            con.Close();

        }
        public static void Get_balance()
        {
            Console.Clear();
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
            cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM Accounts WHERE AccountID=" + user + "AND PIN=" + pass + "";
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                balance = Convert.ToInt32(reader[4]);

            }
            con.Close();
        }
        //public static void Create_admin()
        //{
        //    Console.Clear();
        //    string fname;
        //    string lname;
        //    string PIN;
        //    var random = new Random();


        //    Console.Write("ADMINISTRATOR ACCOUNT REGISTRATION\n\n");
        //    Console.Write("Enter your First Name : ");
        //    fname = Console.ReadLine();
        //    Console.Write("Enter your Last Name : ");
        //    lname = Console.ReadLine();
        //    Console.Write("Enter your Account PIN : ");
        //    PIN = Convert.ToInt32(Console.ReadLine());

        //    if(PIN.ToString().Length != 6)
        //    {
        //        Console.WriteLine("Input 6 Digits Only!");
        //        Console.ReadLine();
        //        Admin_menu();
        //    }

        //    accountId = new string(Enumerable.Repeat("0123456789", 6)
        //    .Select(s => s[random.Next(s.Length)]).ToArray());
        //    Console.WriteLine("\n\nPLEASE DO NOT FORGET YOUR ACCOUNT ID");
        //    Console.WriteLine($"\nAccount ID: {accountId}");
        //    Console.WriteLine("\n\nPress enter to continue...");
        //    Console.ReadLine();

        //    con = new OleDbConnection();
        //    con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
        //    cmd = new OleDbCommand();
        //    cmd.Connection = con;
        //    cmd.CommandText = "INSERT INTO Admins (AccountID,FirstName,LastName,PIN) VALUES ('" + accountId + "','" + fname + "','" + lname + "','" + PIN + "')";
        //    
        //    con.Open();
        //    int sonuc = cmd.ExecuteNonQuery();
        //    con.Close();

        //    Admin_menu();
        //}

        //public static void Admin_menu()
        //{
        //    Console.Clear();
        //    Console.WriteLine("ADMIN ACCOUNT\n");
        //    Console.WriteLine("1. Create Admin Account");
        //    Console.WriteLine("2. Log in Admin Account");
        //    Console.WriteLine("3. Exit");
        //    Console.Write("\nSelect your option:");
        //    input = Console.ReadLine();

        //    switch (input)
        //    {
        //        case "1":
        //            Console.Clear();
        //            Create_admin();
        //            break;
        //        case "2":
        //            Console.Clear();
        //            Login_admin();
        //            break;
        //        case "3":
        //            Console.Clear();
        //            Console.WriteLine("\n\nThank you for banking with us!");
        //            Console.ReadLine();
        //            Main();
        //            break;
        //        default:
        //            Console.WriteLine("Invalid Choice");
        //            Console.ReadLine();
        //            Admin_menu();
        //            break;
        //    }
        //}
        public static void Login_admin()
        {
            Console.Clear();

            Console.Write("LOGIN ADMINISTRATOR ACCOUNT\n\n");
            Console.WriteLine("Admin Account ID:");
            user = Console.ReadLine();
            if (user.ToString().Length != 6)
            {
                Console.WriteLine("Input 6 Digits Only!");
                Console.ReadLine();
                Main();
            }
            else if (!int.TryParse(user, out number))
            {
                Console.WriteLine("Input Digits Only!");
                Console.ReadLine();
                Main();
            }
            enterText = "Admin Account PIN: \n";
            CheckPassword(enterText);
            if (pass.ToString().Length != 6)
            {
                Console.WriteLine("Input 6 Digits Only!");
                Console.ReadLine();
                Main();
            }
            else if (!int.TryParse(pass, out number))
            {
                Console.WriteLine("Input Digits Only!");
                Console.ReadLine();
                Main();
            }
            Admin_Validation();
        }
        public static void Admin_Account()
        {
            Console.Clear();
            Console.WriteLine("DATABASE MANAGER\n\n");
            Console.WriteLine("1. View Accounts");
            Console.WriteLine("2. Add Account");
            Console.WriteLine("3. Update Account");
            Console.WriteLine("4. Delete Account");
            Console.WriteLine("5. Change Password");
            Console.WriteLine("6. Exit");
            Console.Write("\nSelect your option:");
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();

                    int counter = 0;
                    con = new OleDbConnection();
                    con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
                    cmd = new OleDbCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM Accounts";
                    con.Open();
                    reader = cmd.ExecuteReader();
                    Console.Write("LIST OF ACCOUNTS\n\n");
                    while (reader.Read())
                    {
                        counter++;
                        Console.WriteLine("Account ID:     " + reader[0] + "     First Name:     " + reader[1] + "     Last Name:     " + reader[2]+"     Account PIN:     " + reader[3] + "     Available Balance:     " + reader[4]);
                    }
                    con.Close();
                    Console.WriteLine("\n\nNumber of Accounts : " + counter);

                    Console.WriteLine("\n\nPress enter to continue...");
                    Console.ReadLine();
                    Admin_Account();
                    break;

                case "2":
                    Console.Clear();

                    string fname;
                    string lname;
                    var random = new Random();


                    Console.Write("ADD NEW USER ACCOUNT\n\n");
                    Console.Write("Enter User Account First Name : ");
                    fname = Console.ReadLine();
                    Console.Write("Enter User Account Last Name : ");
                    lname = Console.ReadLine();
        
                    enterText = "Enter User Account PIN: \n";
                    CheckPassword(enterText);
                    if (pass.ToString().Length != 6)
                    {
                        Console.WriteLine("Input 6 Digits Only!");
                        Console.ReadLine();
                        Admin_Account();
                    }
                    else if (!int.TryParse(pass, out number))
                    {
                        Console.WriteLine("Input Digits Only!");
                        Console.ReadLine();
                        Admin_Account();
                    }
                    accountId = new string(Enumerable.Repeat("0123456789", 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                    Console.WriteLine("\n\nPLEASE DO NOT FORGET YOUR ACCOUNT ID");
                    Console.WriteLine($"\nAccount ID: {accountId}");
                    Console.WriteLine("\n\nPress enter to continue...");
                    Console.ReadLine();

                    con = new OleDbConnection();
                    con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
                    cmd = new OleDbCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "INSERT INTO Accounts (AccountID,FirstName,LastName,PIN) VALUES ('" + accountId + "','" + fname + "','" + lname + "','" + pass + "')";
                    con.Open();
                    int sonuc = cmd.ExecuteNonQuery();
                    con.Close();
                    if (sonuc > 0)
                    {
                        Admin_Account();
                    }
                    else
                    {
                        Console.WriteLine("There are errors. New User Account Failed to Create.");
                        Console.WriteLine("\n\nPress enter to continue...");
                        Console.ReadLine();
                        Admin_Account();
                    }

                    Admin_Account();

                    break;

                case "3":

                    Console.Clear();

                    Console.Write("UPDATE USER ACCOUNT\n\n");
                    Console.Write("Enter User Account ID : ");
                    user = Console.ReadLine();
                    if (user.ToString().Length != 6)
                    {
                        Console.WriteLine("Input 6 Digits Only!");
                        Console.ReadLine();
                        Admin_Account();
                    }
                    else if (!int.TryParse(user, out number))
                    {
                        Console.WriteLine("Input Digits Only!");
                        Console.ReadLine();
                        Admin_Account();
                    }

                    Console.Write("Enter New First Name : ");
                    fname = Console.ReadLine();
                    Console.Write("Enter New Last Name : ");
                    lname = Console.ReadLine();

                    enterText = "Enter New Account PIN: ";
                    CheckPassword(enterText);
                    if (pass.ToString().Length != 6)
                    {
                        Console.WriteLine("Input 6 Digits Only!");
                        Console.ReadLine();
                        Admin_Account();
                    }
                    else if (!int.TryParse(pass, out number))
                    {
                        Console.WriteLine("Input Digits Only!");
                        Console.ReadLine();
                        Admin_Account();
                    }

                    con = new OleDbConnection();
                    con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
                    cmd = new OleDbCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "UPDATE Accounts SET FirstName='" + fname + "',LastName='" + lname + "',PIN='" + pass + "' WHERE AccountID=" + user;

                    con.Open();
                    sonuc = cmd.ExecuteNonQuery();
                    con.Close();
                    if (sonuc > 0)
                    {
                        Console.WriteLine("User Account Updated");
                        Console.WriteLine("\n\nPress enter to continue...");
                        Console.ReadLine();
                        Admin_Account();
                    }
                    else
                    {
                        Console.WriteLine("There are errors. User Account Failed to Update..");
                        Console.WriteLine("\n\nPress enter to continue...");
                        Console.ReadLine();
                        Admin_Account();
                    }

                    break;
                case "4":
                    Console.Clear();
                    string conf;
                    Console.Write("DELETE USER ACCOUNT\n\n");
                    Console.Write("User Account ID : ");
                    user = Console.ReadLine();
                    if (user.ToString().Length != 6)
                    {
                        Console.WriteLine("Input 6 Digits Only!");
                        Console.ReadLine();
                        Admin_Account();
                    }
                    else if (!int.TryParse(user, out number))
                    {
                        Console.WriteLine("Input Digits Only!");
                        Console.ReadLine();
                        Admin_Account();
                    }

                    con = new OleDbConnection();
                    con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BankDatabase.accdb";
                    cmd = new OleDbCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "DELETE FROM Accounts WHERE AccountID=" + user + "";

                    Console.WriteLine($"Are you sure you want to delete {user} account ?");
                    Console.WriteLine($"Press '1' if yes and '2 if no");
                    conf = Console.ReadLine();

                    if(conf == "1")
                    {
                        con.Open();
                        sonuc = cmd.ExecuteNonQuery();
                        con.Close();
                        if (sonuc > 0)
                        {
                            Console.WriteLine("User Account Deleted.");
                            Console.WriteLine("\n\nPress enter to continue...");
                            Console.ReadLine();
                            Admin_Account();
                        }
                        else
                        {
                            Console.WriteLine("There are errors. The User Account Failed to Delete.");
                            Console.WriteLine("\n\nPress enter to continue...");
                            Console.ReadLine();
                            Admin_Account();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Account deletion cancelled.");
                        Console.WriteLine("\n\nPress enter to continue...");
                        Console.ReadLine();
                        Admin_Account();
                    }

                    break;
                case "5":
                    Console.Clear();
                    ADChange_password();
                    break;
                case "6":
                    Console.Clear();
                    Console.WriteLine("\n\nThank you for banking with us!");
                    Console.ReadLine();
                    Main();
                    break;
                default:
                    Console.WriteLine("Invalid Choice");
                    Console.ReadLine();
                    Admin_Account();
                    break;
            }
        }
        static void CheckPassword(string EnterText)
        {
            try
            {
                Console.Write(EnterText);
                pass = "";
                do
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    // Backspace Should Not Work  
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        pass += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                        {
                            pass = pass.Substring(0, (pass.Length - 1));
                            Console.Write("\b \b");
                        }
                        else if (key.Key == ConsoleKey.Enter)
                        {
                            if (string.IsNullOrWhiteSpace(pass))
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Empty value not allowed.");
                                Console.ReadLine();
                                User_menu();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("");
                                break;
                            }
                        }
                    }
                } while (true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void CheckNewPassword(string EnterNewPassword)
        {
            try
            {
                Console.Write(EnterNewPassword);
                NPIN = "";
                do
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    // Backspace Should Not Work  
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        NPIN += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Backspace && NPIN.Length > 0)
                        {
                            NPIN = NPIN.Substring(0, (NPIN.Length - 1));
                            Console.Write("\b \b");
                        }
                        else if (key.Key == ConsoleKey.Enter)
                        {
                            if (string.IsNullOrWhiteSpace(NPIN))
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Empty value not allowed.");
                                Console.ReadLine();
                                Main();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("");
                                break;
                            }
                        }
                    }
                } while (true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
    
 