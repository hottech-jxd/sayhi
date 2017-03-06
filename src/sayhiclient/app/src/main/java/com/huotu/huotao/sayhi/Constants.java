package com.huotu.huotao.sayhi;

/**
 * Created by Administrator on 2017/2/21.
 */

public class Constants {
    //获得数据发送广播
    public static String ACTION_HOUTAO_SAYHI_GETDATA="com.huotu.huotao.sayhi.GETDATA";

    public static String PARAMETER_LONGITUDE ="longitude";

    public static String PARAMETER_LATITUDE = "latitude";

    public static String PARAMETER_CONTENT="content";

    public static String PARAMETER_MAXCOUNT ="maxcount";

    public static String PARAMETER_SAYHIDATA="sayhidata";

    public static String PARAMETER_TASK_DATA="taskdata";

    //定期请求打招呼任务配置数据的周期（毫秒）
    public static int  Request_SAYHI_Task_PERIOD= 1 * 30*1000;
    //打招呼的动作的间隔时间(毫秒)
    public  static  int SAYHI_PERIOD = 5 * 1000;

    //微信app包名
    public static String WECHAT_APP_PACKAGENAME="com.tencent.mm";

    //模拟点击操作的延迟毫秒时间
    public static int OPERATE_DELAY=500;

    /**
     * 操作平台码
     */
    public static final String OPERATION_CODE = "SAYHI_ANDROID";
    /***
     * 标记 当前任务的指定位置的打招呼操作已经完成
     */
    public static final int TASK_LOCATION_STATUS_FINISHED=1;

}
