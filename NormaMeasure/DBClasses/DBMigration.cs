using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace NormaMeasure.DBClasses
{
    class DBMigration
    {
        private static string dbName = DBClasses.DBSettings.Default.DBName;
        private static string query;
        private static string message;
        private static string connString = DBControl.ConnectionString;
        private static MySqlConnection dbCon;

        public DBMigration()
        {
            try
            {
                dbCon = new MySqlConnection(connString); //Устанавливаем соединение
                dropDBTera();                             //Удаляем бд (необходимо на стадии настройки)
                checkAndCreateDBTera();                   //Создаем бд (если нет её)
                createTables();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка SQL подключения", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Создаёт таблицы в базе данных и добавляет в них значения по умолчанию
        /// </summary>
        public void createTables()
        {
            createUsersTable(); //Проверяет и создает таблицу пользователей
            createRolesTable(); //Проверяет и создает таблицу с типами пользователей
            createCameraTypesTable(); //создаёт таблицу с измерительными камерами
            createIsolationMaterialsTable(); //Добавляет таблицу с изоляционными материалами
            createIsolationMaterialTCoeffsTable(); //Добавляет таблицу с температурными коэффициентами для материалов изоляции
            createBringingTypesTable(); //Добавляет таблицу типов приведения
            createDevicesTable(); // createDevicesTable(); //Добавляет таблицу с приборами
            createEtalonMapsTable(); // создаёт таблицу с эталонами
        }

        private void createEtalonMapsTable()
        {
            string tableName = "tera_etalon_maps";
            string[] colsArray = {
                                    //INSERT INTO devices (serial_number) VALUES {0}
                                    //UPDATE devices SET devices.zero_range_coeff = {1}, devices.first_range_coeff = {2}, devices.second_range_coeff = {3}, devices.third_range_coeff = {4}, devices.third_range_additional_coeff = {5}, devices.one_hundred_volts_coeff = {6}, devices.five_hundred_volts_coeff = {7}, devices.thousand_volts_coeff = {8}, devices.coeffs_check_sum = {9} WHERE devices.serial_number IN("{0}")
                                    //при добавлении столбцов необходимо исправить строки запроса 
                                    "id INT UNSIGNED AUTO_INCREMENT NOT NULL", //0
                                    "name TINYTEXT",                  //1
                                    "one_mom FLOAT Default 1.0",      //2
                                    "ten_mom FLOAT Default 10.0",
                                    "one_hundred_mom FLOAT Default 100.0",     //3
                                    "one_gom FLOAT Default 1000.0",    //4
                                    "ten_gom FLOAT Default 10000.0",     //5
                                    "one_hundred_gom FLOAT Default 100000.0", //6
                                    "one_tom FLOAT Default 1000000.0",      //7
                                    "ten_tom FLOAT Default 10000000.0",     //8
                                    "PRIMARY KEY (id)"
                                 };
            checkAndAddTable(tableName, colsArray);
        }

        private void createDevicesTable()
        {
            string tableName = "devices";
            string[] colsArray = {
                                    //INSERT INTO devices (serial_number) VALUES {0}
                                    //UPDATE devices SET devices.zero_range_coeff = {1}, devices.first_range_coeff = {2}, devices.second_range_coeff = {3}, devices.third_range_coeff = {4}, devices.third_range_additional_coeff = {5}, devices.one_hundred_volts_coeff = {6}, devices.five_hundred_volts_coeff = {7}, devices.thousand_volts_coeff = {8}, devices.coeffs_check_sum = {9} WHERE devices.serial_number IN("{0}")
                                    //при добавлении столбцов необходимо исправить строки запроса 
                                    "id INT UNSIGNED AUTO_INCREMENT NOT NULL", //0
                                    "serial_number TINYTEXT",                  //1
                                    "zero_range_coeff FLOAT Default 0.0",      //2
                                    "first_range_coeff FLOAT Default 0.0",     //3
                                    "second_range_coeff FLOAT Default 0.0",    //4
                                    "third_range_coeff FLOAT Default 0.0",     //5
                                    "third_range_additional_coeff FLOAT Default 0.0", //6
                                    "one_hundred_volts_coeff FLOAT Default 0.0",      //7
                                    "five_hundred_volts_coeff FLOAT Default 0.0",     //8
                                    "thousand_volts_coeff FLOAT Default 0.0",         //9
                                    "coeffs_check_sum INT UNSIGNED Default 0.0", //10 контрольная сумма коэффициентов, для проверки изменились они, или нет
                                    "PRIMARY KEY (id)"
                                 };
            checkAndAddTable(tableName, colsArray);
        }
        private void createBringingTypesTable()
        {
            string tableName = "bringing_types";
            string[][] sysadmParams =
            {
                new string[] { "1","'Без приведения'", "''"},
                new string[] { "2","'Приведение к длине'", "'·'"},
                new string[] { "3","'Объёмное сопротивление'", "'·м'"},
                new string[] { "4","'Поверхностное сопротивление'", "''"},
            };
            string[] colsArray = {
                                    "id INT UNSIGNED AUTO_INCREMENT NOT NULL",
                                    "name TINYTEXT",
                                    "result_prefix TINYTEXT",
                                    "PRIMARY KEY (id)"
                                 };
            checkAndAddTable(tableName, colsArray);
            addBasicDataToTable(tableName, sysadmParams);
        }
        private void createBringingTypeQuantitativeMeasureTable()
        {
            string tableName = "bringing_types";
            string[][] sysadmParams =
            {
                new string[] { "1","'Без приведения'", "''"},
                new string[] { "2","'Приведение к длине'", "'·'"},
                new string[] { "3","'Объёмное сопротивление'", "'·м'"},
                new string[] { "4","'Поверхностное сопротивление'", "''"},
            };
            string[] colsArray = {
                                    "id INT UNSIGNED AUTO_INCREMENT NOT NULL",
                                    "name TINYTEXT",
                                    "result_prefix TINYTEXT",
                                    "PRIMARY KEY (id)"
                                 };
            checkAndAddTable(tableName, colsArray);
            addBasicDataToTable(tableName, sysadmParams);
        }


        private static void createCameraTypesTable()
        {
            string tableName = "camera_types";
            string[][] sysadmParams =
            { new string[] { "1",
                "'ИК-90'",
                "50",
                "54"},
              new string[] { "2",
                "'ИК-50'",
                "25",
                "29"}

            };
            string[] colsArray = {
                                    "id INT UNSIGNED AUTO_INCREMENT NOT NULL",
                                    "name TINYTEXT",
                                    "external_diameter INT UNSIGNED",
                                    "internal_diameter INT UNSIGNED",
                                    "PRIMARY KEY (id)"
                                 };
            checkAndAddTable(tableName, colsArray);
            addBasicDataToTable(tableName, sysadmParams);
        }

        private static void createUsersTable()
        {
            string tableName = "Users";
            string[][] sysadmParams =
            { new string[] { "DEFAULT",
                "'Sysadmin'",
                "'Sysadmin'",
                "'Sysadmin'",
                "0",
                "1",
                "'123456'",
                "1"}

            };
            string[] colsArray = {
                                    "id INT UNSIGNED AUTO_INCREMENT NOT NULL",
                                    "last_name TINYTEXT",
                                    "name TINYTEXT",
                                    "third_name TINYTEXT",
                                    "employee_number INT UNSIGNED",
                                    "role_id INT UNSIGNED",
                                    "password TINYBLOB",
                                    "is_active BOOL",
                                    "PRIMARY KEY (id)"
                                 };
            checkAndAddTable(tableName, colsArray);
            addBasicDataToTable(tableName, sysadmParams);

        }

        /// <summary>
        /// Добавляет таблицу ролей пользователей и заполняет её базовыми ролями, если их нет
        /// </summary>
        private static void createRolesTable()
        {
            string tableName = "Roles";
            string[] colsArray = {
                                    "id INT UNSIGNED AUTO_INCREMENT NOT NULL",
                                    "name TINYTEXT",
                                    "PRIMARY KEY (id)"
                                 };
            string[][] defaultValues = {
                new string[] { "1", "'Администратор БД'" },
                new string[] { "2", "'Метролог'" },
                new string[] { "3", "'Мастер'" },
                new string[] { "4", "'Оператор'" } };
            checkAndAddTable(tableName, colsArray);
            addBasicDataToTable(tableName, defaultValues);
        }


        private static void createIsolationMaterialsTable()
        {
            string tableName = "isolation_materials";
            string[] colsArray = {
                                    "id INT UNSIGNED AUTO_INCREMENT NOT NULL",
                                    "name TINYTEXT",
                                    "PRIMARY KEY (id)"
                                 };
            string[][] defaultValues =
{
                new string[] {"1", "'Полиэтилен'"},
                new string[] {"2", "'Резина'"},
                new string[] {"3", "'Пропитанная бумага'"}
            };
            checkAndAddTable(tableName, colsArray);
            addBasicDataToTable(tableName, defaultValues);
        }
        /// <summary>
        /// Добавляет таблицу с температурными коэффициентами для изоляции
        /// </summary>
        private static void createIsolationMaterialTCoeffsTable()
        {
            string tableName = "isolation_material_tcoeffs";
            string[] colsArray = {
                                    "isolation_material_id INT UNSIGNED NOT NULL",
                                    "temperature FLOAT",
                                    "coeff_val FLOAT"
                                 };
            string[][] defaultValues =
{
                //Для полиэтилена
                new string[] {"1", "5", "0.1"},
                new string[] {"1", "6", "0.12"},
                new string[] {"1", "7", "0.15"},
                new string[] {"1", "8", "0.17"},
                new string[] {"1", "9", "0.19"},
                new string[] {"1", "10", "0.22"},
                new string[] {"1", "11", "0.26"},
                new string[] {"1", "12", "0.3"},
                new string[] {"1", "13", "0.35"},
                new string[] {"1", "14", "0.42"},
                new string[] {"1", "15", "0.48"},
                new string[] {"1", "16", "0.56"},
                new string[] {"1", "17", "0.64"},
                new string[] {"1", "18", "0.75"},
                new string[] {"1", "19", "0.87"},
                new string[] {"1", "20", "1"},
                new string[] {"1", "21", "1.17"},
                new string[] {"1", "22", "1.35"},
                new string[] {"1", "23", "1.57"},
                new string[] {"1", "24", "1.82"},
                new string[] {"1", "25", "2.1"},
                new string[] {"1", "26", "2.42"},
                new string[] {"1", "27", "2.83"},
                new string[] {"1", "28", "3.3"},
                new string[] {"1", "29", "3.82"},
                new string[] {"1", "30", "4.45"},
                new string[] {"1", "31", "5.2"},
                new string[] {"1", "32", "6"},
                new string[] {"1", "33", "6.82"},
                new string[] {"1", "34", "7.75"},
                new string[] {"1", "35", "8.8"},
                //Для резины
                new string[] {"2", "5", "0.5"},
                new string[] {"2", "6", "0.53"},
                new string[] {"2", "7", "0.55"},
                new string[] {"2", "8", "0.58"},
                new string[] {"2", "9", "0.61"},
                new string[] {"2", "10", "0.64"},
                new string[] {"2", "11", "0.68"},
                new string[] {"2", "12", "0.7"},
                new string[] {"2", "13", "0.73"},
                new string[] {"2", "14", "0.76"},
                new string[] {"2", "15", "0.8"},
                new string[] {"2", "16", "0.84"},
                new string[] {"2", "17", "0.88"},
                new string[] {"2", "18", "0.91"},
                new string[] {"2", "19", "0.96"},
                new string[] {"2", "20", "1"},
                new string[] {"2", "21", "1.05"},
                new string[] {"2", "22", "1.13"},
                new string[] {"2", "23", "1.2"},
                new string[] {"2", "24", "1.27"},
                new string[] {"2", "25", "1.35"},
                new string[] {"2", "26", "1.43"},
                new string[] {"2", "27", "1.52"},
                new string[] {"2", "28", "1.61"},
                new string[] {"2", "29", "1.71"},
                new string[] {"2", "30", "1.82"},
                new string[] {"2", "31", "1.93"},
                new string[] {"2", "32", "2.05"},
                new string[] {"2", "33", "2.18"},
                new string[] {"2", "34", "2.31"},
                new string[] {"2", "35", "2.46"},
                //Для пропитанной бумаги
                new string[] {"3", "5", "0.58"},
                new string[] {"3", "6", "0.6"},
                new string[] {"3", "7", "0.64"},
                new string[] {"3", "8", "0.67"},
                new string[] {"3", "9", "0.69"},
                new string[] {"3", "10", "0.72"},
                new string[] {"3", "11", "0.74"},
                new string[] {"3", "12", "0.76"},
                new string[] {"3", "13", "0.79"},
                new string[] {"3", "14", "0.82"},
                new string[] {"3", "15", "0.85"},
                new string[] {"3", "16", "0.87"},
                new string[] {"3", "17", "0.9"},
                new string[] {"3", "18", "0.93"},
                new string[] {"3", "19", "0.97"},
                new string[] {"3", "20", "1"},
                new string[] {"3", "21", "1.03"},
                new string[] {"3", "22", "1.07"},
                new string[] {"3", "23", "1.1"},
                new string[] {"3", "24", "1.14"},
                new string[] {"3", "25", "1.18"},
                new string[] {"3", "26", "1.22"},
                new string[] {"3", "27", "1.27"},
                new string[] {"3", "28", "1.32"},
                new string[] {"3", "29", "1.38"},
                new string[] {"3", "30", "1.44"},
                new string[] {"3", "31", "1.52"},
                new string[] {"3", "32", "1.59"},
                new string[] {"3", "33", "1.67"},
                new string[] {"3", "34", "1.77"},
                new string[] {"3", "35", "1.87"},
            };
            checkAndAddTable(tableName, colsArray);
            addBasicDataToTable(tableName, defaultValues);
        }
        /// <summary>
        /// Проверяет наличие БД db_sak на данном компьютере, если нет то создает ее и выбирает её
        /// </summary>
        /// <returns></returns>
        private static string checkAndCreateDBTera()
        {
            try
            {
                message = "Создаём базу данных db_norma, если она не создана";
                query = "CREATE DATABASE IF NOT EXISTS " + dbName + " DEFAULT CHARACTER SET cp1251";
                sendQuery();
                query = "USE " + dbName;
                sendQuery();
                return message;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка SQL запроса", MessageBoxButtons.OK);
                return ex.Message;
            }

        }

        /// <summary>
        /// Удаление БД. Функция необходимая на момент отладки программы.
        /// </summary>
        private static void dropDBTera()
        {
            query = "DROP DATABASE IF EXISTS " + dbName;
            sendQuery();
        }

        private static int sendQuery()
        {
            int sts = 0;
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, dbCon);
                dbCon.Open();
                sts = cmd.ExecuteNonQuery();
                dbCon.Close();
                return sts;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка SQL запроса", MessageBoxButtons.OK);
                return sts;
            }

        }

        /// <summary>
        /// Универсальный метод для проверки и добавления таблиц в БД
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        private static void checkAndAddTable(string tableName, string[] columns)
        {
            query = makeColumnsStringFromArray(columns);
            query = String.Format("CREATE TABLE IF NOT EXISTS {0} ({1})", tableName, query);
            sendQuery();
        }

        /// <summary>
        /// Добавляет начальные данные в заданную таблицу
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        private static void addBasicDataToTable(string tableName, string[][] data)
        {
            query = "select * from " + tableName;
            int sts = sendQuery();
            if (sts <= 0)
            {
                query = makeColumnsStringFromBiLevelArray(data);
                query = String.Format("INSERT IGNORE INTO {0} VALUES {1}", tableName, query);
                sendQuery();
            }
        }
        /// <summary>
        /// Создаёт из массива строк содержащего столбцы таблицы строку 
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string makeColumnsStringFromArray(string[] arr)
        {
            string cols = "";
            for (int i = 0; i < arr.Length; i++)
            {

                cols += " " + arr[i];
                if (i != arr.Length - 1)
                    cols += ",";
            }

            return cols;

        }

        /// <summary>
        /// Создаёт из двумерного массива параметров строку для передачи в запрос
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string makeColumnsStringFromBiLevelArray(string[][] arr)
        {
            string str = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null) break;
                str += "(" + makeColumnsStringFromArray(arr[i]) + ")";
                if (i != arr.Length - 1)
                    str += ", ";
            }
            return str;
        }




    }
}
