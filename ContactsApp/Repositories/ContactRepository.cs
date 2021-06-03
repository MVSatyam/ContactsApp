using ContactsApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsApp.Repositories
{
    public class ContactRepository : IContactRepository
    {

        private readonly IConfiguration _configuration;
        private readonly string connString;

        public ContactRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connString = _configuration.GetConnectionString("MyConn");
        }

        private List<ContactModel> FillContatsModel(List<DataRow> dataRows)
        {
            List<ContactModel> contactList = new List<ContactModel>();
            foreach (var contact in dataRows)
            {
                ContactModel contactInfo = new ContactModel();
                contactInfo.Id = Convert.ToInt32(contact["Id"]);
                contactInfo.Name = contact["Name"].ToString();
                contactInfo.Email = contact["Email"].ToString();
                contactInfo.PhNo = contact["PhNo"].ToString();
                contactList.Add(contactInfo);
            }
            return contactList;
        }

        private DataTable GetDataFromQuery(string query, int id = 0)
        {
            var dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, conn);
                if (id != 0)
                {
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);
                }
                sqlDataAdapter.Fill(dataTable);
            }
            return dataTable;
        }

        private string GetQuery(string QueryFor)
        {
            string query = "";
            if (QueryFor.Equals("AllContactsQuery"))
            {
                query = "SELECT * FROM Contacts";
            }
            else if (QueryFor.Equals("ContactDetailsQuery"))
            {
                query = "SELECT * FROM Contacts WHERE Id=@Id";
            }
            else if (QueryFor.Equals("CreateContactQuery"))
            {
                query = "INSERT INTO Contacts VALUES(@Name, @Email, @PhNo)";
            }
            else if (QueryFor.Equals("DeleteContactQuery"))
            {
                query = "DELETE FROM Contacts WHERE Id=@Id";
            }
            else if (QueryFor.Equals("UpdateContactQuery"))
            {
                query = "UPDATE Contacts SET Name=@Name, Email=@Email, PhNo=@PhNo WHERE Id=@Id";
            }

            return query;
        }


        public List<ContactModel> AllContacts()
        {

            var query = GetQuery("AllContactsQuery");

            var data = GetDataFromQuery(query);

            var Contacts = FillContatsModel(data.AsEnumerable().ToList());

            return Contacts;
        }


        public ContactModel ContactDetails(int id)
        {
            string query = GetQuery("ContactDetailsQuery");

            var data = GetDataFromQuery(query, id);

            var contact = GetContact(data);

            return contact;
        }

        private ContactModel GetContact(DataTable dataTable)
        {
            ContactModel contactModel = new ContactModel();
            if (dataTable.Rows.Count == 1)
            {

                contactModel.Id = Convert.ToInt32(dataTable.Rows[0]["Id"].ToString());
                contactModel.Name = dataTable.Rows[0]["Name"].ToString();
                contactModel.Email = dataTable.Rows[0]["Email"].ToString();
                contactModel.PhNo = dataTable.Rows[0]["PhNo"].ToString();

                return contactModel;
            }
            else
            {

                return contactModel;
            }
        }

        public void CreateContact(ContactModel contact)
        {
            string query = GetQuery("CreateContactQuery");

            InsertData(query, contact);
            
        }

        private void InsertData(string query, ContactModel contact)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand(query, sqlConnection);
                sqlCmd.Parameters.AddWithValue("@Name", contact.Name);
                sqlCmd.Parameters.AddWithValue("@Email", contact.Email);
                sqlCmd.Parameters.AddWithValue("@PhNo", contact.PhNo);
                sqlCmd.ExecuteNonQuery();
            }
        }

        public void DeleteContact(int id)
        {
            string query = GetQuery("DeleteContactQuery");

            DeleteData(id, query);
        }

        private void DeleteData(int id, string query)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand(query, conn);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlCommand.ExecuteNonQuery();
            }
        }

        public void Update(int id, ContactModel contact)
        {
            string query = GetQuery("UpdateContactQuery");
            Console.WriteLine(query);

            UpdateData(id, query, contact);   
        }

        private void UpdateData(int id, string query, ContactModel contact)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand(query, conn);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlCommand.Parameters.AddWithValue("@Name", contact.Name);
                sqlCommand.Parameters.AddWithValue("@Email", contact.Email);
                sqlCommand.Parameters.AddWithValue("@PhNo", contact.PhNo);
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
