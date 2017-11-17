package bluetooth.mj.com.myapplication;

import android.Manifest;
import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.provider.MediaStore;
import android.provider.Settings;
import android.support.annotation.NonNull;
import android.support.annotation.RequiresPermission;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.util.Log;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import java.io.File;
import java.util.ArrayList;
import java.util.Set;

/**
 * Created by songmyeongjin on 2017. 10. 9..
 */

public class BluetoothPlugin extends UnityPlayerActivity {

    public static final int MESSAGE_STATE_CHANGE = 1;
    public static final int MESSAGE_READ = 2;
    public static final int MESSAGE_WRITE = 3;
    public static final int MESSAGE_DEVICE_NAME = 4;
    public static final int MESSAGE_TOAST = 5;

    private static final int REQUEST_CONNECT_DEVICE = 1;
    private static final int REQUEST_ENABLE_BT = 2;

    private static final int PICK_FROM_CAMERA = 0;
    private static final int PICK_FROM_ALBUM = 1;
    private static final int CROP_FROM_CAMERA = 2;

    private Uri mTakePhotoUri;

    private static final String TAG = "BluetoothPlugin";
    private static final String TARGET = "BluetoothModel";
    private boolean IsScan = false;

    private String mConnectedDeviceName = null;

    private StringBuffer mOutStringBuffer;
    private BluetoothAdapter mBtAdapter = null;
    private BluetoothService mBtService = null;

    private ArrayList<String> singleAddress = new ArrayList();

    @RequiresPermission("android.permission.READ_EXTERNAL_STORAGE")
    private void TakePhotoByGallery() {
        OnCallMethod();
    }
    public void OnCallMethod(){
        Log.e("TakePhotoByGallery","Gallery opened!");
        Intent intent = new Intent(Intent.ACTION_PICK, MediaStore.Images.Media.EXTERNAL_CONTENT_URI);
        intent.setType("image/*");
        intent.setAction(Intent.ACTION_GET_CONTENT);
        startActivityForResult(intent, PICK_FROM_ALBUM);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        Log.e("resultCode:", String.valueOf(resultCode));

        if(requestCode == PICK_FROM_ALBUM){
            if(resultCode == Activity.RESULT_OK && null != data){
                Uri selectedImage = data.getData();

                String UrlPath = getRealPathFromURI(selectedImage);
                Log.e("UrlPath",getRealPathFromURI(selectedImage));

                UnityPlayer.UnitySendMessage("BluetoothController","OnReceiveGallery",UrlPath);

                //Url Path를 유니티로 전달
            }
        }else{
            Log.e("Receive",String.valueOf(resultCode));
        }

    }




