package com.huotu.huotao.sayhi;

import android.accessibilityservice.AccessibilityService;
import android.util.Log;
import android.view.accessibility.AccessibilityEvent;
import android.view.accessibility.AccessibilityNodeInfo;

import com.huotu.huotao.sayhi.bean.SayHiBean;
import com.huotu.huotao.sayhi.bean.SayHiResultBean;

import java.util.List;

import static android.icu.lang.UCharacter.GraphemeClusterBreak.L;
import static com.huotu.huotao.sayhi.SayHiCache.isTaskFinished;

/**
 * Created by Administrator on 2017/3/7.
 */
public class WechatLoginUtils  extends AccessiblityBase{

    private SayHiBean currentSayhiData;

    public WechatLoginUtils(AccessibilityService accessibilityService ){
        this.accessibilityService= accessibilityService;
    }

    public void setCurrentSayhiData(SayHiBean currentSayhiData) {
        this.currentSayhiData = currentSayhiData;
    }

    public void login(AccessibilityNodeInfo rootNode , AccessibilityEvent accessibilityEvent){
        //当检测到当前任务已经完成状态，则不进行登录微信操作
        if( SayHiCache.isTaskFinished( currentSayhiData , accessibilityService ) ){
            LogUtils.log("当前任务已经完成，无需进行登录操作");
            return;
        }

        if( currentSayhiData==null ) return;
        if( currentSayhiData.getWechatusername() ==null  || currentSayhiData.getWechatusername().isEmpty() ) {
            LogUtils.log("当前任务的微信帐号空，无法进行登录操作");
            return;
        }
        if(currentSayhiData.getWechatpwd()==null || currentSayhiData.getWechatpwd().isEmpty() ) {
            LogUtils.log("当前任务的微信密码空，无法进行登录操作");
            return;
        }
        if(currentSayhiData.getWechatloginmode() == null || currentSayhiData.getWechatloginmode().isEmpty()  ){
            LogUtils.log("当前任务的微信登录方式空，无法进行登录操作");
            return;
        }

        loginRegister(rootNode , accessibilityEvent);

        mobileLogin(rootNode, accessibilityEvent);

        otherLogin(rootNode, accessibilityEvent);

        loginHistory( rootNode , accessibilityEvent);

        switch_account_dialog(rootNode);

        find_login_error_dialog(rootNode);

    }

    /***
     * 进入登录注册界面，触发“登录”事件
     * @param rootNode
     * @param accessibilityEvent
     * @return
     */
    public boolean loginRegister(AccessibilityNodeInfo rootNode , AccessibilityEvent accessibilityEvent) {
        if (rootNode == null) return false;
        String loginText = "登录";
        String registText = "注册";
        String loginViewId = accessibilityService.getString(R.string.LauncherUI_login);
        String registerViewId = accessibilityService.getString(R.string.LauncherUI_register);
        List<AccessibilityNodeInfo> loginViews = rootNode.findAccessibilityNodeInfosByViewId(loginViewId);
        List<AccessibilityNodeInfo> registerViews = rootNode.findAccessibilityNodeInfosByViewId(registerViewId);

        if (loginViews == null || loginViews.size() < 1) return false;
        if (registerViews == null || registerViews.size() < 1) return false;

        AccessibilityNodeInfo loginView = loginViews.get(0);
        AccessibilityNodeInfo registerView = registerViews.get(0);
        if (!loginView.getText().toString().trim().equals(loginText)) return false;
        if (!registerView.getText().toString().trim().equals(registText)) return false;


        performClick(loginView);
        return true;
    }

