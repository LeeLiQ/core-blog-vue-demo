using System;
using SqlSugar;

namespace blog_webapi_vue.Repository
{
    public class DbContext
    {
        private static string _connectionString;
        private static DbType _dbType;
        private SqlSugarClient _db;

        public static string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }
        public static DbType DbType
        {
            get => _dbType;
            set => _dbType = value;
        }
        public SqlSugarClient Db
        {
            get => _db;
            set => _db = value;
        }

        public static DbContext Context
        {
            get => new DbContext();
        }

        private DbContext()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new ArgumentNullException("Empty conncection string.");
            _db = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = true,
                IsShardSameThread = true,
                ConfigureExternalServices = new ConfigureExternalServices
                {

                },
                MoreSettings = new ConnMoreSettings
                {
                    IsAutoRemoveDataCache = true
                }
            });
        }

        private DbContext(bool blnIsAutoCloseConnection)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new ArgumentNullException("Empty conncection string.");
            _db = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection,
                IsShardSameThread = true,
                ConfigureExternalServices = new ConfigureExternalServices
                {

                },
                MoreSettings = new ConnMoreSettings
                {
                    IsAutoRemoveDataCache = true
                }
            });
        }

        public SimpleClient<T> GetEntityDB<T>()
            where T : class, new()
        {
            return new SimpleClient<T>(_db);
        }

        public SimpleClient<T> GetEntityDB<T>(SqlSugarClient db)
            where T : class, new()
        {
            return new SimpleClient<T>(_db);
        }

        public void CreateClassFileByDbTable(string strPath)
        {
            CreateClassFileByDbTable(strPath, "Km.PosZC");
        }

        public void CreateClassFileByDbTable(string strPath, string strNameSpace)
        {
            CreateClassFileByDbTable(strPath, strNameSpace, null);
        }

        public void CreateClassFileByDbTable(string strPath, string strNameSpace, string[] lstTableNames)
        {
            CreateClassFileByDBTalbe(strPath, strNameSpace, lstTableNames, string.Empty);
        }

        public void CreateClassFileByDBTalbe(string strPath, string strNameSpace, string[] lstTableNames, string strInterface, bool blnSerializable = false)
        {
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                _db.DbFirst
                    .Where(lstTableNames)
                    .IsCreateDefaultValue()
                    .IsCreateAttribute()
                    .SettingClassTemplate(p =>
                    p = @"{using}
                    namespace {Namespace}
                    {
                        {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
                        public partial class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
                        {
                            public {ClassName}()
                            {
                                {Constructor}
                            }
                            {PropertyName}
                        }
                    }")
                    .SettingPropertyTemplate(p =>
                    p = @"
                        {SugarColumn}
                        public {PropertyType} {PropertyName}
                        {
                            get
                            {
                                return _{PropertyName};
                            }
                            set
                            {
                                if(_{PropertyName}!=value)
                                {
                                    base.SetValueCall(" + "\"{PropertyName}\",_{PropertyName}" + @");
                                }
                                _{PropertyName}=value;
                            }
                        }")
                    .SettingPropertyDescriptionTemplate(p =>
                    p = "          private {PropertyType} _{PropertyName};\r\n" + p)
                    .SettingConstructorTemplate(p =>
                    p = "              this._{PropertyName} ={DefaultValue};")
                    .CreateClassFile(strPath, strNameSpace);
            }
            else
            {
                _db.DbFirst
                    .IsCreateAttribute()
                    .IsCreateDefaultValue()
                    .SettingClassTemplate(p =>
                    p = @"
                        {using}

                        namespace {Namespace}
                        {
                            {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
                            public partial class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
                            {
                                public {ClassName}()
                                {
                        {Constructor}
                                }
                        {PropertyName}
                            }
                        }
                        ")
                    .SettingPropertyTemplate(p =>
                    p = @"
                        {SugarColumn}
                        public {PropertyType} {PropertyName}
                        {
                            get
                            {
                                return _{PropertyName};
                            }
                            set
                            {
                                if(_{PropertyName}!=value)
                                {
                                    base.SetValueCall(" + "\"{PropertyName}\",_{PropertyName}" + @");
                                }
                                _{PropertyName}=value;
                            }
                        }")
                    .SettingPropertyDescriptionTemplate(p => p = "          private {PropertyType} _{PropertyName};\r\n" + p)
                    .SettingConstructorTemplate(p => p = "              this._{PropertyName} ={DefaultValue};")
                    .CreateClassFile(strPath, strNameSpace);
            }
        }

        public void CreateTableByEntity<T>(bool blnBackupTable, params T[] lstEntities)
            where T : class, new()
        {
            Type[] listTypes = null;
            if (lstEntities != null)
            {
                listTypes = new Type[lstEntities.Length];
                for (int i = 0; i < lstEntities.Length; i++)
                {
                    T t = lstEntities[i];
                    listTypes[i] = typeof(T);
                }
            }
            CreateTableByEntity(blnBackupTable, listTypes);
        }

        public void CreateTableByEntity(bool blnBackupTable, params Type[] lstEntities)
        {
            if (blnBackupTable)
            {
                _db.CodeFirst.BackupTable().InitTables(lstEntities); //change entity backupTable            
            }
            else
            {
                _db.CodeFirst.InitTables(lstEntities);
            }
        }

        #region [static method]
        public static DbContext GetDbContext(bool blnIsAutoCloseConnection = true)
        {
            return new DbContext(blnIsAutoCloseConnection);
        }

        public static void Init(string strConnectionString, DbType enmDbType = SqlSugar.DbType.SqlServer)
        {
            _connectionString = strConnectionString;
            _dbType = enmDbType;
        }

        public static ConnectionConfig GetConnectionConfig(bool blnIsAutoCloseConnection = true, bool blnIsShardSameThread = false)
        {
            ConnectionConfig config = new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //DataInfoCacheService = new HttpRuntimeCache()
                },
                IsShardSameThread = blnIsShardSameThread
            };
            return config;
        }

        public static SqlSugarClient GetCustomDB(ConnectionConfig config)
        {
            return new SqlSugarClient(config);
        }
        public static SimpleClient<T> GetCustomEntityDB<T>(SqlSugarClient sugarClient)
            where T : class, new()
        {
            return new SimpleClient<T>(sugarClient);
        }

        public static SimpleClient<T> GetCustomEntityDB<T>(ConnectionConfig config)
            where T : class, new()
        {
            SqlSugarClient sugarClient = GetCustomDB(config);
            return GetCustomEntityDB<T>(sugarClient);
        }
        #endregion
    }
}