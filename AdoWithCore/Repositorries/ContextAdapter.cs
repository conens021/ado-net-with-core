using System.Data;
using System.Data.SqlClient;

namespace AdoWithCore.Repositorries
{
    public class ContextAdapter
    {
        private readonly IConfiguration _configuration;
        private SqlConnection _connection = default!;
        private SqlCommand _command = default!;
        private DataSet dataSet = new DataSet();
        public SqlDataAdapter adapter = default!;

        public enum CommandType
        {
            QUERY,
            PROCEDURE
        }

        public ContextAdapter(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            adapter = new SqlDataAdapter();
            _command = new SqlCommand();
            _command.Connection = _connection;
        }

        public SqlParameter AddQueryParam(string paramName, object? paramValue)
        {
            SqlParameter sqlParameter = _command.Parameters.AddWithValue(paramName, paramValue);
            return sqlParameter;
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

        public DataSet DataReadQuery(string query) {
            if (_connection == null) throw new Exception("Connection it's not set.");
            _command.CommandText = query;
            adapter.SelectCommand = _command;
            adapter.Fill(dataSet);
            return dataSet;
        }

        public DataSet DataUpdateQuery(int id,string query) {
        
            if (_connection == null) throw new Exception("Connection it's not set.");
            _command.CommandText = query;
            adapter.UpdateCommand = _command;
           
            adapter.Update(dataSet);
            return dataSet;
        }
    }
}
