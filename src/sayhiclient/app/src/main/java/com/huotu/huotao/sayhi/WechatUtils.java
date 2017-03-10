package com.huotu.huotao.sayhi;

import android.accessibilityservice.AccessibilityService;
import android.content.ClipData;
import android.content.ClipboardManager;
import android.content.Context;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.accessibility.AccessibilityEvent;
import android.view.accessibility.AccessibilityNodeInfo;

import com.google.gson.Gson;
import com.huotu.huotao.sayhi.bean.NearPersonBean;
import com.huotu.huotao.sayhi.bean.SayHiBean;
import com.huotu.huotao.sayhi.bean.SayHiResultBean;

import java.util.ArrayList;
import java.util.List;

import static com.huotu.huotao.sayhi.LogUtils.log;

/**
 * Created by jinxiangdong on 2017/2/16.
 */
public class WechatUtils extends AccessiblityBase{
    //微信app主界面类名
    String LAUNCHERUI_PACKAGECLASSNAME = "com.tencent.mm.ui.LauncherUI";
    //附近的人介绍界面类名
    String NEARPERSON_INTROUI_PACKAGECLASSNAME  ="com.tencent.mm.plugin.nearby.ui.NearbyFriendsIntroUI";
    //附近的人列表界面类名
    String NEARPERSON_LISTUI_PACKAGECLASSNAME = "com.tencent.mm.plugin.nearby.ui.NearbyFriendsUI";
    //附近的人详细信息界面类名
    String NEARPERSON_CONTACTINFO_PACKAGECLASSNAME = "com.tencent.mm.plugin.profile.ui.ContactInfoUI";
    //打招呼界面类名
    String SAYHI_PACKAGECLASSNAME = "com.tencent.mm.ui.contact.SayHiEditUI";
    //聊天界面
    String CHATTINGUI_PACKAGENCLASSAME ="com.tencent.mm.ui.chatting.ChattingUI";
    //登录界面
    String  LOGINHISTORYUI_PACKAGECLASSNAME ="com.tencent.mm.ui.account.LoginHistoryUI";
    //手机登录界面
    String MOBILEINPUTUI_PACKAGECLASSNAME ="com.tencent.mm.ui.account.mobile.MobileInputUI";
    //
    String WIDGET_LISTVIEW="android.widget.ListView";

//    String faxiang = "发现";
//    String nearperson = "附近的人";
//    String string_startFind="开始查看";
//    boolean isIntroUi=false;

    //boolean is_SayHiUI=false;
    //boolean is_ChattingUI=false;
    //boolean is_ContactInfoUI=false;
    boolean isFindTab = false;
    boolean isFindNear = false;

    //AccessibilityService accessibilityService;
    SayHiBean currentSayhiData;
    NearPersonBean currentNearPerson;
    private  static Gson gson;
    private boolean isLock=false;
    private static Handler handler;


    public WechatUtils(AccessibilityService accessibilityService  ) {
        this.accessibilityService = accessibilityService;
        this.currentSayhiData = null;
        this.currentNearPerson=null;
        SayHiCache.removeAll();
        this.isLock=false;
    }

    public static Handler getHandler(){
        if(handler==null){
            handler = new android.os.Handler();
        }
        return handler;
    }

    public static Gson getGson(){
        if( gson==null) {
            gson = new Gson();
            gson.serializeNulls();
        }
        return gson;
    }

    /***
     * 设置当前任务位置配置信息
     * @param currentSayhiData
     */
    public void setCurrentSayhiData(SayHiBean currentSayhiData) {

        SayHiCache.removeAll();

        SayHiResultBean sayHiResultBean=new SayHiResultBean();
        sayHiResultBean.setSayHiBean(currentSayhiData);
        sayHiResultBean.setNearPersons(new ArrayList<NearPersonBean>());
        if( !SayHiCache.hasSayHi( sayHiResultBean )) {
            SayHiCache.addSayHi(sayHiResultBean);
        }

        this.currentSayhiData = currentSayhiData;
        this.currentNearPerson=null;
        this.isLock=false;
        LogUtils.log(currentSayhiData);
    }

