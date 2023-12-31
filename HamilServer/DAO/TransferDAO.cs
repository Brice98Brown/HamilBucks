﻿using HamilBucks.HamilServer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamilBucks.HamilServer.DAO
{
    public class TransferDAO :ITransferDAO
    {
        private readonly string connectionString;
        public TransferDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public Transfers TransferMoneyToUser(int accountIdTo, decimal ammountToTransfer, int accountIdFrom)
        {
            Transfers transfers;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                const string query = "BEGIN TRANSACTION UPDATE accounts SET balance = balance - @amount WHERE account_id = @account_from UPDATE accounts SET balance = balance + @amount WHERE account_id = @account_to INSERT INTO transfers (transfer_type_id, transfer_status_id,account_from, account_to, amount) VALUES (1001,2001, @account_from, @account_to, @amount) COMMIT TRANSACTION";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@amount", ammountToTransfer);
                cmd.Parameters.AddWithValue("@account_from", accountIdFrom);
                cmd.Parameters.AddWithValue("@account_to", accountIdTo);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT @@IDENTITY", conn);
                int transferId = Convert.ToInt32(cmd.ExecuteScalar());
                transfers = new Transfers();
                transfers.TransferId = transferId;
                transfers.TransferType = "Send";
                transfers.TransferStatus = "Approved";
                transfers.AccountTo = accountIdTo;
                transfers.AccountFrom = accountIdFrom;
                transfers.TransferAmount = ammountToTransfer;
            }
            return transfers;
        }

        public List<Transfers> GetAllTransfersForOneUser(int userId)
        {
            List<Transfers> transfers = new List<Transfers>();

            const string sql = "SELECT account_from, account_to, transfer_id, amount FROM transfers INNER JOIN accounts a on a.account_id = account_from OR account_id = account_to INNER JOIN users u on u.user_id = a.user_id WHERE u.user_id = @userId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Transfers transfer = GetTransfersFromDataReader(reader);

                    transfers.Add(transfer);
                }
            }

            return transfers;

        }

        private Transfers GetTransfersFromDataReader(SqlDataReader reader)
        {
            int transferId = Convert.ToInt32(reader["transfer_id"]);
            int accountIdTo = Convert.ToInt32(reader["account_to"]);
            int accountIdFrom = Convert.ToInt32(reader["account_from"]);
            decimal amountToTransfer = Convert.ToDecimal(reader["amount"]);

            Transfers transfers = new Transfers();
            transfers.TransferId = transferId;
            transfers.TransferType = "Send";
            transfers.TransferStatus = "Approved";

            transfers.AccountTo = accountIdTo;
            transfers.AccountFrom = accountIdFrom;
            transfers.TransferAmount = amountToTransfer;

            return transfers;
        }

        public Transfers RequestTransferMoneyFromUser(int recepiantAccountId, decimal ammountToTransfer, int senderAccountId)
        {
            Transfers transfers;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                const string query = "INSERT INTO transfers (transfer_type_id, transfer_status_id,account_from, account_to, amount) VALUES (1000,2000, @account_from, @account_to, @amount)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@amount", ammountToTransfer);
                cmd.Parameters.AddWithValue("@account_from", senderAccountId);
                cmd.Parameters.AddWithValue("@account_to", recepiantAccountId);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT @@IDENTITY", conn);
                int transferId = Convert.ToInt32(cmd.ExecuteScalar());
                transfers = new Transfers();
                transfers.TransferId = transferId;
                transfers.TransferType = "Request";
                transfers.TransferStatus = "Pending";
                transfers.AccountTo = recepiantAccountId;
                transfers.AccountFrom = senderAccountId;
                transfers.TransferAmount = ammountToTransfer;
            }
            return transfers;
        }
        public Transfers UpdateTrasferStatus(int recepiantAccountId, decimal ammountToTransfer, int transferStatusId, int senderAccountId, int transferId)
        {
            Transfers transfers = GetTransferById(transferId);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = " ";
                string status = " ";
                if (transferStatusId == 2001)//Approved
                {
                    query = "BEGIN TRANSACTION UPDATE accounts SET balance = balance - @amount WHERE account_id = @account_from UPDATE accounts SET balance = balance + @amount WHERE account_id = @account_to UPDATE transfers SET transfer_status_id = @transferStatusId WHERE transfer_id = @transferId ROLLBACK TRANSACTION";
                    status = "Approved";
                }
                else if (transferStatusId == 2002)//rejected
                {
                    query = "UPDATE transfers SET transfer_status_id = @transferStatusId WHERE transfer_id = @transferId";
                    status = "Rejected";
                }
                else
                {
                    return transfers;
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@amount", ammountToTransfer);
                cmd.Parameters.AddWithValue("@account_from", senderAccountId);
                cmd.Parameters.AddWithValue("@account_to", recepiantAccountId);
                cmd.Parameters.AddWithValue("@transferStatusId", transferStatusId);
                cmd.Parameters.AddWithValue("@transferId", transferId);

                cmd.ExecuteNonQuery();



                transfers.TransferStatus = status;

            }
            return transfers;
        }

        public Transfers GetTransferById(int transferId)
        {
            Transfers transfers = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();
                string query = "SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount FROM transfers WHERE transfer_id = @transferId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@transferId", transferId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    transfers = GetTransfersFromDataReader(reader);
                }
                return transfers;
            }

        }
    }
}
