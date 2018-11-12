using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using blog_webapi_vue.IRepository.BASE;
using SqlSugar;

namespace blog_webapi_vue.Repository.BASE
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private DbContext _context;
        private SqlSugarClient _db;
        private SimpleClient<TEntity> _entityDB;

        public DbContext Context { get => _context; set => _context = value; }
        public SqlSugarClient Db { get => _db; set => _db = value; }
        public SimpleClient<TEntity> EntityDB { get => _entityDB; set => _entityDB = value; }
        public BaseRepository()
        {
            DbContext.Init(BaseDBConfig.ConnectionString);
            _context = DbContext.GetDbContext();
            _db = Context.Db;
            _entityDB = Context.GetEntityDB<TEntity>(_db);
        }

        public async Task<int> Add(TEntity model)
        {
            var i = await Task.Run(() => _db.Insertable(model).ExecuteReturnBigIdentity());
            return (int)i;
        }

        public async Task<bool> Delete(TEntity model)
        {
            var i = await Task.Run(() => _db.Deleteable(model).ExecuteCommand());
            return i > 0;
        }

        public async Task<bool> DeleteById(object id)
        {
            var i = await Task.Run(() => _db.Deleteable<TEntity>(id).ExecuteCommand());
            return i > 0;
        }

        public async Task<bool> DeleteByIds(object[] ids)
        {
            var i = await Task.Run(() => _db.Deleteable<TEntity>().In(ids).ExecuteCommand());
            return i > 0;
        }

        public async Task<List<TEntity>> Query()
        {
            return await Task.Run(() => _entityDB.GetList());
        }

        public async Task<List<TEntity>> Query(string strWhere)
        {
            return await Task.Run(() => _db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Task.Run(() => _entityDB.GetList(whereExpression));
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToList());
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToList());
        }

        public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds)
        {
            return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToList());
        }

        public async Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds)
        {
            return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToList());
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds)
        {
            return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToPageList(intPageIndex, intPageSize));
        }

        public async Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds)
        {
            return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageList(intPageIndex, intPageSize));
        }

        public async Task<TEntity> QueryById(object objId)
        {
            return await Task.Run(()=>_db.Queryable<TEntity>().InSingle(objId));
        }

        public async Task<TEntity> QueryByID(object objId, bool blnUseCache = false)
        {
            return await Task.Run(() => _db.Queryable<TEntity>().WithCacheIF(blnUseCache).InSingle(objId));
        }

        public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
        {
            return await Task.Run(() => _db.Queryable<TEntity>().In(lstIds).ToList());
        }

        public async Task<List<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 0, int intPageSize = 20, string strOrderByFileds = null)
        {
            return await Task.Run(() => _db.Queryable<TEntity>()
            .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
            .WhereIF(whereExpression != null, whereExpression)
            .ToPageList(intPageIndex, intPageSize));
        }

        public async Task<bool> Update(TEntity model)
        {
            var i = await Task.Run(() => _db.Updateable(model).ExecuteCommand());
            return i > 0;
        }

        public async Task<bool> Update(TEntity entity, string strWhere)
        {
            return await Task.Run(() => _db.Updateable(entity).Where(strWhere).ExecuteCommand() > 0);
        }

        public async Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
        {
            IUpdateable<TEntity> up = await Task.Run(() => _db.Updateable(entity));
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            {
                up = await Task.Run(() => up.IgnoreColumns(it => lstIgnoreColumns.Contains(it)));
            }
            if (lstColumns != null && lstColumns.Count > 0)
            {
                up = await Task.Run(() => up.UpdateColumns(it => lstColumns.Contains(it)));
            }
            if (!string.IsNullOrEmpty(strWhere))
            {
                up = await Task.Run(() => up.Where(strWhere));
            }
            return await Task.Run(() => up.ExecuteCommand()) > 0;
        }

        /// <summary>
        /// For some reason, this method is not in the interface.
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<bool> Update(string strSql, SugarParameter[] parameters = null)
        {
            return await Task.Run(() => _db.Ado.ExecuteCommand(strSql, parameters) > 0);
        }
    }
}