    /***
     *
     * @param rootNode
     * @param accessibilityEvent
     */
    void sayHi(AccessibilityNodeInfo rootNode , AccessibilityEvent accessibilityEvent ) {
        if (rootNode == null || accessibilityEvent ==null ) return;

//        if( SayHiCache.isTaskFinished(currentSayhiData, accessibilityService) ){//
//            return;
//        }

        //判断当前位置，是否找到附近的人信息，如果没有找到人，则不处理。
        SayHiResultBean sayHiResultBean = SayHiCache.getSayHiResult(currentSayhiData);
        if( sayHiResultBean !=null && sayHiResultBean.isNoFindNearPerson()){
            LogUtils.log("当前位置，无法找到附近的人");
            return;
        }

//        boolean canOperate = canSayHiOperate();
//        if( !canOperate){
//            Log.i( WechatUtils.class.getName(), "打招呼的操作已经完成，无需继续进行打招呼操作");
//            return;
//        }

        String className = accessibilityEvent.getClassName().toString();

        if(className.equals( LAUNCHERUI_PACKAGECLASSNAME )){//微信主界面
            //launcherUI(rootNode , accessibilityEvent);

            //is_SayHiUI=false;
            //is_ChattingUI=false;
            //is_ContactInfoUI=false;

        }else if (className.equals(NEARPERSON_INTROUI_PACKAGECLASSNAME)) {//附近的人的介绍界面
            intro(rootNode);

            //is_SayHiUI=false;
            //is_ChattingUI=false;
            //is_ContactInfoUI=false;

        } else if (className.equals(NEARPERSON_LISTUI_PACKAGECLASSNAME)) {//附近的人列表界面
            //find_nearperson_list( rootNode);

            //is_SayHiUI=false;
            //is_ChattingUI=false;
            //is_ContactInfoUI=false;

        } else if (className.equals(NEARPERSON_CONTACTINFO_PACKAGECLASSNAME)) {//附近的人详细信息界面

            //is_SayHiUI=false;
            //is_ChattingUI=false;
            //is_ContactInfoUI=true;

        } else if (className.equals(SAYHI_PACKAGECLASSNAME)) {//打招呼界面
            //speakSayHi(rootNode);
            //is_SayHiUI=true;
            //is_ChattingUI=false;
            //is_ContactInfoUI=false;

        } else if( className.equals( CHATTINGUI_PACKAGENCLASSAME )) {//聊天界面

            //is_SayHiUI=false;
            //is_ChattingUI=true;
            //is_ContactInfoUI=false;

        } else if( className.equals( WIDGET_LISTVIEW)
                && accessibilityEvent.getEventType()== AccessibilityEvent.TYPE_VIEW_SCROLLED){
            find_nearperson_list(rootNode);
        }

        launcherUI(rootNode , accessibilityEvent);

        //当当前位置无法找到附近的人时，微信会进入的界面
        goto_NoFindUI(rootNode);

        find_nearperson_list( rootNode);

        //if(is_ContactInfoUI){
        goto_ContactInfoUI(rootNode);
        //}

        //if(is_SayHiUI) {
            speakSayHi(rootNode);
        //}

        //if(  is_ChattingUI){
            sendMessage(rootNode);
        //}

//        isFindTab = find_discovery_tab(rootNode, accessibilityEvent);
//        Log.i("WechatUtils", "是否找到[发现]按钮=" + String.valueOf(isFindTab));

        isFindNear = find_nearperson_view( rootNode, accessibilityEvent);
        //Log.i("WechatUtils", "是否找到[附近的人]按钮" + String.valueOf(isFindNear));

        find_tipDialog(rootNode, accessibilityEvent);
        //Log.i("WechatUtils", "是否找到提示弹出框" + String.valueOf(isFindTip));

        //当出现 帐号被强制退出 弹出框
        find_Account_ForceQuit_Dialog(rootNode);

    }

    /***
     *
     * @param rootNode
     *
     */
    void gotoback(AccessibilityNodeInfo rootNode  ){
//        new android.os.Handler().postDelayed(new Runnable() {
//            @Override
//            public void run() {
                accessibilityService.performGlobalAction(AccessibilityService.GLOBAL_ACTION_BACK);
//            }
//        }, Constants.OPERATE_DELAY);
    }

    /***
     *
     * @return
     */
    private boolean canSayHiOperate(){
        if( currentSayhiData==null) return false;
        return  SayHiCache.canSayHi( currentSayhiData );
    }

