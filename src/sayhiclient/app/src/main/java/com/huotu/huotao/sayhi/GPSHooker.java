package com.huotu.huotao.sayhi;

import android.location.GpsStatus;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Build;
import android.util.Log;

import com.huotu.huotao.sayhi.bean.SayHiBean;

import java.lang.reflect.Method;
import java.lang.reflect.Modifier;

import de.robv.android.xposed.IXposedHookLoadPackage;
import de.robv.android.xposed.IXposedHookZygoteInit;
import de.robv.android.xposed.XC_MethodHook;
import de.robv.android.xposed.XposedBridge;
import de.robv.android.xposed.XposedHelpers;
import de.robv.android.xposed.callbacks.XC_LoadPackage.LoadPackageParam;

/**
 * @author Li Jiansong
 * @date:2015-7-15  10:39:39
 * @version :
 *
 *Xprivacy
 *hook com.tencent.mm
 *android.net.wifi.WifiManager.getScanResults
 *android.telephony.TelephonyManagergetCellLocation.getNeighboringCellInfo
 *android.location.LocationManagerrequestLocationUpdatesgetGpsStatus
 *
 *
 */
public class GPSHooker implements IXposedHookLoadPackage , IXposedHookZygoteInit {

	private final String TAG = "FakeXX";
	//private XSharedPreferences mPref;
	private LoadPackageParam mLpp;
	private static SettingsHelper mSettings = new SettingsHelper(BuildConfig.APPLICATION_ID);


	public void log(String s) {
		Log.d(TAG, s);
		XposedBridge.log(s);
	}

	//
	private void hook_method(Class<?> clazz, String methodName, Object... parameterTypesAndCallback) {
		try {
			XposedHelpers.findAndHookMethod(clazz, methodName, parameterTypesAndCallback);
		} catch (Exception e) {
			XposedBridge.log(e);
		}
	}

	//
	private void hook_method(String className, ClassLoader classLoader, String methodName,
							 Object... parameterTypesAndCallback) {
		try {
			XposedHelpers.findAndHookMethod(className, classLoader, methodName, parameterTypesAndCallback);
		} catch (Exception e) {
			XposedBridge.log(e);
		}
	}

	//
	private void hook_methods(String className, String methodName, XC_MethodHook xmh) {
		try {
			Class<?> clazz = Class.forName(className);

			for (Method method : clazz.getDeclaredMethods())
				if (method.getName().equals(methodName)
						&& !Modifier.isAbstract(method.getModifiers())
						&& Modifier.isPublic(method.getModifiers())) {
					XposedBridge.hookMethod(method, xmh);
				}
		} catch (Exception e) {
			XposedBridge.log(e);
		}
	}


