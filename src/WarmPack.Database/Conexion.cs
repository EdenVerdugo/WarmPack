using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using WarmPack.Classes;
using WarmPack.Data;
using WarmPack.Database.Helpers;
using WarmPack.Database.Schema;
using WarmPack.Extensions;

namespace WarmPack.Database
{
    public class Conexion
    {
        internal string ThrowExceptionExecuteMessage = "An error occurred while executing the query.\n\nException: ";
        internal string ThrowExceptionRecordsetRead = "There is nothing more recordsets to read.";

        internal ConexionType _conexionType;
        internal IDbConnection _conexion;
        internal IDbTransaction _transaction;

        internal IDbCommand _commandRecordsets;
        internal IDataReader _readerRecordsets;
        internal ConexionParameters _parametersRecordsets;

        internal ConnectionString _connectionString;
        public ConnectionString ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        internal bool _closeConnection = true;

        public int ConexionTimeOut { get; set; }

        public ConexionSchema DbSchema { get; set; }

        public bool IsMoreRecordsets { get; set; }

        public delegate void ConexionInfoMessageHandler(SqlInfoMessageEventArgs args);

        public event ConexionInfoMessageHandler InfoMessage;

        protected void ConexionInit(ConexionType conexionType, string connectionString)
        {
            ConexionTimeOut = 30;
            _conexionType = conexionType;

            switch (_conexionType)
            {
                case ConexionType.MSSQLServer:

                    _conexion = new SqlConnection(connectionString);
                    var csb = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
                    _connectionString = new ConnectionString(csb.DataSource, csb.InitialCatalog, csb.UserID, csb.Password);
                    
                    break;
                    //case ConexionType.PostgreSQL:

                    //    _conexion = new PgSqlConnection(conectionString);

                    //    break;
            }

            DbSchema = new ConexionSchema(this, conexionType);

            //ConexionHelper = new ConexionHelper(this, conexionType)
            //{
            //    ConectionString = conectionString,
            //    DbConnection = _conexion
            //};
        }

        private void Conexion_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            InfoMessage?.Invoke(e);
        }

        public string ConnectionStringBuilder(ConexionType conexionType, string server, string db, string user, string password)
        {
            var connectionString = "";

            switch (_conexionType)
            {
                case ConexionType.MSSQLServer:

                    SqlConnectionStringBuilder sqlbuilder = new SqlConnectionStringBuilder
                    {
                        DataSource = server,
                        InitialCatalog = db,
                        UserID = user,
                        Password = password
                    };

                    connectionString = sqlbuilder.ConnectionString;
                    _connectionString = new ConnectionString(sqlbuilder.DataSource, sqlbuilder.InitialCatalog, sqlbuilder.UserID, sqlbuilder.Password);

                    break;
                    //case ConexionType.PostgreSQL:

                    //    PgSqlConnectionStringBuilder pgbuilder = new PgSqlConnectionStringBuilder();

                    //    pgbuilder.Host = server;
                    //    pgbuilder.Database = db;
                    //    pgbuilder.UserId = user;
                    //    pgbuilder.Password = password;

                    //    connectionString = pgbuilder.ConnectionString;

                    //    break;
            }

            return connectionString;
        }

        public Conexion(ConexionType conexionType, string server, string db, string user, string password)
        {
            ConexionInit(conexionType, ConnectionStringBuilder(conexionType, server, db, user, password));
        }

        public Conexion(ConexionType conexionType, string connectionString)
        {
            ConexionInit(conexionType, connectionString);
        }

        public Conexion(ConexionType conexionType, ConnectionString connectionString)
        {
            ConexionInit(conexionType, this.ConnectionStringBuilder(conexionType, connectionString.Server, connectionString.Database, connectionString.User, connectionString.Password));
        }

        public void TransactionBegin()
        {
            ConexionOpen();
            _transaction = this._conexion.BeginTransaction();
        }

        public void TransactionCommit()
        {
            TransactionCommit(true);
        }

        public void TransactionCommit(bool closeConexion = true)
        {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;

            if (closeConexion)
                _conexion.Close();
        }

        public void TransactionRollback()
        {
            TransactionRollback(true);
        }

        public void TransactionRollback(bool closeConexion = true)
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;

