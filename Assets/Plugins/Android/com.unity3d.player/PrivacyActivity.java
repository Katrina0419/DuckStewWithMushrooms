package com.unity3d.player;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.webkit.WebView;
 
public class PrivacyActivity extends Activity implements DialogInterface.OnClickListener {

   // ��˽Э������
   final String privacyContext = 
             "��ӭʹ�ñ���Ϸ����ʹ�ñ���Ϸǰ����������Ķ������" +
             "���û�Э�顷</a>��<a href=\">����˽���ߡ�</a>����;\n" +
     "1.�����û���˽�Ǳ���Ϸ��һ��������ߣ�����Ϸ����й¶���ĸ�����Ϣ��\n" +
     "2.���ǻ������ʹ�õľ��幦����Ҫ���ռ���Ҫ���û���Ϣ���������豸��Ϣ���洢�����Ȩ�ޣ���\n" +
     "3.����ͬ��App��˽���ߺ����ǽ����м���SDK�ĳ�ʼ�����������ռ�����android_id�����������Ա���App��������ͳ�ƺͰ�ȫ��أ�\n" +
     "4.Ϊ�˷������Ĳ��ģ�������ͨ�������á����²鿴��Э�飻\n" +
     "5.�������Ķ����������˽���������˽���������ʹ�����Ȩ�޵�������Լ�����������˽�ı�����ʩ��";
     
    
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
  
        // ����Ѿ�ͬ�����˽Э����ֱ�ӽ���Unity Activity
        if (GetPrivacyAccept()){
            EnterUnityActivity();
            return;
        }
        // ������˽Э��Ի���
        ShowPrivacyDialog();
    }
 
    // ��ʾ��˽Э��Ի���
    private void ShowPrivacyDialog(){
        WebView webView = new WebView(this);
        webView.loadData(privacyContext, "text/html", "utf-8");         
        AlertDialog.Builder privacyDialog = new AlertDialog.Builder(this);
        privacyDialog.setCancelable(false);
        privacyDialog.setView(webView);
        privacyDialog.setTitle("��ʾ");
        privacyDialog.setNegativeButton("�ܾ�",this);
        privacyDialog.setPositiveButton("ͬ��",this);
        privacyDialog.create().show();
    }
    
    @Override
    public void onClick(DialogInterface dialogInterface, int i) {
        switch (i){
            case AlertDialog.BUTTON_POSITIVE://���ͬ�ⰴť
                SetPrivacyAccept(true);
                EnterUnityActivity(); //����Unity Activity
                break;
            case AlertDialog.BUTTON_NEGATIVE://����ܾ���ť,ֱ���˳�App
                finish();
                break;
        }
    }
    
    // ����Unity Activity
    private void EnterUnityActivity(){
        Intent unityAct = new Intent();
        unityAct.setClassName(this, "com.unity3d.player.UnityPlayerActivity");
        this.startActivity(unityAct);
    }
    
    // ���ش洢����ͬ����˽Э��״̬
    private void SetPrivacyAccept(boolean accepted){
        SharedPreferences.Editor prefs = this.getSharedPreferences("PlayerPrefs", MODE_PRIVATE).edit();
        prefs.putBoolean("PrivacyAcceptedKey", accepted);
        prefs.apply();
    }
    
    // ��ȡ�Ƿ��Ѿ�ͬ���
    private boolean GetPrivacyAccept(){
        SharedPreferences prefs = this.getSharedPreferences("PlayerPrefs", MODE_PRIVATE);
        return prefs.getBoolean("PrivacyAcceptedKey", false);
    }
}