    /***
     *
     * @param rootNode
     * @return
     */
    boolean launcherUI(AccessibilityNodeInfo rootNode ,AccessibilityEvent accessibilityEvent ){
        if( SayHiCache.isTaskFinished(currentSayhiData , accessibilityService) ) return false;
        isFindTab = find_discovery_tab(rootNode, accessibilityEvent);
        Log.i("WechatUtils", "是否找到[发现]Tab=" + String.valueOf(isFindTab));
        return isFindTab;
    }

    /***
     * 附近的人的介绍界面
     * 当显示 "开始查看" 界面时，则查找到"开始查看"按钮，触发点击事件
     * @return
     */
    boolean intro(AccessibilityNodeInfo rootNode ){
        String string_see="开始查看";
        List<AccessibilityNodeInfo> nodes = rootNode.findAccessibilityNodeInfosByText(string_see);
        if(nodes==null|| nodes.isEmpty()|| nodes.size()<1) return false;

        for(int i=0;i< nodes.size();i++) {
            AccessibilityNodeInfo view = nodes.get(i);
            if (view.isClickable()) {
                view.performAction(AccessibilityNodeInfo.ACTION_CLICK);
            }else{
                AccessibilityNodeInfo parent = view.getParent();
                if(parent.isClickable()){
                    view.performAction(AccessibilityNodeInfo.ACTION_CLICK);
                }
            }
        }
        return true;
    }

    /***
     * 在界面中查找 "发现"  Tab 控件
     * 判断逻辑：如果界面中不存在 viewpager 控件，则判断界面中不存在 ""发现"按钮
     * 否则，查找界面中的其他控件中是否存在 "发现"控件
     * @param rootNode
     * @param event
     * @return
     */
    boolean find_discovery_tab(AccessibilityNodeInfo rootNode , AccessibilityEvent event) {
        if(rootNode==null)return false;
        String tab_discovery="发现";
        String className ="com.tencent.mm.ui.mogic.WxViewPager";
        AccessibilityNodeInfo viewPagerNode = findNodeByClassName( rootNode , className);
        if( viewPagerNode == null){
            //Toast.makeText(accessibilityService.getApplicationContext(), "在界面中无法找到[发现]底部导航栏按钮",Toast.LENGTH_SHORT).show();
            return  false;
        }

        AccessibilityNodeInfo parent = viewPagerNode.getParent();
        if(parent==null){
            return false;
        }

        int count = parent.getChildCount();
        if(count<1)return false;

        for(int i=0;i<count;i++){
            AccessibilityNodeInfo item = parent.getChild(i);
            if( item.equals( viewPagerNode ) ) continue;

            List<AccessibilityNodeInfo> discoveryNodes = item.findAccessibilityNodeInfosByText(tab_discovery);
            if(discoveryNodes==null||discoveryNodes.size()<1) continue;

            AccessibilityNodeInfo discovery = discoveryNodes.get(0);
            if(discovery.isClickable()){
                discovery.performAction(AccessibilityNodeInfo.ACTION_CLICK);
                return true;
            }else{
                AccessibilityNodeInfo discoveryParent  = discovery.getParent();
                if ( discoveryParent !=null && discoveryParent.isClickable()) {
                    discoveryParent.performAction(AccessibilityNodeInfo.ACTION_CLICK);
                    return true;
                }
            }
        }
        return false;
    }

    /***
     * 查找发现这个页面中的 “附近的人”按钮
     * @param rootNode
     * @param event
     * @return
     */
    boolean find_nearperson_view(AccessibilityNodeInfo rootNode , AccessibilityEvent event) {
        if(rootNode==null)return false;
        String className = "com.tencent.mm.ui.mogic.WxViewPager";
        AccessibilityNodeInfo viewPagerNode = findNodeByClassName(rootNode, className);
        if (viewPagerNode == null) {
            return false;
        }
        String string_NearPeron = "附近的人";
        List<AccessibilityNodeInfo> nodes = viewPagerNode.findAccessibilityNodeInfosByText(string_NearPeron);
        if (nodes == null || nodes.size() < 1) return false;

        if( SayHiCache.isTaskFinished(currentSayhiData , accessibilityService) ) return false;

        AccessibilityNodeInfo nearPersonNode = nodes.get(0);

        if (nearPersonNode.isClickable()) {

            //标记 已经进入 附近的人 列表界面
            SayHiResultBean sayHiResultBean = SayHiCache.getSayHiResult(currentSayhiData);
            if(sayHiResultBean!=null){
                sayHiResultBean.setEnterNearbyFriendsUI(true);
            }

            nearPersonNode.performAction(AccessibilityNodeInfo.ACTION_CLICK);

            return true;
        } else {
            AccessibilityNodeInfo parent =nearPersonNode.getParent();
            if (parent.isClickable()) {

                //标记 已经进入 附近的人 列表界面
                SayHiResultBean sayHiResultBean = SayHiCache.getSayHiResult(currentSayhiData);
                if(sayHiResultBean!=null){
                    sayHiResultBean.setEnterNearbyFriendsUI(true);
                }

                parent.performAction(AccessibilityNodeInfo.ACTION_CLICK);

                return true;
            }
        }
        return false;
    }


