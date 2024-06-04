using System.Collections.Generic;

namespace RestAPI.TrafficObject
{
    public class Response
    {
        public ErrorMessage errormessage;
        public List<ArrivalData> realtimeArrivalList;
    }


    public class ArrivalData
    {
        public int rowNum;
        public int subwayId;
        public string subwayNm;
        public string updnLine;
        public string trainLineNm;
        public int statnFid;
        public int statnTid;
        public int statnId;
        public string statnNm;
        public int trainCo;
        public int trnsitCo;
        public string ordkey;
        public string subwayList;
        public string statnList;
        public string btrainSttus;
        public int barvlDt;
        public int btrainNo;
        public int bstatnId;
        public string bstatnNm;
        public string recptnDt;
        public string arvlMsg2;
        public string arvlMsg3;
        public int arvlCd;
    }


    public class ErrorMessage
    {
        public int status;
        public string code;
        public string message;
        public string link;
        public string developerMessage;
        public int total;
    }
}