

// 장소 정보 형식
[System.Serializable]
public class Place
{
    public int id;      // 장소 id
    public string category_group_code;      // 카테고리 그룹 코드
    public string category_group_name;      // 카테고리 그룹명
    public string category_name;        // 카테고리 이름
    
    public string place_name;       // 장소명, 업체명
    public string place_url;        // 장소 상세페이지 URL
    public string phone;        // 전화번호


    public string address_name;     // 전체 지번 주소
    public string road_address_name;        // 전체 도로명주소
        
    public double x;        // X좌표값 (경도)
    public double y;        // Y좌표값 (위도)
    public int distance;         // 중심좌표까지의 거리
}
