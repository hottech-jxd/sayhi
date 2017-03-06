package com.huotu.huotao.sayhi;

import android.text.TextUtils;
import android.util.Log;

import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.TreeMap;
import org.apache.commons.codec.binary.Hex;
/**
 * Created by Administrator on 2017/2/23.
 */

public class SignUtils {

    public static String getSign(Map map){
            String secret = BuildConfig.APPSECRET;
            String values = doSort(map, secret);
            Log.i("sign", values);

            String signHex = encryptMd532(values);
            Log.i("signHex", signHex);
            return signHex;

    }

    /**
     * @throws
     * @方法描述：获取sign码第二步：参数排序
     * @方法名：doSort
     * @参数：@param map
     * @参数：@return
     * @返回：String
     */
    private static String doSort(Map<String, String> map, String secret) {
        //将MAP中的key转成小写
        Map<String, String> lowerMap = new HashMap<String, String>();
        Iterator lowerIt = map.entrySet().iterator();
        while (lowerIt.hasNext()) {
            Map.Entry entry = (Map.Entry) lowerIt.next();
            Object value = entry.getValue();
            if (!TextUtils.isEmpty(String.valueOf(value))) {
                lowerMap.put(String.valueOf(entry.getKey()).toLowerCase(), String.valueOf(value));
            }
        }

        TreeMap<String, String> treeMap = new TreeMap<String, String>(lowerMap);
        StringBuffer buffer = new StringBuffer();
        Iterator it = treeMap.entrySet().iterator();
        while (it.hasNext()) {
            Map.Entry entry = (Map.Entry) it.next();
            buffer.append(entry.getKey() + "=");
            buffer.append(entry.getValue() + "&");
        }
        String suffix = buffer.substring(0, buffer.length() - 1) + secret;//Constants.getAPP_SECRET();
        return suffix;
    }

    public static String encryptMd532(String source) {
        if (null == source || "".equals(source.trim())) {
            return null;
        } else {
            try {
                MessageDigest messageDigest = MessageDigest.getInstance("MD5");
                messageDigest.update(source.getBytes("utf-8"));
                byte[] s1 = messageDigest.digest();
                String tem = new String(Hex.encodeHex(s1)).toLowerCase();
                Log.i("test>>>>>>>>", tem);
                return tem;
            } catch (UnsupportedEncodingException ex) {
                return "";
            } catch (NoSuchAlgorithmException ex2) {
                return "";
            }
        }
    }

}