            if (closeConexion)
                _conexion.Close();
        }

        private Result ResultBuilder(ConexionParameters parameters)
        {
            var result = new Result(true, "the query has been executed correctly");

            var resultParameter = parameters?.Parameters?.FirstOrDefault(p => p.Name.ToLower() == "@result" || p.Name.ToLower() == "@presult" || p.Name.ToLower() == "@resultado" || p.Name.ToLower() == "@presultado");
            if (resultParameter != null)
            {
                if(resultParameter.Value.ToString() == "")
                {
                    throw new Exception($"The '{resultParameter.Name}' parameter was not initialized in the stored procedure");
                }

                result.Value = new Castable(resultParameter.Value).ToBoolean();
            }                

            var messageParameter = parameters?.Parameters?.FirstOrDefault(p => p.Name.ToLower() == "@msg" || p.Name.ToLower() == "@pmsg" || p.Name.ToLower() == "@message" || p.Name.ToLower() == "@pmessage" || p.Name.ToLower() == "@mensaje" || p.Name.ToLower() == "@pmensaje");
            if (messageParameter != null)
            {
                result.Message = new Castable(messageParameter.Value).ToString();
            }                

            var codeParameter = parameters?.Parameters?.FirstOrDefault(p => p.Name.ToLower() == "@codigo" || p.Name.ToLower() == "@pcodigo" || p.Name.ToLower() == "@code" || p.Name.ToLower() == "@pcode");
            if (codeParameter != null)
            {
                if(codeParameter.Value.ToString() == "")
                {
                    throw new Exception($"The '{codeParameter.Name}' parameter was not initialized in the stored procedure");
                }

                result.Code = new Castable(codeParameter.Value).ToInt32();
            }                

            return result;
        }

        private Result<T> ResultBuilder<T>(ConexionParameters parameters)
        {
            var r = ResultBuilder(parameters);

            var result = new Result<T>()
            {
                Value = r.Value,
                Message = r.Message
            };

            return result;
        }

        private void ConexionOpen()
        {
            if (_transaction == null && this._conexion.State != ConnectionState.Open)
            {
                this._conexion.Open();

                AddConexionInfoMessageEventHandler();
            }
        }

        private void ConexionClose()
        {
            if (_transaction == null && _closeConnection)
            {
                this._conexion.Close();
                RemoveConexionInfoMessageEventHandler();
            }
        }

        private void AddConexionInfoMessageEventHandler()
        {
            (this._conexion as SqlConnection).InfoMessage += Conexion_InfoMessage;
        }

        private void RemoveConexionInfoMessageEventHandler()
        {
            (this._conexion as SqlConnection).InfoMessage -= Conexion_InfoMessage;
        }

        public Result Execute(string query)
        {
             return Execute(query, null);
        }

        public Result Execute(string query, ConexionParameters parameters)
        {
            try
            {
                ConexionOpen();                                  

                using (var cmd = this._conexion.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Transaction = _transaction;
                    cmd.CommandTimeout = ConexionTimeOut;

                    if (parameters != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var p in parameters.Parameters)
                        {
                            var pt = cmd.CreateParameter();
                            pt.ParameterName = p.Name;
                            pt.DbType = p.Type;
                            pt.Value = p.Value;
                            pt.Size = p.Size;
                            pt.Direction = p.Direction;

                            cmd.Parameters.Add(pt);
                        }
                    }

                    var rows = cmd.ExecuteNonQuery();

                    if (parameters != null)
                    {
                        foreach (IDbDataParameter p in cmd.Parameters)
                        {
                            var param = parameters.Parameters.FirstOrDefault(x => x.Name == p.ParameterName);
                            if (param != null)
                            {
                                param.Value = p.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ThrowExceptionExecuteMessage + ex.Message, ex);
            }
            finally
            {
                ConexionClose();                    
            }

            return ResultBuilder(parameters);
        }

        public Result ExecuteWithResults(string query, out DataTable dt)
        {
            return ExecuteWithResults(query, null, out dt);
        }

        public Result ExecuteWithResults(string query, ConexionParameters parameters, out DataTable dt)
        {
            try
            {
                ConexionOpen();

                using (var cmd = this._conexion.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Transaction = _transaction;
                    cmd.CommandTimeout = ConexionTimeOut;

                    if (parameters != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var p in parameters.Parameters)
                        {
                            var pt = cmd.CreateParameter();
                            pt.ParameterName = p.Name;
                            pt.DbType = p.Type;
                            pt.Value = p.Value;
                            pt.Size = p.Size;
                            pt.Direction = p.Direction;

                            cmd.Parameters.Add(pt);
                        }
                    }

                    dt = new DataTable();

                    switch (this._conexionType)
                    {
                        case ConexionType.MSSQLServer:
                            var sqlAdapter = new SqlDataAdapter((SqlCommand)cmd);
                            sqlAdapter.Fill(dt);
                            break;
                            //case ConexionType.PostgreSQL:
                            //    var pgAdapter = new PgSqlDataAdapter((PgSqlCommand)cmd);
                            //    pgAdapter.Fill(dt);
                            //    break;
                    }

                    if (parameters != null)
                    {
                        foreach (IDbDataParameter p in cmd.Parameters)
                        {
                            var param = parameters.Parameters.FirstOrDefault(x => x.Name == p.ParameterName);
                            if (param != null)
                            {
                                param.Value = p.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ThrowExceptionExecuteMessage + ex.Message, ex);
            }
            finally
            {
                ConexionClose();
            }

            return ResultBuilder(parameters);
        }

        public Result ExecuteWithResults(string query, out DataSet ds)
        {
            return ExecuteWithResults(query, null, out ds);
        }

        public Result ExecuteWithResults(string query, ConexionParameters parameters, out DataSet ds)
        {
            try
            {
                ConexionOpen();

                using (var cmd = this._conexion.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Transaction = _transaction;
                    cmd.CommandTimeout = ConexionTimeOut;

                    if (parameters != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var p in parameters.Parameters)
                        {
                            var pt = cmd.CreateParameter();
                            pt.ParameterName = p.Name;
                            pt.DbType = p.Type;
                            pt.Value = p.Value;
                            pt.Size = p.Size;
                            pt.Direction = p.Direction;

                            cmd.Parameters.Add(pt);
                        }
                    }

                    ds = new DataSet();

                    switch (this._conexionType)
                    {
                        case ConexionType.MSSQLServer:
                            var sqlAdapter = new SqlDataAdapter((SqlCommand)cmd);
                            sqlAdapter.Fill(ds);
                            break;
                            //case ConexionType.PostgreSQL:
                            //    var pgAdapter = new PgSqlDataAdapter((PgSqlCommand)cmd);
                            //    pgAdapter.Fill(dt);
                            //    break;
                    }

                    if (parameters != null)
                    {
                        foreach (IDbDataParameter p in cmd.Parameters)
                        {
                            var param = parameters.Parameters.FirstOrDefault(x => x.Name == p.ParameterName);
                            if (param != null)
                            {
                                param.Value = p.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ThrowExceptionExecuteMessage + ex.Message, ex);
            }
            finally
            {
                ConexionClose();
            }

            return ResultBuilder(parameters);
        }

        public Result<T> ExecuteScalar<T>(string query)
        {
            return ExecuteScalar<T>(query, null);
        }

        public Result<T> ExecuteScalar<T>(string storeProcedure, ConexionParameters parameters)
        {
            object scalar;
            try
            {
                ConexionOpen();

                using (var cmd = this._conexion.CreateCommand())
                {
                    cmd.CommandText = storeProcedure;
                    cmd.Transaction = _transaction;
                    cmd.CommandTimeout = ConexionTimeOut;

                    if (parameters != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var p in parameters.Parameters)
                        {
                            var pt = cmd.CreateParameter();
                            pt.ParameterName = p.Name;
                            pt.DbType = p.Type;
                            pt.Value = p.Value;
                            pt.Size = p.Size;
                            pt.Direction = p.Direction;

                            cmd.Parameters.Add(pt);
                        }
                    }

                    scalar = cmd.ExecuteScalar();

                    if (parameters != null)
                    {
                        foreach (IDbDataParameter p in cmd.Parameters)
                        {
                            var param = parameters.Parameters.FirstOrDefault(x => x.Name == p.ParameterName);
                            if (param != null)
                            {
                                param.Value = p.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ThrowExceptionExecuteMessage + ex.Message, ex);
            }
            finally
            {
                ConexionClose();
            }

            var result = ResultBuilder<T>(parameters);

            try
            {
                var type = typeof(Result<T>);

                foreach (PropertyInfo pro in type.GetProperties())
                {
                    string propertyName = pro.Name.ToLowerInvariant();

                    if (propertyName == "data")
                    {
                        pro.SetValue(result, scalar, null);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }

        public Castable ExecuteScalar(string query)
        {
            return ExecuteScalar(query, null);
        }

        public Castable ExecuteScalar(string storeProcedure, ConexionParameters parameters)
        {
            Castable result;
            try
            {
                ConexionOpen();

                using (var cmd = this._conexion.CreateCommand())
                {
                    cmd.CommandText = storeProcedure;
                    cmd.Transaction = _transaction;
                    cmd.CommandTimeout = ConexionTimeOut;

                    if (parameters != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var p in parameters.Parameters)
                        {
                            var pt = cmd.CreateParameter();
                            pt.ParameterName = p.Name;
                            pt.DbType = p.Type;
                            pt.Value = p.Value;
                            pt.Size = p.Size;
                            pt.Direction = p.Direction;

                            cmd.Parameters.Add(pt);
                        }
                    }

                    var scalar = cmd.ExecuteScalar();
                    result = new Castable(scalar);

                    if (parameters != null)
                    {
                        foreach (IDbDataParameter p in cmd.Parameters)
                        {
                            var param = parameters.Parameters.FirstOrDefault(x => x.Name == p.ParameterName);
                            if (param != null)
                            {
                                param.Value = p.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ThrowExceptionExecuteMessage + ex.Message, ex);
            }
            finally
            {
                ConexionClose();
            }

            return result;
        }

        public Result<T> ExecuteWithResultsToObject<T>(string query)
        {
            return ExecuteWithResultsToObject<T>(query, null);
        }

        public Result<T> ExecuteWithResultsToObject<T>(string query, ConexionParameters parameters)
        {
            var results = ExecuteWithResults<T>(query, parameters);

            return new Result<T>()
            {
                Value = results.Value,
                Message = results.Message,
                Code = results.Code,
                Data = results.Data.FirstOrDefault()
            };
        }

        public Result<List<T>> ExecuteWithResults<T>(string query, out List<T> results)
        {
            var r = ExecuteWithResults<T>(query);
            results = r.Data as List<T>;

            return r;
        }

        public Result<List<T>> ExecuteWithResults<T>(string query, ConexionParameters parameters, out List<T> results)
        {
            var r = ExecuteWithResults<T>(query, parameters);
            results = r.Data as List<T>;

            return r;
        }

        public Result<List<T>> ExecuteWithResults<T>(string query)
        {
            return ExecuteWithResults<T>(query, null);
        }

        public Result<List<T>> ExecuteWithResults<T>(string query, ConexionParameters parameters)
        {
            List<T> lst = new List<T>();

            try
            {
                ConexionOpen();                   

                using (var cmd = this._conexion.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Transaction = _transaction;
                    cmd.CommandTimeout = ConexionTimeOut;

                    if (parameters != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var p in parameters.Parameters)
                        {
                            var pt = cmd.CreateParameter();
                            pt.ParameterName = p.Name;
                            pt.DbType = p.Type;
                            pt.Value = p.Value;
                            pt.Size = p.Size;
                            pt.Direction = p.Direction;

                            cmd.Parameters.Add(pt);
                        }
                    }

                    var lector = cmd.ExecuteReader();

                    lst = ResultsInDataReader<T>(ref lector);

                    lector.Close();

                    if (parameters != null)
                    {
                        foreach (IDbDataParameter p in cmd.Parameters)
                        {
                            var param = parameters.Parameters.FirstOrDefault(x => x.Name == p.ParameterName);
                            if (param != null)
                            {
                                param.Value = p.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ThrowExceptionExecuteMessage + ex.Message, ex);
            }
            finally
            {
                ConexionClose();
            }

            var result = ResultBuilder<List<T>>(parameters);
            result.Data = lst;

            return result;
        }        

        public void ExecuteWithResults(string query, Action<ConexionCastableRow> action)
        {
            ExecuteWithResults(query, null, action);
        }

        public void ExecuteWithResults(string query, ConexionParameters parameters, Action<ConexionCastableRow> action)
        {
            try
            {
                ConexionOpen();

                using (var cmd = this._conexion.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Transaction = _transaction;
                    cmd.CommandTimeout = ConexionTimeOut;

                    if (parameters != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var p in parameters.Parameters)
                        {
                            var pt = cmd.CreateParameter();
                            pt.ParameterName = p.Name;
                            pt.DbType = p.Type;
                            pt.Value = p.Value;
                            pt.Size = p.Size;
                            pt.Direction = p.Direction;

                            cmd.Parameters.Add(pt);
                        }
                    }

                    var lector = cmd.ExecuteReader();


                    while (lector.Read())
                    {
                        var castableRow = new ConexionCastableRow(lector);

                        action(castableRow);
                    }

                    lector.Close();

                    if (parameters != null)
                    {
                        foreach (IDbDataParameter p in cmd.Parameters)
                        {
                            var param = parameters.Parameters.FirstOrDefault(x => x.Name == p.ParameterName);
                            if (param != null)
                            {
                                param.Value = p.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ThrowExceptionExecuteMessage + ex.Message, ex);
            }
            finally
            {
                ConexionClose();
            }
        }

        public bool RecordsetsExecute(string query)
        {
            return RecordsetsExecute(query, null);
        }

        public bool RecordsetsExecute(string storeProcedure, ConexionParameters parameters)
        {
            bool result = false;

            try
            {
                ConexionOpen();

                this._commandRecordsets = this._conexion.CreateCommand();

                _commandRecordsets.CommandText = storeProcedure;
                _commandRecordsets.Transaction = _transaction;
                _commandRecordsets.CommandTimeout = ConexionTimeOut;

                if (parameters != null)
                {
                    _parametersRecordsets = parameters;

                    _commandRecordsets.CommandType = CommandType.StoredProcedure;

                    foreach (var p in parameters.Parameters)
                    {
                        var pt = _commandRecordsets.CreateParameter();
                        pt.ParameterName = p.Name;
                        pt.DbType = p.Type;
                        pt.Value = p.Value;
                        pt.Size = p.Size;
                        pt.Direction = p.Direction;

                        _commandRecordsets.Parameters.Add(pt);
                    }
                }

                _readerRecordsets = _commandRecordsets.ExecuteReader();                

                switch (this._conexionType)
                {
                    case ConexionType.MSSQLServer:                                                
                        result = ((SqlDataReader)_readerRecordsets).HasRows;
                        break;
                        //case ConexionType.PostgreSQL:
                        //    result = ((PgSqlDataReader)_readerRecordsets).HasRows;
                        //    break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ThrowExceptionExecuteMessage + ex.Message, ex);
            }

            if (!result)
            {
                IsMoreRecordsets = _readerRecordsets.NextResult();
            }

            return result;
        }

        public T RecordsetsResultsToObject<T>()
        {
            var r = RecordsetsResults<T>();

            return r.FirstOrDefault();
        }

        public List<T> RecordsetsResults<T>()
        {
            if (_readerRecordsets == null || _readerRecordsets.IsClosed)
            {
                throw new Exception(ThrowExceptionRecordsetRead);
            }

            var lst = ResultsInDataReader<T>(ref _readerRecordsets);

            IsMoreRecordsets = _readerRecordsets.NextResult();

            if (!IsMoreRecordsets)
            {
                _readerRecordsets.Close();

                if (_parametersRecordsets != null)
                {
                    foreach (IDbDataParameter p in _commandRecordsets.Parameters)
                    {
                        var param = _parametersRecordsets.Parameters.FirstOrDefault(x => x.Name == p.ParameterName);
                        if (param != null)
                        {
                            param.Value = p.Value;
                        }
                    }
                }

                _readerRecordsets.Dispose();
                _readerRecordsets = null;

                ConexionClose();
            }
            
            return lst;
        }

        public void RecordsetsResults(Action<ConexionCastableRow> action)
        {
            if (_readerRecordsets == null || _readerRecordsets.IsClosed)
            {
                throw new Exception(ThrowExceptionRecordsetRead);
            }

            while (_readerRecordsets.Read())
            {
                action(new ConexionCastableRow(_readerRecordsets));
            }

            IsMoreRecordsets = _readerRecordsets.NextResult();

            if (!IsMoreRecordsets)
            {
                _readerRecordsets.Close();

                if (_parametersRecordsets != null)
                {
                    foreach (IDbDataParameter p in _commandRecordsets.Parameters)
                    {
                        var param = _parametersRecordsets.Parameters.FirstOrDefault(x => x.Name == p.ParameterName);
                        if (param != null)
                        {
                            param.Value = p.Value;
                        }
                    }
                }

                _readerRecordsets.Dispose();
                _readerRecordsets = null;

                ConexionClose();
            }
        }

        private List<T> ResultsInDataReader<T>(ref IDataReader reader)
        {
            List<T> lst = null; //new List<T>();
            string currentColumn = null;

            try
            {
                Type temp = typeof(T);

                while (reader.Read())
                {
                    //crear la instancia del objeto 
                    T obj = default(T);
                    bool isPrimitive = true;
                    if (temp != typeof(string) && temp != typeof(int) && temp != typeof(decimal) && temp != typeof(long) && temp != typeof(bool) && temp != typeof(short) && temp != typeof(float) && temp != typeof(double) && temp != typeof(char))
                    {
                        obj = Activator.CreateInstance<T>();
                        isPrimitive = false;
                    }


                    //buscar todos sus campos y propiedades
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader[i].GetType() == typeof(System.DBNull))
                            continue;

                        if (isPrimitive)
                        {
                            obj = (T)reader[i];
                            continue;
                        }

                        foreach (FieldInfo pro in temp.GetFields())
                        {
                            string name = pro.Name.ToLowerInvariant();
                            string columnName = reader.GetName(i).ToLowerInvariant();
                            currentColumn = columnName;

                            var attributes = pro.GetCustomAttributes(true);

                            foreach (var a in attributes)
                            {
                                if (a is ConexionColumnAttribute)
                                {
                                    name = ((ConexionColumnAttribute)a).Name.ToLowerInvariant();
                                }
                            }
                            // si se encuentra se asigna el valor encontrado
                            // nota : esto puede causar una excepcion si el tipo de valor que se encuentra no es el mismo tipo que regresa la consulta
                            if (name == columnName)
                            {
                                pro.SetValue(obj, reader[i]);
                            }
                            else
                            {
                                continue;
                            }
                        }

                        foreach (PropertyInfo pro in temp.GetProperties())
                        {
                            string propertyName = pro.Name.ToLowerInvariant();
                            string columnName = reader.GetName(i).ToLowerInvariant();
                            currentColumn = columnName;

                            var attributes = pro.GetCustomAttributes(true);

                            foreach (var a in attributes)
                            {
                                if (a is ConexionColumnAttribute)
                                {
                                    propertyName = ((ConexionColumnAttribute)a).Name.ToLowerInvariant();
                                }
                            }

                            if (propertyName == columnName)
                            {
                                pro.SetValue(obj, reader[i], null);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    
                    lst?.Add(obj);
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Falló al serializar la columna '{currentColumn}'.\n{ex.Message}", ex);                
            }            

            return lst;
        }

        public bool Test()
        {
            try
            {
                return ExecuteScalar("SELECT 1").ToInt32() == 1;
            }
            catch
            {
                return false;
            }            
        }

        public void ChangeDatabase(string database)
        {
            ConnectionString.Database = database;

            if (_conexion != null)
            {
                _conexion.Dispose();                
            }

            ConexionInit(_conexionType, _conexionType == ConexionType.MSSQLServer ? ConnectionString.ToMsqlConnectionString() : null);
        }

        public Result[] ExecuteScript(string script)
        {
            List<Result> result = new List<Result>();
            var scripts = script.Split(new string[] { "\r\nGO\r\n", "\r\nGO\t", " GO ", "\tGO\t", " go ", "\r\ngo\r\n", "\tgo\t" }, StringSplitOptions.RemoveEmptyEntries);

            scripts
            //.Map(item =>
            //{
            //    return item.Replace("\r", "").Replace("\n", "");
            //})
            .ForEach(scriptText =>
            {
                try
                {
                    var r = this.Execute(scriptText);
                    r.Data = scriptText;
                    result.Add(r);
                }
                catch(Exception ex)
                {
                    result.Add(new Result(ex));
                }
                
            });

            return result.ToArray();
        }


    }
}