    /***
     * 手机登录界面
     * @param rootNode
     * @param accessibilityEvent
     * @return
     */
    public boolean mobileLogin(AccessibilityNodeInfo rootNode,AccessibilityEvent accessibilityEvent){
        if(rootNode==null)return false;

        String titleText = "使用手机号登录";
        String titleViewId = accessibilityService.getString(R.string.MobileInputUI_title);
        String mobileViewId = accessibilityService.getString(R.string.MobileInputUI_mobile);
        String pwdViewId = accessibilityService.getString(R.string.MobileInputUI_password);
        String loginViewId = accessibilityService.getString(R.string.MobileInputUI_login);
        String otherLoginViewId = accessibilityService.getString(R.string.MobileInputUI_otherlogin);

        List<AccessibilityNodeInfo> titleViews = rootNode.findAccessibilityNodeInfosByViewId(titleViewId);
        if(titleViews==null|| titleViews.size()<1) return false;
        AccessibilityNodeInfo titleView = titleViews.get(0);
        if( !titleView.getText().toString().trim().equals( titleText ) ){
            return false;
        }
        List<AccessibilityNodeInfo> mobileViews = rootNode.findAccessibilityNodeInfosByViewId(mobileViewId);
        if(mobileViews==null || mobileViews.size()<1)return false;
        AccessibilityNodeInfo mobileView = mobileViews.get(0);
        List<AccessibilityNodeInfo> pwdViews = rootNode.findAccessibilityNodeInfosByViewId(pwdViewId);
        if(pwdViews==null || pwdViews.size()<1) return false;
        AccessibilityNodeInfo pwdView = pwdViews.get(0);
        List<AccessibilityNodeInfo> loginViews = rootNode.findAccessibilityNodeInfosByViewId(loginViewId);
        if(loginViews==null||loginViews.size()<1)return false;
        AccessibilityNodeInfo loginView = loginViews.get(0);
        List<AccessibilityNodeInfo> otherLoginViews = rootNode.findAccessibilityNodeInfosByViewId(otherLoginViewId);
        if(otherLoginViews==null || otherLoginViews.size()<1) return false;
        AccessibilityNodeInfo otherView = otherLoginViews.get(0);

        if( currentSayhiData.getWechatloginmode().equals( Constants.WECHAT_LOGINMODE_MOBILE)) {
            //设置帐号和密码
            setEditText(mobileView, currentSayhiData.getWechatusername());
            setEditText(pwdView, currentSayhiData.getWechatpwd());
            //登录
            performClick(loginView);
            return true;
        }else{
            performClick(otherView);
            return false;
        }
    }

    /***
     * 其他方式登录
     * @param rootNode
     * @param accessibilityEvent
     * @return
     */
    public boolean otherLogin(AccessibilityNodeInfo rootNode , AccessibilityEvent accessibilityEvent){
        if(rootNode==null)return false;

        String titleText = "登录微信";
        String titleViewId = accessibilityService.getString(R.string.LauncherUI_OtherLogin_title);
        String layUsernameViewId=accessibilityService.getString(R.string.LauncherUI_OtherLogin_layusername);
        String layPasswordViewId = accessibilityService.getString(R.string.LauncherUI_OtherLogin_laypassword);
        String loginViewId = accessibilityService.getString(R.string.LauncherUI_OtherLogin_login);
        String usernameViewId = accessibilityService.getString(R.string.LauncherUI_OtherLogin_username);
        String passwordViewId = accessibilityService.getString(R.string.LauncherUI_OtherLogin_password);
        String backViewId = accessibilityService.getString(R.string.LauncherUI_OtherLogin_back);

        List<AccessibilityNodeInfo> titleViews = rootNode.findAccessibilityNodeInfosByViewId(titleViewId);
        if(titleViews==null|| titleViews.size()<1) return false;
        AccessibilityNodeInfo titleView = titleViews.get(0);
        if( !titleView.getText().toString().trim().equals( titleText ) )return false;

        List<AccessibilityNodeInfo> layUsernameViews = rootNode.findAccessibilityNodeInfosByViewId(layUsernameViewId);
        if(layUsernameViews==null|| layUsernameViews.size()<1) return false;
        AccessibilityNodeInfo layUsernameView = layUsernameViews.get(0);

        List<AccessibilityNodeInfo> layPwdViews = rootNode.findAccessibilityNodeInfosByViewId(layPasswordViewId);
        if(layPwdViews==null|| layPwdViews.size()<1) return false;
        AccessibilityNodeInfo layPwdView = layPwdViews.get(0);

        List<AccessibilityNodeInfo> userViews = layUsernameView.findAccessibilityNodeInfosByViewId(usernameViewId);
        if(userViews==null|| userViews.size()<1) return false;
        AccessibilityNodeInfo userView = userViews.get(0);

        List<AccessibilityNodeInfo> pwdViews = layPwdView.findAccessibilityNodeInfosByViewId(passwordViewId);
        if(pwdViews==null|| pwdViews.size()<1) return false;
        AccessibilityNodeInfo pwdView = pwdViews.get(0);

        List<AccessibilityNodeInfo> backViews = rootNode.findAccessibilityNodeInfosByViewId(backViewId);
        if(backViews==null|| backViews.size()<1) return false;
        AccessibilityNodeInfo backView = backViews.get(0);

        List<AccessibilityNodeInfo> loginViews = rootNode.findAccessibilityNodeInfosByViewId(loginViewId);
        if(loginViews==null|| loginViews.size()<1) return false;
        AccessibilityNodeInfo loginView = loginViews.get(0);


        if( currentSayhiData.getWechatloginmode().equals( Constants.WECHAT_LOGINMODE_OTHER) ) {
            setEditText( userView , currentSayhiData.getWechatusername() );
            setEditText( pwdView , currentSayhiData.getWechatpwd() );
            performClick( loginView );
            return true;
        }else{
            performClick( backView );
            return false;
        }
    }

