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
        
        public string Name = String.Empty;
        public string AlterName
        {
            get { return String.IsNullOrEmpty(this.Name) ? "Карта эталонов " + this.Id : this.Name; }
        }
        public float OneMOm = 1;
        public float TenMOm = 10;
        public float OneHundredMOm = 100;
        public float OneGOm = 1000;
        public float TenGOm = 10000;
        public float OneHundredGOm = 100000;
        public float OneTOm = 1000000;
        public float TenTOm = 10000000;
        
        public int[] CalibrationEtalonNumbers { get { return new int[] { 2, 3, 4, 5, 6 }; } } 

        /// <summary>
        /// Номинальные значения эталонов
        /// </summary>
        public float[] Etalons { get { return new float[] { 1f, 10f, 100f, 1000f, 10000f, 100000f, 1000000f, 10000000f }; } }
        /// <summary>
        /// Реальные значения эталонов
        /// </summary>
        public float[] ResistanceList
        {
            get
            {
                return new float[] { OneMOm, TenMOm, OneHundredMOm, OneGOm, TenGOm, OneHundredGOm, OneTOm, TenTOm };
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
            this.Id = row["id"].ToString();
            this.Name = row["name"].ToString();
            this.OneMOm = ServiceFunctions.convertToFloat(row["one_mom"]);
            this.TenMOm = ServiceFunctions.convertToFloat(row["ten_mom"]);
            this.OneHundredMOm = ServiceFunctions.convertToFloat(row["one_hundred_mom"]);
            this.OneGOm = ServiceFunctions.convertToFloat(row["one_gom"]);
            this.TenGOm = ServiceFunctions.convertToFloat(row["ten_gom"]);
            this.OneHundredGOm = ServiceFunctions.convertToFloat(row["one_hundred_gom"]);
            this.OneTOm = ServiceFunctions.convertToFloat(row["one_tom"]);
            this.TenTOm = ServiceFunctions.convertToFloat(row["ten_tom"]);
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
                                            "ten_tom",
                                            "id",
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
