using System;
using System.Threading;
using UnityEngine;
using System.Timers;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
//using System.Collections;

public class ClientNetMgr
{
    public ClientNetMgr()
    {
        m_Singleton = this;
        m_ip = "192.168.1.68";
        m_nPort = 6001;
        m_tcpClient = null;
        m_byteDataList = new byte[DataDefine.MAX_PACKET_SIZE];//���������б�

        //		m_Behaviour = GameObject.Find("MessageObjectBase").GetComponent<CBehaviour>();
        //Debug.Log( m_Behaviour );

        m_ReceiveDataArray = new ArrayList();//���������б�
        m_DelayReceiveDataArray = new ArrayList();//�������б�
        m_bConnectState = false;
		m_bServerNet = true;

	    bReConnect = false;
        m_ReceiveThread = null;
    }

    public static ClientNetMgr GetSingle()
    {
        if (m_Singleton == null)
            new ClientNetMgr();
        return m_Singleton;
    }

    //connect
    public bool StartConnect(string strIp, int nPort)
    {

        m_ip = strIp;
        m_nPort = nPort;

        //Close();
      //  Debug.Log("StartConnect......");
        if (ReConnect())
        {
            //create send data thread
            bool b = ClientSendDataMgr.GetSingle().CreateSendThread();
            m_ReceiveThread = new Thread(new ThreadStart(StartReceiveData));
            m_ReceiveThread.Start();
            return b;
        }
        return false;
    }

    public bool ReConnect()
    {
        CloseSocket();
        m_tcpClient = new System.Net.Sockets.TcpClient();
        try
        {
            m_tcpClient.Connect(m_ip, m_nPort);
        }
        catch (System.Exception ex)
        {
            //	MessageBox.ShowOK( "Connect error:"+ex.Message, Login_LoadAccount );
        }
        if (m_tcpClient != null)
        {
            if (IsConnect())
            {
                m_NetworkStream = m_tcpClient.GetStream();
                //Start Receive data
                //StartReceiveData();

                m_bConnectState = true;
         //       Debug.Log("Connect server ok!!!");
                return true;
            }
            CloseSocket();
        }
        return false;
    }



    public void Close()
    {
        CloseSocket();
        ClientSendDataMgr.GetSingle().CloseSendThread();
    }

    private void StartReceiveData()
    {
        if (IsConnect() && m_NetworkStream != null)
            m_NetworkStream.BeginRead(m_byteDataList, 0, DataDefine.MAX_PACKET_SIZE,
                                        ReceiveMessage, null);
    }

    //�ж������Ƿ�����
    public bool IsConnect()
    {
        if (m_tcpClient != null && m_tcpClient.Connected)
            return true;
        return false;
    }
    //networkstream
    public System.Net.Sockets.NetworkStream GetNetworkStream()
    {
        return m_NetworkStream;
    }

    //Close Socket
    private void CloseSocket()
    {
        if (m_NetworkStream != null)
        {
            m_NetworkStream.Close();
            m_NetworkStream = null;
        }
        if (m_tcpClient != null)
        {
            m_tcpClient.Close();
            m_tcpClient = null;
        }
        m_ReceiveThread = null;
       
    }

    //receive
    private void ReceiveMessage(System.IAsyncResult ar)
    {
        try
        {
            if (IsConnect() && m_NetworkStream != null)
            {
                lock (m_NetworkStream)
                {
                    int nRecLength = m_NetworkStream.EndRead(ar);
                    if ( nRecLength <= 0 )
                    {
                        Debug.Log( "<color=#FF4040>server off!!</color>" );
                        Close();
						m_bServerNet = false;
						bReConnect = false;
                      //  Application.Quit();
                        return;
                    }
                    else
                    {
                        // Debug.Log( "<color=#FF4040>ReceiveMessageLen:</color>" + nRecLength );
                        //add to main data
                        byte[] bTempData = new byte [ nRecLength ];
                        Array.Copy( m_byteDataList , bTempData , nRecLength );
                        lock ( m_DelayReceiveDataArray )
                        {
                            m_DelayReceiveDataArray.Add( new CSendData( bTempData , ( short ) nRecLength , 0 ) );
                        }
                        //clear
                        Array.Clear( m_byteDataList , 0 , DataDefine.MAX_PACKET_SIZE );
                    }

                    if (IsConnect() && m_NetworkStream != null)
                    {
                        m_NetworkStream.BeginRead(m_byteDataList, 0, DataDefine.MAX_PACKET_SIZE,
                                                  new System.AsyncCallback(ReceiveMessage), m_NetworkStream);
                    }
                }
            }
            else
                return;
        }
        catch (System.Exception ex)
        {
         //   Debug.Log(ex.Message);
        }
    }

    //public void TimerEVent(object myobject,EventArgs myEventArgs)
    //{
    //	Debug.Log("Timer......");

    //}

    public void DisposeMessage()
    {
        //Debug.Log("logic receive message!");
        lock (m_DelayReceiveDataArray)
        {
            if (m_DelayReceiveDataArray.Count > 0)
            {
                while (m_DelayReceiveDataArray.Count > 0)
                {
                    CSendData data = (CSendData)m_DelayReceiveDataArray[0];
                    m_ReceiveDataArray.Add(data);
                    m_DelayReceiveDataArray.RemoveAt(0);
                }
            }
        }

        while (m_ReceiveDataArray.Count > 0)
        {
           // if (!CHandleMgr.GetSingle().GetLock())
            {
                CSendData data = (CSendData)m_ReceiveDataArray[0];
                CHandleMgr.GetSingle().ReceiveByte(data.m_bDataList, data.m_nLength);
                m_ReceiveDataArray.RemoveAt(0);
            }
        }

    }

    public void CheckConnect()
    {
        if (m_bConnectState && !IsConnect())
        {
          //  Debug.Log("Connect break;Start ReStart!");
          
            if (bReConnect)
            {
                if (ReConnect())
                {
                 //   Debug.Log("Connect break;ReConnect OK!");
                    m_LastReConnectTime = System.DateTime.Now;
                    //	ClientSendDataMgr.GetSingle().GetLoginSend().SendLoginServer(
                    //			landingManager.getsingleton().getAccount(), landingManager.getsingleton().getPassword(),
                    //	"iOS", "chs" );
                }
                else
                    m_bConnectState = false;
            }
        }
    }

    public void Update()
    {
        DisposeMessage();

        CheckConnect();
    }

    //behaviour script
    //	public CBehaviour GetBehaviour()
    //	{
    //		return m_Behaviour;
    //	}



    //private data
    private Thread m_ReceiveThread;
    private string m_ip;
    private int m_nPort;
    private System.Net.Sockets.TcpClient m_tcpClient;

    private System.Net.Sockets.UdpClient m_UDPClient;

    private System.Net.Sockets.NetworkStream m_NetworkStream;

    //private System.Timers.Timer			m_ReceiveTimer;
    private ArrayList m_ReceiveDataArray;
    private ArrayList m_DelayReceiveDataArray;

    private byte[] m_byteDataList;
    private static ClientNetMgr m_Singleton;

    private System.DateTime m_LastReConnectTime;
    private bool m_bConnectState;
	public bool m_bServerNet;
	public bool bReConnect;
    //	CBehaviour								m_Behaviour;

}

