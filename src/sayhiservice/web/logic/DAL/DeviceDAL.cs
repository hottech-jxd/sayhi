using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using logic.Model;
using System.Data.SqlClient;
using common;

namespace logic.DAL
{
    public  class DeviceDAL
    {
        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddDevice(DeviceModel model)
        {
            string strSql = string.Format(@"insert into sayhi_device(devicename,deviceno,osversion,devicetype,createtime,brand) values(@devicename,@deviceno,@osversion,@devicetype,@createtime,@brand);select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@devicename", model.devicename),
					new SqlParameter("@deviceno",model.deviceno),
					new SqlParameter("@osversion", model.osversion),
                    new SqlParameter("@devicetype", model.devicetype),
                    new SqlParameter("@createtime",model.createtime),
                    new SqlParameter("@brand",model.brand)
                                        };
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }


        public DeviceModel GetDeviceByDeviceNo(string deviceno)
        {
            string strSql = string.Format(@"select * from sayhi_device where deviceno=@deviceno");
            SqlParameter[] parameters ={
                new SqlParameter("@deviceno", deviceno)
                                       };

            DeviceModel model = new DeviceModel();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader( strSql , parameters))
            {
                model = DbHelperSQL.GetEntity<DeviceModel>(dr);
            }
            return model;
        }

        public List<DeviceModel> GetDeviceList(int pageIndex, int pageSize, out int recordCount,  DeviceWhere where)
        {
            string sql = @"select * from sayhi_device where 1=1 ";

            if (where != null)
            {                
                if (!string.IsNullOrEmpty(where.deviename))
                    sql += string.Format(" and deviename='{0}' ", where.deviename);
                if (!string.IsNullOrEmpty(where.deviceno))
                    sql += string.Format(" and deviceno='{0}' ", where.deviceno);
                if (!string.IsNullOrEmpty(where.osversion))
                    sql += string.Format(" and osversion='{0}'", where.osversion);
                if (!string.IsNullOrEmpty(where.brand))
                    sql+=string.Format(" and brand='{0}'", where.brand);
                
            }

               
            string querySql = DbHelperSQL.buildPageSql(pageIndex, pageSize, sql, "createtime", true);
            string recordCountSql = DbHelperSQL.buildRecordCountSql(sql);
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(recordCountSql));
            List<DeviceModel> lst = new List<DeviceModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(querySql))
            {
                lst = DbHelperSQL.GetEntityList<DeviceModel>(dr);
            }
            return lst;
        }


        public bool ExistByDeviceNo(String deviceno)
        {
            string strSql = string.Format("select count(1) from sayhi_device where deviceno=@deviceno");
            SqlParameter[] parameters ={
                                           new SqlParameter("@deviceno", deviceno)
                                      };

            return DbHelperSQL.Exists(strSql, parameters);
        }

        public void UpdateDeviceByDeviceNo(DeviceModel model)
        {
            string strSql = string.Format("update sayhi_device set devicename=@devicename, devicetype=@devicetype,osversion=@osversion,brand=@brand where deviceno=@deviceno");
            SqlParameter[] parameters ={
                                           new SqlParameter("@devicename",model.devicename),
                                           new SqlParameter("@devicetype",model.devicetype),
                                           new SqlParameter("@osversion",model.osversion),
                                           new SqlParameter("@brand",model.brand),
                                           new SqlParameter("@deviceno",model.deviceno)
                                       };
            DbHelperSQL.ExecuteSql(strSql, parameters);
        }

        public int UpdateDeviceRemark(int deviceid, string remark)
        {
            string strSql = string.Format("update sayhi_device set remark=@remark where deviceid=@deviceid");
            SqlParameter[] parameters ={
                                          new SqlParameter("@remark",remark),
                                          new SqlParameter("@deviceid",deviceid)
                                      };

            return DbHelperSQL.ExecuteSql(strSql, parameters);
        }


        public int DeleteDevice(int deviceid)
        {
            string strSql = string.Format("delete from sayhi_device where deviceid=@deviceid");
            SqlParameter[] parameters ={
                                          new SqlParameter("@deviceid",deviceid)
                                      };

            return DbHelperSQL.ExecuteSql(strSql, parameters);
        }

        public DeviceModel GetDeviceByDeviceId(int deviceId)
        {
            string strSql = string.Format("select * from sayhi_device where deviceid=@deviceid");
            SqlParameter[] parameters ={
                                           new SqlParameter("@deviceid",deviceId)
                                      };

            DeviceModel model = new DeviceModel();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(strSql, parameters))
            {
                model = DbHelperSQL.GetEntity<DeviceModel>(dr);
            }
            return model;
        }

    }
}
