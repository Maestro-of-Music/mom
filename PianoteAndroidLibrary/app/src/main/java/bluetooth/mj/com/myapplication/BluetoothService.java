package bluetooth.mj.com.myapplication;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothServerSocket;
import android.bluetooth.BluetoothSocket;
import android.content.Context;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.annotation.RequiresPermission;
import android.util.Log;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.LinkedList;
import java.util.Queue;
import java.util.UUID;

/**
 * Created by songmyeongjin on 2017. 10. 10..
 */

public class BluetoothService {
    private static final String TAG = "BluetoothService";
    private static final UUID MY_UUID = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB");

    private BluetoothAdapter mBtAdapter;

    private Handler mHandler;

    private ConnectThread mConnectThread;

    private ConnectedThread mConnectedThread;

    private AcceptThread mAcceptThread;

    private int mState;
    private static final int STATE_NONE = 0;
    private static final int STATE_LISTEN = 1;
    private static final int STATE_CONNECTING = 2;
    private static final int STATE_CONNECTED = 3;
    public static final String DEVICE_NAME = "device_name";
    public static final String TOAST = "toast";

    private Queue<byte[]> bufferQueue = new LinkedList<byte[]>();


    public BluetoothService(Handler h)
    {
        mHandler = h;
        mBtAdapter = BluetoothAdapter.getDefaultAdapter();
    }

    public boolean getDeviceState() {
        Log.i("BluetoothService", "Check the Bluetooth support");

        if (mBtAdapter == null) {
            Log.d("BluetoothService", "Bluetooth is not available");
            return false;
        }

        Log.d("BluetoothService", "Bluetooth is available");
        return true;
    }

    private synchronized void setState(int state)
    {
        mState = state;
        mHandler.obtainMessage(1, state, -1).sendToTarget();
        Log.d("BluetoothService", "setState() " + mState + " -> " + state);
        mState = state;
    }

    public synchronized int getState() {
        return mState;
    }

    @RequiresPermission("android.permission.BLUETOOTH")
    public synchronized void start() {
        Log.d("BluetoothService", "start");


        if (mConnectThread != null) {
            mConnectThread.cancel();
            mConnectThread = null;
        }


        if (mConnectedThread != null) {
            mConnectedThread.cancel();
            mConnectedThread = null;
        }


        if (mAcceptThread != null)
        {

        }
        else {
            mAcceptThread = new AcceptThread();
            mAcceptThread.start();
        }

        setState(STATE_LISTEN);
    }

    @RequiresPermission("android.permission.BLUETOOTH")
    public synchronized void connect(BluetoothDevice device) {
        Log.d("BluetoothService", "connect to: " + device);


        if (mState == STATE_CONNECTING) {
            if (mConnectThread == null) {

            } else {
                mConnectThread.cancel();
                mConnectThread = null;
            }
        }


        if (mConnectedThread == null) {

        } else {
            mConnectedThread.cancel();
            mConnectedThread = null;
        }


        mConnectThread = new ConnectThread(device);
        mConnectThread.start();

        setState(STATE_CONNECTING);
    }

    @RequiresPermission("android.permission.BLUETOOTH")
    public synchronized void connected(BluetoothSocket socket, BluetoothDevice device)
    {
        Log.d("BluetoothService", "connected");


        if (mConnectThread != null) {
            mConnectThread.cancel();
            mConnectThread = null;
        }


        if (mConnectedThread != null) {
            mConnectedThread.cancel();
            mConnectedThread = null;
        }


        if (mAcceptThread != null) {
            mAcceptThread.cancel();
            mAcceptThread = null;
        }


        mConnectedThread = new ConnectedThread(socket);
        mConnectedThread.start();
        Message msg = mHandler.obtainMessage(BluetoothPlugin.MESSAGE_DEVICE_NAME);
        Bundle bundle = new Bundle();
        bundle.putString("device_name", device.getName());
        msg.setData(bundle);
        mHandler.sendMessage(msg);
        setState(STATE_CONNECTED);
    }

    public synchronized void stop() {
        Log.d("BluetoothService", "stop");

        if (mConnectThread != null) {
            mConnectThread.cancel();
            mConnectThread = null;
        }

        if (mConnectedThread != null) {
            mConnectedThread.cancel();
            mConnectedThread = null;
        }

        if (mAcceptThread != null) {
            mAcceptThread.cancel();
            mAcceptThread = null;
        }

        setState(0);
    }

    public void write(byte[] out) {
        ConnectedThread r;
        synchronized (this) {
            if (mState != 3)
                return;
            r = mConnectedThread;
        }
        r.write(out);
    }

    private void connectionFailed() {
        setState(STATE_LISTEN);
        Message msg = mHandler.obtainMessage(BluetoothPlugin.MESSAGE_TOAST);
        Bundle bundle = new Bundle();
        bundle.putString(this.TOAST, "Unable to connect device");
        msg.setData(bundle);
        mHandler.sendMessage(msg);
    }

    private void connectionLost() {
        setState(STATE_LISTEN);
        Message msg = mHandler.obtainMessage(BluetoothPlugin.MESSAGE_TOAST);
        Bundle bundle = new Bundle();
        bundle.putString("toast", "Device connection was lost");
        msg.setData(bundle);
        mHandler.sendMessage(msg);
    }

