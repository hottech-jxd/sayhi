using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using logic.Model;
using System.Data.SqlClient;
using common;

namespace logic.DAL
{
    public class LocationDAL
    {
        public List<LocationModel> GetLocations(int taskid , int status )
        {
            string strSql = string.Format("select * from sayhi_location where taskid=@taskid and status=@status");
            SqlParameter[] parameters ={
                                           new SqlParameter("@taskid", taskid),
                                           new SqlParameter("@status",status)
                                       };
            List<LocationModel> list = new List<LocationModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(strSql,parameters ))
            {
                list = DbHelperSQL.GetEntityList<LocationModel>(dr);
            }
            return list;
        }


        public List<LocationModel> GetLocations(int taskid)
        {
            string strSql = string.Format("select * from sayhi_location where taskid=@taskid");
            SqlParameter[] parameters ={
                                           new SqlParameter("@taskid", taskid)                          
                                       };
            List<LocationModel> list = new List<LocationModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(strSql, parameters))
            {
                list = DbHelperSQL.GetEntityList<LocationModel>(dr);
            }
            return list;
        }


        public int AddLocation(LocationModel model)
        {
            string strSql = string.Format("insert into sayhi_location(taskid,longitude,latitude,address,status,remark) values(@taskid,@longitude,@latitude,@address,@status,@remark);select @@IDENTITY");
            SqlParameter[] parameters ={
                                           new SqlParameter("@taskid", model.taskid),
                                           new SqlParameter("@longitude",model.longitude),
                                           new SqlParameter("@latitude", model.latitude),
                                           new SqlParameter("@address",model.address),
                                           new SqlParameter("@status",model.status),
                                           new SqlParameter("@remark",model.remark),
                                       };

            object obj = DbHelperSQL.GetSingle(strSql, parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }


        public int DeleteLocation(int taskid)
        {
            string strSql = string.Format("delete from sayhi_location where taskid=@taskid");
            SqlParameter[] parameter ={
                                          new SqlParameter("@taskid",taskid)
                                     };

            return DbHelperSQL.ExecuteSql(strSql, parameter);

        }

        public int DeleteLocationByLocationId(int locationid)
        {
            string strSql = string.Format("delete from sayhi_location where locationid=@locationid");
            SqlParameter[] parameter ={
                                          new SqlParameter("@locationid",locationid)
                                     };

            return DbHelperSQL.ExecuteSql(strSql, parameter);

        }


        public int ExistByLatLng(string latitude, string longitude, int taskid)
        {
            string strSql = string.Format("select count(1) from sayhi_location where longitude=@longitude and latitude=@latitude and taskid=@taskid");
                  SqlParameter[] parameter ={
                                          new SqlParameter("@longitude",longitude),
                                          new SqlParameter("@latitude",latitude),
                                          new SqlParameter("@taskid",taskid)
                                     };

            Object obj = DbHelperSQL.GetSingle(strSql, parameter);
            if (obj == null) return 0;
            return Convert.ToInt32(obj);
        }

    }
}
