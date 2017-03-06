package com.huotu.huotao.sayhi;

import android.content.Context;
import android.content.Intent;
import android.os.Build;
import android.os.IBinder;
import android.os.RemoteException;

import java.lang.reflect.Field;
import java.lang.reflect.Method;

import de.robv.android.xposed.XC_MethodHook;
import de.robv.android.xposed.XposedBridge;
import de.robv.android.xposed.XposedHelpers;
import okhttp3.internal.Util;

import static de.robv.android.xposed.XposedHelpers.callMethod;
import static de.robv.android.xposed.XposedHelpers.callStaticMethod;

public class SayHiGetDataService extends IGetDataAidlInterface.Stub {
    private static final String SERVICE_NAME = "sayhigetData.service";
    static SayHiGetDataService sayHiGetDataService;
    Context mContext;

    public SayHiGetDataService(Context context) {
        this.mContext = context;
    }

    public static String getServiceName() {
        return Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP  ?  "user." + SERVICE_NAME  : SERVICE_NAME;
    }

    public static IGetDataAidlInterface getClient() {
        try {
            Class<?> ServiceManager = Class.forName("android.os.ServiceManager");
            Method getService = ServiceManager.getDeclaredMethod("getService", String.class);
            return IGetDataAidlInterface.Stub.asInterface((IBinder) getService.invoke(null, getServiceName()));
        } catch (Throwable t) {
            return null;
        }
    }

    @Override
    public void basicTypes(int anInt, long aLong, boolean aBoolean, float aFloat, double aDouble, String aString) throws RemoteException {

    }

    @Override
    public String getData() throws RemoteException {

//        GetDataThread thread = new GetDataThread(this.mContext);
//        thread.startRun();
//        XposedBridge.log("---------GetDataThread Runing---------------");


        return "I am jinxiangdong";


    }

    @Override
    public void systemReady() {
        XposedBridge.log("---------SayHiGetDataService.systemReady---------------");

        GetDataThread thread = new GetDataThread(this.mContext);
        thread.startRun();
        XposedBridge.log("---------GetDataThread Runing---------------");

//        Intent intent = new Intent();
//        intent.setAction( Constants.ACTION_HOUTAO_SAYHI_GETDATA );
//        mContext.sendBroadcast(intent);

    }

    public static void register(){
        if( Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP ){
            register_two();
        }else{
            register_one();
        }
    }

    public static void register_old() {

        final Class activityManagerServiceClzz = XposedHelpers.findClass("com.android.server.am.ActivityManagerService", null);

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            XposedBridge.hookAllConstructors(activityManagerServiceClzz, new XC_MethodHook() {
                @Override
                protected void afterHookedMethod(MethodHookParam param) throws Throwable {
                    //super.afterHookedMethod(param);
                    register((Context) XposedHelpers.getObjectField(param.thisObject, "mContext"));
                }
            });
        } else {
            XposedBridge.hookAllMethods(activityManagerServiceClzz, "main",
                    new XC_MethodHook() {
                        @Override
                        protected void afterHookedMethod(MethodHookParam param) throws Throwable {
                            super.afterHookedMethod(param);

                            Context context = (Context) param.getResult();

                            sayHiGetDataService = new SayHiGetDataService(context);

                            register(context);

                        }
                    });
        }

        XposedBridge.hookAllMethods(activityManagerServiceClzz, "systemReady", new XC_MethodHook() {
            @Override
            protected void afterHookedMethod(MethodHookParam param) throws Throwable {
                super.afterHookedMethod(param);

                sayHiGetDataService.systemReady();
            }
        });

    }

    private static void register(Context context) {
        Class serviceManagerClazz = XposedHelpers.findClass("android.os.ServiceManager", null);

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            Object obj =  XposedHelpers.callStaticMethod(
                    serviceManagerClazz,
                    "addService",
                    getServiceName(),
                    sayHiGetDataService,
                    true
            );

        } else {
            callStaticMethod(
                    serviceManagerClazz,
                    "addService",
                    new Class[]{String.class, IBinder.class},
                    getServiceName(),
                    sayHiGetDataService
            );
        }
    }

    private  static void register_one() {
        final Class activityManagerServiceClzz = XposedHelpers.findClass("com.android.server.am.ActivityManagerService", null);
        XposedBridge.hookAllMethods(activityManagerServiceClzz, "main",
                new XC_MethodHook() {
                    @Override
                    protected void afterHookedMethod(MethodHookParam param) throws Throwable {
                        super.afterHookedMethod(param);

                        Context context = (Context) param.getResult();

                        sayHiGetDataService = new SayHiGetDataService(context);

                        register(context);

                    }
                });
    }

    private static void register_two(){
        try {
            Class<?> at = Class.forName("android.app.ActivityThread");
            XposedBridge.hookAllMethods(at, "systemMain", new XC_MethodHook() {
                @Override
                protected void afterHookedMethod(MethodHookParam param) throws Throwable {
                    final ClassLoader loader = Thread.currentThread().getContextClassLoader();
                    final Class<?> activityManagerServiceClzz = Class.forName("com.android.server.am.ActivityManagerService", false, loader);
                    XposedBridge.hookAllConstructors(activityManagerServiceClzz, new XC_MethodHook() {
                        @Override
                        protected void afterHookedMethod(MethodHookParam param) throws Throwable {
                            //Context context = (Context) param.getResult();
                            Context context;
                            Object am = param.thisObject;
                            Field fContext=null;
                            Class<?> cam = am.getClass();
                            while (cam != null && fContext == null) {
                                try {
                                    fContext = cam.getDeclaredField("mContext");
                                } catch (NoSuchFieldException ignored) {
                                    cam = cam.getSuperclass();
                                }
                            }

                          if(fContext==null){
                              XposedBridge.log("activityManagerService.mContext is not find!");
                              return;
                          }else{
                                fContext.setAccessible(true);
                                context = (Context) fContext.get(  am );
                            }

                            sayHiGetDataService = new SayHiGetDataService(context);
                            register(context);
                        }
                    });

                    XposedBridge.hookAllMethods(activityManagerServiceClzz, "systemReady", new XC_MethodHook() {
                        @Override
                        protected void afterHookedMethod(MethodHookParam param) throws Throwable {
                            super.afterHookedMethod(param);

                            sayHiGetDataService.systemReady();
                        }
                    });

                }
            });
        }catch (Exception ex){
            XposedBridge.log( "errrrrror");
        }
    }

}
