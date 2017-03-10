using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using logic.Model;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using common;

namespace logic.DAL
{
    public class TaskDAL
    {
        public int AddTask( TaskModel model )
        {
            string strSql = string.Format("insert into sayhi_task(starttime,stoptime,sayhi,status,createtime,sayhirate,sayhimaxcount,remark,wechatusername,wechatpwd,wechatloginmode,deviceno) values(@starttime,@stoptime,@sayhi,@status,@createtime,@sayhirate,@sayhimaxcount,@remark,@wechatusername,@wechatpwd,@wechatloginmode,@deviceno);select @@IDENTITY");
            SqlParameter[] parameters ={
                                           new SqlParameter("@starttime",model.starttime),
                                           new SqlParameter("@stoptime",model.stoptime),
                                           new SqlParameter("@sayhi",model.sayhi),
                                           new SqlParameter("@status",model.status),
                                           new SqlParameter("@createtime",model.createtime),
                                           new SqlParameter("@sayhirate",model.sayhirate),
                                           new SqlParameter("@sayhimaxcount",model.sayhimaxcount),
                                           new SqlParameter("@remark",model.remark),
                                           new SqlParameter("@wechatusername",model.wechatusername),
                                           new SqlParameter("@wechatpwd",model.wechatpwd),
                                           new SqlParameter("@wechatloginmode",model.wechatloginmode),
                                           new SqlParameter("@deviceno",model.deviceno),
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


        public List<TaskModel> GetTaskList(string deviceno , DateTime starttime , DateTime stoptime , int status )
        {
            //starttime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            //stoptime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            

            string strSql = string.Format("select * from sayhi_task where status=@status and deviceno=@deviceno and starttime>=@starttime and stoptime <=@stoptime order by starttime asc ");
            SqlParameter[] parameters ={
                                           new SqlParameter("@status", status),
                                           new SqlParameter("@deviceno", deviceno),
                                           new SqlParameter("@starttime", starttime),
                                           new SqlParameter("@stoptime",stoptime)
                                       };

            List<TaskModel> list = new List<TaskModel>();

            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(strSql , parameters ))
            {
                list = DbHelperSQL.GetEntityList<TaskModel>(dr);
            }


            return list;
        }


        public TaskModel GetTask(int taskid)
        {
            string strSql = string.Format("select * from sayhi_task where taskid=@taskid ");
            SqlParameter[] parameters ={
                                           new SqlParameter("@taskid", taskid )            
                                       };

            TaskModel model;

            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(strSql, parameters))
            {
                model = DbHelperSQL.GetEntity<TaskModel>(dr);
            }

            strSql = string.Format("select * from sayhi_location where taskid=@taskid");
            SqlParameter[] parameters2 ={
                                           new SqlParameter("@taskid", taskid )            
                                       };
            List<LocationModel> lst = new List<LocationModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(strSql, parameters2 ))
            {
                lst = DbHelperSQL.GetEntityList<LocationModel>(dr);
            }

            model.locations = lst;

            return model;
        }

