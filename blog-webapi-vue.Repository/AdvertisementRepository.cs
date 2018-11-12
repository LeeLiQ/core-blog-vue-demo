using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using blog_webapi_vue.IRepository;
using blog_webapi_vue.Model;
using blog_webapi_vue.Repository.BASE;
using SqlSugar;

namespace blog_webapi_vue.Repository
{
    public class AdvertisementRepository : BaseRepository<Advertisement>, IAdvertisementRepository
    {
        // private DbContext _context;
        // private SqlSugarClient _db;
        // private SimpleClient<Advertisement> entityDB;

        // public SqlSugarClient Db { get => _db; set => _db = value; }
        // public DbContext Context { get => _context; set => _context = value; }

        // public AdvertisementRepository()
        // {
        //     DbContext.Init(BaseDBConfig.ConnectionString);
        //     _context = DbContext.GetDbContext();
        //     _db = _context.Db;
        //     entityDB = _context.GetEntityDB<Advertisement>(_db);
        // }

        // public int Add(Advertisement model)
        // {
        //     var i = _db.Insertable(model).ExecuteReturnBigIdentity();
        //     return i.ObjectToInt();
        // }

        // public bool Delete(Advertisement model)
        // {
        //     var i = _db.Deleteable(model).ExecuteCommand();
        //     return i > 0;
        // }

        // public List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression)
        // {
        //     return entityDB.GetList(whereExpression);

        // }

        // public int Sum(int i, int j)
        // {
        //     return i + j;
        // }

        // public bool Update(Advertisement model)
        // {
        //     var i = _db.Updateable(model).ExecuteCommand();
        //     return i > 0;
        // }
        public int Sum(int a, int b)
        {
            return a + b;
        }
    }
}