    /***
     * 进入登录历史界面
     * 当 当前帐号与任务中的微信帐号相同，则输入密码，点击 登录
     * 当 帐号不同，则 点击 “更多”，进入 切换帐号
     * @param rootNode
     * @param accessibilityEvent
     * @return
     */
    public boolean loginHistory(AccessibilityNodeInfo rootNode , AccessibilityEvent accessibilityEvent){
        if( rootNode==null) return false;
        if( currentSayhiData==null ) return false;

        String userNameViewId = accessibilityService.getString(R.string.LoginHistoryUI_username);
        String pwdViewId = accessibilityService.getString(R.string.LoginHistoryUI_password);
        String loginViewId = accessibilityService.getString(R.string.LoginHistoryUI_login);
        String moreViewId = accessibilityService.getString(R.string.LoginHistoryUI_more);

        List<AccessibilityNodeInfo> userNameViews = rootNode.findAccessibilityNodeInfosByViewId(userNameViewId);
        if( userNameViews==null || userNameViews.size()<1 ) return false;
        List<AccessibilityNodeInfo> pwdViews= rootNode.findAccessibilityNodeInfosByViewId(pwdViewId);
        if(pwdViews==null || pwdViews.size()<1) return false;
        List<AccessibilityNodeInfo> loginViews = rootNode.findAccessibilityNodeInfosByViewId(loginViewId);
        if(loginViews==null || loginViews.size()<1) return false;
        List<AccessibilityNodeInfo> moreViews = rootNode.findAccessibilityNodeInfosByViewId(moreViewId);
        if( moreViews==null || moreViews.size()<1) return false;

        String username = userNameViews.get(0).getText().toString().trim();
        if( username.equals( currentSayhiData.getWechatusername() )){
            setEditText( pwdViews.get(0) , currentSayhiData.getWechatpwd() );
            performClick( loginViews.get(0) );
            return true;
        }else{
            //
            performClick( moreViews.get(0) );

            return false;
        }
    }

    /***
     * 进入 切换帐号 弹出框
     * @param rootNode
     */
    protected void switch_account_dialog(AccessibilityNodeInfo rootNode ){
        if( rootNode==null)return;
        String listviewViewId = accessibilityService.getString(R.string.Switch_Dialog_listview);
        String listviewItemViewId = accessibilityService.getString(R.string.Switch_Dialog_listview_item);
        List<AccessibilityNodeInfo> listview = rootNode.findAccessibilityNodeInfosByViewId(listviewViewId);
        if( listview ==null || listview.size()<1 ) return;
        List<AccessibilityNodeInfo> listViewItem = rootNode.findAccessibilityNodeInfosByViewId(listviewItemViewId);
        if(listViewItem==null || listViewItem.size()<1) return;

        for(AccessibilityNodeInfo item : listViewItem){
            String text = item.getText().toString();
            if( text.equals("切换帐号")){
                performClick( item );
                break;
            }
        }
    }

    /***
     *  判断微信帐号是否需要退出的条件是：
     *  当前任务完成，则退出。
     * @param rootNode
     * @param accessibilityEvent
     * @return
     */
    public boolean loginout(AccessibilityNodeInfo rootNode , AccessibilityEvent accessibilityEvent){
        if(rootNode==null)return false;
        if(currentSayhiData==null)return false;

        boolean isFinished = SayHiCache.isTaskFinished(currentSayhiData , accessibilityService);
        if(!isFinished) {
            LogUtils.log("当前任务没有完成，不进行退出微信操作");
            return false;
        }

        find_me_tab(rootNode,accessibilityEvent);

        find_set_item(rootNode);

        find_quit_item(rootNode);

        find_quit_select_dialog(rootNode);

        find_quit_tip_dialog(rootNode);

        return  true;
    }

    /***
     * 查找 "我" TAb
     * @param rootNode
     * @param accessibilityEvent
     * @return
     */
    public boolean find_me_tab(AccessibilityNodeInfo rootNode , AccessibilityEvent accessibilityEvent){
        if(rootNode==null)return false;
        String tab_me="我";
        String className ="com.tencent.mm.ui.mogic.WxViewPager";
        AccessibilityNodeInfo viewPagerNode = findNodeByClassName( rootNode , className);
        if( viewPagerNode == null){
            return  false;
        }
        AccessibilityNodeInfo parent = viewPagerNode.getParent();
        if(parent==null){
            return false;
        }

        int count = parent.getChildCount();
        if(count<1)return false;

        for(int i=0;i<count;i++) {
            AccessibilityNodeInfo item = parent.getChild(i);
            if (item.equals(viewPagerNode)) continue;

            List<AccessibilityNodeInfo> meNodes = item.findAccessibilityNodeInfosByText(tab_me);
            if(meNodes==null||meNodes.size()<1) continue;

            AccessibilityNodeInfo discovery = meNodes.get(0);
            performClick( discovery );
            return true;
        }

        return false;
    }

