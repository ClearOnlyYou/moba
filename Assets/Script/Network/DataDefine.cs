using System;
 
public sealed class DataDefine
{
	public DataDefine ()
	{
	}
	public const float	netTimeCancel = 600;
    public const string version =  "1.2.0209";
	public const bool isOutLine =false;
    public const bool isSkipFunction = false;
    public const string inserverip = "http://192.168.3.251/res/";
    public const string outserverip = "http://114.215.78.17/res/";
    //public const string ServerListOutLineUrl = "http://114.215.78.17:8090/ac/serverList.action?account={0}&&types={1}";http://192.168.3.251/api/servelist?account=�˻�&types=��¼����
    public const string ServerListOutLineUrl = "http://114.215.78.17/api/serverlist";//?account={0}&&types={1}"
	public const string ServerListUrl = "http://192.168.3.251/api/serverlist";//?account={0}&&types={1}"

    //public const string LoginOutLineUrl = "http://www.tianyuonline.cn/mp/ac/Login.php?mobile={0}&passwd={1}";http://192.168.3.251/api/login?account=�˻�&passwd=����&types=��¼����
    public const string LoginOutLineUrl = "http://114.215.78.17/api/login";//?account={0}&&passwd={1}";//"http://114.215.78.17/mp/ac/Login.php?mobile={0}&passwd={1}";
    public const string LoginOutLineUrl360 = "http://114.215.78.17/api/extendaccount";
    public const string LoginUrl = "http://192.168.3.251/api/login";//?account={0}&&passwd={1}&&types={2}"

	public const string RegistOutLineUrl = "http://114.215.78.17/api/reg?mobile={0}&passwd={1}&cv={2}&udid={3}+&mc={4}";//"http://114.215.78.17/mp/ac/Reg.php?mobile={0}&passwd={1}";
    public const string RegistLineUrl = "http://192.168.3.251/api/reg?mobile={0}&passwd={1}&cv={2}&udid={3}+&mc={4}";//


    public const int mainchannel = 1000;//��������  1000�Լ��ڲ����ݣ�1001 360������ 1002 ����������
	public const string ClientVersion = "1.2.0209";
    public const string datakey = "bloodgod20160516";
    public const byte isEFS = 0;//�Ƿ���ܴ���
	public const int MAX_PACKET_SIZE									= 1024*640;

	public const bool isConectSocket = true;  //�������汾

    public const bool isLogMsg = true;  // �Ƿ������Ϣ�շ�
    public const bool isLogMsgDetail = false;  // �Ƿ������Ϣ����
    public const bool isLogWalkMsg = false;  // �Ƿ����������Ϣ

    public static bool filterWalkMsg ( uint msgId )
    {
        return !( msgId == 8449 || msgId == 261 || msgId == 260 || msgId == 259) || isLogWalkMsg;
    }
}