    /***
     *
     * 查找 提示 框
     *
     * @param rootNode
     * @return
     */
    boolean find_tipDialog(AccessibilityNodeInfo rootNode , AccessibilityEvent accessibilityEvent) {
        if(rootNode==null)return false;
        String chktip = "下次不提示";
        String okstr = "确定";
        List<AccessibilityNodeInfo> titleList = rootNode.findAccessibilityNodeInfosByText("提示");
        List<AccessibilityNodeInfo> checkList = rootNode.findAccessibilityNodeInfosByText(chktip);
        List<AccessibilityNodeInfo> okList = rootNode.findAccessibilityNodeInfosByText(okstr);
        List<AccessibilityNodeInfo> cancelList = rootNode.findAccessibilityNodeInfosByText("取消");

        if (titleList == null || titleList.isEmpty()) return false;
        if (checkList == null || checkList.isEmpty()) return false;
        if (okList == null || okList.isEmpty()) return false;
        if (cancelList == null || cancelList.isEmpty()) return false;

        for (int i = 0; i < checkList.size(); i++) {
            AccessibilityNodeInfo ckb = checkList.get(i);
            String txt = ckb.getText().toString();
            if (!txt.equals(chktip)) continue;
            if (!ckb.getClassName().equals("android.widget.CheckBox")) continue;
            if (ckb.isClickable()) {
                ckb.performAction(AccessibilityNodeInfo.ACTION_CLICK);
            }
        }

        for (int i = 0; i < okList.size(); i++) {
            AccessibilityNodeInfo ok = okList.get(i);
            String txt = ok.getText().toString();
            if (!txt.equals(okstr)) continue;
            if (ok.isClickable()) {
                ok.performAction(AccessibilityNodeInfo.ACTION_CLICK);
                return true;
            } else {
                AccessibilityNodeInfo parent = ok.getParent();
                if (parent.isClickable()) {
                    parent.performAction(AccessibilityNodeInfo.ACTION_CLICK);
                    return true;
                }
            }
        }
        return false;
    }

    /***
     * 进入无法找到附近的人界面，则进行关闭操作
     */
    private void goto_NoFindUI(AccessibilityNodeInfo rootNode ){
        String text = this.accessibilityService.getApplication().getString(R.string.NoFindUI_Content); //  暂时没有找到附近也使用该功能的人，请稍后再尝试查看。
        List<AccessibilityNodeInfo> list= rootNode.findAccessibilityNodeInfosByText(text);
        if(list==null || list.size()<1){
           return;
        }

        SayHiResultBean resultBean = SayHiCache.getSayHiResult( currentSayhiData );
        if( resultBean !=null) resultBean.setNoFindNearPerson(true);

        String msg =  String.format( "当前位置信息[纬度=%s,经度=%s]，无法找到附近的人", resultBean.getSayHiBean().getLongitude(), resultBean.getSayHiBean().getLatitude());
        log(msg);

        String backViewId = accessibilityService.getString(R.string.NoFindUI_back);
        List<AccessibilityNodeInfo> backViews = rootNode.findAccessibilityNodeInfosByViewId( backViewId);
        backOperate(backViews);
    }