    public String getRealPathFromURI(Uri contentUri) {
        String[] proj = { MediaStore.Images.Media.DATA };
        Cursor cursor = managedQuery(contentUri, proj, null, null, null);
        int column_index = cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA);
        cursor.moveToFirst();
        return cursor.getString(column_index);
    }

    private final Handler mHandler = new Handler() {
        public void handleMessage(Message msg) {
            switch (msg.what) {
                case 1:
                    UnityPlayer.UnitySendMessage("BluetoothModel", "OnStateChanged", String.valueOf(msg.arg1));
                    break;
                case 2:
                    byte[] readBuf = (byte[])msg.obj;
                    String readMessage = new String(readBuf, 0, msg.arg1);
                    UnityPlayer.UnitySendMessage("BluetoothModel", "OnReadMessage", readMessage);
                    break;
                case 3:
                    byte[] writeBuf = (byte[])msg.obj;
                    String writeMessage = new String(writeBuf);
                    UnityPlayer.UnitySendMessage("BluetoothModel", "OnSendMessage",writeMessage);

                    break;
                case 4:
                    mConnectedDeviceName = msg.getData().getString("device_name");
                    Toast.makeText(getApplicationContext(), "Connected to " + mConnectedDeviceName, Toast.LENGTH_SHORT).show();
                    break;
                case 5:
                    Toast.makeText(getApplicationContext(), msg.getData().getString("toast"), Toast.LENGTH_SHORT).show();
            }

        }
    };


    private final BroadcastReceiver mReceiver = new BroadcastReceiver() {
        @RequiresPermission("android.permission.BLUETOOTH")
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            if ("android.bluetooth.device.action.FOUND".equals(action)) {
                BluetoothDevice device = (BluetoothDevice)intent.getParcelableExtra("android.bluetooth.device.extra.DEVICE");
                singleAddress.add(device.getName() + "\n" + device.getAddress());
                UnityPlayer.UnitySendMessage("BluetoothModel", "OnFoundDevice", device.getName() + ",\n" + device.getAddress());
            }
            else if ("android.bluetooth.adapter.action.DISCOVERY_FINISHED".equals(action)) {
                if (IsScan) {
                    UnityPlayer.UnitySendMessage("BluetoothModel", "OnScanFinish", "");
                }

                if (singleAddress.size() == 0) {
                    UnityPlayer.UnitySendMessage("BluetoothModel", "OnFoundNoDevice", "");
                }
            }
        }
    };

    public BluetoothPlugin() {}

    @RequiresPermission("android.permission.BLUETOOTH")
    public void StartPlugin() {
        if (Looper.myLooper() == null) {
            Looper.prepare();
        }

        SetupPlugin();
    }




    @RequiresPermission("android.permission.BLUETOOTH")
    public String SetupPlugin()
    {
        mBtAdapter = BluetoothAdapter.getDefaultAdapter();


        if (mBtAdapter == null) {
            return "Bluetooth is not available";
        }

        if (mBtService == null) {
            startService();
        }

        return "SUCCESS";
    }



    private void startService()
    {
        Log.d(TAG, "setupService()");
        this.mBtService = new BluetoothService(this.mHandler);
        this.mOutStringBuffer = new StringBuffer("");
    }

    public String DeviceName() {
        return mBtAdapter.getName();
    }

    @RequiresPermission("android.permission.BLUETOOTH")
    public String GetDeviceConnectedName() {
        return mBtService.getState() != 3 ? "Not Connected" : !mBtAdapter.isEnabled() ? "You Must Enable The BlueTooth" : mConnectedDeviceName;
    }

    @RequiresPermission("android.permission.BLUETOOTH")
    public boolean IsEnabled() {
        return mBtAdapter.isEnabled();
    }

    public boolean IsConnected() {
        return mBtService.getState() == 3;
    }

    @RequiresPermission("android.permission.BLUETOOTH")
    public void stopThread() {
        Log.d("BluetoothPlugin", "stop");
        if (mBtService != null) {
            mBtService.stop();
            mBtService = null;
        }

        if (mBtAdapter != null) {
            mBtAdapter = null;
        }

        SetupPlugin();
    }


    @RequiresPermission(allOf={"android.permission.BLUETOOTH", "android.permission.BLUETOOTH_ADMIN"})
    public void Connect(String TheAdrees)
    {
        if (mBtAdapter.isDiscovering()) {
            mBtAdapter.cancelDiscovery();
        }

        IsScan = false;
        String address = TheAdrees.substring(TheAdrees.length() - 17);
        mConnectedDeviceName = TheAdrees.split(",")[0];
        BluetoothDevice device = mBtAdapter.getRemoteDevice(address);

        mBtService.connect(device);
    }


    @RequiresPermission(allOf={"android.permission.BLUETOOTH", "android.permission.BLUETOOTH_ADMIN"})
    String ScanDevice()
    {
        Log.d("BluetoothPlugin", "Start - ScanDevice()");
        if (!mBtAdapter.isEnabled()) {
            return "You Must Enable The BlueTooth";
        }
        IsScan = true;
        singleAddress.clear();
        IntentFilter filter = new IntentFilter("android.bluetooth.device.action.FOUND");
        registerReceiver(mReceiver, filter);
        filter = new IntentFilter("android.bluetooth.adapter.action.DISCOVERY_FINISHED");
        registerReceiver(mReceiver, filter);
        mBtAdapter = BluetoothAdapter.getDefaultAdapter();
        Set pairedDevices = mBtAdapter.getBondedDevices();
        if (pairedDevices.size() > 0) {}



        doDiscovery();
        return "SUCCESS";
    }



    @RequiresPermission(allOf={"android.permission.BLUETOOTH", "android.permission.BLUETOOTH_ADMIN"})
    private void doDiscovery()
    {
        Log.d("BluetoothPlugin", "doDiscovery()");
        if (mBtAdapter.isDiscovering()) {
            mBtAdapter.cancelDiscovery();
        }

        mBtAdapter.startDiscovery();
    }


    @RequiresPermission(allOf={"android.permission.BLUETOOTH", "android.permission.BLUETOOTH_ADMIN"})
    String BluetoothSetName(String name)
    {
        if (!mBtAdapter.isEnabled())
            return "You Must Enable The BlueTooth";
        if (mBtService.getState() != 3) {
            return "Not Connected";
        }
        mBtAdapter.setName(name);
        return "SUCCESS";
    }



    @RequiresPermission(allOf={"android.permission.BLUETOOTH", "android.permission.BLUETOOTH_ADMIN"})
    String DisableBluetooth()
    {
        if (!mBtAdapter.isEnabled()) {
            return "You Must Enable The BlueTooth";
        }
        if (mBtAdapter != null) {
            mBtAdapter.cancelDiscovery();
        }

        if (mBtAdapter.isEnabled()) {
            mBtAdapter.disable();
        }

        return "SUCCESS";
    }

    @RequiresPermission("android.permission.BLUETOOTH")
    public String BluetoothEnable()
    {
        try {
            if (!mBtAdapter.isEnabled()) {
                Intent e = new Intent("android.bluetooth.adapter.action.REQUEST_ENABLE");
                startActivityForResult(e, 2);
            }

            return "SUCCESS";
        }
        catch (Exception e) {}
        return "Faild";
    }

    public void showMessage(final String message)
    {
        runOnUiThread(new Runnable() {
            public void run() {
                Toast.makeText(BluetoothPlugin.this, message, Toast.LENGTH_SHORT).show();
            }
        });
    }

    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.e("BluetoothPlugin", "+++ ON CREATE +++");
        /*
        int permissionCheck = ContextCompat.checkSelfPermission(this,
                Manifest.permission.READ_EXTERNAL_STORAGE);

        Log.e("BluetoothPermission",String.valueOf(permissionCheck));
        */
    }


    public void onStart() {
        super.onStart();
    }

    public synchronized void onPause() {
        super.onPause();
        Log.e("BluetoothPlugin", "- ON PAUSE -");
    }

    @RequiresPermission("android.permission.BLUETOOTH")
    public synchronized void onResume() {
        super.onResume();
        Log.d("BluetoothPlugin", "+ ON RESUME +");
        if ((mBtService != null) && (mBtService.getState() == 0)) {
            mBtService.start();
        }
    }

    public void onStop()
    {
        super.onStop();
        Log.e("BluetoothPlugin", "-- ON STOP --");
    }

    public void onDestroy() {
        super.onDestroy();
        if (mBtService != null) {
            mBtService.stop();
        }

        Log.e("BluetoothPlugin", "--- ON DESTROY ---");
    }

    @RequiresPermission("android.permission.BLUETOOTH")
    public String ensureDiscoverable() {
        if (!mBtAdapter.isEnabled()) {
            return "You Must Enable The BlueTooth";
        }
        if (mBtAdapter.getScanMode() != 23) {
            Intent discoverableIntent = new Intent("android.bluetooth.adapter.action.REQUEST_DISCOVERABLE");
            discoverableIntent.putExtra("android.bluetooth.adapter.extra.DISCOVERABLE_DURATION", 300);
            startActivity(discoverableIntent);
        }

        return "SUCCESS";
    }

    @RequiresPermission("android.permission.BLUETOOTH")
    public String sendMessage(String message)
    {
        if (!mBtAdapter.isEnabled()) {
            return "You Must Enable The BlueTooth";
        }
        if (mBtService.getState() != 3) {
            return "Not Connected";
        }

        if (message.length() > 0) {
            byte[] send = message.getBytes();
            Log.e("Bluetooth Check " , message);
            mBtService.write(send);
            mOutStringBuffer.setLength(0);
        }

        return "SUCCESS";
    }
}
