using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NormaMeasure.Utils;
using MySql.Data.MySqlClient;

namespace NormaMeasure.DBClasses
{
    public class DBBase
    {
        public string Id = "0";
        public string getAllQuery;
        protected string getByIdQuery;
        protected string tableName = "default_table_name";
        protected string[] colsList = new string[] { };
        protected DataRow dataRow;

        public DBBase(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
        }

        public DBBase()
        {
            setDefaultParameters();
        }

        public DBBase(DataRow row) : this()
        {
            //this.dataRow = row;
            //setDefaultParameters();
            fillParametersFromRow(row);
        }


        protected virtual string[] getColsValues()
        {
            return new string[] { };
        }

        protected object getAttrFromRow(string colName)
        {
            try
            {
                return this.dataRow[colName];
            }catch(Exception)
            {
                return null;
            }
        } 

        protected decimal getDecimalFromRow(string colName)
        {
            object v = getAttrFromRow(colName);
            if (v != null)
            {
                return ServiceFunctions.convertToDecimal(v);
            }
            else return 0;
        }

        public virtual bool Create()
        {
            string insertQuery = "INSERT INTO {0} ({1}) VALUES ({2})";
            string columns = "";
            string values = "";
            string[] colsVals = getColsValues();
            for (int i = 0; i<this.colsList.Length; i++)
            {
                if (i > 0)
                {
                    columns += ",";
                    values += ",";
                }
                columns += colsList[i];
                values += colsVals[i];
            }
            long val = SendQuery(String.Format(insertQuery, this.tableName, columns, values));
            return val == 0;
        }

        protected virtual void fillParametersFromRow(DataRow row) { }

        protected virtual void setDefaultParameters()
        {
            this.getAllQuery = "SELECT * FROM " + this.tableName;
        }

        protected bool NeedLoadFromDB(DataRow row)
        {
            bool f = false;
            foreach(string colName in colsList)
            {
                f = row.IsNull(colName);
                if(f) break;
            }
            return f;
        }

        protected DataTable getFromDB(string query)
        {
            DataSet ds = makeDataSet();
            DBControl mySql = new DBControl(DBSettings.Default.DBName);
            mySql.MyConn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(query, mySql.MyConn);
            ds.Tables[tableName].Rows.Clear();
            da.Fill(ds.Tables[tableName]);
            mySql.MyConn.Close();
            return ds.Tables[tableName];
        }

        protected long UpdateField(string updVals, string condition)
        {
            DBControl mySql = new DBControl();
            string query = BuildUpdQuery(this.tableName, updVals, condition);
            long v;
            mySql.MyConn.Open();
            v = mySql.RunNoQuery(query);
            mySql.MyConn.Close();
            return v;
        }


        /// <summary>
        /// Отправляет список запросов в базу данных
        /// </summary>
        /// <param name="fields"></param>
        public static void SendQueriesList(string[] fields)
        {
            if (fields.Length == 0) return;
            DBControl mySql = new DBControl();
            mySql.MyConn.Open();
            foreach(string f in fields) mySql.RunNoQuery(f);
            mySql.MyConn.Close();
        }

        /// <summary>
        /// Отправляет одиночный запрос в базу данных
        /// </summary>
        /// <param name="query"></param>
        public static long SendQuery(string query)
        {
            DBControl mySql = new DBControl();
            long val;
            mySql.MyConn.Open();
            val = mySql.RunNoQuery(query);
            mySql.MyConn.Close();
            return val;
        }

        protected static string BuildDestroyQueryWithCriteria(string tableName, string condition)
        {
            return String.Format("DELETE FROM {0} WHERE {1}", tableName, condition);
        }


        protected static string BuildUpdQuery(string tableName, string updVals, string condition)
        {
            return String.Format("UPDATE {0} SET {1} WHERE {2}", tableName, updVals, condition);
        }

        protected DataSet makeDataSet()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(tableName);
            foreach (string colName in colsList) ds.Tables[tableName].Columns.Add(colName); 
            return ds;
        }

        protected bool GetById()
        {
            DataTable tab = getFromDB(getByIdQuery);
            this.dataRow = tab.Rows.Count > 0 ? tab.Rows[0] : null;
            return this.dataRow != null;
        }

        protected DataTable GetAllFromDB()
        {
            return getFromDB(getAllQuery);
        }



    }
}
