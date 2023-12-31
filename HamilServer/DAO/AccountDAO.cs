﻿using HamilBucks.HamilServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace HamilBucks.HamilServer.DAO
{
    public class AccountDAO : IAccountDAO
    {
        private readonly string connectionString;
        public AccountDAO(string connstr)
        {
            this.connectionString = connstr;
        }
        public Account GetAccountByUserId(int userId)
        {
            string query = "SELECT account_id,user_id,balance FROM accounts WHERE user_id = @id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@id", userId);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Account account = GetAccountFromDataReader(reader);
                    return account;
                }
            }
            return null;
        }

        private Account GetAccountFromDataReader(SqlDataReader reader)
        {
            int userId = Convert.ToInt32(reader["user_id"]);
            int accountNumber = Convert.ToInt32(reader["account_id"]);
            decimal balance = Convert.ToDecimal(reader["balance"]);

            Account account = new Account();
            account.AccountId = accountNumber;
            account.UserId = userId;
            account.Balance = balance;

            return account;
        }

        public Account GetAccountByAccountId(int accountId)
        {
            string query = "SELECT account_id,user_id,balance FROM accounts WHERE account_id = @accountid";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@accountid", accountId);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Account account = GetAccountFromDataReader(reader);
                    return account;
                }
            }
            return null;
        }
    }
}
