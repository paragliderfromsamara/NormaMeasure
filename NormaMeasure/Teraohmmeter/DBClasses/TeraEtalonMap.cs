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
        /// <summary>
        /// Флаг использования карты эталонов
        /// </summary>
        public bool InUse = false;
        public string AlterName
        {
            get { return String.IsNullOrEmpty(this.Name) ? "Карта эталонов " + this.Id : this.Name; }
        }

        public int[] CalibrationEtalonNumbers { get { return new int[] { 2, 3, 4, 5, 6 }; } } 

        /// <summary>
        /// Номинальные значения эталонов
        /// </summary>
        public float[] Etalons { get { return new float[] { 1f, 10f, 100f, 1000f, 10000f, 100000f, 1000000f, 10000000f }; } }
        public int[] Dividers = new int[] { 1, 1, 1, 1000, 1000, 1000, 1000000, 1000000 }; 
        /// <summary>
        /// Реальные значения эталонов
        /// </summary>
        public float[][] ResistanceList = new float[][] {
                                                             new float[] { 1f, -1, -1, -1}, //0 1 МОм                                                  
                                                             new float[] { 10f, -1, -1, -1}, //1 10 МОм
                                                             new float[] { 100f, 100f, 100f, 100f}, //2 100 МОм
                                                             new float[] { 1000f, 1000f, 1000f, 1000f}, //3 1ГОм
                                                             new float[] { 10000f, 10000f, 10000f, 10000f}, //4 10 ГОм
                                                             new float[] { 100000f, 100000f, 100000f, 100000f }, //5 100 ГОм
                                                             new float[] { 1000000f, 1000000f, 1000000f, 1000000f }, //6 1 ТОм
                                                             new float[] { 10000000f, 10000000f, 10000000f, 10000000f } //7 10 ТОм
                                                         };

        protected override string[] getColsValues()
        {
            List<string> cols = new List<string>();
            cols.Add(String.Format("\"{0}\"", this.Name));
            float[][] rList = ResistanceList;
            for(int i=0; i<rList.Length; i++)
            {
                float[] f = rList[i];
                for(int j=0; j<f.Length; j++)
                {
                    if (f[j] >= 0) cols.Add(f[j].ToString()); 
                }
            }
            return cols.ToArray();
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
            for(int res = 0; res<ResistanceList.Length; res++ )
            {
                for(int volt=0; volt<ResistanceList[res].Length; volt++)
                {
                    if (ResistanceList[res][volt] >= 0) ResistanceList[res][volt] = ServiceFunctions.convertToFloat(row[String.Format("res{0}_volt{1}", res, volt)]);
                }
            }

        }

        protected override void setDefaultParameters()
        {
            List<string> cols = new List<string>();
            this.tableName = "tera_etalon_maps";
            cols.Add("name");
            for(int res =0; res<ResistanceList.Length; res++)
            {
                for(int volt=0; volt<ResistanceList[res].Length; volt++)
                {
                    if (ResistanceList[res][volt] >= 0) cols.Add(String.Format("res{0}_volt{1}", res, volt));
                }
            }
            cols.Add("id");
            this.colsList = cols.ToArray();
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
