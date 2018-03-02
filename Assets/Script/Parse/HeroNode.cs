using System.Collections.Generic;
using Tianyu;

public class HeroNode : FSDataNodeBase
{
    public long hero_id;        // Ӣ��ID
    public int types;           //Ӣ������ 1��Ӣ�ۣ�2:С�֣�3����ͨ���4����Ӣ��5��BOSS��6������

    public string name;         //Ӣ������
    public string describe;     //Ӣ������
    public int[] dingwei;       //��λ
    public string info;         //Ӣ����Ϣ
    public string icon_atlas;   //ͷ����Դ·��
    public string icon_name;    //ͷ������
    public int[] mount_types;   //���������
    // public string icon_name_y;

    public int model;                //ģ��
    public string original_painting;    //ԭ��
    public int[] skill_order;           //����˳��
    public int attribute;               //��ɫ���� 1:������2��������3������
    public int released;                //��ǰ�汾�Ƿ񿪷� 0 : No��1��Yes
    public int is_icon;
    public int sex;                     // �Ա� 1���У�2��Ů��
    public int init_star;               //��ʼ�Ǽ�
    public int[] characteristic;

    // 1-5�ǳɳ�ϵ����[����, ����, ����]
    public float[] rate1;
    public float[] rate2;
    public float[] rate3;
    public float[] rate4;
    public float[] rate5;

    public int soul_gem;             //���ʯid
    public long[] skill_id;      //����id
    public bool isHas;
    public int dlgAmount;

    public override void parseJson(object jd)
    {
        Dictionary<string, object> item = (Dictionary<string, object>)jd;
        hero_id = long.Parse(item["hero_id"].ToString());
        types = int.Parse(item["types"].ToString());
        name = item["name"].ToString();
        describe = item["describe"].ToString();
        dingwei = item["describe"] as int[];
        info = item["info"].ToString();
        icon_atlas = item["icon_atlas"].ToString();
        icon_name = item["icon_name"].ToString();
        // icon_name_y = item["icon_name_y"].ToString();
        model = int.Parse(item["model"].ToString());
        original_painting = item["original_painting"].ToString();

        skill_order = item["skill_order"] as int[];
        mount_types = item["mount_types"] as int[];

        attribute = int.Parse(item["attribute"].ToString());
        released = int.Parse(item["released"].ToString());
        is_icon = int.Parse(item["is_icon"].ToString());
        sex = int.Parse(item["sex"].ToString());
        init_star = int.Parse(item["init_star"].ToString());

        if (item.ContainsKey("characteristic"))
            characteristic = item["characteristic"] as int[];

        rate1 = FSDataNodeTable<HeroNode>.GetSingleton().ParseToFloatArray(item["rate1"]);
        rate2 = FSDataNodeTable<HeroNode>.GetSingleton().ParseToFloatArray(item["rate2"]);
        rate3 = FSDataNodeTable<HeroNode>.GetSingleton().ParseToFloatArray(item["rate3"]);
        rate4 = FSDataNodeTable<HeroNode>.GetSingleton().ParseToFloatArray(item["rate4"]);
        rate5 = FSDataNodeTable<HeroNode>.GetSingleton().ParseToFloatArray(item["rate5"]);

        soul_gem = int.Parse(item["soul_gem"].ToString());
        // skill_id = (List<long>)item["skill_id"];
        int[] nodeIntarr = (int[])item["skill_id"];
        // nodelist = (int[])item["skill_id"];
        skill_id = new long[nodeIntarr.Length];

        for (int m = 0; m < nodeIntarr.Length; m++)
        {
            skill_id[m] = nodeIntarr[m];
        }
        dlgAmount = int.Parse(item["dlgAmount"].ToString());


    }
    /// <summary>
    /// ��ȡ�Ǽ��ɳ�ϵ��
    /// </summary>
    /// <param name="attrIndex">�������������������������ݡ�</param>
    /// <param name="star">�Ǽ�</param>
    /// <returns></returns>
    public float GetStarGrowUpRate(int attrIndex, int star)
    {
        switch (star)
        {
            case 1:
                return (float)rate1[attrIndex];
            case 2:
                return (float)rate2[attrIndex];
            case 3:
                return (float)rate3[attrIndex];
            case 4:
                return (float)rate4[attrIndex];
            case 5:
                return (float)rate5[attrIndex];
            default:
                return 0f;
        }
    }
}