        public List<TaskModel> GetTaskListPage(int pageIndex, int pageSize, out int recordCount, TaskWhere where)
        {
            string sql = @"select * from sayhi_task where 1=1";

            if (where != null)
            {
                if (!string.IsNullOrEmpty(where.deviceno))
                    sql += string.Format(" and deviceno like'%{0}%' ", where.deviceno);
                if (!string.IsNullOrEmpty(where.starttime))
                    sql += string.Format(" and starttime>= '{0}' ", where.starttime );
                if (!string.IsNullOrEmpty(where.status))
                {
                    sql += string.Format(" and status={0}", where.status);
                }
            }


            string querySql = DbHelperSQL.buildPageSql(pageIndex, pageSize, sql, "createtime", false);
            string recordCountSql = DbHelperSQL.buildRecordCountSql(sql);
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(recordCountSql));
            List<TaskModel> lst = new List<TaskModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(querySql))
            {
                lst = DbHelperSQL.GetEntityList<TaskModel>(dr);
            }
            return lst;

        }


        public int DeleteTask(int taskid)
        {
            string strSql = string.Format("delete from sayhi_task where taskid=@taskid");
            SqlParameter[] parameters ={
                                           new SqlParameter("@taskid",taskid)
                                      };


            return DbHelperSQL.ExecuteSql(strSql, parameters);

        }

        public int UpdateTaskWithNoStatus(TaskModel model)
        {
            string strSql = string.Format("update sayhi_task set starttime=@starttime , stoptime=@stoptime , sayhi=@sayhi , sayhirate=@sayhirate , sayhimaxcount=@sayhimaxcount, remark=@remark , wechatusername=@wechatusername ,wechatpwd =@wechatpwd ,wechatloginmode =@wechatloginmode where taskid=@taskid");
            SqlParameter[] parameters ={
                                           new SqlParameter("@starttime", model.starttime),
                                           new SqlParameter("@stoptime", model.stoptime),
                                           new SqlParameter("@sayhi", model.sayhi),
                                           new SqlParameter("@sayhirate",model.sayhirate),
                                           new SqlParameter("@sayhimaxcount",model.sayhimaxcount),
                                           new SqlParameter("@remark",model.remark),                                          
                                           new SqlParameter("@wechatusername",model.wechatusername),
                                           new SqlParameter("@wechatpwd",model.wechatpwd),
                                           new SqlParameter("@wechatloginmode",model.wechatloginmode),
                                           new SqlParameter("@taskid", model.taskid),
                                      };

            return DbHelperSQL.ExecuteSql(strSql, parameters);

        }


        public int UpdateTaskWithStatus(TaskModel model)
        {
            string strSql = string.Format("update sayhi_task set starttime=@starttime , stoptime=@stoptime , sayhi=@sayhi , sayhirate=@sayhirate , sayhimaxcount=@sayhimaxcount , remark=@remark , status=@status , wechatusername=@wechatusername ,wechatpwd =@wechatpwd ,wechatloginmode =@wechatloginmode where taskid=@taskid");
            SqlParameter[] parameters ={
                                           new SqlParameter("@starttime", model.starttime),
                                           new SqlParameter("@stoptime", model.stoptime),
                                           new SqlParameter("@sayhi", model.sayhi),
                                           new SqlParameter("@sayhirate",model.sayhirate),
                                           new SqlParameter("@sayhimaxcount",model.sayhimaxcount),
                                           new SqlParameter("@remark", model.remark),
                                           new SqlParameter("@status", model.status),
                                           new SqlParameter("@wechatusername",model.wechatusername),
                                           new SqlParameter("@wechatpwd",model.wechatpwd),
                                           new SqlParameter("@wechatloginmode",model.wechatloginmode),
                                           new SqlParameter("@taskid", model.taskid),
                                      };

            return DbHelperSQL.ExecuteSql(strSql, parameters);
        }



        public int SetTaskDevice(int taskid, string deviceno)
        {
            string strSql = string.Format("update sayhi_task set deviceno=@deviceno where taskid=@taskid");
            SqlParameter[] parameters ={
                                           new SqlParameter("@deviceno",deviceno),
                                           new SqlParameter("@taskid",taskid)
                                      };
            return DbHelperSQL.ExecuteSql(strSql, parameters);
        }

        public int UpdateLocationStatus(int taskid, int locationid , int status , string remark)
        {
            string strSql = string.Format("update sayhi_location set status=@status , remark=@remark where taskid=@taskid and locationid=@locationid");
            SqlParameter [] parameters={
                                            new SqlParameter("@status",status),
                                            new SqlParameter("@remark", remark),
                                           new SqlParameter("@taskid",taskid),
                                           new SqlParameter("@locationid",locationid),
                                       };

           int count = DbHelperSQL.ExecuteSql( strSql,parameters);

            strSql = string.Format("select count(1) from sayhi_location where taskid=@taskid and status=@status");
            SqlParameter[] parameters2 ={
                                            new SqlParameter("@taskid",taskid),
                                            new SqlParameter("@status", Constants.SUBTASK_INIT )
                                       };

            object obj = DbHelperSQL.GetSingle(strSql, parameters2);
            int size = 0;
            if (obj != null)
            {
                size = Convert.ToInt32(obj);
            }

            if (size < 1)
            {
                strSql = string.Format("update sayhi_task set status=@status where taskid=@taskid");
                SqlParameter[] parameters3 ={
                                            new SqlParameter("@status",Constants.TASKSTATUS_FINISHED),
                                            new SqlParameter("@taskid", taskid )
                                       };

                DbHelperSQL.ExecuteSql(strSql, parameters3);

            }
            return count;

        }


        public int ClearTaskDevice(int taskid)
        {
            string strSql = string.Format("update sayhi_task set deviceno='' where taskid=@taskid");
            SqlParameter[] parameters ={
                                          new SqlParameter("@taskid",taskid),
                                      };

            return DbHelperSQL.ExecuteSql(strSql, parameters);

        }

        public int UpdateTaskStatusRun(int taskid, int status)
        {
            string strSql = string.Format("update sayhi_task set status=@status where taskid=@taskid");
            SqlParameter[] parameters ={
                                           new SqlParameter("@status", status),
                                           new SqlParameter("@taskid",taskid)
                                      };
            return DbHelperSQL.ExecuteSql( strSql, parameters);
        }

    }
}
