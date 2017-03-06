package com.huotu.huotao.sayhi.bean;

import java.io.Serializable;

/**
 * Created by Administrator on 2017/2/24.
 */

public class BaseBean implements Serializable{
    private int code;
    private String message;

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }
}
