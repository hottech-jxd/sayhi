package com.huotu.huotao.sayhi;

import java.util.concurrent.TimeUnit;

import okhttp3.OkHttpClient;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

/**
 * Created by Administrator on 2016/3/15.
 */
public class ZRetrofitUtil {
    private static Retrofit retrofitClient;

    private static ApiDefine apiService;

    private static final long timeout=15;

    /**
     * 获得UI配置接口
     * @return
     */
    public static Retrofit getInstance(){
        if(BuildConfig.DEBUG) {
            if (retrofitClient == null) {
                OkHttpClient okHttpClient = new OkHttpClient.Builder()
                        .readTimeout( timeout ,TimeUnit.SECONDS)
                        .retryOnConnectionFailure(true).connectTimeout(timeout, TimeUnit.SECONDS).build();
                retrofitClient = new Retrofit.Builder().client(okHttpClient)
                        .baseUrl( BuildConfig.INTERFACE_URL ).addConverterFactory(GsonConverterFactory.create()).build();
            }
        }else {

            if (retrofitClient == null) {
                retrofitClient = new Retrofit.Builder()
                        .baseUrl( BuildConfig.INTERFACE_URL )
                        .addConverterFactory(GsonConverterFactory.create())
                        .build();
            }
        }
        return retrofitClient;
    }

    public static ApiDefine getApiDefine(){
        if( apiService ==null){
            apiService = ZRetrofitUtil.getInstance().create(ApiDefine.class);
        }
        return apiService;
    }
}