    /***
     * 查找 "设置"  项
     * @param rootNode
     * @return
     */
    public boolean find_set_item(AccessibilityNodeInfo rootNode ){
        if(rootNode==null)return false;
        String className = "com.tencent.mm.ui.mogic.WxViewPager";
        AccessibilityNodeInfo viewPagerNode = findNodeByClassName(rootNode, className);
        if (viewPagerNode == null) {
            return false;
        }

        String string_setting = "设置";
        List<AccessibilityNodeInfo> nodes = viewPagerNode.findAccessibilityNodeInfosByText(string_setting);
        if (nodes == null || nodes.size() < 1) return false;

        AccessibilityNodeInfo settingNode = nodes.get(0);
        performClick( settingNode );
        return true;
    }

    /***
     * 查找 “退出”项
     * @param rootNode
     * @return
     */
    public boolean find_quit_item(AccessibilityNodeInfo rootNode){
        if(rootNode==null)return false;
        String className ="android.widget.ListView";
        AccessibilityNodeInfo viewPagerNode = findNodeByClassName(rootNode, className);
        if (viewPagerNode == null) {
            return false;
        }

        String string_setting = "退出";
        List<AccessibilityNodeInfo> nodes = viewPagerNode.findAccessibilityNodeInfosByText(string_setting);
        if (nodes == null || nodes.size() < 1) return false;

        AccessibilityNodeInfo settingNode = nodes.get(0);
        performClick( settingNode );
        return true;

    }

    /***
     *  查找微信 退出选择提示框
     * @param rootNode
     * @return
     */
    public boolean find_quit_select_dialog(AccessibilityNodeInfo rootNode){
        if(rootNode==null)return false;
        String quitText1 = "退出当前帐号";
        String quitText2 ="关闭微信";

        List<AccessibilityNodeInfo> quitNodes1 = rootNode.findAccessibilityNodeInfosByText(quitText1);
        if(quitNodes1==null || quitNodes1.size()<1)return false;
        List<AccessibilityNodeInfo> quitNodes2 = rootNode.findAccessibilityNodeInfosByText(quitText2);
        if(quitNodes2==null||quitNodes2.size()<1)return false;

        performClick(quitNodes1.get(0));
        return true;
    }

    /***
     * 查找 退出提示框
     * @param rootNode
     * @return
     */
    public boolean find_quit_tip_dialog(AccessibilityNodeInfo rootNode){
        if(rootNode==null)return false;
        String msgtext="退出当前帐号后不会删除任何历史数据，下次登录依然可以使用本帐号。";
        String quittext = "退出";
        String quitTextViewId = accessibilityService.getString(R.string.SettingUI_Quit_Dialog_quit);
        String cancletext ="取消";
        String cancelViewId = accessibilityService.getString(R.string.SettingUI_Quit_Dialog_cancel);
        List<AccessibilityNodeInfo> msgviews = rootNode.findAccessibilityNodeInfosByText(msgtext);
        //List<AccessibilityNodeInfo> quitViews = rootNode.findAccessibilityNodeInfosByText(quittext);
        List<AccessibilityNodeInfo> quitViews = rootNode.findAccessibilityNodeInfosByViewId(quitTextViewId);
        List<AccessibilityNodeInfo> cancelViews = rootNode.findAccessibilityNodeInfosByViewId(cancelViewId);
        if(msgviews==null||msgviews.size()<1)return false;
        if(quitViews==null||quitViews.size()<1)return false;
        if(cancelViews==null||cancelViews.size()<1)return false;

        performClick(quitViews.get(0));

        return true;
    }

    /***
     * 当进入“登录密码错误 弹出框时”，点击 “确认”按钮
     * 并且标记 当前任务的微信帐号密码错误=true
     *
     * @param rootNode
     */
    public void find_login_error_dialog(AccessibilityNodeInfo rootNode){
        if(rootNode==null) return;

        String messageViewId =accessibilityService.getString(R.string.Login_Error_Dialog_message);
        String okViewId = accessibilityService.getString(R.string.Login_Error_Dialog_ok);
        List<AccessibilityNodeInfo> messageViews = rootNode.findAccessibilityNodeInfosByViewId(messageViewId);
        List<AccessibilityNodeInfo> okViews = rootNode.findAccessibilityNodeInfosByViewId(okViewId);
        if( messageViews==null || messageViews.size()<1 ) return;
        if( okViews==null || okViews.size()<1) return;

        String message = messageViews.get(0).getText().toString();
        if(message.equals("帐号或密码错误，请重新填写。")) {

            SayHiResultBean sayHiResultBean = SayHiCache.getSayHiResult(currentSayhiData);
            if(sayHiResultBean!=null)  sayHiResultBean.setWechatUserPasswordError(true);

            performClick(okViews.get(0));
        }
    }

}