    /***
     * 在附近的人列表界面，查看陌生人控件，触发点击事件
     * @param rootNode
     * @return
     */
    boolean find_nearperson_list(AccessibilityNodeInfo rootNode ) {
        if(isLock) return false;

        if(currentSayhiData==null) return false;

        String className ="android.widget.ListView";
        AccessibilityNodeInfo listView = findNodeByClassName( rootNode , className);
        if( listView ==null){
            return false;
        }
        int count = listView.getChildCount();
        if(count<1){
            return false;
        }

        String titleViewId = accessibilityService.getString(R.string.NearbyFriendsUI_title);
        String nameViewId = accessibilityService.getApplication().getString(R.string.NearbyFriendsUI_name);
        String sexViewId= accessibilityService.getApplication().getString(R.string.NearbyFriendsUI_sex);
        String backViewId = accessibilityService.getApplication().getString(R.string.NearbyFriendsUI_back);

        List<AccessibilityNodeInfo> titleViews = rootNode.findAccessibilityNodeInfosByViewId(titleViewId);
        if( titleViews==null|| titleViews.size()<1) return false;
        AccessibilityNodeInfo titleNodeInfo = titleViews.get(0);
        if(!titleNodeInfo.getText().toString().equals("附近的人")){
            return false;
        }

        if( SayHiCache.isTaskFinished(currentSayhiData , accessibilityService ) ){//判断是否需要关闭本页
            gotoback(rootNode);
            return false;
        }

        SayHiResultBean current = SayHiCache.getSayHiResult( currentSayhiData );
        if(current==null) {
            List<AccessibilityNodeInfo> backViews = rootNode.findAccessibilityNodeInfosByViewId(backViewId);
            backOperate(backViews);
            return false;
        }
        //判断是否重新进入了"附近的人"列表界面
        if(!current.isEnterNearbyFriendsUI()){
            List<AccessibilityNodeInfo> backViews = rootNode.findAccessibilityNodeInfosByViewId(backViewId);
            backOperate(backViews);
            return false;
        }

        List<NearPersonBean> nearPersonDataList = current.getNearPersons();
        if(nearPersonDataList==null){
            current.setNearPersons( new ArrayList<NearPersonBean>() );
        }

        for (int i = 0; i < count; i++) {
            AccessibilityNodeInfo item = listView.getChild(i);

            List<AccessibilityNodeInfo> nameViews = item.findAccessibilityNodeInfosByViewId(nameViewId);
            List<AccessibilityNodeInfo> sexViews = item.findAccessibilityNodeInfosByViewId(sexViewId);
            if(nameViews==null || nameViews.size()<1) continue;

            String name = nameViews.get(0).getText().toString().trim();
            String sex="";
            if(sexViews!=null && sexViews.size()>0){
                sex=sexViews.get(0).getContentDescription().toString().trim();
            }

            NearPersonBean model = new NearPersonBean();
            model.setName(name);
            model.setSex(sex);
            model.setSayHello(false);

            NearPersonBean newModel = Utils.FindData( nearPersonDataList ,  model);
            if(newModel==null){
                newModel = model;
                nearPersonDataList.add(model);
            }else{
                if(newModel.isSayHello())continue;
            }

            LogUtils.log( newModel );

           delay_Open_NearPerson(item);
            return true;
        }

        if(nearPersonDataList.size()< currentSayhiData.getMaxCount() ){
            if(scrollListView( listView)) {
                return true;
            }else{//当listview 不可以再滚动时，则判断为 微信已经返回没有可打招呼的人了。
                SayHiResultBean sayHiResultBean = SayHiCache.getSayHiResult( currentSayhiData );
                if(sayHiResultBean !=null)  sayHiResultBean.setReachedWechatMaxCount(true);
            }
        }

        List<AccessibilityNodeInfo> backViews = rootNode.findAccessibilityNodeInfosByViewId(backViewId);
        backOperate(backViews);
        return false;
    }

    /***
     *
     * @param node
     */
    void delay_Open_NearPerson( final AccessibilityNodeInfo node) {
        long delay = Constants.SAYHI_PERIOD;
        if(currentSayhiData!=null){
            delay = currentSayhiData.getRate()*1000;
            if(delay< Constants.SAYHI_PERIOD){
                delay = Constants.SAYHI_PERIOD;
            }
        }
        isLock= true;
        getHandler().postDelayed(new Runnable() {
            @Override
            public void run() {
                if ( node !=null && node.isClickable()) {
                    node.performAction(AccessibilityNodeInfo.ACTION_CLICK);
                }
                isLock=false;
            }
        }, delay);
    }

    /***
     * 滚动当前listview控件
     * @param node
     * @return
     */
    boolean scrollListView(AccessibilityNodeInfo node){
        if( node !=null && node.isScrollable()){
            node.performAction(AccessibilityNodeInfo.ACTION_SCROLL_FORWARD);
            return true;
        }else{
            return false;
        }
    }

