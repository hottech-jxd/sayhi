package com.huotu.huotao.sayhi;

import android.accessibilityservice.AccessibilityService;
import android.content.ClipData;
import android.content.ClipboardManager;
import android.content.Context;
import android.os.Build;
import android.os.Bundle;
import android.os.Trace;
import android.view.LayoutInflater;
import android.view.accessibility.AccessibilityNodeInfo;

import java.util.List;

/**
 * Created by Administrator on 2017/3/7.
 */

public class AccessiblityBase {

    protected AccessibilityService accessibilityService;

    /***
     * 执行点击操作
     * @param accessibilityNodeInfo
     */
    public void performClick(AccessibilityNodeInfo accessibilityNodeInfo){
        if(accessibilityNodeInfo.isClickable()){
            accessibilityNodeInfo.performAction(AccessibilityNodeInfo.ACTION_CLICK);
        }else{
            AccessibilityNodeInfo parent = accessibilityNodeInfo.getParent();
            if(parent!=null && parent.isClickable()){
                parent.performAction(AccessibilityNodeInfo.ACTION_CLICK);
            }
        }
    }

    /***
     * 设置EditText控件的内容
     * @param editText
     * @param content
     */
    void setEditText(AccessibilityNodeInfo editText , String content){
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP){
            String txt = editText.getText()==null? "": editText.getText().toString().trim();
            if( txt.equals(content) ) return;
            Bundle arguments = new Bundle();
            arguments.putCharSequence(AccessibilityNodeInfo.ACTION_ARGUMENT_SET_TEXT_CHARSEQUENCE, content );
            editText.performAction(AccessibilityNodeInfo.ACTION_SET_TEXT, arguments);
        }else{
            String txt = editText.getText()==null?"": editText.getText().toString().trim();
            if( txt.equals(content) ) return;
            ClipboardManager clipboardManager = (ClipboardManager) accessibilityService.getSystemService(Context.CLIPBOARD_SERVICE);
            ClipData clipData = ClipData.newPlainText("c2", content);
            clipboardManager.setPrimaryClip(clipData);
            editText.performAction(AccessibilityNodeInfo.ACTION_FOCUS);
            editText.performAction(AccessibilityNodeInfo.ACTION_PASTE);
        }
    }


    /***
     * 递归查找指定类名的节点
     * @param node
     * @param className
     * @return
     */
    public AccessibilityNodeInfo findNodeByClassName(AccessibilityNodeInfo node , String className){
        if( node ==null) return null;
        String nodeClassName = node.getClassName().toString();
        if( nodeClassName.equals( className ) ){
            return node;
        }
        int count = node.getChildCount();
        if( count==0) return null;
        for(int i=0;i<count;i++){
            AccessibilityNodeInfo item = node.getChild(i);
            AccessibilityNodeInfo find = findNodeByClassName( item , className);
            if( find == null ) continue;
            return  find;
        }
        return null;
    }

    /***
     * 退回到微信的主界面
     */
    public boolean backToWechatMainUI(AccessibilityNodeInfo rootNode){
        if( rootNode ==null ) return false;
        String className = "com.tencent.mm.ui.mogic.WxViewPager";
        AccessibilityNodeInfo viewPagerNode = findNodeByClassName(rootNode, className);
        if (viewPagerNode == null) {
            return false;
        }

        String tabitemViewId = accessibilityService.getString(R.string.LauncherUI_TAB_Item);
        List<AccessibilityNodeInfo> tabViews = rootNode.findAccessibilityNodeInfosByViewId(tabitemViewId);
        if(tabViews==null || tabViews.size()<4) return false;

        String tabitemNameViewId = accessibilityService.getString(R.string.LauncherUI_TAB_item_name);
        List<AccessibilityNodeInfo> tabNameViews = rootNode.findAccessibilityNodeInfosByViewId(tabitemNameViewId);
        if(tabNameViews==null || tabNameViews.size()<4) return false;

        boolean hasWechat=false,hasContract=false,hasDiscovery=false,hasMe=false;
        for(AccessibilityNodeInfo item : tabNameViews){
            String name = item.getText().toString().trim();
            if(name.equals("微信")){
                 hasWechat=true;
            }else if( name.equals("通讯录")){
                hasContract=true;
            }else if( name.equals("发现")){
                hasDiscovery=true;
            }else if(name.equals("我")){
                hasMe=true;
            }
        }

        if(hasDiscovery && hasWechat && hasContract && hasMe){
            return true;
        }
        return false;
    }
}
