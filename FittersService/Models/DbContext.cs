using System.Data;
using System.Data.SqlClient;

namespace FittersService.Models
{
    public class DbContext
    {
        string conn = "Server=tcp:jvo-sql-db-server.database.windows.net,1433;Initial Catalog=jvo-sql-db;Persist Security Info=False;User ID=jonas;Password=!Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public List<Fitter> GetFitters()
        {
            List<Fitter> list = new List<Fitter>();
            string query = "Select * from Fitters";
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new Fitter { ID = Convert.ToInt32(dr[0]), FullName = Convert.ToString(dr[1]), PhoneNumber = Convert.ToString(dr[2]), FitterType = Convert.ToInt32(dr[3]), UnderFitters = GetUnderFitters(Convert.ToInt32(dr[0])) });
                    }
                }
            }
            return list;
        }

        public int Add(Fitter obj)
        {
            string query = "insert into Fitters values('" + obj.FullName + "','" + obj.PhoneNumber + "','" + obj.FitterType + "'); "
            + "SELECT CAST(scope_identity() AS int)";
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    int i = (Int32)cmd.ExecuteScalar();
                    if (i >= 1)
                    {
                        return i;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public bool Edit(int id, Fitter obj)
        {
            string query = "update Fitters set FullName= '" + obj.FullName + "', PhoneNumber='" + obj.PhoneNumber + "', FitterTypeID='" + obj.FitterType + "' where Id='" + id + "' ";
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool DeleteFitter(int id)
        {
            //Delete relations first
            if (DeleteFitterRelations(id))
            {
                string query = "delete Fitters where Id='" + id + "'";
                using (SqlConnection con = new SqlConnection(conn))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i >= 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public bool AddUnderFitter(int overfitterID, int underfitterID)
        {
            string query = "insert into FittersRelations values('" + overfitterID + "','" + underfitterID + "')";
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool DeleteFitterRelations(int id)
        {
            string query = "delete from FittersRelations where OverFitterID = " + id + " or UnderFitterId = " + id;
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool DeleteUnderFitter(int underfitterID, int overfitterID)
        {
            string query = "delete from FittersRelations where OverFitterID = " + overfitterID + " and UnderFitterId = " + underfitterID;
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public List<Fitter> GetUnderFitters(int overfitterID)
        {
            List<Fitter> list = new List<Fitter>();
            string query = "SELECT f.ID, f.FullName, f.PhoneNumber, f.FitterTypeID " +
                "FROM FittersRelations fr " +
                "INNER JOIN fitters f ON f.ID = fr.UnderFitterID " +
                "WHERE fr.OverFitterID = " + overfitterID;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new Fitter { ID = Convert.ToInt32(dr[0]), FullName = Convert.ToString(dr[1]), PhoneNumber = Convert.ToString(dr[2]), FitterType = Convert.ToInt32(dr[3]) });
                    }
                }
            }
            return list;
        }
    }
}
