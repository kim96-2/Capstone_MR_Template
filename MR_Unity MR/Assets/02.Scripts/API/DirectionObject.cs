using System;
using System.Collections.Generic;


namespace RestAPI.DirectionObject
{
    [Serializable]
    public class Request
    {
        // 출발 위치 경도
        public double startX;
        // 출발 위치 위도
        public double startY;
        
        // 도착지 위치 경도
        public double endX;
        // 도착지 위치 위도
        public double endY;

        // 출발지 명칭 : "출발지"
        public string startName = "%EC%B6%9C%EB%B0%9C%EC%A7%80%0A";
        // 도착지 명칭 : "도착지"
        public string endName = "%EB%8F%84%EC%B0%A9%EC%A7%80";
    }
    
    
    [Serializable]
    public class Response
    {
        public string type; // geojson 표준 프로퍼티

        public List<Feature> features; // 포인트 및 라인의 형상 정보
    }
    
    
    [Serializable]
    public class Feature
    {
        public string type; // 출발점, 안내점, 경유지, 도착점 정보

        public Geometry geometry; // 형상 정보

        public Properties properties; // 사용자 정의 프로퍼티 정보

            
    }
    
    
    [Serializable]
    public class Geometry
    {
        public string type; // 형상 정보 데이터의 종류 (Point 또는 LineString)

        public List<List<double>> coordinates; // 좌표 정보 (복수 좌표)
    }

    
    [Serializable]
    public class Properties
    {
        public string index; // 경로 순번
        public string pointIndex; // 안내점 노드의 순번
        public string name; // 안내지점의 명칭
        public string description; // 길 안내 정보
        public string direction; // 방면 명칭
        public string intersectionName; // 교차로 명칭
        public string nearPoiX; // 안내지점 근방 poi X좌표
        public string nearPoiY; // 안내지점 근방 poi Y좌표
        public string nearPoiName; // 안내지점 근방 poi
        public int turnType; // 회전 정보
        public string pointType; // 안내지점의 구분
        public string facilityType; // 구간의 시설물 정보
        public string facilityName; // 구간 시설물 타입의 명칭
        public int totalDistance; // 경로 총 길이(단위:m)
        public int totalTime; // 경로 총 소요시간(단위: 초)

        public int lineIndex; // 구간의 순번
        public int time; // 구간의 소요 시간(단위: 초)
        public int distance; // 구간 거리(단위: m)
        public int roadType; // 도로 타입 정보
        public int categoryRoadType; // 특화거리 정보
    }
}