    /***
     * 从用户详细资料界面的“名字”控件，获得人名，然后判断是否已经打过招呼，
     * 如果打过招呼，则触发返回事件操作。否则：
     * 如果是陌生人，则查找界面中的 “打招呼”按钮，然后触发点击事件
     * 如果是好友，则查找界面中的"发消息”按钮，然后触发点击事件
     * 注意：界面有可能出现滚动条，需要触发listview控件的滚动事件，
     * 然后再查找 界面中的相应控件。
     *
     * @param node
     * @return
     */
    boolean goto_ContactInfoUI(AccessibilityNodeInfo node) {
        if( node ==null) {
            LogUtils.log("goto_ContactInfoUI()的node=null");
            return false;
        }
        //if(currentSayhiData==null) return false;

        String titleViewId = accessibilityService.getString(R.string.ContactInfoUI_title);
        String nameViewId = accessibilityService.getApplication().getString(R.string.ContactInfoUI_name); //界面中的名称控件的id
        String backViewId = accessibilityService.getApplication().getString(R.string.ContactInfoUI_back); // 界面中的返回控件的id
        String areaViewId = accessibilityService.getString(R.string.ContactInfoUI_area);
        String distanceViewId =accessibilityService.getString(R.string.ContactInfoUI_distance);
        String sayHiViewId = accessibilityService.getApplication().getString(R.string.ContactInfoUI_sayHi);//界面中的打招呼控件id
        String sendMessageViewId = accessibilityService.getApplication().getString(R.string.ContactInfoUI_sendMessage);//界面中的发消息控件Id

        List<AccessibilityNodeInfo> titleViews = node.findAccessibilityNodeInfosByViewId(titleViewId);
        List<AccessibilityNodeInfo> backViews =node.findAccessibilityNodeInfosByViewId(backViewId);
        List<AccessibilityNodeInfo> nameViews = node.findAccessibilityNodeInfosByViewId(nameViewId);
        List<AccessibilityNodeInfo> areaViews = node.findAccessibilityNodeInfosByViewId(areaViewId);
        List<AccessibilityNodeInfo> distanceViews = node.findAccessibilityNodeInfosByViewId(distanceViewId);
        List<AccessibilityNodeInfo> sayHiViews = node.findAccessibilityNodeInfosByViewId(sayHiViewId);
        List<AccessibilityNodeInfo> sendMessageViews = node.findAccessibilityNodeInfosByViewId(sendMessageViewId);
        String className ="android.widget.ListView";


        if(titleViews==null || titleViews.size()<1){
            //LogUtils.log("在goto_ContactInfoUI()，标题控件没有找到");
            return false;
        }
        String title = "详细资料";
        AccessibilityNodeInfo titleView = titleViews.get(0);
        if(!titleView.getText().toString().trim().equals( title )){
            return false;
        }

        AccessibilityNodeInfo listView = findNodeByClassName( node , className);
        if( listView ==null || listView.getChildCount()<1 ){
            LogUtils.log("在goto_ContactInfoUI(),无法找到ListView控件");
            return false;
        }

        if(currentSayhiData==null) {
            backOperate(backViews);
            return false;
        }

        if( SayHiCache.isTaskFinished(currentSayhiData , accessibilityService) ){//判断是否需要关闭本页
            backOperate(backViews);
            return false;
        }

        if (nameViews == null || nameViews.size() < 1) {
            LogUtils.log("在详情界面没有找到姓名控件");
            return false;
        }

        if(  (sayHiViews == null || sayHiViews.size() <1) && ( sendMessageViews==null || sendMessageViews.size()<1) ) {
            if (!scrollListView(listView)) {
                LogUtils.log("在详情界面没有找到'打招呼'或'发送消息'按钮");
            }
            return false;
        }

//        String area ="";
//        if(areaViews !=null && areaViews.size()>0){
//            area = areaViews.get(0).getText().toString().trim();
//        }
//        String distance ="";
//        if(distanceViews!=null && distanceViews.size()>0){
//            distance=distanceViews.get(0).getText().toString().trim();
//        }

        String name = nameViews.get(0).getText().toString().trim();
        NearPersonBean data = new NearPersonBean();
        data.setName(name);

        SayHiResultBean current = SayHiCache.getSayHiResult( currentSayhiData );
        if( current==null ) {
            LogUtils.log("无法找到相应的任务信息");
            return false;
        }
        NearPersonBean m = Utils.FindData( current.getNearPersons() , data);
        if ( m == null || m.isSayHello()) {
            backOperate(backViews);
            return true;
        }

        if (sayHiViews != null && sayHiViews.size() > 0 ) {
            for (int i = 0; i < sayHiViews.size(); i++) {
                AccessibilityNodeInfo view = sayHiViews.get(i);
                if (!view.getClassName().equals("android.widget.Button")) continue;
                if (view.isClickable()) {
                    currentNearPerson = m;
                    //currentNearPerson.setArea( area );
                    //currentNearPerson.setDistance(distance);
                    view.performAction(AccessibilityNodeInfo.ACTION_CLICK);
                    Log.i(WechatUtils.class.getName(), "点击了打招呼按钮");
                    return true;
                }
            }
        }

        if (sendMessageViews != null && sendMessageViews.size() > 0 ) {
            for (int i = 0; i < sendMessageViews.size(); i++) {
                AccessibilityNodeInfo view = sendMessageViews.get(i);
                if (!view.getClassName().equals("android.widget.Button")) continue;
                if (view.isClickable()) {
                    currentNearPerson = m;
                    //currentNearPerson.setArea( area );
                    //currentNearPerson.setDistance(distance);
                    view.performAction(AccessibilityNodeInfo.ACTION_CLICK);
                    Log.i(WechatUtils.class.getName(), "点击了发消息按钮");
                    return true;
                }
            }
        }

//        String className ="android.widget.ListView";
//        AccessibilityNodeInfo listView = findNodeByClassName( node , className);
//        if( listView ==null || listView.getChildCount()<1 ){
//            backOperate(backViews);
//            return false;
//        }
//        if(  scrollListView(listView)){
//            return false;
//        }

        backOperate(backViews);

        return false;
    }

