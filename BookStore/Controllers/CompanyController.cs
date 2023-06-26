using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BookStore.Controllers
{

    public class CompanyController : ApiController
    {
        [Route("getcompany")]
        [HttpGet]
        public async Task<IEnumerable<Company>> Get()
        {
            var companys = new List<Company>();
            string connectionString = "Data Source=DESKTOP-IV00ME4\\SQLEXPRESS;Initial Catalog=BookStore1;Integrated Security=True";
            //Khoi tao doi tuong
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Doi tuong sql command
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //Khai bao cau truy van
            sqlCommand.CommandText = "dbo.Proc_GetCompany";
            //Mo ket noi
            sqlConnection.Open();
            //Thuc thi cong viec voi database
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            //Xu ly du lieu tra ve
            while (sqlDataReader.Read())
            {
                var company = new Company();
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    //Lay ten cot du lieu dang doc
                    var colName = sqlDataReader.GetName(i);
                    //Lay gia tri o du lieu dang doc
                    var value = sqlDataReader.GetValue(i);
                    //Lay property theo ten cot 
                    var property = company.GetType().GetProperty(colName);

                    if (property != null && value != DBNull.Value)
                    {
                        property.SetValue(company, value);
                    }
                }
                companys.Add(company);
            }
            //Dong ket noi
            sqlConnection.Close();
            return companys;
        }
    }
}
