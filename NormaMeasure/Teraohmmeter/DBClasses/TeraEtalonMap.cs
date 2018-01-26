using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NormaMeasure.DBClasses;
using NormaMeasure.Utils;


namespace NormaMeasure.Teraohmmeter.DBClasses
{
    public class TeraEtalonMap : DBBase
    {
        public string Name = "No name";
        public decimal OneMOm = 1;
        public decimal TenMOm = 10;
        public decimal OneHundredMOm = 100;
        public decimal OneGOm = 1000;
        public decimal TenGOm = 10000;
        public decimal OneHundredGOm = 100000;
        public decimal OneTOm = 1000000;
        public decimal TenTOm = 10000000;

        public int[] CalibrationEtalonNumbers = new int[] { 2, 3, 4, 5, 6 }; 

        /// <summary>
        /// Номинальные значения эталонов
        /// </summary>
        public decimal[] Etalons = new decimal[] { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000 };
        /// <summary>
        /// Реальные значения эталонов
        /// </summary>
        public decimal[] ResistanceList
        {
            get
            {
                return new decimal[] { OneMOm, TenMOm, OneHundredMOm, OneGOm, TenGOm, OneHundredGOm, OneTOm, TenTOm };
            }
        }

        protected override string[] getColsValues()
        {
            return new string[] {
                                    String.Format("\"{0}\"", this.Name),
                                    OneMOm.ToString(),
                                    TenMOm.ToString(),
                                    OneHundredMOm.ToString(),
                                    OneGOm.ToString(),
                                    TenGOm.ToString(),
                                    OneHundredGOm.ToString(),
                                    OneTOm.ToString(),
                                    TenTOm.ToString()
                                };
        }

        public TeraEtalonMap() : base()
        {

        }

        public TeraEtalonMap(string id) : base(id)
        {

        }
        public TeraEtalonMap(DataRow row) : base(row)
        {

        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Name = row["name"].ToString();
            this.OneMOm = ServiceFunctions.convertToDecimal(row["one_mom"]);
            this.TenMOm = ServiceFunctions.convertToDecimal(row["ten_mom"]);
            this.OneHundredMOm = ServiceFunctions.convertToDecimal(row["one_hundred_mom"]);
            this.OneGOm = ServiceFunctions.convertToDecimal(row["one_gom"]);
            this.TenGOm = ServiceFunctions.convertToDecimal(row["ten_gom"]);
            this.OneHundredGOm = ServiceFunctions.convertToDecimal(row["one_hundred_gom"]);
            this.OneTOm = ServiceFunctions.convertToDecimal(row["one_tom"]);
            this.TenTOm = ServiceFunctions.convertToDecimal(row["ten_tom"]);
        }

        protected override void setDefaultParameters()
        {
            this.tableName = "tera_etalon_maps";
            this.colsList = new string[] {
                                            "name",
                                            "one_mom",
                                            "ten_mom",
                                            "one_hundred_mom",
                                            "one_gom",
                                            "ten_gom",
                                            "one_hundred_gom",
                                            "one_tom",
                                            "ten_tom"
                                         };

            base.setDefaultParameters();
        }

        /// <summary>
        /// Возвращает список карт эталонов
        /// </summary>
        /// <returns></returns>
        public TeraEtalonMap[] GetAll()
        {
            TeraEtalonMap[] mTypes = new TeraEtalonMap[] { };
            DataTable dt = GetAllFromDB();
            if (dt.Rows.Count > 0)
            {
                mTypes = new TeraEtalonMap[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++) mTypes[i] = new TeraEtalonMap(dt.Rows[i]);
            }
            return mTypes;
        }
    }
}