    /***
     *  触发 打招呼 事件
     * @param rootNode
     */
    void speakSayHi(AccessibilityNodeInfo rootNode) {
        if(rootNode==null)return;
        if( currentSayhiData==null )return;
        if( currentSayhiData.getContent()==null || currentSayhiData.getContent().isEmpty() )return;

        String titleViewId = accessibilityService.getString(R.string.SayHiEditUI_title);
        String contentViewId = accessibilityService.getApplication().getString(R.string.SayHiEditUI_content);
        String sendViewId = accessibilityService.getApplication().getString(R.string.SayHiEditUI_send);
        String backViewId = accessibilityService.getApplication().getString(R.string.SayHiEditUI_back);
        List<AccessibilityNodeInfo> titleViews = rootNode.findAccessibilityNodeInfosByViewId(titleViewId);
        List<AccessibilityNodeInfo> editList = rootNode.findAccessibilityNodeInfosByViewId(contentViewId);
        List<AccessibilityNodeInfo> sendList = rootNode.findAccessibilityNodeInfosByViewId(sendViewId);
        List<AccessibilityNodeInfo> backViews = rootNode.findAccessibilityNodeInfosByViewId(backViewId);

        if(titleViews==null || titleViews.size()<1) return;
        AccessibilityNodeInfo titleView = titleViews.get(0);
        if( !titleView.getText().toString().trim().equals("打招呼")){
            return;
        }

        if( SayHiCache.isTaskFinished(currentSayhiData , accessibilityService) ){//判断是否需要关闭本页
            backOperate(backViews);
            return;
        }


        if (editList == null || editList.size() < 1) {
            return;
        }
        if (sendList == null || sendList.size() < 1){
            return;
        }

        if( currentNearPerson==null) return;

        AccessibilityNodeInfo edit = editList.get(0);
        AccessibilityNodeInfo send = sendList.get(0);

        setEditText(edit ,  currentSayhiData.getContent() );

        currentNearPerson.setSayHello(true);

        //记录打招呼的信息
        LogUtils.log(currentNearPerson);

        Log.i(WechatUtils.class.getName(), "打招呼发送完毕");

        //---------test----------
        backOperate(backViews);//test
        //------------------------

        //----------正式----------
        //send.performAction(AccessibilityNodeInfo.ACTION_CLICK);
        //-------------------------
    }

    void backOperate(final List<AccessibilityNodeInfo> nodes) {
        if (nodes == null || nodes.size() < 1) {
            return;
        }

        for (int i = 0; i < nodes.size(); i++) {
            if (nodes.get(i) == null) continue;
            if (nodes.get(i).isClickable()) {
                nodes.get(i).performAction(AccessibilityNodeInfo.ACTION_CLICK);
            } else {
                if ( nodes.get(i).getParent() !=null && nodes.get(i).getParent().isClickable()) {
                    nodes.get(i).getParent().performAction(AccessibilityNodeInfo.ACTION_CLICK);
                }
            }
        }
    }

