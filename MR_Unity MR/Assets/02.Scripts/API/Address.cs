

// 주소 정보 형식
[System.Serializable]
public class Address
{
    public string address_name;     // 전체 지번 or 도로명 주소
    public string address_type;     // 타입
    
    public double x;        // X좌표값 (경도)
    public double y;        // Y좌표값 (위도)

    public AddressQuery address;    // 지번 주소 상세 정보
    public RoadAddressQuery road_address;       // 도로명 주소 상세 정보
}


// 지번 주소 정보
[System.Serializable]
public class AddressQuery
{
    public string address_name;     // 전체 지번 주소
    public string region_1depth_name;       // 지역 1 depth, 시도 단위
    public string region_2depth_name;       // 지역 2 depth, 구 단위
    public string region_3depth_name;       // 지역 3 depth, 동 단위
    public string region_3depth_h_name;     // 지역 3 depth, 행정동 명칭

    public string h_code;       // 행정 코드
    public string b_code;       // 법정 코드

    public string mountain_yn;      // 산 여부, Y or N
    public string main_address_no;      // 지번 주번지
    public string sub_address_no;       // 지번 부번지, 없으면 빈 문자열
    
    public double x;        // X좌표값 (경도)
    public double y;        // Y좌표값 (위도)
}


// 도로명 주소 정보
[System.Serializable]
public class RoadAddressQuery
{
    public string address_name;     // 전체 지번 주소
    public string region_1depth_name;       // 지역명 1
    public string region_2depth_name;       // 지역명 2
    public string region_3depth_name;       // 지역명 3

    public string road_name;       // 도로명
    
    public string underground_yn;       // 지하 여부, Y or N
    
    public string main_building_no;      // 건물 본번
    public string sub_building_no;       // 건물 부번, 없으면 빈 문자열

    public string building_name;        // 건물 이름
    public int zone_no;       // 우편번호 5자리
    
    public double x;        // X좌표값 (경도)
    public double y;        // Y좌표값 (위도)
}