    private class AcceptThread extends Thread {
        private final BluetoothServerSocket mmServerSocket;

        public AcceptThread() {
            BluetoothServerSocket tmp = null;
            try
            {
                tmp = mBtAdapter.listenUsingRfcommWithServiceRecord("BluetoothPlugin", BluetoothService.MY_UUID);
            } catch (IOException e) {
                Log.e("BluetoothService", "listen() failed", e);
            }

            mmServerSocket = tmp;
        }

        @RequiresPermission("android.permission.BLUETOOTH")
        public void run() {
            Log.d("BluetoothService", "Accept Thread Begin");
            setName("AcceptThread");
            BluetoothSocket socket = null;

            while (mState != 3) {
                try {
                    socket = mmServerSocket.accept();
                } catch (IOException e1) {
                    Log.e("BluetoothService", "accept() failed", e1);
                    break;
                }

                if (socket != null) {
                    BluetoothService e = BluetoothService.this;
                    synchronized (BluetoothService.this) {
                        switch (mState) {
                            case 0:
                            case 3:
                                try {
                                    socket.close();
                                } catch (IOException e2) {
                                    Log.e("BluetoothService", "Could not close unwanted socket", e2);
                                }

                            case 1:
                            case 2:
                                connected(socket, socket.getRemoteDevice());
                        }

                    }
                }
            }
            Log.i("BluetoothService", "Accept Thread End");
        }

        public void cancel() {
            Log.d("BluetoothService", "cancel " + this);
            try
            {
                mmServerSocket.close();
            } catch (IOException e) {
                Log.e("BluetoothService", "close() of server failed", e);
            }
        }
    }

    private class ConnectThread extends Thread
    {
        private final BluetoothSocket mmSocket;
        private final BluetoothDevice mmDevice;

        public ConnectThread(BluetoothDevice device) {
            mmDevice = device;
            BluetoothSocket tmp = null;
            try
            {
                tmp = device.createRfcommSocketToServiceRecord(BluetoothService.MY_UUID);
            } catch (IOException e) {
                Log.e("BluetoothService", "create() failed", e);
            }
            mmSocket = tmp;
        }

        public void run() {
            Log.i("BluetoothService", "Connect Thread Begin");
            setName("ConnectThread");
            mBtAdapter.cancelDiscovery();

            try
            {
                mmSocket.connect();
                Log.d("BluetoothService", "Connect Success");
            }
            catch (IOException e) {
                BluetoothService.this.connectionFailed();
                Log.d("BluetoothService", "Connect Fail");
                try
                {
                    mmSocket.close();
                } catch (IOException e2) {
                    Log.e("BluetoothService", "unable to close() socket during connection failure", e2);
                }

                start();
                return;
            }

            synchronized (BluetoothService.this) {
                mConnectThread = null;
            }

            connected(mmSocket, mmDevice);
        }

        public void cancel() {
            try {
                mmSocket.close();
            } catch (IOException e) {
                Log.e("BluetoothService", "close() of connect socket failed", e);
            }
        }
    }

    private class ConnectedThread extends Thread {
        private final BluetoothSocket mmSocket;
        private final InputStream mmInStream;
        private final OutputStream mmOutStream;

        public ConnectedThread(BluetoothSocket socket) {
            Log.d("BluetoothService", "create ConnectedThread");
            mmSocket = socket;
            InputStream tmpIn = null;
            OutputStream tmpOut = null;
            try
            {
                tmpIn = socket.getInputStream();
                tmpOut = socket.getOutputStream();
            } catch (IOException e) {
                Log.e("BluetoothService", "temp sockets not created", e);
            }

            mmInStream = tmpIn;
            mmOutStream = tmpOut;
        }

        public void run() {
            Log.i("BluetoothService", "BEGIN mConnectedThread");
            byte[] buffer = new byte[1024];
            int bytes;

            while (true) {
                try {
                    bytes = mmInStream.read(buffer);
                    bufferQueue.offer(new byte[bytes]);
                    System.arraycopy(buffer, 0, bufferQueue.peek(), 0, bytes);
                    mHandler.obtainMessage(BluetoothPlugin.MESSAGE_READ, bytes, -1, bufferQueue.poll()).sendToTarget();
                } catch (IOException e) {
                    Log.e(TAG, "disconnected", e);
                    connectionLost();
                    break;
                }
            }
        }

        public void write(byte[] buffer)
        {
            try
            {

                String a = String.valueOf(buffer.length);
                Log.e("Bluetooth Check 2 " , a);
                mmOutStream.write(buffer);


                mHandler.obtainMessage(BluetoothPlugin.MESSAGE_WRITE, -1, -1, buffer)
                        .sendToTarget();

            } catch (IOException e) {
                Log.e("BluetoothService", "Exception during write", e);
            }
        }

        public void cancel() {
            try {
                mmSocket.close();
            } catch (IOException e) {
                Log.e("BluetoothService", "close() of connect socket failed", e);
            }
        }
    }
}