	@Override
	public void handleLoadPackage(LoadPackageParam lpp) throws Throwable {
		// TODO Auto-generated method stub
		mLpp = lpp;


		//final Object activityThread = XposedHelpers.callStaticMethod(XposedHelpers.findClass("android.app.ActivityThread", null), "currentActivityThread");
		//final Context systemContext = (Context) XposedHelpers.callMethod(activityThread, "getSystemContext");
		//mSettings = new SettingsHelper(systemContext);

		//XposedBridge.log("handleLoadPackage");

		//XposedBridge.log( mLpp.packageName );

		if (!mLpp.packageName.equals(Constants.WECHAT_APP_PACKAGENAME))
			return;


		//XposedBridge.log("hook com.tencent.mm");
		XposedBridge.log("hook com.tencent.mm");

		hook_method("android.net.wifi.WifiManager", mLpp.classLoader, "getScanResults",
				new XC_MethodHook() {
					/**
					 * Android
					 * android.net.wifi.WifiManagergetScanResults
					 * Return the results of the latest access point scan.
					 * @return the list of access points found in the most recent scan.
					 */
					@Override
					protected void afterHookedMethod(MethodHookParam param)
							throws Throwable {
						// TODO Auto-generated method stub

						//XposedBridge.log("android.net.wifi.WifiManager getScanResults");

						//super.afterHookedMethod(param);
						param.setResult(null);//return empty ap list, force apps using gps information
					}
				});

		hook_method("android.telephony.TelephonyManager", mLpp.classLoader, "getCellLocation",
				new XC_MethodHook() {
					/**
					 * android.telephony.TelephonyManageretCellLocation
					 * Returns the current location of the device.
					 * Return null if current location is not available.
					 */
					@Override
					protected void afterHookedMethod(MethodHookParam param)
							throws Throwable {
						// TODO Auto-generated method stub

						//XposedBridge.log("android.telephony.TelephonyManager getCellLocation");

						//super.afterHookedMethod(param);
						param.setResult(null);//return empty cell id list
					}
				});

		hook_method("android.telephony.TelephonyManager", mLpp.classLoader, "getNeighboringCellInfo",
				new XC_MethodHook() {
					/**
					 * android.telephony.TelephonyManagergetNeighboringCellInfo
					 * Returns the neighboring cell information of the device.
					 */
					@Override
					protected void afterHookedMethod(MethodHookParam param)
							throws Throwable {
						// TODO Auto-generated method stub

						//XposedBridge.log("android.telephony.TelephonyManager getNeighboringCellInfo");

						//super.afterHookedMethod(param);
						param.setResult(null);//// return empty neighboring cell info list
					}
				});

		hook_methods("android.location.LocationManager", "requestLocationUpdates",
				new XC_MethodHook() {
					/**
					 * android.location.LocationManagerrequestLocationUpdates
					 *
					 * String provider, long minTime, float minDistance,LocationListener listener
					 * Register for location updates using the named provider, and a pending intent
					 */
					@Override
					protected void beforeHookedMethod(MethodHookParam param) throws Throwable {

						//XposedBridge.log("android.location.LocationManager requestLocationUpdates");

//				XposedBridge.log("requestLocationUpdates   param.args.length=" + String.valueOf( param.args.length ) );
//				for( int i =0;i<param.args.length;i++) {
//					XposedBridge.log("param.args["+ String.valueOf(i) +"]=" + param.args[i].toString() );
//				}

						if (param.args.length >= 4 && (param.args[0] instanceof String)) {

							//
							LocationListener ll = (LocationListener) param.args[3];

							Class<?> clazz = LocationListener.class;
							Method m = null;
							for (Method method : clazz.getDeclaredMethods()) {
								if (method.getName().equals("onLocationChanged")) {
									m = method;
									break;
								}
							}

							try {
								if (m != null) {

//							SayHiBean sayHiBean = LocationUtils.mockLocation();
//							if(sayHiBean!=null) {
//								XposedBridge.log("lat=" + sayHiBean.getLatitude() + " , lon=" + sayHiBean.getLongitude());
//							}else{
//								XposedBridge.log("无法读取配置信息");
//							}

									mSettings.reload();

									Object[] args = new Object[1];
									Location l = new Location(LocationManager.GPS_PROVIDER);


									String json = mSettings.getString(Constants.PARAMETER_SAYHIDATA, "");
									XposedBridge.log("json=" + json);
									SayHiBean model = WechatUtils.getGson().fromJson(json, SayHiBean.class);
									if (model == null) {
										return;
									}




									String latStr = model.getLatitude(); //mSettings.getString( Constants.PARAMETER_LATITUDE  ,"39.862559");
									String lonStr = model.getLongitude(); //mSettings.getString( Constants.PARAMETER_LONGITUDE  , "116.449535");
									XposedBridge.log("lat=" + latStr + " , longitude=" + lonStr);


									double la = Double.parseDouble(latStr); //Double.parseDouble(mSettings.getString("latitude", "39.862559"));
									double lo = Double.parseDouble(lonStr); //Double.parseDouble(mSettings.getString("longitude","116.449535"));

//							double la=39.862559;
//							double lo=116.449535;
									l.setLatitude(la);//
									l.setLongitude(lo);
									l.setAccuracy(10);
									l.setTime(System.currentTimeMillis());
									if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR1) {
										l.setElapsedRealtimeNanos(l.getTime());
									}

									args[0] = l;

									//invoke onLocationChanged directly to pass location infomation
									m.invoke(ll, args);

									//XposedBridge.log("fake location: " + la + ", " + lo);
								}
							} catch (Exception e) {
								XposedBridge.log(e);
							}
						}
					}
				});


		hook_methods("android.location.LocationManager", "getGpsStatus",
				new XC_MethodHook() {
					/**
					 * android.location.LocationManagergetGpsStatus
					 * GpsStatus status
					 * Retrieves information about the current status of the GPS engine.
					 * This should only be called from the {@link GpsStatus.Listener#onGpsStatusChanged}
					 * callback to ensure that the data is copied atomically.
					 *
					 */
					@Override
					protected void afterHookedMethod(MethodHookParam param) throws Throwable {

						//XposedBridge.log("android.location.LocationManager getGpsStatus");

						GpsStatus gss = (GpsStatus) param.getResult();
						if (gss == null)
							return;

						Class<?> clazz = GpsStatus.class;
						Method m = null;
						for (Method method : clazz.getDeclaredMethods()) {
							if (method.getName().equals("setStatus")) {

//						XposedBridge.log( "setStatus.getParameterTypes.length" + String.valueOf( method.getParameterTypes().length ) );
//						for( int i=0;i< method.getParameterTypes().length;i++){
//							XposedBridge.log(" parameter[" + String.valueOf(i)+"]=" + method.getParameterTypes()[i].toString()  );
//						}

								if (method.getParameterTypes().length > 1) {
									m = method;
									break;
								}
							}
						}

						//access the private setStatus function of GpsStatus
						m.setAccessible(true);

						//make the apps belive GPS works fine now
						int svCount = 5;
						int[] prns = {1, 2, 3, 4, 5};
						float[] snrs = {0, 0, 0, 0, 0};
						float[] elevations = {0, 0, 0, 0, 0};
						float[] azimuths = {0, 0, 0, 0, 0};
						int ephemerisMask = 0x1f;
						int almanacMask = 0x1f;

						//5 satellites are fixed
						int usedInFixMask = 0x1f;


						try {
							if (m != null) {

								m.invoke(gss, svCount, prns, snrs, elevations, azimuths, ephemerisMask, almanacMask, usedInFixMask, prns);
								param.setResult(gss);
							}
						} catch (Exception e) {
							XposedBridge.log(e);
						}
					}
				});
	}


	@Override
	public void initZygote(StartupParam startupParam) throws Throwable {

		XposedBridge.log("-----------------initZygote---------------");

		SayHiGetDataService.register();
	}
}
