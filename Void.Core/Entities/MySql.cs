namespace Void.Core.Entities
{
    public class MySql
    {
        /// <summary>
        /// IP Address of the Host
        /// </summary>
        private string MySqlHost { get; set; }

        /// <summary>
        /// The User the bot will log in as
        /// </summary>
        private string MySqlUser { get; set; }

        /// <summary>
        /// The Password Used to log in
        /// </summary>
        private string MySqlPass { get; set; }

        /// <summary>
        /// The Database the bot will access
        /// </summary>
        private string MySqlDb { get; set; }

        /// <summary>
        /// The SQL Server Port
        /// </summary>
        private int MySqlPort { get; set; }


        /// <summary>
        /// Generated Connection String
        /// </summary>
        /// <returns>Connection String</returns>
        public string GetConnectionString()
        {
            return $"Server={MySqlHost}; Database={MySqlUser}; User Id={MySqlPass}; Password={MySqlDb}; Port={MySqlPort};";
        }
    }
}