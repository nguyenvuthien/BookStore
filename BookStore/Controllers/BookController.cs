using System.Web.Http;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using BookStore.Models;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [RoutePrefix("api/v1/books")]
    public class BookController : ApiController
    {
        [Route("page")]
        [HttpGet]
        public async Task<IEnumerable<ViewBook>> GetPage(int pageNum)
        {
            var viewbooks = new List<ViewBook>();
            string connectionString = "Data Source=DESKTOP-IV00ME4\\SQLEXPRESS;Initial Catalog=BookStore1;Integrated Security=True";
            //Khoi tao doi tuong
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Doi tuong sql command
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //Khai bao cau truy van
            sqlCommand.CommandText = "dbo.PhanTrang";
            sqlCommand.Parameters.Add("@pageNum", System.Data.SqlDbType.Int).Value = pageNum;
            //Mo ket noi
            sqlConnection.Open();
            //Thuc thi cong viec voi database
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            //Xu ly du lieu tra ve
            while (sqlDataReader.Read())
            {
                var viewbook = new ViewBook();
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    //Lay ten cot du lieu dang doc
                    var colName = sqlDataReader.GetName(i);
                    //Lay gia tri o du lieu dang doc
                    var value = sqlDataReader.GetValue(i);
                    //Lay property theo ten cot 
                    var property = viewbook.GetType().GetProperty(colName);

                    if (property != null && value != DBNull.Value)
                    {
                        property.SetValue(viewbook, value);
                    }
                }
                viewbooks.Add(viewbook);
            }
            //Dong ket noi
            sqlConnection.Close();
            return viewbooks;
        }

        [Route("search")]
        [HttpGet]
        public async Task<IEnumerable<ViewBook>> GetBookBySearch(string data)
        {
            var viewbooks = new List<ViewBook>();
            string connectionString = "Data Source=DESKTOP-IV00ME4\\SQLEXPRESS;Initial Catalog=BookStore1;Integrated Security=True";
            //Khoi tao doi tuong
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Doi tuong sql command
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //Khai bao cau truy van
            sqlCommand.CommandText = "dbo.Proc_Search";
            sqlCommand.Parameters.Add("@data", System.Data.SqlDbType.NVarChar).Value = data;
            //Mo ket noi
            sqlConnection.Open();
            //Thuc thi cong viec voi database
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            //Xu ly du lieu tra ve
            while (sqlDataReader.Read())
            {
                var viewbook = new ViewBook();
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    //Lay ten cot du lieu dang doc
                    var colName = sqlDataReader.GetName(i);
                    //Lay gia tri o du lieu dang doc
                    var value = sqlDataReader.GetValue(i);
                    //Lay property theo ten cot 
                    var property = viewbook.GetType().GetProperty(colName);

                    if (property != null && value != DBNull.Value)
                    {
                        property.SetValue(viewbook, value);
                    }
                }

                viewbooks.Add(viewbook);
            }
            //Dong ket noi
            sqlConnection.Close();
            return viewbooks;
        }

        [Route("getbook/{bookCode}")]

        public async Task<object> Get(int bookCode)
        {
            var books = new List<Book>();
            string connectionString = "Data Source=DESKTOP-IV00ME4\\SQLEXPRESS;Initial Catalog=BookStore1;Integrated Security=True";
            //Khoi tao doi tuong
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Doi tuong sql command
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //Khai bao cau truy van
            sqlCommand.CommandText = "dbo.Proc_GetData";
            sqlCommand.Parameters.Add("@BookCode", System.Data.SqlDbType.Int).Value = bookCode;
            //Mo ket noi
            sqlConnection.Open();
            //Thuc thi cong viec voi database
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            //Xu ly du lieu tra ve
            while (sqlDataReader.Read())
            {
                var book = new Book();
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    //Lay ten cot du lieu dang doc
                    var colName = sqlDataReader.GetName(i);
                    //Lay gia tri o du lieu dang doc
                    var value = sqlDataReader.GetValue(i);
                    //Lay property theo ten cot 
                    var property = book.GetType().GetProperty(colName);

                    if (property != null && value != DBNull.Value)
                    {
                        property.SetValue(book, value);
                    }
                }

                books.Add(book);
            }
            //Dong ket noi
            sqlConnection.Close();
            return books;
        }
        //Post
        [Route("")]
        public async Task<bool> Post([FromBody] Book book)
        {
            var books = new List<Book>();
            string connectionString = "Data Source=DESKTOP-IV00ME4\\SQLEXPRESS;Initial Catalog=BookStore1;Integrated Security=True";
            //Khoi tao doi tuong
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Doi tuong sql command
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //Khai bao cau truy van
            sqlCommand.CommandText = "dbo.Proc_InsertBook";
            //Gan gia tri dau vao cho cac tham so cua proc
            sqlCommand.Parameters.AddWithValue("@BookName", book.BookName);
            sqlCommand.Parameters.AddWithValue("@Author", book.Author);
            sqlCommand.Parameters.AddWithValue("@Price", book.Price);
            sqlCommand.Parameters.AddWithValue("@GenreCode", book.GenreCode);
            sqlCommand.Parameters.AddWithValue("@CompanyCode", book.CompanyCode);
            sqlCommand.Parameters.AddWithValue("@Mount", book.Mount);
            sqlCommand.Parameters.AddWithValue("@CreateAt", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@ModifiedAt", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@isDeleted", 1);

            //Mo ket noi
            sqlConnection.Open();
            //Thuc thi cong viec
            var result = await sqlCommand.ExecuteNonQueryAsync();

            //Dong ket noi
            sqlConnection.Close();
            return true;
        }
        [Route("putbook/{bookCode}")]
        public async Task<bool> Put([FromBody] Book book, int bookCode)
        {
            var books = new List<Book>();
            string connectionString = "Data Source=DESKTOP-IV00ME4\\SQLEXPRESS;Initial Catalog=BookStore1;Integrated Security=True";
            //Khoi tao doi tuong
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Doi tuong sql command
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //Khai bao cau truy van
            sqlCommand.CommandText = "dbo.Proc_UpdateBook";
            //Gan gia tri dau vao cho cac tham so cua proc
            sqlCommand.Parameters.Add("@BookCode", System.Data.SqlDbType.Int).Value = bookCode;
            sqlCommand.Parameters.AddWithValue("@BookName", book.BookName);
            sqlCommand.Parameters.AddWithValue("@Author", book.Author);
            sqlCommand.Parameters.AddWithValue("@Price", book.Price);
            sqlCommand.Parameters.AddWithValue("@GenreCode", book.GenreCode);
            sqlCommand.Parameters.AddWithValue("@CompanyCode", book.CompanyCode);
            sqlCommand.Parameters.AddWithValue("@Mount", book.Mount);
            sqlCommand.Parameters.AddWithValue("@ModifiedAt", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@isDeleted", 1);

            //Mo ket noi
            sqlConnection.Open();
            //Thuc thi cong viec
            var result = await sqlCommand.ExecuteNonQueryAsync();

            //Dong ket noi
            sqlConnection.Close();
            return true;
        }

        [HttpPut]
        [Route("deletebook")]
        public async Task<bool> DeleteBook(int bookCode)
        {
            var books = new List<Book>();
            string connectionString = "Data Source=DESKTOP-IV00ME4\\SQLEXPRESS;Initial Catalog=BookStore1;Integrated Security=True";
            //Khoi tao doi tuong
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Doi tuong sql command
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //Khai bao cau truy van
            sqlCommand.CommandText = "dbo.Proc_UpdateDeleteBook";
            //Gan gia tri dau vao cho cac tham so cua proc
            sqlCommand.Parameters.Add("@BookCode", System.Data.SqlDbType.Int).Value = bookCode;
            sqlCommand.Parameters.AddWithValue("@ModifiedAt", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@isDeleted", 0);

            //Mo ket noi
            sqlConnection.Open();
            //Thuc thi cong viec
            var result = await sqlCommand.ExecuteNonQueryAsync();

            //Dong ket noi
            sqlConnection.Close();
            return true;
        }

        [HttpDelete]
        [Route("bookCode")]
        public bool Delete(int bookCode)
        {
            return true;
        }
    }
}