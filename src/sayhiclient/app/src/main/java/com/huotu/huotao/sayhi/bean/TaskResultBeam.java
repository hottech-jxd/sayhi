package com.huotu.huotao.sayhi.bean;

import java.io.Serializable;

/**
 * Created by Administrator on 2017/2/25.
 */

public class TaskResultBeam extends BaseBean implements Serializable{
    private TaskBean data;

    public TaskBean getData() {
        return data;
    }

    public void setData(TaskBean data) {
        this.data = data;
    }
}
