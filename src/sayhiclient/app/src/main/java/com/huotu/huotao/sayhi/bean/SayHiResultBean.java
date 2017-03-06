package com.huotu.huotao.sayhi.bean;

import java.util.List;
/**
 * Created by Administrator on 2017/2/22.
 */
public class SayHiResultBean  {
    private SayHiBean sayHiBean;
    private List<NearPersonBean> nearPersons;
    //标记指定的文字是否找到附近的人
    private boolean noFindNearPerson;
    //标记已经达到微信最大可打招呼人数
    private boolean reachedWechatMaxCount=false;
    //标记是否重新进入 "附近的人" 列表界面
    private boolean isEnterNearbyFriendsUI=false;
    //标记任务是否完成
    private boolean isFinished = false;

    public boolean isNoFindNearPerson() {
        return noFindNearPerson;
    }

    public void setNoFindNearPerson(boolean noFindNearPerson) {
        this.noFindNearPerson = noFindNearPerson;
    }

    public List<NearPersonBean> getNearPersons() {
        return nearPersons;
    }

    public void setNearPersons(List<NearPersonBean> nearPersons) {
        this.nearPersons = nearPersons;
    }

    public SayHiBean getSayHiBean() {
        return sayHiBean;
    }

    public void setSayHiBean(SayHiBean sayHiBean) {
        this.sayHiBean = sayHiBean;
    }

    public boolean isFinished() {
        return isFinished;
    }

    public void setFinished(boolean finished) {
        isFinished = finished;
    }

    public boolean isEnterNearbyFriendsUI() {
        return isEnterNearbyFriendsUI;
    }

    public void setEnterNearbyFriendsUI(boolean enterNearbyFriendsUI) {
        isEnterNearbyFriendsUI = enterNearbyFriendsUI;
    }
    public boolean isReachedWechatMaxCount() {
        return reachedWechatMaxCount;
    }
    public void setReachedWechatMaxCount(boolean reachedWechatMaxCount) {
        this.reachedWechatMaxCount = reachedWechatMaxCount;
    }
}
