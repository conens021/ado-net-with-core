using System.Data.SqlClient;

namespace AdoWithCore.Repositorries
{
    public class RepositoryContext
    {

        private readonly IConfiguration _configuration;
        private SqlConnection _connection = default!;
        public SqlCommand _command = default!;

        private bool isOpen = false;

        public enum CommandType
        {
            QUERY,
            PROCEDURE
        }

        public RepositoryContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            _command = new SqlCommand();
            _command.Connection = _connection;
        }

        public SqlDataReader DataReadQuery(String query)
        {
            if (!isOpen)
            {
                _connection.Open();
                isOpen = true;
            }
            _command.CommandText = query;
            SqlDataReader dataReader = _command.ExecuteReader();
            return dataReader;
        }

    

        public SqlDataReader DataReadQuery()
        {
            if (!isOpen)

            {
                _connection.Open();
                isOpen = true;
            }
            SqlDataReader dataReader = _command.ExecuteReader();
            return dataReader;
        }

        public SqlWriteResponse DataWriteQuery(String query)
        {
            if (!isOpen)
            {
                _connection.Open();
                isOpen = true;
            }
            _command.CommandText = query;
            return new SqlWriteResponse() { RowsAffected = _command.ExecuteNonQuery() };

        }

        public SqlWriteResponse DataWriteQuery()
        {
            if (!isOpen)
            {
                _connection.Open();
                isOpen = true;
            }

            return new SqlWriteResponse() { RowsAffected = _command.ExecuteNonQuery() };

        }


        public bool OpenConnection()
        {
            if (!isOpen)
            {
                string db_connection = _configuration.GetConnectionString("DefaultConnection");
                _connection = new SqlConnection(db_connection);
                _connection.Open();
                isOpen = true;
                return true;
            }
            _connection.Close();
            isOpen = false;
            return false;
        }

        public SqlCommand CreateCommand()
        {
            if (!isOpen)
            {
                OpenConnection();
                isOpen = true;
            }
            _command = new SqlCommand();
            _command.Connection = _connection;
            return _command;
        }

        public void AddQueryParam(string paramName, object? paramValue)
        {
            this._command.Parameters.AddWithValue(paramName, paramValue);
            
        }

        public SqlParameter AddOutputParam(string paramName, System.Data.SqlDbType type)
        {
            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = paramName;
            sqlParam.SqlDbType = type;
            sqlParam.Direction = System.Data.ParameterDirection.Output;
            _command.Parameters.Add(sqlParam);
            return sqlParam;
        }

        public void Clean()
        {
            if (isOpen)
            {
                _connection.Close();
                _command.Dispose();
                isOpen = false;

            }
            else throw new Exception("Connection is not open!");
        }

        public void SetQuery(string query)
        {

            this._command.CommandText = query;
        }

        public void SetCommandType(CommandType type)
        {

            switch (type)
            {

                case CommandType.PROCEDURE:
                    _command.CommandType = System.Data.CommandType.StoredProcedure;
                    break;
                case CommandType.QUERY:
                    _command.CommandType = System.Data.CommandType.Text;
                    break;
                default: throw new Exception("Wrong type.");
            }

        }


    }
}
