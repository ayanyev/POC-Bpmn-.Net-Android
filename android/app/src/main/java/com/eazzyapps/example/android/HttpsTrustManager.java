package com.eazzyapps.example.android;

import java.net.Socket;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.cert.CertificateException;

import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLEngine;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509ExtendedTrustManager;
import javax.security.cert.X509Certificate;

public class HttpsTrustManager extends X509ExtendedTrustManager {
    private static TrustManager[] trustManagers;
    private static final java.security.cert.X509Certificate[] _AcceptedIssuers = new java.security.cert.X509Certificate[]{};

    @Override
    public void checkClientTrusted(java.security.cert.X509Certificate[] chain, String authType) throws java.security.cert.CertificateException {

    }

    @Override
    public void checkServerTrusted(java.security.cert.X509Certificate[] chain, String authType) throws java.security.cert.CertificateException {

    }

    @Override
    public void checkClientTrusted(java.security.cert.X509Certificate[] chain, String authType, Socket socket) throws CertificateException {

    }

    @Override
    public void checkServerTrusted(java.security.cert.X509Certificate[] chain, String authType, Socket socket) throws CertificateException {

    }

    @Override
    public void checkClientTrusted(java.security.cert.X509Certificate[] chain, String authType, SSLEngine engine) throws CertificateException {

    }

    @Override
    public void checkServerTrusted(java.security.cert.X509Certificate[] chain, String authType, SSLEngine engine) throws CertificateException {

    }

    public boolean isClientTrusted(X509Certificate[] chain) {
        return true;
    }

    public boolean isServerTrusted(X509Certificate[] chain) {
        return true;
    }

    @Override
    public java.security.cert.X509Certificate[] getAcceptedIssuers() {
        return _AcceptedIssuers;
    }

    public static void allowAllSSL() {

        HttpsURLConnection.setDefaultHostnameVerifier((arg0, arg1) -> true);

        SSLContext context = null;
        if (trustManagers == null) {
            trustManagers = new TrustManager[]{
                    new HttpsTrustManager()
            };
        }

        try {
            context = SSLContext.getInstance("TLS");
            context.init(null, trustManagers, new SecureRandom());
        } catch (NoSuchAlgorithmException | KeyManagementException e) {
            e.printStackTrace();
        }

        HttpsURLConnection.setDefaultSSLSocketFactory(context != null ? context.getSocketFactory() : null);
    }
}