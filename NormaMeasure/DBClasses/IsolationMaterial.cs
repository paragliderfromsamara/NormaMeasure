using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NormaMeasure.Utils;


namespace NormaMeasure.DBClasses
{
    public class IsolationMaterial : DBBase
    {

        public string Name = String.Empty;
        private List<decimal> coeffs = new List<decimal>();
        private List<int> temperatures = new List<int>();


        public decimal GetCoefficient(int temperature)
        {
            int idx = temperatures.IndexOf(temperature);
            return (idx < 0) ? 1 : coeffs[idx];
        }

        public IsolationMaterial(string id) : base(id)
        {
            FillCoeffs();
        }

        public IsolationMaterial(DataRow row) : base(row)
        {
            FillCoeffs();
        }

        protected override void setDefaultParameters()
        {
            this.tableName = "isolation_materials";
            this.getByIdQuery = String.Format("SELECT name, id FROM {0} WHERE isolation_materials.id = {1}", this.tableName, this.Id);
            this.colsList = new string[]
            {
                "name",
                "id"
            };
            base.setDefaultParameters();
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["id"].ToString();
            this.Name = row["name"].ToString();
        }

        private void FillCoeffs()
        {
            string query = "SELECT temperature, coeff_val FROM isolation_material_tcoeffs WHERE isolation_material_id = {0}";
            query = String.Format(query, this.Id);
            DataTable dt = getFromDB(query);
            if (dt.Rows.Count > 0)
            {
                this.coeffs.Clear();
                this.temperatures.Clear();
                for(int i = 0; i<dt.Rows.Count; i++)
                {
                    this.coeffs.Add(ServiceFunctions.convertToDecimal(dt.Rows[i]["coeff_val"]));
                    this.temperatures.Add(ServiceFunctions.convertToInt16(dt.Rows[i]["temperature"]));
                }
            }
        }
    }
}