    /***
     * 触发发送消息 事件
     * @param rootNode
     */
    void sendMessage(AccessibilityNodeInfo rootNode){
        if(rootNode==null)return;
        if( currentSayhiData==null )return;
        if( currentSayhiData.getContent()==null || currentSayhiData.getContent().isEmpty() )return;

        String contentViewId = accessibilityService.getApplication().getString(R.string.ChattingUI_content);
        String sendViewId= accessibilityService.getApplication().getString(R.string.ChattingUI_send);
        String backViewId = accessibilityService.getApplication().getString(R.string.ChattingUI_back);
        String titleViewId = accessibilityService.getString(R.string.ChattingUI_title);

        List<AccessibilityNodeInfo> titleViews = rootNode.findAccessibilityNodeInfosByViewId(titleViewId);
        List<AccessibilityNodeInfo> contentViews = rootNode.findAccessibilityNodeInfosByViewId(contentViewId);
        List<AccessibilityNodeInfo> backViews = rootNode.findAccessibilityNodeInfosByViewId(backViewId);


        if(titleViews==null || titleViews.size()<1) return;

        if( currentNearPerson==null) return;

        if(contentViews==null|| contentViews.size()<1){
            return;
        }

        AccessibilityNodeInfo contentView= contentViews.get(0);

        String content  = currentSayhiData.getContent();
        setEditText(contentView, content);


        List<AccessibilityNodeInfo> sendViews = rootNode.findAccessibilityNodeInfosByViewId(sendViewId);
        if(sendViews==null || sendViews.size()<1){
            return;
        }

        if( SayHiCache.isTaskFinished(currentSayhiData , accessibilityService) ){//判断是否需要关闭本页
            backOperate(backViews);
            return;
        }


        currentNearPerson.setSayHello(true);

        LogUtils.log(currentNearPerson);

        //------------test-------------
        //backOperate(backViews); // test
        //-----------------------------


        AccessibilityNodeInfo sendView = sendViews.get(0);
        //delayClick( sendView );


        if(sendView.isClickable()){
            sendView.performAction(AccessibilityNodeInfo.ACTION_CLICK);
            backOperate(backViews);
        }else{
            AccessibilityNodeInfo parent = sendView.getParent();
            if(parent!=null && parent.isClickable()){
                parent.performAction(AccessibilityNodeInfo.ACTION_CLICK);
                backOperate(backViews);
            }
        }
    }


    void delayClick( final AccessibilityNodeInfo nodeInfo){

        getHandler().postDelayed(new Runnable() {
            @Override
            public void run() {
                if (nodeInfo.isClickable()) {
                    nodeInfo.performAction(AccessibilityNodeInfo.ACTION_CLICK);
                    //Log.i(WechatUtils.class.getName(), "点击了打招呼按钮");
                    return;
                }else {
                    AccessibilityNodeInfo parent = nodeInfo.getParent();
                    if(parent!=null && parent.isClickable()){
                        parent.performAction(AccessibilityNodeInfo.ACTION_CLICK);
                        return;
                    }
                }
            }
        }, Constants.OPERATE_DELAY);

    }

    /***
     * 当进入 “微信帐号被其他设备登录提示框”时，
     * 进行重新登录操作
     * @param rootNode
     */
    void find_Account_ForceQuit_Dialog(AccessibilityNodeInfo rootNode){
        if(rootNode==null) return;
        String titleViewId =accessibilityService.getString(R.string.Account_ForceQuit_Dialog_title);
        String okViewId = accessibilityService.getString(R.string.Account_ForceQuit_Dialog_ok);
        List<AccessibilityNodeInfo> titleViews = rootNode.findAccessibilityNodeInfosByViewId(titleViewId);
        if( titleViews==null || titleViews.size()<1) return;
        List<AccessibilityNodeInfo> okViews = rootNode.findAccessibilityNodeInfosByViewId(okViewId);
        if( okViews==null || okViews.size()<1) return;

        String title = titleViews.get(0).getText().toString();
        if( !title.equals("提示"))return;

        performClick( okViews.get(0) );
    }
}
